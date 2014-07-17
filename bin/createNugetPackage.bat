msbuild ../src/Aardvark.Base/Aardvark.Base.csproj /t:Build /p:Configuration="Release 4.0"
msbuild ../src/Aardvark.Base/Aardvark.Base.csproj /t:Build /p:Configuration="Release 4.5"
nuget pack Aardvark.Base.nuspec