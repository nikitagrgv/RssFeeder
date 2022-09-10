using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RssFeeder.Model.ApplicationSettings;

namespace RssFeeder.ViewModel;

public class ApplicationViewModel : INotifyPropertyChanged
{
    private readonly SettingsManager _settingsManager;
    private Settings _settingsInGui;

    public ApplicationViewModel(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _settingsManager.LoadOrDefault();
        _settingsInGui = _settingsManager.Settings;
    }

    public Settings SettingsInGui
    {
        get => _settingsInGui;
        set
        {
            _settingsInGui = value;
            OnPropertyChanged();
        }
    }
    
    

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}