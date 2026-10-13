using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RandomIEnumerableExtensionsTests
    {
        private sealed class ScriptedRandom : Random
        {
            private readonly double[] m_values;
            public int DrawCount { get; private set; }

            public ScriptedRandom(double[] values) => m_values = values;

            public override double NextDouble()
            {
                int index = DrawCount++;
                if (index >= m_values.Length) throw new InvalidOperationException("Unexpected uniform draw.");
                return m_values[index];
            }
        }

        private static RandomSystem Uniform(ScriptedRandom random)
            => new RandomSystem(0) { Generator = random };

        private static readonly int[] s_values = { 1, 2, 3 };

        [Test]
        public void TakeRandomlyRejectsNaNProbabilityWhenEnumerated()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(double.NaN).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        [Test]
        public void TakeRandomlyWithSelectorRejectsNaNProbabilityWhenEnumerated()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(v => v.ToString(), double.NaN).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        [Test]
        public void TakeRandomlyWithSelectorRejectsNullSelectorWhenEnumerated()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => s_values.TakeRandomly<int, string>(null, 1.0).ToArray());

            Assert.AreEqual("selector", exception.ParamName);
        }

        [TestCase(-0.01)]
        [TestCase(1.01)]
        public void TakeRandomlyRejectsOutOfRangeProbabilities(double p)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(p).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        [TestCase(-0.01)]
        [TestCase(1.01)]
        public void TakeRandomlyWithSelectorRejectsOutOfRangeProbabilities(double p)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(v => v.ToString(), p).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        private static IEnumerable<(double Probability, double[] Draws, int[] Retained)> ThresholdCases()
        {
            const double unit53 = 1.0 / 9007199254740992.0;
            yield return (0.0, new[] { 0.0, 0.0, 0.0 }, Array.Empty<int>());
            yield return (1.0, new[] { 0.0, unit53, 0.5, 1.0 - unit53 }, new[] { 0, 1, 2, 3 });
            foreach (double p in new[] { 0.25, 0.5, 0.75 })
                yield return (p, new[] { p / 2.0, p, (p + 1.0) / 2.0 }, new[] { 0 });
            yield return (0.5, new[] { 0.49999999999999994, 0.5, 0.5000000000000001 }, new[] { 0 });
            yield return (0.5, new[] { 0.5, 0.5 }, Array.Empty<int>());
            yield return (0.5, Enumerable.Range(0, 16).Select(i => i / 16.0).ToArray(), Enumerable.Range(0, 8).ToArray());
            foreach (double p in new[] { 0.0, 0.5, 1.0 })
                yield return (p, Array.Empty<double>(), Array.Empty<int>());
        }

        [Test]
        public void TakeRandomlyUsesHalfOpenSelectionThresholds()
        {
            Assert.Multiple(() =>
            {
                foreach (var c in ThresholdCases())
                {
                    string context = $"p={c.Probability}, draws=[{string.Join(",", c.Draws)}]";
                    var values = Enumerable.Range(0, c.Draws.Length).ToArray();
                    var random = new ScriptedRandom(c.Draws);
                    var query = values.TakeRandomly(c.Probability, Uniform(random));
                    Assert.AreEqual(0, random.DrawCount, context + ", deferred draws");
                    CollectionAssert.AreEqual(c.Retained, query.ToArray(), context);
                    Assert.AreEqual(values.Length, random.DrawCount, context + ", one draw per visited element");
                }
            });
        }

        [Test]
        public void TakeRandomlyProjectsOnlyStrictlyRetainedElements()
        {
            Assert.Multiple(() =>
            {
                foreach (var c in ThresholdCases())
                {
                    string context = $"p={c.Probability}, draws=[{string.Join(",", c.Draws)}]";
                    var values = Enumerable.Range(0, c.Draws.Length).ToArray();
                    var random = new ScriptedRandom(c.Draws);
                    var selected = new List<int>();
                    var query = values.TakeRandomly(v => { selected.Add(v); return "value:" + v; }, c.Probability, Uniform(random));
                    Assert.AreEqual(0, random.DrawCount, context + ", deferred draws");
                    Assert.IsEmpty(selected, context + ", deferred selector");
                    CollectionAssert.AreEqual(c.Retained.Select(v => "value:" + v), query.ToArray(), context);
                    Assert.AreEqual(values.Length, random.DrawCount, context + ", one draw per visited element");
                    Assert.AreEqual(c.Retained.Length, selected.Count, context + ", selector count");
                    CollectionAssert.AreEqual(c.Retained, selected, context + ", selector inputs and order");
                }
            });
        }

        [Test]
        public void TakeRandomlyDefersTraversalAndStopsOnEarlyDisposal()
        {
            foreach (bool project in new[] { false, true })
            {
                string context = $"selector={project}";
                int acquired = 0, visited = 0, disposed = 0, selected = 0;
                IEnumerable<int> Values()
                {
                    acquired++;
                    try { foreach (int value in s_values) { visited++; yield return value; } }
                    finally { disposed++; }
                }
                var random = new ScriptedRandom(new[] { 0.5, 0.25, 0.75 });
                var uniform = Uniform(random);
                var query = project
                    ? Values().TakeRandomly(v => { selected++; return v + 100; }, 0.5, uniform)
                    : Values().TakeRandomly(0.5, uniform);
                using (var iterator = query.GetEnumerator())
                {
                    Assert.AreEqual(0, acquired, context);
                    Assert.AreEqual(0, random.DrawCount, context);
                    Assert.AreEqual(0, selected, context);
                    Assert.IsTrue(iterator.MoveNext(), context);
                    Assert.AreEqual(project ? 102 : 2, iterator.Current, context);
                    Assert.AreEqual(1, acquired, context);
                    Assert.AreEqual(2, visited, context);
                    Assert.AreEqual(2, random.DrawCount, context);
                    Assert.AreEqual(project ? 1 : 0, selected, context);
                }
                Assert.AreEqual(1, disposed, context);
                Assert.AreEqual(2, visited, context + ", no further traversal");
                Assert.AreEqual(2, random.DrawCount, context + ", no further draws");
            }

            foreach (double p in new[] { double.NaN, -0.01, 1.01 })
            foreach (bool project in new[] { false, true })
            {
                var random = new ScriptedRandom(Array.Empty<double>());
                var query = project ? s_values.TakeRandomly(v => v, p, Uniform(random)) : s_values.TakeRandomly(p, Uniform(random));
                using var iterator = query.GetEnumerator();
                string context = $"selector={project}, p={p}";
                Assert.AreEqual(0, random.DrawCount, context);
                Assert.AreEqual("p", Assert.Throws<ArgumentOutOfRangeException>(() => iterator.MoveNext(), context).ParamName, context);
                Assert.AreEqual(0, random.DrawCount, context);
            }

            foreach (double p in new[] { 0.0, 1.0 })
            {
                CollectionAssert.AreEqual(p == 0.0 ? Array.Empty<int>() : s_values, s_values.TakeRandomly(p).ToArray(), $"default generator, p={p}");
                int selected = 0;
                var result = s_values.TakeRandomly(v => { selected++; return v.ToString(); }, p).ToArray();
                CollectionAssert.AreEqual(p == 0.0 ? Array.Empty<string>() : new[] { "1", "2", "3" }, result, $"default generator selector, p={p}");
                Assert.AreEqual(result.Length, selected, $"default generator selector count, p={p}");
            }
        }
    }
}
