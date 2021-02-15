![Windows](https://github.com/aardvark-platform/aardvark.base/workflows/Windows/badge.svg)
![MacOS](https://github.com/aardvark-platform/aardvark.base/workflows/MacOS/badge.svg)
![Linux](https://github.com/aardvark-platform/aardvark.base/workflows/Linux/badge.svg)


[![Join the chat at https://gitter.im/aardvark-platform/Lobby](https://img.shields.io/badge/gitter-join%20chat-blue.svg)](https://gitter.im/aardvark-platform/Lobby)
[![license](https://img.shields.io/github/license/aardvark-platform/aardvark.base.svg)](https://github.com/aardvark-platform/aardvark.base/blob/master/LICENSE)

[The Aardvark Platform](https://aardvarkians.com/) |
[Platform Wiki](https://github.com/aardvarkplatform/aardvark.docs/wiki) | 
[The Platform Walkthrough Repository](https://github.com/aardvark-platform/walkthrough) |
[Gallery](https://github.com/aardvarkplatform/aardvark.docs/wiki/Gallery) | 
[Quickstart](https://github.com/aardvarkplatform/aardvark.docs/wiki/Quickstart-Windows) | 
[Status](https://github.com/aardvarkplatform/aardvark.docs/wiki/Status)


Aardvark.Base consists of multiple platform-independent packages (netstandard2.0) delivering essential tools for visual computing, such as vectors and matrices, as well as many algorithms and data structures.
It is the lowest-level foundation of the open-source [Aardvark Platform](https://github.com/aardvark-platform/aardvark.docs/wiki) for visual computing, real-time graphics and visualization:

repository | description
:-- | --- |
`aardvark.media` | a unified ELM-style UI framework for both 2D and 3D |
`aardvark.rendering` | powerful incremental rendering engine |
`aardvark.base` | math, geometry, algorithms, data structures |

The repository `aardvark.base` includes many packages, e.g.
 - [Aardvark.Base](https://www.nuget.org/packages/Aardvark.Base/): matrices, vectors, geometry, basic algorithms and data structures.
 - [Aardvark.Base.FSharp](https://www.nuget.org/packages/Aardvark.Base.FSharp/): stuff you always need, optimized persistent (e.g. hash maps), ephemeral data structures (e.g. SkipList) as well as spatial data structures (e.g. bounding volume hierarchies). The package also contains an attribute grammar system exposed as an embedded domain specific language. We use it in [aardvark.rendering](https://github.com/aardvark-platform/aardvark.base) for our scene graph system, as described in [Attribute Grammars for Incremental Scene Graph Rendering](https://www.vrvis.at/publications/pdfs/PB-VRVis-2019-004.pdf).
 - [Aardvark.Base.Incremental](https://www.nuget.org/packages/Aardvark.Base.Incremental/): incremental data structures similarly but extended to Hammer et al.'s paper [Adapton: Composable, Demand-Driven Incremental Computation](https://www.cs.umd.edu/~hammer/adapton/). Additionally to modifiable cells, we have more sophisticated optimized incremental data structures such as adaptive sets, maps etc. and computation expression builders to conveniently work with.
 - [Aardvark.Base.Runtime](https://www.nuget.org/packages/Aardvark.Base.Runtime/): Crazy tools such as an AMD64 assembler used for incremental Just In Time Compilation as used in [aardvark.rendering](https://github.com/aardvark-platform/aardvark.base)
 - [Aardvark.Data.Vrml97](https://www.nuget.org/packages/Aardvark.Data.Vrml97/): legacy Vrml97 parser
 - [Aardvark.Geometry](https://www.nuget.org/packages/Aardvark.Geometry/): currently a rather small set of F# geometry tools. Most functionality regarding geometry lives in base and [algodat](https://github.com/aardvark-platform/aardvark.algodat)

All packages are distributed under the [Apache 2.0 license](https://github.com/aardvark-platform/aardvark.base/blob/master/LICENSE).

For support please have a look at [Aardvarkians](https://aardvarkians.com).