# Аггрегатор поисковых запросов
### Требования к использованию
1. Visual Studio 2022.
2. Microsoft SQL Server Management Studio.
3. Node package manager.
4. Учетные записи на:
 -- portal.azure.com 
 -- developer.google.com 
 -- yandex.ru/dev/xml

*На момент публикации для удобства проверки в приложения уже встроены собственные ключи, которые будут доступны некоторое время. В дальнейшем необходимо создать собственные учетные записи для подключения к API поисковых систем.*

____________

### Создание базы данных
1. Открываем Microsoft SQL Server Management Studio и подключаемся к ядру СУБД.
2. Нажимаем ПКМ на каталог "Базы данных".
3. Выбираем "Восстановить базу данных".
4. Указываем путь до .bak файла, который лежит в корневой директории репозитория.

____________

## Запуск клиентского приложения

Открываем Visual Studio => Обозреватель решений => Выбираем папку Frontend и нажимаем ПКМ => Открыть в терминале.
Вводим следующие команды:

*Данная команда установит все необходимые библиотеки для корректной работы приложения*
```
npm install 
```

### Запуск приложения в режиме разработки с возможностью "горячей перезагрузки"
```
npm run serve
```
____________

### Запуск сервера

Открываем Visual Studio => Нажимаем Запуск без отладки (CTRL + F5).


Приложение автоматически откроет web-страницу в браузере для работы. В противном случае используйте адрес, указанный в консоли сервера.

____________

###Конфигурация приложения
Все настройки приложения расположены в файле **launchSettings.json**

По умолчанию используется профиль **http**

```
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:21104",
      "sslPort": 0
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "",
      "applicationUrl": "http://localhost:5133",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "GOOGLE_API_KEY": "AIzaSyCSXH-f4T_d9RV98Nz8vkXF2vlT3jO2gik",
        "GOOGLE_API_SEARCH_URL": "https://www.googleapis.com/customsearch/v1",
        "GOOGLE_API_SEARCH_ENGINE_ID": "e6c1b0c6f5d5142fd",
        "YANDEX_API_USER": "cpt-rex",
        "YANDEX_API_KEY": "03.1566488076:4acf537ed3969f9b42d12e731b3e3162",
        "YANDEX_API_SEARCH_URL": "https://yandex.com/search/xml",
        "BING_API_KEY": "9ef55ba708c7490faf415b7584d8765e",
        "BING_SEARCH_V7_ENDPOINT": "https://api.bing.microsoft.com/v7.0/search"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```
Название ENV переменной        | Описание                                                                |
 :-----------------------------|:-----------------------------------------------------------------------:|
| GOOGLE_API_KEY               | Ключ Google Custom Search API                                           |
| GOOGLE_API_SEARCH_URL        | URL API для поискового запроса                                          |
| GOOGLE_API_SEARCH_ENGINE_ID  | Ваш ID созданного поиского движка в учетной записи developer.google.com |
| YANDEX_API_USER              | Имя учетной записи, которая была использована для создания ключей API   |
| YANDEX_API_KEY               | Сгенерированный ключ для использования API                              |
| YANDEX_API_SEARCH_URL        | URL API для поискового запроса                                          |
| BING_API_KEY                 | Ключ API для поиского запроса Bing                                      |
| BING_SEARCH_V7_ENDPOINT      | URL для отправки запроса API                                            |

### Настройки база данных расположены в **appsettings.json**

```
{
  "ConnectionStrings": {
    "DbConnection": "Server=DESKTOP-DC5BLL6\\SQLEXPRESS;Database=search_aggregator_db;User Id=root;Password=root;TrustServerCertificate=True;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
Измените DBConnection, подставив свои данные