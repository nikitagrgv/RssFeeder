using System;

namespace RssFeeder.Model.Rss;

internal class RssItem
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Link { get; private set; }
    public DateTime PubDate { get; private set; }
}