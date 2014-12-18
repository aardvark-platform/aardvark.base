@echo off
cls
PUSHD
cd src
NuGet.exe restore
POPD
pause