# Aardvark.Base Utilities Reference

AI-targeted reference for logging, telemetry, random sampling, and other utilities.

---

## Report System

Multi-level logging with thread-aware reporting.

```csharp
public static class Report
```

### Verbosity Levels
- Level 0: Critical messages
- Level 1: Important messages
- Level 2: Normal (default console)
- Level 3+: Detailed/debug

### Basic Logging
```csharp
Report.Line("Message");                    // level 0
Report.Line(2, "Detailed message");        // level 2

Report.Warn("Warning message");
Report.Error("Error message");
Report.Fatal("Fatal error");

Report.Debug("Debug info");
Report.Trace("Trace info");
```

### Timed Sections
```csharp
Report.BeginTimed("Processing");
// ... work ...
Report.End();  // prints elapsed time

// With verbosity level
Report.BeginTimed(2, "Loading data");
// ...
Report.End(2);
```

### Nested Sections
```csharp
Report.Begin("Outer");
Report.Line("Info");
Report.Begin("Inner");
Report.Line("More info");
Report.End();  // ends Inner
Report.End();  // ends Outer
```

### Progress Reporting
```csharp
Report.Progress(current, total);
Report.Progress(0.5);  // 50%
```

### Configuration
```csharp
Report.Verbosity = 2;           // set console verbosity
Report.MultiThreaded = true;    // enable per-thread reporting
Report.ThrowOnError = false;    // break on errors in debugger
```

---

## Telemetry System

Performance monitoring with counters, timers, and probes.

```csharp
public static partial class Telemetry
```

### Counter

Thread-safe counter.

```csharp
var counter = new Telemetry.Counter();

counter.Increment();
counter.Increment(5);
counter.Decrement();

long value = counter.Value;
```

### StopWatchTime

Accumulated timing including blocked time.

```csharp
var timer = new Telemetry.StopWatchTime();

using (timer.Timer)
{
    // timed code
}

TimeSpan elapsed = timer.Value;
```

### WallClockTime

Wall-clock time - overlapping measurements from multiple threads don't accumulate.

```csharp
var wallClock = new Telemetry.WallClockTime();

using (wallClock.Timer)
{
    // 4 threads timing 1 second each = 1 second total
}

TimeSpan elapsed = wallClock.Value;
```

### CpuTime

Actual CPU time consumed by thread.

```csharp
var cpuTime = new Telemetry.CpuTime();

using (cpuTime.Timer)
{
    // CPU-bound work
}

TimeSpan cpu = cpuTime.Value;
```

### Registration
```csharp
Telemetry.Register("MyCounter", counter);
Telemetry.Register("MyCounter/s", counter.RatePerSecond());
```

### Reset
```csharp
Telemetry.Reset();  // resets all registered probes
```

---

## Random Sampling

### RandomSystem

Standard random number generation.

```csharp
var rnd = new RandomSystem();
var rnd = new RandomSystem(seed);

double d = rnd.UniformDouble();       // [0, 1)
float f = rnd.UniformFloat();         // [0, 1)
int i = rnd.UniformInt(max);          // [0, max)
int i = rnd.UniformInt(min, max);     // [min, max)
```

### Geometric Sampling
```csharp
// Uniform direction on unit sphere
V3d dir = rnd.UniformV3dDirection();

// Cosine-weighted hemisphere (Lambertian)
V3d dir = rnd.CosineWeightedHemisphere(normal);

// Uniform point on unit sphere
V3d point = rnd.UniformV3dOnSphere();

// Point in unit disk
V2d point = rnd.UniformV2dInDisk();
```

### Halton Sequence

Quasi-random low-discrepancy sequence.

```csharp
// Generate Halton sequence
double[] seq = HaltonSequence.Create(base: 2, count: 100);

// Multi-dimensional
var halton = new HaltonSequence(dimensions: 3);
V3d sample = halton.Next();
```

---

## INode Traversal

Tree/graph traversal utilities for types implementing `INode`.

### Interface
```csharp
public interface INode
{
    IEnumerable<INode> SubNodes { get; }
}
```

### Traversal Methods
```csharp
// Depth-first traversal
foreach (var node in root.DepthFirst())
{
    // process
}

// Breadth-first traversal
foreach (var node in root.BreadthFirst())
{
    // process
}

// All descendants (inclusive)
foreach (var node in root.Descendants())
{
    // includes root
}

// With depth limit
foreach (var node in root.DescendantsAtMaxDepth(maxDepth))
{
    // process
}
```

