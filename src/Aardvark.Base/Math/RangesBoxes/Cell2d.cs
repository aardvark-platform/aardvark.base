using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    /// <summary>
    /// A 2^Exponent sized square positioned at (X,Y) * 2^Exponent.
    /// </summary>
    [DataContract]
    public struct Cell2d : IEquatable<Cell2d>
    {
        /// <summary>
        /// Unit cell (0, 0, 0) -> Box2d[(0.0, 0.0), (1.0, 1.0)]
        /// </summary>
        public static readonly Cell2d Unit = new Cell2d(0, 0, 0);

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

        /// <summary>
        /// Creates a 2^exponent sized square positioned at (x,y) * 2^exponent.
        /// </summary>
        public Cell2d(long x, long y, int exponent)
        {
            X = x; Y = y; Exponent = exponent;
        }

        /// <summary>
        /// Cell with min corner at index*2^exponent and dimension 2^exponent.
        /// </summary>
        public Cell2d(V2l index, int exponent) : this(index.X, index.Y, exponent) { }

        /// <summary>
        /// Special cell, which is centered at origin, with dimension 2^exponent.
        /// </summary>
        public Cell2d(int exponent) : this(long.MaxValue, long.MaxValue, exponent) { }

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
        /// Smallest cell that contains given box.
        /// </summary>
        public Cell2d(Box2d box)
        {
            if (!box.IsValid) throw new InvalidOperationException();

            // case 1: contains origin
            if (box.Min.X < 0.0 && box.Max.X > 0.0 ||
                box.Min.Y < 0.0 && box.Max.Y > 0.0)
            {
                X = Y = long.MaxValue;
                Exponent = Math.Max(box.Min.NormMax, box.Max.NormMax).Log2Int() + 1;
            }
            else // case 2: doesn't contain origin
            {
                Exponent = (box.Min == box.Max)
                        ? (box.Min.NormMax / (long.MaxValue >> 1)).Log2Int()
                        : box.Size.NormMax.Log2Int()
                        ;
                var s = Math.Pow(2.0, Exponent);
                var a = (box.Min / s).Floor() * s;
                while (a.X + s < box.Max.X || a.Y + s < box.Max.Y)
                {
                    s *= 2.0; Exponent++;
                    a = (box.Min / s).Floor() * s;
                }
                X = (long)Math.Floor(a.X / s);
                Y = (long)Math.Floor(a.Y / s);
            }
        }

        /// <summary>
        /// </summary>
        public V2l XY => new V2l(X, Y);

        /// <summary>
        /// Gets whether this cell is a special cell centered at origin.
        /// </summary>
        public bool IsCenteredAtOrigin => X == long.MaxValue && Y == long.MaxValue;

        /// <summary>
        /// Returns true if the cell completely contains the other cell.
        /// A cell contains itself.
        /// </summary>
        public bool Contains(Cell2d other)
        {
            if (other.Exponent > Exponent) return false;
            if (other.Exponent == Exponent) return other.X == X && other.Y == Y;

            if (IsCenteredAtOrigin)
            {
                if (other.IsCenteredAtOrigin)
                {
                    return Exponent >= other.Exponent;
                }
                else
                {
                    if (Exponent > other.Exponent)
                    {
                        // bring other into scale of this
                        var d = Exponent - other.Exponent;

                        // right-shift with long argument only uses low-order 6 bits
                        // https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/operators/right-shift-operator
                        if (d > 63) d = 63;

                        var x = other.X >> d;
                        var y = other.Y >> d;
                        return x >= -1 && x <= 0 && y >= -1 && y <= 0;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
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
                    if (Exponent > other.Exponent)
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
                    else
                    {
                        // bring this into scale of other
                        var d = other.Exponent - Exponent;

                        // right-shift with long argument only uses low-order 6 bits
                        // https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/operators/right-shift-operator
                        if (d > 63) d = 63;

                        var x = X >> d;
                        var y = Y >> d;
                        return x == other.X && y == other.Y;
                    }
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
        /// Gets indices of the 8 subcells.
        /// </summary>
        public Cell2d[] Children
        {
            get
            {
                var x0 = X << 1; var y0 = Y << 1;
                if (IsCenteredAtOrigin) { x0++; y0++; }
                var x1 = x0 + 1; var y1 = y0 + 1;
                var e = Exponent - 1;
                return new[]
                {
                    new Cell2d(x0, y0, e),
                    new Cell2d(x1, y0, e),
                    new Cell2d(x0, y1, e),
                    new Cell2d(x1, y1, e),
                };
            }
        }

        /// <summary>
        /// Gets parent cell.
        /// </summary>
        public Cell2d Parent => IsCenteredAtOrigin ? new Cell2d(Exponent + 1) : new Cell2d(X >> 1, Y >> 1, Exponent + 1);

        /// <summary>
        /// True if one corner of this cell touches the origin.
        /// Centered cells DO NOT touch the origin.
        /// </summary>
        public bool TouchesOrigin => IsCenteredAtOrigin ? false : (X == -1 || X == 0) && (Y == -1 || Y == 0);

        /// <summary>
        /// Gets cell's bounds.
        /// </summary>
        public Box2d BoundingBox => ComputeBoundingBox(X, Y, Exponent);

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
            // then it cannot be contained in any octant
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
                var o = 0;
                if (other.X == 1) o = 1; else if (other.X != 0) return null;
                if (other.Y == 1) o |= 2; else if (other.Y != 0) return null;
                return o;
            }
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
        public override bool Equals(object obj) => obj is Cell2d && this == (Cell2d)obj;

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

        /// <summary></summary>
        public override string ToString()
        {
            return $"[{X}, {Y}, {Exponent}]";
        }

        private static Box2d ComputeBoundingBox(long x, long y, int e)
        {
            var d = Math.Pow(2.0, e);
            var isCenteredAtOrigin = x == long.MaxValue && y == long.MaxValue;
            var min = isCenteredAtOrigin ? new V2d(-0.5 * d) : new V2d(x * d, y * d);
            return Box2d.FromMinAndSize(min, new V2d(d, d));
        }
    }
}
