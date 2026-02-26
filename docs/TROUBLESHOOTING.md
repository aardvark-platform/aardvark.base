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

If still broken:

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
dotnet test src/Aardvark.sln --filter "FullyQualifiedName~Vector"
```

`test.sh` / `test.cmd` do not forward filter arguments.

## Script execution on macOS/Linux

If scripts are not executable:

```bash
chmod +x build.sh test.sh generate.sh check-docs.sh
```

## Line ending/script issues across OSes

If shell scripts break after editing on Windows, normalize line endings through Git configuration and recommit affected files.
