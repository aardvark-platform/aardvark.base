using Aardvark.Base;
using System;
using System.Reflection.Emit;

namespace Aardvark.VRVis
{
    public static class FastObjectFactory
    {
        private static readonly FastConcurrentDict<Type, Func<object>> s_creatorCache =
            new FastConcurrentDict<Type, Func<object>>();

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

        private static readonly Func<Type, Func<object>> s_createObjectFun = (Type t) =>
        {
            var constructor = t.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
                throw new InvalidOperationException(
                    String.Format("cannot create object of type \"{0}\" - type does not have parameterless constructor",
                                  t.Name));
            var dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + t.Name, typeof(Func<object>), null, t);
            ILGenerator ilGen = dynMethod.GetILGenerator();
            ilGen.Emit(OpCodes.Newobj, constructor);
            ilGen.Emit(OpCodes.Ret);
            return (Func<object>)dynMethod.CreateDelegate(typeof(Func<object>));
        };
    }

}
