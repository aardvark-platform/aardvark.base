using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public enum ShaderStage
    {
        Vertex,
        TessControl,
        TessEval,
        Geometry,
        Pixel
    }

    public interface ISurface
    {

    }

    public interface IBackendSurface : ISurface
    {
        string Code { get; }
        Dictionary<ShaderStage, string> EntryPoints { get; }
    }


}
