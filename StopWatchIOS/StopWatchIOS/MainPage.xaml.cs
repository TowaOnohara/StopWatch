using StopWatchCore.Frameworks.Messengers;
using StopWatchCore.Models;
using StopWatchCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StopWatchIOS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var _viewModel = new StopWatchViewModel(App.Current as IModelPool);
            this.BindingContext = _viewModel;



            //// メッセージ受信処理：Laps画面の表示
            //_viewModel.Messenger.Register(typeof(StartViewMessage).Name, message =>
            //{
            //    var msg = message as StartViewMessage;
            //    var v = new LapView()
            //    {
            //        DataContext = new LapViewModel(App.Current as IModelPool),
            //        Owner = this,
            //        WindowStartupLocation = WindowStartupLocation.CenterOwner,
            //    };
            //    v.ShowDialog();
            //});

    //        // メッセージ受信処理：トーストの表示
    //        _viewModel.Messenger.Register(typeof(ShowToastMessage).Name, message =>
    //        {
    //            var msg = message as ShowToastMessage;
				//AlertCenter.Default.PostMessage ("Notification", msg.Text);
    //        });

        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var vm = this.BindingContext as StopWatchViewModel;
            vm.CommandToggleVisibleMillis.Execute();
        }

    
    }
}
