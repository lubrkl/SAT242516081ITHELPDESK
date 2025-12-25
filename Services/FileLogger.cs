namespace ITHelpDesk.Services;

public interface IFileLogger
{
    Task LogAsync(string message);
}

public class FileLogger : IFileLogger
{
    private readonly string _logPath = "Logs";

    public async Task LogAsync(string message)
    {
        if (!Directory.Exists(_logPath))
            Directory.CreateDirectory(_logPath);

        var fileName = $"{_logPath}/log_{DateTime.Now:yyyyMMdd}.txt";
        var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";

        await File.AppendAllTextAsync(fileName, logMessage);
    }
}