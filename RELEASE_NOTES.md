### 5.1.19
- fixed cell-from-box constructor for boxes with Max including 0.0
- Relax asserts in matrix and rot methods and constructors
### 5.1.18
- cleaned up and fixed cell construction from boxes

### 5.1.17
- Add Fun.Log2CeilingInt
- Fix power of two edge cases in Cell and Cell2d

### 5.1.16
- updated build script
- fixed PixImage.CreateRaw loader fallback
- fixed default AssemblyFilter
- fixed Introspection ignoring GLSLangSharp
- unbreak Cell to work with fixed Log2Int (same for Cell2d)

### 5.1.15
 - added functions for computing mipmap level count and size
 - added support for signed integer PixImages
 - changed default introspection cache location to LocalApplicationData
 - fixed Log2Int
 - fixed missing FileStream Dispose in Vrml97 parser

### 5.1.14
 - fixed Introspection caching

### 5.1.13
 - added Plane2d.Intersects overload that was removed accidentally
 
### 5.1.12
 - added missing XmlIgnore attribute to vector swizzles 
 - added ValuesWithKeyEnumerator to Dicts
 - preparations for .net 5 single file deployment
 - added missing finite checks to vectors and matrices
 - added vector variants for Fun.FloatToBits and Fun.FloatFromBits
 - fixed Box2d - Line2d intersection test
 - fixed Plane2d - Ray2d intersection test
 - fixed polygon extensions

### 5.1.11
 - added some IAssemblerStream methods (AddInt, MulInt, etc.)
 
### 5.1.10
 - fixed mscorlib recursive resource lookup (#61)
 - added Constant<T> initialization (#59)

### 5.1.9
- removed System.Reactive depdendency
- raised FSharp.Core version to >= 4.7.0

### 5.1.8

- added polygon transformataions (#57)
- prioritization of custom images loaders: considering last registered first
- consistency and completeness of Ray3d.Hits overloads
- changed Ray3d.Hits default range to [0, double.MaxValue]

### 5.1.6

- fixed Fun.Frac for tiny negative numbers
- added invLerp to FSharpMath

### 5.1.5

- devil loader functions are now public

### 5.1.4

- updated to FSharp.Data.Adaptive 1.2
- added IEquatable to math types
- added Array.binarySearch

### 5.1.3

- udpated build script

### 5.1.2

- testing
- new package mechanism

### 5.1.1

- switched to FSharp.Data.Adaptive 1.1