using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        private static Dictionary<string, IProbe> s_namedProbes
            = new Dictionary<string, IProbe>();
        private static Dictionary<string, Func<IEnumerable<TimingStats>>> s_providersForTimingStats
            = new Dictionary<string, Func<IEnumerable<TimingStats>>>();

        public static Tuple<string, IProbe>[] NamedProbes
        {
            get
            {
                lock (s_namedProbes)
                {
                    return s_namedProbes
                        .Select(x => Tuple.Create(x.Key, x.Value))
                        .ToArray()
                        ;
                }
            }
        }
        public static IProbe GetNamedProbe(string name)
        {
            lock (s_namedProbes)
            {
                return s_namedProbes.ContainsKey(name) ? s_namedProbes[name] : null;
            }
        }
        private class Disposable : IDisposable
        {
            private Action m_disposeAction;
            public static IDisposable Create(Action disposeAction) { return new Disposable() { m_disposeAction = disposeAction }; }
            public void Dispose() { m_disposeAction(); }
        }
        public static IDisposable Register(string name, IProbe probe)
        {
            Requires.NotEmpty(name);
            Requires.NotNull(probe);

            lock (s_namedProbes)
                s_namedProbes[name] = probe;

            return Disposable.Create(
                () => { lock (s_namedProbes) s_namedProbes.Remove(name); }
                );
        }
        //public static IDisposable RegisterObservable(string name, IObservable<long> probe)
        //{
        //    Requires.NotEmpty(name);
        //    Requires.NotNull(probe);
        //    return Register(name, new Telemetry.ObservableCustomProbeLong(probe));
        //}
        //public static IDisposable RegisterObservable(string name, IObservable<double> probe)
        //{
        //    Requires.NotEmpty(name);
        //    Requires.NotNull(probe);
        //    return Register(name, new Telemetry.ObservableCustomProbeDouble(probe));
        //}
        //public static IDisposable RegisterObservable(string name, IObservable<TimeSpan> probe)
        //{
        //    Requires.NotEmpty(name);
        //    Requires.NotNull(probe);
        //    return Register(name, new Telemetry.ObservableCustomProbeTimeSpan(probe));
        //}

        public static void Register(string name, Func<IEnumerable<TimingStats>> provider)
        {
            Requires.NotEmpty(name);
            Requires.NotNull(provider);

            lock (s_providersForTimingStats)
                s_providersForTimingStats[name] = provider;
        }
        public static Tuple<string, Func<IEnumerable<TimingStats>>>[] ProvidersForTimingStats
        {
            get
            {
                lock (s_providersForTimingStats)
                {
                    return s_providersForTimingStats
                        .Select(x => Tuple.Create(x.Key, x.Value))
                        .ToArray()
                        ;
                }
            }
        }
        public static Func<IEnumerable<TimingStats>> GetProviderForTimingStats(string name)
        {
            lock (s_providersForTimingStats)
            {
                return s_providersForTimingStats.ContainsKey(name)
                    ? s_providersForTimingStats[name] : null;
            }
        }
    }
}
