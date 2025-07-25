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

//# Action comma = () => Out(", ");
//# Action andand = () => Out(" && ");
//# Action oror = () => Out(" || ");
//# var types   = new[] { null, null,   "Line1i",   "Triangle1i",   "Quad1i"    };
//# var names   = new[] { null, null,   "line",     "triangle",     "quad"      };
//# var points  = new[] { null, null,   "endpoint", "vertex",       "vertex"    };
//#
//# for (int d = 2; d <= 4; d++)
//# {
//#     string type = types[d], name = names[d], point = points[d].Plural(d);
//#
#region __type__

/// <summary>
/// A structure that holds the indices of the __point__ of a __name__.
/// </summary>
public partial struct __type__ : IEquatable<__type__>
{
    //# d.ForEach(i => {
    public int I__i__;
    //# });

    #region Constructor

    public __type__(/*# d.ForEach(i => { */int i__i__/*# }, comma); */)
    {
        /*# d.ForEach(i => { */I__i__ = i__i__; /*# }); */
    }

    #endregion

    #region Properties

    public readonly IEnumerable<int> Indices
    {
        get
        {
            //# d.ForEach(i => {
            yield return I__i__;
            //# });
        }
    }

    //# if (d > 2) {
    //#     d.ForEach(i0 =>  {
    //#         d.ForEach(i1 => { if (i1 != i0) {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Line1i Line__i0____i1__ { get { return new Line1i(I__i0__, I__i1__); } }
    //#         }});
    //#     });

    //# }
    //# if (d > 3) {
    //#     d.ForEach(i0 => {
    //#         d.ForEach(i1 => { if (i1 != i0) {
    //#             d.ForEach(i2 => { if (i2 != i1 && i2 != i0) {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Triangle1i Triangle__i0____i1____i2__ { get { return new Triangle1i(I__i0__, I__i1__, I__i2__); } }
    //#             }});
    //#         }});
    //#     });

    //# }
    #endregion

    #region Indexer

    public int this[int index]
    {
        readonly get
        {
            return index switch
            {
                //# d.ForEach(i => {
                __i__ => I__i__,
                //# });
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                //# d.ForEach(i => {
                case __i__: I__i__ = value; return;
                //# });
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Constants

    public static __type__ Invalid => new() { /*# d.ForEach(i => { */I__i__ = -1/*# }, comma); */ };

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(__type__ a, __type__ b)
    {
        return /*# d.ForEach(i => { */a.I__i__ == b.I__i__/*# }, andand); */;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(__type__ a, __type__ b)
    {
        return /*# d.ForEach(i => { */a.I__i__ != b.I__i__/*# }, oror); */;
    }

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(/*# d.ForEach(i => { */I__i__/*# }, comma); */);
    }

    public override readonly bool Equals(object other)
        => (other is __type__ o) && Equals(o);

    #endregion

    #region IEquatable<__type__> Members

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(__type__ other)
    {
        return this == other;
    }

    #endregion
}

#endregion

//# }