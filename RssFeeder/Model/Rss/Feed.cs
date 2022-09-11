using System;
using System.Collections.ObjectModel;
using RssFeeder.Model.ApplicationSettings;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;


namespace RssFeeder.Model.Rss;

internal class Feed
{
    public ObservableCollection<RssItem> RssItems { get; set; } = new AsyncObservableCollection<RssItem>();

    private readonly Timer _timer;
    private readonly SettingsManager _settingsManager;
    
    public Feed(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        settingsManager.SettingsChanged += OnSettingsChanged;
        
        _timer = new Timer(_settingsManager.Settings.UpdatePeriodInSeconds);
        _timer.TimerElapsed += UpdateRssFeedItems;
    }

    private void OnSettingsChanged()
    {
        var s = _settingsManager.Settings;
        _timer.PeriodInSeconds = s.UpdatePeriodInSeconds;
        ProxyHandler.SetProxyFromSettings(s);
        
        UpdateRssFeedItems();
    }

    public async void UpdateRssFeedItems()
    {
        try
        {
            RssItems.Clear();

            var rssReader = GetRssReaderFromUrl(_settingsManager.Settings.RssFeedUrl);

            while (await rssReader.Read())
                if (rssReader.ElementType == SyndicationElementType.Item)
                {
                    var syndicationItem = await rssReader.ReadItem();

                    RssItems.Add(new RssItem
                    {
                        Title = syndicationItem.Title,
                        DescriptionHtml = syndicationItem.Description,
                        Link = GetLinkFromRssItem(syndicationItem),
                        PubDate = syndicationItem.Published.LocalDateTime
                    });
                }
        }
        catch
        {
            ShowError();
        }
    }

    private static string GetLinkFromRssItem(ISyndicationItem syndicationItem)
    {
        using var linksEnumerator = syndicationItem.Links.GetEnumerator();
        linksEnumerator.MoveNext();
        var rssItemLink = linksEnumerator.Current?.Uri.ToString() ?? "";
        
        return rssItemLink;
    }

    private static RssFeedReader GetRssReaderFromUrl(string rssFeedUrl)
    {
        var rssReader = new RssFeedReader(XmlReader.Create(rssFeedUrl));
        return rssReader;
    }

    private void ShowError()
    {
        var errorItem = new RssItem
        {
            DescriptionHtml = "<h1>Error while loading rss feed</h1>",
            PubDate = DateTime.Now
        };
        RssItems.Clear();
        RssItems.Add(errorItem);
    }
}