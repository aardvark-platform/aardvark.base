using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class Meta
    {
        #region MetaTypes

        public class SimpleType
        {
            public string Name { get; set; }
            public string Char { get; set; }
            public string Read { get; set; }
            public string Caps { get; set; }
            public bool IsInteger { get; set; }
            public bool IsReal { get; set; }
            public bool IsCharOrString { get; set; }


            public SimpleType()
            { }

            public SimpleType(string name)
            {
                Name = name;
                Char = null;
                Read = null;
            }
        }

        public class TensorType : SimpleType
        {
            public SimpleType FieldType { get; set; }

            public TensorType(string name)
                : base(name)
            {
                Read = "Read" + name;
            }
        }

        public class GenericTensorType : SimpleType
        {
            public SimpleType DataType { get; set; }
            public SimpleType ViewType { get; set; }
            public SimpleType IndexType { get; set; }
            public SimpleType IntIndexType { get; set; }
            public int Dim { get; set; }

            public GenericTensorType(string name)
                : base(name)
            { }
        }

        public class VecType : TensorType
        {
            public int Len { get; set; }
            public string[] Fields { get; set; }

            public VecType(string name)
                : base(name)
            { }
        }

        public struct MatDims
        {
            public int Rows;
            public int Cols;
            public MatDims(int rows, int cols) { Rows = rows; Cols = cols; }
        }

        public class MatType : TensorType
        {
            public int Rows { get; set; }
            public int Cols { get; set; }

            public MatDims Dims { get { return new MatDims(Rows, Cols); } }

            public MatType(string name)
                : base(name)
            { }
        }

        public class TrafoType : SimpleType
        {
            public SimpleType MatType { get; set; }

            public TrafoType(string name)
                : base(name)
            {
                Read = "Read" + name;
            }
        }

        public class RangeType : SimpleType
        {
            public SimpleType LimitType { get; set; }

            public RangeType(string name)
                : base(name)
            {
                Read = "Read" + name;
            }
        }

        public class ColorType : TensorType
        {
            public int Len { get; set; }
            public string[] Channels { get; set; }
            public string[] Fields { get; set; }
            public bool HasAlpha { get; set; }
            public string MinValue { get; set; }
            public string MaxValue { get; set; }

            public ColorType(string name)
                : base(name)
            { }
        }

        #endregion

        public static readonly SimpleType BoolType = new SimpleType()
        {
            Name = "bool",
            Caps = "Bool",
            Read = "ReadBoolean",
        };

        public static readonly SimpleType ByteType = new SimpleType()
        {
            Name = "byte",
            Caps = "Byte",
            Char = "b",
            Read = "ReadByte",
            IsInteger = true,
        };

        public static readonly SimpleType SByteType = new SimpleType()
        {
            Name = "sbyte",
            Caps = "SByte",
            Char = "sb",
            Read = "ReadSByte",
            IsInteger = true,
        };

        public static readonly SimpleType ShortType = new SimpleType()
        {
            Name = "short",
            Caps = "Short",
            Char = "s",
            Read = "ReadInt16",
            IsInteger = true,
        };

        public static readonly SimpleType UShortType = new SimpleType()
        {
            Name = "ushort",
            Caps = "UShort",
            Char = "us",
            Read = "ReadUInt16",
            IsInteger = true,
        };

        public static readonly SimpleType IntType = new SimpleType()
        {
            Name = "int",
            Caps = "Int",
            Char = "i",
            Read = "ReadInt32",
            IsInteger = true,
        };

        public static readonly SimpleType UIntType = new SimpleType()
        {
            Name = "uint",
            Caps = "UInt",
            Char = "ui",
            Read = "ReadUInt32",
            IsInteger = true,
        };

        public static readonly SimpleType LongType = new SimpleType()
        {
            Name = "long",
            Caps = "Long",
            Char = "l",
            Read = "ReadInt64",
            IsInteger = true,
        };

        public static readonly SimpleType ULongType = new SimpleType()
        {
            Name = "ulong",
            Caps = "ULong",
            Char = "ul",
            Read = "ReadUInt64",
            IsInteger = true,
        };

        public static readonly SimpleType FloatType = new SimpleType()
        {
            Name = "float",
            Caps = "Float",
            Char = "f",
            Read = "ReadSingle",
            IsReal = true,
        };

        public static readonly SimpleType DoubleType = new SimpleType()
        {
            Name = "double",
            Caps = "Double",
            Char = "d",
            Read = "ReadDouble",
            IsReal = true,
        };

        public static readonly SimpleType DecimalType = new SimpleType()
        {
            Name = "decimal",
            Caps = "Decimal",
            Read = "ReadDecimal",
        };

        public static readonly SimpleType CharType = new SimpleType()
        {
            Name = "Char",
            Read = "ReadChar",
            IsCharOrString = true,
        };

        public static readonly SimpleType StringType = new SimpleType()
        {
            Name = "String",
            Read = "ReadString",
            IsCharOrString = true,
        };

        public static readonly SimpleType TypeType = new SimpleType()
        {
            Name = "Type",
            Read = "ReadType",
        };

        public static readonly SimpleType GuidType = new SimpleType()
        {
            Name = "Guid",
            Read = "ReadGuid",
        };

        public static readonly SimpleType FractionType = new SimpleType()
        {
            Name = "Fraction",
            Read = "ReadFraction",
        };

        public static readonly SimpleType DateTimeType = new SimpleType()
        {
            Name = "DateTime",
        };

        public static readonly SimpleType TimeSpanType = new SimpleType()
        {
            Name = "TimeSpan",
        };

        public static readonly SimpleType[] TextTypes = new[]
        {
            CharType, StringType,
        };

        public static readonly SimpleType[] SystemTypes = new[]
        {
            CharType, StringType, TypeType, GuidType,
        };

        public static readonly SimpleType[] TimeTypes = new[]
        {
            DateTimeType, TimeSpanType,
        };

        public static readonly SimpleType Euclidean3fType = new SimpleType()
        {
            Name = "Euclidean3f",
            Read = "ReadEuclidean3f",
        };

        public static readonly SimpleType Euclidean3dType = new SimpleType()
        {
            Name = "Euclidean3d",
            Read = "ReadEuclidean3d",
        };

        public static readonly SimpleType Rot2fType = new SimpleType()
        {
            Name = "Rot2f",
            Read = "ReadRot2f",
        };

        public static readonly SimpleType Rot2dType = new SimpleType()
        {
            Name = "Rot2d",
            Read = "ReadRot2d",
        };

        public static readonly SimpleType Rot3fType = new SimpleType()
        {
            Name = "Rot3f",
            Read = "ReadRot3f",
        };

        public static readonly SimpleType Rot3dType = new SimpleType()
        {
            Name = "Rot3d",
            Read = "ReadRot3d",
        };

        public static readonly SimpleType Scale3fType = new SimpleType()
        {
            Name = "Scale3f",
            Read = "ReadScale3f",
        };

        public static readonly SimpleType Scale3dType = new SimpleType()
        {
            Name = "Scale3d",
            Read = "ReadScale3d",
        };

        public static readonly SimpleType Shift3fType = new SimpleType()
        {
            Name = "Shift3f",
            Read = "ReadShift3f",
        };

        public static readonly SimpleType Shift3dType = new SimpleType()
        {
            Name = "Shift3d",
            Read = "ReadShift3d",
        };

        public static readonly SimpleType Trafo2fType = new SimpleType()
        {
            Name = "Trafo2f",
            Read = "ReadTrafo2f",
        };
        
        public static readonly SimpleType Trafo2dType = new SimpleType()
        {
            Name = "Trafo2d",
            Read = "ReadTrafo2d",
        };

        public static readonly SimpleType Trafo3fType = new SimpleType()
        {
            Name = "Trafo3f",
            Read = "ReadTrafo3f",
        };
        
        public static readonly SimpleType Trafo3dType = new SimpleType()
        {
            Name = "Trafo3d",
            Read = "ReadTrafo3d",
        };

        public static readonly SimpleType[] IntegerTypes = new SimpleType[]
        {
            ByteType, SByteType, ShortType, UShortType,
            IntType, UIntType, LongType, ULongType,
        };

        public static readonly SimpleType[] IndexTypes = new SimpleType[]
        {
            IntType, LongType,
        };

        public static readonly SimpleType[] RealTypes = new SimpleType[]
        {
            FloatType, DoubleType,
        };

        public static readonly SimpleType[] SignedTypes = new SimpleType[]
        {
            SByteType, ShortType, IntType, LongType,
            FloatType, DoubleType, DecimalType,
        };

        public static readonly SimpleType[] UnsignedTypes = new SimpleType[]
        {
            ByteType, UShortType, UIntType, ULongType,
        };

        public static readonly SimpleType[] TrafoTypes = new SimpleType[]
        {
            Euclidean3fType, Euclidean3dType,
            Rot2fType, Rot2dType, Rot3fType, Rot3dType,
            Scale3fType, Scale3dType,
            Shift3fType, Shift3dType,
            Trafo2fType, Trafo2dType,
            Trafo3fType, Trafo3dType,
        };

        public static readonly string[] VecFields = new string[]
        {
            "X", "Y", "Z", "W",
        };

        public static readonly string[] VecArgs = new string[]
        {
            "x", "y", "z", "w",
        };

        public static readonly SimpleType[] VecFieldTypes = new SimpleType[]
        {
            IntType, LongType, FloatType, DoubleType,
        };

        public static readonly SimpleType[] MatFieldTypes = VecFieldTypes;

        public static readonly string[] ColorFields = new string[]
        {
            "R", "G", "B", "A",
        };

        public static readonly string[] ColorArgs = new string[]
        {
            "r", "g", "b", "a",
        };

        public static readonly SimpleType[] ColorFieldTypes = new SimpleType[]
        {
            ByteType, UShortType, UIntType, FloatType, DoubleType
        };

        private static readonly Dictionary<SimpleType, string> ColorFieldTypeMinValue
            = new Dictionary<SimpleType, string>()
            {
                { ByteType, "0" }, { UShortType, "0" },
                { UIntType, "0" }, { FloatType, "0.0f" },
                { DoubleType, "0" },
            };
        private static readonly Dictionary<SimpleType, string> ColorFieldTypeMaxValue
            = new Dictionary<SimpleType, string>()
            {
                { ByteType, "255" }, { UShortType, "65535" },
                { UIntType, "UInt32.MaxValue" }, { FloatType, "1.0f" },
                { DoubleType, "1.0" }
            };

        public static readonly int[] VecTypeDimensions = new[] { 2, 3, 4 };
        private static readonly Dictionary<SimpleType, VecType>[] VecTypeMapArray;
        public static readonly VecType[] VecTypes;

        public static VecType VecTypeOf(int dimensions, SimpleType fieldType)
        {
            return VecTypeMapArray[dimensions][fieldType];
        }

        public static readonly MatDims[] MatTypeDimensions = new[]
        {
            new MatDims(2, 2), new MatDims(2, 3),
            new MatDims(3, 3), new MatDims(3, 4),
            new MatDims(4, 4),
        };

        private static readonly Dictionary<SimpleType, MatType>[,] MatTypeMapArray;

        public static MatType MatTypeOf(int rows, int cols, SimpleType fieldType)
        {
            return MatTypeMapArray[rows, cols][fieldType];
        }

        public static readonly MatType[] MatTypes;
        public static readonly RangeType[] RangeTypes;
        public static readonly RangeType[] BoxTypes;
        public static readonly RangeType[] RangeAndBoxTypes;

        public static readonly int[] ColorTypeDimensions = new[] { 3, 4 };

        private static readonly Dictionary<SimpleType, ColorType>[] ColorTypeMapArray;
        public static readonly ColorType[] ColorTypes;

        public static ColorType ColorTypeOf(int dim, SimpleType fieldType)
        {
            return ColorTypeMapArray[dim][fieldType];
        }

        public static readonly SimpleType[] NumericTypes;
        public static readonly SimpleType[] BuiltInTypes;
        public static readonly SimpleType[] StandardNumericTypes;
        public static readonly SimpleType[] BuiltInNumericTypes;
        public static readonly SimpleType[] ComparableTypes;

        private static readonly Dictionary<SimpleType, SimpleType> ComputationTypeMap
            = new Dictionary<SimpleType, SimpleType>()
            {
                { IntType, DoubleType }, { LongType, DoubleType },
            };

        public static SimpleType ComputationTypeOf(SimpleType type)
        {
            SimpleType computationType;
            if (ComputationTypeMap.TryGetValue(type, out computationType))
                return computationType;
            return type;
        }

        private static readonly Dictionary<SimpleType, SimpleType> HighPrecisionTypeMap
            = new Dictionary<SimpleType, SimpleType>()
            {
                { ByteType, IntType }, { UShortType, IntType }, { UIntType, LongType },
                { IntType, LongType }, { FloatType, DoubleType },
            };

        public static SimpleType HighPrecisionTypeOf(SimpleType type)
        {
            SimpleType result;
            if (HighPrecisionTypeMap.TryGetValue(type, out result))
                return result;
            return type;
        }

        private static readonly Dictionary<SimpleType, SimpleType> SummationTypeMap
            = new Dictionary<SimpleType, SimpleType>()
            {
                { ByteType, IntType }, { SByteType, IntType },
                { ShortType, LongType }, { UShortType, LongType },
                { IntType, LongType }, { UIntType, LongType },
                { FloatType, DoubleType }, { DoubleType, DoubleType },
            };

        public static SimpleType SummationTypeOf(SimpleType type)
        {
            SimpleType summationType;
            if (SummationTypeMap.TryGetValue(type, out summationType))
                return summationType;
            return type;
        }

        public static readonly SimpleType[] StructTypes;


        public static readonly SimpleType TViewType = new SimpleType("Tv");
        public static readonly SimpleType TDataType = new SimpleType("Td");

        public static readonly GenericTensorType VectorType;
        public static readonly GenericTensorType VectorWithViewType;
        public static readonly GenericTensorType MatrixType;
        public static readonly GenericTensorType MatrixWithViewType;
        public static readonly GenericTensorType VolumeType;
        public static readonly GenericTensorType VolumeWithViewType;
        public static readonly GenericTensorType Tensor4Type;
        public static readonly GenericTensorType Tensor4WithViewType;

        public static readonly GenericTensorType[] GenericTensorTypes;

        public static readonly SimpleType[] DirectlyCodeableBuiltinTypes;
        public static readonly SimpleType[] DirectlyCodeableTypes;


        private static readonly Dictionary<string, string> s_typeNameSeparatorMap =
            new Dictionary<string, string>
            {
                { " ", "" },
                { "[]", "Array" },
                { "[,]", "Array2d" },
                { "[,,]", "Array3d" },
                { "<", "_of_" },
                { ",", "_" },
                { ">", "_" },
            };

        private static readonly Dictionary<string, string> s_typeNameIdentifierMap =
            new Dictionary<string, string>();

        public static string GetXmlTypeName(string typeName)
        {
            typeName = typeName.ReplaceIdentifiers(s_typeNameIdentifierMap);
            foreach (var kvp in s_typeNameSeparatorMap)
                typeName = typeName.Replace(kvp.Key, kvp.Value);
            return typeName;
        }

        static Meta()
        {
            #region VecTypes

            var vectorTypes = new List<VecType>();
            VecTypeMapArray = new Dictionary<SimpleType, VecType>[5];

            foreach (var n in VecTypeDimensions)
            {
                var typeMap = new Dictionary<SimpleType, VecType>();
                foreach (var ft in VecFieldTypes)
                {
                    var t = new VecType("V" + n + ft.Char)
                    {
                        Len = n,
                        FieldType = ft,
                        Fields = VecFields.Take(n).ToArray(),
                    };
                    typeMap[ft] = t;
                    vectorTypes.Add(t);
                }
                VecTypeMapArray[n] = typeMap;
            }

            VecTypes = vectorTypes.ToArray();

            #endregion

            #region MatTypes

            var matrixTypes = new List<MatType>();
            MatTypeMapArray = new Dictionary<SimpleType, MatType>[5, 5];

            foreach (var dim in MatTypeDimensions)
            {
                var typeMap = new Dictionary<SimpleType, MatType>();
                foreach (var ft in MatFieldTypes)
                {
                    var t = new MatType("M" + dim.Rows + dim.Cols + ft.Char)
                    { Rows = dim.Rows, Cols = dim.Cols, FieldType = ft };
                    typeMap[ft] = t;
                    matrixTypes.Add(t);
                }
                MatTypeMapArray[dim.Rows, dim.Cols] = typeMap;
            }

            MatTypes = matrixTypes.ToArray();

            #endregion

            VectorType = new GenericTensorType("Vector")
            {
                DataType = TDataType,
                ViewType = TDataType,
                Dim = 1,
                IndexType = LongType,
                IntIndexType = IntType,
            };

            VectorWithViewType = new GenericTensorType("Vector")
            {
                DataType = TDataType,
                ViewType = TViewType,
                Dim = 1,
                IndexType = LongType,
                IntIndexType = IntType,
            };

            MatrixType = new GenericTensorType("Matrix")
            {
                DataType = TDataType,
                ViewType = TDataType,
                Dim = 2,
                IndexType = VecTypeOf(2, LongType),
                IntIndexType = VecTypeOf(2, IntType),
            };

            MatrixWithViewType = new GenericTensorType("Matrix")
            {
                DataType = TDataType,
                ViewType = TViewType,
                Dim = 2,
                IndexType = VecTypeOf(2, LongType),
                IntIndexType = VecTypeOf(2, IntType),
            };

            VolumeType = new GenericTensorType("Volume")
            {
                DataType = TDataType,
                ViewType = TDataType,
                Dim = 3,
                IndexType = VecTypeOf(3, LongType),
                IntIndexType = VecTypeOf(3, IntType),
            };

            VolumeWithViewType = new GenericTensorType("Volume")
            {
                DataType = TDataType,
                ViewType = TViewType,
                Dim = 3,
                IndexType = VecTypeOf(3, LongType),
                IntIndexType = VecTypeOf(3, IntType),
            };

            Tensor4Type = new GenericTensorType("Tensor4")
            {
                DataType = TDataType,
                ViewType = TDataType,
                Dim = 4,
                IndexType = VecTypeOf(4, LongType),
                IntIndexType = VecTypeOf(4, IntType),
            };

            Tensor4WithViewType = new GenericTensorType("Tensor4")
            {
                DataType = TDataType,
                ViewType = TViewType,
                Dim = 4,
                IndexType = VecTypeOf(4, LongType),
                IntIndexType = VecTypeOf(4, IntType),
            };

            GenericTensorTypes = new GenericTensorType[]
            {
                VectorType, VectorWithViewType,
                MatrixType, MatrixWithViewType,
                VolumeType, VolumeWithViewType,
                Tensor4Type, Tensor4WithViewType,
            };

            RangeTypes = GenerateRangeTypes().ToArray();
            BoxTypes = GenerateBoxTypes().ToArray();
            RangeAndBoxTypes = RangeTypes.Concat(BoxTypes).ToArray();

            #region ColorTypes

            var colorTypes = new List<ColorType>();
            ColorTypeMapArray = new Dictionary<SimpleType, ColorType>[5];

            foreach (var n in ColorTypeDimensions)
            {
                var typeMap = new Dictionary<SimpleType, ColorType>();
                foreach (var ft in ColorFieldTypes)
                {
                    var t = new ColorType("C" + n + ft.Char)
                    {
                        Len = n,
                        FieldType = ft,
                        Fields = ColorFields.Take(n).ToArray(),
                        Channels = ColorFields.Take(3).ToArray(),
                        HasAlpha = n == 4,
                        MinValue = ColorFieldTypeMinValue[ft],
                        MaxValue = ColorFieldTypeMaxValue[ft],
                    };
                    typeMap[ft] = t;
                    colorTypes.Add(t);
                }
                ColorTypeMapArray[n] = typeMap;
            }

            ColorTypes = colorTypes.ToArray();

            #endregion

            DirectlyCodeableBuiltinTypes = IntegerTypes.Concat(RealTypes).ToArray();

            BuiltInTypes = BoolType.IntoArray()
                            .Concat(DirectlyCodeableBuiltinTypes)
                            .ToArray();

            StandardNumericTypes = IntegerTypes
                                    .Concat(RealTypes)
                                    .ToArray();

            foreach (var t in StandardNumericTypes)
                s_typeNameIdentifierMap[t.Name] = t.Caps;
            foreach (var t in TextTypes)
                s_typeNameIdentifierMap[t.Name.ToLower()] = t.Name;
            s_typeNameIdentifierMap[BoolType.Name] = BoolType.Caps;

            BuiltInNumericTypes = StandardNumericTypes
                                    .Concat(DecimalType.IntoArray())
                                    .ToArray();

            NumericTypes = StandardNumericTypes
                            .Concat(FractionType.IntoArray())
                            .ToArray();

            ComparableTypes = NumericTypes
                            .Concat(TimeTypes)
                            .ToArray();

            StructTypes = BuiltInTypes
                            .Concat(FractionType.IntoArray())
                            .Concat(SystemTypes)
                            .Concat(VecTypes)
                            .Concat(MatTypes)
                            .Concat(ColorTypes)
                            .Concat(RangeAndBoxTypes)
                            .Concat(TrafoTypes)
                            .ToArray();

            DirectlyCodeableTypes = DirectlyCodeableBuiltinTypes
                            .Concat(FractionType.IntoArray())
                            .Concat(VecTypes)
                            .Concat(MatTypes)
                            .Concat(ColorTypes)
                            .Concat(RangeAndBoxTypes)
                            .Concat(TrafoTypes)
                            .ToArray();
        }

        private static IEnumerable<RangeType> GenerateRangeTypes()
        {
            foreach (var t in IntegerTypes.Concat(RealTypes))
                yield return new RangeType("Range1" + t.Char) { LimitType = t };
        }

        private static IEnumerable<RangeType> GenerateBoxTypes()
        {
            foreach (var vt in VecTypes)
                if (vt.Len < 4)
                    yield return new RangeType("Box" + vt.Len + vt.FieldType.Char)
                    { LimitType = vt };
        }

        public static readonly SimpleType Circle2dType = new SimpleType() { Name = "Circle2d" };
        public static readonly SimpleType Line2dType = new SimpleType() { Name = "Line2d" };
        public static readonly SimpleType Line3dType = new SimpleType() { Name = "Line3d" };
        public static readonly SimpleType Plane2dType = new SimpleType() { Name = "Plane2d" };
        public static readonly SimpleType Plane3dType = new SimpleType() { Name = "Plane3d" };
        public static readonly SimpleType PlaneWithPoint3dType = new SimpleType() { Name = "PlaneWithPoint3d" };
        public static readonly SimpleType Quad2dType = new SimpleType() { Name = "Quad2d" };
        public static readonly SimpleType Quad3dType = new SimpleType() { Name = "Quad3d" };
        public static readonly SimpleType Ray2dType = new SimpleType() { Name = "Ray2d" };
        public static readonly SimpleType Ray3dType = new SimpleType() { Name = "Ray3d" };
        public static readonly SimpleType Sphere3dType = new SimpleType() { Name = "Sphere3d" };
        public static readonly SimpleType Triangle2dType = new SimpleType() { Name = "Triangle2d" };
        public static readonly SimpleType Triangle3dType = new SimpleType() { Name = "Triangle3d" };

        /// <summary>
        /// All geometry types that need to be serialized.
        /// </summary>
        public static readonly SimpleType[] GeometryTypes = new SimpleType[]
        {
            Circle2dType, Line2dType, Line3dType, Plane2dType, Plane3dType, PlaneWithPoint3dType,
            Quad2dType, Quad3dType, Ray2dType, Ray3dType, Sphere3dType,
            Triangle2dType, Triangle3dType
        };

        #region Fun related
        // Instead of manually copy pasting methods defined in the Fun class to work
        // elementwise with vectors and matrices, we simply define the methods we are interested in here.
        // It's a bit verbose since we require a lot of meta information, but much easier and less error-prone than 
        // manually copy pasting the code.

        public class ElementwiseFun
        {
            public enum ParamClass
            {
                Scalar,
                Tensor
            }

            public class Parameter
            {
                public string Name { get; set; }

                public ParamClass Class { get; set; }

                // Parameters can specify an element type
                // If null, it is generic and takes the element type of the current class
                // E.g. for V2i -> int
                public SimpleType ElementType { get; set; }

                public Parameter(string name, ParamClass c, SimpleType t)
                {
                    Name = name;
                    Class = c;
                    ElementType = t;
                }
            }

            public string Name { get; set; }

            // Element type of the result, if null it takes the element type of the current class
            // E.g. for V2i -> int
            public SimpleType ReturnType { get; set; }

            public Parameter[] Parameters { get; set; }

            public bool IsExtension { get; set; }

            // Only valid for the given element types
            public SimpleType[] Domain { get; set; }

            public ElementwiseFun(string name, SimpleType returnType, bool extension,
                                    SimpleType[] domain, params Parameter[] parameters)
            {
                Name = name;
                ReturnType = returnType;
                Parameters = parameters;
                IsExtension = extension;
                Domain = domain;
            }
        }

        public static bool IsScalar(this ElementwiseFun.Parameter p)
        {
            return p.Class == ElementwiseFun.ParamClass.Scalar;
        }

        public static bool IsTensor(this ElementwiseFun.Parameter p)
        {
            return p.Class == ElementwiseFun.ParamClass.Tensor;
        }

        private static ElementwiseFun.Parameter Scalar(string name, SimpleType t = null)
        {
            return new ElementwiseFun.Parameter(name, ElementwiseFun.ParamClass.Scalar, t);
        }

        private static ElementwiseFun.Parameter Tensor(string name, SimpleType t = null)
        {
            return new ElementwiseFun.Parameter(name, ElementwiseFun.ParamClass.Tensor, t);
        }

        private static ElementwiseFun Method(string name, SimpleType returnType,
                                                SimpleType[] domain, params ElementwiseFun.Parameter[] parameters)
        {
            return new ElementwiseFun(name, returnType, true, domain, parameters);
        }

        private static ElementwiseFun Method(string name, SimpleType[] domain, params ElementwiseFun.Parameter[] parameters)
        {
            return Method(name, null, domain, parameters);
        }

        private static ElementwiseFun Method(string name, SimpleType returnType, params ElementwiseFun.Parameter[] parameters)
        {
            return new ElementwiseFun(name, returnType, true, VecFieldTypes, parameters);
        }

        private static ElementwiseFun Method(string name, params ElementwiseFun.Parameter[] parameters)
        {
            return Method(name, VecFieldTypes, parameters);
        }

        private static Dictionary<string, ElementwiseFun[]> getElementwiseFuns()
        {
            var dict = new Dictionary<string, ElementwiseFun[]>();

            void Add(string category, params ElementwiseFun[] funs)
            {
                dict.Add(category, funs);
            }

            SimpleType[] Domain(params SimpleType[] types)
                => types;

            SimpleType[] AllExcept(params SimpleType[] types)
                => types.FoldLeft(VecFieldTypes, (arr, t) => arr.WithRemoved(t));

            SimpleType[] NotReal()
                => AllExcept(FloatType, DoubleType);

            #region Min and Max
            Add(
                "Min and Max",
                Method("Min", Tensor("a"), Tensor("b")),
                Method("Max", Tensor("a"), Tensor("b")),
                Method("Min", Tensor("a"), Tensor("b"), Tensor("c")),
                Method("Max", Tensor("a"), Tensor("b"), Tensor("c")),
                Method("Min", Tensor("a"), Tensor("b"), Tensor("c"), Tensor("d")),
                Method("Max", Tensor("a"), Tensor("b"), Tensor("c"), Tensor("d"))
            );
            #endregion

            #region Abs
            Add("Abs",
                Method("Abs", Tensor("x"))
            );
            #endregion

            #region Rounding
            Add("Rounding",
                Method("Floor", RealTypes, Tensor("x")),
                Method("Ceiling", RealTypes, Tensor("x")),
                Method("Round", RealTypes, Tensor("x")),
                Method("Truncate", RealTypes, Tensor("x"))
            );
            #endregion

            #region Frac
            Add("Frac", Method("Frac", RealTypes, Tensor("x")));
            #endregion

            #region Clamping
            Add("Clamping",
                    Method("Clamp", Tensor("x"), Tensor("a"), Tensor("b")),
                    Method("Clamp", Tensor("x"), Scalar("a"), Scalar("b")),
                    Method("ClampExcl", NotReal(), Tensor("x"), Tensor("a"), Tensor("b")),
                    Method("ClampExcl", NotReal(), Tensor("x"), Scalar("a"), Scalar("b")),
                    Method("ClampWrap", Tensor("x"), Tensor("a"), Tensor("b")),
                    Method("ClampWrap", Tensor("x"), Scalar("a"), Scalar("b")),
                    Method("Saturate", Tensor("x"))
            );
            #endregion

            #region MapToUnitInterval
            Add("MapToUnitInterval",
                Method("MapToUnitInterval", RealTypes,
                       Tensor("t"), Tensor("tMax"), Scalar("repeat", BoolType), Scalar("mirror", BoolType)),
                Method("MapToUnitInterval", RealTypes,
                       Tensor("t"), Tensor("tMax"), Scalar("repeat", BoolType)),
                Method("MapToUnitInterval", RealTypes,
                       Tensor("t"), Tensor("tMax")),
                Method("MapToUnitInterval", RealTypes,
                       Tensor("t"), Tensor("tMin"), Tensor("tMax"))
            );
            #endregion

            #region Sign
            Add("Sign",
                Method("Sign", IntType, Tensor("x")),
                Method("Signumi", IntType, Tensor("x")),
                Method("Signum", Tensor("x"))
            );
            #endregion

            #region Roots
            Add("Roots",
                Method("Sqrt", RealTypes, Tensor("x")),
                Method("Sqrt", DoubleType, NotReal(), Tensor("x")),
                Method("Cbrt", RealTypes, Tensor("x")),
                Method("Cbrt", DoubleType, NotReal(), Tensor("x"))
            );
            #endregion

            #region Square and Power
            Add("Square and Power",
                Method("Square", Tensor("x")),
                Method("Pow", FloatType, AllExcept(DoubleType), Tensor("x"), Tensor("y", FloatType)),
                Method("Pow", DoubleType, AllExcept(FloatType), Tensor("x"), Tensor("y", DoubleType))
            );
            #endregion

            #region Exp and Log
            Add("Exp and Log",
                Method("Exp", RealTypes, Tensor("x")),
                Method("Exp", DoubleType, NotReal(), Tensor("x")),
                Method("Log", RealTypes, Tensor("x")),
                Method("Log", DoubleType, NotReal(), Tensor("x")),
                Method("Log2", RealTypes, Tensor("x")),
                Method("Log2", DoubleType, NotReal(), Tensor("x")),
                Method("Log10", RealTypes, Tensor("x")),
                Method("Log10", DoubleType, NotReal(), Tensor("x")),
                Method("Log", RealTypes, Tensor("x"), Scalar("basis")),
                Method("Log", DoubleType, NotReal(), Tensor("x"), Scalar("basis", DoubleType))
            );
            #endregion

            #region ModP
            Add("ModP", Method("ModP", Tensor("a"), Tensor("b")));
            #endregion

            #region Power of Two
            Add("Power of Two",
                Method("PowerOfTwo", Domain(LongType, FloatType, DoubleType), Tensor("x")),
                Method("NextPowerOfTwo", Domain(IntType, LongType), Tensor("x")),
                Method("PrevPowerOfTwo", Domain(IntType, LongType), Tensor("x"))
            );
            #endregion

            #region Trigonometry
            Add("Trigonometry",
                Method("Sin", RealTypes, Tensor("x")),
                Method("Cos", RealTypes, Tensor("x")),
                Method("Tan", RealTypes, Tensor("x")),
                Method("Asin", RealTypes, Tensor("x")),
                Method("AsinClamped", RealTypes, Tensor("x")),
                Method("Acos", RealTypes, Tensor("x")),
                Method("AcosClamped", RealTypes, Tensor("x")),
                Method("Atan", RealTypes, Tensor("x")),
                Method("Atan2", RealTypes, Tensor("y"), Tensor("x")),
                Method("FastAtan2", RealTypes, Tensor("y"), Tensor("x")),
                Method("Sinh", RealTypes, Tensor("x")),
                Method("Cosh", RealTypes, Tensor("x")),
                Method("Tanh", RealTypes, Tensor("x")),
                Method("Asinh", RealTypes, Tensor("x")),
                Method("Acosh", RealTypes, Tensor("x")),
                Method("Atanh", RealTypes, Tensor("x"))
            );
            #endregion

            #region Interpolation
            Add("Interpolation",
                Method("Lerp", FloatType, AllExcept(DoubleType), Scalar("t", FloatType), Tensor("a"), Tensor("b")),
                Method("Lerp", FloatType, AllExcept(DoubleType), Tensor("t", FloatType), Tensor("a"), Tensor("b")),
                Method("Lerp", DoubleType, AllExcept(FloatType), Scalar("t", DoubleType), Tensor("a"), Tensor("b")),
                Method("Lerp", DoubleType, AllExcept(FloatType), Tensor("t", DoubleType), Tensor("a"), Tensor("b")),
                Method("Smoothstep", RealTypes, Tensor("x"), Tensor("edge0"), Tensor("edge1")),
                Method("Smoothstep", RealTypes, Tensor("x"), Scalar("edge0"), Scalar("edge1")),
                Method("InvLerp", Domain(FloatType), Tensor("y"), Tensor("a"), Tensor("b")),
                Method("InvLerp", DoubleType, AllExcept(FloatType), Tensor("y"), Tensor("a"), Tensor("b"))
            );
            #endregion

            #region Common Divisor and Multiple
            Add("Common Divisor and Multiple",
                Method("GreatestCommonDivisor", Domain(IntType, LongType, UIntType, ULongType), Tensor("a"), Tensor("b")),
                Method("LeastCommonMultiple", Domain(IntType, LongType, UIntType, ULongType), Tensor("a"), Tensor("b"))
            );
            #endregion

            return dict;
        }

        public static readonly Dictionary<string, ElementwiseFun[]> ElementwiseFuns = getElementwiseFuns();

        #endregion
    }
}
