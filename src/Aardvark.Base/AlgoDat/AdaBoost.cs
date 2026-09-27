using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// Adaptive boosting creates a strong binary classifier
    /// out of an ensemble of weak classifiers.
    /// </summary>
    public static class AdaBoost
    {
        /// <summary>
        /// Creates a strong binary classifier out of an ensemble of weak classifiers.
        /// Each iteration invokes <paramref name="getNextWeakClassifier"/> at most once, so
        /// <paramref name="iterations"/> is also a strict upper bound on factory invocations.
        /// Training terminates when the next learner is within 0.02 of random error. A learner
        /// that is correct for every training item, or wrong for every item and therefore a
        /// perfect inverse, replaces the current ensemble and terminates training immediately.
        /// </summary>
        /// <typeparam name="T">Type of items to be classified.</typeparam>
        /// <param name="items">Training set.</param>
        /// <param name="groundTruth">Correct classification of training set.</param>
        /// <param name="getNextWeakClassifier">
        /// Returns a new weak classifier for given weighted items.
        /// (double[] weights, T[] items, bool[] groundTruth) => T => bool.
        /// </param>
        /// <param name="iterations">Maximum number of weak-classifier factory invocations.</param>
        /// <param name="onIteration">Optional callback after each ordinary weak classifier is
        /// accepted and its sample weights are updated. If the callback returns true, learning
        /// stops. Degenerate random, perfect, and perfectly inverted learners do not invoke it.</param>
        /// <param name="stopIfWeakClassifierHasLessImportanceThan">Stops before adding an
        /// ordinary learner whose absolute vote weight is below this value. The default is 0.0.</param>
        /// <returns>A strong classifier for <typeparamref name="T"/> based on a weighted majority
        /// vote. Classification takes linear time in the retained learner count and allocates no
        /// managed memory after the returned delegate has been created.</returns>
        public static Func<T, bool> Train<T>(
            T[] items, bool[] groundTruth,
            Func<double[], T[], bool[], Func<T, bool>> getNextWeakClassifier,
            int iterations,
            Func<Func<T, bool>, bool> onIteration = null,
            double stopIfWeakClassifierHasLessImportanceThan = 0.0)
        {
            var count = items.Length;
            var weights = new double[count].Set(1.0 / count);
            var predictions = new bool[count];
            var weakClassifiers = new List<WeakClassifier<T>>();

            while (iterations-- > 0)
            {
                try
                {
                    // Get the next weak classifier based on the current sample weights.
                    var classifier = getNextWeakClassifier(weights, items, groundTruth);

                    // Evaluate once per item and retain predictions for the weight update.
                    double error = 0.0;
                    bool allCorrect = true;
                    bool allWrong = count > 0;
                    for (int i = 0; i < count; i++)
                    {
                        bool prediction = classifier(items[i]);
                        predictions[i] = prediction;
                        if (prediction == groundTruth[i])
                        {
                            allWrong = false;
                        }
                        else
                        {
                            allCorrect = false;
                            error += weights[i];
                        }
                    }

                    if (allCorrect)
                    {
                        weakClassifiers.Clear();
                        weakClassifiers.Add(new WeakClassifier<T>(1.0, classifier));
                        break;
                    }

                    if (allWrong)
                    {
                        // The inverse is perfect; a finite negative vote performs that inversion.
                        weakClassifiers.Clear();
                        weakClassifiers.Add(new WeakClassifier<T>(-1.0, classifier));
                        break;
                    }

                    // A random learner contributes no useful information. Do not refund the
                    // iteration, since doing so can make a finite training budget non-terminating.
                    if (Math.Abs(0.5 - error) < 0.02) break;

                    // Higher error gives lower importance. Degenerate arithmetic terminates
                    // instead of publishing non-finite votes or weights to another iteration.
                    double alpha = 0.5 * Math.Log((1.0 - error) / error);
                    if (double.IsNaN(alpha) || double.IsInfinity(alpha)) break;
                    if (Math.Abs(alpha) < stopIfWeakClassifierHasLessImportanceThan) break;

                    double up = Math.Exp(alpha);
                    double down = Math.Exp(-alpha);
                    double weightSum = 0.0;
                    for (int i = 0; i < count; i++)
                    {
                        weights[i] *= predictions[i] == groundTruth[i] ? down : up;
                        weightSum += weights[i];
                    }

                    if (!(weightSum > 0.0) || double.IsInfinity(weightSum)) break;

                    double normalization = 1.0 / weightSum;
                    if (double.IsInfinity(normalization)) break;
                    for (int i = 0; i < count; i++) weights[i] *= normalization;

                    weakClassifiers.Add(new WeakClassifier<T>(alpha, classifier));

                    if (onIteration != null)
                    {
                        var current = new Classifier<T>(weakClassifiers.ToArray());
                        if (onIteration(current.Classify)) break;
                    }
                }
                catch
                {
                    Report.Warn("AdaBoost.Train");
                }
            }

            return new Classifier<T>(weakClassifiers.ToArray()).Classify;
        }

        private sealed class Classifier<T>
        {
            private readonly WeakClassifier<T>[] m_weakClassifiers;

            public Classifier(WeakClassifier<T>[] weakClassifiers)
            {
                m_weakClassifiers = weakClassifiers;
            }

            public bool Classify(T value)
            {
                double sum = 0.0;
                for (int i = 0; i < m_weakClassifiers.Length; i++)
                {
                    var weak = m_weakClassifiers[i];
                    sum += weak.Alpha * (weak.Classifier(value) ? 1.0 : -1.0);
                }
                return sum > 0.0;
            }
        }

        private readonly struct WeakClassifier<T>
        {
            public readonly double Alpha;
            public readonly Func<T, bool> Classifier;

            public WeakClassifier(double alpha, Func<T, bool> classifier)
            {
                Alpha = alpha;
                Classifier = classifier ?? throw new ArgumentNullException(nameof(classifier));
            }
        }
    }
}
