using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using Aardvark.Base;

namespace Aardvark.Tests.Extensions
{
    static class Arrays
    {
        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var ex = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, ex.ParamName);
        }

        [Test]
        public static void GroupCopiesPreserveUncoveredElementsAndStorage()
        {
            foreach (var core in GroupCopyCores())
            {
                foreach (var source in core.Sources(8))
                foreach (var boundaries in new[]
                {
                    new[] { 2,5 }, new[] { 0,5 }, new[] { 2,8 }, new[] { 2,2,5,5,7 },
                    new[] { 0,0,3,8,8 }, new[] { 4 }, new[] { 0 }, new[] { 8 }, new[] { 0,8 }
                })
                foreach (var mode in new[] { 0,1,2 })
                    CheckGroupCopy(core,source,boundaries,Enumerable.Range(0,boundaries.Length-1).Select(g => mode == 1 || mode == 2 && (g & 1) == 0).ToArray(),"fixed partial/zero/full, mode="+mode);
                foreach (var source in core.Sources(0))
                foreach (var boundaries in new[] { new[] { 0 }, new[] { 0,0,0 } })
                    CheckGroupCopy(core,source,boundaries,Enumerable.Repeat(true,boundaries.Length-1).ToArray(),"empty array/groups");

                var first = new[] { 11 }; var last = new[] { 14 };
                var references = new[] { first,new[] { 12 },new[] { 13 },last };
                var result = (int[][])core.Copy(references,new[] { 1,3 },1,_ => true,false);
                Assert.AreSame(first,result[0],core.Name+" shallow prefix"); Assert.AreSame(last,result[3],core.Name+" shallow suffix");
                result[0][0] = 91; Assert.AreEqual(91,references[0][0],core.Name+" shared element, not deep copy");
                result[0] = new[] { -1 }; Assert.AreSame(first,references[0],core.Name+" independent array storage");
                var covariant = core.Copy(references,new[] { 0,4 },1,_ => false,false,typeof(object));
                Assert.AreEqual(typeof(object[]),covariant.GetType(),core.Name+" result uses generic element type, not runtime source type");
            }
        }

        [Test]
        public static void SeededGroupCopiesMatchCloneAndReverse()
        {
            foreach (var core in GroupCopyCores())
            {
                var random = new Random(157109);
                for (var sample = 0; sample < 256; sample++)
                {
                    var length = random.Next(0,66); var count = random.Next(0,18);
                    var boundaries = Enumerable.Range(0,count+1).Select(_ => random.Next(0,length+1)).OrderBy(x => x).ToArray();
                    if (count > 0 && sample % 4 == 0) { boundaries[0] = 0; boundaries[count] = length; }
                    var randomDecisions = Enumerable.Range(0,count).Select(_ => random.Next(2) != 0).ToArray();
                    foreach (var mode in new[] { 0,1,2,3 })
                    {
                        var decisions = Enumerable.Range(0,count).Select(g => mode == 1 || mode == 2 && (g & 1) == 0 || mode == 3 && randomDecisions[g]).ToArray();
                        foreach (var source in core.Sources(length))
                            CheckGroupCopy(core,source,boundaries,decisions,$"seed=157109, sample={sample}, mode={mode}");
                    }
                }
            }
        }

        [Test]
        public static void GroupCopyPredicatesKeepOrderAndExceptions()
        {
            foreach (var core in GroupCopyCores())
            foreach (var source in new Array[] { Enumerable.Range(11,8).ToArray(), new object[] { new object(),null,"a","b",null,"c","d","e" } })
            foreach (var untyped in source is int[] ? new[] { false,true } : new[] { false })
            {
                var boundaries = new[] { 0,0,3,3,8,8 }; var snapshot = (Array)source.Clone(); var savedBoundaries = (int[])boundaries.Clone();
                var calls = new List<int>();
                var result = core.Copy(source,boundaries,5,g => { calls.Add(g); return (g & 1) != 0; },untyped);
                CollectionAssert.AreEqual(Enumerable.Range(0,5),calls,core.Name+" increasing order, including empty groups");
                CollectionAssert.AreEqual(GroupCopyOracle(source,boundaries,new[] { false,true,false,true,false }),result,core.Name+" full coverage control");
                for (var stop = 0; stop < 5; stop++)
                {
                    calls.Clear(); var sentinel = new InvalidOperationException("predicate "+stop);
                    var error = Assert.Throws<InvalidOperationException>(() => core.Copy(source,boundaries,5,g =>
                    {
                        calls.Add(g); if (g == stop) throw sentinel; return true;
                    },untyped));
                    Assert.AreSame(sentinel,error,core.Name+", stop="+stop+", untyped="+untyped);
                    CollectionAssert.AreEqual(Enumerable.Range(0,stop+1),calls,core.Name+" exception stops later predicates");
                    CollectionAssert.AreEqual(snapshot,source,core.Name+" source unchanged on exception");
                    CollectionAssert.AreEqual(savedBoundaries,boundaries,core.Name+" boundaries unchanged on exception");
                }
                var zero = core.Copy(source,new[] { 4 },0,null,untyped);
                Assert.AreEqual(source.Length,zero.Length,core.Name+" zero groups never evaluate a null predicate");
            }
        }

        private static Array GroupCopyOracle(Array source, int[] boundaries, bool[] decisions)
        {
            var expected = (Array)source.Clone();
            for (var g = 0; g < decisions.Length; g++)
                if (decisions[g]) Array.Reverse(expected,boundaries[g],boundaries[g+1]-boundaries[g]);
            return expected;
        }
        private static void CheckGroupCopy(GroupCopyCore core, Array source, int[] boundaries, bool[] decisions, string label)
        {
            var expected = GroupCopyOracle(source,boundaries,decisions); var snapshot = (Array)source.Clone(); var savedBoundaries = (int[])boundaries.Clone();
            foreach (var untyped in core.SupportsUntyped(source) ? new[] { false,true } : new[] { false })
            {
                var context = $"{core.Name}: {source.GetType()}, {label}, boundaries=[{string.Join(",",boundaries)}], reversals=[{string.Join(",",decisions)}], untyped={untyped}";
                var calls = new List<int>(); var result = core.Copy(source,boundaries,decisions.Length,g => { calls.Add(g); return decisions[g]; },untyped);
                Assert.AreNotSame(source,result,context+" independent storage"); Assert.AreEqual(source.GetType(),result.GetType(),context+" result type");
                CollectionAssert.AreEqual(expected,result,context); CollectionAssert.AreEqual(snapshot,source,context+" source unchanged");
                CollectionAssert.AreEqual(savedBoundaries,boundaries,context+" boundaries unchanged");
                CollectionAssert.AreEqual(Enumerable.Range(0,decisions.Length),calls,context+" predicate order");
                if (!source.GetType().GetElementType().IsValueType)
                    for (var i=0;i<source.Length;i++) Assert.AreSame(expected.GetValue(i),result.GetValue(i),context+" shallow reference, index="+i);
                if (result.Length > 0) result.SetValue(null,0);
                CollectionAssert.AreEqual(snapshot,source,context+" result storage mutation is independent");
            }
        }
        private sealed class GroupCopyCore
        {
            public readonly string Name;
            private readonly MethodInfo generic, untyped;
            private readonly Type[] supported;
            public GroupCopyCore(Assembly assembly, string target)
            {
                Name = target;
                generic = assembly.GetType("Aardvark.Base.ArrayFun",true).GetMethods().Single(m => m.Name == "GroupReversedCopy" && m.IsGenericMethodDefinition);
                untyped = assembly.GetType("Aardvark.Base.NonGenericArrayExtensions",true).GetMethod("GroupReversedCopy",new[] { typeof(Array),typeof(int[]),typeof(int),typeof(Func<int,bool>) });
                Assert.AreSame(assembly,generic.DeclaringType.Assembly,target); Assert.AreSame(assembly,untyped.DeclaringType.Assembly,target);
                supported = new[] { typeof(byte),typeof(sbyte),typeof(short),typeof(ushort),typeof(int),typeof(uint),typeof(long),typeof(ulong),typeof(float),typeof(double) }
                    .Concat(new[] { "C3b","C3us","C3ui","C3f","C3d","C4b","C4us","C4ui","C4f","C4d","V2i","V2l","V2f","V2d","V3i","V3l","V3f","V3d","V4i","V4l","V4f","V4d" }.Select(n => assembly.GetType("Aardvark.Base."+n,true))).ToArray();
            }
            public bool SupportsUntyped(Array source) => supported.Contains(source.GetType().GetElementType());
            public IEnumerable<Array> Sources(int length)
            {
                foreach (var type in supported)
                {
                    var array = Array.CreateInstance(type,length); var fields = type.GetFields(BindingFlags.Public|BindingFlags.Instance);
                    for (var i=0;i<length;i++)
                    {
                        object value;
                        if (type.IsPrimitive) value = Convert.ChangeType(i+11,type);
                        else
                        {
                            value = Activator.CreateInstance(type); Assert.IsTrue(fields.Length > 0,Name+" structured fixture fields");
                            for (var j=0;j<fields.Length;j++) fields[j].SetValue(value,Convert.ChangeType((i*17+j*13)%97+1,fields[j].FieldType));
                        }
                        array.SetValue(value,i);
                    }
                    yield return array;
                }
                yield return Enumerable.Range(0,length).Select(i => i%3 == 1 ? null : new[] { i+11 }).ToArray();
                yield return Enumerable.Range(0,length).Select(i => i%3 == 1 ? null : "entry "+i).ToArray();
                yield return Enumerable.Range(0,length).Select(i => i%3 == 1 ? null : (object)new[] { i+11 }).ToArray();
            }
            public Array Copy(Array source, int[] boundaries, int count, Func<int,bool> predicate, bool forward, Type element = null)
            {
                try { return (Array)(forward ? untyped : generic.MakeGenericMethod(element ?? source.GetType().GetElementType())).Invoke(null,new object[] { source,boundaries,count,predicate }); }
                catch (TargetInvocationException e) when (e.InnerException != null) { ExceptionDispatchInfo.Capture(e.InnerException).Throw(); throw; }
            }
        }
        private static IEnumerable<GroupCopyCore> GroupCopyCores()
        {
            foreach (var target in new[] { "net10.0","netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(ArrayFun).Assembly.Location),"..",target,"Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path),"Required sibling core: "+path); var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0",assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,path);
                yield return new GroupCopyCore(assembly,target);
            }
        }

        [Test]
        public unsafe static void CopyArrayToNative()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[3];

            fixed (ushort* ptr = dst)
            {
                src.CopyTo(1, 3, (IntPtr)ptr);
            }

            Assert.AreEqual(src[1], dst[0]);
            Assert.AreEqual(src[2], dst[1]);
            Assert.AreEqual(src[3], dst[2]);
        }

        [Test]
        public unsafe static void CopyNativeToArray()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[5];

            fixed (ushort* ptr = src)
            {
                ((IntPtr)ptr).CopyTo(dst, 1, 3);
            }

            Assert.AreEqual(src[0], dst[1]);
            Assert.AreEqual(src[1], dst[2]);
            Assert.AreEqual(src[2], dst[3]);
        }

        [Test]
        public unsafe static void CopyNativeToNative()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[src.Length];

            fixed (ushort* srcPtr = src, dstPtr = dst)
            {
                ((IntPtr)srcPtr).CopyTo((IntPtr)dstPtr, sizeof(ushort) * src.Length);
            }

            for (int i = 0; i < src.Length; i++)
                Assert.AreEqual(src[i], dst[i]);
        }

        [Test]
        public static void TakeFractionValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            CollectionAssert.AreEqual(Array.Empty<int>(), array.TakeFraction(0.0).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 2 }, array.TakeFraction(0.4).ToArray());
            CollectionAssert.AreEqual(array, array.TakeFraction(1.0).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.TakeFraction(0.5).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("fraction", () => array.TakeFraction(double.NaN).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("fraction", () => array.TakeFraction(-0.1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("fraction", () => array.TakeFraction(1.1).ToArray());
        }

        [Test]
        public static void SkipLastValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4 };

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, array.SkipLast(1).ToArray());
            CollectionAssert.AreEqual(array, array.SkipLast(0).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 2 }, array.SkipLast(2).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.SkipLast(4).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.SkipLast(5).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.SkipLast(5L).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.SkipLast(1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.SkipLast(-1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.SkipLast(-1L).ToArray());
        }

        [Test]
        public static void ElementsValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, array.Elements(1, 3).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.Elements(array.LongLength, 0).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.Elements(0, 0).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.Elements(-1, 0).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.Elements(array.LongLength + 1, 0).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.Elements(0, -1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.Elements(1, array.LongLength).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.Elements(1, long.MaxValue).ToArray());
        }

        [Test]
        public static void ElementsWhereIntValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            Func<int, bool> even = x => x % 2 == 0;

            CollectionAssert.AreEqual(new[] { 2, 4 }, array.ElementsWhere(1, 4, even).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.ElementsWhere(array.Length, 0, even).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.ElementsWhere(0, 0, even).ToArray());
            AssertParamName<ArgumentNullException>("predicate", () => array.ElementsWhere(0, 0, (Func<int, bool>)null).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(-1, 0, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(array.Length + 1, 0, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(0, -1, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1, array.Length, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1, int.MaxValue, even).ToArray());
        }

        [Test]
        public static void ElementsWhereLongValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            Func<int, bool> even = x => x % 2 == 0;

            CollectionAssert.AreEqual(new[] { 2, 4 }, array.ElementsWhere(1L, 4L, even).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.ElementsWhere(array.LongLength, 0L, even).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.ElementsWhere(0L, 0L, even).ToArray());
            AssertParamName<ArgumentNullException>("predicate", () => array.ElementsWhere(0L, 0L, (Func<int, bool>)null).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(-1L, 0L, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(array.LongLength + 1, 0L, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(0L, -1L, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1L, array.LongLength, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1L, long.MaxValue, even).ToArray());
        }
    }
}
