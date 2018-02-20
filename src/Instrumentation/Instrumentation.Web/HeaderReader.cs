using System;
using System.Collections.Generic;
using System.Linq;

namespace Evoq.Instrumentation.Web
{
    /// <summary>
    /// Reads header values.
    /// </summary>
    public sealed class HeaderReader
    {
        /// <summary>
        /// Creates one.
        /// </summary>
        /// <param name="includeMissingHeaders">When true, any missing headers will be included in the results but with a null value.</param>
        public HeaderReader(bool includeMissingHeaders)
        {
            this.IncludeMissingHeaders = includeMissingHeaders;
        }

        //

        /// <summary>
        /// When true, any missing headers will be included in the results but with a null value.
        /// </summary>
        public bool IncludeMissingHeaders { get; } = true;

        //

        /// <summary>
        /// Reads the specified headers.
        /// </summary>
        /// <param name="headers">The headers from the request.</param>
        /// <param name="headerKeys">The header key names to return.</param>        
        /// <returns>A dictionary of header keys and values which may be null.</returns>
        public Dictionary<string, string> ReadHeaderValues(IDictionary<string, string[]> headers, params string[] headerKeys)
        {
            if (headers == null)
            {
                throw new System.ArgumentNullException(nameof(headers));
            }

            Dictionary<string, string> results = new Dictionary<string, string>(12);

            foreach (var key in headerKeys)
            {
                string headerValue;
                if (TryGetFirstHeaderValue(headers, key, out headerValue))
                {
                    results.Add(key, headerValue);
                }
                else
                {
                    if (this.IncludeMissingHeaders)
                    {
                        results.Add(key, null);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Reads the specified headers as a set of key and value pairs separated by commas.
        /// </summary>
        /// <param name="headers">The headers from the request.</param>
        /// <param name="headerKeys">The header key names to return.</param>        
        public string ReadHeaderValuesAsString(IDictionary<string, string[]> headers, params string[] headerKeys)
        {
            return FormatHeaderValues(this.ReadHeaderValues(headers, headerKeys));
        }

        /// <summary>
        /// Formats headers as a set of key and value pairs separated by commas. 
        /// </summary>
        public string FormatHeaderValues(Dictionary<string, string> headers)
        {
            return String.Join(", ", headers.Select(pair => FormatPair(pair)).ToArray());
        }

        private string FormatPair(KeyValuePair<string, string> pair)
        {
            if (pair.Value != null)
            {
                return $"{pair.Key}: {pair.Value}";
            }
            else
            {
                return $"{pair.Key}: null";
            }
        }

        private static bool TryGetFirstHeaderValue(IDictionary<string, string[]> headers, string headerKey, out string value)
        {
            string[] values;
            if (headers != null && headers.TryGetValue(headerKey, out values))
            {
                value = values[0];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
