using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RssFeeder.ViewModel
{
    public class DateTimeToStringConverter : IValueConverter
    {
        private const string DateFormat = "dd.MM.yyyy hh:mm";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? ((DateTime) value).ToString(DateFormat) : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}