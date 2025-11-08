@echo off
dotnet tool restore
.paket\paket.exe restore

IF "%1"=="restore" exit /B

dotnet build src\Aardvark.sln
