namespace RssFeeder.Model.ApplicationSettings;

public class Settings
{
    // Proxy settings
    public bool UsingProxy { get; set; }
    public string ProxyUrl { get; set; }
    public uint ProxyPort { get; set; }
    public string ProxyUsername { get; set; }
    public string ProxyPassword { get; set; }

    // Rss settings
    public string RssFeedUrl { get; set; }
    public uint UpdatePeriodInSeconds { get; set; }

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
}