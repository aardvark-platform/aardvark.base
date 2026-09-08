# Aardvark.Base Serialization Reference

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

Reading assigns to the `ref` parameter; writing reads from it.

```csharp
public void Serialize(ICoder coder)
{
    coder.CodeInt(ref _x);
    coder.CodeString(ref _name);
    coder.CodeV3d(ref _position);
}
```

---

## Implementations

### BinaryWritingCoder

```csharp
using (var coder = new BinaryWritingCoder(stream))
{
    object data = myObject;
    coder.Code(ref data);
}
```

### BinaryReadingCoder

```csharp
using (var coder = new BinaryReadingCoder(stream))
{
    object data = null;
    coder.Code(ref data);
    var myObject = (MyType)data;
}
```

### XML Coders

`XmlWritingCoder` / `XmlReadingCoder` implement the same `ICoder` contract for XML streams.

### Network-order primitive streams

`NetworkOrderBinaryReader` and `NetworkOrderBinaryWriter` in `Aardvark.Base.Coder`
read and write a fixed big-endian wire format:

- `short`/`ushort`, `int`/`uint`, and `long`/`ulong` use 16, 32, and 64 bits respectively, most-significant byte first.
- `float` and `double` preserve their complete IEEE 754 bit patterns, including signed zero, infinities, subnormals, and NaN payloads.
- `byte` and `sbyte` are unchanged because they have no byte order.
- `V2f`, `V2d`, `V3f`, `V3d`, `C3f`, and `C4f` writes concatenate scalar components in declaration order; the corresponding available aggregate reads use the same ordering.
- Truncated multi-byte scalar reads throw `EndOfStreamException`.

After construction and stream provisioning, these numeric scalar and aggregate operations are allocation-free. String and character layout remains controlled by the constructor encoding; use `Encoding.BigEndianUnicode` when big-endian UTF-16 is required.

---

## Type-Specific Methods

Primitive method names use C# keywords (`CodeInt`, `CodeLong`), not BCL names
(`CodeInt32`, `CodeInt64`). Aardvark values use `Code<TypeName>`:

```csharp
void CodeInt(ref int value);
void CodeLong(ref long value);
void CodeV3d(ref V3d value);
void CodeM44d(ref M44d value);
```

See [ICoder_auto.cs](../src/Aardvark.Base.IO/ICoder_auto.cs) for the full list.

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

Guard fields by version; do not assume forward compatibility:
```csharp
public void Serialize(ICoder coder)
{
    coder.CodeInt(ref _x);

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

Register names and versions for custom types:

```csharp
coder.Add(new TypeInfo[] {
    new TypeInfo("MyType", typeof(MyType), version: 1),
    // ...
});

coder.Del(typeInfoArray);  // remove registration
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

## See Also

- [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) - All primitives (V3d, M44d, Trafo3d) have `CodeXxx` methods
- [TENSORS.md](TENSORS.md) - N-dimensional tensors serialize via `Code(Type t, ref IArrayVolume volume)`
- [PIXIMAGE.md](PIXIMAGE.md) - `PixImage` serialization for binary save/load workflows
- [COLLECTIONS.md](COLLECTIONS.md) - `Symbol`, `SymbolDict`, `LruCache` serialization patterns
