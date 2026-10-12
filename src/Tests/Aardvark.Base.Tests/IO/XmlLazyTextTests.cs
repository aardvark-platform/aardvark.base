using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.IO
{
    [TestFixture]
    public class XmlLazyTextTests
    {
        private static void AssertClosed(StreamWriter writer, Stream stream, string context)
        {
            Assert.IsFalse(stream.CanRead, context);
            Assert.IsFalse(stream.CanWrite, context);
            Assert.Throws<ObjectDisposedException>(() => writer.Write("after disposal"), context);
        }

        [Test]
        public void GetTextReadsBufferedAndExplicitlyFlushedPayloads()
        {
            var payloads = new List<string> { "", "a", " \t\r\n", "é漢🙂e\u0301<&>\"\r\n" };
            foreach (var length in new[] { 1023, 1024, 1025, 2047, 2048, 2049, 8193 })
            {
                payloads.Add(new string('a', length) + " \t\r\n<&>");
                payloads.Add(new string('漢', length) + "🙂é\uFEFF \t\r\n");
            }
            foreach (var payload in payloads)
            foreach (var chunk in new[] { 1, 31, 1024 })
            foreach (var mode in new[] { 0, 1, 2 })
            {
                var calls = 0;
                var context = $"length={payload.Length}, chunk={chunk}, flush/rewind mode={mode}";
                var lazy = new XmlLazyText(writer =>
                {
                    calls++;
                    Assert.AreEqual(Encoding.UTF8.CodePage, writer.Encoding.CodePage, context);
                    Assert.AreEqual(0, writer.Encoding.GetPreamble().Length, context);
                    for (var i = 0; i < payload.Length; i += chunk)
                        writer.Write(payload.Substring(i, Math.Min(chunk, payload.Length - i)));
                    if (mode != 0) writer.Flush();
                    if (mode == 2) writer.BaseStream.Position = 0;
                });
                var result = lazy.GetText();
                Assert.AreEqual(payload, result.ToString(), context);
                Assert.AreEqual(0, result.Start, context);
                Assert.AreEqual(payload.Length, result.End, context);
                Assert.AreEqual(1, calls, context);
            }
        }

        [Test]
        public void GetTextRunsFreshCallbacksThroughXmlItemAccessors()
        {
            foreach (var access in new[] { 0, 1, 2 })
            {
                var calls = 0;
                StreamWriter capturedWriter = null;
                Stream capturedStream = null;
                Stream previousStream = null;
                var lazy = new XmlLazyText(writer =>
                {
                    calls++;
                    capturedWriter = writer;
                    capturedStream = writer.BaseStream;
                    writer.Write($"{calls}: 漢🙂 \t\r\n<&>");
                });
                var item = new XmlItem();
                item.Add(lazy);
                for (var call = 1; call <= 3; call++)
                {
                    var context = $"access={access}, call={call}";
                    var result = access == 0 ? lazy.GetText() : access == 1 ? item.RequiredOnlySubText : item.RequiredOnlySubString.ToText();
                    Assert.AreEqual($"{call}: 漢🙂 \t\r\n<&>", result.ToString(), context);
                    Assert.AreEqual(call, calls, context);
                    Assert.AreNotSame(previousStream, capturedStream, context);
                    AssertClosed(capturedWriter, capturedStream, context);
                    previousStream = capturedStream;
                }
            }
        }

        [Test]
        public void GetTextRetainsCallbackFailuresAndClosesOwnedResources()
        {
            foreach (var partial in new[] { "", "a", new string('a', 1023), new string('漢', 4097), "prefix\ud800" })
            foreach (var flush in new[] { false, true })
            {
                // Leave the incomplete surrogate buffered so cleanup, not the callback, fails to encode it.
                if (flush && char.IsHighSurrogate(partial.Length == 0 ? '\0' : partial[partial.Length - 1])) continue;
                var error = new InvalidOperationException("callback failed");
                var calls = 0;
                StreamWriter capturedWriter = null;
                Stream capturedStream = null;
                var lazy = new XmlLazyText(writer =>
                {
                    calls++;
                    capturedWriter = writer;
                    capturedStream = writer.BaseStream;
                    writer.Write(partial);
                    if (flush) writer.Flush();
                    throw error;
                });
                for (var call = 1; call <= 2; call++)
                {
                    var context = $"partial length={partial.Length}, flush={flush}, call={call}";
                    Assert.AreSame(error, Assert.Throws<InvalidOperationException>(() => lazy.GetText()), context);
                    Assert.AreEqual(call, calls, context);
                    AssertClosed(capturedWriter, capturedStream, context);
                }
            }
            Assert.Throws<NullReferenceException>(() => new XmlLazyText(null).GetText());
        }
    }
}
