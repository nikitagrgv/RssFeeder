using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using HTMLConverter;
using RssFeeder.Model.ApplicationSettings;
using RssFeeder.Model.Rss;

namespace RssFeeder;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Parser _parser;
    private readonly Settings _settings;

    private readonly DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();
        AddUriValidator(SettingsProxyURL);
        AddUriValidator(SettingsRssFeed);

        _settings = new Settings("settings.xml");
        _parser = new Parser(_settings);
        _parser.ParsingDone += UpdateRssFeedItems;
        _parser.ParsingNotDone += ShowRssFeedError;

        UpdateGUISettingsFromObject();

        _parser.Run();
    }

    private void OpenLinkInBrowser(string url)
    {
        var psi = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    private void ShowRssFeedError()
    {
        // InfoTextBlock.Dispatcher.BeginInvoke(() => { InfoTextBlock.Text = "Error "; });
        RssFeedStackPanel.Dispatcher.BeginInvoke(() =>
        {
            RssFeedStackPanel.Children.Clear();
            RssFeedStackPanel.Children.Add(new TextBlock
            {
                Text = "Error",
                FontSize = 24
            });
        });
    }

    private void UpdateRssFeedItems()
    {
        RssFeedStackPanel.Dispatcher.BeginInvoke(() =>
        {
            RssFeedStackPanel.Children.Clear();
            foreach (var item in _parser.Items)
            {
                var itemPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(4),
                    Background = Brushes.Ivory
                };


                var title = new TextBlock
                {
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var hyperlink = new Hyperlink();


                using var enumerator = item.Links.GetEnumerator();
                enumerator.MoveNext();
                var uri = enumerator.Current?.Uri;


                hyperlink.NavigateUri = uri;
                hyperlink.Inlines.Add(new Run(item.Title));
                title.Inlines.Add(hyperlink);
                itemPanel.Children.Add(title);

                hyperlink.Click += (sender, args) =>
                    OpenLinkInBrowser(uri?.ToString());


                try
                {
                    var xamlDescription = HtmlToXamlConverter.ConvertHtmlToXaml(item.Description, true);
                    var document = (FlowDocument) XamlReader.Parse(xamlDescription);

                    var description = new RichTextBox
                    {
                        IsReadOnly = true,
                        Document = document
                    };
                    itemPanel.Children.Add(description);
                }
                catch
                {
                }


                var pubTime = item.Published.LocalDateTime;
                var pubDate = new TextBlock
                {
                    Text = pubTime.ToString(CultureInfo.CurrentCulture),
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                itemPanel.Children.Add(pubDate);

                var border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(2),
                    Child = itemPanel,
                    Padding = new Thickness(2)
                };

                RssFeedStackPanel.Children.Add(border);
            }
        });
    }

    // private async Task GetRss()
    // {
    //     _parser.Parse();
    //
    //     InfoTextBlock.Dispatcher.Invoke(() =>
    //     {
    //         var sb = new StringBuilder();
    //
    //         InfoTextBlock.Text = "";
    //
    //         foreach (var item in _parser.Items)
    //         {
    //             sb.AppendLine(item.Title);
    //             sb.AppendLine(item.Description);
    //             sb.AppendLine("----" + item.Published + "----");
    //             sb.AppendLine("------------------------------------------------------------");
    //         }
    //
    //         InfoTextBlock.Text = sb.ToString();
    //     });
    //
    //     await Task.Delay(51);
    // }


    private void OnSettingsChanged(object sender, PropertyChangedEventArgs args)
    {
        // InfoTextBlock.Text += args?.PropertyName + "\n";
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !uint.TryParse(e.Text, out _);
    }

    private void AddUriValidator(TextBox tb)
    {
        tb.TextChanged += (o, e) =>
        {
            if (IsUriValid(tb.Text))
                tb.Background = Brushes.White;
            else
                tb.Background = Brushes.IndianRed;
        };
    }

    private bool IsUriValid(string uriString)
    {
        return Uri.TryCreate(uriString, UriKind.Absolute, out _);
    }

    private void OnSettingsResetClicked(object sender, RoutedEventArgs e)
    {
        _settings.DeserializeSettings();
        UpdateGUISettingsFromObject();
    }

    private void OnSettingsApplyClicked(object sender, RoutedEventArgs e)
    {
        UpdateObjectSettingsFromGUI();
    }

    private void OnSettingsDefaultClicked(object sender, RoutedEventArgs e)
    {
        _settings.SetDefaultSettings();
        UpdateGUISettingsFromObject();
    }

    private void UpdateGUISettingsFromObject()
    {
        SettingsUseProxy.IsChecked = _settings.UsingProxy;
        SettingsProxyURL.Text = _settings.ProxyURL;
        SettingsProxyPort.Text = _settings.ProxyPort.ToString();
        SettingsProxyUser.Text = _settings.ProxyUsername;
        SettingsProxyPass.Password = _settings.ProxyPassword;
        SettingsRssFeed.Text = _settings.RssFeed;
        SettingsUpdateRate.Text = _settings.UpdatePeriodInSeconds.ToString();
    }

    private void UpdateObjectSettingsFromGUI()
    {
        Debug.Assert(SettingsUseProxy.IsChecked != null, "SettingsUseProxy.IsChecked != null");

        _settings.UsingProxy = (bool) SettingsUseProxy.IsChecked;
        _settings.ProxyURL = SettingsProxyURL.Text;
        _settings.ProxyPort = uint.Parse(SettingsProxyPort.Text);
        _settings.ProxyUsername = SettingsProxyUser.Text;
        _settings.ProxyPassword = SettingsProxyPass.Password;
        _settings.RssFeed = SettingsRssFeed.Text;
        _settings.UpdatePeriodInSeconds = uint.Parse(SettingsUpdateRate.Text);

        _settings.SerializeSettings();
    }

    private void ResetTimerButton_OnClick(object sender, RoutedEventArgs e)
    {
        _parser.Run();
    }
}