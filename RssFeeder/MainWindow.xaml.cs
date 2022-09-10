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
    public MainWindow()
    {
        InitializeComponent();

        var settingsManager = new SettingsManager("settings.xml");
        
        DataContext = new ApplicationViewModel(settingsManager);
    }
    
    private void UrlClick(object sender, RoutedEventArgs e)
    {
        string url = ((Hyperlink) sender).NavigateUri.ToString();

        try
        {
            var process = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(process);
        }
        catch
        {
            MessageBox.Show(
                $"Unable to go to the URL: {url}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

    }
}