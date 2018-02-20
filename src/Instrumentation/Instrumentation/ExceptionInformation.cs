
namespace Evoq.Instrumentation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Creates a small synopsis of an exception for logging or UI.
    /// </summary>
    public sealed class ExceptionInformation
    {
        #region Fields

        List<Exception> _exceptions = new List<Exception>();
        string[] _sup;

        #endregion

        #region Ctor

        private ExceptionInformation(Exception e, string[] supplementary)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            this.Exception = e;

            var ex = e;
            while (ex != null)
            {
                _exceptions.Add(ex);
                ex = ex.InnerException;
            }

            _sup = supplementary;
        }

        #endregion

        #region Properties

        public Exception Exception { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Produces a string representation of the ExceptionInformation.
        /// </summary>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (string s in _sup)
                result.AppendLine(s);

            for (int i = 0; i < _exceptions.Count; i++)
                result.AppendLine(ExceptionInformation.BuildExceptionInfo(i * 4, _exceptions[i]));

            return result.ToString();
        }

        private static string BuildExceptionInfo(int padding, Exception e)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Indent("Exception.Type: ", padding));
            sb.AppendLine(Indent(e.GetType().Name, padding));

            sb.Append(Indent("Exception.Source: ", padding));
            sb.AppendLine(String.IsNullOrEmpty(e.Source) ? Indent(String.Empty, padding) : Indent(e.Source, padding));

            sb.Append(Indent("Exception.Message: ", padding));
            sb.AppendLine(String.IsNullOrEmpty(e.Message) ? Indent(String.Empty, padding) : Indent(e.Message, padding));

            sb.Append(Indent("Exception.StackTrace: ", padding));
            sb.AppendLine(String.IsNullOrEmpty(e.StackTrace) ? Indent(String.Empty, padding) : Indent(e.StackTrace, padding));

            return sb.ToString();
        }

        private static string Indent(string text, int spaces)
        {
            return text.Insert(0, "".PadLeft(spaces));
        }

        /// <summary>
        /// Creates a new instance from an exception and optional strings.
        /// </summary>
        public static string Create(Exception e, params string[] supplementary)
        {
            return (new ExceptionInformation(e, supplementary)).ToString();
        }

        #endregion
    }
}