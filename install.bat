@echo off
cls
PUSHD
cd src
NuGet.exe restore -ConfigFile nuget.config
POPD
pause
