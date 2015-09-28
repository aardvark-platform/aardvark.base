#!/bin/bash

if [ ! -d "packages/FAKE" ]; then
	echo "downloading FAKE"
	mono --runtime=v4.0 bin/nuget.exe install FAKE -OutputDirectory packages -ExcludeVersion
	mono --runtime=v4.0 bin/nuget.exe install FSharp.Formatting.CommandTool -OutputDirectory packages -ExcludeVersion -Prerelease 
	mono --runtime=v4.0 bin/nuget.exe install SourceLink.Fake -OutputDirectory packages -ExcludeVersion 
	mono --runtime=v4.0 bin/nuget.exe install Aardvark.Build -OutputDirectory packages -ExcludeVersion 
fi

wget -q --no-check-certificate https://github.com/vrvis/Aardvark.Fake/raw/master/bin/Aardvark.Fake.dll -O bin/Aardvark.Fake.dll

mono packages/FAKE/tools/FAKE.exe "build.fsx"  $@

