# Aardvark.Base Algorithms Reference

## ShortestPath<T>

`ShortestPath<T>` implements `IShortestPath<T>` and runs asynchronous shortest-path computation.
Starting a calculation validates its seed synchronously, then atomically replaces and cancels
the previous calculation. Only a successfully completed current run publishes a result.
Path queries observe the last completed result while a replacement is running. `Cancel()`
invalidates, cancels, and waits for the current run; expected cancellation is suppressed while
worker failures are propagated.

Costs must be finite and non-negative, and accumulated path costs must remain finite.
Equal-cost routes have no guaranteed tie order. Returned reachable paths run from target
toward seed, excluding the seed; the seed path is empty, and an unreachable target
returns `[target, seed]`.

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

## Dense Graph Minimum-Spanning Trees

`DenseGraph<TVertex, TCost>.BuildMinimumSpanningTreePrim()` builds a tree rooted at
vertex index zero in `O(V^2)` time. Equal costs select the lowest vertex index and
then the lowest parent index.
Empty and singleton graphs produce trees with no edges.

`AbstractGraph<TVertex, TCost>.Tree.Traverse` visits every reachable vertex once
and invokes its edge callback exactly once per undirected tree edge. `Tree.Cost`
therefore counts each edge once. `Tree.TraverseEuler` emits no values for an empty
tree; otherwise it emits a depth-first `2E+1` walk that starts and ends at index
zero and traverses every tree edge once in each direction.

## Generic Minimum-Spanning Trees

`MinimumSpanningTree.Create<TVertex, TWeight>(edges)` consumes the source edge
sequence once and treats each edge as undirected. Vertices are discovered in source
order; Prim traversal starts at the first discovered vertex, and equal-weight frontier
edges are selected in source edge order. The emitted orientation runs from the visited
endpoint to the newly reached endpoint. Self-loops are ignored as candidates, while
parallel edges participate normally.

Empty input and input describing only one distinct vertex produce no edges. Connected
input produces exactly `V - 1` edges of minimum total weight. Input describing two or
more disconnected components throws `InvalidOperationException`; the API defines a
single tree rather than a spanning forest.

Complexity is `O(E log E)` time and `O(V + E)` auxiliary memory.

## AdaBoost

`AdaBoost.Train<T>` treats `iterations` as a strict upper bound on weak-classifier
factory invocations. Every ordinary accepted learner updates the sample weights and then
invokes the optional callback with a stable snapshot of the current ensemble. A callback
result of `true` stops training.

A learner whose weighted error is within the open band of 0.02 around 0.5
terminates training without being retained. A learner that is correct for all samples
replaces the ensemble with one positive finite vote. An all-wrong learner is a perfect inverse
and replaces it with one negative finite vote. Both perfect cases stop immediately and do not
invoke the ordinary-iteration callback. Degenerate non-finite importance or normalization
also terminates before another factory invocation receives invalid weights.

Excluding work performed by the supplied factory, `I` attempted iterations over `N`
training items take `O(I * N)` time and `O(N + K)` auxiliary memory for `K` retained
learners. Inference takes `O(K)` time and allocates no managed memory per call after warmup.

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

`LuFactorize` is in-place and may partially overwrite its input before reporting a singular
matrix. Boolean overloads return `false` when any pivot, including the final post-elimination
diagonal, is tiny according to the existing scalar or complex-component tolerance; an order-zero
factorization succeeds. The allocating multidimensional-array overloads throw `ArgumentException`
for the same singular condition. Fixed-size `LuInvert` returns `false` without changing its matrix,
while `LuInverse` returns the corresponding zero matrix.

`QrFactorize` stores normalized Householder vectors in place and returns the diagonal of the
triangular factor. It supports tall and wide matrices, including offset/strided data.
Rank-deficient input may produce non-finite factors.

## Rolling Median Window

