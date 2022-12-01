using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aardvark.Base
{
    public static class JsonConverterExtensions
    {
        private const string FLOAT_NEGATIVE_INFINITY = "-Infinity";
        private const string FLOAT_POSITIVE_INFINITY = "Infinity";
        private const string FLOAT_NAN = "NaN";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloatValue(this Utf8JsonWriter writer, double x)
        {
            if (double.IsNegativeInfinity(x)) writer.WriteStringValue(FLOAT_NEGATIVE_INFINITY);
            else if (double.IsPositiveInfinity(x)) writer.WriteStringValue(FLOAT_POSITIVE_INFINITY);
            else if (double.IsNaN(x)) writer.WriteStringValue(FLOAT_NAN);
            else writer.WriteNumberValue(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloatValue(this Utf8JsonWriter writer, float x)
        {
            if (float.IsNegativeInfinity(x)) writer.WriteStringValue(FLOAT_NEGATIVE_INFINITY);
            else if (float.IsPositiveInfinity(x)) writer.WriteStringValue(FLOAT_POSITIVE_INFINITY);
            else if (float.IsNaN(x)) writer.WriteStringValue(FLOAT_NAN);
            else writer.WriteNumberValue(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloat(this Utf8JsonWriter writer, string propertyName, double x)
        {
            if (double.IsNegativeInfinity(x)) writer.WriteString(propertyName, FLOAT_NEGATIVE_INFINITY);
            else if (double.IsPositiveInfinity(x)) writer.WriteString(propertyName, FLOAT_POSITIVE_INFINITY);
            else if (double.IsNaN(x)) writer.WriteString(propertyName, FLOAT_NAN);
            else writer.WriteNumber(propertyName, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloat(this Utf8JsonWriter writer, string propertyName, float x)
        {
            if (float.IsNegativeInfinity(x)) writer.WriteString(propertyName, FLOAT_NEGATIVE_INFINITY);
            else if (float.IsPositiveInfinity(x)) writer.WriteString(propertyName, FLOAT_POSITIVE_INFINITY);
            else if (float.IsNaN(x)) writer.WriteString(propertyName, FLOAT_NAN);
            else writer.WriteNumber(propertyName, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetDoubleExtended(this ref Utf8JsonReader reader)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDouble();
            }
            else
            {
                return reader.GetString() switch
                {
                    FLOAT_NEGATIVE_INFINITY => double.NegativeInfinity,
                    FLOAT_POSITIVE_INFINITY => double.PositiveInfinity,
                    FLOAT_NAN => double.NaN,
                    _ => throw new Exception($"Invalid floating point value {reader.GetString()}. Error 2b204bf5-8b7e-471b-bdcf-beda7281251f.")
                };
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSingleExtended(this ref Utf8JsonReader reader)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetSingle();
            }
            else
            {
                return reader.GetString() switch
                {
                    FLOAT_NEGATIVE_INFINITY => float.NegativeInfinity,
                    FLOAT_POSITIVE_INFINITY => float.PositiveInfinity,
                    FLOAT_NAN => float.NaN,
                    _ => throw new Exception($"Invalid floating point value {reader.GetString()}. Error 403b381c-ef85-4a49-8aef-fd0cac8f0735.")
                };
            }
        }

        #region Colors

        public static void W(this Utf8JsonWriter writer, in C3b value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C3b value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetByte();
                reader.Read(); value.G = reader.GetByte();
                reader.Read(); value.B = reader.GetByte();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C3d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.R);
            writer.WriteFloatValue(value.G);
            writer.WriteFloatValue(value.B);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C3d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetDoubleExtended();
                reader.Read(); value.G = reader.GetDoubleExtended();
                reader.Read(); value.B = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C3f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.R);
            writer.WriteFloatValue(value.G);
            writer.WriteFloatValue(value.B);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C3f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetSingleExtended();
                reader.Read(); value.G = reader.GetSingleExtended();
                reader.Read(); value.B = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C3ui value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C3ui value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetUInt32();
                reader.Read(); value.G = reader.GetUInt32();
                reader.Read(); value.B = reader.GetUInt32();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C3us value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C3us value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetUInt16();
                reader.Read(); value.G = reader.GetUInt16();
                reader.Read(); value.B = reader.GetUInt16();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in C4b value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteNumberValue(value.A);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C4b value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetByte();
                reader.Read(); value.G = reader.GetByte();
                reader.Read(); value.B = reader.GetByte();
                reader.Read(); value.A = reader.GetByte();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C4d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.R);
            writer.WriteFloatValue(value.G);
            writer.WriteFloatValue(value.B);
            writer.WriteFloatValue(value.A);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C4d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetDoubleExtended();
                reader.Read(); value.G = reader.GetDoubleExtended();
                reader.Read(); value.B = reader.GetDoubleExtended();
                reader.Read(); value.A = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C4f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.R);
            writer.WriteFloatValue(value.G);
            writer.WriteFloatValue(value.B);
            writer.WriteFloatValue(value.A);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C4f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetSingleExtended();
                reader.Read(); value.G = reader.GetSingleExtended();
                reader.Read(); value.B = reader.GetSingleExtended();
                reader.Read(); value.A = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C4ui value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteNumberValue(value.A);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C4ui value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetUInt32();
                reader.Read(); value.G = reader.GetUInt32();
                reader.Read(); value.B = reader.GetUInt32();
                reader.Read(); value.A = reader.GetUInt32();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in C4us value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteNumberValue(value.A);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C4us value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetUInt16();
                reader.Read(); value.G = reader.GetUInt16();
                reader.Read(); value.B = reader.GetUInt16();
                reader.Read(); value.A = reader.GetUInt16();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in CieLabf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.L);
            writer.WriteFloatValue(value.a);
            writer.WriteFloatValue(value.b);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CieLabf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.L = reader.GetSingleExtended();
                reader.Read(); value.a = reader.GetSingleExtended();
                reader.Read(); value.b = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in CIeLuvf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.L);
            writer.WriteFloatValue(value.u);
            writer.WriteFloatValue(value.v);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CIeLuvf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.L = reader.GetSingleExtended();
                reader.Read(); value.u = reader.GetSingleExtended();
                reader.Read(); value.v = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in CieXYZf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CieXYZf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetSingleExtended();
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); value.Z = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in CieYxyf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.x);
            writer.WriteFloatValue(value.y);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CieYxyf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); value.x = reader.GetSingleExtended();
                reader.Read(); value.y = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in CMYKf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.C);
            writer.WriteFloatValue(value.M);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.K);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CMYKf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.C = reader.GetSingleExtended();
                reader.Read(); value.M = reader.GetSingleExtended();
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); value.K = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in HSLf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.H);
            writer.WriteFloatValue(value.S);
            writer.WriteFloatValue(value.L);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out HSLf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.H = reader.GetSingleExtended();
                reader.Read(); value.S = reader.GetSingleExtended();
                reader.Read(); value.L = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in HSVf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.H);
            writer.WriteFloatValue(value.S);
            writer.WriteFloatValue(value.V);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out HSVf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.H = reader.GetSingleExtended();
                reader.Read(); value.S = reader.GetSingleExtended();
                reader.Read(); value.V = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Yuvf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.u);
            writer.WriteFloatValue(value.v);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Yuvf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); value.u = reader.GetSingleExtended();
                reader.Read(); value.v = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Euclidean

        public static void W(this Utf8JsonWriter writer, in Euclidean2d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Rot"); writer.W(value.Rot, options);
            writer.WritePropertyName("Trans"); writer.W(value.Trans, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Euclidean2d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "rot": case "Rot": reader.R(out result.Rot, options); break;
                        case "trans": case "Trans": reader.R(out result.Trans, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Euclidean2f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Rot"); writer.W(value.Rot, options);
            writer.WritePropertyName("Trans"); writer.W(value.Trans, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Euclidean2f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "rot": case "Rot": reader.R(out result.Rot, options); break;
                        case "trans": case "Trans": reader.R(out result.Trans, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Euclidean3d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Rot"); writer.W(value.Rot, options);
            writer.WritePropertyName("Trans"); writer.W(value.Trans, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Euclidean3d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "rot": case "Rot": reader.R(out result.Rot, options); break;
                        case "trans": case "Trans": reader.R(out result.Trans, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Euclidean3f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Rot"); writer.W(value.Rot, options);
            writer.WritePropertyName("Trans"); writer.W(value.Trans, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Euclidean3f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "rot": case "Rot": reader.R(out result.Rot, options); break;
                        case "trans": case "Trans": reader.R(out result.Trans, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Lines

        public static void W(this Utf8JsonWriter writer, in Line2d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("P0"); writer.W(value.P0, options);
            writer.WritePropertyName("P1"); writer.W(value.P1, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Line2d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "p0": case "P0": reader.R(out result.P0, options); break;
                        case "p1": case "P1": reader.R(out result.P1, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Line2f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("P0"); writer.W(value.P0, options);
            writer.WritePropertyName("P1"); writer.W(value.P1, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Line2f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "p0": case "P0": reader.R(out result.P0, options); break;
                        case "p1": case "P1": reader.R(out result.P1, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Line3d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("P0"); writer.W(value.P0, options);
            writer.WritePropertyName("P1"); writer.W(value.P1, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Line3d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "p0": case "P0": reader.R(out result.P0, options); break;
                        case "p1": case "P1": reader.R(out result.P1, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Line3f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("P0"); writer.W(value.P0, options);
            writer.WritePropertyName("P1"); writer.W(value.P1, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Line3f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "p0": case "P0": reader.R(out result.P0, options); break;
                        case "p1": case "P1": reader.R(out result.P1, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Matrices

        public static void W(this Utf8JsonWriter writer, in M22d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out M22d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new double[4];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetDoubleExtended();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M22d(xs);
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in M22f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out M22f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new float[4];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetSingleExtended();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M22f(xs);
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in M33d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out M33d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new double[9];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetDoubleExtended();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M33d(xs);
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in M33f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out M33f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new float[9];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetSingleExtended();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M33f(xs);
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in M44d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out M44d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new double[16];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetDoubleExtended();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M44d(xs);
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in M44f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out M44f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new float[16];
                for (var i = 0; i < xs.Length; i++)
                {
                    reader.Read();
                    xs[i] = reader.GetSingleExtended();
                }
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                value = new M44f(xs);
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Planes

        public static void W(this Utf8JsonWriter writer, in Plane2d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Normal");
            writer.W(value.Normal, options);
            writer.WriteFloat("Distance", value.Distance);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane2d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "normal": case "Normal": reader.R(out result.Normal, options); break;
                        case "distance": case "Distance": result.Distance = reader.GetDoubleExtended(); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Plane2f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Normal");
            writer.W(value.Normal, options);
            writer.WriteFloat("Distance", value.Distance);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane2f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "normal": case "Normal": reader.R(out result.Normal, options); break;
                        case "distance": case "Distance": result.Distance = reader.GetSingleExtended(); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Plane3d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Normal");
            writer.W(value.Normal, options);
            writer.WriteFloat("Distance", value.Distance);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane3d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "normal": case "Normal": reader.R(out result.Normal, options); break;
                        case "distance": case "Distance": result.Distance = reader.GetDoubleExtended(); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Plane3f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Normal");
            writer.W(value.Normal, options);
            writer.WriteFloat("Distance", value.Distance);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane3f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "normal": case "Normal": reader.R(out result.Normal, options); break;
                        case "distance": case "Distance": result.Distance = reader.GetSingleExtended(); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, IEnumerable<Plane2d> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane2d[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<Plane2d>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    var x = default(Plane2d);
                    reader.R(ref x, options);
                    xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, IEnumerable<Plane2f> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane2f[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<Plane2f>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    var x = default(Plane2f);
                    reader.R(ref x, options);
                    xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, IEnumerable<Plane3d> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane3d[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<Plane3d>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    var x = default(Plane3d);
                    reader.R(ref x, options);
                    xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, IEnumerable<Plane3f> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref Plane3f[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<Plane3f>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    var x = default(Plane3f);
                    reader.R(ref x, options);
                    xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Vectors

        public static void W(this Utf8JsonWriter writer, in Range1b value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1b value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetByte();
                reader.Read(); value.Max = reader.GetByte();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.Min);
            writer.WriteFloatValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetDoubleExtended();
                reader.Read(); value.Max = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.Min);
            writer.WriteFloatValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetSingleExtended();
                reader.Read(); value.Max = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1i value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1i value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetInt32();
                reader.Read(); value.Max = reader.GetInt32();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1l value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1l value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetInt64();
                reader.Read(); value.Max = reader.GetInt64();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1s value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1s value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetInt16();
                reader.Read(); value.Max = reader.GetInt16();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1sb value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1sb value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetSByte();
                reader.Read(); value.Max = reader.GetSByte();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1ui value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1ui value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetUInt32();
                reader.Read(); value.Max = reader.GetUInt32();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1ul value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1ul value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetUInt64();
                reader.Read(); value.Max = reader.GetUInt64();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Range1us value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Min);
            writer.WriteNumberValue(value.Max);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Range1us value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Min = reader.GetUInt16();
                reader.Read(); value.Max = reader.GetUInt16();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Rays

        public static void W(this Utf8JsonWriter writer, in Ray2d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Origin"); writer.W(value.Origin, options);
            writer.WritePropertyName("Direction"); writer.W(value.Direction, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Ray2d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "origin": case "Origin": reader.R(out result.Origin, options); break;
                        case "direction": case "Direction": reader.R(out result.Direction, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Ray2f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Origin"); writer.W(value.Origin, options);
            writer.WritePropertyName("Direction"); writer.W(value.Direction, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Ray2f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "origin": case "Origin": reader.R(out result.Origin, options); break;
                        case "direction": case "Direction": reader.R(out result.Direction, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Ray3d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Origin"); writer.W(value.Origin, options);
            writer.WritePropertyName("Direction"); writer.W(value.Direction, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Ray3d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "origin": case "Origin": reader.R(out result.Origin, options); break;
                        case "direction": case "Direction": reader.R(out result.Direction, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Ray3f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Origin"); writer.W(value.Origin, options);
            writer.WritePropertyName("Direction"); writer.W(value.Direction, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Ray3f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "origin": case "Origin": reader.R(out result.Origin, options); break;
                        case "direction": case "Direction": reader.R(out result.Direction, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Rotations, Quaternions

        public static void W(this Utf8JsonWriter writer, in Rot2d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.Angle);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot2d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Angle = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Rot2f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.Angle);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot2f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Angle = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in Rot3d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.W);
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot3d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetDoubleExtended();
                reader.Read(); value.X = reader.GetDoubleExtended();
                reader.Read(); value.Y = reader.GetDoubleExtended();
                reader.Read(); value.Z = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Rot3f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.W);
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot3f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetSingleExtended();
                reader.Read(); value.X = reader.GetSingleExtended();
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); value.Z = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, in QuaternionD value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.W);
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out QuaternionD value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetDoubleExtended();
                reader.Read(); value.X = reader.GetDoubleExtended();
                reader.Read(); value.Y = reader.GetDoubleExtended();
                reader.Read(); value.Z = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in QuaternionF value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.W);
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out QuaternionF value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetSingleExtended();
                reader.Read(); value.X = reader.GetSingleExtended();
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); value.Z = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Similarity

        public static void W(this Utf8JsonWriter writer, in Similarity2d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteFloat("Scale", value.Scale);
            writer.WritePropertyName("Euclidean"); writer.W(value.Euclidean, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Similarity2d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "scale": case "Scale": result.Scale = reader.GetDoubleExtended(); break;
                        case "euclidean": case "Euclidean": reader.R(ref result.Euclidean, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Similarity2f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteFloat("Scale", value.Scale);
            writer.WritePropertyName("Euclidean"); writer.W(value.Euclidean, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Similarity2f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "scale": case "Scale": result.Scale = reader.GetSingleExtended(); break;
                        case "euclidean": case "Euclidean": reader.R(ref result.Euclidean, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Similarity3d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteFloat("Scale", value.Scale);
            writer.WritePropertyName("Euclidean"); writer.W(value.Euclidean, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Similarity3d result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "scale": case "Scale": result.Scale = reader.GetDoubleExtended(); break;
                        case "euclidean": case "Euclidean": reader.R(ref result.Euclidean, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in Similarity3f value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteFloat("Scale", value.Scale);
            writer.WritePropertyName("Euclidean"); writer.W(value.Euclidean, options);
            writer.WriteEndObject();
        }
        public static void R(this ref Utf8JsonReader reader, ref Similarity3f result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var p = reader.GetString();
                    reader.Read();
                    switch (p)
                    {
                        case "scale": case "Scale": result.Scale = reader.GetSingleExtended(); break;
                        case "euclidean": case "Euclidean": reader.R(ref result.Euclidean, options); break;
                        default: throw new JsonException($"Invalid property {p}.");
                    }
                }
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion

        #region Vectors

        public static void W(this Utf8JsonWriter writer, in V2d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V2d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetDoubleExtended();
                reader.Read(); value.Y = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in V2f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V2f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetSingleExtended();
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in V3d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V3d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetDoubleExtended();
                reader.Read(); value.Y = reader.GetDoubleExtended();
                reader.Read(); value.Z = reader.GetDoubleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in V3f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteFloatValue(value.X);
            writer.WriteFloatValue(value.Y);
            writer.WriteFloatValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V3f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.X = reader.GetSingleExtended();
                reader.Read(); value.Y = reader.GetSingleExtended();
                reader.Read(); value.Z = reader.GetSingleExtended();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }

        public static void W(this Utf8JsonWriter writer, IEnumerable<V2d> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref V2d[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<V2d>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    reader.R(out V2d x, options); xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, IEnumerable<V2f> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref V2f[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<V2f>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    reader.R(out V2f x, options); xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, IEnumerable<V3d> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref V3d[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<V3d>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    reader.R(out V3d x, options); xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, IEnumerable<V3f> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var x in value) writer.W(x, options);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, ref V3f[] result, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var xs = new List<V3f>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    reader.R(out V3f x, options); xs.Add(x);
                }
                result = xs.ToArray();
            }
            else
            {
                throw new JsonException();
            }
        }

        #endregion
    }

    //[JsonConverter(typeof(Converter))]
    //public partial struct _Template
    //{
    //}

    #region Affine

    [JsonConverter(typeof(Converter))]
    public partial struct Affine2d
    {
        private class Converter : JsonConverter<Affine2d>
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
                            case "linear": case "Linear": reader.R(out result.Linear, options); break;
                            case "trans": case "Trans": reader.R(out result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}.");
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
                writer.W(value.Linear, options);
                writer.WritePropertyName("Trans");
                writer.W(value.Trans, options);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Affine2f
    {
        private class Converter : JsonConverter<Affine2f>
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
                            case "linear": case "Linear": reader.R(out result.Linear, options); break;
                            case "trans": case "Trans": reader.R(out result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}.");
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
                writer.W(value.Linear, options);
                writer.WritePropertyName("Trans");
                writer.W(value.Trans, options);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Affine3d
    {
        private class Converter : JsonConverter<Affine3d>
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
                            case "linear": case "Linear": reader.R(out result.Linear, options); break;
                            case "trans": case "Trans": reader.R(out result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}.");
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
                writer.W(value.Linear, options);
                writer.WritePropertyName("Trans");
                writer.W(value.Trans, options);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Affine3f
    {
        private class Converter : JsonConverter<Affine3f>
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
                            case "linear": case "Linear": reader.R(out result.Linear, options); break;
                            case "trans": case "Trans": reader.R(out result.Trans, options); break;
                            default: throw new JsonException($"Invalid property {p}.");
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
                writer.W(value.Linear, options);
                writer.WritePropertyName("Trans");
                writer.W(value.Trans, options);
                writer.WriteEndObject();
            }
        }
    }

    #endregion

    #region Box[23][dfil]

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
                    reader.Read(); min.X = reader.GetDoubleExtended();
                    reader.Read(); min.Y = reader.GetDoubleExtended();
                    var max = new V2d();
                    reader.Read(); max.X = reader.GetDoubleExtended();
                    reader.Read(); max.Y = reader.GetDoubleExtended();
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
                writer.WriteFloatValue(value.Min.X);
                writer.WriteFloatValue(value.Min.Y);
                writer.WriteFloatValue(value.Max.X);
                writer.WriteFloatValue(value.Max.Y);
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
                    reader.Read(); min.X = reader.GetSingleExtended();
                    reader.Read(); min.Y = reader.GetSingleExtended();
                    var max = new V2f();
                    reader.Read(); max.X = reader.GetSingleExtended();
                    reader.Read(); max.Y = reader.GetSingleExtended();
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
                writer.WriteFloatValue(value.Min.X);
                writer.WriteFloatValue(value.Min.Y);
                writer.WriteFloatValue(value.Max.X);
                writer.WriteFloatValue(value.Max.Y);
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
    public partial struct Box3d
    {
        private class Converter : JsonConverter<Box3d>
        {
            public override Box3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var min = new V3d();
                    reader.Read(); min.X = reader.GetDoubleExtended();
                    reader.Read(); min.Y = reader.GetDoubleExtended();
                    reader.Read(); min.Z = reader.GetDoubleExtended();
                    var max = new V3d();
                    reader.Read(); max.X = reader.GetDoubleExtended();
                    reader.Read(); max.Y = reader.GetDoubleExtended();
                    reader.Read(); max.Z = reader.GetDoubleExtended();
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
                writer.WriteFloatValue(value.Min.X);
                writer.WriteFloatValue(value.Min.Y);
                writer.WriteFloatValue(value.Min.Z);
                writer.WriteFloatValue(value.Max.X);
                writer.WriteFloatValue(value.Max.Y);
                writer.WriteFloatValue(value.Max.Z);
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
                    reader.Read(); min.X = reader.GetSingleExtended();
                    reader.Read(); min.Y = reader.GetSingleExtended();
                    reader.Read(); min.Z = reader.GetSingleExtended();
                    var max = new V3f();
                    reader.Read(); max.X = reader.GetSingleExtended();
                    reader.Read(); max.Y = reader.GetSingleExtended();
                    reader.Read(); max.Z = reader.GetSingleExtended();
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
                writer.WriteFloatValue(value.Min.X);
                writer.WriteFloatValue(value.Min.Y);
                writer.WriteFloatValue(value.Min.Z);
                writer.WriteFloatValue(value.Max.X);
                writer.WriteFloatValue(value.Max.Y);
                writer.WriteFloatValue(value.Max.Z);
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

    #endregion

    #region Capsules

    [JsonConverter(typeof(Converter))]
    public partial struct Capsule3d
    {
        private class Converter : JsonConverter<Capsule3d>
        {
            public override Capsule3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Capsule3d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "p0": case "P0": reader.R(out result.P0, options); break;
                            case "p1": case "P1": reader.R(out result.P1, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetDoubleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Capsule3d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("P0");
                writer.W(value.P0, options);
                writer.WritePropertyName("P1");
                writer.W(value.P1, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Capsule3f
    {
        private class Converter : JsonConverter<Capsule3f>
        {
            public override Capsule3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Capsule3f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "p0": case "P0": reader.R(out result.P0, options); break;
                            case "p1": case "P1": reader.R(out result.P1, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetSingleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Capsule3f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("P0");
                writer.W(value.P0, options);
                writer.WritePropertyName("P1");
                writer.W(value.P1, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }

    }

    #endregion

    #region Cell, Cell2d

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
                        if (x == int.MinValue) return Invalid;
                        if (x < int.MinValue) throw new JsonException(
                            $"Expected exponent < int.MinValue, but found {x}. " +
                            $"Error e4fa77f3-e47b-47a2-8be7-9414f9715826."
                            );
                        return new Cell2d(exponent: (int)x);
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

                    if (e == int.MinValue)
                    {
                        if (hasX || hasY) throw new JsonException();
                        return Invalid;
                    }

                    if (hasX == false || hasY == false || hasE == false)
                    {
                        if (hasE && !hasX && !hasY)
                        {
                            return new Cell2d(e);
                        }
                        else
                        {
                            throw new JsonException("Error 41366f74-f340-4229-8a24-ed6689c8685f.");
                        }
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
                if (value.IsCenteredAtOrigin || value.IsInvalid)
                {
                    writer.WriteStartArray();
                    writer.WriteNumberValue(value.Exponent);
                    writer.WriteEndArray();
                }
                else
                {
                    writer.WriteStartArray();
                    writer.WriteNumberValue(value.X);
                    writer.WriteNumberValue(value.Y);
                    writer.WriteNumberValue(value.Exponent);
                    writer.WriteEndArray();
                }
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
                    reader.Read();
                    var x = reader.GetInt64();

                    reader.Read();
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        if (x == int.MinValue) return Invalid;
                        if (x < int.MinValue) throw new JsonException(
                            $"Expected exponent < int.MinValue, but found {x}. " +
                            $"Error c9f85be5-2856-405b-adf2-c499ad8107b6."
                            );
                        return new Cell(exponent: (int)x);
                    }
                    var y = reader.GetInt64();

                    reader.Read();
                    var z = reader.GetInt64();

                    reader.Read();
                    var e = reader.GetInt32();

                    reader.Read();
                    if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
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

                    if (e == int.MinValue)
                    {
                        if (hasX || hasY || hasZ) throw new JsonException();
                        return Invalid;
                    }

                    if (hasX == false || hasY == false || hasZ == false || hasE == false)
                    {
                        if (hasE && !hasX && !hasY && !hasZ)
                        {
                            return new Cell(e);
                        }
                        else
                        {
                            throw new JsonException("Error e4446e9c-0dd2-481c-8082-901400b231fa.");
                        }
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
                if (value.IsCenteredAtOrigin || value.IsInvalid)
                {
                    writer.WriteStartArray();
                    writer.WriteNumberValue(value.Exponent);
                    writer.WriteEndArray();
                }
                else
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
    }

    #endregion

    #region Circles

    [JsonConverter(typeof(Converter))]
    public partial struct Circle2d
    {
        private class Converter : JsonConverter<Circle2d>
        {
            public override Circle2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Circle2d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "center": case "Center": reader.R(out result.Center, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetDoubleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Circle2d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Center");
                writer.W(value.Center, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Circle2f
    {
        private class Converter : JsonConverter<Circle2f>
        {
            public override Circle2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Circle2f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "center": case "Center": reader.R(out result.Center, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetSingleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Circle2f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Center");
                writer.W(value.Center, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Circle3d
    {
        private class Converter : JsonConverter<Circle3d>
        {
            public override Circle3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Circle3d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "center": case "Center": reader.R(out result.Center, options); break;
                            case "normal": case "Normal": reader.R(out result.Normal, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetDoubleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Circle3d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Center");
                writer.W(value.Center, options);
                writer.WritePropertyName("Normal");
                writer.W(value.Normal, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Circle3f
    {
        private class Converter : JsonConverter<Circle3f>
        {
            public override Circle3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Circle3f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "center": case "Center": reader.R(out result.Center, options); break;
                            case "normal": case "Normal": reader.R(out result.Normal, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetSingleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Circle3f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Center");
                writer.W(value.Center, options);
                writer.WritePropertyName("Normal");
                writer.W(value.Normal, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    #endregion

    #region Colors

    [JsonConverter(typeof(Converter))]
    public partial struct C3b
    {
        private class Converter : JsonConverter<C3b>
        {
            public override C3b Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out C3b value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C3b value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C3d
    {
        private class Converter : JsonConverter<C3d>
        {
            public override C3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out C3d value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C3d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C3f
    {
        private class Converter : JsonConverter<C3f>
        {
            public override C3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C3f value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C3f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C3ui
    {
        private class Converter : JsonConverter<C3ui>
        {
            public override C3ui Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C3ui value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C3ui value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C3us
    {
        private class Converter : JsonConverter<C3us>
        {
            public override C3us Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C3us value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C3us value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C4b
    {
        private class Converter : JsonConverter<C4b>
        {
            public override C4b Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C4b value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C4b value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C4d
    {
        private class Converter : JsonConverter<C4d>
        {
            public override C4d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C4d value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C4d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C4f
    {
        private class Converter : JsonConverter<C4f>
        {
            public override C4f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C4f value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C4f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C4ui
    {
        private class Converter : JsonConverter<C4ui>
        {
            public override C4ui Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C4ui value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C4ui value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct C4us
    {
        private class Converter : JsonConverter<C4us>
        {
            public override C4us Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { C4us value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, C4us value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct CieLabf
    {
        private class Converter : JsonConverter<CieLabf>
        {
            public override CieLabf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { CieLabf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, CieLabf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct CIeLuvf
    {
        private class Converter : JsonConverter<CIeLuvf>
        {
            public override CIeLuvf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { CIeLuvf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, CIeLuvf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct CieXYZf
    {
        private class Converter : JsonConverter<CieXYZf>
        {
            public override CieXYZf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { CieXYZf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, CieXYZf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct CieYxyf
    {
        private class Converter : JsonConverter<CieYxyf>
        {
            public override CieYxyf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { CieYxyf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, CieYxyf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct CMYKf
    {
        private class Converter : JsonConverter<CMYKf>
        {
            public override CMYKf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { CMYKf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, CMYKf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct HSLf
    {
        private class Converter : JsonConverter<HSLf>
        {
            public override HSLf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { HSLf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, HSLf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct HSVf
    {
        private class Converter : JsonConverter<HSVf>
        {
            public override HSVf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { HSVf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, HSVf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Yuvf
    {
        private class Converter : JsonConverter<Yuvf>
        {
            public override Yuvf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Yuvf value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Yuvf value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region Cones

    [JsonConverter(typeof(Converter))]
    public partial struct Cone3d
    {
        private class Converter : JsonConverter<Cone3d>
        {
            public override Cone3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Cone3d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "origin": case "Origin": reader.R(out result.Origin, options); break;
                            case "direction": case "Direction": reader.R(out result.Direction, options); break;
                            case "angle": case "Angle": result.Angle = reader.GetDoubleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Cone3d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Origin");
                writer.W(value.Origin, options);
                writer.WritePropertyName("Direction");
                writer.W(value.Direction, options);
                writer.WriteFloat("Angle", value.Angle);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Cone3f
    {
        private class Converter : JsonConverter<Cone3f>
        {
            public override Cone3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Cone3f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "origin": case "Origin": reader.R(out result.Origin, options); break;
                            case "direction": case "Direction": reader.R(out result.Direction, options); break;
                            case "angle": case "Angle": result.Angle = reader.GetSingleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Cone3f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Origin");
                writer.W(value.Origin, options);
                writer.WritePropertyName("Direction");
                writer.W(value.Direction, options);
                writer.WriteFloat("Angle", value.Angle);
                writer.WriteEndObject();
            }
        }
    }

    #endregion

    #region Cylinder

    [JsonConverter(typeof(Converter))]
    public partial struct Cylinder3d
    {
        private class Converter : JsonConverter<Cylinder3d>
        {
            public override Cylinder3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Cylinder3d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "p0": case "P0": reader.R(out result.P0, options); break;
                            case "p1": case "P1": reader.R(out result.P1, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetDoubleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Cylinder3d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("P0");
                writer.W(value.P0, options);
                writer.WritePropertyName("P1");
                writer.W(value.P1, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Cylinder3f
    {
        private class Converter : JsonConverter<Cylinder3f>
        {
            public override Cylinder3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Cylinder3f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "p0": case "P0": reader.R(out result.P0, options); break;
                            case "p1": case "P1": reader.R(out result.P1, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetSingleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Cylinder3f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("P0");
                writer.W(value.P0, options);
                writer.WritePropertyName("P1");
                writer.W(value.P1, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    #endregion

    #region Euclideans

    [JsonConverter(typeof(Converter))]
    public partial struct Euclidean2d
    {
        private class Converter : JsonConverter<Euclidean2d>
        {
            public override Euclidean2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Euclidean2d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Euclidean2d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Euclidean2f
    {
        private class Converter : JsonConverter<Euclidean2f>
        {
            public override Euclidean2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Euclidean2f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Euclidean2f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Euclidean3d
    {
        private class Converter : JsonConverter<Euclidean3d>
        {
            public override Euclidean3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Euclidean3d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Euclidean3d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Euclidean3f
    {
        private class Converter : JsonConverter<Euclidean3f>
        {
            public override Euclidean3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Euclidean3f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Euclidean3f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region Hulls

    [JsonConverter(typeof(Converter))]
    public partial struct Hull2d
    {
        private class Converter : JsonConverter<Hull2d>
        {
            public override Hull2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Hull2d value = default; reader.R(ref value.PlaneArray, options); return value; }
            public override void Write(Utf8JsonWriter writer, Hull2d value, JsonSerializerOptions options)
                => writer.W(value.PlaneArray, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Hull2f
    {
        private class Converter : JsonConverter<Hull2f>
        {
            public override Hull2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Hull2f value = default; reader.R(ref value.PlaneArray, options); return value; }
            public override void Write(Utf8JsonWriter writer, Hull2f value, JsonSerializerOptions options)
                => writer.W(value.PlaneArray, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Hull3d
    {
        private class Converter : JsonConverter<Hull3d>
        {
            public override Hull3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Hull3d value = default; reader.R(ref value.PlaneArray, options); return value; }
            public override void Write(Utf8JsonWriter writer, Hull3d value, JsonSerializerOptions options)
                => writer.W(value.PlaneArray, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Hull3f
    {
        private class Converter : JsonConverter<Hull3f>
        {
            public override Hull3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Hull3f value = default; reader.R(ref value.PlaneArray, options); return value; }
            public override void Write(Utf8JsonWriter writer, Hull3f value, JsonSerializerOptions options)
                => writer.W(value.PlaneArray, options);
        }
    }

    #endregion

    #region Lines

    [JsonConverter(typeof(Converter))]
    public partial struct Line2d
    {
        private class Converter : JsonConverter<Line2d>
        {
            public override Line2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Line2d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Line2d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Line2f
    {
        private class Converter : JsonConverter<Line2f>
        {
            public override Line2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Line2f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Line2f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Line3d
    {
        private class Converter : JsonConverter<Line3d>
        {
            public override Line3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Line3d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Line3d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Line3f
    {
        private class Converter : JsonConverter<Line3f>
        {
            public override Line3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Line3f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Line3f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region M[22|23|33|34|44][dfil]

    [JsonConverter(typeof(Converter))]
    public partial struct M22d
    {
        private class Converter : JsonConverter<M22d>
        {
            public override M22d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new double[4];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetDoubleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M22d(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M22d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M22f
    {
        private class Converter : JsonConverter<M22f>
        {
            public override M22f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new float[4];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetSingleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M22f(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M22f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M22i
    {
        private class Converter : JsonConverter<M22i>
        {
            public override M22i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new int[4];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt32();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M22i(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M22i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M22l
    {
        private class Converter : JsonConverter<M22l>
        {
            public override M22l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new long[4];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt64();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M22l(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M22l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }


    [JsonConverter(typeof(Converter))]
    public partial struct M23d
    {
        private class Converter : JsonConverter<M23d>
        {
            public override M23d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new double[6];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetDoubleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M23d(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M23d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M23f
    {
        private class Converter : JsonConverter<M23f>
        {
            public override M23f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new float[6];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetSingleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M23f(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M23f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M23i
    {
        private class Converter : JsonConverter<M23i>
        {
            public override M23i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new int[6];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt32();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M23i(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M23i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M23l
    {
        private class Converter : JsonConverter<M23l>
        {
            public override M23l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new long[6];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt64();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M23l(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M23l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }


    [JsonConverter(typeof(Converter))]
    public partial struct M33d
    {
        private class Converter : JsonConverter<M33d>
        {
            public override M33d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new double[9];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetDoubleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M33d(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M33d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M33f
    {
        private class Converter : JsonConverter<M33f>
        {
            public override M33f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new float[9];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetSingleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M33f(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M33f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M33i
    {
        private class Converter : JsonConverter<M33i>
        {
            public override M33i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new int[9];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt32();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M33i(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M33i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M33l
    {
        private class Converter : JsonConverter<M33l>
        {
            public override M33l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new long[9];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt64();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M33l(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M33l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }


    [JsonConverter(typeof(Converter))]
    public partial struct M34d
    {
        private class Converter : JsonConverter<M34d>
        {
            public override M34d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new double[12];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetDoubleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M34d(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M34d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M34f
    {
        private class Converter : JsonConverter<M34f>
        {
            public override M34f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new float[12];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetSingleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M34f(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M34f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M34i
    {
        private class Converter : JsonConverter<M34i>
        {
            public override M34i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new int[12];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt32();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M34i(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M34i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M34l
    {
        private class Converter : JsonConverter<M34l>
        {
            public override M34l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new long[12];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt64();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M34l(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M34l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }


    [JsonConverter(typeof(Converter))]
    public partial struct M44d
    {
        private class Converter : JsonConverter<M44d>
        {
            public override M44d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new double[16];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetDoubleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M44d(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M44d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M44f
    {
        private class Converter : JsonConverter<M44f>
        {
            public override M44f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new float[16];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetSingleExtended();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M44f(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M44f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteFloatValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M44i
    {
        private class Converter : JsonConverter<M44i>
        {
            public override M44i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new int[16];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt32();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M44i(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M44i value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }
    [JsonConverter(typeof(Converter))]
    public partial struct M44l
    {
        private class Converter : JsonConverter<M44l>
        {
            public override M44l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var xs = new long[16];
                    for (var i = 0; i < xs.Length; i++)
                    {
                        reader.Read();
                        xs[i] = reader.GetInt64();
                    }
                    reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                    return new M44l(xs);
                }
                else
                {
                    throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, M44l value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var xs = value.ToArray();
                for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
                writer.WriteEndArray();
            }
        }
    }

    #endregion

    #region Planes

    [JsonConverter(typeof(Converter))]
    public partial struct Plane2d
    {
        private class Converter : JsonConverter<Plane2d>
        {
            public override Plane2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Plane2d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Plane2d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Plane2f
    {
        private class Converter : JsonConverter<Plane2f>
        {
            public override Plane2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Plane2f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Plane2f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Plane3d
    {
        private class Converter : JsonConverter<Plane3d>
        {
            public override Plane3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Plane3d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Plane3d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Plane3f
    {
        private class Converter : JsonConverter<Plane3f>
        {
            public override Plane3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Plane3f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Plane3f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region Polygons

    [JsonConverter(typeof(Converter))]
    public partial struct Polygon2d
    {
        private class Converter : JsonConverter<Polygon2d>
        {
            public override Polygon2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V2d[] value = default; reader.R(ref value, options); return new Polygon2d(value); }
            public override void Write(Utf8JsonWriter writer, Polygon2d value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Polygon2f
    {
        private class Converter : JsonConverter<Polygon2f>
        {
            public override Polygon2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V2f[] value = default; reader.R(ref value, options); return new Polygon2f(value); }
            public override void Write(Utf8JsonWriter writer, Polygon2f value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Polygon3d
    {
        private class Converter : JsonConverter<Polygon3d>
        {
            public override Polygon3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V3d[] value = default; reader.R(ref value, options); return new Polygon3d(value); }
            public override void Write(Utf8JsonWriter writer, Polygon3d value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Polygon3f
    {
        private class Converter : JsonConverter<Polygon3f>
        {
            public override Polygon3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V3f[] value = default; reader.R(ref value, options); return new Polygon3f(value); }
            public override void Write(Utf8JsonWriter writer, Polygon3f value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    #endregion

    #region Quads

    [JsonConverter(typeof(Converter))]
    public partial struct Quad2d
    {
        private class Converter : JsonConverter<Quad2d>
        {
            public override Quad2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V2d[] value = default; reader.R(ref value, options); return new Quad2d(value); }
            public override void Write(Utf8JsonWriter writer, Quad2d value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Quad2f
    {
        private class Converter : JsonConverter<Quad2f>
        {
            public override Quad2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V2f[] value = default; reader.R(ref value, options); return new Quad2f(value); }
            public override void Write(Utf8JsonWriter writer, Quad2f value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Quad3d
    {
        private class Converter : JsonConverter<Quad3d>
        {
            public override Quad3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V3d[] value = default; reader.R(ref value, options); return new Quad3d(value); }
            public override void Write(Utf8JsonWriter writer, Quad3d value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Quad3f
    {
        private class Converter : JsonConverter<Quad3f>
        {
            public override Quad3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V3f[] value = default; reader.R(ref value, options); return new Quad3f(value); }
            public override void Write(Utf8JsonWriter writer, Quad3f value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    #endregion

    #region Ranges

    [JsonConverter(typeof(Converter))]
    public partial struct Range1b
    {
        private class Converter : JsonConverter<Range1b>
        {
            public override Range1b Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1b value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1b value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1d
    {
        private class Converter : JsonConverter<Range1d>
        {
            public override Range1d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1d value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1f
    {
        private class Converter : JsonConverter<Range1f>
        {
            public override Range1f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1f value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1i
    {
        private class Converter : JsonConverter<Range1i>
        {
            public override Range1i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1i value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1i value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1l
    {
        private class Converter : JsonConverter<Range1l>
        {
            public override Range1l Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1l value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1l value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1s
    {
        private class Converter : JsonConverter<Range1s>
        {
            public override Range1s Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1s value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1s value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1sb
    {
        private class Converter : JsonConverter<Range1sb>
        {
            public override Range1sb Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1sb value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1sb value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1ui
    {
        private class Converter : JsonConverter<Range1ui>
        {
            public override Range1ui Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1ui value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1ui value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1ul
    {
        private class Converter : JsonConverter<Range1ul>
        {
            public override Range1ul Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1ul value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1ul value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Range1us
    {
        private class Converter : JsonConverter<Range1us>
        {
            public override Range1us Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Range1us value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Range1us value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region Rays

    [JsonConverter(typeof(Converter))]
    public partial struct Ray2d
    {
        private class Converter : JsonConverter<Ray2d>
        {
            public override Ray2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Ray2d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Ray2d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Ray2f
    {
        private class Converter : JsonConverter<Ray2f>
        {
            public override Ray2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Ray2f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Ray2f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Ray3d
    {
        private class Converter : JsonConverter<Ray3d>
        {
            public override Ray3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Ray3d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Ray3d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Ray3f
    {
        private class Converter : JsonConverter<Ray3f>
        {
            public override Ray3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Ray3f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Ray3f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region Rotations, Quaternions

    [JsonConverter(typeof(Converter))]
    public partial struct Rot2d
    {
        private class Converter : JsonConverter<Rot2d>
        {
            public override Rot2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Rot2d value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Rot2d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Rot2f
    {
        private class Converter : JsonConverter<Rot2f>
        {
            public override Rot2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Rot2f value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Rot2f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Rot3d
    {
        private class Converter : JsonConverter<Rot3d>
        {
            public override Rot3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Rot3d value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Rot3d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Rot3f
    {
        private class Converter : JsonConverter<Rot3f>
        {
            public override Rot3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Rot3f value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Rot3f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct QuaternionD
    {
        private class Converter : JsonConverter<QuaternionD>
        {
            public override QuaternionD Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { QuaternionD value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, QuaternionD value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct QuaternionF
    {
        private class Converter : JsonConverter<QuaternionF>
        {
            public override QuaternionF Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { QuaternionF value = default; reader.R(out value, options); return value; }
            public override void Write(Utf8JsonWriter writer, QuaternionF value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region Scales

    [JsonConverter(typeof(Converter))]
    public partial struct Scale2d
    {
        private class Converter : JsonConverter<Scale2d>
        {
            public override Scale2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V2d value, options); return new Scale2d(value); }
            public override void Write(Utf8JsonWriter writer, Scale2d value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Scale2f
    {
        private class Converter : JsonConverter<Scale2f>
        {
            public override Scale2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V2f value, options); return new Scale2f(value); }
            public override void Write(Utf8JsonWriter writer, Scale2f value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Scale3d
    {
        private class Converter : JsonConverter<Scale3d>
        {
            public override Scale3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V3d value, options); return new Scale3d(value); }
            public override void Write(Utf8JsonWriter writer, Scale3d value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Scale3f
    {
        private class Converter : JsonConverter<Scale3f>
        {
            public override Scale3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V3f value, options); return new Scale3f(value); }
            public override void Write(Utf8JsonWriter writer, Scale3f value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    #endregion

    #region Shifts

    [JsonConverter(typeof(Converter))]
    public partial struct Shift2d
    {
        private class Converter : JsonConverter<Shift2d>
        {
            public override Shift2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V2d value, options); return new Shift2d(value); }
            public override void Write(Utf8JsonWriter writer, Shift2d value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Shift2f
    {
        private class Converter : JsonConverter<Shift2f>
        {
            public override Shift2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V2f value, options); return new Shift2f(value); }
            public override void Write(Utf8JsonWriter writer, Shift2f value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Shift3d
    {
        private class Converter : JsonConverter<Shift3d>
        {
            public override Shift3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V3d value, options); return new Shift3d(value); }
            public override void Write(Utf8JsonWriter writer, Shift3d value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Shift3f
    {
        private class Converter : JsonConverter<Shift3f>
        {
            public override Shift3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { reader.R(out V3f value, options); return new Shift3f(value); }
            public override void Write(Utf8JsonWriter writer, Shift3f value, JsonSerializerOptions options)
                => writer.W(value.V, options);
        }
    }

    #endregion

    #region Similarities

    [JsonConverter(typeof(Converter))]
    public partial struct Similarity2d
    {
        private class Converter : JsonConverter<Similarity2d>
        {
            public override Similarity2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Similarity2d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Similarity2d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Similarity2f
    {
        private class Converter : JsonConverter<Similarity2f>
        {
            public override Similarity2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Similarity2f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Similarity2f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Similarity3d
    {
        private class Converter : JsonConverter<Similarity3d>
        {
            public override Similarity3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Similarity3d value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Similarity3d value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Similarity3f
    {
        private class Converter : JsonConverter<Similarity3f>
        {
            public override Similarity3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { Similarity3f value = default; reader.R(ref value, options); return value; }
            public override void Write(Utf8JsonWriter writer, Similarity3f value, JsonSerializerOptions options)
                => writer.W(value, options);
        }
    }

    #endregion

    #region Spheres

    [JsonConverter(typeof(Converter))]
    public partial struct Sphere3d
    {
        private class Converter : JsonConverter<Sphere3d>
        {
            public override Sphere3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Sphere3d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "center": case "Center": reader.R(out result.Center, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetDoubleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Sphere3d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Center");
                writer.W(value.Center, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Sphere3f
    {
        private class Converter : JsonConverter<Sphere3f>
        {
            public override Sphere3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Sphere3f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "center": case "Center": reader.R(out result.Center, options); break;
                            case "radius": case "Radius": result.Radius = reader.GetSingleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Sphere3f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Center");
                writer.W(value.Center, options);
                writer.WriteFloat("Radius", value.Radius);
                writer.WriteEndObject();
            }
        }
    }

    #endregion

    #region Tori

    [JsonConverter(typeof(Converter))]
    public partial struct Torus3d
    {
        private class Converter : JsonConverter<Torus3d>
        {
            public override Torus3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Torus3d result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "position": case "Position": reader.R(out result.Position, options); break;
                            case "direction": case "Direction": reader.R(out result.Direction, options); break;
                            case "majorRadius": case "MajorRadius": result.MajorRadius = reader.GetDoubleExtended(); break;
                            case "minorRadius": case "MinorRadius": result.MinorRadius = reader.GetDoubleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Torus3d value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Position");
                writer.W(value.Position, options);
                writer.WritePropertyName("Direction");
                writer.W(value.Direction, options);
                writer.WriteFloat("MajorRadius", value.MajorRadius);
                writer.WriteFloat("MinorRadius", value.MinorRadius);
                writer.WriteEndObject();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Torus3f
    {
        private class Converter : JsonConverter<Torus3f>
        {
            public override Torus3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    Torus3f result = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject) break;

                        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
                        var p = reader.GetString();
                        reader.Read();
                        switch (p)
                        {
                            case "position": case "Position": reader.R(out result.Position, options); break;
                            case "direction": case "Direction": reader.R(out result.Direction, options); break;
                            case "majorRadius": case "MajorRadius": result.MajorRadius = reader.GetSingleExtended(); break;
                            case "minorRadius": case "MinorRadius": result.MinorRadius = reader.GetSingleExtended(); break;
                            default: throw new JsonException($"Invalid property {p}.");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new JsonException();
                }
            }
            public override void Write(Utf8JsonWriter writer, Torus3f value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Position");
                writer.W(value.Position, options);
                writer.WritePropertyName("Direction");
                writer.W(value.Direction, options);
                writer.WriteFloat("MajorRadius", value.MajorRadius);
                writer.WriteFloat("MinorRadius", value.MinorRadius);
                writer.WriteEndObject();
            }
        }
    }

    #endregion

    #region Trafos

    [JsonConverter(typeof(Converter))]
    public partial struct Trafo2d
    {
        private class Converter : JsonConverter<Trafo2d>
        {
            public override Trafo2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                M33d forward;
                M33d backward;
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out forward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out backward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                return new Trafo2d(forward, backward);
            }
            public override void Write(Utf8JsonWriter writer, Trafo2d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.W(value.Forward, options);
                writer.W(value.Backward, options);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Trafo2f
    {
        private class Converter : JsonConverter<Trafo2f>
        {
            public override Trafo2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                M33f forward;
                M33f backward;
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out forward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out backward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                return new Trafo2f(forward, backward);
            }
            public override void Write(Utf8JsonWriter writer, Trafo2f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.W(value.Forward, options);
                writer.W(value.Backward, options);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Trafo3d
    {
        private class Converter : JsonConverter<Trafo3d>
        {
            public override Trafo3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                M44d forward;
                M44d backward;
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out forward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out backward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                return new Trafo3d(forward, backward);
            }
            public override void Write(Utf8JsonWriter writer, Trafo3d value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.W(value.Forward, options);
                writer.W(value.Backward, options);
                writer.WriteEndArray();
            }
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Trafo3f
    {
        private class Converter : JsonConverter<Trafo3f>
        {
            public override Trafo3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                M44f forward;
                M44f backward;
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out forward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.R(out backward, options);
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                return new Trafo3f(forward, backward);
            }
            public override void Write(Utf8JsonWriter writer, Trafo3f value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                writer.W(value.Forward, options);
                writer.W(value.Backward, options);
                writer.WriteEndArray();
            }
        }
    }

    #endregion

    #region Triangles

    [JsonConverter(typeof(Converter))]
    public partial struct Triangle2d
    {
        private class Converter : JsonConverter<Triangle2d>
        {
            public override Triangle2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V2d[] value = default; reader.R(ref value, options); return new Triangle2d(value); }
            public override void Write(Utf8JsonWriter writer, Triangle2d value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Triangle2f
    {
        private class Converter : JsonConverter<Triangle2f>
        {
            public override Triangle2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V2f[] value = default; reader.R(ref value, options); return new Triangle2f(value); }
            public override void Write(Utf8JsonWriter writer, Triangle2f value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Triangle3d
    {
        private class Converter : JsonConverter<Triangle3d>
        {
            public override Triangle3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V3d[] value = default; reader.R(ref value, options); return new Triangle3d(value); }
            public override void Write(Utf8JsonWriter writer, Triangle3d value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    [JsonConverter(typeof(Converter))]
    public partial struct Triangle3f
    {
        private class Converter : JsonConverter<Triangle3f>
        {
            public override Triangle3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            { V3f[] value = default; reader.R(ref value, options); return new Triangle3f(value); }
            public override void Write(Utf8JsonWriter writer, Triangle3f value, JsonSerializerOptions options)
                => writer.W(value.Points, options);
        }
    }

    #endregion

    #region V[234][dfil]

    [JsonConverter(typeof(Converter))]
    public partial struct V2d
    {
        private class Converter : JsonConverter<V2d>
        {
            public override V2d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetDoubleExtended();
                    reader.Read(); var y = reader.GetDoubleExtended();
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
                writer.WriteFloatValue(value.X);
                writer.WriteFloatValue(value.Y);
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
                    reader.Read(); var x = reader.GetSingleExtended();
                    reader.Read(); var y = reader.GetSingleExtended();
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
                writer.WriteFloatValue(value.X);
                writer.WriteFloatValue(value.Y);
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
    public partial struct V3d
    {
        private class Converter : JsonConverter<V3d>
        {
            public override V3d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetDoubleExtended();
                    reader.Read(); var y = reader.GetDoubleExtended();
                    reader.Read(); var z = reader.GetDoubleExtended();
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
                writer.WriteFloatValue(value.X);
                writer.WriteFloatValue(value.Y);
                writer.WriteFloatValue(value.Z);
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
                    reader.Read(); var x = reader.GetSingleExtended();
                    reader.Read(); var y = reader.GetSingleExtended();
                    reader.Read(); var z = reader.GetSingleExtended();
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
                writer.WriteFloatValue(value.X);
                writer.WriteFloatValue(value.Y);
                writer.WriteFloatValue(value.Z);
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
    public partial struct V4d
    {
        private class Converter : JsonConverter<V4d>
        {
            public override V4d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read(); var x = reader.GetDoubleExtended();
                    reader.Read(); var y = reader.GetDoubleExtended();
                    reader.Read(); var z = reader.GetDoubleExtended();
                    reader.Read(); var w = reader.GetDoubleExtended();
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
                writer.WriteFloatValue(value.X);
                writer.WriteFloatValue(value.Y);
                writer.WriteFloatValue(value.Z);
                writer.WriteFloatValue(value.W);
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
                    reader.Read(); var x = reader.GetSingleExtended();
                    reader.Read(); var y = reader.GetSingleExtended();
                    reader.Read(); var z = reader.GetSingleExtended();
                    reader.Read(); var w = reader.GetSingleExtended();
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
                writer.WriteFloatValue(value.X);
                writer.WriteFloatValue(value.Y);
                writer.WriteFloatValue(value.Z);
                writer.WriteFloatValue(value.W);
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

    #endregion
}