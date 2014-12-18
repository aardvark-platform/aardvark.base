msbuild ../src/Aardvark.Base/Aardvark.Base.csproj /t:Build /p:Configuration="Release 4.0"
msbuild ../src/Aardvark.Base/Aardvark.Base.csproj /t:Build /p:Configuration="Release 4.5"
nuget pack Aardvark.Base.nuspec

msbuild ../src/Aardvark.Base.TypeProviders/Aardvark.Base.TypeProviders.fsproj /t:Build /p:Configuration="Release"
msbuild ../src/Aardvark.Base.FSharp/Aardvark.Base.FSharp.fsproj /t:Build /p:Configuration="Release"
nuget pack Aardvark.Base.FSharp.nuspec