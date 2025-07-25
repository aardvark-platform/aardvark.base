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
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Aardvark.Base;

public static partial class Telemetry
{
    public class Env : DynamicObject, IEnumerable<NamedProbe>
    {
        private readonly Dictionary<string, NamedProbe> m_probes = [];

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
            if (value is not IProbe probe) throw new ArgumentException();
            SetMember(binder.Name, probe);
            return true;
        }

        public IEnumerator<NamedProbe> GetEnumerator()
            => ((IEnumerable<NamedProbe>)m_probes.Values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
