# Incremental/Adaptive System

Reactive computation with automatic dependency tracking. Values form a DAG; changes propagate lazily through dependencies.

## Architecture

Built on **FSharp.Data.Adaptive** (external NuGet package). Aardvark.Base.Incremental adds:
- `afun` - adaptive functions
- `astate` - stateful adaptive computations
- `Controller` - UI-like state with previous-value tracking
- `astream` - event streams with history (experimental)
- `Proc` - cancellable continuations

Namespace: `FSharp.Data.Adaptive` (core types from external package)

## Core Types

### Adaptive Values

| Type | Description |
|------|-------------|
| `aval<'a>` / `IAdaptiveValue<T>` | Read-only adaptive value |
| `cval<'a>` / `ChangeableValue<T>` | Mutable adaptive value (source) |

```fsharp
// F#
let counter = cval 0
let doubled = counter |> AVal.map ((*) 2)
let value = AVal.force doubled  // 0
```

```csharp
// C#
var counter = new ChangeableValue<int>(0);
var doubled = counter.Map(x => x * 2);
int value = doubled.GetValue();  // 0
```

### Adaptive Collections

| Type | C# Type | Delta Type |
|------|---------|------------|
| `aset<'a>` | `ChangeableHashSet<T>` | `HashSetDelta<T>` |
| `amap<'k,'v>` | `ChangeableHashMap<K,V>` | `HashMapDelta<K,V>` |
| `alist<'a>` | `ChangeableIndexList<T>` | `IndexListDelta<T>` |

Collections track changes via delta objects. Use readers to get incremental updates.

```csharp
// C#
var set = new ChangeableHashSet<int>(new[] { 1, 2, 3 });
var filtered = set.Filter(x => x > 1);
var reader = filtered.GetReader();

// Get deltas since last read
var deltas = reader.GetChanges(AdaptiveToken.Top);
foreach (var d in deltas)
{
    if (d.Count > 0) Console.WriteLine($"add {d.Value}");
    else Console.WriteLine($"rem {d.Value}");
}
```

## Transactions

Mutations must occur inside transactions. Changes batch until transaction ends.

```fsharp
// F#
transact (fun () ->
    counter.Value <- 1
    counter.Value <- 2  // only final value propagates
)
```

```csharp
// C#
using (Adaptive.Transact)
{
    counter.Value = 1;
    counter.Value = 2;  // only final value propagates
}
```

### AdaptiveToken

Tracks evaluation context and dependencies. Use `AdaptiveToken.Top` for top-level reads.

```fsharp
let value = myAVal.GetValue(AdaptiveToken.Top)
```

```csharp
var value = myAVal.GetValue(AdaptiveToken.Top);
```

## F# Operators

From `FSharp.Data.Adaptive.Operators`:

| Operator | Description | Example |
|----------|-------------|---------|
| `%+` | Adaptive add | `a %+ b` |
| `%-` | Adaptive subtract | `a %- b` |
| `%*` | Adaptive multiply | `a %* b` |
| `%/` | Adaptive divide | `a %/ b` |
| `%&&` | Adaptive AND | `a %&& b` |
| `%||` | Adaptive OR | `a %|| b` |
| `%?` / `%.` | Conditional | `cond %? trueVal %. falseVal` |
| `!!` | Force value | `!!aval` |
| `~~` | Make constant | `~~42` |

```fsharp
open FSharp.Data.Adaptive.Operators

let a = cval 10
let b = cval 20
let sum = a %+ b        // aval<int> = 30
let forced = !!sum      // int = 30
let const = ~~100       // aval<int> (constant)
```

## Adaptive Functions (afun)

Functions that depend on adaptive values. Re-evaluated when dependencies change.

```fsharp
type afun<'a, 'b>  // IAdaptiveFunc in C#
```

### AFun Module

| Function | Description |
|----------|-------------|
| `AFun.create f` | Wrap pure function |
| `AFun.constant v` | Always return v |
| `AFun.bind f m` | Bind aval to afun |
| `AFun.compose g f` | Compose: f then g |
| `AFun.apply v f` | Apply afun to aval |
| `AFun.run v f` | Run afun with constant input |

```fsharp
// Builder syntax
let myFunc = afun {
    let! scale = scaleAVal
    return fun x -> x * scale
}

// Composition
let combined = f >>. g  // f then g
let combined = g <<. f  // same as above
```

### Integration

```fsharp
let result : aval<'b> = AFun.apply inputAVal myFunc
```

## Adaptive State (astate)

Stateful computations that thread state through evaluation.

```fsharp
type astate<'s, 'a> = { runState: afun<'s, 's * 'a> }
```

### AState Module

| Function | Description |
|----------|-------------|
| `AState.create v` | Return value, preserve state |
| `AState.map f m` | Transform result |
| `AState.bind f m` | Sequence computations |
| `AState.getState` | Get current state |
| `AState.putState s` | Replace state |
| `AState.modifyState f` | Transform state |

