using Microsoft.Extensions.Logging;

namespace Evoq.Instrumentation
{
    /// <summary>
    /// Creates instances of the <see cref="EmptyLogger"/> class.
    /// </summary>
    public sealed class EmptyLoggerFactory : ILoggerFactory
    {
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
            return new EmptyLogger(categoryName);
        }

        /// <summary>
        /// Does nothing on this empty logger.
        /// </summary>
        public void Dispose() { }
    }
}
