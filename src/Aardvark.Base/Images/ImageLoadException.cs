using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
