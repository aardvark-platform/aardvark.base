[![Build Status linux](https://github.com/aardvark-platform/aardvark.base/workflows/linux/badge.svg)](https://github.com/aardvark-platform/aardvark.base/actions?query=workflow%3A%22linux%22)
[![Build Status windows](https://github.com/aardvark-platform/aardvark.base/workflows/windows/badge.svg)](https://github.com/aardvark-platform/aardvark.base/actions?query=workflow%3A%22windows%22)
[![Join the chat at https://gitter.im/aardvark-platform/Lobby](https://img.shields.io/badge/gitter-join%20chat-blue.svg)](https://gitter.im/aardvark-platform/Lobby)
[![license](https://img.shields.io/github/license/aardvark-platform/aardvark.base.svg)](https://github.com/aardvark-platform/aardvark.base/blob/master/LICENSE)

[The Aardvark Platform](https://aardvarkians.com/) |
[Platform Wiki](https://github.com/aardvarkplatform/aardvark.docs/wiki) | 
[The Platform Walkthrough Repository](https://github.com/aardvark-platform/walkthrough) |
[Gallery](https://github.com/aardvarkplatform/aardvark.docs/wiki/Gallery) | 
[Quickstart](https://github.com/aardvarkplatform/aardvark.docs/wiki/Quickstart-Windows) | 
[Status](https://github.com/aardvarkplatform/aardvark.docs/wiki/Status)

Aardvark.Base provides multiple, standalone nuget packages constituting a set of essential tools often required in computer graphics and functional programming such as vectors, matrices as well as algorithms and datastructures.
Aardvark.Base is part of the open-source [Aardvark platform](https://github.com/aardvark-platform/aardvark.docs/wiki) for visual computing, real-time graphics and visualization. Base is the base for the rendering engine, functional application libraries and many projects:


![Alt text](./data/context.svg)


Some highlighted (netstandard 2.0 compatible) packages of this repository are:
 - [Aardvark.Base](https://www.nuget.org/packages/Aardvark.Base/): matrices, vectors, extensions, clipping and geometry, basic Algorithms and data structures.
 - [Aardvark.Base.FSharp](https://www.nuget.org/packages/Aardvark.Base.FSharp/): stuff you always need, optimized persistent (e.g. hash maps), ephemeral data structures (e.g. SkipList) as well as spatial data structures (e.g. bounding volume hierarchies). The package also contains an attribute grammar system exposed as embedded domain specific language. We use it in [aardvark.rendering](https://github.com/aardvark-platform/aardvark.base) for our scene graph system, as described in [Attribute Grammars for Incremental Scene Graph Rendering](https://www.vrvis.at/publications/pdfs/PB-VRVis-2019-004.pdf).
 - [Aardvark.Base.Incremental](https://www.nuget.org/packages/Aardvark.Base.Incremental/): incremental data structures similarly but extended to Hammer et al.'s paper [Adapton: Composable, Demand-Driven Incremental Computation](https://www.cs.umd.edu/~hammer/adapton/). Additionally to modifiable cells, we have more sophisticated optimized incremental data structures such as adaptive sets, maps etc. and computation expression builders to conveniently work with.
 - [Aardvark.Base.Runtime](https://www.nuget.org/packages/Aardvark.Base.Runtime/): Crazy tools such as an AMD64 assembler used for incremental Just In Time Compilation as used in [aardvark.rendering](https://github.com/aardvark-platform/aardvark.base)
 - [Aardvark.Data.Vrml97](https://www.nuget.org/packages/Aardvark.Data.Vrml97/): Vrml97 legacy parser
 - [Aardvark.Geometry](https://www.nuget.org/packages/Aardvark.Geometry/): now rather small set of F# geometry tools. Most stuff regarding geometry is still in base and [algodat](https://github.com/aardvark-platform/aardvark.algodat)

Those packages are under permissive license, for support take a look at [aardvarkians](https://aardvarkians.com).
