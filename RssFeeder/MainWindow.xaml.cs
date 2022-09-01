using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using RssFeeder.Model.ApplicationSettings;

namespace RssFeeder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings _settings;

        public MainWindow()
        {
            InitializeComponent();
            AddUriValidator(SettingsProxyURI);
            AddUriValidator(SettingsRssFeed);

            _settings = new Settings("settings.xml");
            UpdateGUISettingsFromObject();


            //
            _settings.PropertyChanged += OnSettingsChanged;
            OnSettingsChanged(null, null);
            //
        }

        private void OnSettingsChanged(Object sender, PropertyChangedEventArgs args)
        {
            // var sb = new StringBuilder();
            // sb.AppendLine(_settings.UsingProxy.ToString());
            // sb.AppendLine(_settings.ProxyPort.ToString());
            // sb.AppendLine(_settings.ProxyURI);
            // sb.AppendLine(_settings.ProxyUsername);
            // sb.AppendLine(_settings.ProxyPassword);
            // sb.AppendLine(_settings.RssFeed);
            // sb.AppendLine(_settings.UpdatePeriodInSeconds.ToString());
            // InfoTextBlock.Text = sb.ToString();
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
                {
                    tb.Background = Brushes.White;
                }
                else
                {
                    tb.Background = Brushes.IndianRed;
                }
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

            _settings.UsingProxy = (bool) SettingsUseProxy.IsChecked;
            _settings.ProxyURI = SettingsProxyURI.Text;
            _settings.ProxyPort = uint.Parse(SettingsProxyPort.Text);
            _settings.ProxyUsername = SettingsProxyUser.Text;
            _settings.ProxyPassword = SettingsProxyPass.Password;
            _settings.RssFeed = SettingsRssFeed.Text;
            _settings.UpdatePeriodInSeconds = uint.Parse(SettingsUpdateRate.Text);

            _settings.SerializeSettings();
        }

    }
}