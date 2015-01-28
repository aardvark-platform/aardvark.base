@echo off
cls
PUSHD
cd src
NuGet.exe restore -ConfigFile nuget.config  -NonInteractive
POPD
pause
