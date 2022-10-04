using DSharpPlus;
using DSharpPlus.CommandsNext;
using EDDiscordWatcher.Commands;
using EDDiscordWatcher.Configurations;
using EDDiscordWatcher.Services;
using EDDiscordWatcher.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Core;
using System;
using System.Globalization;

internal class Bot : IDisposable
{
    private CommandsNextExtension _commandsNext;
    private DiscordClient _discord;
    private Settings _settings;

    private IServiceProvider _services;

    private ILogger<Bot> _logger;
    private ILoggerFactory _logFactory;

    private bool _isDisposed;
    private bool _isRunning;

    public Bot(Settings settings)
    {
        _settings = settings;
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
        _discord.Ready += Discord_Ready;
        
        // For correct datetime recognizing
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        ConfigureBot();
        ConfigureServices();
        ConfigureCommands();
    }

    private void ConfigureCommands()
    {
        Log.Logger.Debug("Registering commands");
        var commandsNextConfiguration = new CommandsNextConfiguration
        {
            StringPrefixes = new[] { _settings.Prefix },
            Services = _services
        };
        _commandsNext = _discord.UseCommandsNext(commandsNextConfiguration);
        _commandsNext.RegisterCommands<DrakeWatcherCommands>();

        _commandsNext.CommandErrored += CommandsNext_CommandErrored;
    }

    private void ConfigureServices()
    {
        Log.Logger.Debug("Configuring services");
        _services = new ServiceCollection()
            .AddLogging(conf => conf.AddSerilog(dispose: true))
            .AddSingleton(_settings)
            .AddSingleton<IEDDNMessagesService, EDDNMessagesService>()
            .AddSingleton<IDrakeWatcherService, DrakeWatcherService>()
            .BuildServiceProvider();
    }
    public async Task RunAsync()
    {
        if (_isRunning)
        {
            throw new MethodAccessException("The bot is already running");
        }

        await _discord.ConnectAsync();

        _isRunning = true;

        while (_isRunning)
        {
            await Task.Delay(200);
        }
    }

    private void ConfigureBot()
    {
        if (!Directory.Exists("logs"))
            Directory.CreateDirectory("logs");
    }

    private async Task Discord_ClientErrored(DiscordClient sender, DSharpPlus.EventArgs.ClientErrorEventArgs e)
    {
        _logger.LogError(e.Exception, "Discord_ClientErrored");
    }

    private async Task Discord_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
    {
        Log.Logger.Information("The bot is online");
    }

    private async Task CommandsNext_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        _logger.LogError(e.Exception, "Discord_CommandErrored");
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _discord.Dispose();

        _isDisposed = true;
    }
}