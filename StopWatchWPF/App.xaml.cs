using StopWatchCore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StopWatchWPF
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application, IModelPool
    {
        public StopWatchModel StopWatch { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            StopWatch = new StopWatchModel();
        }
    }
     

}
