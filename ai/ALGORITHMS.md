# Aardvark.Base Algorithms Reference

AI-targeted reference for algorithms in Aardvark.Base: graph algorithms, spatial structures, numerical methods, sampling.

---

## Graph Algorithms

### Shortest Path (Dijkstra with Fibonacci Heap)

```csharp
public class ShortestPath<T> : IShortestPath<T>
```

Dijkstra's algorithm using Fibonacci Heap for O(1) amortized decrease-key.

#### Construction
```csharp
// From nodes and edge list
var sp = new ShortestPath<V3d>(
    nodes,                          // List<T> nodes
    edges,                          // List<(int, int)> edges (index pairs)
    (a, b) => (float)(a - b).Length // cost function
);

// From adjacency list
var sp = new ShortestPath<V3d>(
    nodes.ToArray(),
    neighbors,                      // List<int>[] adjacency lists
    getCostFunc
);
```

#### Usage
```csharp
// Calculate shortest paths from seed (async)
sp.CalculateShortestPaths(seedNode);
sp.CalculateShortestPathsByIndex(seedIndex);

// Get path to target
List<T> path = sp.GetMinimalPath(targetNode);
List<T> path = sp.GetMinimalPathByIndex(targetIndex);

// Cancel running computation
sp.Cancel();
```

### Minimum Spanning Tree (Prim's Algorithm)

```csharp
// Generic MST using Prim's algorithm
// Available via graph utilities
```

### Traveling Salesman (TSP)

Located in `SalesmanOfDeath.cs`:

```csharp
// Graph classes
public abstract class AbstractGraph<TVertex, TCost>
public class DenseGraph<TVertex, TCost> : AbstractGraph<TVertex, TCost>

// TSP optimization
// Uses 2-opt and 3-opt local search improvements
// MST-based heuristics with Euler tour traversal
```

---

## Spatial Data Structures

### BbTree (Bounding Box Tree / BVH)

SAH-based bounding volume hierarchy for ray tracing and spatial queries.

```csharp
public class BbTree
```

#### Construction
```csharp
var tree = new BbTree(
    boundingBoxes,                    // Box3d[] for each primitive
    BbTree.BuildFlags.Default,        // build options
    countArray                        // optional weights
);
```

#### Build Flags
```csharp
[Flags]
public enum BuildFlags
{
    None,
    CreateBoxArrays = 0x01,      // store left/right box arrays
    CreateCombinedArray = 0x02,  // combined box + flags
    LeafLimit01 = 0x010,         // 1 primitive per leaf
    LeafLimit04 = 0x040,         // 4 primitives per leaf
    LeafLimit08 = 0x080,         // 8 primitives per leaf
    LeafLimit16 = 0x100,         // 16 primitives per leaf
    Default = CreateBoxArrays | LeafLimit01
}
```

#### Properties
```csharp
tree.NodeCount        // int - number of internal nodes
tree.Box3d            // Box3d - root bounding box
tree.Boxes            // Box3d[] - original bounding boxes
tree.LeftBoxArray     // Box3d[] - left child boxes
tree.RightBoxArray    // Box3d[] - right child boxes
tree.IndexArray       // int[] - child indices
tree.LeafArray        // int[] - leaf primitive indices

tree.GetLeft(nodeIndex)   // left child index
tree.GetRight(nodeIndex)  // right child index
```

#### Hit Structure
```csharp
public struct BbTreeHit
{
    public int NodeIndex;
    public double RayT;
}
```

---

## Numerical Algorithms

### LU Factorization

In-place LU decomposition with partial pivoting.

```csharp
// Extension method on arrays
int[] perm = matrix.LuFactorize();  // float[,] or double[,]

// With pre-allocated permutation array
bool success = matrix.LuFactorize(permutation);

// On Matrix<T> tensors
int[] perm = matrixTensor.LuFactorize();  // Matrix<float> or Matrix<double>
```

#### Solving Linear Systems
```csharp
// After factorization
vector.LuSolve(luMatrix, permutation);  // modifies vector in-place

// Compute inverse
var inverse = luMatrix.LuInverse(permutation);
```

### QR Factorization

Householder reflection-based QR decomposition.

```csharp
// Available for Matrix<float> and Matrix<double>
var (q, r) = matrix.QrFactorize();
```

### Polynomial Operations

```csharp
public static class Polynomial
```

