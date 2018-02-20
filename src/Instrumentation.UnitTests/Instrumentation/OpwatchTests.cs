using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evoq.Instrumentation
{
    [TestClass]
    public class OpwatchTests
    {
        private string _caption = null;

        [TestMethod]
        public void OpWatch_works_with_action_of_timespan()
        {
            int sleepMs = 100;

            TimeSpan ts = TimeSpan.Zero;
            using (var op = new Opwatch((d, _) => ts = d, null))
            {
                Thread.Sleep(sleepMs);
            }

            const int tolerance = 21;

            Assert.IsTrue(ts.TotalMilliseconds > (sleepMs - tolerance) && ts.TotalMilliseconds < (sleepMs + tolerance));
        }

        [TestMethod]
        public void OpWatch_works_with_action_of_string()
        {
            // Loops because sometimes the sleep period is 1ms off, just by chance.

            int c = 0;

            while (true)
            {
                c++;

                using (var op = new Opwatch(WriteOutput))
                {
                    Thread.Sleep(100);
                }

                bool isPass = _caption.StartsWith("Operation duration: 0.1") && _caption.EndsWith("s");

                if (isPass)
                    break;

                if (c > 10)
                    Assert.Fail(_caption);                
            }
        }

        private void WriteOutput(string caption)
        {
            _caption = caption;
        }
    }
}
