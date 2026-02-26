# Aardvark.Base Utilities Reference

Source-verified orientation for reporting, telemetry, random, traversal, and geodesy APIs.

## Report

`Report` is global process state.

Common methods:

```csharp
Report.Line("msg");
Report.Warn("msg");
Report.Debug("msg");
Report.Trace("msg");
Report.Error("msg");
Report.Fatal("msg");

Report.BeginTimed("load");
// ...
Report.End();

Report.Progress(0.5);
Report.ProgressDelta(0.1);
```

Key settings:

```csharp
Report.Verbosity = 2;
Report.MultiThreaded = true;
Report.ThrowOnError = false;
```

## Telemetry

Core probe types:

- `Telemetry.Counter`
- `Telemetry.StopWatchTime`
- `Telemetry.WallClockTime`
- `Telemetry.CpuTime`

Registration:

```csharp
var c = new Telemetry.Counter();
Telemetry.Register("Frames", c);
Telemetry.Register("Frames/s", c.RatePerSecond());
```

Reset API:

```csharp
Telemetry.ResetTelemetrySystem();
```

## Random

`RandomSystem` implements `IRandomUniform`.

```csharp
var rnd = new RandomSystem(1);
int raw = rnd.UniformInt();
int bounded = rnd.UniformInt(100);   // extension method on IRandomUniform
double u = rnd.UniformDouble();
```

Geometric sampling:

```csharp
var dir = RandomSample.Spherical(rnd, 0);
var hemi = RandomSample.Lambertian(V3d.ZAxis, rnd, 0);
var disk = RandomSample.Disk(rnd, 0);
```

Low-discrepancy:

```csharp
var halton = new HaltonRandomSeries(2, rnd);
double q = Quasi.QuasiHaltonWithIndex(2, 0.123);
```

## INode Traversal

`INode` extensions:

- `DepthFirst()`
- `BreadthFirst()`
- `NodesAtDepth(depth)`
- `DescendentsAndSelf()` and `Descendents()` (spelling in code is `Descendents`)

## Geodesy

Main conversions:

```csharp
var xyz = Geo.XyzFromLonLatHeight(new V3d(lonDeg, latDeg, hMeters), GeoEllipsoid.Wgs84);
var llh = Geo.LonLatHeightFromXyz(xyz, GeoEllipsoid.Wgs84);
```

`GeoEllipsoid` presets include `Wgs84`, `Grs80`, `Bessel1841`.

## Constants

`Constant<T>` exposes machine-epsilon/tiny/parseable min/max style values.

Mathematical constants are on non-generic classes:

- `Constant.Pi`, `Constant.E`
- `ConstantF.Pi`, `ConstantF.E`

## Source Anchors

- `src/Aardvark.Base/Reporting/Report.cs`
- `src/Aardvark.Base.Telemetry/Probes.cs`
- `src/Aardvark.Base.Telemetry/Registry.cs`
- `src/Aardvark.Base.Telemetry/IProbe.cs`
- `src/Aardvark.Base.Telemetry/TelemetryExtensions.cs`
- `src/Aardvark.Base/Random/RandomSystem.cs`
- `src/Aardvark.Base/Random/IRandomUniform.cs`
- `src/Aardvark.Base/Random/RandomSample.cs`
- `src/Aardvark.Base/Random/HaltonRandomSeries.cs`
- `src/Aardvark.Base/Random/Quasi.cs`
- `src/Aardvark.Base/AlgoDat/INode.cs`
- `src/Aardvark.Base/Geodesy/GeoConversion.cs`
- `src/Aardvark.Base/Geodesy/GeoConsts.cs`
- `src/Aardvark.Base/Math/Base/Constant.cs`
