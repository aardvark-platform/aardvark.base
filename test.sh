#!/bin/bash

# Intentionally runs only the maintained test projects.
# The deprecated incremental test project is excluded on purpose; see issue #94.
dotnet tool restore
dotnet paket restore
dotnet test src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj
dotnet test src/Tests/Aardvark.Base.Runtime.Tests/Aardvark.Base.Runtime.Tests.fsproj
dotnet test src/Tests/Aardvark.Base.Fonts.Tests/Aardvark.Base.Fonts.Tests.fsproj
dotnet test src/Tests/Aardvark.Geometry.Tests/Aardvark.Geometry.Tests.fsproj
dotnet test src/Tests/Aardvark.Base.FSharp.Tests/Aardvark.Base.FSharp.Tests.fsproj
