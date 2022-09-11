﻿using System;

namespace RssFeeder.Model.Rss
{
    internal class RssItem
    {
        public string Title { get; set; }
        public string DescriptionHtml { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
    }
}