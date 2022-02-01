@echo off
dotnet build src\CodeGenerator\CodeGenerator.csproj
dotnet build src\Aardvark.Base\Aardvark.Base.csproj
bin\Debug\net471\CodeGenerator.exe