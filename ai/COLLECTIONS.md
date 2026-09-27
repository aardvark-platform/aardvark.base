# Aardvark.Base Collections Reference

## Symbol and TypedSymbol

`Symbol` is interned and integer-backed.

```csharp
Symbol s0 = Symbol.Create("name");
Symbol s1 = "name";                 // implicit conversion
Symbol s2 = Symbol.Create(Guid.NewGuid());
Symbol empty = Symbol.Empty;
Symbol neg = -s0;                   // negative key variant
```

Important members:

- `Id`
- `IsEmpty` / `IsNotEmpty`
- `IsPositive` / `IsNegative`
- `ToGuid()`

`TypedSymbol<T>` provides compile-time value type pairing for typed dictionary access.

## Dict Family

Main types in `Symbol/Dict_auto.cs`:

- `Dict<TKey, TValue>`
- `DictSet<TKey>`
- `SymbolDict<TValue>`
- `SymbolSet`

Common helpers:

- `GetOrCreate(...)`
- `TryRemove(...)`
- `ValuesWithKey(...)`

`SymbolDict<T>` also has typed overloads:

- `Add<TType>(TypedSymbol<TType> key, TType value)`
- `Get<TType>(TypedSymbol<TType> key)`
- `TryGetValue<TType>(TypedSymbol<TType> key, out TType value)`

`DictFun.PopAll(dict, key)` removes each matching entry before yielding its value. For dictionaries configured to stack duplicate keys, values are returned in last-in-first-out order, and stopping enumeration early leaves every already-yielded entry removed.

`SingleValueDict<TKey, TValue>` and `SingleValueSymbolDict<TValue>` associate every current key with one shared value. Their `Values` sequences contain that shared value once per key and are empty when the dictionaries have no keys.

## EnumerableEx.ToDictionaryDistinct

All `ToDictionaryDistinct` overloads evaluate the key and element selectors exactly once per source element. Duplicate keys are handled explicitly according to the selected policy:

- The fixed duplicate-value overload replaces the stored value with that fixed value.
- The keep-or-replace callback receives the existing value first and the incoming value second. Returning `true` keeps the existing value; returning `false` stores the incoming value.
- The merge callback receives the existing value first and the incoming value second, and its result becomes the stored value.

Selector and key hashing or equality exceptions propagate directly without retrying the operation.

## LruCache<TKey, TValue>

`LruCache` is synchronized and capacity-driven.

Constructors:

```csharp
new LruCache<TKey, TValue>(capacity);
new LruCache<TKey, TValue>(capacity, sizeFun, readFun, deleteAct);
```

Key operations:

- indexer `cache[key]` (auto-load with `readFun`)
- `GetOrAdd(...)`
- `TryRemove(...)`
- `Remove(...)`
- mutable `Capacity` (can trigger eviction)

`GetOrAdd(...)` accepts an optional per-entry delete action. That action is invoked when the entry leaves the cache through `Remove(...)`, `TryRemove(...)`, or capacity eviction.

Eviction removes the least-recently-used entry and updates size accounting before invoking cleanup callbacks. The cache-wide delete action runs before the per-entry delete action. If either callback throws, the same exception propagates, the victim remains removed, later callbacks in that eviction are not invoked, and subsequent cache operations remain valid. A candidate value whose eviction cleanup fails is not inserted.

## ConcurrentHashSet<T>

Defined in namespace `System.Collections.Concurrent`.

Sequence constructors apply set semantics while consuming the input once: duplicate elements are ignored according to either the default equality comparer or the supplied custom comparer. Generic and non-generic enumeration both expose elements of type `T`.

Core operations:

```csharp
set.Add(item);
set.Remove(item);   // no TryRemove API on this type
set.Contains(item);
set.Clear();
set.UnionWith(other);
```

## Integer Range Sets

`RangeSet1i`, `RangeSet1ui`, `RangeSet1l`, and `RangeSet1ul` represent unions of
closed integer intervals. `ofList`, `ofArray`, and `ofSeq` ignore intervals with
`Max < Min`, including `Invalid`, and coalesce overlaps and adjacency regardless
of input order. Duplicate and nested intervals do not add extra ranges.

Construction owns its sorting buffer: caller arrays are never reordered or
retained, and sequence inputs are enumerated once. Empty and singleton inputs
retain fast paths. Bulk construction takes `O(n log n)` time and `O(n)` peak
auxiliary space; it sorts intervals and emits unique alternating boundaries.
A terminal `MaxValue` is represented implicitly, without computing `MaxValue + 1`.

Enumeration, `ToArray`, and `ToList` return maximal disjoint intervals in ascending
order. `Count` counts those intervals, not individual values. For an empty set,
`Min` is the element type's `MaxValue` and `Max` is its `MinValue`.

## SortedSetExt<T> Neighbours

Neighbour order follows the configured comparer, including descending comparers.
`FindNeighbours` and `FindNeighboursV` return the strict predecessor, the actual
stored comparer-equal value, and the strict successor. `TryFindSmaller` and
`TryFindGreater` use the same lookup. Absent value outputs are `default(T)`;
optional outputs are `None`.

`GetViewBetween` has inclusive bounds. Neighbour queries see current parent-set
contents, even after parent mutations, and queries outside the view return its
nearest boundary-side element rather than an element outside the view. Lookup
is logarithmic in parent-tree size and does not recount or enumerate the view.
The value/try APIs allocate no managed memory. F# `SortedSet.neighbourhood`
exposes the same results as options.

## SingleEntryDict

`SingleEntryDict<TKey, TValue>` exists in `Symbol/Dicts.cs` and is optimized for one optional key/value entry. It supports normal `IDict<TKey, TValue>` lookup, removal, and re-adding the configured key after removal.

## Source Anchors

- `src/Aardvark.Base.FSharp/Datastructures/Immutable/RangeSet_template.fs`
- `src/Aardvark.Base.FSharp/Datastructures/Immutable/RangeSet_auto.fs`
- `src/Aardvark.Base/AlgoDat/ExtendedCore/SortedSetExt.cs`
- `src/Aardvark.Base.FSharp/Utilities/Interop/SortedSet.fs`

- `src/Aardvark.Base/Symbol/Symbol.cs`
- `src/Aardvark.Base/Symbol/Dict_auto.cs`
- `src/Aardvark.Base/Symbol/Dicts.cs`
- `src/Aardvark.Base/Symbol/IDict.cs`
- `src/Aardvark.Base/AlgoDat/LruCache.cs`
- `src/Aardvark.Base/AlgoDat/ConcurrentHashSet.cs`
- `src/Aardvark.Base/Extensions/IEnumerableExtensions.cs`
