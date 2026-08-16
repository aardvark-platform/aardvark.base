# Aardvark.Base Utilities Reference

Reporting, telemetry, random, traversal, and geodesy APIs.

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

## Awaitables

`Awaitable<T>` is a thread-safe, one-shot completion source. The first `Emit`
wins, `Result` blocks until completion, and subscriptions accepted before or
after completion run exactly once with the published value. Continuations run
synchronously outside internal synchronization.

## Ordered Hash Combination

`HashCode.GetCombinedHashCode<T>` uses the same order-sensitive fold for arrays
and `IEnumerable<T>` values. Empty inputs return zero, singletons return the
element hash directly, and each later element hash is incorporated with
`HashCode.UCombine`. The enumerable overload consumes the sequence once and
disposes its enumerator, so the same ordered values hash identically regardless
of whether the caller exposes them as an array, list, or lazy sequence.

## Introspection Method Queries

`Introspection.GetAllMethodsWithAttribute<T>(Assembly)` scans public instance
and static methods declared directly by each assembly type. Each matching
`MethodInfo` is returned once together with all attached `T` attribute instances.

Method queries use a versioned cache discriminator that includes the attribute's
assembly-qualified name, keeping them separate from type queries and older method
semantics. Cache files store one assembly-qualified name per declaring type in
first-seen order. Reads deduplicate those lines and reject resolved types from any
assembly other than the one being queried. Older cache keys are ignored and
rebuilt on the next query.

## Random

`RandomSystem` implements `IRandomUniform`.

```csharp
var rnd = new RandomSystem(1);
int raw = rnd.UniformInt();
int bounded = rnd.UniformInt(100);   // extension method on IRandomUniform
double u = rnd.UniformDouble();
```

`Randomize` uses an allocation-free Fisher-Yates shuffle to uniformly permute
arrays, lists, prefixes, and ranges in place. Elements outside a selected range
are unchanged, and empty or singleton selections consume no random values.
`CreatePermutationArray` and `CreatePermutationArrayLong` use the same shuffle.
Do not rely on an exact permutation for a given seed remaining stable across
library versions.

Geometric sampling takes an `IRandomSeries` (e.g. `HaltonRandomSeries`), not an `IRandomUniform`:

```csharp
var series = new HaltonRandomSeries(2, rnd);

var dir = RandomSample.Spherical(series, 0);
var hemi = RandomSample.Lambertian(V3d.ZAxis, series, 0);
var disk = RandomSample.Disk(series, 0);
```

Alternatively, use the `(double x1, double x2)` overloads with raw uniform samples.

Low-discrepancy:

```csharp
double q = Quasi.QuasiHaltonWithIndex(2, 0.123);
```

## INode Traversal

`INode` extensions:

- `ComputeDepth()`
- `DepthFirst()`
- `BreadthFirst()`
- `NodesAtDepth(depth)`
- `DescendentsAndSelf()` and `Descendents()` (spelling in code is `Descendents`)

`ComputeDepth()` treats leaves as depth zero and enumerates each node's
`SubNodes` sequence once. `DepthFirst()` is iterative preorder traversal; it
owns each child enumerator and disposes it after exhaustion, early termination,
or traversal failure.

## Path Utilities

`Aardvark.Base.Coder.Dir.RelativeDir` and `RelativeFile` compute lexical paths
without checking path existence or resolving symbolic links. Path roots must be
compatible. Roots and components use ordinal case-insensitive comparison on
Windows and ordinal case-sensitive comparison on other platforms.

`RelativeDir` returns an empty string for identical directories and appends the
platform directory separator to every non-empty result. Incompatible roots
return `null`; the `TryGetRelative*` helpers then select the requested absolute
path or empty fallback.

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
- `src/Aardvark.Base.Essentials/System/Awaitable.cs`
- `src/Aardvark.Base/Hashing/HashCode.cs`
- `src/Aardvark.Base/Introspection/Introspection.cs`
- `src/Aardvark.Base/Random/RandomSystem.cs`
- `src/Aardvark.Base/Random/IRandomUniform.cs`
- `src/Aardvark.Base/Random/RandomSample.cs`
- `src/Aardvark.Base/Random/HaltonRandomSeries.cs`
- `src/Aardvark.Base/Random/Quasi.cs`
- `src/Aardvark.Base/AlgoDat/INode.cs`
- `src/Aardvark.Base/Extensions/DagExtensions.cs`
- `src/Aardvark.Base.IO/WorkDir.cs`
- `src/Aardvark.Base/Geodesy/GeoConversion.cs`
- `src/Aardvark.Base/Geodesy/GeoConsts.cs`
- `src/Aardvark.Base/Math/Base/Constant.cs`
