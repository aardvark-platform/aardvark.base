using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;

namespace Aardvark.Tests.TextUtilities
{
    [TestFixture]
    public static class TextRealParsingTests
    {
        private sealed class Api
        {
            internal readonly bool Single;
            internal readonly string Name;
            private readonly ConstructorInfo textConstructor, parserConstructor, valueConstructor;
            private readonly MethodInfo parsed, parse, tryGet;
            private readonly Delegate genericParsed, genericParse;
            private readonly FieldInfo error, length, value, parserText, position, lastEnd, lastCase, lastWhiteSpace;
            private readonly PropertyInfo valid, invalid;

            internal Api(Assembly assembly, bool single, string target)
            {
                Single = single;
                Name = target + (single ? "/float" : "/double");
                var scalar = single ? typeof(float) : typeof(double);
                var text = assembly.GetType("Aardvark.Base.Text", true);
                var result = assembly.GetType("Aardvark.Base.ParsedValue`1", true).MakeGenericType(scalar);
                var generic = assembly.GetType("Aardvark.Base.Text`1", true).MakeGenericType(scalar);
                var parser = assembly.GetType("Aardvark.Base.TextParser", true);
                textConstructor = text.GetConstructor(new[] { typeof(string), typeof(int), typeof(int) });
                parserConstructor = parser.GetConstructor(Type.EmptyTypes);
                valueConstructor = result.GetConstructor(new[] { scalar, typeof(int) });
                parsed = text.GetMethod(single ? "ParsedValueOfFloatAt" : "ParsedValueOfDoubleAt", new[] { typeof(int) });
                parse = text.GetMethod(single ? "ParseFloat" : "ParseDouble", Type.EmptyTypes);
                tryGet = parser.GetMethod(single ? "TryGetFloat" : "TryGetDouble", new[] { result.MakeByRefType() });
                genericParsed = (Delegate)generic.GetField("ParsedValueAt").GetValue(null);
                genericParse = (Delegate)generic.GetField("Parse").GetValue(null);
                error = result.GetField("Error"); length = result.GetField("Length"); value = result.GetField("Value");
                valid = result.GetProperty("IsValid"); invalid = result.GetProperty("IsInValid");
                parserText = parser.GetField("Text"); position = parser.GetField("Pos");
                lastEnd = parser.GetField("LastEnd"); lastCase = parser.GetField("LastCase"); lastWhiteSpace = parser.GetField("LastWhiteSpace");
                Assert.That(textConstructor, Is.Not.Null, Name);
                Assert.That(parserConstructor, Is.Not.Null, Name);
                Assert.That(valueConstructor, Is.Not.Null, Name);
                Assert.That(parsed.ReturnType, Is.EqualTo(result), Name);
                Assert.That(parse.ReturnType, Is.EqualTo(scalar), Name);
                Assert.That(tryGet.ReturnType, Is.EqualTo(typeof(bool)), Name);
                Assert.That(value.FieldType, Is.EqualTo(scalar), Name);
                foreach (var method in new[] { parsed, parse, tryGet, genericParsed.Method, genericParse.Method })
                    Assert.That(method.DeclaringType.Assembly, Is.SameAs(assembly), Name);
            }

