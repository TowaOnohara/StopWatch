using Foundation;
using Plugin.Toast;
using StopWatchCore.Frameworks.Messengers;
using StopWatchCore.Models;
using StopWatchCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(StopWatchIOS.MessageIOS))]
namespace StopWatchIOS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var _viewModel = new StopWatchViewModel(App.Current as IModelPool);
            this.BindingContext = _viewModel;


            // メッセージ受信処理：Laps画面の表示
            _viewModel.Messenger.Register(typeof(StartViewMessage).Name, async message =>
            {
                var msg = message as StartViewMessage;
                var v = new LapView()
                {
                    BindingContext = new LapViewModel(App.Current as IModelPool),
                };
                await this.Navigation.PushAsync(v, true);
            });

            // メッセージ受信処理：トーストの表示
            _viewModel.Messenger.Register(typeof(ShowToastMessage).Name, message =>
            {
                var msg = message as ShowToastMessage;
                //CrossToastPopUp.Current.ShowToastMessage(msg.Text);

                //var toast = new MessageIOS();
                //toast.ShortAlert(msg.Text);
            });

        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var vm = this.BindingContext as StopWatchViewModel;
            vm.CommandToggleVisibleMillis.Execute();
        }

    }


    public interface IToastMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }

    public class MessageIOS : IToastMessage
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }
        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }

}
