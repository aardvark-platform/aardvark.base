@echo off
SETLOCAL
PUSHD %~dp0

IF NOT exist .paket\paket.exe (
	dotnet tool install Paket --tool-path .paket
)

if NOT exist paket.lock (
    echo No paket.lock found, running paket install.
    .paket\paket.exe install
)

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

dotnet packages\build\fake-cli\tools\netcoreapp2.1\any\fake-cli.dll build %* 



