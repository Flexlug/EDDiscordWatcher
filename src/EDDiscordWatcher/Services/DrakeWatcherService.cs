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
        private string _drakeName;
        private string _drakeWebhookEmbedImage;

        private DateTime _lastMessageDate;

        public DrakeWatcherService(IEDDNMessagesService eddnMessagesService, ILogger<DrakeWatcherService> logger, Settings settings, DiscordClient client)
        {
            _webhook = client.GetWebhookWithTokenAsync(settings.DrakeWebhookId, settings.DrakeWebhookToken).Result;

            _drakeId = settings.DrakeId;
            _drakeName = settings.DrakeName;
            _drakeWebhookEmbedImage = settings.DrakeWebhookEmbedImage;
            _logger = logger;

            _eddnMessagesService = eddnMessagesService;
            _eddnMessagesService.OnMessage += EddnMessagesService_OnMessage;

            // Set yesterday to guarantee that first message after bot execution will be processed
            _lastMessageDate = DateTime.Now - TimeSpan.FromDays(1);
            
            logger.LogInformation($"{nameof(DrakeWatcherService)} is loaded");
        }
        
        private void EddnMessagesService_OnMessage(string message)
        {
            if (message.Contains($"\"StationName\": \"{_drakeId}\"") && message.Contains("\"event\": \"CarrierJump\""))
            {
                _logger.LogInformation("Got CarrierJump event");

                if (DateTime.Now - _lastMessageDate < TimeSpan.FromMinutes(10))
                {
                    _logger.LogInformation("CarrierJump event is duplicate");
                    return;
                }
                
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
                    //.WithAuthor("COVAS KELVIN", iconUrl:@"https://cdn.discordapp.com/attachments/839633777491574785/1028419699282812938/unknown.png")
                    .WithDescription($"{_drakeName} прибыл в систему {drakeJumpMessage.message.StarSystem} к объекту {drakeJumpMessage.message.Body}. Тип объекта: {drakeJumpMessage.message.BodyType}")
                    .WithTimestamp(DateTime.Now)
                    .WithFooter($"Message source: {drakeJumpMessage.header.uploaderID}")
                    .Build();

                _webhook.ExecuteAsync(new DiscordWebhookBuilder()
                    .AddEmbed(embedBuilder)).ConfigureAwait(false);
                
                _lastMessageDate = DateTime.Now;
            }
        }
    }
}
