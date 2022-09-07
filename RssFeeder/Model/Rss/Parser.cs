using System;
using RssFeeder.Model.ApplicationSettings;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace RssFeeder.Model.Rss
{
    class Parser
    {
        private Settings _settings;
        public bool IsDone { get; private set; }
        public List<ISyndicationItem> Items { get; } = new List<ISyndicationItem>();

        public event Action ParsingDone;
        public event Action ParsingNotDone;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public Parser(Settings settings)
        {
            _settings = settings;
        }

        public void Run()
        {
            _settings.PropertyChanged += (sender, args) => _tokenSource.Cancel();
            Thread parseThread = new Thread(Parse);
            parseThread.Start();
        }

        public async void Parse()
        {
            while (true)
            {
                try
                {
                    Items.Clear();
                    var rssReader = new RssFeedReader(XmlReader.Create(_settings.RssFeed));

                    while (await rssReader.Read())
                    {
                        if (rssReader.ElementType == SyndicationElementType.Item)
                        {
                            Items.Add(await rssReader.ReadItem());
                        }
                    }

                    OnParsingDone();
                }
                catch
                {
                    OnParsingNotDone();
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_settings.UpdatePeriodInSeconds), _tokenSource.Token);
                }
                catch
                {
                    _tokenSource.Dispose();
                    _tokenSource = new CancellationTokenSource();
                    // ignored
                }
            }
        }

        protected virtual void OnParsingDone()
        {
            ParsingDone?.Invoke();
        }

        protected virtual void OnParsingNotDone()
        {
            ParsingNotDone?.Invoke();
        }
    }
}
