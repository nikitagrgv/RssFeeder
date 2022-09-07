using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Model.ApplicationSettings
{
    public struct RawSettings
    {
        // Proxy settings
        public bool UsingProxy;
        public string ProxyURI;
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
                ProxyURI = "http://example.com",
                ProxyPort = 80,
                ProxyUsername = "username",
                ProxyPassword = "password",
                RssFeed = "https://habr.com/rss/interesting/",
                UpdatePeriodInSeconds = 60,
            };
        }
    }
}
