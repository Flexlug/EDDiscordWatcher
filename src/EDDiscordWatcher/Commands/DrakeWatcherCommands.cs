using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using EDDiscordWatcher.Services.Interfaces;
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
        private IDrakeWatcherService _service;

        public DrakeWatcherCommands(ILogger<DrakeWatcherCommands> loger, IDrakeWatcherService service)
        {
            _logger = loger;
            _service = service;

            _logger.LogInformation($"{nameof(DrakeWatcherCommands)} is loaded");
        }

        [Command("drake-watcher-commands-ping"), Hidden, RequireOwner]
        public async Task DrakeWatcherCommand(CommandContext ctx)
            => await ctx.RespondAsync($"Pong! {nameof(DrakeWatcherCommands)} is loaded");
    }
}
