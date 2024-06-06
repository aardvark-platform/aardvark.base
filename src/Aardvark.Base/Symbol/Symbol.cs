using System;
using System.Collections.Generic;
using System.Threading;

namespace Aardvark.Base
{
    public readonly struct Symbol : IEquatable<Symbol>, IComparable<Symbol>, IComparable
    {
        public readonly int Id;

        #region Internal Constructor

        /// <summary>
        /// DO NOT USE THIS CONSTRUCTOR!
        /// Use Create(...) instead.
        /// </summary>
        internal Symbol(int id) => Id = id;

        #endregion

        #region Static Creators

        public static Symbol Create(string str) => SymbolManager.GetSymbol(str);

        public static Symbol CreateNewGuid() => SymbolManager.GetSymbol(Guid.NewGuid());

        public static Symbol Create(Guid guid) => SymbolManager.GetSymbol(guid);

        public static readonly Symbol Empty = default;

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if the Symbol is negative.
        /// For details on negative symbols see the
        /// unary minus operator.
        /// </summary>
        public bool IsNegative => Id < 0;

        /// <summary>
        /// Returns true if the Symbol is not negative.
        /// For details on negative symbols see the
        /// unary minus operator.
        /// </summary>
        public bool IsPositive => Id > 0;

        public bool IsNotEmpty => Id != 0;

        public bool IsEmpty => Id == 0;

        #endregion

        #region Overrides

        public override int GetHashCode() => Id;

        public override bool Equals(object obj) => (obj is Symbol symbol) ? (Id == symbol.Id) : false;

        public override string ToString() => SymbolManager.GetString(Id);

        public Guid ToGuid() => SymbolManager.GetGuid(Id);

        #endregion

        #region IEquatable<Symbol> Members

        public bool Equals(Symbol other) => Id == other.Id;

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Symbol) return Id.CompareTo(((Symbol)obj).Id);
            else throw new NotSupportedException(string.Format("Cannot compare symbol to {0}", obj));
        }

        #endregion

        #region IComparable<Symbol> Members

        public int CompareTo(Symbol other) => Id.CompareTo(other.Id);

        #endregion

        #region Operators

        public static bool operator ==(Symbol a, Symbol b) => a.Id == b.Id;

        public static bool operator !=(Symbol a, Symbol b) => a.Id != b.Id;

        /// <summary>
        /// Creates a negative symbol from an ordinary symbol.
        /// Negative symbols have no string representation, they
        /// are however useful to store a second value in a
        /// dictionary.
        /// </summary>
        public static Symbol operator -(Symbol symbol) => new Symbol(-symbol.Id);

        #endregion

        #region Conversion

        public static implicit operator Symbol(string str) => Create(str);

        #endregion
    }

    public interface ITypedSymbol
    {
        Symbol GetSymbol();
        Type GetSymbolType();
    }

    /// <summary>
    /// A typed symbol is a symbol that is associated with
    /// a type at compile time. This can be used in Dicts
    /// to associate each key with a value type.
    /// </summary>
    public readonly struct TypedSymbol<T> : ITypedSymbol
    {
        public readonly Symbol Symbol;

        #region Constructor

        public TypedSymbol(string str) => Symbol = str;

        public TypedSymbol(Symbol symbol) => Symbol = symbol;

        #endregion

        #region ITypedSymbol Members

        public Symbol GetSymbol() => Symbol;

        public Type GetSymbolType() => typeof(T);

        #endregion

        #region Conversion

        public static implicit operator TypedSymbol<T>(string str) => new TypedSymbol<T>(str);

        #endregion
    }

    public static class SymbolExtensions
    {
        public static Symbol ToSymbol(this string str) => Symbol.Create(str);

        public static TypedSymbol<T> WithType<T>(this Symbol symbol) => new TypedSymbol<T>(symbol);

        /// <summary>
        /// Returns the result of .ToString() of an objects as Symbol.
        /// </summary>
        public static Symbol ToSymbol(this object self) => Symbol.Create(self.ToString());
    }
    
    internal static class SymbolManager
    {
        private static readonly Dict<string, int> s_stringDict = new Dict<string, int>(1024);
        private static readonly Dict<Guid, int> s_guidDict = new Dict<Guid, int>(1024);
        private static readonly List<string> s_allStrings = new List<string>(1024);
        private static readonly List<Guid> s_allGuids = new List<Guid>(1024);
        private static SpinLock s_lock = new SpinLock();
        
        static SymbolManager()
        {
            s_allStrings.Add(string.Empty);
            s_allGuids.Add(Guid.Empty);
        }

        internal static Symbol GetSymbol(Guid guid)
        {
            int id;
            var locked = false;
            try
            {
                s_lock.Enter(ref locked);
                if (!s_guidDict.TryGetValue(guid, out id))
                {
                    id = s_allStrings.Count;
                    var str = guid.ToString();
                    s_guidDict.Add(guid, id);
                    s_stringDict.Add(str, id);
                    s_allStrings.Add(str);
                    s_allGuids.Add(guid);
                }
            }
            finally { if (locked) s_lock.Exit(); }
            return new Symbol(id);
        }

        internal static Symbol GetSymbol(string str)
        {
            if (string.IsNullOrEmpty(str))
                return default;

            int id;
            int hash = str.GetHashCode(); // hashcode computation outside spinlock
            var locked = false;
            try
            {
                s_lock.Enter(ref locked);
                if (!s_stringDict.TryGetValue(str, hash, out id))
                {
                    id = s_allStrings.Count;
                    s_stringDict.Add(str, hash, id);
                    s_allStrings.Add(str);
                    s_allGuids.Add(Guid.Empty);
                }
            }
            finally { if (locked) s_lock.Exit(); }
            return new Symbol(id);
        }

        internal static Guid GetGuid(int id)
        {
            if (id > 0)
            {
                var locked = false;
                try
                {
                    s_lock.Enter(ref locked);
                    return s_allGuids[id];
                }
                finally { if (locked) s_lock.Exit(); }
            }
            return Guid.Empty;
        }

        internal static string GetString(int id)
        {
            var locked = false;
            if (id > 0)
            {
                try
                {
                    s_lock.Enter(ref locked);
                    return s_allStrings[id];
                }
                finally { if (locked) s_lock.Exit(); }
            }
            else if (id < 0)
            {
                string str;
                try
                {
                    s_lock.Enter(ref locked);
                    str = s_allStrings[-id];
                }
                finally { if (locked) s_lock.Exit(); }
                return "-" + str;
            }
            return string.Empty;
        }
    }
}
