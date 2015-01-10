using System;
using System.Collections.Generic;
using System.Linq;

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
        /// </summary>
        /// <typeparam name="T">Type of items to be classified.</typeparam>
        /// <param name="items">Training set.</param>
        /// <param name="groundTruth">Correct classification of training set.</param>
        /// <param name="getNextWeakClassifier">
        /// Returns a new weak classifier for given weighted items.
        /// (double[] weights, T[] items, bool[] groundTruth) => T => bool.
        /// </param>
        /// <param name="iterations">Maximium number of weak classifiers to combine.</param>
        /// <param name="onIteration">Optional callback for each learning iteration.
        /// If onIteration returns true, then learning is stopped.</param>
        /// <param name="stopIfWeakClassifierHasLessImportanceThan">Default is 0.0, which means that the
        /// maximum number of iterations will be performed.</param>
        /// <returns>A strong classifier for Ts based on a weighted majority vote of
        /// weak classifiers.</returns>
        public static Func<T, bool> Train<T>(
            T[] items, bool[] groundTruth,
            Func<double[], T[], bool[], Func<T, bool>> getNextWeakClassifier,
            int iterations,
            Func<Func<T, bool>, bool> onIteration = null,
            double stopIfWeakClassifierHasLessImportanceThan = 0.0
            )
        {
            var count = items.Length;
            var ws = new double[count].Set(1.0 / count);
            var classifiers = new List<Func<T, bool>>();
            var alphas = new List<double>();

            while (iterations-- > 0)
            {
                try
                {
                    // get next weak classifier (based on examples and weights)
                    var classifier = getNextWeakClassifier(ws, items, groundTruth);

                    // predict values with new classifier
                    var predictions = items.Select(x => classifier(x)).ToArray();

                    // compare predictions to reality (and compute error rate e)
                    double e = 0.0;
                    for (var j = 0; j < count; j++)
                    {
                        e += ws[j] * (predictions[j] == groundTruth[j] ? 0 : 1);
                    }

                    if (Math.Abs(0.5 - e) < 0.02) { iterations++; continue; }

                    if (e == 0.0) // we found a perfect classifier
                    {
                        classifiers.Clear(); classifiers.Add(classifier);
                        alphas.Clear(); alphas.Add(1.0);
                        break;
                    }

                    // compute importance for this classifier
                    // (higher error rate gives less importance)
                    var alpha = 0.5 * Math.Log((1 - e) / e);
                    if (Math.Abs(alpha) < stopIfWeakClassifierHasLessImportanceThan) break;

                    // increase weights of incorrectly classified examples, and
                    // decrease weights of correctly classified examples
                    var up = Math.Exp(alpha);
                    var down = Math.Exp(-alpha);
                    for (var j = 0; j < count; j++)
                    {
                        ws[j] *= (predictions[j] == groundTruth[j]) ? down : up;
                    }
                    var wnormf = 1.0 / ws.Sum(); // normalization factor for weights
                    for (var j = 0; j < count; j++) ws[j] *= wnormf;

                    // add classifier and its importance to list
                    classifiers.Add(classifier);
                    alphas.Add(alpha);

                    // optional callback
                    if (onIteration != null)
                    {
                        var c = new Classifier<T>(
                            Enumerable.Range(0, ws.Length).Select(i => new WeightedExample<T>(ws[i], items[i])),
                            Enumerable.Range(0, alphas.Count).Select(i => new WeakClassifier<T>(alphas[i], classifiers[i]))
                            );
                        if (onIteration(c.Classify))
                        {
                            break;
                        }
                    }
                }
                catch /*(Exception e)*/
                {
                    Report.Warn("AdaBoost.Train");
                }
            }

            // create strong classifier from weak classifiers
            return new Classifier<T>(
                Enumerable.Range(0, ws.Length).Select(i => new WeightedExample<T>(ws[i], items[i])),
                Enumerable.Range(0, alphas.Count).Select(i => new WeakClassifier<T>(alphas[i], classifiers[i]))
                ).Classify;
        }

        private struct Classifier<T>
        {
			//private readonly WeightedExample<T>[] m_examples;  // hs: assigned but not used warning in mono, with warning as error this is a build error :(
            private readonly WeakClassifier<T>[] m_weakClassifiers;

            public Classifier(
                IEnumerable<WeightedExample<T>> examples,
                IEnumerable<WeakClassifier<T>> weakClassifiers
                )
            {
				//m_examples = examples.ToArray();
                m_weakClassifiers = weakClassifiers.ToArray();
            }

            public bool Classify(T x)
            {
                return SumAlphaWeightedWeakClassifiers(x) > 0.0;
            }

            /// <summary>
            /// Computes propability of positive classification given x.
            /// </summary>
            public double P(T x)
            {
                var f = SumAlphaWeightedWeakClassifiers(x);
                return 1.0 / (1.0 + Math.Exp(-2.0 * f));
            }

            private double SumAlphaWeightedWeakClassifiers(T x)
            {
                return m_weakClassifiers.Sum(c => c.Alpha * (c.Classifier(x) ? +1 : -1));
            }
        }

        private struct WeightedExample<T>
        {
            public readonly double Weight;
            public readonly T Example;

            public WeightedExample(double weight, T example)
            {
                Weight = weight;
                Example = example;
            }
        }

        private struct WeakClassifier<T>
        {
            public readonly double Alpha;
            public readonly Func<T, bool> Classifier;

            public WeakClassifier(double alpha, Func<T, bool> classifier)
            {
                if (classifier == null) throw new ArgumentNullException("classifier");
                Alpha = alpha;
                Classifier = classifier;
            }
        }
    }
}
