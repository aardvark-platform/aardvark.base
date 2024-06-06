using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base.Coder
{
    internal static class AnnotatedExtensions
    {
        public static Annotated<T> Annotate<T>(this T self)
        {
            return new Annotated<T>(self);
        }
        public static Annotated<T> Annotate<T>(this T self, string key, object value)
        {
            return new Annotated<T>(self, key, value);
        }
        public static Annotated<T> Annotate<T>(this T self, IEnumerable<Annotation> tags)
        {
            return new Annotated<T>(self, tags);
        }
        public static Annotated<T> Annotate<T>(this Annotated<T> self, string key, object value)
        {
            return new Annotated<T>(self, key, value);
        }
        public static Annotated<T> Annotate<T>(this Annotated<T> self, IEnumerable<Annotation> tags)
        {
            return new Annotated<T>(self, tags);
        }
    }

    internal struct Annotation
    {
        public string Key { get; private set; }
        public object Value { get; private set; }
        public override readonly string ToString() { return string.Format("{0} -> {1}", Key, Value); }

        public Annotation(string key, object value)
            : this()
        {
            Key = key;
            Value = value;
        }
    }

    internal readonly struct Annotated<T>
    {
        private readonly T m_value;
        private readonly IEnumerable<Annotation> m_tags;
        private static readonly IEnumerable<Annotation> s_empty = System.Array.Empty<Annotation>();

        public Annotated(T value)
        {
            m_value = value;
            m_tags = s_empty;
        }
        public Annotated(T value, string tagKey, object tagValue)
        {
            m_value = value;
            m_tags = new[] { new Annotation(tagKey, tagValue) };
        }
        public Annotated(T value, IEnumerable<Annotation> tags)
        {
            m_value = value;
            m_tags = tags.ToArray();
        }
        public Annotated(Annotated<T> a)
        {
            m_value = a.Value;
            m_tags = a.Tags.ToArray();
        }
        public Annotated(Annotated<T> a, string tagKey, object tagValue)
        {
            m_value = a.Value;
            m_tags = a.Tags.Concat(new[] { new Annotation(tagKey, tagValue) });
        }
        public Annotated(Annotated<T> a, IEnumerable<Annotation> tags)
        {
            m_value = a.Value;
            m_tags = a.Tags.Concat(tags.ToArray());
        }

        public T Value { get { return m_value; } }
        public IEnumerable<Annotation> Tags { get { return m_tags ?? s_empty; } }

        private string _debugView { get { return m_value.ToString(); } }
        private Annotation[] _tagsDebugView { get { return Tags.ToArray(); } }
    }
}
