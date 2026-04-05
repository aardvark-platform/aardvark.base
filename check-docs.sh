#!/bin/bash
set -euo pipefail

dotnet run --project tools/DocsChecker/DocsChecker.csproj -c Release
