using System;
using System.Globalization;
using Xamarin.Forms;

namespace Indigo.Client.Helpers
{
	public class PartnerListViewHeightCalculator : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return 117 * ((int)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value) / 117;
		}
	}
}