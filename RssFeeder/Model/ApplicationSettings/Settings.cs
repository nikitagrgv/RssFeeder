using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace RssFeeder.Model.ApplicationSettings
{
    internal class Settings : INotifyPropertyChanged
    {
        private readonly string _filename;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(RawSettings));
        private RawSettings _rawSettings;

        public Settings(string settingsFilename)
        {
            _filename = settingsFilename;

            try
            {
                DeserializeSettings();
            }
            catch
            {
                SetDefaultSettings();
                SerializeSettings();
            }
        }

        public void SetDefaultSettings()
        {
            _rawSettings = RawSettings.GetDefault();
        }

        public bool UsingProxy
        {
            get => _rawSettings.UsingProxy;
            set
            {
                _rawSettings.UsingProxy = value;
                OnPropertyChanged();
            }
        }

        public string ProxyURI
        {
            get => _rawSettings.ProxyURI;
            set
            {
                _rawSettings.ProxyURI = value;
                OnPropertyChanged();
            }
        }

        public uint ProxyPort
        {
            get => _rawSettings.ProxyPort;
            set
            {
                _rawSettings.ProxyPort = value;
                OnPropertyChanged();
            }
        }

        public string ProxyUsername
        {
            get => _rawSettings.ProxyUsername;
            set
            {
                _rawSettings.ProxyUsername = value;
                OnPropertyChanged();
            }
        }

        public string ProxyPassword
        {
            get => _rawSettings.ProxyPassword;
            set
            {
                _rawSettings.ProxyPassword = value;
                OnPropertyChanged();
            }
        }

        public string RssFeed
        {
            get => _rawSettings.RssFeed;
            set
            {
                _rawSettings.RssFeed = value;
                OnPropertyChanged();
            }
        }

        public uint UpdatePeriodInSeconds
        {
            get => _rawSettings.UpdatePeriodInSeconds;
            set
            {
                _rawSettings.UpdatePeriodInSeconds = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void DeserializeSettings()
        {
            using var fs = File.OpenRead(_filename);
            _rawSettings = (RawSettings) _serializer.Deserialize(fs);

        }

        public void SerializeSettings()
        {
            using var fs = File.Open(_filename, FileMode.Create);
            _serializer.Serialize(fs, _rawSettings);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}