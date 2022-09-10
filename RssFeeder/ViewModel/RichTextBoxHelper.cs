using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace RssFeeder.ViewModel;

// Source:
// https://stackoverflow.com/a/66459117/19031745

public class RichTextBoxHelper : DependencyObject
{
    public static readonly DependencyProperty DocumentXamlProperty = DependencyProperty.RegisterAttached
    (
        "DocumentXaml",
        typeof(string),
        typeof(RichTextBoxHelper),
        new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true,
            PropertyChangedCallback = (obj, e) =>
            {
                var richTextBox = (RichTextBox) obj;
                var xaml = GetDocumentXaml(richTextBox);
                Stream sm = new MemoryStream(Encoding.UTF8.GetBytes(xaml));
                try
                {
                    richTextBox.Document = (FlowDocument) XamlReader.Load(sm);
                }
                catch
                {
                    // ignore
                }
                finally
                {
                    sm.Close();
                }
            }
        }
    );

    public static string GetDocumentXaml(DependencyObject obj)
    {
        return (string) obj.GetValue(DocumentXamlProperty);
    }

    public static void SetDocumentXaml(DependencyObject obj,
        string value)
    {
        obj.SetValue(DocumentXamlProperty, value);
    }
}