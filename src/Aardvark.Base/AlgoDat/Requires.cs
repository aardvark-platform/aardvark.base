using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    /// <summary>
    /// "Design by contract" helpers for pre-conditions.
    /// </summary>
    public static class Requires
    {
		class ValidatedNotNullAttribute : Attribute { }

		public static void NotNull([ValidatedNotNull]object o)
        {
            if (o == null) throw new ArgumentNullException("Requires.NotNull(object) failed.");
        }

		public static void NotNull([ValidatedNotNull]params object[] o)
        {
            if (o.Any(x => x == null)) throw new ArgumentNullException("Requires.NotNull(object) failed.");
        }

		public static void NotNull([ValidatedNotNull]object o, string description, params object[] args)
        {
            if (o == null) throw new ArgumentNullException("Requires.NotNull(object) failed with: " + string.Format(description, args));
        }

        public static void IsNull(object o)
        {
            if (o != null) throw new ArgumentNullException("Requires.IsNull(object) failed.");
        }

        public static void IsNull(params object[] o)
        {
            if (o.Any(x => x != null)) throw new ArgumentNullException("Requires.IsNull(object) failed.");
        }

        public static void IsNull(object o, string description, params object[] args)
        {
            if (o != null) throw new ArgumentNullException("Requires.IsNull(IsNull) failed with: " + string.Format(description, args));
        }

        /// <summary>
        /// Requires that string is not null or empty.
        /// </summary>
        public static void NotEmpty(string s)
        {
            if (string.IsNullOrEmpty(s)) throw new ArgumentException("Requires.NotEmpty(string) failed.");
        }

        public static void NotEmpty(string s, string description, params object[] args)
        {
            if (string.IsNullOrEmpty(s)) throw new ArgumentException("Requires.NotEmpty(string) failed with: " + string.Format(description, args));
        }

        public static void That(bool ok)
        {
            if (!ok) throw new InvalidOperationException("Requirement violated.");
        }

        public static void That(bool ok, string description, params object[] args)
        {
            if (!ok) throw new InvalidOperationException("Requires.That(bool) failed with: " + string.Format(description, args));
        }



        public static IEnumerable<T> RequiresThat<T>(this IEnumerable<T> self, Func<T, bool> isOkFunc)
        {
            foreach (var x in self)
                if (isOkFunc(x)) yield return x;
                else throw new InvalidOperationException("Requirement violated.");
        }
    }

    /// <summary>
    /// "Design by contract" helpers for post-conditions.
    /// </summary>
    public static class Ensures
    {
        public static void NotNull(object o)
        {
            if (o == null) throw new ArgumentNullException("Ensures.NotNull(object) failed.");
        }

        public static void NotNull(params object[] o)
        {
            if (o.Any(x => x == null)) throw new ArgumentNullException("Ensures.NotNull(object) failed.");
        }

        public static void NotNull(object o, string description, params object[] args)
        {
            if (o == null) throw new ArgumentNullException("Ensures.NotNull(object) failed with: " + string.Format(description, args));
        }

        public static void NotEmpty(string s)
        {
            if (string.IsNullOrEmpty(s)) throw new ArgumentException("Ensures.NotEmpty(string) failed.");
        }

        public static void NotEmpty(string s, string description, params object[] args)
        {
            if (string.IsNullOrEmpty(s)) throw new ArgumentException("Ensures.NotEmpty(string) failed with: " + string.Format(description, args));
        }

        public static void That(bool ok)
        {
            if (!ok) throw new InvalidOperationException("Ensures.That(bool) failed.");
        }

        public static void That(bool ok, string description, params object[] args)
        {
            if (!ok) throw new InvalidOperationException("Ensures.That(bool) failed with: " + string.Format(description, args));
        }
    }
}
