@echo off
dotnet tool restore
REM `dotnet paket restore` alone does not recreate Paket.Restore.targets if it is missing.
IF NOT EXIST ".paket\Paket.Restore.targets" (
    dotnet paket install
) ELSE (
    dotnet paket restore
)

IF "%1"=="restore" exit /B

dotnet build src\Aardvark.sln