```fsharp
let computation = astate {
    let! current = AState.getState
    do! AState.modifyState ((+) 1)
    return current
}
```

### Controller Pattern

Tracks previous values across evaluations. Used for UI-like delta computation.

```fsharp
type Controller<'a> = astate<ControllerState, 'a>
```

| Function | Description |
|----------|-------------|
| `withLast aval` | Get `(previous, current)` tuple |
| `pre aval` | Get previous value |
| `differentiate aval` | Get `current - previous` |

```fsharp
let deltaController = controller {
    let! (prev, curr) = withLast positionAVal
    return curr - prev  // movement delta
}

let myFunc : afun<'a, 'b> = controller.Run(deltaController)
```

## Adaptive Streams (astream)

Event streams with timestamped history. **Experimental** - namespace: `Aardvark.Base.Incremental.Experimental`

```fsharp
type astream<'a>  // IAdaptiveStream in C#

type EventHistory<'a> =
    | Cancel
    | Faulted of Exception
    | History of list<DateTime * 'a>
```

### IStreamReader

```fsharp
type IStreamReader<'a> =
    inherit IDisposable
    inherit IAdaptiveObject
    abstract GetHistory : IAdaptiveObject -> EventHistory<'a>
    abstract SubscribeOnEvaluate : (EventHistory<'a> -> unit) -> IDisposable
```

### EventHistory Module

| Function | Description |
|----------|-------------|
| `EventHistory.empty` | Empty history |
| `EventHistory.map f h` | Transform events |
| `EventHistory.choose f h` | Filter/transform |
| `EventHistory.filter f h` | Filter events |
| `EventHistory.union l r` | Merge histories |
| `EventHistory.concat hs` | Concatenate list |

## Proc (Cancellable Continuations)

Continuation-based async with cancellation support.

```fsharp
type Proc<'a, 'r>

type ProcResult<'a> =
    | Value of 'a
    | Cancelled
    | Faulted of exn
```

### Creating Procs

| Method | Description |
|--------|-------------|
| `Proc.Create v` | Immediate value |
| `Proc.Await task` | Await Task<T> |
| `Proc.Await async` | Await Async<T> |
| `Proc.Await sem` | Await SemaphoreSlim |
| `Proc.Sleep ms` | Delay |

### Running Procs

| Method | Description |
|--------|-------------|
| `Proc.RunSynchronously(p, ?ct)` | Block until complete |
| `Proc.StartAsTask(p, ?ct)` | Return Task<ProcResult> |
| `Proc.Start(p, ?ct)` | Fire and forget |

### Builder Syntax

```fsharp
let download url = proc {
    let! response = httpClient.GetAsync(url)
    let! content = response.Content.ReadAsStringAsync()
    return content
}

// Run with cancellation
use cts = new CancellationTokenSource()
let result = Proc.RunSynchronously(download "http://...", cts.Token)
match result with
| Value content -> printfn "Got: %s" content
| Cancelled -> printfn "Cancelled"
| Faulted e -> printfn "Error: %A" e
```

## ChangeTracker

Detect value changes with memoization. Used internally by afun/astate.

```fsharp
// Default equality
let hasChanged = ChangeTracker.track<MyType>
if hasChanged newValue then
    // value changed since last call

// Custom equality
let hasChanged = ChangeTracker.trackCustom (Some myEqualityFn)
```

## Common Patterns

### Reactive Collection Processing

```csharp
var items = new ChangeableHashSet<Item>();
var processed = items
    .Filter(x => x.IsActive)
    .Map(x => ProcessItem(x));

var reader = processed.GetReader();
// On each frame/update:
var changes = reader.GetChanges(AdaptiveToken.Top);
ApplyChanges(changes);
```

### Nested Collections (SelectMany)

```csharp
var outer = new ChangeableHashSet<IAdaptiveHashSet<int>>();
var flattened = outer.SelectMany(x => x);
// flattened updates when outer or any inner set changes
```

### Constant Optimization

```fsharp
// Check if value is constant (never changes)
if myAVal.IsConstant then
    // Can cache result permanently
```

## Gotchas

1. **Always use transactions** - mutations outside `transact`/`Adaptive.Transact` throw
2. **Force outside transactions** - calling `GetValue`/`AVal.force` inside transact can cause reentrancy issues
3. **Dispose readers** - `IStreamReader` and collection readers hold references; dispose when done
4. **astream is experimental** - namespace is `Aardvark.Base.Incremental.Experimental`, API may change
5. **Proc vs Async** - Proc is simpler continuation-based; use for UI/rendering pipelines, Async for I/O
6. **Controller state** - `ControllerState` tracks pulled values between evaluations; initialize via `controller.Run`

## See Also

- [FSHARP_INTEROP.md](FSHARP_INTEROP.md) - F# modules and functional patterns
- FSharp.Data.Adaptive documentation: https://fsprojects.github.io/FSharp.Data.Adaptive/
