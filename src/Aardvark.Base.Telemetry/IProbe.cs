using System;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        public interface IProbe
        {
            double ValueDouble { get; }
        }

        public interface IProbe<T> : IProbe
        {
            T Value { get; }
        }

        public class NamedProbe
        {
            public readonly string Name;
            public readonly Telemetry.IProbe Probe;

            public NamedProbe(string name, Telemetry.IProbe probe)
            {
                if (probe == null) throw new ArgumentNullException();
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException();

                Name = name;
                Probe = probe;
            }
        }

        /// <summary>
        /// Global reset of all telemetry probes to zero (or the equivalent).
        /// This is mainly used for benchmarking, e.g. to cleanly restart telemetry
        /// after a warm up phase or before a new benchmark iteration.
        /// Custom IProbe implementations must handle the Telemetry.OnReset event.
        /// </summary>
        public static void ResetTelemetrySystem()
        {
            var eh = OnReset;
            if (eh != null) eh(null, EventArgs.Empty);
        }

        /// <summary>
        /// Signals that ResetTelemetrySystem has been called.
        /// Custom IProbe implementations must handle the Telemetry.OnReset event.
        /// </summary>
        public static event EventHandler OnReset;
    }
}
