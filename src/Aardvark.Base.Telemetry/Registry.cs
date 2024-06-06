using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        private static readonly Dictionary<string, IProbe> s_namedProbes
            = new Dictionary<string, IProbe>();
        private static readonly Dictionary<string, Func<IEnumerable<TimingStats>>> s_providersForTimingStats
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
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (probe is null) throw new ArgumentNullException(nameof(probe));

            lock (s_namedProbes)
                s_namedProbes[name] = probe;

            return Disposable.Create(
                () => { lock (s_namedProbes) s_namedProbes.Remove(name); }
                );
        }

        public static void Register(string name, Func<IEnumerable<TimingStats>> provider)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (provider is null) throw new ArgumentNullException(nameof(provider));

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
