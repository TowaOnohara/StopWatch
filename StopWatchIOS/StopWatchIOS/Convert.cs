using System;
using System.Globalization;
using Xamarin.Forms;

namespace StopWatchIOS.Converters
{
    public class RunningStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return value; }
            bool isRunning = (bool)value;
            return isRunning ? "Stop" : "Start";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}