- [FSharp] Added workaround for compiler-related bug in NativePtr.pin, pinArr, and pinArri

### 5.3.13
- [FSharp] Added null-coalesing operator ||?
- [FSharp] Added Seq.asArray
- [FSharp] Added IDictionary and IReadOnlyDictionary interfaces for MapExt
- [FSharp] Added tryFindV, tryPickV, pickV, tryHeadV for Seq, List, and Array
- [Base] Added IDictionary and IReadOnlyDictionary interfaces for Dict, IntDict, SymbolDict, DictIEq
- [Base] Added ContainsValue() for Dict types
- [Ag] Use ValueOption internally and added overloads for public API returning ValueOption

### 5.3.12
- Fixed regression in range constructors

### 5.3.11
- Fixed range constructors for empty and null arrays and sequences
- Added GrayAlpha pixel format constants
- Added ToString() override for PixFormat
- Added PixFileFormat.Webp and PixWebpSaveParams
- Added more known image file extensions
- Added PixTiffSaveParams for choosing the compression scheme when saving TIFF images
- Added PixExrSaveParams for choosing the compression scheme when saving EXR images

### 5.3.10
- [IO] Added SymMap constructor overload

### 5.3.9
- [Base] Added AliasTable
- [Base] Added Triangle.Distance
- [Base] Added Triangle.SolidAngle
- [FSharp] Added NativePtr.stackUseArr
- [FSharp] Added Map.tryFindV
- [FSharp] Added Map.ofSeqWithDuplicates
- [FSharp] Added choosei, collecti for Seq, List, Array
- [FSharp] Added assignment operators
- [FSharp] Added Type.CLRSize extension property
- [Introspection] Made native library loading more robust
- [Introspection] Fixed issue with plugin cache invalidation
- [Report] Using Stopwatch.GetTimestamp() instead of Stopwatch instance in ReportJob

### 5.3.8
- [FSharp] Added stableSum and stableSumBy for List, Array, and Seq
- added basic BbTree implementation without operations

### 5.3.7
- Marked quaternion to matrix conversion obsolete as it is misleading
- Added conversions between ranges and 2D vectors
- Added Lerp and InvLerp for integer ranges and boxes
- Unified native dependency unpacking and added Aardvark.UnpackAndListNativeDependencies

### 5.3.6
- Updated System.Text.Json dependency (CVE-2024-43485)
- Added angle conversion utilities for ranges
- Added Ray3.Transformed overloads
- Added Ray3.Normalized
- Added Vec.AngleBetweenSigned for 2D vectors
- Added Fun.WrapToPi
- Added 2D ray-circle intersection methods

### 5.3.5
- [Base] added IsEmpty/IsEmptyOrNull overloads for Array/ICollection with efficient implementation
- [Base] added FindValue and FindNeighboursV to SortedSet
- [FSharp] changed MemoryManager/FreeList to use value tuples/options and use new SortedSet methods
- [FSharp] added Path.normalizeDirectorySeparators
- [FSharp] fixed Path.withTrailingSlash and Path.withoutTrailingSlash to consider Path.AltDirectorySeparatorChar

### 5.3.4
- [FSharp] Moved pin utilities to modules; added pinArri, temp
- Prevent multiple invocations of Aardvark.Init
- Added new PixProcessor API for plugin-based scaling, rotating and remapping of images (replaces PixImage.SetScaledFun et al.)

