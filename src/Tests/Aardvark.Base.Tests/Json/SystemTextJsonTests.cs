using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aardvark.Base.Tests.Json
{
    [TestFixture]
    class SystemTextJsonTests
    {
        #region Helpers

        public static void W(Utf8JsonWriter writer, M22d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref M22d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new double[4];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetDouble();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M22d(xs);
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(Utf8JsonWriter writer, M22f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref M22f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new float[4];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetSingle();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M22f(xs);
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(Utf8JsonWriter writer, M33d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref M33d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new double[9];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetDouble();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M33d(xs);
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(Utf8JsonWriter writer, M33f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref M33f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new float[9];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetSingle();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M33f(xs);
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(Utf8JsonWriter writer, V2d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref V2d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetDouble();
                reader.Read(); value.Y = reader.GetDouble();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
       
        public static void W(Utf8JsonWriter writer, V2f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref V2f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetSingle();
                reader.Read(); value.Y = reader.GetSingle();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(Utf8JsonWriter writer, V3d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref V3d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetDouble();
                reader.Read(); value.Y = reader.GetDouble();
                reader.Read(); value.Z = reader.GetDouble();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(Utf8JsonWriter writer, V3f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(ref Utf8JsonReader reader, ref V3f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetSingle();
                reader.Read(); value.Y = reader.GetSingle();
                reader.Read(); value.Z = reader.GetSingle();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        private class Affine2dConverter : JsonConverter<Affine2d>
        {
            public override Affine2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Affine2d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "linear": case "Linear": R(ref reader, ref result.Linear, options); break;
                            case "trans": case "Trans": R(ref reader, ref result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}. Error 6f0df661-69db-449b-8b03-2b083d0d4f1d.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Affine2d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Linear");
                W(writer, value.Linear, options);
                writer.WritePropertyName("Trans");
                W(writer, value.Trans, options);
                writer.WriteEndObject();
            }
        }
        private class Affine2fConverter : JsonConverter<Affine2f>
        {
            public override Affine2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Affine2f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "linear": case "Linear": R(ref reader, ref result.Linear, options); break;
                            case "trans": case "Trans": R(ref reader, ref result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}. Error b0e8b383-d38b-4ac7-91d0-870137757902.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Affine2f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Linear");
                W(writer, value.Linear, options);
                writer.WritePropertyName("Trans");
                W(writer, value.Trans, options);
                writer.WriteEndObject();
            }
        }
        private class Affine3dConverter : JsonConverter<Affine3d>
        {
            public override Affine3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Affine3d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "linear": case "Linear": R(ref reader, ref result.Linear, options); break;
                            case "trans": case "Trans": R(ref reader, ref result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}. Error c5b2eee9-bd52-44ac-bfae-6fb30f45dea0.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Affine3d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Linear");
                W(writer, value.Linear, options);
                writer.WritePropertyName("Trans");
                W(writer, value.Trans, options);
                writer.WriteEndObject();
            }
        }
        private class Affine3fConverter : JsonConverter<Affine3f>
        {
            public override Affine3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Affine3f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "linear": case "Linear": R(ref reader, ref result.Linear, options); break;
                            case "trans": case "Trans": R(ref reader, ref result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}. Error 4ea64c2a-7739-46b9-a95a-a85bec985735.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Affine3f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Linear");
                W(writer, value.Linear, options);
                writer.WritePropertyName("Trans");
                W(writer, value.Trans, options);
                writer.WriteEndObject();
            }
        }

        private static readonly List<JsonConverter> _converters = new()
        {
            new Affine2dConverter(), new Affine2fConverter(), new Affine3dConverter(), new Affine3fConverter(),
        };
        private static readonly JsonSerializerOptions _options;
        static SystemTextJsonTests()
        {
            _options = new();
            _options.Converters.AddRange(_converters);
        }

        private static void SerializeTest<T>(T a, string s) where T : IEquatable<T>
        {
            var json = JsonSerializer.Serialize(a, _options);
            Assert.IsTrue(json == s);
        }
        private static void DeserializeTest<T>(string json, T a) where T : IEquatable<T>
        {
            var b = JsonSerializer.Deserialize<T>(json, _options);
            Assert.IsTrue(a.Equals(b));
        }
        private static void RoundtripTest<T>(T a) where T : IEquatable<T>
        {
            var json = JsonSerializer.Serialize(a, _options);
            var b = JsonSerializer.Deserialize<T>(json, _options);
            Assert.IsTrue(a.Equals(b));
        }

        #endregion

        #region Affine[23][df]

        [Test] public void Affine2d_Roundtrip() => RoundtripTest(new Affine2d(M22d.Identity));
        [Test] public void Affine2f_Roundtrip() => RoundtripTest(new Affine2f(M22f.Identity));
        [Test] public void Affine3d_Roundtrip() => RoundtripTest(new Affine3d(M33d.Identity));
        [Test] public void Affine3f_Roundtrip() => RoundtripTest(new Affine3f(M33f.Identity));

        #endregion

        #region Box[23][dfil]

        [Test] public void Box2i_Roundtrip() => RoundtripTest(new Box2i(new V2i(1, -2), new V2i(-17, 0)));
        [Test] public void Box2l_Roundtrip() => RoundtripTest(new Box2l(new V2l(1, -2), new V2l(-17, 0)));
        [Test] public void Box2f_Roundtrip() => RoundtripTest(new Box2f(new V2f(1.1, -2.2), new V2f(-17.17, 0)));
        [Test] public void Box2d_Roundtrip() => RoundtripTest(new Box2d(new V2d(1.1, -2.2), new V2d(-17.17, 0)));
        [Test] public void Box3i_Roundtrip() => RoundtripTest(new Box3i(new V3i(1, -2, 0), new V3i(-17, 42, -555555555)));
        [Test] public void Box3l_Roundtrip() => RoundtripTest(new Box3l(new V3l(1, -2, 0), new V3l(-17, 42, -555555555)));
        [Test] public void Box3f_Roundtrip() => RoundtripTest(new Box3f(new V3f(1.1, -2.2, 0), new V3f(-17.17, 42.42, -555.001)));
        [Test] public void Box3d_Roundtrip() => RoundtripTest(new Box3d(new V3d(1.1, -2.2, 0), new V3d(-17.18, 42.42, -555.001)));

        #endregion

        #region C[34][b|d|f|ui|us], CieLabf, CIeLuvf, CieXYZf, CieYxyf, CMYKf, HSLf, HSVf, Yuvf

        [Test] public void C3b_Roundtrip() => RoundtripTest(new C3b(1,2,3));
        [Test] public void C3d_Roundtrip() => RoundtripTest(new C3b(1.2, 2.3, 3.4));
        [Test] public void C3f_Roundtrip() => RoundtripTest(new C3f(1.2, 2.3, 3.4));
        [Test] public void C3ui_Roundtrip() => RoundtripTest(new C3ui(1, 2, 3));
        [Test] public void C3us_Roundtrip() => RoundtripTest(new C3us(1, 2, 3));

        [Test] public void C4b_Roundtrip() => RoundtripTest(new C4b(1, 2, 3, 4));
        [Test] public void C4d_Roundtrip() => RoundtripTest(new C4b(1.2, 2.3, 3.4, 4.5));
        [Test] public void C4f_Roundtrip() => RoundtripTest(new C4f(1.2, 2.3, 3.4, 4.5));
        [Test] public void C4ui_Roundtrip() => RoundtripTest(new C4ui(1, 2, 3, 4));
        [Test] public void C4us_Roundtrip() => RoundtripTest(new C4us(1, 2, 3, 4));

        [Test] public void CieLabf_Roundtrip() => RoundtripTest(new CieLabf(0.1, 0.2, 0.3));
        [Test] public void CIeLuvf_Roundtrip() => RoundtripTest(new CIeLuvf(0.1, 0.2, 0.3));
        [Test] public void CieXYZf_Roundtrip() => RoundtripTest(new CieXYZf(0.1, 0.2, 0.3));
        [Test] public void CieYxyf_Roundtrip() => RoundtripTest(new CieYxyf(0.1, 0.2, 0.3));
        [Test] public void CMYKf_Roundtrip() => RoundtripTest(new CMYKf(0.1, 0.2, 0.3, 0.4));
        [Test] public void HSLf_Roundtrip() => RoundtripTest(new HSLf(0.1, 0.2, 0.3));
        [Test] public void HSVf_Roundtrip() => RoundtripTest(new HSVf(0.1, 0.2, 0.3));
        [Test] public void Yuvf_Roundtrip() => RoundtripTest(new Yuvf(0.1, 0.2, 0.3));

        #endregion

        #region Capsule3[df]

        [Test] public void Capsule3d_Roundtrip() => RoundtripTest(new Capsule3d(new V3d(1, -2, 3), new V3d(-4, 5, -6), 7.14));
        [Test] public void Capsule3f_Roundtrip() => RoundtripTest(new Capsule3f(new V3f(1, -2, 3), new V3f(-4, 5, -6), 7.14f));

        #endregion

        #region Cell, Cell2d

        [Test]
        public void Cell2d_Serialize()
        {
            SerializeTest(new Cell2d(1, 2, -1), "[1,2,-1]");
            SerializeTest(new Cell2d(10), "[10]");
            SerializeTest(Cell2d.Invalid, $"[{int.MinValue}]");
        }
        [Test]
        public void Cell2d_Deserialize()
        {
            DeserializeTest("\"[1,2,-1]\"", new Cell2d(1, 2, -1));
            DeserializeTest("[1,2,-1]", new Cell2d(1, 2, -1));
            DeserializeTest("{\"x\":1,\"y\":2,\"e\":-1}", new Cell2d(1, 2, -1));
            DeserializeTest("{\"X\":1,\"Y\":2,\"E\":-1}", new Cell2d(1, 2, -1));

            DeserializeTest("\"[10]\"", new Cell2d(10));
            DeserializeTest("[10]", new Cell2d(10));
            DeserializeTest("{\"e\":10}", new Cell2d(10));
            DeserializeTest("{\"E\":10}", new Cell2d(10));

            DeserializeTest($"\"[{int.MinValue}]\"", Cell2d.Invalid);
            DeserializeTest($"[{int.MinValue}]", Cell2d.Invalid);
            DeserializeTest($"{{\"e\":{int.MinValue}}}", Cell2d.Invalid);
            DeserializeTest($"{{\"E\":{int.MinValue}}}", Cell2d.Invalid);
        }
        [Test]
        public void Cell2d_Roundtrip()
        {
            RoundtripTest(new Cell2d(1, 2, -1));
            RoundtripTest(new Cell2d(10));
            RoundtripTest(Cell2d.Invalid);
        }

        [Test]
        public void Cell_Serialize()
        {
            SerializeTest(new Cell(1, 2, 3, -1), "[1,2,3,-1]");
            SerializeTest(new Cell(10), "[10]");
            SerializeTest(Cell.Invalid, $"[{int.MinValue}]");
        }
        [Test]
        public void Cell_Deserialize()
        {
            DeserializeTest("\"[1,2,3,-1]\"", new Cell(1, 2, 3, -1));
            DeserializeTest("[1,2,3,-1]", new Cell(1, 2, 3, -1));
            DeserializeTest("{\"x\":1,\"y\":2,\"z\":3,\"e\":-1}", new Cell(1, 2, 3, -1));
            DeserializeTest("{\"X\":1,\"Y\":2,\"Z\":3,\"E\":-1}", new Cell(1, 2, 3, -1));

            DeserializeTest("\"[10]\"", new Cell(10));
            DeserializeTest("[10]", new Cell(10));
            DeserializeTest("{\"e\":10}", new Cell(10));
            DeserializeTest("{\"E\":10}", new Cell(10));

            DeserializeTest($"\"[{int.MinValue}]\"", Cell.Invalid);
            DeserializeTest($"[{int.MinValue}]", Cell.Invalid);
            DeserializeTest($"{{\"e\":{int.MinValue}}}", Cell.Invalid);
            DeserializeTest($"{{\"E\":{int.MinValue}}}", Cell.Invalid);
        }
        [Test]
        public void Cell_Roundtrip()
        {
            RoundtripTest(new Cell(1, 2, 3, -1));
            RoundtripTest(new Cell(10));
            RoundtripTest(Cell.Invalid);
        }

        #endregion

        #region Circle[23][df]

        [Test] public void Circle2d_Roundtrip() => RoundtripTest(new Circle2d(new V2d(1.2, -3.4), 5.6));
        [Test] public void Circle2f_Roundtrip() => RoundtripTest(new Circle2f(new V2f(1.2f, -3.4f), 5.6f));
        [Test] public void Circle3d_Roundtrip() => RoundtripTest(new Circle3d(new V3d(1.2, -3.4, 5.6), new V3d(1,2,3).Normalized, 5.6));
        [Test] public void Circle3f_Roundtrip() => RoundtripTest(new Circle3f(new V3f(1.2, -3.4, 5.6), new V3f(1, 2, 3).Normalized, 5.6f));

        #endregion

        #region Cone3[df]

        [Test] public void Cone3d_Roundtrip() => RoundtripTest(new Cone3d(new V3d(1, -2, 3), new V3d(-4, 5, -6), 7.14));
        [Test] public void Cone3f_Roundtrip() => RoundtripTest(new Cone3f(new V3f(1, -2, 3), new V3f(-4, 5, -6), 7.14f));

        #endregion

        #region Cylinder3[df]

        [Test] public void Cylinder3d_Roundtrip() => RoundtripTest(new Cylinder3d(new V3d(1, -2, 3), new V3d(-4, 5, -6), 7.14));
        [Test] public void Cylinder3f_Roundtrip() => RoundtripTest(new Cylinder3f(new V3f(1, -2, 3), new V3f(-4, 5, -6), 7.14f));

        #endregion

        #region Ellipse[23][df]

        //[Test] public void Ellipse3d_Roundtrip() => RoundtripTest(new Ellipse3d(new V3d(1.2, -2.3, 3.4), new V3d(-4.5, 5.6, -6.7), new V3d(7.8, -8.9, 9.10), new V3d(-10.11, 11.12, -12.13)));
        //[Test] public void Ellipse3f_Roundtrip() => RoundtripTest(new Ellipse3f(new V3f(1.2, -2.3, 3.4), new V3f(-4.5, 5.6, -6.7), new V3f(7.8, -8.9, 9.10), new V3f(-10.11, 11.12, -12.13)));

        #endregion

        #region Euclidean[23][df]

        [Test] public void Euclidean2d_Roundtrip() => RoundtripTest(new Euclidean2d(Rot2d.FromDegrees(42.17)));
        [Test] public void Euclidean2f_Roundtrip() => RoundtripTest(new Euclidean2f(Rot2f.FromDegrees(42.17f)));
        [Test] public void Euclidean3d_Roundtrip() => RoundtripTest(new Euclidean3d(Rot3d.FromAngleAxis(new V3d(1.2, -2.3, 3.4))));
        [Test] public void Euclidean3f_Roundtrip() => RoundtripTest(new Euclidean3f(Rot3f.FromAngleAxis(new V3f(1.2, -2.3, 3.4))));

        #endregion

        #region Fraction

        //[Test] public void Fraction_Roundtrip() => RoundtripTest(new Fraction(-123, 456789));

        #endregion

        #region Hull[23][df]

        [Test] public void Hull2d_Roundtrip() => RoundtripTest(new Hull2d(Box2d.Unit));
        [Test] public void Hull2f_Roundtrip() => RoundtripTest(new Hull2f(Box2f.Unit));

        [Test] public void Hull3d_Roundtrip() => RoundtripTest(new Hull3d(Box3d.Unit));
        [Test] public void Hull3f_Roundtrip() => RoundtripTest(new Hull3f(Box3f.Unit));

        #endregion

        #region Line[23][df]

        [Test] public void Line2d_Roundtrip() => RoundtripTest(new Line2d(new V2d(1.2, -2.3), new V2d(-4.5, 5.6)));
        [Test] public void Line2f_Roundtrip() => RoundtripTest(new Line2f(new V2f(1.2, -2.3), new V2f(-4.5, 5.6)));
        [Test] public void Line3d_Roundtrip() => RoundtripTest(new Line3d(new V3d(1.2, -2.3, 3.4), new V3d(-4.5, 5.6, -6.7)));
        [Test] public void Line3f_Roundtrip() => RoundtripTest(new Line3f(new V3f(1.2, -2.3, 3.4), new V3f(-4.5, 5.6, -6.7)));

        #endregion

        #region M[22|23|33|34|44][dfil]

        [Test] public void M22d_Roundtrip() => RoundtripTest(new M22d(1.2, -2.3, 3.4, -4.5));
        [Test] public void M22f_Roundtrip() => RoundtripTest(new M22f(1.2f, -2.3f, 3.4f, -4.5f));
        [Test] public void M22i_Roundtrip() => RoundtripTest(new M22i(1, -2, 3, -4));
        [Test] public void M22l_Roundtrip() => RoundtripTest(new M22l(1, -2, 3, -4));

        [Test] public void M23d_Roundtrip() => RoundtripTest(new M23d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7));
        [Test] public void M23f_Roundtrip() => RoundtripTest(new M23f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f));
        [Test] public void M23i_Roundtrip() => RoundtripTest(new M23i(1, -2, 3, -4, 5, -6));
        [Test] public void M23l_Roundtrip() => RoundtripTest(new M23l(1, -2, 3, -4, 6, -6));

        [Test] public void M33d_Roundtrip() => RoundtripTest(new M33d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7, 7.8, -8.9, 9.10));
        [Test] public void M33f_Roundtrip() => RoundtripTest(new M33f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f, 7.8f, -8.9f, 9.10f));
        [Test] public void M33i_Roundtrip() => RoundtripTest(new M33i(1, -2, 3, -4, 5, -6, 7, -8, 9));
        [Test] public void M33l_Roundtrip() => RoundtripTest(new M33l(1, -2, 3, -4, 6, -6, 7, -8, 9));

        [Test] public void M34d_Roundtrip() => RoundtripTest(new M34d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7, 7.8, -8.9, 9.10, -10.11, 11.12, -12.13));
        [Test] public void M34f_Roundtrip() => RoundtripTest(new M34f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f, 7.8f, -8.9f, 9.10f, -10.11f, 11.12f, -12.13f));
        [Test] public void M34i_Roundtrip() => RoundtripTest(new M34i(1, -2, 3, -4, 5, -6, 7, -8, 9, -10, 11, -12));
        [Test] public void M34l_Roundtrip() => RoundtripTest(new M34l(1, -2, 3, -4, 6, -6, 7, -8, 9, -10, 11, -12));

        [Test] public void M44d_Roundtrip() => RoundtripTest(new M44d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7, 7.8, -8.9, 9.10, -10.11, 11.12, -12.13, 13.14, -14.15, 15.16, -16.17));
        [Test] public void M44f_Roundtrip() => RoundtripTest(new M44f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f, 7.8f, -8.9f, 9.10f, -10.11f, 11.12f, -12.13f, 13.14f, -14.15f, 15.16f, -16.17f));
        [Test] public void M44i_Roundtrip() => RoundtripTest(new M44i(1, -2, 3, -4, 5, -6, 7, -8, 9, -10, 11, -12, 13, -14, 15, -16));
        [Test] public void M44l_Roundtrip() => RoundtripTest(new M44l(1, -2, 3, -4, 6, -6, 7, -8, 9, -10, 11, -12, 13, -14, 15, -16));

        #endregion

        #region OrientedBox[23][df]

        //[Test] public void OrientedBox2d_Roundtrip() => RoundtripTest(new OrientedBox2d(Box2d.Unit));
        //[Test] public void OrientedBox2f_Roundtrip() => RoundtripTest(new OrientedBox2f(Box2f.Unit));
        //[Test] public void OrientedBox3d_Roundtrip() => RoundtripTest(new OrientedBox3d(Box3d.Unit));
        //[Test] public void OrientedBox3f_Roundtrip() => RoundtripTest(new OrientedBox3f(Box3f.Unit));

        #endregion

        #region Plane[23][df]

        [Test] public void Plane2d_Roundtrip() => RoundtripTest(new Plane2d(new V2d(1.2, -3.4).Normalized, new V2d(-5.6, 7.8)));
        [Test] public void Plane2f_Roundtrip() => RoundtripTest(new Plane2f(new V2f(1.2, -3.4).Normalized, new V2f(-5.6, 7.8)));
        [Test] public void Plane3d_Roundtrip() => RoundtripTest(new Plane3d(new V3d(1.2, -3.4, 5.6).Normalized, new V3d(-7.8, 9.10, -11.12)));
        [Test] public void Plane3f_Roundtrip() => RoundtripTest(new Plane3f(new V3f(1.2, -3.4, 5.6).Normalized, new V3f(-7.8, 9.10, -11.12)));

        #endregion

        #region Polygon[23][df]

        [Test] public void Polygon2d_Roundtrip() => RoundtripTest(new Polygon2d(new[] { new V2d(1.2, -2.3), new V2d(3.4, -4.5), new V2d(-5.6, 6.7) }));
        [Test] public void Polygon2f_Roundtrip() => RoundtripTest(new Polygon2f(new[] { new V2f(1.2, -2.3), new V2f(3.4, -4.5), new V2f(-5.6, 6.7) }));
        [Test] public void Polygon3d_Roundtrip() => RoundtripTest(new Polygon3d(new[] { new V3d(1.2, -2.3, 0.1), new V3d(3.4, -4.5, -0.2), new V3d(-5.6, 6.7, -0.3) }));
        [Test] public void Polygon3f_Roundtrip() => RoundtripTest(new Polygon3f(new[] { new V3f(1.2, -2.3, 0.1), new V3f(3.4, -4.5, -0.2), new V3f(-5.6, 6.7, -0.3) }));

        #endregion

        #region Quad[23][df]

        [Test] public void Quad2d_Roundtrip() => RoundtripTest(new Quad2d(new V2d(1.2, -2.3), new V2d(3.4, -4.5), new V2d(5.6, 6.7), new V2d(-7.8, 8.9)));
        [Test] public void Quad2f_Roundtrip() => RoundtripTest(new Quad2f(new V2f(1.2, -2.3), new V2f(3.4, -4.5), new V2f(5.6, 6.7), new V2f(-7.8, 8.9)));
        [Test] public void Quad3d_Roundtrip() => RoundtripTest(new Quad3d(new V3d(1.2, -2.3, 0.1), new V3d(3.4, -4.5, 0.1), new V3d(5.6, 6.7, 0.1), new V3d(-7.8, 8.9, 0.1)));
        [Test] public void Quad3f_Roundtrip() => RoundtripTest(new Quad3f(new V3f(1.2, -2.3, 0.1), new V3f(3.4, -4.5, 0.1), new V3f(5.6, 6.7, 0.1), new V3f(-7.8, 8.9, 0.1)));

        #endregion

        #region Quaternion[DF]

        [Test] public void QuaternionD_Roundtrip() => RoundtripTest(new QuaternionD(1.2, -2.3, 3.4, 1.0));
        [Test] public void QuaternionF_Roundtrip() => RoundtripTest(new QuaternionF(1.2f, -2.3f, 3.4f, 1.0f));

        #endregion

        #region Ray[23][df], FastRay[23][df]

        [Test] public void Ray2d_Roundtrip() => RoundtripTest(new Ray2d(new V2d(-1.2, 2.3), new V2d(3.4, -4.5).Normalized));
        [Test] public void Ray2f_Roundtrip() => RoundtripTest(new Ray2f(new V2f(-1.2, 2.3), new V2f(3.4, -4.5).Normalized));
        [Test] public void Ray3d_Roundtrip() => RoundtripTest(new Ray3d(new V3d(-1.2, 2.3, -3.4), new V3d(3.4, -4.5, 5.6).Normalized));
        [Test] public void Ray3f_Roundtrip() => RoundtripTest(new Ray3f(new V3f(-1.2, 2.3, -3.4), new V3f(3.4, -4.5, 5.6).Normalized));

        #endregion

        #region Rot[23][df]

        [Test] public void Rot2d_Roundtrip() => RoundtripTest(Rot2d.FromDegrees(123.456));
        [Test] public void Rot2f_Roundtrip() => RoundtripTest(Rot2f.FromDegrees(123.456f));
        [Test] public void Rot3d_Roundtrip() => RoundtripTest(Rot3d.FromAngleAxis(new V3d(1.2, -2.3, 3.4)));
        [Test] public void Rot3f_Roundtrip() => RoundtripTest(Rot3f.FromAngleAxis(new V3f(1.2, -2.3, 3.4)));

        #endregion

        #region Scale[23][df]

        [Test] public void Scale2d_Roundtrip() => RoundtripTest(new Scale2d(1.2, 2.3));
        [Test] public void Scale2f_Roundtrip() => RoundtripTest(new Scale2f(1.2f, 2.3f));
        [Test] public void Scale3d_Roundtrip() => RoundtripTest(new Scale3d(1.2, 2.3, 3.4));
        [Test] public void Scale3f_Roundtrip() => RoundtripTest(new Scale3f(1.2f, 2.3f, 3.4f));

        #endregion

        #region Shift[23][df]

        [Test] public void Shift2d_Roundtrip() => RoundtripTest(new Shift2d(1.2, 2.3));
        [Test] public void Shift2f_Roundtrip() => RoundtripTest(new Shift2f(1.2f, 2.3f));
        [Test] public void Shift3d_Roundtrip() => RoundtripTest(new Shift3d(1.2, 2.3, 3.4));
        [Test] public void Shift3f_Roundtrip() => RoundtripTest(new Shift3f(1.2f, 2.3f, 3.4f));

        #endregion

        #region Similarity[23][df]

        [Test] public void Similarity2d_Roundtrip() => RoundtripTest(new Similarity2d(Rot2d.FromDegrees(123.456), 1.2, 2.3));
        [Test] public void Similarity2f_Roundtrip() => RoundtripTest(new Similarity2f(Rot2f.FromDegrees(123.456f), 1.2f, 2.3f));
        [Test] public void Similarity3d_Roundtrip() => RoundtripTest(new Similarity3d(Rot3d.FromAngleAxis(new V3d(1.2, -2.3, 3.4)), 2.3, 3.4, 4.5));
        [Test] public void Similarity3f_Roundtrip() => RoundtripTest(new Similarity3f(Rot3f.FromAngleAxis(new V3f(1.2, -2.3, 3.4)), 2.3f, 3.4f, 4.5f));

        #endregion

        #region Spectrum

        //[Test] public void Spectrum_Roundtrip() => RoundtripTest(new Spectrum(300.5, 800.7, new[] { 1.2, 3.4,5.6, 7.8, 9.10 } ));

        #endregion

        #region Sphere3[df]

        [Test] public void Sphere3d_Roundtrip() => RoundtripTest(new Sphere3d(new V3d(1.2, -2.3, 3.4), 4.5));
        [Test] public void Sphere3f_Roundtrip() => RoundtripTest(new Sphere3f(new V3f(1.2, -2.3, 3.4), 4.5f));

        #endregion

        #region Torus3[df]

        [Test] public void Torus3d_Roundtrip() => RoundtripTest(new Torus3d(new V3d(1.2, -2.3, 3.4), new V3d(1.2, -2.3, 3.4).Normalized, 5.6, 4.5));
        [Test] public void Torus3f_Roundtrip() => RoundtripTest(new Torus3f(new V3f(1.2, -2.3, 3.4), new V3f(1.2, -2.3, 3.4).Normalized, 5.6f, 4.5f));

        #endregion

        #region Trafo[23][df]

        [Test] public void Trafo2d_Roundtrip() => RoundtripTest(new Trafo2d(Rot2d.FromDegrees(123.456)));
        [Test] public void Trafo2f_Roundtrip() => RoundtripTest(new Trafo2f(Rot2f.FromDegrees(123.456f)));
        [Test] public void Trafo3d_Roundtrip() => RoundtripTest(new Trafo3d(Rot3d.FromAngleAxis(new V3d(1.2, -2.3, 3.4))));
        [Test] public void Trafo3f_Roundtrip() => RoundtripTest(new Trafo3f(Rot3f.FromAngleAxis(new V3f(1.2, -2.3, 3.4))));

        #endregion

        #region Triangle[23][df]

        [Test] public void Triangle2d_Roundtrip() => RoundtripTest(new Triangle2d(new V2d(1.2, -2.3), new V2d(3.4, -4.5), new V2d(5.6, 6.7)));
        [Test] public void Triangle2f_Roundtrip() => RoundtripTest(new Triangle2f(new V2f(1.2, -2.3), new V2f(3.4, -4.5), new V2f(5.6, 6.7)));
        [Test] public void Triangle3d_Roundtrip() => RoundtripTest(new Triangle3d(new V3d(1.2, -2.3, 0.1), new V3d(3.4, -4.5, 0.1), new V3d(5.6, 6.7, 0.1)));
        [Test] public void Triangle3f_Roundtrip() => RoundtripTest(new Triangle3f(new V3f(1.2, -2.3, 0.1), new V3f(3.4, -4.5, 0.1), new V3f(5.6, 6.7, 0.1)));

        #endregion

        #region V[234][dfil]

        [Test] public void V2f_Roundtrip() => RoundtripTest(new V2f(1.1, -2.2));
        [Test] public void V2d_Roundtrip() => RoundtripTest(new V2d(1.1, -2.2));
        [Test] public void V2i_Roundtrip() => RoundtripTest(new V2i(1, -2));
        [Test] public void V2l_Roundtrip() => RoundtripTest(new V2l(1, -2));

        [Test] public void V3f_Roundtrip() => RoundtripTest(new V3f(1.1, 0, -2.2));
        [Test] public void V3d_Roundtrip() => RoundtripTest(new V3d(1.1, 0, -2.2));
        [Test] public void V3i_Roundtrip() => RoundtripTest(new V3i(1, 0, -2));
        [Test] public void V3l_Roundtrip() => RoundtripTest(new V3l(1, 0, -2));

        [Test] public void V4f_Roundtrip() => RoundtripTest(new V4f(1.1, 0, -2.2, 3.3));
        [Test] public void V4d_Roundtrip() => RoundtripTest(new V4d(1.1, 0, -2.2, 3.3));
        [Test] public void V4i_Roundtrip() => RoundtripTest(new V4i(1, 0, -2, 3));
        [Test] public void V4l_Roundtrip() => RoundtripTest(new V4l(1, 0, -2, 3));

        #endregion
    }
}
