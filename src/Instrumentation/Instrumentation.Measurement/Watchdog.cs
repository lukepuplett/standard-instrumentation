namespace Evoq.Instrumentation.Measurement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Continually runs probes looking for changes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Watchdog<T>
    {
        /// <summary>
        /// Creates an instance.
        /// </summary>
        public Watchdog() { }

        //

        /// <summary>
        /// The time to wait between running all probes.
        /// </summary>
        /// <remarks>
        /// This value should be kept relatively short. Probes will not take measurements until 
        /// after their scheduled time.
        /// </remarks>
        public int IntervalMs { get; protected set; } = 100;

        /// <summary>
        /// Gets the probes being watched.
        /// </summary>
        public IEnumerable<ScheduledProbe<T>> Probes => this.InnerProbes.ToArray();

        /// <summary>
        /// Gets or sets the collection of probes.
        /// </summary>
        protected List<ScheduledProbe<T>> InnerProbes { get; set; } = new List<ScheduledProbe<T>>();

        //

        /// <summary>
        /// Runs the watchdog until the cancellation token is cancelled.
        /// </summary>
        /// <param name="cancellationToken"></param>        
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(this.IntervalMs);

                foreach (ScheduledProbe<T> probe in this.Probes)
                {
                    (T _, bool changed) = await probe.RunAsync();

                    if (changed)
                    {
                        await this.OnChange(probe);
                    }
                }
            }
        }

        /// <summary>
        /// Runs when a probe changes status.
        /// </summary>
        /// <param name="probe"></param>
        /// <returns></returns>
        protected abstract Task OnChange(ScheduledProbe<T> probe);
    }
}
