using Aardvark.Base;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    [NonParallelizable]
    public class IntrospectionTests
    {
        private static readonly Assembly s_assembly = typeof(IntrospectionTests).Assembly;

        [Test]
        public void MethodAttributeCacheMissStoresEachDeclaringTypeOnce()
        {
            WithFreshCache<MethodCacheTestAttribute>(() =>
            {
                var miss = Query();
                AssertExpectedMethods(miss);

                var lines = File.ReadAllLines(GetSingleCacheFile<MethodCacheTestAttribute>());
                Assert.That(lines[0], Does.StartWith("version 1 "));

                var expectedTypes = miss
                    .Select(result => result.Item1.DeclaringType.AssemblyQualifiedName)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();

                Assert.AreEqual(2, expectedTypes.Length);
                CollectionAssert.AreEqual(expectedTypes, lines.Skip(1).ToArray());

                var hit = Query();
                AssertExpectedMethods(hit);
                CollectionAssert.AreEqual(MethodKeys(miss), MethodKeys(hit));

                var multiple = miss.Single(result => result.Item1.Name == nameof(MethodCacheBaseFixture.BaseInstance));
                CollectionAssert.AreEquivalent(
                    new[] { "base-first", "base-second" },
                    multiple.Item2.Select(attribute => attribute.Name).ToArray());
            });
        }

        [Test]
        public void LegacyMethodAttributeCacheLinesAreDeduplicated()
        {
            WithFreshCache<MethodCacheTestAttribute>(() =>
            {
                var miss = Query();
                AssertExpectedMethods(miss);

                var cacheFile = GetSingleCacheFile<MethodCacheTestAttribute>();
                var lines = File.ReadAllLines(cacheFile);
                Assert.AreEqual(3, lines.Length);

                File.WriteAllLines(cacheFile, new[]
                {
                    lines[0],
                    lines[1],
                    lines[1],
                    lines[2],
                    lines[1],
                    lines[2]
                });

                var hit = Query();
                AssertExpectedMethods(hit);
                CollectionAssert.AreEqual(MethodKeys(miss), MethodKeys(hit));
            });
        }

        [Test]
        public void TypeAttributeFailuresPreservePartialResultsWithoutCaching()
        {
            WithFreshCache<RecoverableTypeAttribute>(() =>
            {
                RecoverableTypeAttribute.ThrowOnBad = true;
                var (partial, report) = CaptureReport(() =>
                    Introspection.GetAllTypesWithAttribute<RecoverableTypeAttribute>(s_assembly));

                CollectionAssert.AreEqual(
                    new[] { typeof(RecoverableTypeGoodFixture) },
                    partial.Select(result => result.Item1).ToArray());
                Assert.That(GetCacheFiles<RecoverableTypeAttribute>(), Is.Empty);
                StringAssert.Contains("type attribute construction: 5 failure(s), 1 unique diagnostic(s)", report);
                Assert.AreEqual(1, CountOccurrences(report, RecoverableTypeAttribute.FailureMessage));

                RecoverableTypeAttribute.ThrowOnBad = false;
                var recovered = Introspection.GetAllTypesWithAttribute<RecoverableTypeAttribute>(s_assembly);
                Assert.AreEqual(6, recovered.Length);
                Assert.That(GetCacheFiles<RecoverableTypeAttribute>(), Has.Length.EqualTo(1));

                var hit = Introspection.GetAllTypesWithAttribute<RecoverableTypeAttribute>(s_assembly);
                CollectionAssert.AreEqual(
                    recovered.Select(result => result.Item1).ToArray(),
                    hit.Select(result => result.Item1).ToArray());
            }, () => RecoverableTypeAttribute.ThrowOnBad = false);
        }

        [Test]
        public void MethodAttributeCacheFailuresRetryLiveAndRecover()
        {
            WithFreshCache<RecoverableMethodAttribute>(() =>
            {
                RecoverableMethodAttribute.ThrowOnBad = false;
                var initial = Introspection.GetAllMethodsWithAttribute<RecoverableMethodAttribute>(s_assembly);
                Assert.AreEqual(6, initial.Length);
                Assert.That(GetCacheFiles<RecoverableMethodAttribute>(), Has.Length.EqualTo(1));

                RecoverableMethodAttribute.ThrowOnBad = true;
                var (partial, report) = CaptureReport(() =>
                    Introspection.GetAllMethodsWithAttribute<RecoverableMethodAttribute>(s_assembly));

                CollectionAssert.AreEqual(
                    new[] { nameof(RecoverableMethodFixture.Good) },
                    partial.Select(result => result.Item1.Name).ToArray());
                StringAssert.Contains("Retrying incomplete cache query live", report);
                StringAssert.Contains("method attribute construction: 10 failure(s), 1 unique diagnostic(s)", report);
                Assert.AreEqual(1, CountOccurrences(report, RecoverableMethodAttribute.FailureMessage));
                Assert.That(GetCacheFiles<RecoverableMethodAttribute>(), Is.Empty);

                RecoverableMethodAttribute.ThrowOnBad = false;
                var recovered = Introspection.GetAllMethodsWithAttribute<RecoverableMethodAttribute>(s_assembly);
                Assert.AreEqual(6, recovered.Length);
                Assert.That(GetCacheFiles<RecoverableMethodAttribute>(), Has.Length.EqualTo(1));

                var hit = Introspection.GetAllMethodsWithAttribute<RecoverableMethodAttribute>(s_assembly);
                CollectionAssert.AreEqual(MethodKeys(recovered), MethodKeys(hit));
            }, () => RecoverableMethodAttribute.ThrowOnBad = false);
        }

        [Test]
        public void ReflectionTypeLoadDiagnosticsAreBoundedAndRecoverable()
        {
            WithFreshCache<LoaderPartialAttribute>(() =>
            {
                var assembly = new ControllableAssembly(
                    "IntrospectionLoaderFixture",
                    new[] { typeof(LoaderPartialFixture) },
                    new Exception[]
                    {
                        new FileNotFoundException("shared missing dependency"),
                        new FileNotFoundException("shared missing dependency"),
                        new FileNotFoundException("shared missing dependency"),
                        new TypeLoadException("missing type one"),
                        new TypeLoadException("missing type two"),
                        new TypeLoadException("missing type three"),
                        new TypeLoadException("missing type four"),
                    });

                assembly.ThrowTypeLoadException = true;
                var (partial, report) = CaptureReport(() =>
                    Introspection.GetAllTypesWithAttribute<LoaderPartialAttribute>(assembly));

                CollectionAssert.AreEqual(
                    new[] { typeof(LoaderPartialFixture) },
                    partial.Select(result => result.Item1).ToArray());
                Assert.That(GetCacheFiles<LoaderPartialAttribute>(), Is.Empty);
                StringAssert.Contains("ReflectionTypeLoadException affected 1 assembly type scan(s)", report);
                StringAssert.Contains("loader exceptions: 7 failure(s), 5 unique diagnostic(s)", report);
                StringAssert.Contains("2 additional unique diagnostic(s) omitted", report);
                Assert.AreEqual(1, CountOccurrences(report, "shared missing dependency"));
                StringAssert.DoesNotContain("   at ", report);
                Assert.LessOrEqual(report.Split('\n').Length, 12);

                assembly.ThrowTypeLoadException = false;
                var recovered = Introspection.GetAllTypesWithAttribute<LoaderPartialAttribute>(assembly);
                Assert.AreEqual(1, recovered.Length);
                Assert.That(GetCacheFiles<LoaderPartialAttribute>(), Has.Length.EqualTo(1));

                var hit = Introspection.GetAllTypesWithAttribute<LoaderPartialAttribute>(assembly);
                CollectionAssert.AreEqual(
                    recovered.Select(result => result.Item1).ToArray(),
                    hit.Select(result => result.Item1).ToArray());
            });
        }

        [Test]
        public void MethodEnumerationFailuresPreserveOtherTypesAndRecover()
        {
            WithFreshCache<MethodEnumerationAttribute>(() =>
            {
                var failingType = new RecoverableMethodsType(typeof(MethodEnumerationFailingFixture));
                var assembly = new ControllableAssembly(
                    "IntrospectionMethodEnumerationFixture",
                    new Type[] { typeof(MethodEnumerationGoodFixture), failingType },
                    Array.Empty<Exception>());

                failingType.ThrowOnGetMethods = true;
                var (partial, report) = CaptureReport(() =>
                    Introspection.GetAllMethodsWithAttribute<MethodEnumerationAttribute>(assembly));

                CollectionAssert.AreEqual(
                    new[] { nameof(MethodEnumerationGoodFixture.Good) },
                    partial.Select(result => result.Item1.Name).ToArray());
                StringAssert.Contains("method enumeration: 1 failure(s), 1 unique diagnostic(s)", report);
                Assert.That(GetCacheFiles<MethodEnumerationAttribute>(), Is.Empty);

                failingType.ThrowOnGetMethods = false;
                var recovered = Introspection.GetAllMethodsWithAttribute<MethodEnumerationAttribute>(assembly);
                CollectionAssert.AreEquivalent(
                    new[]
                    {
                        nameof(MethodEnumerationGoodFixture.Good),
                        nameof(MethodEnumerationFailingFixture.Recovered),
                    },
                    recovered.Select(result => result.Item1.Name).ToArray());
                Assert.That(GetCacheFiles<MethodEnumerationAttribute>(), Has.Length.EqualTo(1));

                var hit = Introspection.GetAllMethodsWithAttribute<MethodEnumerationAttribute>(assembly);
                CollectionAssert.AreEqual(MethodKeys(recovered), MethodKeys(hit));
            });
        }

        private static (MethodInfo, MethodCacheTestAttribute[])[] Query()
            => Introspection.GetAllMethodsWithAttribute<MethodCacheTestAttribute>(s_assembly);

        private static void AssertExpectedMethods((MethodInfo, MethodCacheTestAttribute[])[] results)
        {
            Assert.AreEqual(3, results.Length);
            Assert.AreEqual(results.Length, results.Select(result => result.Item1).Distinct().Count());
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(MethodCacheBaseFixture.BaseInstance),
                    nameof(MethodCacheBaseFixture.BaseStatic),
                    nameof(MethodCacheDerivedFixture.DerivedInstance)
                },
                results.Select(result => result.Item1.Name).ToArray());
        }

        private static string[] MethodKeys<TAttribute>((MethodInfo, TAttribute[])[] results)
            => results
                .Select(result => $"{result.Item1.DeclaringType.AssemblyQualifiedName}|{result.Item1.MetadataToken}")
                .ToArray();

        private static void WithFreshCache<TAttribute>(Action action, Action cleanup = null)
        {
            DeleteCacheFiles<TAttribute>();
            try
            {
                action();
            }
            finally
            {
                cleanup?.Invoke();
                DeleteCacheFiles<TAttribute>();
            }
        }

        private static string GetSingleCacheFile<TAttribute>()
        {
            var files = GetCacheFiles<TAttribute>();
            Assert.AreEqual(1, files.Length);
            return files[0];
        }

        private static string[] GetCacheFiles<TAttribute>()
        {
            if (!Directory.Exists(Introspection.CacheDirectory)) return Array.Empty<string>();

            var guid = typeof(TAttribute).FullName.ToGuid();
            return Directory.GetFiles(
                Introspection.CacheDirectory,
                $"*_{guid}.query",
                SearchOption.TopDirectoryOnly);
        }

        private static void DeleteCacheFiles<TAttribute>()
        {
            foreach (var file in GetCacheFiles<TAttribute>()) File.Delete(file);
        }

        private static (T Result, string Report) CaptureReport<T>(Func<T> action)
        {
            var previousRootTarget = Report.RootTarget;
            var output = new StringBuilder();
            var target = new TextLogTarget(
                (threadIndex, type, level, message) => output.Append(message))
            {
                Verbosity = int.MaxValue,
                LogCompleteLinesOnly = true,
            };
            target.PrefixFun = _ => "";

            try
            {
                Report.RootTarget = target;
                return (action(), output.ToString());
            }
            finally
            {
                target.Dispose();
                Report.RootTarget = previousRootTarget;
            }
        }

        private static int CountOccurrences(string text, string value)
        {
            var count = 0;
            var offset = 0;
            while ((offset = text.IndexOf(value, offset, StringComparison.Ordinal)) >= 0)
            {
                count++;
                offset += value.Length;
            }
            return count;
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
        private sealed class MethodCacheTestAttribute : Attribute
        {
            public string Name { get; }

            public MethodCacheTestAttribute(string name)
            {
                Name = name;
            }
        }

        private class MethodCacheBaseFixture
        {
            [MethodCacheTest("base-first")]
            [MethodCacheTest("base-second")]
            public void BaseInstance() { }

            [MethodCacheTest("base-static")]
            public static void BaseStatic() { }
        }

        private sealed class MethodCacheDerivedFixture : MethodCacheBaseFixture
        {
            [MethodCacheTest("derived")]
            public void DerivedInstance() { }
        }

        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        private sealed class RecoverableTypeAttribute : Attribute
        {
            public const string FailureMessage = "recoverable type attribute failure";
            public static bool ThrowOnBad;

            public RecoverableTypeAttribute(string value)
            {
                if (ThrowOnBad && value == "bad")
                    throw new InvalidOperationException(FailureMessage);
            }
        }

        [RecoverableType("good")]
        private sealed class RecoverableTypeGoodFixture { }

        [RecoverableType("bad")]
        private sealed class RecoverableTypeBadFixture1 { }

        [RecoverableType("bad")]
        private sealed class RecoverableTypeBadFixture2 { }

        [RecoverableType("bad")]
        private sealed class RecoverableTypeBadFixture3 { }

        [RecoverableType("bad")]
        private sealed class RecoverableTypeBadFixture4 { }

        [RecoverableType("bad")]
        private sealed class RecoverableTypeBadFixture5 { }

        [AttributeUsage(AttributeTargets.Method, Inherited = false)]
        private sealed class RecoverableMethodAttribute : Attribute
        {
            public const string FailureMessage = "recoverable method attribute failure";
            public static bool ThrowOnBad;

            public RecoverableMethodAttribute(string value)
            {
                if (ThrowOnBad && value == "bad")
                    throw new InvalidOperationException(FailureMessage);
            }
        }

        private sealed class RecoverableMethodFixture
        {
            [RecoverableMethod("good")]
            public void Good() { }

            [RecoverableMethod("bad")]
            public void Bad1() { }

            [RecoverableMethod("bad")]
            public void Bad2() { }

            [RecoverableMethod("bad")]
            public void Bad3() { }

            [RecoverableMethod("bad")]
            public void Bad4() { }

            [RecoverableMethod("bad")]
            public void Bad5() { }
        }

        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        private sealed class LoaderPartialAttribute : Attribute { }

        [LoaderPartial]
        private sealed class LoaderPartialFixture { }

        [AttributeUsage(AttributeTargets.Method, Inherited = false)]
        private sealed class MethodEnumerationAttribute : Attribute { }

        private sealed class MethodEnumerationGoodFixture
        {
            [MethodEnumeration]
            public void Good() { }
        }

        private sealed class MethodEnumerationFailingFixture
        {
            [MethodEnumeration]
            public void Recovered() { }
        }

        private sealed class RecoverableMethodsType : TypeDelegator
        {
            public bool ThrowOnGetMethods { get; set; }

            public RecoverableMethodsType(Type delegatingType)
                : base(delegatingType)
            {
            }

            public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
            {
                if (ThrowOnGetMethods)
                    throw new InvalidOperationException("recoverable method enumeration failure");
                return base.GetMethods(bindingAttr);
            }
        }

        private sealed class ControllableAssembly : Assembly
        {
            private readonly AssemblyName m_name;
            private readonly Type[] m_types;
            private readonly Exception[] m_loaderExceptions;

            public bool ThrowTypeLoadException { get; set; }

            public ControllableAssembly(string name, Type[] types, Exception[] loaderExceptions)
            {
                m_name = new AssemblyName(name) { Version = new Version(1, 0, 0, 0) };
                m_types = types;
                m_loaderExceptions = loaderExceptions;
            }

            public override string FullName => m_name.FullName;
            public override string Location => s_assembly.Location;

            public override AssemblyName GetName(bool copiedName)
                => new(m_name.FullName);

            public override Type[] GetTypes()
            {
                if (!ThrowTypeLoadException) return m_types;

                var partialTypes = new Type[m_types.Length + 1];
                Array.Copy(m_types, partialTypes, m_types.Length);
                throw new ReflectionTypeLoadException(partialTypes, m_loaderExceptions);
            }
        }
    }
}
