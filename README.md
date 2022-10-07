# EDDiscordWatcher
Discord bot which automaticly posts messages about specific Drake-Class carrier jumps.

## Requirements
- Registered Discord bot;
- Discord Webhook;
- Any machine with Docker, where you can run bot.

## Installation
**You can skip first 2 steps if you already have Discord bot and webhook.**
1. Register your Discord bot in [Discord Developer Portal](https://discord.com/developers/applications). In "Bot" category gain and copy secret token;
2. Create a webhook. Go to "Channel Settings -> Integrations -> Webhooks". Copy webhook URL. You'll need `webhook_id` and `webhook-token`.
```
https://discord.com/api/webhooks/[webhook_id]/[webhook-token]
```
3. Clone this repository. Go to repository folder.
```shell
git clone https://github.com/Flexlug/EDDiscordWatcher.git
cd EDDiscordWatcher
```
4. Open `docker-compose.yml` with any prefered text editor. In terminal you can open it in `nano`.
```shell
nano docker-compose.yml
```
5. You'll see `docker-cmpose.yml`:
```yaml
version: '3.8'

services:
    eddiscordwatcher:
        build:
            image: eddiscordwatcher
            context: .
        image: eddiscordwatcher
        restart: always

        environment:
            "DISCORD_TOKEN": "Single string. Discord token here"
            "DISCORD_PREFIX": "Single string. dr!"
            "DRAKE_ID": "Single string. Insert drake id here. Not name!"
            "DRAKE_NAME": "Single string. Insert drake name here, which will be displayed"
            "DRAKE_WEBHOOK_ID": "ulong (or UInt64). Insert webhook id, where drake events will be publeshed"
            "DRAKE_WEBHOOK_EMBED_IMAGE": "Single string. Insert image, which will be displayed, when drake jumped"
            "DRAKE_WEBHOOK_TOKEN": "Single string. Insert webhook token, where drake events will be publeshed"
```
You have to fill in environment variables. All variables are required.
Keep in mind, that all `String` variables have to be covered in double quotes!

Variable | Type   | Description                                                                                                                                           
--- |--------|-------------------------------------------------------------------------------------------------------------------------------------------------------|
DISCORD_TOKEN | String | Discord bot token. You've got it in step 1.                                                                                                           |
DISCORD_PREFIX | String | Prefix, which will be used to execute bot commands. This bot doesn't have any commands, so you can leave here default `"dr!"`.                        |
DRAKE_ID | String | Drake-Class Carrier ID. For example, if you see on galactic map `VOID WALKER T8G-20Q`, the id will be `T8G-20Q`. Drake ID is unique and won't change. |
DRAKE_NAME | String | Drake-Class Carrier name. You can insert here any name which you want to see in message.
DRAKE_WEBHOOK_ID | ulong  | Webhook ID. You can get it from webhook URL. __Notice that you don't have to cover this value in double quotes__.                                     |
DRAKE_WEBHOOK_TOKEN | String | Webhook token. You can get it from webhook URL.                                                                                                       |
DRAKE_WEBHOOK_EMBED_IMAGE | String | Image WEB URL. Specified image will be displayed in the bottom of the message.                                                                        |

6. Build a container.
```shell
docker compose build
```

7. Run a container.
```shell
docker compose up -d
```

## Demo
![](https://github.com/Flexlug/EDDiscordWatcher/raw/471267997bfe856f8dd44fa2bf35a6460da9ea50/demo.jpg)

## Требования
- Зарегистрированный Discord bot;
- Discord Webhook;
- Компьютер под управлением любой ОС с установленным Docker, где будет возможность развернуть бота.

## Установка
**Вы можете пропустить первые 2 шага, если у Вас уже есть зарегистрированный Discord бот и Webhook.**
1. Зарегистрируйте нового бота на сайте [Discord Developer Portal](https://discord.com/developers/applications). Во вкладке "Bot" получите секретный токен;
2. Создайте вебхук. "Настроить канал -> Интеграция -> Вебхуки". Скопируйте URL вебхука. Вам понадобится `webhook_id` и `webhook-token`.
```
https://discord.com/api/webhooks/[webhook_id]/[webhook-token]
```
3. Склонируйте репозиторий. Перейдите в папку репозитория.
```shell
git clone https://github.com/Flexlug/EDDiscordWatcher.git
cd EDDiscordWatcher
```
4. Откройте `docker-compose.yml` в любом удобном тектовом редакторе. В терминале вы можете воспользоваться `nano`.
```shell
nano docker-compose.yml
```

5. Вы увидите `docker-cmpose.yml`:
```yaml
version: '3.8'

services:
    eddiscordwatcher:
        build:
            image: eddiscordwatcher
            context: .
        image: eddiscordwatcher
        restart: always

        environment:
            "DISCORD_TOKEN": "Single string. Discord token here"
            "DISCORD_PREFIX": "Single string. dr!"
            "DRAKE_ID": "Single string. Insert drake id here. Not name!"
            "DRAKE_NAME": "Single string. Insert drake name here, which will be displayed"
            "DRAKE_WEBHOOK_ID": "Single string. Insert webhook id, where drake events will be publeshed"
            "DRAKE_WEBHOOK_TOKEN": "Single string. Insert webhook token, where drake events will be publeshed"
            "DRAKE_WEBHOOK_EMBED_IMAGE": "Single string. Insert image, which will be displayed, when drake jumped"
```
Вам необходимо заполнить все переменные среды. Необходимо заполнить все переменные. Имейте в виду, что все переменные типа `String` необходимо оборачивать в двойные кавычки.

Variable | Type   | Description                                                                                                                                                                          
--- |--------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
DISCORD_TOKEN | String | Токен бота Discord. Вы его можете получить, выполнив пункт 1.                                                                                                                        |
DISCORD_PREFIX | String | Префикс бота, через который к боту можно обращаться для вызова команд. Никаких команд у бота на данный момент нет, поэтому можете оставить значене - `"dr!"`.                        |
DRAKE_ID | String | Drake-Class Carrier ID. Например если флотоносец отображается на галактической карте как `VOID WALKER T8G-20Q`, то его ID будет `T8G-20Q`. Drake ID уникален и никогда не изменится. |
DRAKE_NAME | String | Название корабля. Можете вставить здесь любое название. Оно будет отображаться в сообщении о прыжке.
DRAKE_WEBHOOK_ID | ulong  | Webhook ID. Вы можете получить его из URL вебхука. __Обратие внимание, что данное значение не нужно оборачивать в двойные кавычки__.                                                 |
DRAKE_WEBHOOK_TOKEN | String | Токен Webhook. Вы можете получить его из URL вебхука.                                                                                                                                |
DRAKE_WEBHOOK_EMBED_IMAGE | String | WEB URL картинки. Указанная картинка будет отображаться в сообщении о прыжке снизу.                                                                                                  |

6. Соберите контейнер.
```shell
docker compose build
```

7. Запустите контейнер.
```shell
docker compose up -d
```

## Демонстрация
![](https://github.com/Flexlug/EDDiscordWatcher/raw/471267997bfe856f8dd44fa2bf35a6460da9ea50/demo.jpg)
