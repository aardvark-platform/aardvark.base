# Aardvark.Base Collections Reference

Source-verified reference for custom collection and symbol infrastructure.

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

## ConcurrentHashSet<T>

Defined in namespace `System.Collections.Concurrent`.

Core operations:

```csharp
set.Add(item);
set.Remove(item);   // no TryRemove API on this type
set.Contains(item);
set.Clear();
set.UnionWith(other);
```

## SingleEntryDict

`SingleEntryDict<TKey, TValue>` exists in `Symbol/Dicts.cs` and is optimized for one key/value.

## Source Anchors

- `src/Aardvark.Base/Symbol/Symbol.cs`
- `src/Aardvark.Base/Symbol/Dict_auto.cs`
- `src/Aardvark.Base/Symbol/Dicts.cs`
- `src/Aardvark.Base/Symbol/IDict.cs`
- `src/Aardvark.Base/AlgoDat/LruCache.cs`
- `src/Aardvark.Base/AlgoDat/ConcurrentHashSet.cs`
