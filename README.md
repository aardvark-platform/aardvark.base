[![Build Status](https://travis-ci.org/aardvark-platform/aardvark.base.svg?branch=master)](https://travis-ci.org/aardvark-platform/aardvark.base)
[![Build status](https://ci.appveyor.com/api/projects/status/px8242ird5aa6svs/branch/master?svg=true)](https://ci.appveyor.com/project/haraldsteinlechner/aardvark/branch/master)

Aardvark.Base is part of the open-source [Aardvark platform](https://github.com/aardvark-platform/aardvark.docs) for visual computing, real-time graphics and visualization.

## Docs and Tutorials

[Getting Started](https://github.com/vrvis/aardvark/wiki)

[Example Code and Demos](https://github.com/vrvis/aardvark.rendering/tree/master/src/Demo/Examples)

[Tutorial: Terrain Generator](https://aszabo314.github.io/stuff/terraingenerator.html)

[Shaders: FShade](http://www.fshade.org/)

[Game: Samy the Salmon](https://github.com/gnufu/SamyTheSalmon)

## How to build

Windows:
- Visual Studio 2015,
- Visual FSharp Tools installed (we use 4.0 now) 
- run build.cmd which will install all dependencies
- msbuild src\Aardvark.sln or use VisualStudio to build the solution

Linux:
- install mono >= 4.2.3.0 (might work in older versions as well)
- install fsharp 4.0 (http://fsharp.org/use/linux/)
- run build.sh which will install all dependencies
- run xbuild src/Aardvark.sln

A visual studio solution is located at: src/Aardvark.sln.
While building, visual studio might prompt for permission to use our type providers. Unfortunately,
the build fails immediately afterwards no matter if you granted permission. In this case
simply rebuild all again and it should work this time.

## License
Aardvark libraries are free to use: [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0.txt).
