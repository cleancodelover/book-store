using Microsoft.Extensions.Logging;

namespace BookStore.API.Helpers.Logger
{
    public static class LoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, LoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
