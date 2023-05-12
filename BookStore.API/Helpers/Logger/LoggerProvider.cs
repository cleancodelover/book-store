using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace BookStore.API.Helpers.Logger
{
    [ProviderAlias("ResureFile")]
    public class LoggerProvider : ILoggerProvider
    {
        public readonly LoggerOptions options;

        public LoggerProvider(IOptions<LoggerOptions> _options)
        {
            options = _options.Value;

            if (!Directory.Exists(options.FolderPath))
            {
                Directory.CreateDirectory(options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {

        }
    }
    public class FileLogger : ILogger
    {
        private readonly LoggerProvider _fileLogger;

        public FileLogger([NotNull] LoggerProvider fileLogger)
        {
            _fileLogger = fileLogger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (_fileLogger.options.FilePath == null)
            {

            }
            var fullFilePath = _fileLogger.options.FolderPath + "/" + _fileLogger.options.FilePath?.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var logRecord = string.Format("{0} [{1}] {2} {3}", "[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]", logLevel.ToString(), formatter(state, exception), exception != null ? exception.StackTrace : "");

            using (var streamWriter = new StreamWriter(fullFilePath, true))
            {
                streamWriter.WriteLine(logRecord);
            }
            _fileLogger.Dispose();

        }

    }
}