#### Root Finding
```csharp
// Solve polynomial equations
Polynomial.RealRootsOfNormed(a);           // x + a = 0
Polynomial.RealRootsOfNormed(a, b);        // x^2 + ax + b = 0
Polynomial.RealRootsOfNormed(a, b, c);     // x^3 + ax^2 + bx + c = 0
Polynomial.RealRootsOfNormed(a, b, c, d);  // x^4 + ... (up to 4th order)
```

#### Polynomial Evaluation
```csharp
// Horner's method
double y = Polynomial.Evaluate(coefficients, x);
```

---

## Probability & Sampling

### Alias Table (Walker's Algorithm)

O(1) sampling from discrete probability distributions.

```csharp
public class AliasTableF  // float
public class AliasTableD  // double
```

#### Construction
```csharp
// From unnormalized PDF
var table = new AliasTableD(pdf, 1.0 / pdf.Sum());

// Factory methods
var table = AliasTableD.FromPdf(pdf);            // auto-normalize
var table = AliasTableD.FromNormalizedPdf(pdf);  // already normalized
```

#### Sampling
```csharp
// Sample with random value in [0, 1)
int index = table.Sample(random.UniformDouble());
```

#### Update
```csharp
// Update with new PDF (same length)
table.Update(newPdf, normalizationFactor);
```

### Distribution Function (CDF Sampling)

Binary search sampling O(log n).

```csharp
public class DistributionFunction
```

### Random Sampling Utilities

```csharp
// Uniform sphere sampling
V3d direction = random.UniformV3dDirection();

// Cosine-weighted hemisphere (Lambertian)
V3d direction = random.CosineWeightedHemisphere(normal);

// Halton sequence (quasi-random, low-discrepancy)
double[] halton = HaltonSequence.Create(base, count);
```

---

## Sorting Algorithms

Extension methods on arrays with multiple strategies.

### Available Methods
```csharp
array.SortAscending();           // in-place ascending
array.SortDescending();          // in-place descending
array.SortByKey(keySelector);    // sort by key function

// Permutation-based (returns index array)
int[] perm = array.CreatePermutationAscending();
int[] perm = array.CreatePermutationDescending();
```

### Algorithm Selection
- **QuickSort**: General case
- **InsertionSort**: Small arrays (<31 elements)
- **TimSort-like hybrid**: Merge + gallop for partially sorted data

### Median Selection
```csharp
// Find k-th smallest element
T kth = array.KthSmallest(k);
T median = array.Median();
```

---

## Convex Hull

### Hull2 / Hull3

Convex polyhedra represented as half-space intersections.

```csharp
public struct Hull2f { public Plane2f[] PlaneArray; }
public struct Hull2d { public Plane2d[] PlaneArray; }
public struct Hull3f { public Plane3f[] PlaneArray; }
public struct Hull3d { public Plane3d[] PlaneArray; }
```

#### Construction
```csharp
// From bounding box
var hull = new Hull3d(box3d);

// From plane array
var hull = new Hull3d(planes);
```

#### Containment
```csharp
bool inside = hull.Contains(point);
```

---

## Intersection Tests

Extensive geometric intersection functions in `IntersectionTests_template.cs`.

### Ray Intersections
```csharp
ray.Hits(triangle);
ray.Hits(box);
ray.Hits(sphere);
ray.Hits(plane);
ray.HitsTriangle(p0, p1, p2, tmin, tmax, ref hit);
```

### Point Containment
```csharp
box.Contains(point);
triangle.Contains(point);
polygon.Contains(point);
hull.Contains(point);
```

### Primitive-Primitive
```csharp
box.Intersects(box);
sphere.Intersects(sphere);
plane.Intersects(ray);
```

---

## Machine Learning

### AdaBoost

Adaptive boosting for binary classification.

```csharp
public class AdaBoost<TClassifier>
```

Ensemble method combining weak classifiers with weighted voting. Uses exponential error-based weight adjustment.

---

## Usage Patterns

### Build BVH and Ray Trace
```csharp
var boxes = triangles.Map(t => t.BoundingBox3d);
var tree = new BbTree(boxes);

var ray = new Ray3d(origin, direction);
// Traverse tree, test intersections
```

### Solve Linear System
```csharp
var A = new double[3, 3] { ... };
var b = new double[] { ... };
var perm = A.LuFactorize();
b.LuSolve(A, perm);
// b now contains solution
```

### Weighted Random Sampling
```csharp
var weights = new double[] { 1, 2, 5, 2 };  // relative weights
var table = new AliasTableD(weights, 1.0 / weights.Sum());
var rnd = new RandomSystem();
int sample = table.Sample(rnd.UniformDouble());
```
