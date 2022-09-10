using System;
using System.IO;
using System.Xml.Serialization;

namespace RssFeeder.Model.ApplicationSettings;

internal class SettingsSerializer
{
    private readonly string _filename;
    private readonly XmlSerializer _serializer = new(typeof(Settings));

    public SettingsSerializer(string filename)
    {
        _filename = filename;
    }
    
    /// <exception cref="CannotDeserializeSettingsException" />
    public Settings Deserialize()
    {
        try
        {
            using var fs = File.OpenRead(_filename);
            var settings = (Settings) _serializer.Deserialize(fs);
            return settings;
        }
        catch
        {
            throw new CannotDeserializeSettingsException();
        }
    }
    
    /// <exception cref="CannotSerializeSettingsException"></exception>
    public void Serialize(Settings settings)
    {
        try
        {
            using var fs = File.Open(_filename, FileMode.Create);
            _serializer.Serialize(fs, settings);
        }
        catch (Exception e)
        {
            throw new CannotSerializeSettingsException();
        }
    }
}