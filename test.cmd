@echo off
REM Intentionally runs only the maintained test projects.
REM The deprecated incremental test project is excluded on purpose; see issue #94.
dotnet tool restore
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
REM `dotnet paket restore` alone does not recreate Paket.Restore.targets if it is missing.
IF NOT EXIST ".paket\Paket.Restore.targets" (
    dotnet paket install
    IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
) ELSE (
    dotnet paket restore
    IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
)
dotnet test src\Tests\Aardvark.Base.Tests\Aardvark.Base.Tests.csproj
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
dotnet test src\Tests\Aardvark.Base.Runtime.Tests\Aardvark.Base.Runtime.Tests.fsproj
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
dotnet test src\Tests\Aardvark.Base.Fonts.Tests\Aardvark.Base.Fonts.Tests.fsproj
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
dotnet test src\Tests\Aardvark.Geometry.Tests\Aardvark.Geometry.Tests.fsproj
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
dotnet test src\Tests\Aardvark.Base.FSharp.Tests\Aardvark.Base.FSharp.Tests.fsproj
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
