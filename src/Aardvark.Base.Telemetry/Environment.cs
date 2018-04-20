using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        public class Env : DynamicObject, IEnumerable<NamedProbe>
        {
            private Dictionary<string, NamedProbe> m_probes =
                new Dictionary<string, NamedProbe>();

            public void SetMember(string name, IProbe probe)
            {
                if (probe == null) throw new ArgumentNullException();
                m_probes[name] = new NamedProbe(name, probe);
            }

            public IProbe GetMember(string name)
                => m_probes.TryGetValue(name, out NamedProbe x) ? x.Probe : null;

            public override IEnumerable<string> GetDynamicMemberNames() => m_probes.Keys;

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = GetMember(binder.Name);
                return result != null;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                var probe = value as IProbe;
                if (probe == null) throw new ArgumentException();
                SetMember(binder.Name, probe);
                return true;
            }

            public IEnumerator<NamedProbe> GetEnumerator()
                => ((IEnumerable<NamedProbe>)m_probes.Values).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
