using System;

namespace Aardvark.Base
{
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
}
