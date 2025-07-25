/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base;

public static partial class Telemetry
{
    private static readonly Dictionary<string, IProbe> s_namedProbes = [];
    private static readonly Dictionary<string, Func<IEnumerable<TimingStats>>> s_providersForTimingStats = [];

    public static Tuple<string, IProbe>[] NamedProbes
    {
        get
        {
            lock (s_namedProbes)
            {
                return [.. s_namedProbes.Select(x => Tuple.Create(x.Key, x.Value))];
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
                return [.. s_providersForTimingStats.Select(x => Tuple.Create(x.Key, x.Value))];
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
