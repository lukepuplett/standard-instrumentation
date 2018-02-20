using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evoq.Instrumentation
{
    public sealed class Opwatch : IDisposable
    {
        private readonly Action<TimeSpan, object> _timeSpanAction;
        private readonly Action<string> _stringAction;
        private readonly object _state;
        private System.Diagnostics.Stopwatch _timer;

        private Opwatch()
        {
            _timer = System.Diagnostics.Stopwatch.StartNew();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Opwatch"/> class.
        /// </summary>
        /// <param name="writeDuration">Duration of the write.</param>
        /// <exception cref="System.ArgumentNullException">writeDuration</exception>
        public Opwatch(Action<TimeSpan, object> writeDuration, object state)
            : this()
        {
            if (writeDuration == null)
                throw new ArgumentNullException("writeDuration");

            _timeSpanAction = writeDuration;
            _state = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Opwatch"/> class.
        /// </summary>
        /// <param name="writeLog">The write log.</param>
        /// <exception cref="System.ArgumentNullException">writeLog</exception>
        public Opwatch(Action<string> writeLog)
            : this()
        {
            if (writeLog == null)
                throw new ArgumentNullException("writeLog");

            _stringAction = writeLog;
        }



        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _timer.Stop();

            if (_timeSpanAction != null)
            {
                _timeSpanAction.Invoke(_timer.Elapsed, _state);
                _timer = null;
            }
            else
            {
                _stringAction.Invoke(String.Concat("Operation duration: ", _timer.Elapsed.TotalSeconds.ToString("N3"), "s"));
                _timer = null;
            }
        }

        #endregion
    }
}
