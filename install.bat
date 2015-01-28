@echo off
cls
PUSHD src
NuGet.exe restore -ConfigFile nuget.config  -NonInteractive
POPD
