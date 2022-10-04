using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscordWatcher.Commands
{
    internal class DrakeWatcherCommands : BaseCommandModule
    {
        private ILogger<DrakeWatcherCommands> _logger;

        public DrakeWatcherCommands(ILogger<DrakeWatcherCommands> loger)
        {
            _logger = loger;

            _logger.LogInformation($"{nameof(DrakeWatcherCommands)} is loaded");
        }

        [Command("drake-watcher-commands-ping"), Hidden, RequireOwner]
        public async Task DrakeWatcherCommand(CommandContext ctx)
            => await ctx.RespondAsync($"Pong! {nameof(DrakeWatcherCommands)} is loaded");
    }
}
