@echo off

PUSHD %~dp0
REM cls

IF exist packages\FAKE ( echo skipping FAKE download ) ELSE ( 
echo downloading FAKE
REM dir
"bin\nuget.exe" "install" "FAKE" "-Version" "3.35.2" "-OutputDirectory" "Packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "FSharp.Formatting.CommandTool" "-OutputDirectory" "Packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "SourceLink.Fake" "-OutputDirectory" "Packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "NUnit.Runners" "-OutputDirectory" "Packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "Aardvark.Build" "-OutputDirectory" "Packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "Paket.Core" "-Version" "1.18.5" "-OutputDirectory" "packages" "-ExcludeVersion"
)

SET TARGET=Default
IF NOT [%1]==[] (set TARGET=%1)

>tmp ECHO(%*
SET /P t=<tmp
SETLOCAL EnableDelayedExpansion
IF DEFINED t SET "t=!t:%1 =!"
SET args=!t!
del tmp

"packages\FAKE\tools\Fake.exe" "build.fsx" "target=%TARGET%" %args%


