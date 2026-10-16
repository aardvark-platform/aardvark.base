using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ColorFormatTests
    {
        internal static readonly string[] OrdinaryNames =
        {
            "BW", "Gray", "Alpha", "GrayAlpha", "RG", "RGB", "BGR", "RGBA", "BGRA",
            "CieXYZ", "CieYxy", "CieLab", "CieLuv", "Yuv", "HSL", "HSV"
        };

        private static IEnumerable<Assembly> Assemblies()
        {
            yield return typeof(Col).Assembly;
            var directory = Path.GetDirectoryName(typeof(ColorFormatTests).Assembly.Location);
            var path = Path.GetFullPath(Path.Combine(directory!, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Expected sibling core assembly at '{path}'.");
            yield return Assembly.LoadFile(path);
        }

        [Test]
        public void PremultipliedNamesRoundTripWithoutChangingIntentsOrChannels()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in Assemblies())
                {
                    var col = assembly.GetType(typeof(Col).FullName!);
                    var formatType = assembly.GetType(typeof(Col.Format).FullName!);
                    foreach (var name in new[] { "RGBP", "BGRP" })
                    {
                        var format = Enum.Parse(formatType!, name);
                        var intent = name == "RGBP" ? "RGBA" : "BGRA";
                        var symbol = col!.GetMethod("GetName")!.Invoke(null, new[] { format });
                        var context = $"{assembly.Location}: {name}";
                        Assert.AreEqual(name, symbol!.ToString(), context);
                        Assert.AreEqual(format, col.GetMethod("FormatOfName")!.Invoke(null, new[] { symbol }), context);
                        Assert.AreEqual(intent, col.GetMethod("GetIntent")!.Invoke(null, new[] { format })!.ToString(), context);
                        Assert.AreEqual(true, col.GetMethod("IsPremultiplied")!.Invoke(null, new[] { format }), context);
                        Assert.AreEqual(4, col.GetMethod("ChannelCount")!.Invoke(null, new[] { format }), context);
                        var channels = (Array)col.GetMethod("ChannelsOfFormat")!.Invoke(null, new[] { format })!;
                        CollectionAssert.AreEqual(
                            name == "RGBP"
                                ? new[] { "Red", "Green", "Blue", "PremultipliedAlpha" }
                                : new[] { "Blue", "Green", "Red", "PremultipliedAlpha" },
                            channels.Cast<object>().Select(x => x.ToString()).ToArray(), context);
                    }
                }
            });
        }

        [Test]
        public void OrdinaryNamesAndUnsupportedValuesKeepLookupBehavior()
        {
            foreach (var assembly in Assemblies())
            {
                var col = assembly.GetType(typeof(Col).FullName!);
                var formatType = assembly.GetType(typeof(Col.Format).FullName!);
                var createSymbol = assembly.GetType(typeof(Symbol).FullName!)!.GetMethod("Create", new[] { typeof(string) });
                foreach (var name in OrdinaryNames)
                {
                    var format = Enum.Parse(formatType!, name);
                    var symbol = createSymbol!.Invoke(null, new object[] { name });
                    var context = $"{assembly.Location}: {name}";
                    Assert.AreEqual(name, col!.GetMethod("GetName")!.Invoke(null, new[] { format })!.ToString(), context);
                    Assert.AreEqual(format, col.GetMethod("FormatOfName")!.Invoke(null, new[] { symbol }), context);
                    Assert.AreEqual(name, col.GetMethod("GetIntent")!.Invoke(null, new[] { format })!.ToString(), context);
                    Assert.AreEqual(false, col.GetMethod("IsPremultiplied")!.Invoke(null, new[] { format }), context);
                }

                foreach (int value in new[] { (int)Col.Format.None, -1, int.MaxValue })
                foreach (var method in new[] { "GetName", "GetIntent", "ChannelCount" })
                {
                    var error = Assert.Throws<TargetInvocationException>(() =>
                        col!.GetMethod(method)!.Invoke(null, new[] { Enum.ToObject(formatType!, value) }));
                    Assert.That(error!.InnerException, Is.TypeOf<KeyNotFoundException>(), $"{assembly.Location}: {method}({value})");
                }
                var unknown = createSymbol!.Invoke(null, new object[] { "UnknownColorFormat" });
                var decodeError = Assert.Throws<TargetInvocationException>(() =>
                    col!.GetMethod("FormatOfName")!.Invoke(null, new[] { unknown }));
                Assert.That(decodeError!.InnerException, Is.TypeOf<KeyNotFoundException>(), assembly.Location);
            }
        }
    }
}
