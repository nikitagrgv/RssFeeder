using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Model.ApplicationSettings
{
    public class RawSettings
    {
        // Proxy settings
        public bool UsingProxy { get; set; } = false;
        public string ProxyURI { get; set; } = "http://example.com";
        public uint ProxyPort { get; set; } = 80;
        public string ProxyUsername { get; set; } = "username";
        public string ProxyPassword { get; set; } = "password";

        // Rss settings
        public string RssFeed { get; set; } = "https://habr.com/rss/interesting/";
        public uint UpdatePeriodInSeconds { get; set; } = 60;
    }
}
