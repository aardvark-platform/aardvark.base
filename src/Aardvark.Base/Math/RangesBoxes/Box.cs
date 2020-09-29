using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aardvark.Base
{
    #region Box3dAndFlags

    [DataContract]
    public struct Box3dAndFlags
    {
        [DataMember]
        public Box.Flags BFlags;
        [DataMember]
        public Box3d BBox;

        public Box3dAndFlags(Box3d union, Box3d box0, Box3d box1)
        {
            BFlags = 0;
            BBox = union;
            if (box0.Min.X > union.Min.X) { BBox.Min.X = box0.Min.X; BFlags |= Box.Flags.MinX0; }
            if (box0.Min.Y > union.Min.Y) { BBox.Min.Y = box0.Min.Y; BFlags |= Box.Flags.MinY0; }
            if (box0.Min.Z > union.Min.Z) { BBox.Min.Z = box0.Min.Z; BFlags |= Box.Flags.MinZ0; }
            if (box0.Max.X < union.Max.X) { BBox.Max.X = box0.Max.X; BFlags |= Box.Flags.MaxX0; }
            if (box0.Max.Y < union.Max.Y) { BBox.Max.Y = box0.Max.Y; BFlags |= Box.Flags.MaxY0; }
            if (box0.Max.Z < union.Max.Z) { BBox.Max.Z = box0.Max.Z; BFlags |= Box.Flags.MaxZ0; }
            if (box1.Min.X > union.Min.X) { BBox.Min.X = box1.Min.X; BFlags |= Box.Flags.MinX1; }
            if (box1.Min.Y > union.Min.Y) { BBox.Min.Y = box1.Min.Y; BFlags |= Box.Flags.MinY1; }
            if (box1.Min.Z > union.Min.Z) { BBox.Min.Z = box1.Min.Z; BFlags |= Box.Flags.MinZ1; }
            if (box1.Max.X < union.Max.X) { BBox.Max.X = box1.Max.X; BFlags |= Box.Flags.MaxX1; }
            if (box1.Max.Y < union.Max.Y) { BBox.Max.Y = box1.Max.Y; BFlags |= Box.Flags.MaxY1; }
            if (box1.Max.Z < union.Max.Z) { BBox.Max.Z = box1.Max.Z; BFlags |= Box.Flags.MaxZ1; }
        }
    }

    #endregion

    #region OctoBox2d

    [DataContract]
    public struct OctoBox2d
    {
        [DataMember]
        public double PX, PY, NX, NY, PXPY, PXNY, NXPY, NXNY;

        #region Constructors

        public OctoBox2d(
                double px, double py, double nx, double ny,
                double pxpy, double pxny, double nxpy, double nxny)
        {
            PX = px; PY = py; NX = nx; NY = ny;
            PXPY = pxpy; PXNY = pxny; NXPY = nxpy; NXNY = nxny;
        }

        #endregion

        #region Constants

        public static readonly OctoBox2d Invalid =
                new OctoBox2d(double.MinValue, double.MinValue, double.MinValue, double.MinValue,
                              double.MinValue, double.MinValue, double.MinValue, double.MinValue);

        public static readonly OctoBox2d Zero =
                new OctoBox2d(0, 0, 0, 0, 0, 0, 0, 0);

        #endregion

        #region Properties

        public double Area
        {
            get
            {
                return (PX + NX) * (PY + NY)
                        - 0.5 * (Fun.Square(PX + PY - PXPY) + Fun.Square(PX + NY - PXNY)
                                 + Fun.Square(NX + PY - NXPY) + Fun.Square(NX + NY - NXNY));
            }
        }

        #endregion

        #region Manipulation

        public void ExtendBy(V2d p)
        {
            ExtendBy(p.X, p.Y);
        }

        public void ExtendBy(double x, double y)
        {
            if (x > PX) PX = x;
            if (y > PY) PY = y;
            var nx = -x; if (nx > NX) NX = nx;
            var ny = -y; if (ny > NY) NY = ny;
            var pxpy = x + y; if (pxpy > PXPY) PXPY = pxpy;
            var pxny = x - y; if (pxny > PXNY) PXNY = pxny;
            var nxpy = -x + y; if (nxpy > NXPY) NXPY = nxpy;
            var nxny = -x - y; if (nxny > NXNY) NXNY = nxny;
        }

        #endregion
    }

    #endregion

    #region json serialization (System.Text.Json)

    [JsonConverter(typeof(Converter))]
    public partial struct V2i
    {
        public class Converter : JsonConverter<V2i>
        {
            public override V2i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetInt32();
                    reader.Read(); var y = reader.GetInt32();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V2i(x, y);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V2i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V2l
    {
        public class Converter : JsonConverter<V2l>
        {
            public override V2l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetInt64();
                    reader.Read(); var y = reader.GetInt64();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V2l(x, y);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V2l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V2f
    {
        public class Converter : JsonConverter<V2f>
        {
            public override V2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetSingle();
                    reader.Read(); var y = reader.GetSingle();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V2f(x, y);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V2f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V2d
    {
        public class Converter : JsonConverter<V2d>
        {
            public override V2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetDouble();
                    reader.Read(); var y = reader.GetDouble();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V2d(x, y);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V2d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V3i
    {
        public class Converter : JsonConverter<V3i>
        {
            public override V3i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetInt32();
                    reader.Read(); var y = reader.GetInt32();
                    reader.Read(); var z = reader.GetInt32();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V3i(x, y, z);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V3i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V3l
    {
        public class Converter : JsonConverter<V3l>
        {
            public override V3l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetInt64();
                    reader.Read(); var y = reader.GetInt64();
                    reader.Read(); var z = reader.GetInt64();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V3l(x, y, z);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V3l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V3f
    {
        public class Converter : JsonConverter<V3f>
        {
            public override V3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetSingle();
                    reader.Read(); var y = reader.GetSingle();
                    reader.Read(); var z = reader.GetSingle();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V3f(x, y, z);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V3f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V3d
    {
        public class Converter : JsonConverter<V3d>
        {
            public override V3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetDouble();
                    reader.Read(); var y = reader.GetDouble();
                    reader.Read(); var z = reader.GetDouble();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V3d(x, y, z);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V3d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V4i
    {
        public class Converter : JsonConverter<V4i>
        {
            public override V4i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetInt32();
                    reader.Read(); var y = reader.GetInt32();
                    reader.Read(); var z = reader.GetInt32();
                    reader.Read(); var w = reader.GetInt32();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V4i(x, y, z, w);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V4i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteNumberValue(value.W);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V4l
    {
        public class Converter : JsonConverter<V4l>
        {
            public override V4l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetInt64();
                    reader.Read(); var y = reader.GetInt64();
                    reader.Read(); var z = reader.GetInt64();
                    reader.Read(); var w = reader.GetInt64();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V4l(x, y, z, w);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V4l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteNumberValue(value.W);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V4f
    {
        public class Converter : JsonConverter<V4f>
        {
            public override V4f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetSingle();
                    reader.Read(); var y = reader.GetSingle();
                    reader.Read(); var z = reader.GetSingle();
                    reader.Read(); var w = reader.GetSingle();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V4f(x, y, z, w);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V4f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteNumberValue(value.W);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct V4d
    {
        public class Converter : JsonConverter<V4d>
        {
            public override V4d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetDouble();
                    reader.Read(); var y = reader.GetDouble();
                    reader.Read(); var z = reader.GetDouble();
                    reader.Read(); var w = reader.GetDouble();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new V4d(x, y, z, w);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, V4d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteNumberValue(value.W);
                writer.WriteEndArray();
            }
        }
    }





    [JsonConverter(typeof(Converter))]
    public partial struct Box2i
    {
        public class Converter : JsonConverter<Box2i>
        {
            public override Box2i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V2i();
                    reader.Read(); min.X = reader.GetInt32();
                    reader.Read(); min.Y = reader.GetInt32();
                    var max = new V2i();
                    reader.Read(); max.X = reader.GetInt32();
                    reader.Read(); max.Y = reader.GetInt32();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box2i(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box2i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Box2l
    {
        public class Converter : JsonConverter<Box2l>
        {
            public override Box2l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V2l();
                    reader.Read(); min.X = reader.GetInt64();
                    reader.Read(); min.Y = reader.GetInt64();
                    var max = new V2l();
                    reader.Read(); max.X = reader.GetInt64();
                    reader.Read(); max.Y = reader.GetInt64();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box2l(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box2l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Box2f
    {
        public class Converter : JsonConverter<Box2f>
        {
            public override Box2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V2f();
                    reader.Read(); min.X = reader.GetSingle();
                    reader.Read(); min.Y = reader.GetSingle();
                    var max = new V2f();
                    reader.Read(); max.X = reader.GetSingle();
                    reader.Read(); max.Y = reader.GetSingle();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box2f(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box2f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Box2d
    {
        public class Converter : JsonConverter<Box2d>
        {
            public override Box2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V2d();
                    reader.Read(); min.X = reader.GetDouble();
                    reader.Read(); min.Y = reader.GetDouble();
                    var max = new V2d();
                    reader.Read(); max.X = reader.GetDouble();
                    reader.Read(); max.Y = reader.GetDouble();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box2d(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box2d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Box3i
    {
        public class Converter : JsonConverter<Box3i>
        {
            public override Box3i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V3i();
                    reader.Read(); min.X = reader.GetInt32();
                    reader.Read(); min.Y = reader.GetInt32();
                    reader.Read(); min.Z = reader.GetInt32();
                    var max = new V3i();
                    reader.Read(); max.X = reader.GetInt32();
                    reader.Read(); max.Y = reader.GetInt32();
                    reader.Read(); max.Z = reader.GetInt32();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box3i(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box3i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Min.Z);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteNumberValue(value.Max.Z);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Box3l
    {
        public class Converter : JsonConverter<Box3l>
        {
            public override Box3l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V3l();
                    reader.Read(); min.X = reader.GetInt64();
                    reader.Read(); min.Y = reader.GetInt64();
                    reader.Read(); min.Z = reader.GetInt64();
                    var max = new V3l();
                    reader.Read(); max.X = reader.GetInt64();
                    reader.Read(); max.Y = reader.GetInt64();
                    reader.Read(); max.Z = reader.GetInt64();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box3l(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box3l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Min.Z);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteNumberValue(value.Max.Z);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Box3f
    {
        public class Converter : JsonConverter<Box3f>
        {
            public override Box3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V3f();
                    reader.Read(); min.X = reader.GetSingle();
                    reader.Read(); min.Y = reader.GetSingle();
                    reader.Read(); min.Z = reader.GetSingle();
                    var max = new V3f();
                    reader.Read(); max.X = reader.GetSingle();
                    reader.Read(); max.Y = reader.GetSingle();
                    reader.Read(); max.Z = reader.GetSingle();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box3f(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box3f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Min.Z);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteNumberValue(value.Max.Z);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Box3d
    {
        public class Converter : JsonConverter<Box3d>
        {
            public override Box3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V3d();
                    reader.Read(); min.X = reader.GetDouble();
                    reader.Read(); min.Y = reader.GetDouble();
                    reader.Read(); min.Z = reader.GetDouble();
                    var max = new V3d();
                    reader.Read(); max.X = reader.GetDouble();
                    reader.Read(); max.Y = reader.GetDouble();
                    reader.Read(); max.Z = reader.GetDouble();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Box3d(min, max);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Box3d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.Min.X);
                writer.WriteNumberValue(value.Min.Y);
                writer.WriteNumberValue(value.Min.Z);
                writer.WriteNumberValue(value.Max.X);
                writer.WriteNumberValue(value.Max.Y);
                writer.WriteNumberValue(value.Max.Z);
                writer.WriteEndArray();
            }
        }
    }

    #endregion
}