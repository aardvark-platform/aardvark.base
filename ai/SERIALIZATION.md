# Aardvark.Base Serialization Reference

AI-targeted reference for the ICoder serialization system - bidirectional read/write abstraction.

---

## ICoder Pattern

The ICoder interface provides a unified API for both reading and writing. The same code handles both directions.

```csharp
public partial interface ICoder
{
    bool IsReading { get; }
    bool IsWriting { get; }
    int CoderVersion { get; }
    int MemoryVersion { get; }
    int StreamVersion { get; }

    void Code(ref object obj);
    void CodeT<T>(ref T obj);
    // ... type-specific methods
}
```

### Key Insight
Same method signature for read and write:
- **Writing**: reads value from `ref` parameter, writes to stream
- **Reading**: reads from stream, assigns to `ref` parameter

```csharp
// This code works for BOTH reading and writing
public void Serialize(ICoder coder)
{
    coder.CodeInt32(ref _x);
    coder.CodeString(ref _name);
    coder.CodeV3d(ref _position);
}
```

---

## Implementations

### BinaryWritingCoder

Writes objects to binary stream.

```csharp
using (var coder = new BinaryWritingCoder(stream))
{
    object data = myObject;
    coder.Code(ref data);
}
```

### BinaryReadingCoder

Reads objects from binary stream.

```csharp
using (var coder = new BinaryReadingCoder(stream))
{
    object data = null;
    coder.Code(ref data);
    var myObject = (MyType)data;
}
```

---

## Type-Specific Methods

### Primitive Types
```csharp
void CodeBool(ref bool value);
void CodeByte(ref byte value);
void CodeSByte(ref sbyte value);
void CodeInt16(ref short value);
void CodeUInt16(ref ushort value);
void CodeInt32(ref int value);
void CodeUInt32(ref uint value);
void CodeInt64(ref long value);
void CodeUInt64(ref ulong value);
void CodeFloat(ref float value);
void CodeDouble(ref double value);
void CodeChar(ref char value);
void CodeString(ref string value);
void CodeGuid(ref Guid value);
void CodeType(ref Type value);
void CodeSymbol(ref Symbol value);
```

### Aardvark Types
```csharp
// Vectors
void CodeV2i(ref V2i value);
void CodeV2f(ref V2f value);
void CodeV2d(ref V2d value);
void CodeV3i(ref V3i value);
void CodeV3f(ref V3f value);
void CodeV3d(ref V3d value);
void CodeV4i(ref V4i value);
void CodeV4f(ref V4f value);
void CodeV4d(ref V4d value);

// Matrices
void CodeM22f(ref M22f value);
void CodeM33f(ref M33f value);
void CodeM44f(ref M44f value);
void CodeM22d(ref M22d value);
void CodeM33d(ref M33d value);
void CodeM44d(ref M44d value);

// Transformations
void CodeTrafo2f(ref Trafo2f value);
void CodeTrafo3f(ref Trafo3f value);
void CodeTrafo2d(ref Trafo2d value);
void CodeTrafo3d(ref Trafo3d value);

// Colors
void CodeC3b(ref C3b value);
void CodeC4b(ref C4b value);
void CodeC3f(ref C3f value);
void CodeC4f(ref C4f value);

// Geometric
void CodeBox2i(ref Box2i value);
void CodeBox3i(ref Box3i value);
void CodeBox2f(ref Box2f value);
void CodeBox3f(ref Box3f value);
void CodeBox2d(ref Box2d value);
void CodeBox3d(ref Box3d value);
```

### Collections
```csharp
void CodeT<T>(ref T obj);
void CodeTArray<T>(ref T[] array);
void CodeList_of_T_<T>(ref List<T> list);
void CodeHashSet_of_T_<T>(ref HashSet<T> set);

void Code(Type t, ref Array array);
void Code(Type t, ref IList list);
void Code(Type t, ref IDictionary dict);
```

### Tensors
```csharp
void Code(Type t, ref IArrayVector vector);
void Code(Type t, ref IArrayMatrix matrix);
void Code(Type t, ref IArrayVolume volume);
void Code(Type t, ref IArrayTensor4 tensor4);
void Code(Type t, ref IArrayTensorN tensor);
```

