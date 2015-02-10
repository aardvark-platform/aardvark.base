Aardvark.Base is a collection of algorithms and data structures for computer graphics and visualization. It is under active development since 2006 as part of a much larger set of libraries at VRVis Research Center.

Getting started: https://github.com/vrvis/aardvark/wiki

License: http://www.apache.org/licenses/LICENSE-2.0.txt

Copyright © 2014 VRVis Zentrum für Virtual Reality und Visualisierung Forschungs-GmbH, Donau-City-Strasse 1, A-1220 Wien, Austria. http://www.vrvis.at.

How to build:
======================

Windows:
- Visual Studio 2013,
- FSharp 3.1 (at least Daily Builds Preview 10-27-2014) [1]
- run install.bat
- run msbuild src\Aardvark.sln or use VisualStudio to build the solution

Linux:
- install mono >= 3.2.8 (might work in older versions as well)
- install fsharp 3.1 (http://fsharp.org/use/linux/)
- run install.sh (this restores all nuget packages in ./packages which is referenced by the projects)
- run xbuild src/Aardvark.sln

A visual studio solution is located at: src/Aardvark.sln.
While building, visual studio might prompt for permission to use our type providers. Unfortunately,
the build fails immediately afterwards no matter if you granted permission. In this case
simply rebuild all again and it should work this time.

CI, linux build: [![Build Status](https://travis-ci.org/vrvis/aardvark.svg?branch=master)](https://travis-ci.org/vrvis/aardvark)

CI, windows build: [![Build status](https://ci.appveyor.com/api/projects/status/px8242ird5aa6svs/branch/master?svg=true)](https://ci.appveyor.com/project/haraldsteinlechner/aardvark/branch/master)


[1] https://visualfsharp.codeplex.com/releases/view/161288
