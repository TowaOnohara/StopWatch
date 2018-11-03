using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace StopWatchCore.Models
{
    /// <summary>
    /// ストップウォッチ機能を実装したロジッククラス
    /// </summary>
    public class StopWatchModel
    {
        #region private property

        private readonly ReactiveProperty<long> _time = new ReactiveProperty<long>(0L);             // タイマー時間
        private readonly ReactiveProperty<bool> _isRunning = new ReactiveProperty<bool>(false);     // 実行中か？
        private readonly ReactiveProperty<IList<long>> _laps = new ReactiveProperty<IList<long>>(new List<long>()); // 経過時間群
        private readonly ReactiveProperty<bool> _isVisibleMillis = new ReactiveProperty<bool>(true);
        private readonly ReactiveProperty<string> _timeFormat;

        #endregion

        #region public property

        public ReadOnlyReactiveProperty<string> FormattedTime { get; }  // フォーマットされたタイマー時間
        public ReadOnlyReactiveProperty<bool> IsRunning { get; }    // 実行中か？
        public ReadOnlyReactiveProperty<IEnumerable<string>> FormattedLaps { get; } // フォーマットされた経過時間群
        public ReadOnlyReactiveProperty<bool> IsVisibleMillis { get; }  // ミリ秒表示するか？

        #endregion

        /// <summary>
        /// タイマーの購読状況
        /// </summary>
        private IDisposable _timerSubscription = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StopWatchModel()
        {
            Debug.WriteLine("StopWatchModel ctor");

            IsRunning = _isRunning.ToReadOnlyReactiveProperty();
            IsVisibleMillis = _isVisibleMillis.ToReadOnlyReactiveProperty();

            _timeFormat = IsVisibleMillis
                .Select(v => v ? @"mm\:ss\.fff" : @"mm\:ss")
                .ToReactiveProperty();

            // CombineLatest: 
            // _time と _timeFormat イベントを合成する。
            // どちらかのイベントが発生したら（値が変更されたら）FormattedTimeの値が変わる。
            FormattedTime = _time.CombineLatest(_timeFormat, (time, format) =>
                    TimeSpan.FromMilliseconds(time).ToString(format))
                .ToReadOnlyReactiveProperty();

            FormattedLaps = _laps.CombineLatest(_timeFormat,
                (laps, f) => laps.Select((x, i) => TimeSpan.FromMilliseconds(x).ToString(f)))
                .ToReadOnlyReactiveProperty();
        }

        public void StartOrStop()
        {
            if (!IsRunning.Value)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        private void Start()
        {
            if (IsRunning.Value)
            {
                Stop();
            }

            // 開始時にLapsをクリア。実行中フラグをONする。
            _laps.Value = new List<long>();
            _isRunning.Value = true;
            var now = DateTime.Now;

            // 10msec 周期で値を生成。_timeに経過時間をセットしている。
            _timerSubscription =
                Observable.Interval(TimeSpan.FromMilliseconds(10), Scheduler.Default)
                .Subscribe(TimeSpan =>
                {
                    _time.Value = Convert.ToInt64((DateTime.Now - now).TotalMilliseconds);
                }); // タイマー値を通知
        }


        private void Stop()
        {
            if (_timerSubscription != null)
            {
                _timerSubscription.Dispose();
                _timerSubscription = null;
            }

            _isRunning.Value = false;
        }

        /// <summary>
        /// 経過時間を記録する（経過時間をlaspプロパティに追加する。）
        /// </summary>
        public void Lap()
        {
            var laps = _laps.Value;
            var newLaps = new List<long>();

            newLaps.AddRange(laps);

            var totalLap = 0L;
            foreach (var lap in laps)
            {
                totalLap += lap;
            }

            newLaps.Add(_time.Value - totalLap);

            _laps.Value = newLaps;
        }

        public string FastestLaps
        {
            get
            {
                var laps = _laps.Value;
                if (laps.Count == 0)
                {
                    return TimeSpan.FromMilliseconds(_time.Value).ToString(_timeFormat.Value);
                }
                else
                {
                    return TimeSpan.FromMilliseconds(laps.Min()).ToString(_timeFormat.Value);
                }
            }
        }

        /// <summary>
        /// 最遅ラップを取得
        /// </summary>
        public string WorstLaps
        {
            get
            {
                var laps = _laps.Value;
                if (laps.Count == 0)
                {
                    return TimeSpan.FromMilliseconds(_time.Value).ToString(_timeFormat.Value);
                }
                else
                {
                    return TimeSpan.FromMilliseconds(laps.Max()).ToString(_timeFormat.Value);
                }
            }
        }

        /// <summary>
        /// 小数点以下の表示有無を切り替える。
        /// </summary>
        public void ToggleVisibleMillis()
        {
            _isVisibleMillis.Value = !_isVisibleMillis.Value;
        }


        #region IDisposable member

        public void Dispose()
        {
            Stop();
        }

        #endregion

    }
}
