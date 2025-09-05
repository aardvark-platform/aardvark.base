#!/bin/bash

dotnet tool restore
dotnet paket restore
dotnet test src/Aardvark.sln