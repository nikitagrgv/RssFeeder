# Задание для стажировки в Infotecs
## RssFeeder - Rss фидер.

Графическое приложение для чтения RSS ленты с сайта.

В файле настроек хранится:
Лента, которую обрабатывает фидер. По умолчанию это (https://habr.com/rss/interesting/).
Частота обновления ленты приложением.  Настройки для прокси-сервера - адрес и учётные данные для подключения.

Основное окно должно содержать список элементов ленты (item): 
Каждый элемент ленты должен отображать название и дату публикации (поля item->title и item->pubDate ). Дата публикации статьи должна отображаться в удобном для чтения пользователю формате.
При нажатии на заголовок должен открываться браузер с переходом на выбранную статью (или в уже открытом браузере должна открываться ссылка).
Пользователь должен иметь возможность посмотреть описание статьи 
(поле item->description). Содержание описание выводится в виде обычного текста (как есть), без форматирования по тегам.

Данные должны периодически обновляться. Частота обновления должна быть взята из файла конфигурации.
При подключении к RSS ленте должны использоваться заданные настройки прокси-севера из файла конфигурации. 

Пункты со звездочкой являются дополнительными и не обязательны для выполнения.

- Файл настроек имеет формат XML.
- Имеется возможность изменить ленту в меню настроек.
- Имеется возможность изменить частоту обновления в меню настроек.
- Опции в меню настроек валидируются.
- Имеется возможность включить несколько лент. Пользователь должен иметь возможность включать и выключать ленты в окне приложения.
- Выводить описание в виде, форматированном по тегам.
- Иметь возможность переключения отображения описания в форматированном виде и без форматирования.
- Реализовать на asp.net mvc/blazor или asp.net spa с использование dotnet core > 2.x или dotnet 5

  <img width="60%" src="https://github.com/nikitagrgv/RssFeeder/blob/master/gitassets/image.png?raw=true"><br>
