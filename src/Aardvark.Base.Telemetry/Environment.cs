using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        public class Env : DynamicObject, IEnumerable<Telemetry.NamedProbe>
        {
            private Dictionary<string, Telemetry.NamedProbe> m_probes =
                new Dictionary<string, Telemetry.NamedProbe>();

            public void SetMember(string name, Telemetry.IProbe probe)
            {
                if (probe == null) throw new ArgumentNullException();
                m_probes[name] = new Telemetry.NamedProbe(name, probe);
            }
            public Telemetry.IProbe GetMember(string name)
            {
                Telemetry.NamedProbe x;
                if (m_probes.TryGetValue(name, out x))
                {
                    return x.Probe;
                }
                else
                {
                    return null;
                }
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return m_probes.Keys;
            }
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = GetMember(binder.Name);
                return result != null;
            }
            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                var probe = value as Telemetry.IProbe;
                if (probe == null) throw new ArgumentException();
                SetMember(binder.Name, probe);
                return true;
            }

            public IEnumerator<Telemetry.NamedProbe> GetEnumerator()
            {
                var result = (IEnumerable<Telemetry.NamedProbe>)m_probes.Values;
                return result.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
