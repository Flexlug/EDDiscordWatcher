using DSharpPlus;
using DSharpPlus.CommandsNext;
using EDDiscordWatcher.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using System.Globalization;

internal class Bot : IDisposable
{
    private CommandsNextExtension _commandsNext;
    private DiscordClient _discord;

    private IServiceProvider _services;

    private ILogger<Bot> _logger;
    private ILoggerFactory _logFactory;

    public Bot(Settings settings)
    {
        _logFactory = new LoggerFactory().AddSerilog();
        _logger = _logFactory.CreateLogger<Bot>();

        _discord = new DiscordClient(new()
        {
            Token = settings.Token,
            TokenType = TokenType.Bot,
            LoggerFactory = _logFactory,
            Intents = DiscordIntents.All
        });

        _discord.ClientErrored += Discord_ClientErrored;
        
        // For correct datetime recognizing
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        ConfigureBot();
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        Log.Logger.Debug("Configuring services");
        _services = new ServiceCollection()
            .AddLogging(conf => conf.AddSerilog(dispose: true))
            .BuildServiceProvider();
    }

    private void ConfigureBot()
    {
        if (!Directory.Exists("logs"))
            Directory.CreateDirectory("logs");
    }

    private Task Discord_ClientErrored(DiscordClient sender, DSharpPlus.EventArgs.ClientErrorEventArgs e)
    {
        throw new NotImplementedException();
    }
}