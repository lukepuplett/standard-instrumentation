using Microsoft.Extensions.Logging;
using System;

namespace Evoq.Instrumentation
{
    /// <summary>
    /// Implements <see cref="ILogger"/> but does nothing.
    /// </summary>
    public class EmptyLogger : ILogger
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name"></param>
        public EmptyLogger(string name)
        {
            this.Name = name ?? nameof(EmptyLogger);
        }

        //

        /// <summary>
        /// Gets the name of this logger.
        /// </summary>
        public string Name { get; }

        //

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
    }
}
