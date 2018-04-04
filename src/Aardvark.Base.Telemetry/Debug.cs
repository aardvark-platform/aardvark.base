using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        public static class Debug
        {
            private static List<Action<string, IProbe>> s_registrationActions = new List<Action<string, IProbe>>();

            /// <summary>
            /// e.g.: using (Telemetry.Debug.CpuTimers.YourTimerNameHere) { /* stuff to time */ }
            /// </summary>
            public static readonly dynamic CpuTimers = new DynamicCpuTimeProvider();

            /// <summary>
            /// e.g.: Telemetry.Debug.Counters.YourCounterNameHere.Increment();
            /// </summary>
            public static readonly dynamic Counters = new DynamicCounterProvider();
            
            /// <summary>
            /// Use this to register a custom action that will be called each time
            /// someone creates a dynamic CpuTimer (with Telemetry.Debug.CpuTimers.YourTimerNameHere).
            /// The parameters are the user-provided name (e.g. "YourTimerNameHere" in the
            /// above example and the created CpuTimer. E.g. the kernel registers an action
            /// that for each timer automatically creates a derived probe showing the values
            /// per cycle.
            /// </summary>
            public static void AddCustomRegistrationAction(Action<string, IProbe> action)
            {
                if (action is null) throw new ArgumentNullException(nameof(action));
                s_registrationActions.Add(action);
            }

            static Debug()
            {
                AddCustomRegistrationAction(
                    (name, probe) => Register(name + "/s", probe.RatePerSecond())
                    );
            }

            private class DynamicCpuTimeProvider : DynamicObject
            {
                private Dictionary<string, CpuTime> m_probes = new Dictionary<string, CpuTime>();

                public override IEnumerable<string> GetDynamicMemberNames()
                {
                    return m_probes.Keys;
                }

                public override bool TryGetMember(GetMemberBinder binder, out object result)
                {
                    CpuTime x;
                    if (m_probes.TryGetValue(binder.Name, out x))
                    {
                        result = x.Timer;
                    }
                    else
                    {
                        x = new CpuTime();
                        m_probes[binder.Name] = x;
                        Register(binder.Name, x);
                        s_registrationActions.ForEach(a => a(binder.Name, x));
                        result = x.Timer;
                    }

                    return true;
                }
            }

            private class DynamicCounterProvider : DynamicObject
            {
                private Dictionary<string, Counter> m_probes = new Dictionary<string, Counter>();

                public override IEnumerable<string> GetDynamicMemberNames()
                {
                    return m_probes.Keys;
                }

                public override bool TryGetMember(GetMemberBinder binder, out object result)
                {
                    Counter x;
                    if (m_probes.TryGetValue(binder.Name, out x))
                    {
                        result = x;
                    }
                    else
                    {
                        x = new Counter();
                        m_probes[binder.Name] = x;
                        Register(binder.Name, x);
                        s_registrationActions.ForEach(a => a(binder.Name, x));
                        result = x;
                    }

                    return true;
                }
            }
        }
    }
}
