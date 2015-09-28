#!/bin/bash

if [ ! -d "packages/FAKE" ]; then
	echo "downloading FAKE"
	mono --runtime=v4.0 bin/nuget.exe install FAKE -OutputDirectory packages -Version 3.35.2 -ExcludeVersion
	mono --runtime=v4.0 bin/nuget.exe install Paket.Core -OutputDirectory packages -Version 1.18.5 -ExcludeVersion 
fi


mono packages/FAKE/tools/FAKE.exe "build.fsx"  $@
