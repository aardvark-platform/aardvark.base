#!/bin/bash

mono .paket/paket.bootstrapper.exe
mono .paket/paket.exe restore group Build


#mono packages/build/FAKE/tools/FAKE.exe "build.fsx" Dummy --fsiargs build.fsx $@
echo "currently our build script does not work on mono >= 5.0. for that reason we use vanilla builds (which makes addsource etc unavailable in that form. you need to use fsi sessions for portions of the build script which fork. please do so and create issues describing the mono/fsharp issues)"
mono .paket/paket.exe install
msbuild src/Aardvark.sln
