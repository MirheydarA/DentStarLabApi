using Serilog.Core;
using Serilog.Events;

namespace DentStarLab.Api.Logging;

public class TelegramLogSink : ILogEventSink
{
    private readonly string _botToken;
    private readonly string _chatId;
    private static readonly HttpClient HttpClient = new();

    public TelegramLogSink(string botToken, string chatId)
    {
        _botToken = botToken;
        _chatId = chatId;
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < LogEventLevel.Error)
            return;

        _ = SendAsync(logEvent);
    }

    private async Task SendAsync(LogEvent logEvent)
    {
        try
        {
            var message = $"🚨 *{logEvent.Level}*\n" +
                          $"{logEvent.RenderMessage()}\n" +
                          (logEvent.Exception != null
                              ? $"\n```\n{logEvent.Exception.Message}\n```"
                              : "");

            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

            var payload = new Dictionary<string, string>
            {
                ["chat_id"] = _chatId,
                ["text"] = message,
                ["parse_mode"] = "Markdown"
            };

            var response = await HttpClient.PostAsync(url, new FormUrlEncodedContent(payload));
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"TELEGRAM RESPONSE: {response.StatusCode} - {responseBody}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TELEGRAM ERROR: {ex.Message}");
        }
    }
}