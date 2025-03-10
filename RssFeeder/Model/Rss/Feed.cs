﻿using System;
using System.Collections.ObjectModel;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using RssFeeder.Model.ApplicationSettings;

namespace RssFeeder.Model.Rss
{
    internal class Feed
    {
        private readonly SettingsManager _settingsManager;

        private readonly Timer _timer;

        public Feed(SettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            settingsManager.SettingsChanged += OnSettingsChanged;

            _timer = new Timer(_settingsManager.Settings.UpdatePeriodInSeconds);
            _timer.TimerElapsed += UpdateRssFeedItems;
        }

        public ObservableCollection<RssItem> RssItems { get; set; } = new AsyncObservableCollection<RssItem>();

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
                ShowErrorInItemsList();
            }
        }

        private static RssFeedReader GetRssReaderFromUrl(string rssFeedUrl)
        {
            var rssReader = new RssFeedReader(XmlReader.Create(rssFeedUrl));
            return rssReader;
        }

        private static string GetLinkFromRssItem(ISyndicationItem syndicationItem)
        {
            using var linksEnumerator = syndicationItem.Links.GetEnumerator();
            linksEnumerator.MoveNext();
            var rssItemLink = linksEnumerator.Current?.Uri.ToString() ?? "";

            return rssItemLink;
        }

        private void ShowErrorInItemsList()
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
}