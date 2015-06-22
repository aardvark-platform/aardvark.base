@echo off

PUSHD %~dp0
REM cls

IF exist packages\FAKE ( echo skipping FAKE download ) ELSE ( 
echo downloading FAKE
"bin\nuget.exe" "install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion" "-Prerelease"
"bin\nuget.exe" "install" "FSharp.Formatting.CommandTool" "-OutputDirectory" "packages" "-ExcludeVersion" "-Prerelease"
"bin\nuget.exe" "install" "SourceLink.Fake" "-OutputDirectory" "packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "NUnit.Runners" "-OutputDirectory" "packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "Aardvark.Build" "-OutputDirectory" "packages" "-ExcludeVersion"
)

SET TARGET=Default
IF NOT [%1]==[] (set TARGET=%1)

>tmp ECHO(%*
SET /P t=<tmp
SETLOCAL EnableDelayedExpansion
IF DEFINED t SET "t=!t:%1 =!"
SET args=!t!

"packages\FAKE\tools\Fake.exe" "build.fsx" "target=%TARGET%" %args%
RM tmp
