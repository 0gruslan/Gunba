using System.Collections.Concurrent;
using System.Text;

namespace Lab1;


public static class ErrorLogger
{
    private static readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log");
    private static readonly SemaphoreSlim _logLock = new(1, 1);


    public static void LogError(string message, Exception? exception = null)
    {
        _logLock.Wait();
        try
        {
            var logEntry = new StringBuilder();
            logEntry.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
            
            if (exception != null)
            {
                logEntry.AppendLine($"Исключение: {exception.GetType().Name}");
                logEntry.AppendLine($"Сообщение: {exception.Message}");
                logEntry.AppendLine($"Стек вызовов: {exception.StackTrace}");
            }
            
            logEntry.AppendLine(new string('-', 80));

            File.AppendAllText(_logFilePath, logEntry.ToString(), Encoding.UTF8);
        }
        catch
        {
            
        }
        finally
        {
            _logLock.Release();
        }
    }
}


