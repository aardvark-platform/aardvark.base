/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aardvark.Base;

/// <summary>
/// A 2^Exponent sized square positioned at (X,Y) * 2^Exponent.
/// </summary>
[DataContract]
public readonly partial struct Cell2d : IEquatable<Cell2d>
{
    /// <summary>
    /// Unit cell (0, 0, 0) -> Box2d[(0.0, 0.0), (1.0, 1.0)]
    /// </summary>
    public static readonly Cell2d Unit = new(0L, 0L, 0);

    /// <summary>
    /// Special value denoting "invalid cell".
    /// </summary>
    public static Cell2d Invalid => new(long.MinValue, long.MinValue, int.MinValue);

    /// <summary>
    /// </summary>
    [DataMember]
    public readonly long X;

    /// <summary>
    /// </summary>
    [DataMember]
    public readonly long Y;

    /// <summary>
    /// </summary>
    [DataMember(Name = "E")]
    public readonly int Exponent;

    #region constructors

    /// <summary>
    /// Creates a 2^exponent sized square positioned at (x,y) * 2^exponent.
    /// </summary>
    public Cell2d(long x, long y, int exponent)
    {
        X = x; Y = y; Exponent = exponent;
    }

    /// <summary>
    /// Creates a 2^exponent sized square positioned at (x,y) * 2^exponent.
    /// </summary>
    public Cell2d(int x, int y, int exponent)
    {
        X = x; Y = y; Exponent = exponent;
    }

    /// <summary>
    /// Creates copy of cell.
    /// </summary>
    public Cell2d(Cell2d cell)
    {
        X = cell.X; Y = cell.Y; Exponent = cell.Exponent;
    }

    /// <summary>
    /// Cell with min corner at index*2^exponent and dimension 2^exponent.
    /// </summary>
    public Cell2d(V2l index, int exponent) : this(index.X, index.Y, exponent) { }

    /// <summary>
    /// Cell with min corner at index*2^exponent and dimension 2^exponent.
    /// </summary>
    public Cell2d(V2i index, int exponent) : this(index.X, index.Y, exponent) { }

    /// <summary>
    /// Special cell, which is centered at origin, with dimension 2^exponent.
    /// </summary>
    public Cell2d(int exponent)
    {
        if (exponent == int.MinValue)
        {
            X = long.MinValue; Y = long.MinValue; Exponent = exponent;
        }
        else
        {
            X = long.MaxValue; Y = long.MaxValue; Exponent = exponent;
        }
    }

    /// <summary>
    /// Smallest cell that contains given box.
    /// </summary>
    public Cell2d(Box2f box) : this(new Box2d(box)) { }

    /// <summary>
    /// Smallest cell that contains given points.
    /// </summary>
    public Cell2d(V2f[] ps) : this(new Box2f(ps)) { }

    /// <summary>
    /// Smallest cell that contains given points.
    /// </summary>
    public Cell2d(V2d[] ps) : this(new Box2d(ps)) { }

    /// <summary>
    /// Smallest cell that contains given points.
    /// </summary>
    public Cell2d(IEnumerable<V2f> ps) : this(new Box2f(ps)) { }

    /// <summary>
    /// Smallest cell that contains given points.
    /// </summary>
    public Cell2d(IEnumerable<V2d> ps) : this(new Box2d(ps)) { }

    /// <summary>
    /// Smallest cell that contains given point.
    /// </summary>
    public Cell2d(V2d p) : this(new Box2d(p)) { }

    /// <summary>
    /// Smallest cell that contains given box, 
    /// where box.Min and box.Max are both interpreted as inclusive
    /// (whereas a cell's maximum is defined as exclusive).
    /// </summary>
    public Cell2d(Box2d box)
    {
        if (!box.IsValid) throw new InvalidOperationException();

        // case 1: contains origin
        if (box.Min.X < 0.0 && box.Max.X >= 0.0 ||
            box.Min.Y < 0.0 && box.Max.Y >= 0.0)
        {
            X = Y = long.MaxValue;
            Exponent = Math.Max(box.Min.NormMax, box.Max.NormMax).Log2CeilingInt() + 1;
        }
        else // case 2: doesn't contain origin
        {
            Exponent = (box.Min == box.Max)
                    ? (box.Min.NormMax / (long.MaxValue >> 1)).Log2CeilingInt()
                    : box.Size.NormMax.Log2CeilingInt()
                    ;
            var s = Math.Pow(2.0, Exponent);
            var a = (box.Min * (1.0 / s)).Floor();

            X = (long)a.X;
            Y = (long)a.Y;

            while (box.Max.X >= X * s + s || box.Max.Y >= Y * s + s)
            {
                X >>= 1; Y >>= 1; ++Exponent;
                s *= 2.0;
            }
        }
    }

    /// <summary>
    /// Common root of a and b.
    /// </summary>
    public Cell2d(Cell2d a, Cell2d b) : this(GetCommonRoot(a, b)) { }

    /// <summary>
    /// Common root of cells.
    /// </summary>
    public Cell2d(params Cell2d[] cells) : this(GetCommonRoot(cells)) { }

    #endregion

    /// <summary>
    /// </summary>
    [JsonIgnore]
    public V2l XY => new(X, Y);

    /// <summary>
    /// Gets whether this cell is a special cell centered at origin.
    /// </summary>
    [JsonIgnore]
    public bool IsCenteredAtOrigin => X == long.MaxValue && Y == long.MaxValue;

    /// <summary>
    /// Returns true if this is the special invalid cell.
    /// </summary>
    [JsonIgnore]
    public bool IsInvalid => X == long.MinValue && Y == long.MinValue && Exponent == int.MinValue;

    /// <summary>
    /// Returns true if this is NOT the special invalid cell.
    /// </summary>
    [JsonIgnore]
    public bool IsValid => X != long.MinValue || Y != long.MinValue || Exponent != int.MinValue;

    /// <summary>
    /// Returns true if the cell completely contains the other cell.
    /// A cell contains itself.
    /// </summary>
    public bool Contains(Cell2d other)
    {
        if (other.Exponent > Exponent) return false;
        if (other.Exponent == Exponent) return other.X == X && other.Y == Y;

        // always true from here on:
        //   Exponent > other.Exponent
        //   other.Exponent < Exponent

        if (IsCenteredAtOrigin)
        {
            if (other.IsCenteredAtOrigin)
            {
                return true;
            }
            else
            {
                // bring other into scale of this
                var d = (Exponent - 1) - other.Exponent;

                // right-shift with long argument only uses low-order 6 bits
                // https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/operators/right-shift-operator
                if (d > 63) d = 63;

                var x = other.X >> d;
                var y = other.Y >> d;
                return x >= -1 && x <= 0 && y >= -1 && y <= 0;
            }
        }
        else
        {
            if (other.IsCenteredAtOrigin)
            {
                return false;
            }
            else
            {
                // bring other into scale of this
                var d = Exponent - other.Exponent;

                // right-shift with long argument only uses low-order 6 bits
                // https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/operators/right-shift-operator
                if (d > 63) d = 63;

                var x = other.X >> d;
                var y = other.Y >> d;
                return x == X && y == Y;
            }
        }
    }

    /// <summary>
    /// Returns true if two cells intersect each other (or one contains the other).
    /// Cells DO NOT intersect if only touching from the outside.
    /// A cell intersects itself.
    /// </summary>
    public bool Intersects(Cell2d other)
    {
        if (X == other.X && Y == other.Y && Exponent == other.Exponent) return true;
        return BoundingBox.Intersects(other.BoundingBox);
    }

    /// <summary>
    /// Gets the 4 subcells.
    /// Order is (0,0), (1,0), (0,1), (1,1).
    /// </summary>
    [JsonIgnore]
    public Cell2d[] Children
    {
        get
        {
            var x0 = X << 1; var y0 = Y << 1;
            if (IsCenteredAtOrigin) { x0++; y0++; }
            var x1 = x0 + 1; var y1 = y0 + 1;
            var e = Exponent - 1;
            return
            [
                new Cell2d(x0, y0, e),
                new Cell2d(x1, y0, e),
                new Cell2d(x0, y1, e),
                new Cell2d(x1, y1, e),
            ];
        }
    }

    /// <summary>
    /// Gets parent cell.
    /// </summary>
    [JsonIgnore]
    public Cell2d Parent => IsCenteredAtOrigin ? new Cell2d(Exponent + 1) : new Cell2d(X >> 1, Y >> 1, Exponent + 1);

    /// <summary>
    /// True if one corner of this cell touches the origin.
    /// Centered cells DO NOT touch the origin.
    /// </summary>
    [JsonIgnore]
    public bool TouchesOrigin => !IsCenteredAtOrigin && (X == -1 || X == 0) && (Y == -1 || Y == 0);

    /// <summary>
    /// Gets cell's bounds.
    /// </summary>
    [JsonIgnore]
    public Box2d BoundingBox => ComputeBoundingBox(X, Y, Exponent);

    private static Box2d ComputeBoundingBox(long x, long y, int e)
    {
        var d = Math.Pow(2.0, e);
        var isCenteredAtOrigin = x == long.MaxValue && y == long.MaxValue;
        var min = isCenteredAtOrigin ? new V2d(-0.5 * d) : new V2d(x * d, y * d);
        return Box2d.FromMinAndSize(min, new V2d(d, d));
    }

    /// <summary>
    /// Computes cell's center.
    /// </summary>
    public V2d GetCenter() => BoundingBox.Center;

    /// <summary>
    /// Quadrant 0-3.
    /// 0th and 1st bit encodes x-, y-axis, respectively.
    /// E.g. 0 is quadrant at origin, 3 is quadrant oposite from origin.
    /// </summary>
    public Cell2d GetQuadrant(int i)
    {
        if (i < 0 || i > 3) throw new IndexOutOfRangeException();

        if (IsCenteredAtOrigin)
        {
            return new Cell2d(
                (i & 1) == 0 ? -1 : 0,
                (i & 2) == 0 ? -1 : 0,
                Exponent - 1
                );
        }
        else
        {
            return new Cell2d(
                (X << 1) + ((i & 1) == 0 ? 0 : 1),
                (Y << 1) + ((i & 2) == 0 ? 0 : 1),
                Exponent - 1
                );
        }
    }

    /// <summary>
    /// Returns the quadrant of this cell, in which the other cell is contained.
    /// If the other cell is not contained in any quadrant, then no value is returned.
    /// </summary>
    public int? GetQuadrant(Cell2d other)
    {
        // if other cell is bigger or same size, 
        // then it cannot be contained in any quadrant
        if (other.Exponent >= Exponent) return null;

        if (IsCenteredAtOrigin)
        {
            if (other.IsCenteredAtOrigin) return null;

            // scale up other to scale of this cell's quadrants
            // where coord values 0 or 1 mean left or right,
            // and all other values mean that the other cell is outside
            other <<= Exponent - other.Exponent - 1;
            var o = 0;
            if (other.X == 0) o = 1; else if (other.X != -1) return null;
            if (other.Y == 0) o |= 2; else if (other.Y != -1) return null;
            return o;
        }
        else
        {
            if (other.IsCenteredAtOrigin) return null;

            // scale up other to scale of this cell's quadrants,
            // where coord values -1 or 0 mean left or right,
            // and all other values mean that the other cell is outside
            other <<= Exponent - other.Exponent - 1;
            var x = X << 1;
            var y = Y << 1;
            var o = 0;
            if (other.X == x + 1) o = 1; else if (other.X != x) return null;
            if (other.Y == y + 1) o |= 2; else if (other.Y != y) return null;
            return o;
        }
    }

    /// <summary>
    /// Get the global quadtrant this cell is located in, or no value if this is a centered cell.
    /// </summary>
    public int? Quadrant
    {
        get
        {
            if (IsCenteredAtOrigin) return null;

            var o = 0;
            if (X >= 0) o = 1;
            if (Y >= 0) o |= 2;
            return o;
        }
    }

    /// <summary>
    /// Returns smallest common root of a and b.
    /// </summary>
    public static Cell2d GetCommonRoot(Cell2d a, Cell2d b)
    {
        if (a == b) return a;

        if (a.IsCenteredAtOrigin)
        {
            if (b.IsCenteredAtOrigin)
            {
                // if both are centered, then the larger cell is the result
                return a.Exponent > b.Exponent ? a : b;
            }
            else
            {
                // if A is centered and B is not centered,
                // then enlarge A until it contains B
                while (!a.Contains(b)) a = a.Parent;
                return a;
            }
        }
        else
        {
            if (b.IsCenteredAtOrigin)
            {
                // if A is not centered and B is centered,
                // then enlarge B until it contains A
                while (!b.Contains(a)) b = b.Parent;
                return b;
            }
            else
            {
                // if no cell is centered, ...
                if (a.Quadrant == b.Quadrant)
                {
                    // and both cells are located in the same global quadrant,
                    // then recursively grow smaller cell (take parent)
                    // until a and b meet at their common root
                    return a.Exponent > b.Exponent ? GetCommonRoot(a, b.Parent) : GetCommonRoot(a.Parent, b);
                }
                else
                {
                    // and each cell is located in a different global quadrant,
                    // then take a centered cell and enlarge it until it contains both A and B
                    var c = new Cell2d(Math.Max(a.Exponent, b.Exponent) + 1); // centered cell needs to be at least twice as large as the bigger one of A and B
                    while (!c.Contains(a)) c = c.Parent;
                    while (!c.Contains(b)) c = c.Parent;
                    return c;
                }
            }
        }
    }

    /// <summary>
    /// Returns smallest common root of cells.
    /// </summary>
    public static Cell2d GetCommonRoot(params Cell2d[] cells)
    {
        if (cells == null || cells.Length == 0) throw new ArgumentException("No cells.");
        var r = cells[0];
        for (var i = 1; i < cells.Length; i++) r = new Cell2d(r, cells[i]);
        return r;
    }

    #region operators

    /// <summary>
    /// Scales down cell by 'd' powers of two.
    /// BoundingBox.Min stays the same, except for cells centered at origin, which stay centered.
    /// </summary>
    public static Cell2d operator >>(Cell2d a, int d)
    {
        if (d < 1) return a;

        if (a.IsCenteredAtOrigin) return new Cell2d(a.Exponent - d);

        // right-shift with long argument only uses low-order 6 bits
        // https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/operators/right-shift-operator
        if (d > 63) d = 63;

        return new Cell2d(a.X << d, a.Y << d, a.Exponent - d);
    }

    /// <summary>
    /// Scales up cell by 'd' powers of two.
    /// BoundingBox.Min will snap to the next smaller or equal position allowed by the new cell size, except for cells centered at origin, which stay centered.
    /// </summary>
    public static Cell2d operator <<(Cell2d a, int d)
    {
        if (d < 1) return a;

        if (a.IsCenteredAtOrigin) return new Cell2d(a.Exponent + d);

        // right-shift with long argument only uses low-order 6 bits
        // https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/operators/right-shift-operator
        if (d > 63) d = 63;

        return new Cell2d(a.X >> d, a.Y >> d, a.Exponent + d);
    }

    #endregion

    #region equality and hashing

    /// <summary>
    /// </summary>
    public bool Equals(Cell2d other) => this == other;

    /// <summary>
    /// </summary>
    public static bool operator ==(Cell2d a, Cell2d b) => a.X == b.X && a.Y == b.Y && a.Exponent == b.Exponent;

    /// <summary>
    /// </summary>
    public static bool operator !=(Cell2d a, Cell2d b) => !(a == b);

    /// <summary>
    /// </summary>
    public override bool Equals(object obj) => obj is Cell2d other && this == other;

    /// <summary>
    /// </summary>
    public override int GetHashCode() => HashCode.GetCombined(X, Y, Exponent);

    #endregion

    #region binary serialization

    /// <summary>
    /// </summary>
    public byte[] ToByteArray()
    {
        var buffer = new byte[20];
        using var ms = new MemoryStream(buffer);
        using var bw = new BinaryWriter(ms);
        bw.Write(X); bw.Write(Y); bw.Write(Exponent);
        return buffer;
    }

    /// <summary>
    /// </summary>
    public static Cell2d Parse(byte[] buffer)
    {
        using var ms = new MemoryStream(buffer, 0, 20);
        using var br = new BinaryReader(ms);
        var x = br.ReadInt64(); var y = br.ReadInt64(); var e = br.ReadInt32();
        return new Cell2d(x, y, e);
    }

    #endregion

    #region string serialization

    /// <summary></summary>
    public override string ToString()
    {
        if (IsCenteredAtOrigin || IsInvalid)
        {
            return $"[{Exponent}]";
        }
        else
        {
            return $"[{X}, {Y}, {Exponent}]";
        }
    }

    public static Cell2d Parse(string s)
    {
#if NETSTANDARD2_0
        var xs = s.Substring(1, s.Length - 2).Split(',');
#else
        var xs = s[1..^1].Split(',');
#endif
        return xs.Length switch
        {
            1 => new Cell2d(exponent: int.Parse(xs[0])),
            3 => new Cell2d(x: long.Parse(xs[0]), y: long.Parse(xs[1]), exponent: int.Parse(xs[2])),
            _ => throw new FormatException(
                $"Expected [<x>,<y>,<z>,<exponent>], or [<exponent>], but found {s}. " +
                $"Error 47575d6f-072a-4166-ac83-5c4effa33427."
                )
        };
    }

#endregion
}
