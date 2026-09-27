using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Tests
{
    [TestFixture]
    public class AdaBoostTests
    {
        private static readonly int[] s_items = { 0, 1, 2, 3 };
        private static readonly bool[] s_groundTruth = { false, false, true, true };

        [Test]
        public void RandomLearnerTerminatesWithoutRefundingIterationBudget()
        {
            int factoryCalls = 0;
            int callbackCalls = 0;

            var classifier = AdaBoost.Train(
                s_items,
                s_groundTruth,
                (_, _, _) =>
                {
                    factoryCalls++;
                    return factoryCalls < 5 ? _ => false : GroundTruth;
                },
                iterations: 2,
                onIteration: _ =>
                {
                    callbackCalls++;
                    return false;
                });

            Assert.That(factoryCalls, Is.EqualTo(1));
            Assert.That(callbackCalls, Is.Zero);
            Assert.That(classifier(0), Is.False);
            Assert.That(classifier(3), Is.False);
        }

        [Test]
        public void AntiPerfectLearnerReplacesEnsembleAndStopsWithFiniteState()
        {
            int factoryCalls = 0;
            int callbackCalls = 0;
            var observedWeights = new List<double[]>();

            var classifier = AdaBoost.Train(
                s_items,
                s_groundTruth,
                (weights, _, _) =>
                {
                    observedWeights.Add((double[])weights.Clone());
                    factoryCalls++;
                    if (factoryCalls == 1) return value => value >= 1;
                    if (factoryCalls == 2) return value => !GroundTruth(value);
                    return GroundTruth;
                },
                iterations: 8,
                onIteration: _ =>
                {
                    callbackCalls++;
                    return false;
                });

            Assert.That(factoryCalls, Is.EqualTo(2));
            Assert.That(callbackCalls, Is.EqualTo(1));
            Assert.That(observedWeights.Count, Is.EqualTo(2));
            foreach (var weights in observedWeights)
                foreach (double weight in weights)
                    Assert.That(double.IsFinite(weight), Is.True);

            AssertMatchesGroundTruth(classifier);
        }

        [Test]
        public void OrdinaryLearnersRetainWeightedVotingAndCallbackSnapshots()
        {
            Func<int, bool>[] learners =
            {
                value => value >= 1,
                value => value == 2,
                value => value == 3,
            };
            var observedWeights = new List<double[]>();
            var callbackClassifiers = new List<Func<int, bool>>();
            int factoryCalls = 0;

            var classifier = AdaBoost.Train(
                s_items,
                s_groundTruth,
                (weights, _, _) =>
                {
                    observedWeights.Add((double[])weights.Clone());
                    return learners[factoryCalls++];
                },
                iterations: learners.Length,
                onIteration: current =>
                {
                    callbackClassifiers.Add(current);
                    return false;
                });

            Assert.That(factoryCalls, Is.EqualTo(3));
            Assert.That(callbackClassifiers.Count, Is.EqualTo(3));
            AssertWeights(observedWeights[0], 0.25, 0.25, 0.25, 0.25);
            AssertWeights(observedWeights[1], 1.0 / 6.0, 0.5, 1.0 / 6.0, 1.0 / 6.0);
            AssertWeights(observedWeights[2], 0.1, 0.3, 0.1, 0.5);

            AssertClassifications(callbackClassifiers[0], false, true, true, true);
            AssertClassifications(callbackClassifiers[1], false, false, true, false);
            AssertMatchesGroundTruth(callbackClassifiers[2]);
            AssertMatchesGroundTruth(classifier);
        }

        [Test]
        public void CallbackStopsAfterAcceptedOrdinaryLearner()
        {
            int factoryCalls = 0;
            int callbackCalls = 0;
            Func<int, bool> weak = value => value >= 1;

            var classifier = AdaBoost.Train(
                s_items,
                s_groundTruth,
                (_, _, _) =>
                {
                    factoryCalls++;
                    return weak;
                },
                iterations: 10,
                onIteration: current =>
                {
                    callbackCalls++;
                    AssertClassifications(current, false, true, true, true);
                    return true;
                });

            Assert.That(factoryCalls, Is.EqualTo(1));
            Assert.That(callbackCalls, Is.EqualTo(1));
            AssertClassifications(classifier, false, true, true, true);
        }

        [Test]
        public void WarmedInferenceAllocatesNoManagedMemory()
        {
            var classifier = AdaBoost.Train(
                s_items,
                s_groundTruth,
                (_, _, _) => GroundTruth,
                iterations: 4);

            int warmup = ClassifyMany(classifier, 20_000);
            long before = GC.GetAllocatedBytesForCurrentThread();
            int checksum = ClassifyMany(classifier, 100_000);
            long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            Assert.That(warmup, Is.EqualTo(10_000));
            Assert.That(checksum, Is.EqualTo(50_000));
            Assert.That(allocated, Is.Zero);
        }

        private static bool GroundTruth(int value) => value >= 2;

        private static int ClassifyMany(Func<int, bool> classifier, int count)
        {
            int result = 0;
            for (int i = 0; i < count; i++)
                if (classifier(i & 3)) result++;
            return result;
        }

        private static void AssertMatchesGroundTruth(Func<int, bool> classifier)
            => AssertClassifications(classifier, s_groundTruth);

        private static void AssertClassifications(Func<int, bool> classifier, params bool[] expected)
        {
            for (int i = 0; i < expected.Length; i++)
                Assert.That(classifier(i), Is.EqualTo(expected[i]), $"item {i}");
        }

        private static void AssertWeights(double[] actual, params double[] expected)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length));
            for (int i = 0; i < expected.Length; i++)
                Assert.That(actual[i], Is.EqualTo(expected[i]).Within(1e-12), $"weight {i}");
        }
    }
}
