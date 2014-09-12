using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static class ICollectionExtensions
    {
        public static void ToggleContainment<T>(this ICollection<T> self, T item)
        {
            if (!self.Remove(item))
                self.Add(item);
        }
    }
}
