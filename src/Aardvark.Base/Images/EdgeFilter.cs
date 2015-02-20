using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public enum EdgeFilter : int
    {
        Sharr, Sobel, Roberts, Simoncelli,
    }

    public enum EdgeDirection : int
    {
        Horizontal, Vertical, Both,
    }
}
