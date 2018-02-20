using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Evoq.Instrumentation
{
    /// <summary>
    /// Logs to a list of messages.
    /// </summary>
    public class InMemoryLogger : ILogger
    {
        public InMemoryLogger(string name, ConcurrentBag<string> messageStore = null)
        {
            this.Name = name ?? "Untitled";
            this.MessageStore = messageStore ?? new ConcurrentBag<string>();
        }

        //

        public string Name { get; }

        public ConcurrentBag<string> MessageStore { get; }

        //

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.MessageStore.Add($"{logLevel}: [{eventId.Id}]: {formatter?.Invoke(state, exception) ?? state.ToString()}");
        }
    }
}
