using Markdig;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Indigo.Client.Helpers
{
    public class MarkdownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Markdown.ToHtml((string)value, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;
        }
    }
}