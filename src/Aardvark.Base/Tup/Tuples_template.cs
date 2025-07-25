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
namespace Aardvark.Base;

#pragma warning disable IDE0290 // Use primary constructor

// AUTO GENERATED CODE - DO NOT CHANGE!
//# Action comma = () => Out(", ");
//# Action space = () => Out(" ");
//# int MaxTupleCount = 10;
//# int MaxSimpleTupleCount = 16;

#region Simple Tuples of uniform type

//# for (int tc = 2; tc <= MaxSimpleTupleCount; tc++) {
//# Func<int, string> ename = i => tc < 10 ? i.ToString() : i.ToString("00");
#region Tup__tc__

public struct Tup__tc__<T>
{
    public T /*# tc.ForEach(i => { var e = ename(i); */E__e__/*# }, comma); */;

    #region Constructors

    public Tup__tc__(T e) {/*# tc.ForEach(i => { var e = ename(i); */ E__e__ = e;/*# }); */ }
    public Tup__tc__(/*# tc.ForEach(i => { var e = ename(i); */T e__e__/*# }, comma); */) {/*# tc.ForEach(i => { var e = ename(i); */ E__e__ = e__e__;/*# }); */ }

    #endregion
}

#endregion

//# } // tc
#endregion

//# for (int tc = 2; tc <= MaxTupleCount; tc++) {
#region Tuple of __tc__ Elements

/// <summary>
/// A tuple is a structure containing three elements of different types,
/// that can be accessed using the fields /*# tc.ForEach(i => { */E__i__/*# }, comma); */.
/// </summary>
public struct Tup</*# tc.ForEach(i => { */T__i__/*# }, comma); */>
{
    //# for (int i = 0; i < tc; i++) {
    /// <summary>
    /// Element __i__ of the tuple.
    /// </summary>
    public T__i__ E__i__;

    //# } // i
    public Tup(/*# tc.ForEach(i => { */T__i__ e__i__/*# }, comma); */)
    {/*# tc.ForEach(i => { */ E__i__ = e__i__;/*# }); */ }

    public override readonly string ToString()
    {
        return string.Format("(/*# tc.ForEach(i => { */{__i__}/*# }, comma); */)", /*# tc.ForEach(i => { */E__i__/*# }, comma); */);
    }
}

#endregion

//# } // tc
#region Static Tuple Create Methods

/// <summary>
/// This static class allows the creation of tuples without specifying
/// the types in angle brackets.
/// </summary>
public static class Tup
{
    //# for (int tc = 2; tc <= MaxTupleCount; tc++) {
    //# var tpar = tc.Range().Select(i => "T" + i).Join(", ");
    public static Tup<__tpar__> Create<__tpar__>(/*# tc.ForEach(i => { */T__i__ e__i__/*# }, comma); */)
    {
        return new Tup<__tpar__>(/*# tc.ForEach(i => { */e__i__/*# }, comma); */);
    }

    //# } // tc
}

#endregion
