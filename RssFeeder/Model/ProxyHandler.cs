using System;
using System.Net;
using RssFeeder.Model.ApplicationSettings;

namespace RssFeeder.Model;

internal static class ProxyHandler
{
    public static void SetProxyFromSettings(Settings currentSettings)
    {
        if (currentSettings.UseProxy)
        {
            var proxyUri = GetProxyUri(currentSettings.ProxyUrl, currentSettings.ProxyPort);
            var credentials = GetCredentials(currentSettings.ProxyUsername, currentSettings.ProxyPassword);
            var proxy = GetProxy(proxyUri, credentials);

            WebRequest.DefaultWebProxy = proxy;
        }
        else
        {
            WebRequest.DefaultWebProxy = null;
        }
    }

    private static WebProxy GetProxy(Uri proxyUri, NetworkCredential credentials)
    {
        var proxy = new WebProxy(proxyUri, true, null, credentials);
        return proxy;
    }

    private static NetworkCredential GetCredentials(string proxyUsername, string proxyPassword)
    {
        NetworkCredential credentials;
        if (proxyUsername.Length == 0 || proxyPassword.Length == 0)
        {
            credentials = null;
        }
        else
        {
            credentials = new NetworkCredential(proxyUsername, proxyPassword);
        }

        return credentials;
    }

    private static Uri GetProxyUri(string proxyUrl, uint proxyPort)
    {
        var uriBuilder = new UriBuilder(proxyUrl)
        {
            Port = (int) proxyPort
        };

        var proxyUri = uriBuilder.Uri;
        return proxyUri;
    }
}