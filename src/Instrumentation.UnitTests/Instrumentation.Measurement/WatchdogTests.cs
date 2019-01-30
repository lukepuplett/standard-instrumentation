using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evoq.Instrumentation.Measurement
{
    [TestClass]
    public class WatchdogTests
    {
        class LoggingWatchdog : Watchdog<string>
        {
            public LoggingWatchdog(ScheduledProbe<string> probe = null)
            {
                this.Changes = new List<string>(1000);

                if (probe != null)
                    this.InnerProbes.Add(probe);
            }

            public List<string> Changes { get; }

            protected override Task OnChange(ScheduledProbe<string> probe)
            {
                this.Changes.Add($"Probe '{probe.Name}' changed to '{probe.LastStatus}'.");

                return Task.CompletedTask;
            }
        }

        class RandomProbe : ScheduledProbe<string>
        {
            private readonly Random _random = new Random();

            protected override Task<Measurement> MeasureAsync()
            {
                byte[] buffer = new byte[16];

                _random.NextBytes(buffer);

                string s = Convert.ToBase64String(buffer);

                var m = new Measurement(s, DateTime.Now + TimeSpan.FromMilliseconds(200));

                return Task.FromResult(m);
            }
        }

        [TestMethod]
        public void Watchdog__RunAsync__when__no_probes__then__changes_empty()
        {
            var watchdog = new LoggingWatchdog();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(1000);

            var task = Task.Run(() => watchdog.RunAsync(cancellationTokenSource.Token), cancellationTokenSource.Token);

            task.Wait(2000);

            Assert.IsFalse(watchdog.Changes.Any());
        }

        [TestMethod]
        public void Watchdog__RunAsync__when__no_probes__then__changes_captured()
        {
            var watchdog = new LoggingWatchdog(new RandomProbe());

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(1000);
            
            var task = Task.Run(() => watchdog.RunAsync(cancellationTokenSource.Token), cancellationTokenSource.Token);

            task.Wait(2000);

            Assert.IsTrue(watchdog.Changes.Any());
        }
    }
}
