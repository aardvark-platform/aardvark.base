@echo off

PUSHD %~dp0

IF exist packages\FAKE ( echo skipping FAKE download ) ELSE ( 
echo downloading FAKE
"bin\nuget.exe" "install" "FAKE" "-Version" "3.35.2" "-OutputDirectory" "Packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "Paket.Core" "-Version" "1.18.5" "-OutputDirectory" "packages" "-ExcludeVersion"
"bin\nuget.exe" "install" "NUnit.Runners" "-Version" "2.6.4"  "-OutputDirectory" "packages" "-ExcludeVersion"
)

bin\wget.exe -q --no-check-certificate https://github.com/vrvis/Aardvark.Fake/raw/master/bin/Aardvark.Fake.dll -O bin/Aardvark.Fake.dll

SET TARGET=Default
IF NOT [%1]==[] (set TARGET=%1)

>tmp ECHO(%*
SET /P t=<tmp
SETLOCAL EnableDelayedExpansion
IF DEFINED t SET "t=!t:%1 =!"
SET args=!t!
del tmp

"packages\FAKE\tools\Fake.exe" "build.fsx" "target=%TARGET%" %args%


