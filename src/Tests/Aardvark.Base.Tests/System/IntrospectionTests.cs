using Aardvark.Base;
using NUnit.Framework;
using System;
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
        public void LegacyMethodAttributeCacheLinesAreDeduplicated()
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

        private static void WithFreshCache(Action action)
        {
            DeleteFixtureCacheFiles();
            try
            {
                action();
            }
            finally
            {
                DeleteFixtureCacheFiles();
            }
        }

        private static string GetSingleCacheFile()
        {
            var files = GetFixtureCacheFiles();
            Assert.AreEqual(1, files.Length);
            return files[0];
        }

        private static string[] GetFixtureCacheFiles()
        {
            if (!Directory.Exists(Introspection.CacheDirectory)) return Array.Empty<string>();

            var guid = typeof(MethodCacheTestAttribute).FullName.ToGuid();
            return Directory.GetFiles(
                Introspection.CacheDirectory,
                $"*_{guid}.query",
                SearchOption.TopDirectoryOnly);
        }

        private static void DeleteFixtureCacheFiles()
        {
            foreach (var file in GetFixtureCacheFiles()) File.Delete(file);
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
    }
}
