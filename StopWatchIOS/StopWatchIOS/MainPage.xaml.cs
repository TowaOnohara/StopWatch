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

        }
    }
}
