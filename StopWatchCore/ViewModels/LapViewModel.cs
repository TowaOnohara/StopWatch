using Reactive.Bindings;
using StopWatchCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StopWatchCore.ViewModels
{
    public class LapViewModel : IDisposable 
    {
        /// <summary>
        /// 経過時間群
        /// </summary>
        public ReadOnlyReactiveProperty<IEnumerable<string>> FormattedLaps { get; }

        /// <summary>
        /// 時間の表示フォーマット
        /// </summary>
        public ReadOnlyReactiveProperty<string> TimeFormat { get; }

        public LapViewModel(IModelPool modelPool)
        {
            var model = modelPool.StopWatch;
            FormattedLaps = model.FormattedLaps;
        }

        public void Dispose()
        {
        }
    }
}
