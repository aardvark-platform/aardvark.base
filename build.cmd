@echo off
dotnet tool restore
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
REM `dotnet paket restore` alone does not recreate Paket.Restore.targets if it is missing.
IF NOT EXIST ".paket\Paket.Restore.targets" (
    dotnet paket install
    IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
) ELSE (
    dotnet paket restore
    IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
)

IF "%1"=="restore" exit /B

dotnet build src\Aardvark.sln
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
