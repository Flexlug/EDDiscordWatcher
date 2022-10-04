using DSharpPlus;
using DSharpPlus.Entities;
using EDDiscordWatcher.Configurations;
using EDDiscordWatcher.Models;
using EDDiscordWatcher.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscordWatcher.Services
{
    internal class DrakeWatcherService : IDrakeWatcherService
    {
        private DiscordWebhook _webhook;
        private IEDDNMessagesService _eddnMessagesService;

        private string _drakeId;
        private string _drakeWebhookEmbedImage;

        public DrakeWatcherService(IEDDNMessagesService eddnMessagesService, Settings settings, DiscordClient client)
        {
            _webhook = client.GetWebhookWithTokenAsync(settings.DrakeWebhookId, settings.DrakeWebhookToken).Result;

            _drakeId = settings.DrakeId;
            _drakeWebhookEmbedImage = settings.DrakeWebhookEmbedImage;

            _eddnMessagesService = eddnMessagesService;
            _eddnMessagesService.OnMessage += _eddnMessagesService_OnMessage;
        }

        private void _eddnMessagesService_OnMessage(string message)
        {
            //if (message.Contains($"\"StationName\": {_drakeId}") && message.Contains("\"event\": \"CarrierJump\""))
            if (message.Contains("\"event\": \"CarrierJump\""))
            {
                var drakeJumpMessage = JsonConvert.DeserializeObject<DrakeJumpMessage>(message);

                var embedBuilder = new DiscordEmbedBuilder()
                    .WithImageUrl(_drakeWebhookEmbedImage)
                    .WithTitle($"⚠️ {_drakeId} AUTOMATIC LOG ⚠️")
                    .WithDescription($"{_drakeId} прибыл в систему {drakeJumpMessage.message.StarSystem} к объекту {drakeJumpMessage.message.Body}. Тип объекта: {drakeJumpMessage.message.BodyType}")
                    .WithTimestamp(DateTime.Now)
                    .WithFooter($"Message source: {drakeJumpMessage.header.uploaderID}")
                    .Build();

                _webhook.ExecuteAsync(new DiscordWebhookBuilder()
                    .AddEmbed(embedBuilder)).ConfigureAwait(false);
            }
        }
    }
}
