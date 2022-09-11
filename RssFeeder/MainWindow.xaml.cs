using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using RssFeeder.Model.ApplicationSettings;
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
        try
        {
            var url = ((Hyperlink) sender).NavigateUri.ToString();
            
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
                $"Unable to go to the URL",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

    }
}