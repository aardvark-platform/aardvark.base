# Troubleshooting

Common issues for building/testing Aardvark.Base.

## Paket restore fails

Symptoms:

- `dotnet paket restore` errors
- tool/package mismatch

Try:

```bash
dotnet tool restore
dotnet paket restore
```

The top-level `build.sh`, `build.cmd`, `test.sh`, and `test.cmd` scripts already handle the common missing-targets bootstrap case automatically: if `.paket/Paket.Restore.targets` is missing, they run `dotnet paket install`; otherwise they use `dotnet paket restore`.
They also stop on the first failing restore/build/test command and return a nonzero exit code instead of continuing to later steps.

If you are restoring manually or the targets file is already missing:

```bash
dotnet paket install
```

## Generated code is out of sync

Symptoms:

- compile errors in `*_auto.cs` / `*_auto.fs`

Fix:

```bash
# Windows
.\generate.cmd

# Linux/macOS
./generate.sh
```

## SDK/framework mismatch

Symptoms:

- build fails on target framework resolution

Check:

```bash
dotnet --info
```

Ensure SDK matches `global.json` expectations.

## Test filter returns zero tests

Use `dotnet test` directly with a fully-qualified-name filter:

```bash
dotnet test src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj --filter "FullyQualifiedName~Vector"
```

`test.sh` / `test.cmd` do not forward filter arguments, and they run only the real test projects rather than the full solution.

## Script execution on macOS/Linux

If scripts are not executable:

```bash
chmod +x build.sh test.sh generate.sh check-docs.sh
```

## Line ending/script issues across OSes

If shell scripts break after editing on Windows, normalize line endings through Git configuration and recommit affected files.
