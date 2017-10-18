using System;
using System.Globalization;
using Xamarin.Forms;

namespace Indigo.Client.Helpers
{
    public class FullLocalDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = (DateTime)value;
            return dateTime.ToLocalTime().ToString("F");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = DateTime.Parse((string)value);
            return dateTime.ToUniversalTime();
        }
    }
}