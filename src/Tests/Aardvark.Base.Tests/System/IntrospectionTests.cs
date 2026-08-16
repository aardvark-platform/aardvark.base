using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
            WithFreshCache(() =>
            {
                var miss = Query();
                AssertExpectedMethods(miss);

                var lines = File.ReadAllLines(GetSingleCacheFile());
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
        public void CurrentMethodAttributeCacheLinesAreDeduplicated()
        {
            WithFreshCache(() =>
            {
                var miss = Query();
                AssertExpectedMethods(miss);

                var cacheFile = GetSingleCacheFile();
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
        public void LegacyMethodCacheIsInvalidated()
        {
            WithFreshCache(() =>
            {
                var miss = Query();
                AssertExpectedMethods(miss);

                var currentFile = GetSingleCacheFile();
                var header = File.ReadLines(currentFile).First();
                File.Delete(currentFile);

                var currentGuid = GetMethodCacheDiscriminator(typeof(MethodCacheTestAttribute)).ToGuid();
                var legacyGuid = typeof(MethodCacheTestAttribute).FullName.ToGuid();
                var legacyFile = currentFile.Replace(currentGuid.ToString(), legacyGuid.ToString());
                File.WriteAllLines(legacyFile, new[] { header });

                var result = Query();
                AssertExpectedMethods(result);
                Assert.That(GetCurrentCacheFiles(typeof(MethodCacheTestAttribute)), Has.Length.EqualTo(1));
            });
        }

        [Test]
        public void TypeAndMethodAttributeQueriesUseIndependentCaches()
        {
            WithFreshCache(() =>
            {
                var types = Introspection.GetAllTypesWithAttribute<MixedCacheTestAttribute>(s_assembly);
                var methods = Introspection.GetAllMethodsWithAttribute<MixedCacheTestAttribute>(s_assembly);

                CollectionAssert.AreEqual(new[] { typeof(MixedCacheFixture) }, types.Select(x => x.Item1).ToArray());
                CollectionAssert.AreEqual(
                    new[] { nameof(MixedCacheFixture.Marked) },
                    methods.Select(x => x.Item1.Name).ToArray()
                );
                Assert.That(GetAllCacheFiles(typeof(MixedCacheTestAttribute)), Has.Length.EqualTo(2));
            }, typeof(MixedCacheTestAttribute));
        }

        [Test]
        public void ForeignAssemblyTypesInCurrentMethodCacheAreIgnored()
        {
            WithFreshCache(() =>
            {
                var miss = Introspection.GetAllMethodsWithAttribute<ObsoleteAttribute>(s_assembly);
                var cacheFile = GetCurrentCacheFiles(typeof(ObsoleteAttribute)).Single();
                File.AppendAllLines(cacheFile, new[] { typeof(Introspection).AssemblyQualifiedName });

                var hit = Introspection.GetAllMethodsWithAttribute<ObsoleteAttribute>(s_assembly);
                CollectionAssert.AreEqual(
                    miss.Select(result => result.Item1).ToArray(),
                    hit.Select(result => result.Item1).ToArray()
                );
                Assert.That(hit.All(result => result.Item1.DeclaringType?.Assembly == s_assembly), Is.True);
            }, typeof(ObsoleteAttribute));
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

        private static string[] MethodKeys((MethodInfo, MethodCacheTestAttribute[])[] results)
            => results
                .Select(result => $"{result.Item1.DeclaringType.AssemblyQualifiedName}|{result.Item1.MetadataToken}")
                .ToArray();

        private const string MethodQueryCacheVersion = "public-declared-v2";

        private static string GetMethodCacheDiscriminator(Type attributeType)
            => $"{MethodQueryCacheVersion}|{attributeType.AssemblyQualifiedName}";

        private static void WithFreshCache(Action action, params Type[] attributeTypes)
        {
            if (attributeTypes.Length == 0)
                attributeTypes = new[] { typeof(MethodCacheTestAttribute) };

            DeleteFixtureCacheFiles(attributeTypes);
            try
            {
                action();
            }
            finally
            {
                DeleteFixtureCacheFiles(attributeTypes);
            }
        }

        private static string GetSingleCacheFile()
        {
            var files = GetCurrentCacheFiles(typeof(MethodCacheTestAttribute));
            Assert.AreEqual(1, files.Length);
            return files[0];
        }

        private static string[] GetCurrentCacheFiles(Type attributeType)
            => GetCacheFiles(GetMethodCacheDiscriminator(attributeType));

        private static string[] GetAllCacheFiles(Type attributeType)
            => GetCacheFiles(attributeType.FullName)
                .Concat(GetCurrentCacheFiles(attributeType))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

        private static string[] GetCacheFiles(string discriminator)
        {
            if (!Directory.Exists(Introspection.CacheDirectory)) return Array.Empty<string>();

            var guid = discriminator.ToGuid();
            return Directory.GetFiles(
                Introspection.CacheDirectory,
                $"*_{guid}.query",
                SearchOption.TopDirectoryOnly);
        }

        private static void DeleteFixtureCacheFiles(IEnumerable<Type> attributeTypes)
        {
            foreach (var file in attributeTypes.SelectMany(GetAllCacheFiles))
                File.Delete(file);
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

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
        private sealed class MixedCacheTestAttribute : Attribute
        {
        }

        [MixedCacheTest]
        private sealed class MixedCacheFixture
        {
            [MixedCacheTest]
            public void Marked() { }
        }
    }
}
