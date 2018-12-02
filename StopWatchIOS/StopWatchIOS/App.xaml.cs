using StopWatchCore.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StopWatchIOS
{
    public partial class App : Application, IModelPool
    {
        public StopWatchModel StopWatch { get; private set; }

        public App()
        {
            InitializeComponent();
            StopWatch = new StopWatchModel();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
