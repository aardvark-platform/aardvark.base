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
using System.Diagnostics;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0290 // Use primary constructor

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

#region Line1i

/// <summary>
/// A structure that holds the indices of the endpoints of a line.
/// </summary>
public partial struct Line1i : IEquatable<Line1i>
{
    public int I0;
    public int I1;

    #region Constructor

    public Line1i(int i0, int i1)
    {
        I0 = i0; I1 = i1; 
    }

    #endregion

    #region Properties

    public readonly IEnumerable<int> Indices
    {
        get
        {
            yield return I0;
            yield return I1;
        }
    }

    #endregion

    #region Indexer

    public int this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => I0,
                1 => I1,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: I0 = value; return;
                case 1: I1 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Constants

    public static Line1i Invalid => new() { I0 = -1, I1 = -1 };

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Line1i a, Line1i b)
    {
        return a.I0 == b.I0 && a.I1 == b.I1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Line1i a, Line1i b)
    {
        return a.I0 != b.I0 || a.I1 != b.I1;
    }

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(I0, I1);
    }

    public override readonly bool Equals(object other)
        => (other is Line1i o) && Equals(o);

    #endregion

    #region IEquatable<Line1i> Members

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Line1i other)
    {
        return this == other;
    }

    #endregion
}

#endregion

#region Triangle1i

/// <summary>
/// A structure that holds the indices of the vertices of a triangle.
/// </summary>
public partial struct Triangle1i : IEquatable<Triangle1i>
{
    public int I0;
    public int I1;
    public int I2;

    #region Constructor

    public Triangle1i(int i0, int i1, int i2)
    {
        I0 = i0; I1 = i1; I2 = i2; 
    }

    #endregion

    #region Properties

    public readonly IEnumerable<int> Indices
    {
        get
        {
            yield return I0;
            yield return I1;
            yield return I2;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line01 { get { return new Line1i(I0, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line02 { get { return new Line1i(I0, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line10 { get { return new Line1i(I1, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line12 { get { return new Line1i(I1, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line20 { get { return new Line1i(I2, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line21 { get { return new Line1i(I2, I1); } }

    #endregion

    #region Indexer

    public int this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => I0,
                1 => I1,
                2 => I2,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: I0 = value; return;
                case 1: I1 = value; return;
                case 2: I2 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Constants

    public static Triangle1i Invalid => new() { I0 = -1, I1 = -1, I2 = -1 };

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Triangle1i a, Triangle1i b)
    {
        return a.I0 == b.I0 && a.I1 == b.I1 && a.I2 == b.I2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Triangle1i a, Triangle1i b)
    {
        return a.I0 != b.I0 || a.I1 != b.I1 || a.I2 != b.I2;
    }

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(I0, I1, I2);
    }

    public override readonly bool Equals(object other)
        => (other is Triangle1i o) && Equals(o);

    #endregion

    #region IEquatable<Triangle1i> Members

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Triangle1i other)
    {
        return this == other;
    }

    #endregion
}

#endregion

#region Quad1i

/// <summary>
/// A structure that holds the indices of the vertices of a quad.
/// </summary>
public partial struct Quad1i : IEquatable<Quad1i>
{
    public int I0;
    public int I1;
    public int I2;
    public int I3;

    #region Constructor

    public Quad1i(int i0, int i1, int i2, int i3)
    {
        I0 = i0; I1 = i1; I2 = i2; I3 = i3; 
    }

    #endregion

    #region Properties

    public readonly IEnumerable<int> Indices
    {
        get
        {
            yield return I0;
            yield return I1;
            yield return I2;
            yield return I3;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line01 { get { return new Line1i(I0, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line02 { get { return new Line1i(I0, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line03 { get { return new Line1i(I0, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line10 { get { return new Line1i(I1, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line12 { get { return new Line1i(I1, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line13 { get { return new Line1i(I1, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line20 { get { return new Line1i(I2, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line21 { get { return new Line1i(I2, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line23 { get { return new Line1i(I2, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line30 { get { return new Line1i(I3, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line31 { get { return new Line1i(I3, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line32 { get { return new Line1i(I3, I2); } }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle012 { get { return new Triangle1i(I0, I1, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle013 { get { return new Triangle1i(I0, I1, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle021 { get { return new Triangle1i(I0, I2, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle023 { get { return new Triangle1i(I0, I2, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle031 { get { return new Triangle1i(I0, I3, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle032 { get { return new Triangle1i(I0, I3, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle102 { get { return new Triangle1i(I1, I0, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle103 { get { return new Triangle1i(I1, I0, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle120 { get { return new Triangle1i(I1, I2, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle123 { get { return new Triangle1i(I1, I2, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle130 { get { return new Triangle1i(I1, I3, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle132 { get { return new Triangle1i(I1, I3, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle201 { get { return new Triangle1i(I2, I0, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle203 { get { return new Triangle1i(I2, I0, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle210 { get { return new Triangle1i(I2, I1, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle213 { get { return new Triangle1i(I2, I1, I3); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle230 { get { return new Triangle1i(I2, I3, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle231 { get { return new Triangle1i(I2, I3, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle301 { get { return new Triangle1i(I3, I0, I1); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle302 { get { return new Triangle1i(I3, I0, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle310 { get { return new Triangle1i(I3, I1, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle312 { get { return new Triangle1i(I3, I1, I2); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle320 { get { return new Triangle1i(I3, I2, I0); } }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle321 { get { return new Triangle1i(I3, I2, I1); } }

    #endregion

    #region Indexer

    public int this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => I0,
                1 => I1,
                2 => I2,
                3 => I3,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: I0 = value; return;
                case 1: I1 = value; return;
                case 2: I2 = value; return;
                case 3: I3 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Constants

    public static Quad1i Invalid => new() { I0 = -1, I1 = -1, I2 = -1, I3 = -1 };

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quad1i a, Quad1i b)
    {
        return a.I0 == b.I0 && a.I1 == b.I1 && a.I2 == b.I2 && a.I3 == b.I3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quad1i a, Quad1i b)
    {
        return a.I0 != b.I0 || a.I1 != b.I1 || a.I2 != b.I2 || a.I3 != b.I3;
    }

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(I0, I1, I2, I3);
    }

    public override readonly bool Equals(object other)
        => (other is Quad1i o) && Equals(o);

    #endregion

    #region IEquatable<Quad1i> Members

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Quad1i other)
    {
        return this == other;
    }

    #endregion
}

#endregion

