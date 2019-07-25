#!/bin/bash

mono .paket/paket.bootstrapper.exe
mono .paket/paket.exe restore 

dotnet packages/build/fake-cli/tools/netcoreapp2.1/any/fake-cli.dll build $@
