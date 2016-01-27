@echo off
SETLOCAL
PUSHD %~dp0

bin\wget.exe -q --no-check-certificate https://github.com/vrvis/Aardvark.Fake/blob/paket_2_47/bin/Aardvark.Fake.dll -O bin/Aardvark.Fake.dll

cls

.paket\paket.bootstrapper.exe
if errorlevel 1 (
  exit /b %errorlevel%
)

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

SET FAKE_PATH=packages\build\FAKE\tools\Fake.exe

IF [%1]==[] (
    "%FAKE_PATH%" "build.fsx" "Default" 
) ELSE (
    "%FAKE_PATH%" "build.fsx" %* 
) 

