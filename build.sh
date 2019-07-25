#!/bin/bash

if [ ! -f .paket/paket ]; then
    dotnet tool install Paket --tool-path .paket
fi

./.paket/paket restore 

dotnet packages/build/fake-cli/tools/netcoreapp2.1/any/fake-cli.dll build $@