`MedianWindow` retains the latest values in a fixed-size window without transient allocations.
Before the window is full, `Insert` returns the upper median of all values seen
so far; afterward it returns the upper median of the latest window. For an even
active count, the upper of the two middle values is selected. Equal values,
including signed zero, retain their current tie order.

`Value` is the current median. `Last` returns the most recently inserted value,
or `0.0` before the first insertion and after `Reset()`. `History` exposes the
ring storage and is not cleared by reset. `Reset()` is constant-time and starts
a new active window while preserving that storage.

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
Empty PDFs are supported as zero-count sentinel tables, e.g. `new AliasTableD(Array.Empty<double>(), 0.0)`, and expose empty `U`/`K` arrays. Do not call `Sample` on empty tables; non-empty PDFs still require finite, non-negative entries and a positive finite normalization factor.

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

Polynomial coefficient arrays are stored in ascending degree order.
`RealRoots` and `RealRootsNormed` return only finite real roots, sorted ascending.
Fixed-width tuple APIs fill unused trailing entries with `NaN`. Cubic solvers preserve
repeated roots. Array conversion drops trailing `NaN` entries.

## Cubic Curve Evaluation

`Ipol.CubicHermite.Eval`, `EvalD1`, `EvalD2`, and `EvalD3` evaluate scalar, `V2d`,
and `V3d` cubic Hermite segments.

Parameters are not clamped: values outside `[0, 1]` extrapolate the same cubic polynomial.
At `t = 0` and `t = 1`, `Eval` returns `a` and `b`, while `EvalD1` returns the respective
incoming and outgoing tangent. `EvalD3` is the constant
`12a + 6tangentIn + 6tangentOut - 12b` and is independent of `t`, including for non-finite
parameter values.

`Ipol.CatmullRom` and `Ipol.KochanekBartels` provide the same value/derivative orders.
All three families evaluate in constant time and are allocation-free after warmup.

## Enumerable Population Variance

`Fun.Variance` and `Fun.StandardDeviation` enumerate `IEnumerable<int>`,
`IEnumerable<long>`, `IEnumerable<float>`, and `IEnumerable<double>` inputs once.
The selector overloads likewise invoke the selector exactly once per element.
Variance uses the population divisor `N`, not `N - 1`. `StandardDeviation` is its square root.
Empty or non-finite inputs return `NaN`; singleton and all-equal finite inputs return zero.

## Compositional Statistics

`Stats<T>` accumulates selected moments according to `StatsOptions`. `Variance` and
`StandardDeviation` independently control reporting and both enable sum-of-squares
collection.

`Stats<T>.Add(Stats<T>)` and `operator +` require matching options and compose inputs
in left-to-right order. Minimum and maximum values are merged independently; when
an extremum is tied, its associated data comes from the left aggregate, matching
sequential accumulation.

`Histogram.Add(Histogram)` and `operator +` require identical `SlotRange` values and
slot counts. They sum bins plus underflow/overflow counts and union the observed
`DataRange`; observed ranges do not need to match.

## Source Anchors

- `src/Aardvark.Base/AlgoDat/ShortestPath.cs`
- `src/Aardvark.Base/AlgoDat/MinimumSpanningTree.cs`
- `src/Aardvark.Base/AlgoDat/AdaBoost.cs`
- `src/Aardvark.Base/AlgoDat/SalesmanOfDeath.cs`
- `src/Aardvark.Base/Geometry/BbTree.cs`
- `src/Aardvark.Base/Math/LuFactorization.cs`
- `src/Aardvark.Base/Math/QrFactorization.cs`
- `src/Aardvark.Base/Math/Base/AliasTable_auto.cs`
- `src/Aardvark.Base/Math/Base/DistributionFunction.cs`
- `src/Aardvark.Base/Math/Base/Statistics.cs`
- `src/Aardvark.Base/Math/Base/Fun_auto.cs`
- `src/Aardvark.Base/Math/Base/MedianWindow.cs`
- `src/Aardvark.Base/Math/Numerics/Polynomial.cs`
- `src/Aardvark.Base/Math/Curves/Curves.cs`
