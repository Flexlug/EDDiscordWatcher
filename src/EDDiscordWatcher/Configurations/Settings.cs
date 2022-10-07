using static System.Net.WebRequestMethods;

namespace EDDiscordWatcher.Configurations
{
    public class Settings
    {
        #region Discord credentials
        public string Token { get; set; }
        public string Prefix { get; set; }
        #endregion

        #region Drake Watcher
        public string DrakeId { get; set; }
        public string DrakeName { get; set; }
        public ulong DrakeWebhookId { get; set; }
        public string DrakeWebhookToken { get; set; }
        public string DrakeWebhookEmbedImage { get; set; }
        #endregion
    }
}