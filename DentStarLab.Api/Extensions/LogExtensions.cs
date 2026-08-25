using DentStarLab.Api.Logging;
using Serilog;
using Serilog.Events;

namespace DentStarLab.Api.Extensions;

public static class LogExtensions
{
    public static void InitLog(this WebApplicationBuilder builder)
    {
        var telegramBotToken =
            builder.Configuration["Telegram:BotToken"];

        var telegramChatId =
            builder.Configuration["Telegram:ChatId"];

        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override(
                "Microsoft",
                LogEventLevel.Warning)
            .MinimumLevel.Override(
                "Microsoft.EntityFrameworkCore",
                LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console();

        if (!string.IsNullOrWhiteSpace(telegramBotToken) &&
            !string.IsNullOrWhiteSpace(telegramChatId))
        {
            loggerConfig = loggerConfig.WriteTo.Sink(
                new TelegramLogSink(
                    telegramBotToken,
                    telegramChatId),
                restrictedToMinimumLevel: LogEventLevel.Error);
        }

        Log.Logger = loggerConfig.CreateLogger();

        builder.Host.UseSerilog();
    }
}