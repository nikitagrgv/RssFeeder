namespace RssFeeder.Model.ApplicationSettings;

public struct RawSettings
{
    // Proxy settings
    public bool UsingProxy;
    public string ProxyURL;
    public uint ProxyPort;
    public string ProxyUsername;
    public string ProxyPassword;

    // Rss settings
    public string RssFeed;
    public uint UpdatePeriodInSeconds;

    public static RawSettings GetDefault()
    {
        return new RawSettings
        {
            UsingProxy = false,
            ProxyURL = "http://12.34.56.78",
            ProxyPort = 11949,
            ProxyUsername = "username",
            ProxyPassword = "password",
            RssFeed = "https://habr.com/rss/interesting/",
            UpdatePeriodInSeconds = 60
        };
    }
}