using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static class VariousExtensions
    {
        public static IEnumerable<T> Expand<T>(this int n, Func<int, T> fun)
        {
            for (int i = 0; i < n; i++)
                yield return fun(i);
        }
    }
}
