using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscordWatcher.Configurations
{    
    /// <summary>
    /// Singleton class that loads settings from a file on the first usage.
    /// Settings are stored in the Cfg-Property.
    /// </summary>
    internal class SettingsLoader
    {
        /// <summary>
        /// Deserializes a file to a Settings object
        /// </summary>
        /// <param name="configFile">File to deserialize</param>
        /// <returns>deserialized Settings object</returns>
        public Settings Load()
        {
            return new Settings()
            {
                Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                Prefix = Environment.GetEnvironmentVariable("DISCORD_PREFIX"),
                DrakeId = Environment.GetEnvironmentVariable("DRAKE_ID"),
                DrakeName = Environment.GetEnvironmentVariable("DRAKE_NAME"),
                DrakeWebhookId = Convert.ToUInt64(Environment.GetEnvironmentVariable("DRAKE_WEBHOOK_ID")),
                DrakeWebhookToken = Environment.GetEnvironmentVariable("DRAKE_WEBHOOK_TOKEN"),
                DrakeWebhookEmbedImage = Environment.GetEnvironmentVariable("DRAKE_WEBHOOK_EMBED_IMAGE")
            };
        }
    }
}
