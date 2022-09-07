using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using RssFeeder.Model.ApplicationSettings;
using RssFeeder.Model.Rss;

namespace RssFeeder
{
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
            AddUriValidator(SettingsProxyURI);
            AddUriValidator(SettingsRssFeed);

            _settings = new Settings("settings.xml");
            _parser = new Parser(_settings);
            _parser.ParsingDone += () =>
            {
                var sb = new StringBuilder();
                foreach (var item in _parser.Items)
                {
                    sb.AppendLine(item.Title);
                    sb.AppendLine(item.Description);
                    sb.AppendLine("----" + item.Published + "----");
                    sb.AppendLine("------------------------------------------------------------");
                }
                
                InfoTextBlock.Dispatcher.BeginInvoke( () =>
                {
                    InfoTextBlock.Text = sb.ToString();
                });
            };

            _parser.ParsingNotDone += () =>
            {
                InfoTextBlock.Dispatcher.BeginInvoke(() =>
                {
                    InfoTextBlock.Text = "Error ";
                });
            };

            UpdateGUISettingsFromObject();

            // -----------------------------------------------------------
            // _timer = new DispatcherTimer();
            // _timer.Interval = TimeSpan.FromSeconds(_settings.UpdatePeriodInSeconds);
            // _timer.Tick += OnTimerOnTick;
            // _timer.Start();
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
            InfoTextBlock.Text += args?.PropertyName + "\n";
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
            SettingsProxyURI.Text = _settings.ProxyURI;
            SettingsProxyPort.Text = _settings.ProxyPort.ToString();
            SettingsProxyUser.Text = _settings.ProxyUsername;
            SettingsProxyPass.Password = _settings.ProxyPassword;
            SettingsRssFeed.Text = _settings.RssFeed;
            SettingsUpdateRate.Text = _settings.UpdatePeriodInSeconds.ToString();
        }

        private void UpdateObjectSettingsFromGUI()
        {
            Debug.Assert(SettingsUseProxy.IsChecked != null, "SettingsUseProxy.IsChecked != null");

            _settings.UsingProxy = (bool)SettingsUseProxy.IsChecked;
            _settings.ProxyURI = SettingsProxyURI.Text;
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
}