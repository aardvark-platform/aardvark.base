#!/bin/bash

dotnet tool restore
# `dotnet paket restore` alone does not recreate Paket.Restore.targets if it is missing.
if [ ! -f ".paket/Paket.Restore.targets" ]; then
    dotnet paket install
else
    dotnet paket restore
fi

if [ "$1" = "restore" ]; then
    exit 0
fi

dotnet build src/Aardvark.sln
