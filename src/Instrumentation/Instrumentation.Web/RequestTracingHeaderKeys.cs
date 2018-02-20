using System.Collections.Generic;

namespace Evoq.Instrumentation.Web
{
    public static class RequestTracingHeaderKeys
    {
        public static string XAmazonTraceIdHeaderKey { get; } = "X-Amzn-Trace-Id";

        public static string XRequestIdHeaderKey { get; } = "X-Request-Id";

        public static string XTraceIdHeaderKey { get; } = "X-Trace-Id";

        public static string XCorrelationId { get; } = "X-Correlation-ID";

        public static string RequestIdHeaderKey { get; } = "Request-Id";

        public static string TraceIdHeaderKey { get; } = "Trace-Id";

        public static string CorrelationId { get; } = "Correlation-ID";

        /// <summary>
        /// Contains all the different request tracing header keys in order of preference.
        /// </summary>
        public static IReadOnlyCollection<string> All = new string[]
        {
            CorrelationId,
            TraceIdHeaderKey,
            XCorrelationId,
            XTraceIdHeaderKey,
            RequestIdHeaderKey,
            XRequestIdHeaderKey,
            XAmazonTraceIdHeaderKey,
        };
    }
}
