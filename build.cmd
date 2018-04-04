@echo off
SETLOCAL
PUSHD %~dp0


.paket\paket.bootstrapper.exe
if errorlevel 1 (
  exit /b %errorlevel%
)

if NOT exist paket.lock (
    echo No paket.lock found, running paket install.
    .paket\paket.exe install
)

.paket\paket.exe restore --group Build
if errorlevel 1 (
  exit /b %errorlevel%
)

dotnet restore src\Aardvark.Base.Extensions
dotnet restore src\Aardvark.Base.Delegates
dotnet restore src\Aardvark.Base.Telemetry
dotnet restore src\Aardvark.Base.Reporting
dotnet restore src\Aardvark.Base.TextParser
dotnet restore src\Aardvark.Base.Symbol

SET FSI_PATH=packages\build\FAKE\tools\Fake.exe
"%FSI_PATH%" "build.fsx" Dummy --fsiargs build.fsx --shadowcopyreferences+ %* 



