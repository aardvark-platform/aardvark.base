using System;
using System.IO;
using System.Linq;
using Aardvark.Base;
using Aardvark.Base.Coder;
using NUnit.Framework;

namespace Aardvark.Tests.IO
{
    [TestFixture]
    public class ColorFormatCodingTests
    {
        [Test]
        public void PremultipliedMetadataRetainsNamesAndPixelTypes()
        {
            Assert.Multiple(() =>
            {
                foreach (var name in new[] { "RGBP", "BGRP" })
                foreach (var type in new[] { typeof(byte), typeof(float) })
                    CheckMetadata(name, type, canonical: false);
            });
        }

        [Test]
        public void OrdinaryRecordsAndCanonicalNamesKeepTheirDecoding()
        {
            foreach (var name in ColorFormatTests.OrdinaryNames)
            foreach (var type in new[] { typeof(byte), typeof(float) })
                CheckMetadata(name, type, canonical: false);

            // These records are independent of GetName, including names already
            // recognized by existing readers before the writing correction.
            foreach (var name in ColorFormatTests.OrdinaryNames.Concat(new[] { "RGBP", "BGRP" }))
            foreach (var type in new[] { typeof(byte), typeof(float) })
                CheckMetadata(name, type, canonical: true);
        }

        private static void CheckMetadata(string name, Type type, bool canonical)
        {
            var expected = (Col.Format)Enum.Parse(typeof(Col.Format), name);
            bool premultiplied = name == "RGBP" || name == "BGRP";
            var context = $"{typeof(Col).Assembly.Location}: {name}/{type.Name}, canonical={canonical}";
            using var stream = new MemoryStream();
            using (var writer = new BinaryWritingCoder(stream))
            {
                if (canonical)
                {
                    string formatName = name;
                    writer.CodeString(ref formatName);
                    writer.CodeType(ref type);
                    writer.CodeString(ref formatName);
                }
                else
                {
                    var format = expected;
                    var pixel = new PixFormat(type, expected);
                    writer.CodeColFormat(ref format);
                    writer.CodePixFormat(ref pixel);
                }
            }

            stream.Position = 0;
            using (var reader = new BinaryReadingCoder(stream))
            {
                string colorName = null, pixelName = null;
                Type pixelType = null;
                reader.CodeString(ref colorName);
                reader.CodeType(ref pixelType);
                reader.CodeString(ref pixelName);
                Assert.AreEqual(name, colorName, context);
                Assert.AreEqual(name, pixelName, context);
                Assert.AreSame(type, pixelType, context);
                Assert.AreEqual(stream.Length, stream.Position, context);
            }

            stream.Position = 0;
            using (var reader = new BinaryReadingCoder(stream))
            {
                var format = Col.Format.None;
                var pixel = default(PixFormat);
                reader.CodeColFormat(ref format);
                reader.CodePixFormat(ref pixel);
                Assert.AreEqual(expected, format, context);
                Assert.AreEqual(expected, pixel.Format, context);
                Assert.AreSame(type, pixel.Type, context);
                Assert.AreEqual(premultiplied, format.IsPremultiplied(), context);
                Assert.AreEqual(premultiplied, pixel.Format.IsPremultiplied(), context);
                Assert.AreEqual(stream.Length, stream.Position, context);
            }
        }
    }
}
