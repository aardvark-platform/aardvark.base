@echo off
echo %~dp0

PUSHD %~dp0
cls

IF exist packages\FAKE ( echo skipping FAKE download ) ELSE ( 
echo downloading FAKE
"bin\nuget.exe" "install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion" "-Prerelease"
"bin\nuget.exe" "install" "FSharp.Formatting.CommandTool" "-OutputDirectory" "packages" "-ExcludeVersion" "-Prerelease"
"bin\nuget.exe" "install" "SourceLink.Fake" "-OutputDirectory" "packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "NUnit.Runners" "-OutputDirectory" "packages" "-ExcludeVersion"
)

SET TARGET="Default"

IF NOT [%1]==[] (set TARGET="%1")

"packages\FAKE\tools\Fake.exe" "build.fsx" "target=%TARGET%"
