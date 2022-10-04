using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using EDDiscordWatcher.Configurations;

var settings = new SettingsLoader().Load();

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}",
                                 theme: SystemConsoleTheme.Colored)
                .WriteTo.File($"logs/log-{DateTime.Now.Ticks}-", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();

using (var bot = new Bot(settings))
    await bot.RunAsync();