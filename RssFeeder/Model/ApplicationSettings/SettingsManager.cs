using System;

namespace RssFeeder.Model.ApplicationSettings
{
    public class SettingsManager
    {
        private readonly SettingsSerializer _serializer;
        private Settings _settings;

        public SettingsManager(string settingsFilename)
        {
            _serializer = new SettingsSerializer(settingsFilename);
            LoadOrDefault();
        }

        public Settings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnSettingsChanged();
            }
        }

        public event Action SettingsChanged;

        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke();
        }

        public void LoadOrDefault()
        {
            try
            {
                Settings = _serializer.Deserialize();
            }
            catch (CannotDeserializeSettingsException)
            {
                SetDefaultSettings();
            }
        }

        public void SetDefaultSettings()
        {
            Settings = Settings.GetDefault();
        }

        /// <exception cref="CannotSerializeSettingsException"></exception>
        public void Save()
        {
            _serializer.Serialize(_settings);
        }
    }
}