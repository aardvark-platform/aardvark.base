using System;

namespace Aardvark.Data.Vrml97
{
    /// <summary>
    /// VRML97 parse exception.
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ParseException(string s)
            : base(s)
        {
        }
    }
}
