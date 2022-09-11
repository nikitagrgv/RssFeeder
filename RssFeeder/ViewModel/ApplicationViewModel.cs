using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using RssFeeder.Model;
using RssFeeder.Model.ApplicationSettings;
using RssFeeder.Model.Rss;

namespace RssFeeder.ViewModel;

internal class ApplicationViewModel : INotifyPropertyChanged
{
    private readonly SettingsManager _settingsManager;

    private RelayCommand _applyCommand;
    private RelayCommand _defaultCommand;
    private RelayCommand _loadCommand;
    private RelayCommand _saveCommand;
    
    private Settings _settingsInGui;

    public ApplicationViewModel()
    {
        _settingsManager = new SettingsManager("settings.xml");
        _settingsInGui = _settingsManager.Settings;

        ProxyHandler.SetProxyFromSettings(_settingsManager.Settings);
        
        Feed = new Feed(_settingsManager);
        Feed.UpdateRssFeedItems();
    }

    public Feed Feed { get; }
    
    // The settings in the GUI can be changed without affecting the actual settings
    // The settings are applied only after clicking the "Apply" button
    // So we need this property
    public Settings SettingsInGui
    {
        get => _settingsInGui;
        set
        {
            _settingsInGui = value;
            OnPropertyChanged();
        }
    }
    
    public RelayCommand ApplyCommand
    {
        get { return _applyCommand ??= new RelayCommand(_ => { UpdateManagerSettingsFromGui(); }); }
    }

    public RelayCommand LoadCommand
    {
        get
        {
            return _loadCommand ??= new RelayCommand(_ =>
            {
                _settingsManager.LoadOrDefault();
                UpdateGuiSettingsFromManager();
            });
        }
    }

    public RelayCommand DefaultCommand
    {
        get
        {
            return _defaultCommand ??= new RelayCommand(_ =>
            {
                _settingsManager.SetDefaultSettings();
                UpdateGuiSettingsFromManager();
            });
        }
    }

    public RelayCommand SaveCommand
    {
        get
        {
            return _saveCommand ??= new RelayCommand(_ =>
            {
                try
                {
                    _settingsManager.Save();
                }
                catch
                {
                    MessageBox.Show(
                        "Unable to save settings",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void UpdateManagerSettingsFromGui()
    {
        _settingsManager.Settings = SettingsInGui;
    }

    private void UpdateGuiSettingsFromManager()
    {
        SettingsInGui = _settingsManager.Settings;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}