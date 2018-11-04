using StopWatchCore.Frameworks.Messengers;
using StopWatchCore.Models;
using StopWatchCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StopWatchWPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var _viewModel = new StopWatchViewModel(App.Current as IModelPool);
            this.DataContext = _viewModel;


            // メッセージ受信処理：Laps画面の表示
            _viewModel.Messenger.Register(typeof(StartViewMessage).Name, message =>
            {
                var msg = message as StartViewMessage;
                var v = new LapView()
                {
                    DataContext = new LapViewModel(App.Current as IModelPool),
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                };
                v.ShowDialog();
            });


            // メッセージ受信処理：トーストの表示（WPFではメッセージボックスにした）
            _viewModel.Messenger.Register(typeof(ShowToastMessage).Name, message =>
            {
                var msg = message as ShowToastMessage;
                MessageBox.Show(msg.Text);
            });
        }
    }
}
