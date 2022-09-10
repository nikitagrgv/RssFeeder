using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using HTMLConverter;
using Microsoft.SyndicationFeed;
using RssFeeder.Model.ApplicationSettings;
using RssFeeder.Model.Rss;
using RssFeeder.ViewModel;


namespace RssFeeder;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Parser _parser;
    private readonly SettingsManager _settingsManager;
    private ApplicationViewModel _context;

    public MainWindow()
    {
        InitializeComponent();

        _settingsManager = new SettingsManager("settings.xml");
        _context = new ApplicationViewModel(_settingsManager);
        DataContext = _context;
        

        // AddUriValidator(SettingsProxyUrl);
        // AddUriValidator(SettingsRssFeedUrl);
        //
        // _parser = new Parser(_settingsManager);
        // _parser.ParsingDone += UpdateRssFeedItems;
        // _parser.ParsingNotDone += ShowRssFeedError;
        //
        // UpdateGuiSettingsFromObject();
        //
        // _parser.Run();
    }



    //
    // private void OpenLinkInBrowser(string url)
    // {
    //     var process = new ProcessStartInfo
    //     {
    //         FileName = url,
    //         UseShellExecute = true
    //     };
    //     Process.Start(process);
    // }
    //
    // private void ShowRssFeedError()
    // {
    //     // return;
    //     RssFeedStackPanel.Dispatcher.Invoke(ShowErrorOnGui);
    // }
    //
    // private void ShowErrorOnGui()
    // {
    //     RssFeedStackPanel.Children.Clear();
    //     RssFeedStackPanel.Children.Add(new TextBlock {Text = "Error", FontSize = 24});
    // }
    //
    // private void UpdateRssFeedItems()
    // {
    //     RssFeedStackPanel.Dispatcher.BeginInvoke(UpdateRssFeedPanel);
    // }
    //
    // private void UpdateRssFeedPanel()
    // {
    //     RssFeedStackPanel.Children.Clear();
    //     
    //     foreach (var item in _parser.Items)
    //     {
    //         var itemPanel = new StackPanel
    //         {
    //             Orientation = Orientation.Vertical,
    //             Margin = new Thickness(4),
    //             Background = Brushes.Ivory
    //         };
    //
    //
    //         var title = new TextBlock
    //         {
    //             FontWeight = FontWeights.Bold,
    //             TextWrapping = TextWrapping.Wrap,
    //             HorizontalAlignment = HorizontalAlignment.Center
    //         };
    //
    //         var hyperlink = new Hyperlink();
    //
    //
    //         using var enumerator = item.Links.GetEnumerator();
    //         enumerator.MoveNext();
    //         var uri = enumerator.Current?.Uri;
    //
    //
    //         hyperlink.NavigateUri = uri;
    //         hyperlink.Inlines.Add(new Run(item.Title));
    //         title.Inlines.Add(hyperlink);
    //         itemPanel.Children.Add(title);
    //
    //         hyperlink.Click += (sender, args) =>
    //             OpenLinkInBrowser(uri?.ToString());
    //
    //
    //         try
    //         {
    //             var description = GetRichTextBoxFromHtml(item.Description);
    //             itemPanel.Children.Add(description);
    //         }
    //         catch
    //         {
    //         }
    //
    //
    //         var pubTime = item.Published.LocalDateTime;
    //         var pubDate = new TextBlock
    //         {
    //             Text = pubTime.ToString(CultureInfo.CurrentCulture),
    //             TextWrapping = TextWrapping.Wrap,
    //             HorizontalAlignment = HorizontalAlignment.Right
    //         };
    //         itemPanel.Children.Add(pubDate);
    //
    //         var border = new Border
    //         {
    //             BorderBrush = Brushes.Gray,
    //             BorderThickness = new Thickness(2),
    //             Child = itemPanel,
    //             Padding = new Thickness(2)
    //         };
    //
    //         RssFeedStackPanel.Children.Add(border);
    //     }
    // }
    //
    // private static RichTextBox GetRichTextBoxFromHtml(string html)
    // {
    //     var xamlDescription = HtmlToXamlConverter.ConvertHtmlToXaml(html, true);
    //     var document = (FlowDocument) XamlReader.Parse(xamlDescription);
    //
    //     var description = new RichTextBox
    //     {
    //         IsReadOnly = true,
    //         Document = document
    //     };
    //     return description;
    // }
    //

    //
    // private void AddUriValidator(TextBox tb)
    // {
    //     tb.TextChanged += (o, e) =>
    //     {
    //         if (IsUriValid(tb.Text))
    //             tb.Background = Brushes.White;
    //         else
    //             tb.Background = Brushes.IndianRed;
    //     };
    // }
    //
    // private bool IsUriValid(string uriString)
    // {
    //     return Uri.TryCreate(uriString, UriKind.Absolute, out _);
    // }
    //
    // private void OnSettingsResetClicked(object sender, RoutedEventArgs e)
    // {
    //     _settingsManager.LoadOrDefault(); // TODO fix reset
    //     UpdateGuiSettingsFromObject();
    // }
    //
    // private void OnSettingsApplyClicked(object sender, RoutedEventArgs e)
    // {
    //     UpdateObjectSettingsFromGui();
    // }
    //
    // private void OnSettingsDefaultClicked(object sender, RoutedEventArgs e)
    // {
    //     _settingsManager.SetDefaultSettings();
    //     UpdateGuiSettingsFromObject();
    // }
    //
    // private void UpdateGuiSettingsFromObject()
    // {
    //     var currentSettings = _settingsManager.Settings;
    //     
    //     SettingsUseProxy.IsChecked = currentSettings.UsingProxy;
    //     SettingsProxyUrl.Text = currentSettings.ProxyUrl;
    //     SettingsProxyPort.Text = currentSettings.ProxyPort.ToString();
    //     SettingsProxyUser.Text = currentSettings.ProxyUsername;
    //     SettingsProxyPass.Password = currentSettings.ProxyPassword;
    //     SettingsRssFeedUrl.Text = currentSettings.RssFeedUrl;
    //     SettingsUpdateRate.Text = currentSettings.UpdatePeriodInSeconds.ToString();
    // }
    //
    // private void UpdateObjectSettingsFromGui()
    // {
    //     var newSettings = new Settings();
    //
    //     newSettings.UsingProxy = (bool) SettingsUseProxy.IsChecked;
    //     newSettings.ProxyUrl = SettingsProxyUrl.Text;
    //     newSettings.ProxyPort = uint.Parse(SettingsProxyPort.Text);
    //     newSettings.ProxyUsername = SettingsProxyUser.Text;
    //     newSettings.ProxyPassword = SettingsProxyPass.Password;
    //     newSettings.RssFeedUrl = SettingsRssFeedUrl.Text;
    //     newSettings.UpdatePeriodInSeconds = uint.Parse(SettingsUpdateRate.Text);
    //
    //     _settingsManager.Settings = newSettings;
    //     
    //     _settingsManager.Save(); // TODO move from this or rename method
    // }


    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !uint.TryParse(e.Text, out _);
    }


    private void OnSettingsApplyClicked(object sender, RoutedEventArgs e)
    {
        UpdateManagerSettingsFromGui();
    }

    private void OnSettingsLoadClicked(object sender, RoutedEventArgs e)
    {
        _settingsManager.LoadOrDefault();
        UpdateGuiSettingsFromManager();
    }

    private void OnSettingsDefaultClicked(object sender, RoutedEventArgs e)
    {
        _settingsManager.SetDefaultSettings();
        UpdateGuiSettingsFromManager();
    }

    private void OnSettingsSaveClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            _settingsManager.Save();
        }
        catch (Exception exception)
        {
            MessageBox.Show("Unable to save settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateGuiSettingsFromManager()
    {
        _context.SettingsInGui = _settingsManager.Settings;
    }

    private void UpdateManagerSettingsFromGui()
    {
        _settingsManager.Settings = _context.SettingsInGui;
    }
}