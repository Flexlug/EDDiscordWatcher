using DSharpPlus;
using DSharpPlus.Entities;
using EDDiscordWatcher.Configurations;
using EDDiscordWatcher.Models;
using EDDiscordWatcher.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscordWatcher.Services
{
    internal class DrakeWatcherService : IDrakeWatcherService
    {
        private DiscordWebhook _webhook;
        private IEDDNMessagesService _eddnMessagesService;
        private ILogger<DrakeWatcherService> _logger;

        private string _drakeId;
        private string _drakeWebhookEmbedImage;

        public DrakeWatcherService(IEDDNMessagesService eddnMessagesService, ILogger<DrakeWatcherService> logger, Settings settings, DiscordClient client)
        {
            _webhook = client.GetWebhookWithTokenAsync(settings.DrakeWebhookId, settings.DrakeWebhookToken).Result;

            _drakeId = settings.DrakeId;
            _drakeWebhookEmbedImage = settings.DrakeWebhookEmbedImage;
            _logger = logger;

            _eddnMessagesService = eddnMessagesService;
            _eddnMessagesService.OnMessage += EddnMessagesService_OnMessage;

            logger.LogInformation($"{nameof(DrakeWatcherService)} is loaded");
        }

        private void EddnMessagesService_OnMessage(string message)
        {
            //if (message.Contains($"\"StationName\": {_drakeId}") && message.Contains("\"event\": \"CarrierJump\""))
            if (message.Contains("\"event\": \"CarrierJump\""))
            {
                _logger.LogInformation("Got CarrierJump event");

                DrakeJumpMessage drakeJumpMessage = null;
                try
                {
                    drakeJumpMessage = JsonConvert.DeserializeObject<DrakeJumpMessage>(message);

                    if (drakeJumpMessage.message is null)
                    {
                        throw new NullReferenceException("message field in DrakeJumpMessage is null");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("Couldn't parse drake message.", e);
                    return;
                }

                var embedBuilder = new DiscordEmbedBuilder()
                    .WithImageUrl(_drakeWebhookEmbedImage)
                    .WithTitle($"⚠️ {drakeJumpMessage.message.StationName} AUTOMATIC LOG ⚠️")
                    .WithDescription($"{drakeJumpMessage.message.StationName} прибыл в систему {drakeJumpMessage.message.StarSystem} к объекту {drakeJumpMessage.message.Body}. Тип объекта: {drakeJumpMessage.message.BodyType}")
                    .WithTimestamp(DateTime.Now)
                    .WithFooter($"Message source: {drakeJumpMessage.header.uploaderID}")
                    .Build();

                _webhook.ExecuteAsync(new DiscordWebhookBuilder()
                    .AddEmbed(embedBuilder)).ConfigureAwait(false);
            }
        }
    }
}
