Aardvark.Base is a collection of algorithms and data structures for computer graphics and visualization. It is under active development since 2006 as part of a much larger set of libraries at VRVis Research Center.

Getting started: https://github.com/vrvis/aardvark/wiki

License: http://www.apache.org/licenses/LICENSE-2.0.txt

Copyright © 2014 VRVis Zentrum für Virtual Reality und Visualisierung Forschungs-GmbH, Donau-City-Strasse 1, A-1220 Wien, Austria. http://www.vrvis.at.

How to build:
======================

Prerequisites:
Visual Studio 2013, newest fsharp tools (at least Daily Builds Preview 10-27-2014) [1]

We use some NuGet packages. To install them while keeping the VisualStudio build
system sane please run install.bat/install.sh to install those.

A visual studio solution is located at: src/Aardvark.sln.
While building, visual studio might prompt for permission to use our type providers. Unfortunately,
the build fails immediately afterwards no matter if you granted permission. In this case
simply rebuild all again and it should work this time.


[1] https://visualfsharp.codeplex.com/