using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using RssFeeder.Model.ApplicationSettings;

namespace RssFeeder.Model.Rss;

internal class Parser
{
    private readonly Settings _settings;
    private CancellationTokenSource _tokenSource = new();

    public Parser(Settings settings)
    {
        _settings = settings;
    }

    public List<ISyndicationItem> Items { get; } = new();

    public event Action ParsingDone;
    public event Action ParsingNotDone;

    public void Run()
    {
        _settings.PropertyChanged += (sender, args) => _tokenSource.Cancel();
        var parseThread = new Thread(Parse);
        parseThread.Start();
    }

    private async void Parse()
    {
        while (true)
        {
            try
            {
                Items.Clear();

                if (_settings.UsingProxy)
                {
                    var builder = new UriBuilder(_settings.ProxyURL);
                    builder.Port = (int)_settings.ProxyPort;

                    var proxyURI = builder.Uri;
                    
                    NetworkCredential credentials;
                    if (_settings.ProxyUsername.Length == 0 || _settings.ProxyPassword.Length == 0)
                    {
                        credentials = null;
                    }
                    else
                    {
                        credentials = new NetworkCredential(_settings.ProxyUsername, _settings.ProxyPassword);
                    }

                    var proxy = new WebProxy(proxyURI, true, null, credentials);
                    System.Net.WebRequest.DefaultWebProxy = proxy;
                }
                else
                {
                    System.Net.WebRequest.DefaultWebProxy = null;
                }
                
                
                

                var rssReader = new RssFeedReader(XmlReader.Create(_settings.RssFeed));

                while (await rssReader.Read())
                    if (rssReader.ElementType == SyndicationElementType.Item)
                        Items.Add(await rssReader.ReadItem());

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