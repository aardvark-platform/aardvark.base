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
using System.Dynamic;

namespace Aardvark.Base;

public static partial class Telemetry
{
    public static class Debug
    {
        private static readonly List<Action<string, IProbe>> s_registrationActions = [];

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

        static Debug() => AddCustomRegistrationAction(
            (name, probe) => Register(name + "/s", probe.RatePerSecond())
            );

        private class DynamicCpuTimeProvider : DynamicObject
        {
            private readonly Dictionary<string, CpuTime> m_probes = [];

            public override IEnumerable<string> GetDynamicMemberNames() => m_probes.Keys;

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (m_probes.TryGetValue(binder.Name, out CpuTime x))
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
            private readonly Dictionary<string, Counter> m_probes = [];

            public override IEnumerable<string> GetDynamicMemberNames() => m_probes.Keys;

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (m_probes.TryGetValue(binder.Name, out Counter x))
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