            private object Scalar(double x) => Single ? (object)(float)x : x;
            internal object View(string token, int offset, int relative, string suffix = "")
            {
                string payload = new string(' ', relative) + token + suffix;
                string backing = offset == 0 ? payload : "HEAD" + payload + "e9999!";
                return textConstructor.Invoke(new object[] { backing, offset, payload.Length });
            }
            private static object Invoke(Func<object> action)
            {
                try { return action(); }
                catch (TargetInvocationException e) when (e.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                    throw;
                }
            }
            internal object Parsed(object text, int start, bool generic)
                => Invoke(() => generic ? genericParsed.DynamicInvoke(text, start) : parsed.Invoke(text, new object[] { start }));
            internal double Parse(object text, bool generic)
                => Convert.ToDouble(Invoke(() => generic ? genericParse.DynamicInvoke(text) : parse.Invoke(text, null)));
            internal void Check(object result, int expectedError, int expectedLength, double expectedValue, string context)
            {
                Assert.That(Convert.ToInt32(error.GetValue(result)), Is.EqualTo(expectedError), context + " error");
                Assert.That(length.GetValue(result), Is.EqualTo(expectedLength), context + " length");
                Assert.That(valid.GetValue(result), Is.EqualTo(expectedError == 0), context + " valid");
                Assert.That(invalid.GetValue(result), Is.EqualTo(expectedError != 0), context + " invalid");
                Assert.That(BitConverter.DoubleToInt64Bits(Convert.ToDouble(value.GetValue(result))),
                    Is.EqualTo(BitConverter.DoubleToInt64Bits(expectedValue)), context + " value bits");
            }
            internal void Try(object text, int absoluteStart, bool success, int expectedError, int expectedLength, double expected, string context)
            {
                var parser = parserConstructor.Invoke(null);
                parserText.SetValue(parser, text); position.SetValue(parser, absoluteStart);
                lastEnd.SetValue(parser, 41); lastCase.SetValue(parser, 43); lastWhiteSpace.SetValue(parser, 47);
                var args = new[] { valueConstructor.Invoke(new[] { Scalar(7), (object)99 }) };
                Assert.That(tryGet.Invoke(parser, args), Is.EqualTo(success), context + " TryGet");
                Check(args[0], expectedError, expectedLength, expected, context);
                Assert.That(position.GetValue(parser), Is.EqualTo(absoluteStart + (success ? expectedLength : 0)), context + " cursor");
                Assert.That(parserText.GetValue(parser), Is.EqualTo(text), context + " parser text");
                Assert.That(lastEnd.GetValue(parser), Is.EqualTo(41), context + " LastEnd");
                Assert.That(lastCase.GetValue(parser), Is.EqualTo(43), context + " LastCase");
                Assert.That(lastWhiteSpace.GetValue(parser), Is.EqualTo(47), context + " LastWhiteSpace");
            }
        }

