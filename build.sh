#!/bin/bash

mono .paket/paket.bootstrapper.exe
mono .paket/paket.exe restore group Build

mono packages/Build/FAKE/tools/Fake.exe "build.fsx" Dummy --fsiargs build.fsx $@