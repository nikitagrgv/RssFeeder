using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using RssFeeder.ViewModel;

namespace RssFeeder.Model.Rss;

internal class Feed
{
    public ObservableCollection<RssItem> RssItems { get; set; } = new AsyncObservableCollection<RssItem>();

    // TODO del
    private string test = @"<p>В наше время нейросетью уже мало кого удивишь, эти штуки умеют обрабатывать видео, вести диалог с человеком, выполнять поиск материалов в интернете, писать музыку, распознавать объекты на фото, помогают обрабатывать фото и многое другое. Сегодня я хочу рассказать о сетке рисующей картинки —&nbsp;<a href=""https://www.midjourney.com/home/"" rel=""noopener noreferrer nofollow"">Midjourney</a>.</p><p>Миджорни умеет распознавать текст и интерпретировать его в картинки. Для этого необходимо на английском языке описать сюжет, направить его на обработку сетке и дождаться результата. После полученный результат можно немного модернизировать, увеличить его качество и скачать.</p><p>Получаются вот такие картинки.</p> <a href=""https://habr.com/ru/post/687524/?utm_source=habrahabr&amp;utm_medium=rss&amp;utm_campaign=687524#habracut"">Читать далее</a>";
    
    public Feed()
    {
        RssItems.Add(new RssItem {Title = "fjldsj fjgf ogsj gjs gj gjsd gj sgjs gjsgj", Description = test, Link = "htf/11.3.php", PubDate = DateTime.Now});
    }
    
    public void Start()
    {
        var task = new Task(async () =>
        {
            for (int i = 0; i < 10; i++)
            {
                var rssItem = new RssItem {Title = "Title1", Description = test, Link = "https://metanit.com/sharp/wpf/11.3.php", PubDate = DateTime.Now};
                RssItems.Add(rssItem);
                await Task.Delay(300);
            }
        });
        task.Start();
    }

}