using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RssFeeder.Model.ApplicationSettings;

public class Settings : INotifyPropertyChanged
{
    private string _rssFeedUrl;
    private uint _updatePeriodInSeconds;
    private bool _usingProxy;
    private string _proxyUrl;
    private uint _proxyPort;
    private string _proxyUsername;
    private string _proxyPassword;

    public bool UsingProxy
    {
        get => _usingProxy;
        set
        {
            if (value == _usingProxy) return;
            _usingProxy = value;
            OnPropertyChanged();
        }
    }

    public string ProxyUrl
    {
        get => _proxyUrl;
        set
        {
            if (value == _proxyUrl) return;
            _proxyUrl = value;
            OnPropertyChanged();
        }
    }

    public uint ProxyPort
    {
        get => _proxyPort;
        set
        {
            if (value == _proxyPort) return;
            _proxyPort = value;
            OnPropertyChanged();
        }
    }

    public string ProxyUsername
    {
        get => _proxyUsername;
        set
        {
            if (value == _proxyUsername) return;
            _proxyUsername = value;
            OnPropertyChanged();
        }
    }

    public string ProxyPassword
    {
        get => _proxyPassword;
        set
        {
            if (value == _proxyPassword) return;
            _proxyPassword = value;
            OnPropertyChanged();
        }
    }

    public string RssFeedUrl
    {
        get => _rssFeedUrl;
        set
        {
            if (value == _rssFeedUrl) return;
            _rssFeedUrl = value;
            OnPropertyChanged();
        }
    }

    public uint UpdatePeriodInSeconds
    {
        get => _updatePeriodInSeconds;
        set
        {
            if (value == _updatePeriodInSeconds) return;
            _updatePeriodInSeconds = value;
            OnPropertyChanged();
        }
    }

    public static Settings GetDefault()
    {
        return new Settings
        {
            UsingProxy = false,
            ProxyUrl = "http://12.34.56.78",
            ProxyPort = 11949,
            ProxyUsername = "username",
            ProxyPassword = "password",
            RssFeedUrl = "https://habr.com/rss/interesting/",
            UpdatePeriodInSeconds = 60
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}