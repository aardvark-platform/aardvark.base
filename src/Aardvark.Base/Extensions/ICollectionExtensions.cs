using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds the item to the collection if not contained, otherwise removes the item from the collection.
        /// </summary>
        /// <returns>Containment state of the item after the operation</returns>
        public static bool ToggleContainment<T>(this ICollection<T> self, T item)
        {
            if (!self.Remove(item))
            {
                self.Add(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the elements in the items enumeration to the collection.
        /// </summary>
        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items)
        {
            foreach (var item in items)
                self.Add(item);
        }
    }
}
