@echo off
dotnet build src\CodeGenerator\CodeGenerator.csproj
dotnet bin\Debug\net10.0\CodeGenerator.dll %*
