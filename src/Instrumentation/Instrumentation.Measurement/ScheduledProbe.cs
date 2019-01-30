namespace Evoq.Instrumentation.Measurement
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents something that can take measurements and produce a status.
    /// </summary>
    public abstract class ScheduledProbe<T>
    {
        /// <summary>
        /// Represents a measurement result.
        /// </summary>
        protected struct Measurement
        {
            /// <summary>
            /// Create a measurement.
            /// </summary>
            public Measurement(T status, DateTime measureAfter) : this()
            {
                Status = status;
                MeasureAfter = measureAfter;
            }

            /// <summary>
            /// The status.
            /// </summary>
            public T Status;

            /// <summary>
            /// The time after which to next measure.
            /// </summary>
            public DateTime MeasureAfter;
        }

        /// <summary>
        /// Creates a new probe.
        /// </summary>
        protected ScheduledProbe()
        {
            this.Name = this.GetType().Name;
        }

        /// <summary>
        /// Creates a new probe.
        /// </summary>
        /// <param name="name">The name of the probe.</param>
        public ScheduledProbe(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("String argument cannot be null or whitespace.", nameof(name));
            }

            this.Name = name;
        }

        //

        /// <summary>
        /// Gets the name of this probe.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets when the next measurement should be taken after.
        /// </summary>
        public DateTime MeasureAfter { get; private set; } = DateTime.Now;

        /// <summary>
        /// Gets the last known status of the probe.
        /// </summary>
        public T LastStatus { get; private set; }

        //

        /// <summary>
        /// Runs the measurement routine.
        /// </summary>
        /// <returns>A tuple of the status and whether it changed.</returns>
        internal async Task<(T, bool)> RunAsync()
        {
            if (DateTime.Now > this.MeasureAfter)
            {
                Measurement measurement = await this.MeasureAsync();
                bool isDifferent = this.IsDifferent(measurement);

                this.LastStatus = measurement.Status;
                this.MeasureAfter = measurement.MeasureAfter;

                return (measurement.Status, isDifferent);
            }
            else
            {
                return (this.LastStatus, false);
            }
        }

        private bool IsDifferent(Measurement measurement)
        {
            IEquatable<T> equatableStatus = measurement.Status as IEquatable<T>;

            if (equatableStatus != null)
            {
                return !equatableStatus.Equals(this.LastStatus);
            }
            else
            {
                IComparable<T> comparableStatus = measurement.Status as IComparable<T>;

                if (comparableStatus != null)
                {
                    return !(comparableStatus.CompareTo(this.LastStatus) == 0);
                }
                else
                {
                    return !measurement.Status.Equals(this.LastStatus);
                }
            }
        }

        /// <summary>
        /// When implemented in a derived class, this returns the current measurement.
        /// </summary>
        /// <returns>A status result.</returns>
        protected abstract Task<Measurement> MeasureAsync();
    }
}