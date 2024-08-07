﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aardvark.Base.Tests.Json
{
    public static class JsonConverterExtensions
    {
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
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C3d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetDouble();
                reader.Read(); value.G = reader.GetDouble();
                reader.Read(); value.B = reader.GetDouble();
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
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C3f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetSingle();
                reader.Read(); value.G = reader.GetSingle();
                reader.Read(); value.B = reader.GetSingle();
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
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteNumberValue(value.A);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C4d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetDouble();
                reader.Read(); value.G = reader.GetDouble();
                reader.Read(); value.B = reader.GetDouble();
                reader.Read(); value.A = reader.GetDouble();
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
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteNumberValue(value.A);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out C4f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.R = reader.GetSingle();
                reader.Read(); value.G = reader.GetSingle();
                reader.Read(); value.B = reader.GetSingle();
                reader.Read(); value.A = reader.GetSingle();
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
            writer.WriteNumberValue(value.L);
            writer.WriteNumberValue(value.a);
            writer.WriteNumberValue(value.b);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CieLabf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.L = reader.GetSingle();
                reader.Read(); value.a = reader.GetSingle();
                reader.Read(); value.b = reader.GetSingle();
                reader.Read(); if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
            }
            else
            {
                throw new JsonException();
            }
        }
        public static void W(this Utf8JsonWriter writer, in CieLuvf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.L);
            writer.WriteNumberValue(value.u);
            writer.WriteNumberValue(value.v);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CieLuvf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.L = reader.GetSingle();
                reader.Read(); value.u = reader.GetSingle();
                reader.Read(); value.v = reader.GetSingle();
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
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CieXYZf value, JsonSerializerOptions options)
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
        public static void W(this Utf8JsonWriter writer, in CieYxyf value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.x);
            writer.WriteNumberValue(value.y);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CieYxyf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Y = reader.GetSingle();
                reader.Read(); value.x = reader.GetSingle();
                reader.Read(); value.y = reader.GetSingle();
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
            writer.WriteNumberValue(value.C);
            writer.WriteNumberValue(value.M);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.K);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out CMYKf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.C = reader.GetSingle();
                reader.Read(); value.M = reader.GetSingle();
                reader.Read(); value.Y = reader.GetSingle();
                reader.Read(); value.K = reader.GetSingle();
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
            writer.WriteNumberValue(value.H);
            writer.WriteNumberValue(value.S);
            writer.WriteNumberValue(value.L);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out HSLf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.H = reader.GetSingle();
                reader.Read(); value.S = reader.GetSingle();
                reader.Read(); value.L = reader.GetSingle();
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
            writer.WriteNumberValue(value.H);
            writer.WriteNumberValue(value.S);
            writer.WriteNumberValue(value.V);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out HSVf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.H = reader.GetSingle();
                reader.Read(); value.S = reader.GetSingle();
                reader.Read(); value.V = reader.GetSingle();
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
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.u);
            writer.WriteNumberValue(value.v);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Yuvf value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Y = reader.GetSingle();
                reader.Read(); value.u = reader.GetSingle();
                reader.Read(); value.v = reader.GetSingle();
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
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
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
        public static void W(this Utf8JsonWriter writer, in M22f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
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
        public static void W(this Utf8JsonWriter writer, in M33d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
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
        public static void W(this Utf8JsonWriter writer, in M33f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
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
        public static void W(this Utf8JsonWriter writer, in M44d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var xs = value.ToArray();
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
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
                    xs[i] = reader.GetDouble();
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
            for (var i = 0; i < xs.Length; i++) writer.WriteNumberValue(xs[i]);
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
                    xs[i] = reader.GetSingle();
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
            writer.WriteNumber("Distance", value.Distance);
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
                        case "distance": case "Distance": result.Distance = reader.GetDouble(); break;
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
            writer.WriteNumber("Distance", value.Distance);
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
                        case "distance": case "Distance": result.Distance = reader.GetSingle(); break;
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
            writer.WriteNumber("Distance", value.Distance);
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
                        case "distance": case "Distance": result.Distance = reader.GetDouble(); break;
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
            writer.WriteNumber("Distance", value.Distance);
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
                        case "distance": case "Distance": result.Distance = reader.GetSingle(); break;
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
            writer.WriteNumberValue(value.Angle);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot2d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Angle = reader.GetDouble();
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
            writer.WriteNumberValue(value.Angle);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot2f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.Angle = reader.GetSingle();
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
            writer.WriteNumberValue(value.W);
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot3d value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetDouble();
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
        public static void W(this Utf8JsonWriter writer, in Rot3f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.W);
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out Rot3f value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetSingle();
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

        public static void W(this Utf8JsonWriter writer, in QuaternionD value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.W);
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out QuaternionD value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetDouble();
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
        public static void W(this Utf8JsonWriter writer, in QuaternionF value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.W);
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out QuaternionF value, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read(); value.W = reader.GetSingle();
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

        #endregion

        #region Similarity

        public static void W(this Utf8JsonWriter writer, in Similarity2d value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("Scale", value.Scale);
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
                        case "scale": case "Scale": result.Scale = reader.GetDouble(); break;
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
            writer.WriteNumber("Scale", value.Scale);
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
                        case "scale": case "Scale": result.Scale = reader.GetSingle(); break;
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
            writer.WriteNumber("Scale", value.Scale);
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
                        case "scale": case "Scale": result.Scale = reader.GetDouble(); break;
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
            writer.WriteNumber("Scale", value.Scale);
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
                        case "scale": case "Scale": result.Scale = reader.GetSingle(); break;
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
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V2d value, JsonSerializerOptions options)
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
        public static void W(this Utf8JsonWriter writer, in V2f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V2f value, JsonSerializerOptions options)
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
        public static void W(this Utf8JsonWriter writer, in V3d value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V3d value, JsonSerializerOptions options)
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
        public static void W(this Utf8JsonWriter writer, in V3f value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteEndArray();
        }
        public static void R(this ref Utf8JsonReader reader, out V3f value, JsonSerializerOptions options)
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

    [TestFixture]
    class SystemTextJsonTests
    {
        #region Helpers

        private static readonly List<JsonConverter> _converters = new()
        {
        };
        private static readonly JsonSerializerOptions _options;
        static SystemTextJsonTests()
        {
            _options = new()
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                //WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString,
                Converters =
                {
                    new JsonStringEnumConverter(),
                },
            };

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

        [Test] public void Box2i_Roundtrip()            => RoundtripTest(new Box2i(new V2i(1, -2), new V2i(-17, 0)));
        [Test] public void Box2i_Roundtrip_Infinite()   => RoundtripTest(Box2i.Infinite);
        [Test] public void Box2i_Roundtrip_Invalid()    => RoundtripTest(Box2i.Invalid);

        [Test] public void Box2l_Roundtrip()            => RoundtripTest(new Box2l(new V2l(1, -2), new V2l(-17, 0)));
        [Test] public void Box2l_Roundtrip_Infinite()   => RoundtripTest(Box2l.Infinite);
        [Test] public void Box2l_Roundtrip_Invalid()    => RoundtripTest(Box2l.Invalid);

        [Test] public void Box2f_Roundtrip()            => RoundtripTest(new Box2f(new V2f(1.1, -2.2), new V2f(-17.17, 0)));
        [Test] public void Box2f_Roundtrip_Infinite()   => RoundtripTest(Box2f.Infinite);
        [Test] public void Box2f_Roundtrip_Invalid()    => RoundtripTest(Box2f.Invalid);

        [Test] public void Box2d_Roundtrip()            => RoundtripTest(new Box2d(new V2d(1.1, -2.2), new V2d(-17.17, 0)));
        [Test] public void Box2d_Roundtrip_Infinite()   => RoundtripTest(Box2d.Infinite);
        [Test] public void Box2d_Roundtrip_Invalid()    => RoundtripTest(Box2d.Invalid);

        [Test] public void Box3i_Roundtrip()            => RoundtripTest(new Box3i(new V3i(1, -2, 0), new V3i(-17, 42, -555555555)));
        [Test] public void Box3i_Roundtrip_Infinite()   => RoundtripTest(Box3i.Infinite);
        [Test] public void Box3i_Roundtrip_Invalid()    => RoundtripTest(Box3i.Invalid);

        [Test] public void Box3l_Roundtrip()            => RoundtripTest(new Box3l(new V3l(1, -2, 0), new V3l(-17, 42, -555555555)));
        [Test] public void Box3l_Roundtrip_Infinite()   => RoundtripTest(Box3l.Infinite);
        [Test] public void Box3l_Roundtrip_Invalid()    => RoundtripTest(Box3l.Invalid);

        [Test] public void Box3f_Roundtrip()            => RoundtripTest(new Box3f(new V3f(1.1, -2.2, 0), new V3f(-17.17, 42.42, -555.001)));
        [Test] public void Box3f_Roundtrip_Infinite()   => RoundtripTest(Box3f.Infinite);
        [Test] public void Box3f_Roundtrip_Invalid()    => RoundtripTest(Box3f.Invalid);

        [Test] public void Box3d_Roundtrip()            => RoundtripTest(new Box3d(new V3d(1.1, -2.2, 0), new V3d(-17.18, 42.42, -555.001)));
        [Test] public void Box3d_Roundtrip_Infinite()   => RoundtripTest(Box3d.Infinite);
        [Test] public void Box3d_Roundtrip_Invalid()    => RoundtripTest(Box3d.Invalid);

        #endregion

        #region C[34][b|d|f|ui|us], CieLabf, CieLuvf, CieXYZf, CieYxyf, CMYKf, HSLf, HSVf, Yuvf

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
        [Test] public void CIeLuvf_Roundtrip() => RoundtripTest(new CieLuvf(0.1, 0.2, 0.3));
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

        #region Range1*

        [Test] public void Range1b_Roundtrip() => RoundtripTest(new Range1b(7, 42));
        [Test] public void Range1d_Roundtrip() => RoundtripTest(new Range1d(-7.1, 42.2));
        [Test] public void Range1f_Roundtrip() => RoundtripTest(new Range1f(-7.1f, 42.2f));
        [Test] public void Range1i_Roundtrip() => RoundtripTest(new Range1i(-7, 42));
        [Test] public void Range1l_Roundtrip() => RoundtripTest(new Range1l(-7, 42));
        [Test] public void Range1s_Roundtrip() => RoundtripTest(new Range1s(-7, 42));
        [Test] public void Range1sb_Roundtrip() => RoundtripTest(new Range1sb(-7, 42));
        [Test] public void Range1ui_Roundtrip() => RoundtripTest(new Range1ui(7, 42));
        [Test] public void Range1ul_Roundtrip() => RoundtripTest(new Range1ul(7, 42));
        [Test] public void Range1us_Roundtrip() => RoundtripTest(new Range1us(7, 42));

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

        [Test] public void V2f_Roundtrip()                  => RoundtripTest(new V2f(1.1, -2.2));
        [Test] public void V2f_Roundtrip_MinValue()         => RoundtripTest(V2f.MinValue);
        [Test] public void V2f_Roundtrip_MaxValue()         => RoundtripTest(V2f.MaxValue);
        [Test] public void V2f_Roundtrip_NegativeInfinity() => RoundtripTest(V2f.NegativeInfinity);
        [Test] public void V2f_Roundtrip_PositiveInfinity() => RoundtripTest(V2f.PositiveInfinity);
        [Test] public void V2f_Roundtrip_NaN()              => RoundtripTest(V2f.NaN);

        [Test] public void V2d_Roundtrip()                  => RoundtripTest(new V2d(1.1, -2.2));
        [Test] public void V2d_Roundtrip_MinValue()         => RoundtripTest(V2d.MinValue);
        [Test] public void V2d_Roundtrip_MaxValue()         => RoundtripTest(V2d.MaxValue);
        [Test] public void V2d_Roundtrip_NegativeInfinity() => RoundtripTest(V2d.NegativeInfinity);
        [Test] public void V2d_Roundtrip_PositiveInfinity() => RoundtripTest(V2d.PositiveInfinity);
        [Test] public void V2d_Roundtrip_NaN()              => RoundtripTest(V2d.NaN);

        [Test] public void V2i_Roundtrip()                  => RoundtripTest(new V2i(1, -2));
        [Test] public void V2i_Roundtrip_MinValue()         => RoundtripTest(V2i.MinValue);
        [Test] public void V2i_Roundtrip_MaxValue()         => RoundtripTest(V2i.MaxValue);

        [Test] public void V2l_Roundtrip()                  => RoundtripTest(new V2l(1, -2));
        [Test] public void V2l_Roundtrip_MinValue()         => RoundtripTest(V2l.MinValue);
        [Test] public void V2l_Roundtrip_MaxValue()         => RoundtripTest(V2l.MaxValue);



        [Test] public void V3f_Roundtrip()                  => RoundtripTest(new V3f(1.1, 0, -2.2));
        [Test] public void V3f_Roundtrip_MinValue()         => RoundtripTest(V3f.MinValue);
        [Test] public void V3f_Roundtrip_MaxValue()         => RoundtripTest(V3f.MaxValue);
        [Test] public void V3f_Roundtrip_NegativeInfinity() => RoundtripTest(V3f.NegativeInfinity);
        [Test] public void V3f_Roundtrip_PositiveInfinity() => RoundtripTest(V3f.PositiveInfinity);
        [Test] public void V3f_Roundtrip_NaN()              => RoundtripTest(V3f.NaN);

        [Test] public void V3d_Roundtrip()                  => RoundtripTest(new V3d(1.1, 0, -2.2));
        [Test] public void V3d_Roundtrip_MinValue()         => RoundtripTest(V3d.MinValue);
        [Test] public void V3d_Roundtrip_MaxValue()         => RoundtripTest(V3d.MaxValue);
        [Test] public void V3d_Roundtrip_NegativeInfinity() => RoundtripTest(V3d.NegativeInfinity);
        [Test] public void V3d_Roundtrip_PositiveInfinity() => RoundtripTest(V3d.PositiveInfinity);
        [Test] public void V3d_Roundtrip_NaN()              => RoundtripTest(V3d.NaN);

        [Test] public void V3i_Roundtrip()                  => RoundtripTest(new V3i(1, 0, -2));
        [Test] public void V3i_Roundtrip_MinValue()         => RoundtripTest(V3i.MinValue);
        [Test] public void V3i_Roundtrip_MaxValue()         => RoundtripTest(V3i.MaxValue);

        [Test] public void V3l_Roundtrip()                  => RoundtripTest(new V3l(1, 0, -2));
        [Test] public void V3l_Roundtrip_MinValue()         => RoundtripTest(V3l.MinValue);
        [Test] public void V3l_Roundtrip_MaxValue()         => RoundtripTest(V3l.MaxValue);



        [Test] public void V4f_Roundtrip()                  => RoundtripTest(new V4f(1.1, 0, -2.2, 3.3));
        [Test] public void V4f_Roundtrip_MinValue()         => RoundtripTest(V4f.MinValue);
        [Test] public void V4f_Roundtrip_MaxValue()         => RoundtripTest(V4f.MaxValue);
        [Test] public void V4f_Roundtrip_NegativeInfinity() => RoundtripTest(V4f.NegativeInfinity);
        [Test] public void V4f_Roundtrip_PositiveInfinity() => RoundtripTest(V4f.PositiveInfinity);
        [Test] public void V4f_Roundtrip_NaN()              => RoundtripTest(V4f.NaN);

        [Test] public void V4d_Roundtrip()                  => RoundtripTest(new V4d(1.1, 0, -2.2, 3.3));
        [Test] public void V4d_Roundtrip_MinValue()         => RoundtripTest(V4d.MinValue);
        [Test] public void V4d_Roundtrip_MaxValue()         => RoundtripTest(V4d.MaxValue);
        [Test] public void V4d_Roundtrip_NegativeInfinity() => RoundtripTest(V4d.NegativeInfinity);
        [Test] public void V4d_Roundtrip_PositiveInfinity() => RoundtripTest(V4d.PositiveInfinity);
        [Test] public void V4d_Roundtrip_NaN()              => RoundtripTest(V4d.NaN);

        [Test] public void V4i_Roundtrip()                  => RoundtripTest(new V4i(1, 0, -2, 3));
        [Test] public void V4i_Roundtrip_MinValue()         => RoundtripTest(V4i.MinValue);
        [Test] public void V4i_Roundtrip_MaxValue()         => RoundtripTest(V4i.MaxValue);

        [Test] public void V4l_Roundtrip()                  => RoundtripTest(new V4l(1, 0, -2, 3));
        [Test] public void V4l_Roundtrip_MinValue()         => RoundtripTest(V4l.MinValue);
        [Test] public void V4l_Roundtrip_MaxValue()         => RoundtripTest(V4l.MaxValue);

        #endregion
    }
}
