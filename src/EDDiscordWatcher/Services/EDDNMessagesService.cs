using EDDiscordWatcher.Configurations;
using EDDiscordWatcher.Services.Interfaces;
using Ionic.Zlib;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscordWatcher.Services
{
    internal class EDDNMessagesService : IEDDNMessagesService
    {
        public event IEDDNMessagesService.EventMessageHandler OnMessage;
        public bool IsEnabled { get; set; } = true;

        private Thread _thread;
        private ILogger<EDDNMessagesService> _logger;

        public EDDNMessagesService(ILogger<EDDNMessagesService> logger)
        {
            _logger = logger;

            _thread = new Thread(new ThreadStart(Start));
            _thread.Start();

            logger.LogInformation($"{nameof(EDDNMessagesService)} is loaded");
        }

        private void Start()
        {
            // Autorestart socket on failure
            while (IsEnabled)
            {
                try
                {
                    using (var client = new SubscriberSocket())
                    {
                        int count = 1;

                        client.Options.ReceiveHighWatermark = 1000;
                        client.Connect("tcp://eddn.edcd.io:9500");
                        client.SubscribeToAnyTopic();

                        while (IsEnabled)
                        {
                            byte[] bytes;
                            if (!client.TryReceiveFrameBytes(TimeSpan.FromMinutes(1), out bytes))
                            {
                                continue;
                            }

                            var uncompressed = ZlibStream.UncompressBuffer(bytes);
                            var result = Encoding.UTF8.GetString(uncompressed);

                            OnMessage?.Invoke(result);
                        }
                    }
                }
                catch(Exception e)
                {
                    _logger.LogError($"Error in {nameof(EDDNMessagesService)} parsing thread", e);                
                }
            }
        }

        ~EDDNMessagesService()
        {
            IsEnabled = false;
        }
    }
}