### Struct Arrays
```csharp
void CodeStructArray<T>(ref T[] a) where T : struct;
void CodeStructList<T>(ref List<T> l) where T : struct;
```

---

## Extended Interfaces

### IReadingCoder
```csharp
public interface IReadingCoder : ICoder
{
    // Code count with creation function
    int CodeCount<T>(ref T value, Func<int, T> creator) where T : class;
}
```

### IWritingCoder
```csharp
public interface IWritingCoder : ICoder
{
    // Code count with counting function
    int CodeCount<T>(ref T value, Func<T, int> counter) where T : class;
}
```

---

## Version Handling

ICoder supports versioned serialization:

```csharp
coder.MemoryVersion  // current in-memory format version
coder.StreamVersion  // version in the stream being read
coder.CoderVersion   // coder implementation version
```

Use for backward compatibility:
```csharp
public void Serialize(ICoder coder)
{
    coder.CodeInt32(ref _x);

    if (coder.StreamVersion >= 2)
    {
        coder.CodeString(ref _newField);
    }
    else if (coder.IsReading)
    {
        _newField = "default";  // provide default for old data
    }
}
```

---

## TypeInfo Registration

Register custom types for polymorphic serialization:

```csharp
coder.Add(new TypeInfo[] {
    new TypeInfo(typeof(MyType), "MyType", /* version */ 1),
    // ...
});

coder.Del(typeInfoArray);  // remove registration
```

---

## Usage Patterns

### Basic Serialization
```csharp
// Write
using (var stream = File.Create("data.bin"))
using (var coder = new BinaryWritingCoder(stream))
{
    var position = new V3d(1, 2, 3);
    var name = "test";
    coder.CodeV3d(ref position);
    coder.CodeString(ref name);
}

// Read
using (var stream = File.OpenRead("data.bin"))
using (var coder = new BinaryReadingCoder(stream))
{
    var position = default(V3d);
    var name = default(string);
    coder.CodeV3d(ref position);
    coder.CodeString(ref name);
}
```

### Unified Read/Write Method
```csharp
public class MyData
{
    private V3d _position;
    private string _name;
    private List<int> _values;

    public void Code(ICoder coder)
    {
        coder.CodeV3d(ref _position);
        coder.CodeString(ref _name);
        coder.CodeList_of_T_(ref _values);
    }
}

// Write
myData.Code(writingCoder);

// Read
myData.Code(readingCoder);
```

### Conditional Coding
```csharp
public void Code(ICoder coder)
{
    coder.CodeInt32(ref _count);

    if (coder.IsWriting && _data != null)
    {
        coder.CodeTArray(ref _data);
    }
    else if (coder.IsReading)
    {
        _data = new float[_count];
        coder.CodeTArray(ref _data);
    }
}
```

---

## Special Methods

### Symbol Variants
```csharp
// Symbol known to be from a GUID
void CodeGuidSymbol(ref Symbol v);

// Symbol known to be positive (has string representation)
void CodePositiveSymbol(ref Symbol v);
```

### Set Types
```csharp
void CodeIntSet(ref IntSet v);
void CodeSymbolSet(ref SymbolSet v);
```

### Enum Coding
```csharp
void CodeEnum(Type t, ref object value);
```

---

## Gotchas

1. **Unidirectional Reference Pattern**: The `ref` parameter pattern is elegant but *confusing* for debugging. Read-mode passes `null` into `ref`, write-mode reads from the ref. Always verify `IsReading`/`IsWriting` in conditional logic
2. **Version Mismatch Silent Failures**: If code reads a newer format than `StreamVersion`, old fields stay at default values without warning. Use version guards explicitly; don't assume forward compatibility
3. **Polymorphic Type Registration**: Polymorphic serialization requires exact `TypeInfo` registration. Missing a subclass? It silently serializes as the base type, causing silent data loss on read

---

## See Also

- [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) - All primitives (V3d, M44d, Trafo3d) have `CodeXxx` methods
- [TENSORS.md](TENSORS.md) - N-dimensional tensors serialize via `Code(Type t, ref IArrayVolume volume)`
- [PIXIMAGE.md](PIXIMAGE.md) - `PixImage` serialization for binary save/load workflows
- [COLLECTIONS.md](COLLECTIONS.md) - `Symbol`, `SymbolDict`, `LruCache` serialization patterns
