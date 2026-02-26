# Aardvark.Base Algorithms Reference

Source-verified map of key algorithm types and entry points.

## ShortestPath<T>

`ShortestPath<T>` implements `IShortestPath<T>` and runs asynchronous shortest-path computation.

Key methods:

- `CalculateShortestPaths(T seed)`
- `CalculateShortestPathsByIndex(int seedIndex)`
- `GetMinimalPath(T target)`
- `GetMinimalPathByIndex(int targetIndex)`
- `Cancel()`

Constructors:

```csharp
new ShortestPath<T>(List<T> nodes, List<(int,int)> edges, Func<T,T,float> getCost);
new ShortestPath<T>(T[] nodes, List<int>[] neighbors, Func<T,T,float> getCost);
```

## BbTree

Bounding-box hierarchy in `Geometry/BbTree.cs`.

Constructor:

```csharp
new BbTree(Box3d[] boundingBoxes, BbTree.BuildFlags flags = BbTree.BuildFlags.Default, int[] countArray = null);
```

Useful members:

- `NodeCount`
- `Box3d`
- `IndexArray`
- `LeafArray`
- `LeftBoxArray`
- `RightBoxArray`
- `GetLeft(i)`, `GetRight(i)`

`BbTreeHit` contains `NodeIndex` and `RayT`.

## Linear Algebra Numerics

Available in `Math/LuFactorization.cs` and `Math/QrFactorization.cs`:

- `LuFactorize`
- `LuSolve`
- `LuInverse`
- `QrFactorize`

`LuFactorize` is in-place.

## Probability Sampling

### Alias tables

Types:

- `AliasTableF`
- `AliasTableD`

Construction/update:

```csharp
var t = new AliasTableD(pdf, 1.0 / pdf.Sum());
t.Update(newPdf, 1.0 / newPdf.Sum());
int index = t.Sample(rnd.UniformDouble());
```

`FromPdf` / `FromNormalizedPdf` exist as instance methods on the class.

### DistributionFunction

`DistributionFunction` provides CDF-based sampling:

```csharp
var d = new DistributionFunction(pdf);
int i = d.Sample(rnd);
int j = DistributionFunction.SampleCDF(d.CDF, rnd.UniformDouble());
```

## Polynomial

`Polynomial` is in `Math/Numerics/Polynomial.cs` (not `Math/Base`).

Examples:

- `coeff.Evaluate(x)`
- `coeff.Derivative()`
- `Polynomial.RealRootsOfNormed(...)`

## Source Anchors

- `src/Aardvark.Base/AlgoDat/ShortestPath.cs`
- `src/Aardvark.Base/Geometry/BbTree.cs`
- `src/Aardvark.Base/Math/LuFactorization.cs`
- `src/Aardvark.Base/Math/QrFactorization.cs`
- `src/Aardvark.Base/Math/Base/AliasTable_auto.cs`
- `src/Aardvark.Base/Math/Base/DistributionFunction.cs`
- `src/Aardvark.Base/Math/Numerics/Polynomial.cs`
