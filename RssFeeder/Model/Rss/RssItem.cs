using System;

namespace RssFeeder.Model.Rss;

internal struct RssItem
{
    public string Title;
    public string Description;
    public string Link;
    public DateTime PublicationDate;
}