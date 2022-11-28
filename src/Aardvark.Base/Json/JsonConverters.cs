using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aardvark.Base
{
    [JsonConverter(typeof(Converter))]
    public partial struct Cell2d
    {
        private class Converter : JsonConverter<Cell2d>
        {
            public override Cell2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); 
                    var x = reader.GetInt64();

                    reader.Read(); 
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        if (x != int.MaxValue) throw new JsonException();
                        return Invalid;
                    }
                    var y = reader.GetInt64();

                    reader.Read(); 
                    var e = reader.GetInt32();

                    reader.Read(); 
                    if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Cell2d(x, y, e);
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var x = 0L; var y = 0L; var e = 0;
                    bool hasX = false, hasY = false, hasE = false;
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read(); Debug.Assert(reader.TokenType == JsonTokenType.Number);
                        switch (p)
                        {
                            case "x": case "X": if (hasX) throw new JsonException(); x = reader.GetInt64(); hasX = true; break;
                            case "y": case "Y": if (hasY) throw new JsonException(); y = reader.GetInt64(); hasY = true; break;
                            case "e": case "E": if (hasE) throw new JsonException(); e = reader.GetInt32(); hasE = true; break;
                            default: throw new JsonException($"Invalid property {p}. Error 09514e99-055a-4f0a-8573-0e10bcb29446.");
                        }
                    }

                    if (e == int.MaxValue)
                    {
                        if (hasX || hasY) throw new JsonException();
                        return Invalid;
                    }

                    if (hasX == false || hasY == false || hasE == false)
                    {
                        throw new JsonException();
                    }

                    return new Cell2d(x, y, e);
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    var s = reader.GetString();
                    reader.Read();
                    return Parse(s);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Cell2d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Exponent);
                writer.WriteEndArray();
            }
        }

    }

    [JsonConverter(typeof(Converter))]
    public partial struct Cell
    {
        private class Converter : JsonConverter<Cell>
        {
            public override Cell Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetInt64();
                    reader.Read(); var y = reader.GetInt64();
                    reader.Read(); var z = reader.GetInt64();
                    reader.Read(); var e = reader.GetInt32();
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new Cell(x, y, z, e);
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var x = 0L; var y = 0L; var z = 0L; var e = 0;
                    bool hasX = false, hasY = false, hasZ = false, hasE = false;
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read(); Debug.Assert(reader.TokenType == JsonTokenType.Number);
                        switch (p)
                        {
                            case "x": case "X": if (hasX) throw new JsonException(); x = reader.GetInt64(); hasX = true; break;
                            case "y": case "Y": if (hasY) throw new JsonException(); y = reader.GetInt64(); hasY = true; break;
                            case "z": case "Z": if (hasZ) throw new JsonException(); z = reader.GetInt64(); hasZ = true; break;
                            case "e": case "E": if (hasE) throw new JsonException(); e = reader.GetInt32(); hasE = true; break;
                            default: throw new JsonException($"Invalid property {p}. Error 17d72803-3cee-4451-9768-81bdb8374c72.");
                        }
                    }

                    if (e == int.MaxValue)
                    {
                        if (hasX || hasY || hasZ) throw new JsonException();
                        return Invalid;
                    }

                    if (hasX == false || hasY == false || hasZ == false || hasE == false)
                    {
                        throw new JsonException();
                    }

                    return new Cell(x, y, z, e);
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    var s = reader.GetString();
                    reader.Read();
                    return Parse(s);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, Cell value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteNumberValue(value.Z);
                writer.WriteNumberValue(value.Exponent);
                writer.WriteEndArray();
            }
        }

    }

    [JsonConverter(typeof(Converter))]
    public partial struct V2i
    {
        private class Converter : JsonConverter<V2i>
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
        private class Converter : JsonConverter<V2l>
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
        private class Converter : JsonConverter<V2f>
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
        private class Converter : JsonConverter<V2d>
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
        private class Converter : JsonConverter<V3i>
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
        private class Converter : JsonConverter<V3l>
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
        private class Converter : JsonConverter<V3f>
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
        private class Converter : JsonConverter<V3d>
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
        private class Converter : JsonConverter<V4i>
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
        private class Converter : JsonConverter<V4l>
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
        private class Converter : JsonConverter<V4f>
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
        private class Converter : JsonConverter<V4d>
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
        private class Converter : JsonConverter<Box2i>
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
        private class Converter : JsonConverter<Box2l>
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
        private class Converter : JsonConverter<Box2f>
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
        private class Converter : JsonConverter<Box2d>
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
        private class Converter : JsonConverter<Box3i>
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
        private class Converter : JsonConverter<Box3l>
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
        private class Converter : JsonConverter<Box3f>
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
        private class Converter : JsonConverter<Box3d>
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
}