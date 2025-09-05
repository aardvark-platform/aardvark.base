#!/bin/bash

dotnet tool restore
dotnet paket restore

if [ "$1" = "restore" ]; then
    exit 0
fi

dotnet build src/Aardvark.sln