---

## Geodetic Conversions

Geographic coordinate transformations.

### GeoEllipsoid

Reference ellipsoid parameters.

```csharp
public static class GeoEllipsoid
{
    public static readonly GeoEllipsoid WGS84;
    // Semi-major axis, semi-minor axis, eccentricity
}
```

### Conversions
```csharp
// Longitude/Latitude/Height to ECEF XYZ
V3d xyz = GeoConversion.LatLonHeightToXyz(
    latitude,   // radians
    longitude,  // radians
    height,     // meters
    GeoEllipsoid.WGS84
);

// ECEF XYZ to Longitude/Latitude/Height
var (lat, lon, h) = GeoConversion.XyzToLatLonHeight(xyz, GeoEllipsoid.WGS84);
```

---

## Fun (Math Utilities)

Common math functions.

```csharp
public static class Fun
```

### Basic
```csharp
Fun.Min(a, b);
Fun.Max(a, b);
Fun.Clamp(value, min, max);
Fun.Abs(x);
Fun.Sign(x);
```

### Interpolation
```csharp
Fun.Lerp(a, b, t);           // linear interpolation
Fun.SmoothStep(edge0, edge1, x);
```

### Comparison
```csharp
Fun.ApproximateEquals(a, b, epsilon);
Fun.IsTiny(x);               // near zero
```

### Rounding
```csharp
Fun.Floor(x);
Fun.Ceiling(x);
Fun.Round(x);
Fun.Truncate(x);
```

### Trigonometry
```csharp
Fun.Sin(x);
Fun.Cos(x);
Fun.Tan(x);
Fun.Asin(x);
Fun.Acos(x);
Fun.Atan(x);
Fun.Atan2(y, x);
```

### Powers
```csharp
Fun.Sqrt(x);
Fun.Pow(x, y);
Fun.Exp(x);
Fun.Log(x);
Fun.Log10(x);
```

---

## Constant<T>

Type-specific constants.

```csharp
Constant<float>.Pi
Constant<double>.Pi
Constant<float>.E
Constant<double>.E
Constant<float>.PositiveTinyValue
Constant<double>.PositiveTinyValue
Constant<float>.MaxValue
Constant<double>.MaxValue
```

---

## Usage Patterns

### Timed Operation with Logging
```csharp
Report.BeginTimed(1, "Loading scene");
var scene = LoadScene(path);
Report.End(1);  // prints: Loading scene: 1.234s
```

### Performance Monitoring
```csharp
static readonly Telemetry.Counter FrameCount = new Telemetry.Counter();
static readonly Telemetry.StopWatchTime RenderTime = new Telemetry.StopWatchTime();

static MyClass()
{
    Telemetry.Register("Frames", FrameCount);
    Telemetry.Register("Render Time", RenderTime);
}

void Render()
{
    FrameCount.Increment();
    using (RenderTime.Timer)
    {
        // render
    }
}
```

### Quasi-Random Sampling
```csharp
var halton = new HaltonSequence(2);
for (int i = 0; i < sampleCount; i++)
{
    var (u, v) = halton.Next2D();
    // use (u, v) for low-discrepancy sampling
}
```

---

## Gotchas

1. **Report.Verbosity Global State**: Setting `Report.Verbosity` affects *all* Report output globally. In multi-threaded code, one thread changing verbosity affects all threads. Use `Report.BeginTimed(verbosity, ...)` instead
2. **Telemetry Registration Permanence**: `Telemetry.Register()` persists until `Telemetry.Reset()`. Re-registering the same counter twice creates duplicate entries with different names. Keep a static reference
3. **Halton Sequence Determinism**: `HaltonSequence` is deterministic (same seed = same sequence) but NOT rewindable. Create a new instance if you need to restart; don't share across threads

---

## See Also

- [COLLECTIONS.md](COLLECTIONS.md) - `Symbol` used for telemetry counter/timer registration keys
- [ALGORITHMS.md](ALGORITHMS.md) - Random sampling via `RandomSystem` used in weighted selection and Monte Carlo algorithms
- [SERIALIZATION.md](SERIALIZATION.md) - Telemetry data can be persisted via `ICoder` for benchmarking comparisons
