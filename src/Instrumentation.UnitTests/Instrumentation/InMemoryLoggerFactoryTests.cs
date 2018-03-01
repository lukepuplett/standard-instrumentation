
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evoq.Instrumentation.Instrumentation
{
    [TestClass]
    public class InMemoryLoggerFactoryTests
    {
        [TestMethod]
        public void InMemoryLoggerFactory__ctor__when_called__then__returns()
        {
            InMemoryLoggerFactory loggerFactory = new InMemoryLoggerFactory();

            Assert.IsNotNull(loggerFactory);
        }

        [TestMethod]
        public void InMemoryLoggerFactory__CreateLogger__when_called_with_a_name__then__logger_has_name()
        {
            InMemoryLoggerFactory loggerFactory = new InMemoryLoggerFactory();

            var logger = loggerFactory.CreateLogger("Test") as InMemoryLogger;
            
            Assert.AreEqual("Test", logger.Name);            
        }

        [TestMethod]
        public void InMemoryLoggerFactory__CreateLogger__when_logger_used_once__then__factory_has_single_message()
        {
            InMemoryLoggerFactory loggerFactory = new InMemoryLoggerFactory();

            var logger = loggerFactory.CreateLogger("Test") as InMemoryLogger;

            logger.LogDebug("Hey");

            Assert.AreEqual(1, loggerFactory.Messages.Count);
        }

        [TestMethod]
        public void InMemoryLoggerFactory__CreateLogger__when_two_loggers_created_and_used_once_each__then__factory_has_both_messages()
        {
            InMemoryLoggerFactory loggerFactory = new InMemoryLoggerFactory();

            var logger = loggerFactory.CreateLogger("Test") as InMemoryLogger;
            var secondLogger = loggerFactory.CreateLogger("Second");

            logger.LogDebug("Hey");
            secondLogger.LogDebug("Hey again");

            Assert.AreEqual(2, loggerFactory.Messages.Count);
        }
    }
}
