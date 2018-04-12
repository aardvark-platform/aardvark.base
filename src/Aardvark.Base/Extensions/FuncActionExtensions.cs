using System;

namespace Aardvark.Base
{
    public static partial class FuncActionExtensions
    {
        /// <summary>
         /// Encapsulates the expression "[object] != null ? 'select something from [object]' : defaultValue
         /// </summary>
        public static Tr TrySelect<To, Tr>(this To o, Func<To, Tr> selector, Tr defaultValue = default(Tr))
            where To : class
        {
            return o != null ? selector(o) : defaultValue;
        }
    }
}
