using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using HTMLConverter;

namespace RssFeeder.ViewModel;

public class HtmlToXamlConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            string html = (string)value;
            var xaml = HTMLConverter.HtmlToXamlConverter.ConvertHtmlToXaml(html, true);
            return xaml;
        }
        catch
        {
            return "";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}