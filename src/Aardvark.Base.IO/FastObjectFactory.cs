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
using System.Reflection;
using System.Reflection.Emit;

namespace Aardvark.Base.Coder;

public static class FastObjectFactory
{
    private static readonly FastConcurrentDict<Type, Func<object>> s_creatorCache = [];

    /// <summary>
    /// Returns an object factory that can be used to create instances
    /// of the specified type T.
    /// </summary>
    public static Func<object> ObjectFactory(Type type)
    {
        var creator = s_creatorCache.Get(type, null);   // Some (non-exhaustive) tests
        if (creator != null)                            // suggest this is faster than
            return creator;                             // just using the call in the
        else                                            // else branch.
            return s_creatorCache.GetOrCreate(type, s_createObjectFun);
    }

    private static readonly Func<Type, Func<object>> s_createObjectFun = t =>
    {
        // get the parameterless constructor (including non-public) of the type t
        var constructor =
            t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null)
            ?? throw new InvalidOperationException(
                string.Format("cannot create object of type \"{0}\" - type does not have parameterless constructor", t.Name)
                );
        var dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + t.Name, typeof(Func<object>), null, t);
        ILGenerator ilGen = dynMethod.GetILGenerator();
        ilGen.Emit(OpCodes.Newobj, constructor);
        ilGen.Emit(OpCodes.Ret);
        return (Func<object>)dynMethod.CreateDelegate(typeof(Func<object>));
    };
}
