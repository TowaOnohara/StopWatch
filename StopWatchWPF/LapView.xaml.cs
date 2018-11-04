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
using System.Windows.Shapes;

namespace StopWatchWPF
{
    /// <summary>
    /// LapView.xaml の相互作用ロジック
    /// </summary>
    public partial class LapView : Window
    {
        public LapView()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += LapView_MouseLeftButtonDown;
        }

        private void LapView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState != MouseButtonState.Pressed) { return; }
            this.DragMove();
        }
    }
}