### 5.3.3
- Optimized ReferenceCountingSet (struct enumerator, internal struct tuples)
- Restored MD5 hashing extensions (fixes: https://github.com/aardvark-platform/aardvark.base/issues/72)
- [Introspection] Native libraries are now unpacked to the default Aardvark cache directory with subfolders for each platform (fixes: https://github.com/aardvark-platform/aardvark.base/issues/81).
- [Introspection] Improved support for single file deployment. Fixed plugin loading and caching (fixes: https://github.com/aardvark-platform/aardvark.base/issues/65).
- [Introspection] Made assembly loading more robust for NET 8. If an assembly cannot be resolved the entry path is checked for the assembly file, making it possible to load assemblies by name that are not runtime or compilation dependencies (see also: https://github.com/Particular/Workshop/issues/64).
- [FSharp] Added RequireQualifiedAccess attribute to ProcListValue
- [FSharp] Added RequireQualifiedAccess attribute to Tree
- [FSharp] Added pin, pinArr, and NativePtr.Address
- Added Stream.ReadBytes extension

### 5.3.2
- [FSharp] Revert LookupTable utilities to use lists instead of seq

### 5.3.1
- [FSharp] Added Stream.readAllBytes
- [FSharp] Added Path.withTrailingSlash and withoutTrailingSlash
- [FSharp] Added approximateEquals
- [FSharp] Reworked LookupTable utilities
- [FSharp] Optimized and reworked type patterns
- [FSharp] Imported PrimitiveValueConverter from Aardvark.Rendering

### 5.3.0
- https://github.com/aardvark-platform/aardvark.base/wiki/Aardvark-5.3-changelog

### 5.3.0-prerelease0005
- Reintroduced `IPixImage` as `IPix`
- Renamed `PixImageCube` to `PixCube`

### 5.3.0-prerelease0004
- Fixed PixImage/Volume conversion with redundant channels
- Added Width, Height, and Depth properties for PixImage and PixVolume

### 5.3.0-prerelease0003
- Restored Error<'T>
- [PixImageMipMap] Re-added PixFormat and reverted rename of ImageArray field
- [PixImageMipMap] Renamed Count to LevelCount
- [PixImageCube] Reverted rename of MipMapArray field
- [PixLoader] Added CanEncode and CanDecode
- Made Array and Data of PixImage and PixVolume consistent with tensors

### 5.3.0-prerelease0002
- Removed Result<'T> alias
- [FSharpMath] Fixed conflicts in SRTP member names with .NET
- [Color] Added Fun.Lerp overloads with vector parameter

### 5.3.0-prerelease0001
- Initial prerelease

### 5.2.32
- Fixed Type.GetCLRSize for structs
- Use .NET functions for Array.Copy and Set
- Fixed Array CopyTo extension overloads for copying between an array and native memory
- Changed SymMapBaseCollectionTraversal.Collect typeName argument to Symbol
- Added ToBase64 extensions with span
- Added generic Compute**Hash overloads (avoiding GetCLRSize)
- Added AsCastSpan extension

### 5.2.31
* fixed Hash Computations for non-primitive types
* added `AsByteSpan` extension method for System.Array/string in >=net6.0

### 5.2.30
* removed UnsafeCoerce usages and several other net6.0+ fixes
 
### 5.2.29
* Fixed color parsing to be independent of the current culture (regression in 5.2.27)
* Added more value variants for Dictionary, Dict, and SymbolDict functions

### 5.2.28
* Added Brewer color schemes
* Added String.replace
* Added IDictionary.TryPop overload with output argument

### 5.2.27
* Improved and fixed RangeSet implementation
* Added utilities for ValueOption
* Added more ArraySegment utilities
* [Text] Fixed NestedBracketSplitCount
* [Color] Fixed overflow issue in C4ui constructors
* [Color] Improved color parsing, now supports hexadecimal color strings
* [PixImage] Fixed file handle leak in Windows Media loader
* [MapExt] Added value variants of some operations

### 5.2.26
* LinearRegression3d: made fields public
* Added getLines, normalizeLineEndings, withLineNumbers in String module
* Fixed FGetValueOrDefault dictionary extension
* Added value variants for Dict functions
* Added F# ArraySegment utilities
* [Introspection] Tidied up exception reporting

### 5.2.25
* Fixed issue with PixImageMipMap loading
* Improved exception message of EnumHelpers methods
* [Introspection] Use XML serialization for plugin cache
* Replaced netcoreapp3.1 target with net6.0

### 5.2.24
* fixed and optimized Cell.Contains
* update Aardvark.Build to 1.0.19

### 5.2.23
* Fixed build for VS 2019 / .NET 6

### 5.2.22
* Fixed RandomSample.Lambertian internal calculation precision
* Added float bits method overloads for unsigned vectors
* Use reflection-based PixImageMipMap creation
* [TypeInfo] Added color-related types and patterns
* [Trafo] Implemented infinite far plane for perspective projection
* [Trafo] Added reversed perspective projection variants
* [Introspection] Increased verbosity level of custom attribute error

### 5.2.21
* Made non-generic PixImage creation more flexible and robust
* Added NativePtr.Item and NativePtr.Value
* Added Buffer.MemoryCopy overloads with nativeint arguments

### 5.2.20
* [Base] added float overloads to RandomSample methods
* [Base] changed array memory copies to use Buffer.MemoryCopy
* [PixImage] respect channel order in format and type conversion
* [PixImage] improved support for dual-channel images
* [PixImage] fixed and improved format conversion
* [Tensors] added simple accessors for Gray and BW
* [Vrml] added support for gzip compressed files
* [Vrml] allow registration of custom field parsers
* [Vrml] improved robustness in case of unknown field tokens

### 5.2.19
* Added IPixMipmapLoader for loading images with mipmap chains
* Marked some exotic image loading functions obsolete

### 5.2.18
* made introspection attribute querying more robust

### 5.2.17
* JSON support
	- add missing Range1* types
	- handling of special float values (NaN, +Inf, -Inf)
* Add Vec.w
* Update Aardvark.Build

### 5.2.16
* support System.Text.Json for primitive types 
  (vectors, matrices, boxes, transforms, geometric primitives, ...)

### 5.2.15
* Added File.writeAll* variants that create parent directories
* [IO] Added logging output in case there is a stream position mismatch
* [PixImage] Print path when loading from file stream

### 5.2.14
* Updated to ImageSharp 2.1
* Implemented default PixImage scaling function
* Configurable PixImage functions throw proper exceptions
* Added float16 support for PixImageMipMap
* [Introspection] changed logging message when using CustomEntryAssembly from warning to normal

### 5.2.13
* Added uint32 vector types (V2ui, V3ui, V4ui)
* Added Disposable.empty
* Inlined color conversion methods, deprecating lambdas
* Implemented color conversions for Float16
* [Patterns] Added 2x3 matrices

### 5.2.12
* Added Fun.PowerOfTwo for int32 vectors
* [FSharpMath] Added exp2, step, and linearstep
* [Patterns] Added missing vector and matrix types
* Added Fun.Step and Fun.Linearstep

### 5.2.11
* [Ag] Removed compiler message

### 5.2.10
* fixed VRML transformations (missing normalization)

### 5.2.9
* Executable memory tools are now obsolete (use Aardvark.Assembler nuget package instead)

### 5.2.8
* revert obsolete AMD64 assembler to original behaviour

### 5.2.7
* fixed AMD64 assembler Begin/End (saving callee saved registers)

### 5.2.6
- [PixImage.SystemDrawing] removed System.Drawing.Common upper version constraint
- [IO] BinaryReadingCoder: patch object reference after converter/proxy conversion (#77)

### 5.2.5
- Ported System.Drawing bitmap extensions to new PixLoader API (renamed to Aardvark.PixImage.SystemDrawing)
- Added Aardvark.PixImage.WindowsMedia
- Added Trafo.(Inv)TransformPosProj(Full)
- Added constructors and comments for tensor info structs

### 5.2.4
- Added new API for PixImage loading and saving
- Added PixImage.meanSquaredError and PixImage.peakSignalToNoiseRatio
- Moved System.Drawing primitives extensions to Aardvark.Base.SystemDrawingInterop

### 5.2.3
- Added transform methods for Trafo types
- Added float32 overload for WeightedSum
- Fixed Plane3d.Transformed methods
- Added remainder operator for vector types
- Made Col.FormatDefaultOf more flexible
- [Report] Create directory for log file if it does not exist
- Added RangeSet64

### 5.2.2
- [ImageSharp] Implemented proper per row processing

### 5.2.1
- [ImageSharp] Switched to contiguous image buffers

### 5.2.0
- https://github.com/aardvark-platform/aardvark.docs/wiki/Aardvark-5.2-changelog

### 5.1.27
- added Transformed method for MatrixInfo and VolumeInfo
- fixed ldconfig regex for entries with ABI

### 5.1.26
- added PixImage and PixVolume related extensions to native tensors
- added PolyRegion2d.Overlaps(Box2d)
- fixed byte color indexing
- removed unnecessary DevILSharp references
- added Map.unionMany
- fixed Ceres native unpack
- workaround for loading native IPP dependencies

### 5.1.25
- another SHA1 truncate

### 5.1.24
- "simulated" MD5 via truncated SHA1 (please don't hit me security bubble)

### 5.1.23
- switched MD5 to SHA1

### 5.1.22
- updated to FSharp.Core >= 5.0.0

### 5.1.21
- [Report] workaround for getting/setting Console.ForegroundColor when not available. added `NoTarget` to Report for disabling

### 5.1.20
- add Cell.GetCommonRoot(...) and corresponding constructor overloads; same for Cell2d

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