using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RssFeeder.Model.ApplicationSettings
{
    public class Settings : INotifyPropertyChanged
    {
        private string _proxyPassword;
        private uint _proxyPort;
        private string _proxyUrl;
        private string _proxyUsername;
        private string _rssFeedUrl;
        private uint _updatePeriodInSeconds;
        private bool _useProxy;

        public bool UseProxy
        {
            get => _useProxy;
            set
            {
                if (value == _useProxy) return;
                _useProxy = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public static Settings GetDefault()
        {
            return new Settings
            {
                UseProxy = false,
                ProxyUrl = "http://12.34.56.78",
                ProxyPort = 11949,
                ProxyUsername = "username",
                ProxyPassword = "password",
                RssFeedUrl = "https://habr.com/rss/interesting/",
                UpdatePeriodInSeconds = 60
            };
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}