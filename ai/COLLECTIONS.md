# Aardvark.Base Collections Reference

AI-targeted reference for specialized collections beyond standard .NET types.

---

## Symbol System

Interned strings/GUIDs for efficient comparison and storage.

### Symbol

```csharp
public readonly struct Symbol : IEquatable<Symbol>, IComparable<Symbol>
{
    public readonly int Id;
}
```

Symbols are interned - each unique string/GUID gets a unique integer ID. Comparison is O(1) integer comparison.

#### Creation
```csharp
// From string (interned)
Symbol sym = Symbol.Create("mySymbol");
Symbol sym = "mySymbol";  // implicit conversion

// From GUID
Symbol sym = Symbol.Create(guid);
Symbol sym = Symbol.CreateNewGuid();

// Empty symbol
Symbol empty = Symbol.Empty;
```

#### Properties
```csharp
sym.Id           // int - unique identifier
sym.IsEmpty      // true if Id == 0
sym.IsNotEmpty
sym.IsPositive   // Id > 0
sym.IsNegative   // Id < 0
```

#### Negative Symbols
Unary minus creates a "negative" symbol - useful for storing two values per key:
```csharp
Symbol pos = "key";
Symbol neg = -pos;     // negative version, no string representation

// In a SymbolDict, pos and neg are distinct keys
dict[pos] = value1;
dict[neg] = value2;    // stored separately
```

#### Conversion
```csharp
string str = sym.ToString();  // get original string
Guid guid = sym.ToGuid();     // get GUID (if created from GUID)
```

### TypedSymbol<T>

Symbol with compile-time type association.

```csharp
public readonly struct TypedSymbol<T>
{
    public readonly Symbol Symbol;
}
```

Useful for type-safe dictionary access:
```csharp
var key = new TypedSymbol<int>("count");
// Can be used with typed dictionary APIs
```

---

## Symbol Collections

### SymbolDict<TValue>

Dictionary with Symbol keys, optional stack behavior.

```csharp
public class SymbolDict<TValue> : IDictionary<Symbol, TValue>
```

Features:
- Hashtable with external linking
- Prime table sizes to reduce collisions
- Optional stack behavior for duplicate keys

```csharp
var dict = new SymbolDict<int>();
dict["key"] = 42;
int value = dict["key"];

// Check existence
if (dict.TryGetValue("key", out var val)) { ... }
if (dict.ContainsKey("key")) { ... }
```

### SymbolSet

HashSet with Symbol elements.

```csharp
public class SymbolSet : ICollection<Symbol>
```

```csharp
var set = new SymbolSet();
set.Add("item");
bool contains = set.Contains("item");
```

---

## LruCache<TKey, TValue>

Thread-safe least-recently-used cache with customizable eviction.

```csharp
public class LruCache<TKey, TValue>
```

### Construction
```csharp
// Full constructor
var cache = new LruCache<string, byte[]>(
    capacity: 100_000_000,           // max total size
    sizeFun: key => GetFileSize(key), // size per item
    readFun: key => File.ReadAllBytes(key), // lazy load
    deleteAct: (key, value) => { }   // cleanup on eviction
);

// Simple constructor (no lazy loading)
var cache = new LruCache<string, object>(capacity);
```

### Usage
```csharp
// Access via indexer (auto-loads if missing)
var value = cache[key];

// Manual add with custom size and cleanup
cache.GetOrAdd(key, size, () => CreateValue(), () => Cleanup());

// Adjust capacity (evicts if necessary)
cache.Capacity = newCapacity;
```

### Behavior
- Thread-safe (all operations synchronized)
- Evicts least-recently-used items when over capacity
- Calls delete action on eviction
- Uses min-heap for efficient LRU tracking

---

## ConcurrentHashSet<T>

Thread-safe hash set built on ConcurrentDictionary.

```csharp
// Namespace: System.Collections.Concurrent
public class ConcurrentHashSet<T> : ICollection<T>
```

### Construction
```csharp
var set = new ConcurrentHashSet<int>();
var set = new ConcurrentHashSet<int>(collection);
var set = new ConcurrentHashSet<int>(comparer);
var set = new ConcurrentHashSet<int>(concurrencyLevel, capacity);
```

### Operations
```csharp
set.Add(item);           // returns true if added
set.TryRemove(item);     // returns true if removed
set.Contains(item);
set.Clear();

// Set operations
set.UnionWith(other);
set.IntersectWith(other);
set.ExceptWith(other);

// Properties
set.Count
set.IsEmpty
```

---

## Dict<TKey, TValue>

Enhanced dictionary with additional features.

```csharp
public class Dict<TKey, TValue> : IDictionary<TKey, TValue>
```

Similar to standard Dictionary but with:
- Prime table sizes
- External linking for collision handling
- Additional utility methods

---

## Specialized Dictionaries

### SingleEntryDict

Dictionary optimized for exactly one entry.

### ConstantDict

Dictionary where all keys map to the same value.

---

## Usage Patterns

### Symbol-Based Configuration
```csharp
// Define symbols once
static readonly Symbol Width = "Width";
static readonly Symbol Height = "Height";

// Use for efficient dictionary access
var config = new SymbolDict<object>();
config[Width] = 1920;
config[Height] = 1080;

// Negative symbol for metadata
config[-Width] = "screen width in pixels";
```

### File Cache
```csharp
var imageCache = new LruCache<string, PixImage>(
    capacity: 500_000_000,  // 500MB
    sizeFun: path => new FileInfo(path).Length,
    readFun: path => PixImage.Load(path),
    deleteAct: (path, img) => img.Dispose()
);

// Auto-loads and caches
var img = imageCache["image.png"];
```

### Thread-Safe Set Operations
```csharp
var processed = new ConcurrentHashSet<int>();

Parallel.ForEach(items, item =>
{
    if (processed.Add(item.Id))  // only process once
    {
        Process(item);
    }
});
```

---

## Gotchas

1. **Negative Symbols Are Keys**: `Symbol.Create("x")` and `-Symbol.Create("x")` are *different* dictionary keys. Negative symbols have no string representation (GUID-based). Use carefully or you'll lose data
2. **LruCache Eviction Overhead**: `LruCache` calls your `deleteAct` callback on eviction. If callbacks are expensive (dispose, I/O), this blocks the thread. Cache evictions are synchronous
3. **SymbolDict Growth**: SymbolDict uses prime table sizes for good collision distribution, but growth is table-size doubling. For millions of symbols, expect fragmentation overhead

---

## See Also

- [SERIALIZATION.md](SERIALIZATION.md) - `Symbol`, `SymbolDict`, `SymbolSet` have `CodeSymbol`, `Code(SymbolDict)` serialization methods
- [ALGORITHMS.md](ALGORITHMS.md) - `Symbol` used for node labeling in graphs; `LruCache` for memoization in shortest-path queries
- [UTILITIES.md](UTILITIES.md) - `Telemetry` uses `Symbol` for counter/timer registration and lookup
