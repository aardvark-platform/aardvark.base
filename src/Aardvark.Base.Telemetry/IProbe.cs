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

namespace Aardvark.Base;

public static partial class Telemetry
{
    public interface IProbe
    {
        /// <summary>
        /// Current value of probe.
        /// </summary>
        double ValueDouble { get; }
    }

    public interface IProbe<T> : IProbe
    {
        /// <summary>
        /// Current typed value of probe.
        /// </summary>
        T Value { get; }
    }

    public class NamedProbe
    {
        public readonly string Name;
        public readonly IProbe Probe;

        public NamedProbe(string name, IProbe probe)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException();

            Name = name;
            Probe = probe ?? throw new ArgumentNullException();
        }
    }

    /// <summary>
    /// Global reset of all telemetry probes to zero (or the equivalent).
    /// This is mainly used for benchmarking, e.g. to cleanly restart telemetry
    /// after a warm up phase or before a new benchmark iteration.
    /// Custom IProbe implementations must handle the Telemetry.OnReset event.
    /// </summary>
    public static void ResetTelemetrySystem() => OnReset?.Invoke(null, EventArgs.Empty);

    /// <summary>
    /// Signals that ResetTelemetrySystem has been called.
    /// Custom IProbe implementations must handle the Telemetry.OnReset event.
    /// </summary>
    public static event EventHandler OnReset;
}
