#!/bin/bash

if [ ! -d "packages/FAKE" ]; then
	echo "downloading FAKE"
	mono --runtime=v4.0 bin/nuget.exe install FAKE -OutputDirectory packages -ExcludeVersion
	mono --runtime=v4.0 bin/nuget.exe install FSharp.Formatting.CommandTool -OutputDirectory packages -ExcludeVersion -Prerelease 
	mono --runtime=v4.0 bin/nuget.exe install SourceLink.Fake -OutputDirectory packages -ExcludeVersion 
fi


mono packages/FAKE/tools/FAKE.exe "build.fsx"  $@

