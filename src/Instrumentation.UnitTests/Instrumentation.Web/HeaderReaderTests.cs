using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Evoq.Instrumentation.Web
{
    [TestClass]
    public class HeaderReaderTests
    {
        [TestMethod]
        public void RequestTracingHeaderReader_ctor__when__set_to_exclude__then__property_is_false()
        {
            var reader = new HeaderReader(false);

            Assert.IsFalse(reader.IncludeMissingHeaders);
        }

        [TestMethod]
        public void RequestTracingHeaderReader_ctor__when__set_to_include__then__property_is_true()
        {
            var reader = new HeaderReader(true);

            Assert.IsTrue(reader.IncludeMissingHeaders);
        }

        [TestMethod]
        public void RequestTracingHeaderReader__ReadHeaderValues__when__include__and__headers_contains_one_key__then__only_one_key_is_returned()
        {
            var reader = new HeaderReader(includeMissingHeaders: true);

            var actual = reader.ReadHeaderValues(new Dictionary<string, string[]>()
            {
                ["Header1"] = new string[] { "Value1" },
                ["Header2"] = new string[] { "Value2" },
                [RequestTracingHeaderKeys.RequestIdHeaderKey] = new string[] { "Present" }

            }, RequestTracingHeaderKeys.RequestIdHeaderKey, RequestTracingHeaderKeys.XRequestIdHeaderKey);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("Present", actual[RequestTracingHeaderKeys.RequestIdHeaderKey]);
            Assert.IsNull(actual[RequestTracingHeaderKeys.XRequestIdHeaderKey]);
        }

        [TestMethod]
        public void RequestTracingHeaderReader__ReadHeaderValues__when__exclude__and__headers_contains_one_key__then__only_one_key_is_returned()
        {
            var reader = new HeaderReader(includeMissingHeaders: false);

            var actual = reader.ReadHeaderValues(new Dictionary<string, string[]>()
            {
                ["Header1"] = new string[] { "Value1" },
                ["Header2"] = new string[] { "Value2" },
                [RequestTracingHeaderKeys.RequestIdHeaderKey] = new string[] { "Present" }

            }, RequestTracingHeaderKeys.RequestIdHeaderKey, RequestTracingHeaderKeys.XRequestIdHeaderKey);

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("Present", actual[RequestTracingHeaderKeys.RequestIdHeaderKey]);
        }

        [TestMethod]
        public void RequestTracingHeaderReader__ReadHeaderValuesAsString__when__include__and__headers_contains_one_key__then__only_one_key_is_returned()
        {
            var reader = new HeaderReader(includeMissingHeaders: true);

            var actual = reader.ReadHeaderValuesAsString(new Dictionary<string, string[]>()
            {
                ["Header1"] = new string[] { "Value1" },
                ["Header2"] = new string[] { "Value2" },
                [RequestTracingHeaderKeys.RequestIdHeaderKey] = new string[] { "Present" }

            }, RequestTracingHeaderKeys.RequestIdHeaderKey, RequestTracingHeaderKeys.XRequestIdHeaderKey);

            Assert.AreEqual($"{RequestTracingHeaderKeys.RequestIdHeaderKey}: Present, {RequestTracingHeaderKeys.XRequestIdHeaderKey}: null", actual);
        }
    }
}
