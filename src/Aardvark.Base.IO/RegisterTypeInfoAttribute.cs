using System;

namespace Aardvark.Base.Coder
{
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
}