        private static IEnumerable<Api> Implementations()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required actual core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                yield return new Api(assembly, true, target);
                yield return new Api(assembly, false, target);
            }
        }

        private static IEnumerable<string> OverflowTokens(bool single)
        {
            foreach (string magnitude in new[] { single ? "1e39" : "1e309", single ? "3.4028236e38" : "1.7976931348623159e308",
                "1e400", "9E+9999", new string('9', single ? 80 : 400), new string('9', single ? 80 : 400) + "e-1",
                new string('9', single ? 80 : 400) + ".125e-20" })
            foreach (string sign in new[] { "", "+", "-" }) yield return sign + magnitude;
        }

        [Test]
        public static void OverflowReportsOutOfRangeWithWholeTokenLength()
        {
            foreach (var api in Implementations())
            foreach (string token in OverflowTokens(api.Single))
            foreach (int offset in new[] { 0, 4 })
            foreach (int relative in new[] { 0, 3 })
            foreach (string suffix in new[] { "", ";42" })
            foreach (bool generic in new[] { false, true })
            {
                string context = $"{api.Name}: {token}, offset={offset}, relative={relative}, suffix={suffix}, generic={generic}";
                api.Check(api.Parsed(api.View(token, offset, relative, suffix), relative, generic),
                    (int)ParseError.OutOfRange, token.Length, 0, context);
            }
        }

        [Test]
        public static void OverflowWrappersRejectAndTryGetPreservesCursor()
        {
            foreach (var api in Implementations())
            foreach (string token in OverflowTokens(api.Single))
            foreach (int offset in new[] { 0, 4 })
            foreach (int relative in new[] { 0, 3 })
            {
                string context = $"{api.Name}: {token}, offset={offset}, relative={relative}";
                foreach (bool generic in new[] { false, true })
                    Assert.Throws<ArgumentException>(() => api.Parse(api.View(token, offset, relative), generic), context + $", generic={generic}");
                foreach (string suffix in new[] { "", ";42" })
                    api.Try(api.View(token, offset, relative, suffix), offset + relative, false,
                        (int)ParseError.OutOfRange, token.Length, 0, context + ", suffix=" + suffix);
            }
        }

        private static IEnumerable<string> FiniteTokens(bool single)
        {
            var magnitudes = new List<string> { "0", "0.0", "0e999999999999999999", "0." + new string('0', 400) + "e9999",
                "1", "1.", "1.25", "125E+2", "25e-2", "1e-9999", "1e-4000" };
            magnitudes.AddRange(single ? new[] { "3.40282346638528859811704183484516925440e38", "3.4028235e38", "1.1754943508222875e-38", "1.401298464324817e-45", "7.006492321624086e-46" }
                : new[] { "1.7976931348623157e308", "1.7976931348623158e308", "2.2250738585072014e-308", "4.9406564584124654e-324", "2.4703282292062327e-324", "1e39", "1e100" });
            foreach (string magnitude in magnitudes)
            foreach (string sign in new[] { "", "+", "-" }) yield return sign + magnitude;
            var random = new Random(199157);
            for (int i = 0; i < 1024; i++)
            {
                double x = Math.ScaleB(1 + random.NextDouble(), random.Next(single ? -149 : -1074, single ? 127 : 1023));
                if ((i & 1) != 0) x = -x;
                yield return single ? ((float)x).ToString("R", CultureInfo.InvariantCulture) : x.ToString("R", CultureInfo.InvariantCulture);
            }
        }

        [Test]
        public static void FiniteRoundingUnderflowAndLexicalContractsRemain()
        {
            foreach (var api in Implementations())
            {
                foreach (string token in FiniteTokens(api.Single))
                {
                    double expected = api.Single ? float.Parse(token, CultureInfo.InvariantCulture) : double.Parse(token, CultureInfo.InvariantCulture);
                    Assert.That(double.IsFinite(expected), Is.True, api.Name + ": finite oracle " + token);
                    foreach (int offset in new[] { 0, 4 })
                    foreach (int relative in new[] { 0, 3 })
                    {
                        string context = $"{api.Name}: {token}, offset={offset}, relative={relative}";
                        foreach (bool generic in new[] { false, true })
                        {
                            api.Check(api.Parsed(api.View(token, offset, relative, ";42"), relative, generic), 0, token.Length, expected, context);
                            Assert.That(BitConverter.DoubleToInt64Bits(api.Parse(api.View(token, offset, relative), generic)),
                                Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), context + ", wrapper");
                        }
                        api.Try(api.View(token, offset, relative, ";42"), offset + relative, true, 0, token.Length, expected, context);
                    }
                }
                foreach (var spec in new[] { ("", ParseError.EndOfText, 0), ("+", ParseError.EndOfText, 1), ("-", ParseError.EndOfText, 1),
                    ("1e", ParseError.EndOfText, 2), ("1e+", ParseError.EndOfText, 3), ("1e-", ParseError.EndOfText, 3), ("1.e+", ParseError.EndOfText, 4),
                    ("1eX", ParseError.IllegalCharacter, 2), ("1e+X", ParseError.IllegalCharacter, 3), ("-1e-!", ParseError.IllegalCharacter, 4),
                    (".5", ParseError.IllegalCharacter, 0), ("-.5", ParseError.IllegalCharacter, 1), ("NaN", ParseError.IllegalCharacter, 0),
                    ("Infinity", ParseError.IllegalCharacter, 0), ("-Infinity", ParseError.IllegalCharacter, 1), (",", ParseError.IllegalCharacter, 0) })
                foreach (int offset in new[] { 0, 4 })
                foreach (int relative in new[] { 0, 3 })
                {
                    string context = $"{api.Name}: malformed={spec.Item1}, offset={offset}, relative={relative}";
                    var text = api.View(spec.Item1, offset, relative);
                    foreach (bool generic in new[] { false, true })
                    {
                        api.Check(api.Parsed(text, relative, generic), (int)spec.Item2, spec.Item3, 0, context);
                        Assert.Throws<ArgumentException>(() => api.Parse(text, generic), context);
                    }
                    api.Try(text, offset + relative, false, (int)spec.Item2, spec.Item3, 0, context);
                }
                foreach (var spec in new[] { ("12,34", "12"), ("1.2.3", "1.2"), ("1e2x", "1e2"), ("1 2", "1"), ("0x10", "0") })
                foreach (int offset in new[] { 0, 4 })
                foreach (int relative in new[] { 0, 3 })
                {
                    double expected = api.Single ? float.Parse(spec.Item2, CultureInfo.InvariantCulture) : double.Parse(spec.Item2, CultureInfo.InvariantCulture);
                    var text = api.View(spec.Item1, offset, relative);
                    string context = $"{api.Name}: prefix={spec.Item1}, offset={offset}, relative={relative}";
                    foreach (bool generic in new[] { false, true })
                    {
                        api.Check(api.Parsed(text, relative, generic), 0, spec.Item2.Length, expected, context);
                        Assert.Throws<ArgumentException>(() => api.Parse(text, generic), context);
                    }
                    api.Try(text, offset + relative, true, 0, spec.Item2.Length, expected, context);
                }
            }
        }
    }
}
