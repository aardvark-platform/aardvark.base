using System;

namespace Aardvark.Base
{
    /// <summary>
    /// Exception that is thrown when loading or saving an image fails.
    /// </summary>
    public class ImageLoadException : Exception
    {
        /// <summary>
        /// Creates a new ImageLoadException with the given error message.
        /// </summary>
        /// <param name="message">A description of the error.</param>
        public ImageLoadException(string message)
            : base(message)
        {  }

        /// <summary>
        /// Creates a new ImageLoadException with the given error message and inner exception.
        /// </summary>
        /// <param name="message">A description of the error.</param>
        /// <param name="inner">The exception that caused the current exception.</param>
        public ImageLoadException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
