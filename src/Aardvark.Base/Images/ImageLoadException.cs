using System;

namespace Aardvark.Base
{
    public class ImageLoadException : Exception
    {
        public ImageLoadException(string message)
            : base(message)
        {  }

        public ImageLoadException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
