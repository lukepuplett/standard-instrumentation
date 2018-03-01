using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Evoq.Instrumentation
{
    /// <summary>
    /// Creates instances of the <see cref="EmptyLogger"/> class.
    /// </summary>
    public sealed class InMemoryLoggerFactory : ILoggerFactory
    {
        private ConcurrentBag<string> _messageStore = new ConcurrentBag<string>();

        /// <summary>
        /// Gets a view of the messages logged so far.
        /// </summary>
        public IReadOnlyCollection<string> Messages => _messageStore;

        /// <summary>
        /// Does nothing on this empty factory.
        /// </summary>
        /// <param name="provider">Not used.</param>
        public void AddProvider(ILoggerProvider provider) { }

        /// <summary>
        /// Creates an instance of an <see cref="EmptyLogger"/>.
        /// </summary>
        /// <param name="categoryName">The name to give to the logger.</param>
        /// <returns>An logger that does nothing.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new InMemoryLogger(categoryName, _messageStore);
        }

        /// <summary>
        /// Does nothing on this empty logger.
        /// </summary>
        public void Dispose() { }
    }
}
