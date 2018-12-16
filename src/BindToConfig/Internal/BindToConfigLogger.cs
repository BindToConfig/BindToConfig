using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace BindToConfig.Internal
{
  internal class AddBoundToConfigLogger
  {
    internal static AddBoundToConfigLogger Instance = new AddBoundToConfigLogger();
    private readonly object _loggerSync=new object();
    private readonly ConcurrentQueue<LogEvent> _logsBuffer = new ConcurrentQueue<LogEvent>();

    internal ILogger<AddBoundToConfigLogger> Logger { get; private set; }

    internal void LogBindingError<T>(Exception ex) =>
      LogOrCache(new LogEvent(GetMessage<T>( "failed. The last one will be used"), ex));

    internal void LogBindSuccess<T>() =>
      LogOrCache(new LogEvent(GetMessage<T>("succeed")));

    private static string GetMessage<T>(string result) =>
      $"Detected configuration changes at '{DateTimeOffset.Now}'. Bound '{typeof(T).Name}' object to Configuration {result}.";

    internal bool TrySetLogger(ILogger<AddBoundToConfigLogger> logger)
    {
      lock (_loggerSync)
      {
        if (Logger != null|| logger is null)
        {
          return false;
        }
        Logger = logger;
      }
      FlushLogs();
      return true;
    }

    private void FlushLogs()
    {
      while (_logsBuffer.TryDequeue(out var result))
      {
          LogMsg(result);
      }
    }

    private void LogMsg(LogEvent result)
    {
      if (result.Exception != null)
      {
        Logger.LogWarning(result.Exception,result.Message);
      }
      else
      {
        Logger.LogInformation(result.Message);
      }
    }

    private void LogOrCache(LogEvent logEvent)
    {
      if (Logger == null)
      {
        lock (_loggerSync)
        {
          if (Logger == null)
          {
            _logsBuffer.Enqueue(logEvent);
            return;
          }
        }
      }
      LogMsg(logEvent);
    }

    private class LogEvent
    {
      internal LogEvent(string message, Exception exception = null)
      {
        Message = message;
        Exception = exception;
      }
      public string Message { get; }
      public Exception Exception { get; }
    }
  }
}
