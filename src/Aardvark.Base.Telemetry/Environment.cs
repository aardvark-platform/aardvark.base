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
            private readonly Dictionary<string, NamedProbe> m_probes =
                new Dictionary<string, NamedProbe>();

            public void SetMember(string name, IProbe probe)
            {
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
                if (probe == null) throw new ArgumentNullException(nameof(probe));
                m_probes[name] = new NamedProbe(name, probe);
            }

            public IProbe GetMember(string name)
            {
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
                return m_probes.TryGetValue(name, out NamedProbe x) ? x.Probe : null;
            }

            public override IEnumerable<string> GetDynamicMemberNames() => m_probes.Keys;

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = GetMember(binder.Name);
                return result != null;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (binder == null) throw new ArgumentNullException(nameof(binder));

                var probe = value as IProbe;
                if (probe == null) throw new ArgumentException("Value must implement Telemetry.IProbe.", nameof(value));
                SetMember(binder.Name, probe);
                return true;
            }

            public IEnumerator<NamedProbe> GetEnumerator()
                => ((IEnumerable<NamedProbe>)m_probes.Values).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
