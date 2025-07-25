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

namespace Aardvark.Base.Coder;

/// <summary>
/// Mark all classes that should be serializable with this attribute.
/// The class has to inherit from Map or Instance,
/// or implement the IFieldCodeable interface.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct,
                Inherited = false, AllowMultiple = false)]
public sealed class RegisterTypeInfoAttribute : Attribute
{
    private int m_version;

    public RegisterTypeInfoAttribute()
    {
        m_version = 0;
    }

    public int Version
    {
        get { return m_version; }
        set { m_version = value; }
    }
}
