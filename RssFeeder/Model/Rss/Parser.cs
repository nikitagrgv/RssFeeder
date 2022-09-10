using System;
using System.Collections.Generic;
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
    private readonly SettingsManager _settingsManager;
    private CancellationTokenSource _tokenSource = new();

    public Parser(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public List<ISyndicationItem> Items { get; } = new();

    public event Action ParsingDone;
    public event Action ParsingNotDone;

    public void Run()
    {
        _settingsManager.SettingsChanged += () => _tokenSource.Cancel();
        var parseThread = new Task(Parse);
        parseThread.Start();
    }

    private async void Parse()
    {
        while (true)
        {
            var currentSettings = _settingsManager.Settings;
            
            try
            {
                Items.Clear();
                
                if (currentSettings.UsingProxy)
                {
                    var builder = new UriBuilder(currentSettings.ProxyUrl);
                    builder.Port = (int)currentSettings.ProxyPort;

                    var proxyUri = builder.Uri;
                    
                    NetworkCredential credentials;
                    if (currentSettings.ProxyUsername.Length == 0 || currentSettings.ProxyPassword.Length == 0)
                    {
                        credentials = null;
                    }
                    else
                    {
                        credentials = new NetworkCredential(currentSettings.ProxyUsername, currentSettings.ProxyPassword);
                    }

                    var proxy = new WebProxy(proxyUri, true, null, credentials);
                    WebRequest.DefaultWebProxy = proxy;
                }
                else
                {
                    WebRequest.DefaultWebProxy = null;
                }
                
                
                

                var rssReader = new RssFeedReader(XmlReader.Create(currentSettings.RssFeedUrl));

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
                await Task.Delay(TimeSpan.FromSeconds(currentSettings.UpdatePeriodInSeconds), _tokenSource.Token);
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