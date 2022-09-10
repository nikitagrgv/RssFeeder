using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Threading;
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

    public ApplicationViewModel(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _settingsInGui = _settingsManager.Settings;

        Feed = new Feed();
        
        Feed.Start();
    }

    public Feed Feed { get; set; }
    
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
        get { return _applyCommand ??= new RelayCommand(o => { UpdateManagerSettingsFromGui(); }); }
    }

    public RelayCommand LoadCommand
    {
        get
        {
            return _loadCommand ??= new RelayCommand(o =>
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
            return _defaultCommand ??= new RelayCommand(o =>
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
            return _saveCommand ??= new RelayCommand(o =>
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