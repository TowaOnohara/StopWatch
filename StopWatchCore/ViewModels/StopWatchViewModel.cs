using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using StopWatchCore.Frameworks.Messengers;
using StopWatchCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace StopWatchCore.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class StopWatchViewModel : IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Messenger Messenger { get; } = new Messenger();

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        /// <summary>
        /// タイマー時間
        /// </summary>
        public ReadOnlyReactiveProperty<string> FormattedTime { get; }

        /// <summary>
        /// 実行中かどうか
        /// </summary>
        public ReadOnlyReactiveProperty<bool> IsRunning { get; }

        /// <summary>
        /// フォーマットされた経過時間群
        /// </summary>
        public ReadOnlyReactiveProperty<IEnumerable<string>> FormattedLaps { get; }

        /// <summary>
        /// ミリ秒を表示するか
        /// </summary>
        public ReadOnlyReactiveProperty<bool> IsVisibleMills { get; }

        /// <summary>
        /// 開始 or 停止
        /// </summary>
        public ReactiveCommand CommandStartOrStop { get; }

        /// <summary>
        /// 経過時間の記録
        /// </summary>
        public ReactiveCommand CommandLap { get; }

        /// <summary>
        /// ミリ秒以下の表示切替
        /// </summary>
        public ReactiveCommand CommandToggleVisibleMillis { get; } = new ReactiveCommand(); // いつでも実行可能

        [Obsolete("for designer", true)]
        public StopWatchViewModel() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="modelPool"></param>
        public StopWatchViewModel(IModelPool modelPool)
        {
            // モデルの取得
            var model = modelPool.StopWatch;

            // モデルプロパティを取得
            IsRunning = model.IsRunning;
            FormattedLaps = model.FormattedLaps;
            IsVisibleMills = model.IsVisibleMillis;

            // 表示用に20msec毎に間引き
            FormattedTime = model.FormattedTime
                .Throttle(TimeSpan.FromMilliseconds(20), Scheduler.Immediate)
                .ToReadOnlyReactiveProperty();

            // アンダースコア：破棄。使用しないということ。
            // https://ufcpp.net/study/csharp/datatype/declarationexpressions/#underscore
            // http://blog.xin9le.net/entry/2017/05/28/020129

            // STOPされたら、最速/最遅 ラップを表示して LapActivityへ遷移
            IsRunning.Where(x => !x).
                Subscribe(_ =>
                {
                    // Toastを表示させる
                    Messenger.Send(new ShowToastMessage(
                        $"最速ラップ:{model.FastestLaps}, 最遅ラップ:{model.WorstLaps}"));

                    // LapActivityへ遷移させる
                    Messenger.Send(new StartViewMessage(typeof(LapViewModel)));
                })
                .AddTo(_subscriptions);

            // 開始 or 停止
            CommandStartOrStop = new ReactiveCommand(); // いつでも実行可能
            CommandStartOrStop.Subscribe(_ =>
            {
                model.StartOrStop();
            })
            .AddTo(_subscriptions);

            // 経過時間の記録
            CommandLap = IsRunning.ToReactiveCommand(); // 実行中(IsRunning = true)のみ記録可能
            CommandLap.Subscribe(_ =>
            {
                model.Lap();
            })
            .AddTo(_subscriptions);

            // ミリ秒以下表示切替
            CommandToggleVisibleMillis = new ReactiveCommand(); // いつでも実行可能
            CommandToggleVisibleMillis.Subscribe(_ =>
            {
                model.ToggleVisibleMillis();
            })
            .AddTo(_subscriptions);

        }

        public void Dispose()
        {
            _subscriptions.Dispose();
            _subscriptions.Clear();
        }
    }
}
