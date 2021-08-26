using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# var typea   = new[] { "Dict",   "DictSet", "IntDict", "IntSet", "SymbolDict", "SymbolSet",
    //#                       "BigDict", "BigDictSet", "LongDict", "LongSet" };
    //# var tka     = new[] { "TKey",   "TKey",    "int",     "int",    "Symbol",     "Symbol",
    //#                       "TKey",   "TKey",    "long",    "long" };
    //# var tva     = new[] { "TValue", "",        "TValue",  "",       "TValue",     "",
    //#                       "TValue", "",        "TValue",  "" };
    //# var kia     = new[] { 0, 0, 1, 1, 2, 2, 3, 3, 1, 1 };
    //# var hfa     = new[] { "", "", "", "m_hfun(" };
    //# var gha     = new[] { ".GetHashCode()", "", ".Id", ")" };
    //# var hia     = new[] { "HashKeyValue", "HashKey", "IntValue", "int", "SymbolValue", "Symbol",
    //#                       "HashKeyValue", "HashKey", "LongValue", "long" };
    //# var ha      = new[] { ".Hash", ".Hash", ".Key", "", ".Key.Id", ".Id",
    //#                       ".Hash", ".Hash", ".Key", "" };
    //# var ka      = new[] { ".Key", ".Key", ".Key", "", ".Key", "", ".Key", ".Key", ".Key", "" };
    //# foreach (var equatable in new bool[] { false, true }) { var eqString = (equatable ? "IEq" : "");
    //# foreach (var fast in new bool[] { false, true }) {
    //# foreach (var concurrent in new bool[] { false, true }) { if (fast && !concurrent) continue;
    //# for (int ti = 0; ti < typea.Length; ti++) {
    //#     var big = ti >= 6;
    //#     var izero = big ? "0L" : "0";
    //#     var wrapped = concurrent && !fast;
    //#     var concurrentString = (fast ? "Fast" : "") + (concurrent ? "Concurrent" : "");
    //#     var type = concurrentString + typea[ti] + eqString; var ki = kia[ti];
    //#     var itype = big ? "long" : "int";
    //#     var icast = big ? "(long)" : "";
    //#     var uitype = big ? "ulong" : "uint";
    //#     var nctype = typea[ti] + eqString;
    //#     var ctype = "Concurrent" + nctype;
    //#     var hasKey = tka[ti] == "TKey";
    //#     if (equatable && !hasKey) continue;
    //#     var hasValue = tva[ti] == "TValue";
    //#     var isGeneric = hasKey || hasValue;
    //#     var tpar = (isGeneric ? "<" : "")
    //#                + (hasKey ? tka[ti] : "")
    //#                + (hasKey && hasValue ? ", " : "")
    //#                + (hasValue ? tva[ti] : "")
    //#                + (isGeneric ? ">" : "");
    //#     var isSym = tka[ti] == "Symbol";
    //#     var hitem = hia[ti];
    //#     var fun = big && hasKey;
    //#     var ext = fun ? "Long" : "";
    //#     var HashItem = hitem + ext + tpar;
    //#     var NextHashItem = "Next" + hitem.Capitalized() + ext + tpar;
    //#     var HashItemNext = hitem.Capitalized() + "Next" + ext + tpar;
    //#     var HashFun = hfa[ki];
    //#     var GetHash = gha[ki];
    //#     var Hash = ha[ti];
    //#     var Key = ka[ti];
    //#     var tkey = tka[ti];
    //#     var tvalue = tva[ti];
    //#     var uintCast = hasKey ? "" : "(uint)";
    //#     var ValuePair = hasValue ? "ValuePair" : "";
    //#     var idict = "ICountableDict" + (hasValue ? "" : "Set");
    #region __type____tpar__

    /// <summary>
    //# if (hasValue) {
    /// A __type__ is an alternate implementation of a Dictionary that can
    /// optionally act as a stack for all items with the same key.
    //# } else {
    /// A __type__ is an alternate implementation of a HashSet.
    //# }
    /// It is implemented as a hashtable with external linking, that uses
    /// primes as the table size in order to reduce the danger of funneling.
    /// </summary>
    public class __type__/*# if (isGeneric) { */</*# if (hasKey) { */TKey/*# } if (hasValue) { if (hasKey) { */, /*# } */TValue/*# } */>/*# } */
            //# if (!wrapped) {
            : /*# if (!big) { */IIntCountable, /*# } */__idict__,/*# if (!hasValue) { */ IDictSet<__tkey__>,/*# } else { */ IDict<__tkey__, TValue>,/*# } */
              IEnumerable, IEnumerable</*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */>/*# if (!big && !concurrent) { */,
              ICollection, ICollection</*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */>/*# } */
        //# }
        //# if (equatable) {
        where TKey : IEquatable<TKey>
        //# }
    {
        //# if (wrapped) {
        private __nctype____tpar__ m_dict;
        //# } else { // !wrapped
        //# if (concurrent) { 
        private long m_version;
        //# }
        //# if (fun) {
        private Func<__tkey__, __itype__> m_hfun;
        //# }
        private __uitype__ m_capacity;
        private __NextHashItem__[] m_firstArray;
        private __HashItemNext__[] m_extraArray;
        private __uitype__ m_count;
        private __uitype__ m_increaseThreshold;
        private __uitype__ m_decreaseThreshold;
        //# if (hasValue) {
        private bool m_doNotStackDuplicateKeys;
        //# }
        private __itype__ m_freeIndex;
        private int m_capacityIndex;
        private __uitype__ m_extraCount;
        private float m_maxFillFactor;
        private float m_minFillFactor;
        public DictReport Report;
        //# } // !wrapped

        #region Constructors

        //# if (wrapped) {
        /// <summary>
        /// A __ctype__ can only be concstructed by wrapping its non-concurrent
        /// counterpart.
        /// </summary>
        public __type__(__nctype____tpar__ dict)
        {
            m_dict = dict;
        }

        //# } else { // !wrapped
        /// <summary>
        /// Create a __type__ that autmatically grows and shrinks as necessary.
        //# if (hasValue) {
        /// If the optional parameter stackDuplicateKeys is set to true, the
        /// __type__ acts as a stack for all items with the same key.
        //# }
        /// </summary>
        public __type__(/*# if (fun) { */Func<__tkey__, __itype__> hfun/*# } if (fun && hasValue) { */, /*# } if (hasValue) { */bool stackDuplicateKeys = false/*# } */)
            : this(/*# if (fun) { */hfun, /*# } */DictConstant.PrimeSizes__ext__[0], (__uitype__)DictConstant.MinExtraCapacity/*# if (hasValue) { */,
                   stackDuplicateKeys/*# } */)
        { }

        /// <summary>
        /// Create a __type__ that autmatically grows and shrinks as necessary,
        /// but also specify an initial capacity.
        //# if (hasValue) {
        /// If the optional parameter stackDuplicateKeys is set to true, the
        /// __type__ acts as a stack for all items with the same key.
        //# }
        /// </summary>
        public __type__(/*# if (fun) { */Func<__tkey__, __itype__> hfun, /*# } */__itype__ initialCapacity)
            : this(/*# if (fun) { */hfun, /*# } */System.Math.Max((__uitype__)initialCapacity,
                                         DictConstant.PrimeSizes__ext__[0]),
                   (__uitype__)DictConstant.MinExtraCapacity/*# if (hasValue) { */, false/*# } */)
        { }

        //# if (hasValue) {
        /// <summary>
        /// Create a __type__ that autmatically grows and shrinks as necessary,
        /// but also specify an initial capacity.
        //# if (hasValue) {
        /// If the optional parameter stackDuplicateKeys is set to true, the
        /// __type__ acts as a stack for all items with the same key.
        //# }
        /// </summary>
        public __type__(/*# if (fun) { */Func<__tkey__, __itype__> hfun, /*# } */__itype__ initialCapacity, bool stackDuplicateKeys)
            : this(/*# if (fun) { */hfun, /*# } */System.Math.Max((__uitype__)initialCapacity,
                                         DictConstant.PrimeSizes__ext__[0]),
                   (__uitype__)DictConstant.MinExtraCapacity, stackDuplicateKeys)
        { }

        //# }
        /// <summary>
        /// Create a __type__ and initialize it to contain the supplied
        /// items.
        //# if (hasValue) {
        /// If the optional parameter stackDuplicateKeys is set to true, the
        /// __type__ acts as a stack for all items with the same key.
        //# }
        /// </summary>
        public __type__(/*# if (fun) { */Func<__tkey__, __itype__> hfun, /*# } */IEnumerable</*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */> items/*# if (hasValue) { */,
                bool stackDuplicateKeys = false/*# } */)
            : this(/*# if (fun) { */hfun, /*# } */Math.Max(items is ICollection c ? (uint)c.Count : 0u, DictConstant.PrimeSizes__ext__[0]), (__uitype__)DictConstant.MinExtraCapacity/*# if (hasValue) { */,
                   stackDuplicateKeys/*# } */)
        {
            foreach (var item in items) Add(item);
        }

        //# if (isSym) {
        /// <summary>
        /// Create a __type__ and initialize to contain the supplied items.
        /// Note hat this incurs the overhead of converting all string keys
        /// to symbols.
        //# if (hasValue) {
        /// If the optional parameter stackDuplicateKeys is set to true, the
        /// __type__ acts as a stack for all items with the same key.
        //# }
        /// </summary>
        public __type__(IEnumerable</*# if (hasValue) { */KeyValuePair</*# } */string/*# if (hasValue) { */, TValue>/*# } */> items/*# if (hasValue) { */,
                bool stackDuplicateKeys = false/*# } */)
            : this(Math.Max(items is ICollection c ? (uint)c.Count : 0u, DictConstant.PrimeSizes__ext__[0]), DictConstant.MinExtraCapacity/*# if (hasValue) { */,
                   stackDuplicateKeys/*# } */)
        {
            foreach (var item in items) Add(item);
        }

        //# } // isSym
        private __type__(
                /*# if (fun) { */Func<__tkey__, __itype__> hfun, /*# } */__uitype__ firstCapacity, __uitype__ extraCapacity/*# if (hasValue) { */,
                bool stackDuplicateKeys = false/*# } */)
        {
            //# if (concurrent) {
            m_version = 0;
            //# }
            //# if (fun) {
            m_hfun = hfun;
            //# }
            m_maxFillFactor = DictConstant.MaxFillFactorDefault;
            m_minFillFactor = DictConstant.MinFillFactorDefault;
            //# if (hasValue) {
            m_doNotStackDuplicateKeys = !stackDuplicateKeys;
            //# }
            m_capacityIndex = 0;
            m_capacity = DictConstant.PrimeSizes__ext__[m_capacityIndex];
            m_increaseThreshold = ComputeIncreaseThreshold(m_capacity);
            while (firstCapacity >= m_increaseThreshold)
            {
                m_capacity = DictConstant.PrimeSizes__ext__[++m_capacityIndex];
                m_increaseThreshold = ComputeIncreaseThreshold(m_capacity);
            }
            firstCapacity /= 4;
            while (firstCapacity >= extraCapacity) extraCapacity *= 2;
            m_decreaseThreshold = ComputeDecreaseThreshold(m_capacity);
            m_firstArray = new __NextHashItem__[m_capacity];
            m_extraArray = new __HashItemNext__[extraCapacity];
            m_freeIndex = AddSlotsToFreeList(m_extraArray, 1);
            m_count = 0;
            m_extraCount = 0;
            Report = 0;
        }

        //# } // !wrapped
        #endregion

        #region Properties

        //# if (!big) {
        /// <summary>
        /// Returns the number of items currently contained in the __type__.
        /// </summary>
        public int Count
        {
            get
            {
                //# if (wrapped) {
                return m_dict.Count;
                //# } else {
                return (int)m_count;
                //# }
            }
        }

        //# } // !big
        /// <summary>
        /// Returns the number of items currently contained in the __type__
        /// as long.
        /// </summary>
        public long LongCount
        {
            get
            {
                //# if (wrapped) {
                return m_dict.LongCount;
                //# } else {
                return (long)m_count;
                //# }
            }
        }

        /// <summary>
        /// Returns the number of items currently contained in the __type__
        /// as long.
        /// </summary>
        public __uitype__ Capacity
        {
            get
            {
                //# if (wrapped) {
                return m_dict.Capacity;
                //# } else {
                return m_capacity;
                //# }
            }
            set
            {
                //# if (wrapped) {
                m_dict.Capacity = value;
                //# } else {
                if (value < m_count)
                    throw new System.ArgumentOutOfRangeException("The new capacity is less than the current number of elements.");
                Resize(value, m_capacity);
                //# }
            }
        }

        //# if (!wrapped) {
        /// <summary>
        /// Setting the maximal fill factor makes it possible to fine-tune
        /// the performance for certain applications. Normally this should
        /// not be necessary.
        /// </summary>
        public float MaxFillFactor
        {
            get { return m_maxFillFactor; }
            set
            {
                //# if (concurrent) {
                Monitor.Enter(this); try {
                //# }
                m_maxFillFactor = value;
                m_increaseThreshold = ComputeIncreaseThreshold(m_capacity);
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
            }
        }

        /// <summary>
        /// Setting the minimal fill factor makes it possible to influence
        /// the shrinking behaviour of the __type__. Normally this should
        /// be set to a quater of the maximal fill factor. In order to
        /// completely prevent shrinking it can also be set to 0.0f.
        /// </summary>
        public float MinFillFactor
        {
            get { return m_minFillFactor; }
            set
            {
                //# if (concurrent) {
                Monitor.Enter(this); try {
                //# }
                m_minFillFactor = value;
                m_decreaseThreshold = ComputeDecreaseThreshold(m_capacity);
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
            }
        }

        //# if (!concurrent) {
        /// <summary>
        /// Always returns false. Part of the ICollection implementation.
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        public bool IsSynchronized { get { return false; } }

        public object SyncRoot { get { return this; } }
        //# }
        //# if (!hasValue) {
        public IEnumerable<__tkey__> Items { get { return Keys; } }
        //# } else { // hasValue
        public IEnumerable<KeyValuePair<__tkey__, TValue>> Items { get { return KeyValuePairs; } }
        //# }
        /// <summary>
        /// Returns all keys in the dictionary.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the enumeration.
        //# }
        /// </summary>
        public IEnumerable<__tkey__> Keys
        {
            get
            {
                //# if (concurrent) {
                Monitor.Enter(this); try { var version = m_version;
                //# }
                for (__uitype__ fi = 0; fi < m_capacity; fi++)
                {
                    var ei = m_firstArray[fi].Next;
                    if (ei == 0) continue;
                    //# if (!concurrent) {
                    yield return m_firstArray[fi].Item__Key__;
                    //# } else {
                    var k = m_firstArray[fi].Item__Key__;
                    Monitor.Exit(this); yield return k; Monitor.Enter(this);
                    if (version != m_version)
                        throw new ConcurrentDataModifiedException();
                    //# }
                    while (ei > 0)
                    {
                        //# if (!concurrent) {
                        yield return m_extraArray[ei].Item__Key__;
                        //# } else {
                        k = m_extraArray[ei].Item__Key__;
                        Monitor.Exit(this); yield return k; Monitor.Enter(this);
                        if (version != m_version)
                            throw new ConcurrentDataModifiedException();
                        //# }
                        ei = m_extraArray[ei].Next;
                    }
                }
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
            }
        }

        public Type KeyType { get { return typeof(__tkey__); } }

        //# if (!hasValue) {
        public IEnumerable<object> Objects
        {
            get
            {
                foreach (object obj in Keys)
                    yield return obj;
            }
        }

        //# }
        //# if (hasValue) {
        /// <summary>
        /// Returns all values in the __type__.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the enumeration.
        //# }
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                //# if (concurrent) {
                Monitor.Enter(this); try { var version = m_version;
                //# }
                for (__uitype__ fi = 0; fi < m_capacity; fi++)
                {
                    var ei = m_firstArray[fi].Next;
                    if (ei == 0) continue;
                    //# if (!concurrent) {
                    yield return m_firstArray[fi].Item.Value;
                    //# } else {
                    var v = m_firstArray[fi].Item.Value;
                    Monitor.Exit(this); yield return v; Monitor.Enter(this);
                    if (version != m_version)
                        throw new ConcurrentDataModifiedException();
                    //# }
                    while (ei > 0)
                    {
                        //# if (!concurrent) {
                        yield return m_extraArray[ei].Item.Value;
                        //# } else {
                        v = m_extraArray[ei].Item.Value;
                        Monitor.Exit(this); yield return v; Monitor.Enter(this);
                        if (version != m_version)
                            throw new ConcurrentDataModifiedException();
                        //# }
                        ei = m_extraArray[ei].Next;
                    }
                }
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
            }
        }

        public Type ValueType { get { return typeof(TValue); } }

        /// <summary>
        /// Returns all key value pairs in the __type__.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the enumeration.
        //# }
        /// </summary>
        public IEnumerable<KeyValuePair<__tkey__, TValue>> KeyValuePairs
        {
            get
            {
                //# if (concurrent) {
                Monitor.Enter(this); try { var version = m_version;
                //# }
                for (__uitype__ fi = 0; fi < m_capacity; fi++)
                {
                    var ei = m_firstArray[fi].Next;
                    if (ei == 0) continue;
                    //# if (!concurrent) {
                    yield return new KeyValuePair<__tkey__, TValue>(
                            m_firstArray[fi].Item__Key__, m_firstArray[fi].Item.Value);
                    //# } else {
                    var kvp = new KeyValuePair<__tkey__, TValue>(
                            m_firstArray[fi].Item__Key__, m_firstArray[fi].Item.Value);
                    Monitor.Exit(this); yield return kvp; Monitor.Enter(this);
                    if (version != m_version)
                        throw new ConcurrentDataModifiedException();
                    //# }
                    while (ei > 0)
                    {
                        //# if (!concurrent) {
                        yield return new KeyValuePair<__tkey__, TValue>(
                                m_extraArray[ei].Item__Key__, m_extraArray[ei].Item.Value);
                        //# } else {
                        kvp = new KeyValuePair<__tkey__, TValue>(
                                m_extraArray[ei].Item__Key__, m_extraArray[ei].Item.Value);
                        Monitor.Exit(this); yield return kvp; Monitor.Enter(this);
                        if (version != m_version)
                            throw new ConcurrentDataModifiedException();
                        //# }
                        ei = m_extraArray[ei].Next;
                    }
                }
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
            }
        }

        /// <summary>
        /// Returns all key value pairs in the __type__, in such a way,
        /// that the stack order is correct when the pairs are put into a
        /// new __type__ in the order they are returned.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the enumeration.
        //# }
        /// </summary>
        public IEnumerable<KeyValuePair<__tkey__, TValue>> KeyValuePairsForStorage
        {
            get
            {
                //# if (concurrent) {
                Monitor.Enter(this); try { var version = m_version;
                //# }
                var stack = new Stack<__itype__>();
                for (__uitype__ fi = 0; fi < m_capacity; fi++)
                {
                    var ei = m_firstArray[fi].Next;
                    if (ei == 0) continue;
                    while (ei > 0) { stack.Push(ei); ei = m_extraArray[ei].Next; }
                    //# if (concurrent) {
                    var kvp = default(KeyValuePair<__tkey__, TValue>);
                    //# }
                    while (stack.Count > 0)
                    {
                        ei = stack.Pop();
                        //# if (!concurrent) {
                        yield return new KeyValuePair<__tkey__, TValue>(
                                m_extraArray[ei].Item__Key__, m_extraArray[ei].Item.Value);
                        //# } else {
                        kvp = new KeyValuePair<__tkey__, TValue>(
                                m_extraArray[ei].Item__Key__, m_extraArray[ei].Item.Value);
                        Monitor.Exit(this); yield return kvp; Monitor.Enter(this);
                        if (version != m_version)
                            throw new ConcurrentDataModifiedException();
                        //# }
                    }
                    //# if (!concurrent) {
                    yield return new KeyValuePair<__tkey__, TValue>(
                            m_firstArray[fi].Item__Key__, m_firstArray[fi].Item.Value);
                    //# } else {
                    kvp = new KeyValuePair<__tkey__, TValue>(
                            m_firstArray[fi].Item__Key__, m_firstArray[fi].Item.Value);
                    Monitor.Exit(this); yield return kvp; Monitor.Enter(this);
                    if (version != m_version)
                        throw new ConcurrentDataModifiedException();
                    //# }
                }
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
            }
        }

        public IEnumerable<(object, object)> ObjectPairs
        {
            get
            {
                foreach (var kvp in KeyValuePairsForStorage)
                    yield return (kvp.Key, kvp.Value);
            }
        }

        //# } // hasValue
        //# } // !wrapped
        //# if (wrapped) {
        /// <summary>
        /// Return the non-concurrent contained __type__.
        /// </summary>
        public __nctype____tpar__ NonConcurrent
        {
            get { return m_dict; }
        }

        //# }
        #endregion

        //# if (hasValue) {
        #region Indexer

        /// <summary>
        /// Get or set the item with the supplied key. If multiple items with
        /// the same key are allowed, the __type__ acts as a stack.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[__tkey__ key]
        {
            get
            {
                //# if (wrapped) {
                Monitor.Enter(this); try { return m_dict[key]; } finally { Monitor.Exit(this); }
                //# } else { // !wrapped
                var hash = __HashFun__key__GetHash__;
                //# if (concurrent) {
                Monitor.Enter(this); try {
                //# }
                var fi = ((__uitype__)hash) % m_capacity;
                var ei = m_firstArray[fi].Next;
                if (ei == 0)
                    throw new KeyNotFoundException();
                if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
                    return m_firstArray[fi].Item.Value;
                while (ei > 0)
                {
                    if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                        && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                        return m_extraArray[ei].Item.Value;
                    ei = m_extraArray[ei].Next;
                }
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
                throw new KeyNotFoundException();
                //# } // !wrapped
            }
            set
            {
                //# if (wrapped) {
                Monitor.Enter(this); try { m_dict[key] = value; } finally { Monitor.Exit(this); }
                //# } else { // !wrapped
                var hash = __HashFun__key__GetHash__;
                //# if (concurrent) {
                Monitor.Enter(this); try { ++m_version;
                //# }
                if (m_count >= m_increaseThreshold) IncreaseCapacity();
                var fi = ((__uitype__)hash) % m_capacity;
                var ei = m_firstArray[fi].Next;
                if (ei == 0)
                {
                    ++m_count;
                    m_firstArray[fi].Next = -1;
                    //# if (hasKey) {
                    m_firstArray[fi].Item.Hash = hash;
                    //# }
                    m_firstArray[fi].Item__Key__ = key;
                    m_firstArray[fi].Item.Value = value;
                    return;
                }
                if (m_doNotStackDuplicateKeys)
                {
                    if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                        && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
                    {
                        m_firstArray[fi].Item.Value = value;
                        return;
                    }
                    while (ei > 0)
                    {
                        if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                            && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                        {
                            m_extraArray[ei].Item.Value = value;
                            return;
                        }
                        ei = m_extraArray[ei].Next;
                    }
                    ei = m_firstArray[fi].Next;
                }
                ++m_count;
                ++m_extraCount;
                if (m_freeIndex < 0)
                {
                    var length = m_extraArray.__ext__Length;
                    m_extraArray = m_extraArray.Resized(length * 2);
                    m_freeIndex = AddSlotsToFreeList(m_extraArray, length);
                }
                var ni = m_freeIndex;
                m_freeIndex = m_extraArray[ni].Next;

                m_extraArray[ni].Item = m_firstArray[fi].Item;
                m_extraArray[ni].Next = ei;

                m_firstArray[fi].Next = ni;
                //# if (hasKey) {
                m_firstArray[fi].Item.Hash = hash;
                //# }
                m_firstArray[fi].Item__Key__ = key;
                m_firstArray[fi].Item.Value = value;
                //# if (concurrent) {
                } finally { Monitor.Exit(this); }
                //# }
                //# } // !wrapped
            }
        }

        #endregion

        //# } // hasValue
        #region Public Methods

        //# var aortype = hasValue ? "void" : "bool";
        /// <summary>
        /// Add the item with supplied key/*# if (hasValue) { */ and the supplied value/*# } */
        /// both supplied as generic objects, to the __type__. Note that the supplied key
        /// and value are cast to the concrete type of the keys and values used in the __type__
        /// and this will fail if they are of different types.
        /// </summary>
        public __aortype__ AddObject(object objkey/*# if (hasValue) { */, object objvalue/*# } */)
        {
            /*# if (!hasValue) { */return /*# } */Add((__tkey__)objkey/*# if (hasValue) { */, (__tvalue__)objvalue/*# } */);
        }

        //# if (!hasValue && !big && !concurrent) {
        void ICollection<__tkey__>.Add(__tkey__ key)
        {
            Add(key);
        }

        //# }
        //# foreach (var isTry in new[] { false, true }) {
        //# var rtype = (isTry || !hasValue) ? "bool" : "void";
        //# var tryPrefix = isTry ? "Try" : "";
        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Add the item with the supplied key/*# if (hasValue) { */ and the supplied value/*# } */
        /// to the __type__.
        /// </summary>
        public __rtype__ __tryPrefix__Add(__tkey__ key/*#
                if (hasHashPar) { */, __itype__ hash/*# } 
                if (hasValue) { */, __tvalue__ value/*# } */)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                /*# if (isTry || !hasValue) { */return /*# } */m_dict.__tryPrefix__Add(key/*#
                        if (hasHashPar) { */, hash/*# } if (hasValue) { */, value/*# } */);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (!hasHashPar) {
            var hash = __HashFun__key__GetHash__;
            //# }
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            if (m_count >= m_increaseThreshold) IncreaseCapacity();
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0)
            {
                ++m_count;
                //# if (concurrent) {
                ++m_version;
                //# }
               m_firstArray[fi].Next = -1;
                //# if (hasKey) {
                m_firstArray[fi].Item.Hash = hash;
                //# }
                m_firstArray[fi].Item__Key__ = key;
                //# if (hasValue) {
                m_firstArray[fi].Item.Value = value;
                //# }
                return/*# if (isTry || !hasValue) { */ true/*# } */;
            }
            //# if (hasValue) {
            if (m_doNotStackDuplicateKeys)
            //# }
            {
                // no duplicate keys, check for existing entries
                if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
                {
                    //# if (hasValue && !isTry) {
                    throw new ArgumentException("duplicate key");
                    //# } else {
                    return/*# if (isTry || !hasValue) { */ false/*# } */;
                    //# }
                }
                while (ei > 0)
                {
                    if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                        && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                    {
                        //# if (hasValue && !isTry) {
                        throw new ArgumentException("duplicate key");
                        //# } else {
                        return/*# if (isTry || !hasValue) { */ false/*# } */;
                        //# }
                    }
                    ei = m_extraArray[ei].Next;
                }
                ei = m_firstArray[fi].Next;
            }
            ++m_count;
            //# if (concurrent) {
            ++m_version;
            //# }
            ++m_extraCount;
            if (m_freeIndex < 0)
            {
                var length = m_extraArray.Length;
                m_extraArray = m_extraArray.Resized(length * 2);
                m_freeIndex = AddSlotsToFreeList(m_extraArray, length);
            }
            var ni = m_freeIndex;
            m_freeIndex = m_extraArray[ni].Next;
            m_extraArray[ni].Item = m_firstArray[fi].Item;
            m_extraArray[ni].Next = ei;
            m_firstArray[fi].Next = ni;
            //# if (hasKey) {
            m_firstArray[fi].Item.Hash = hash;
            //# }
            m_firstArray[fi].Item__Key__ = key;
            //# if (hasValue) {
            m_firstArray[fi].Item.Value = value;
            //# }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            //# if (isTry || !hasValue) {
            return true;
            //# }
            //# } // !wrapped
        }

        //# } // hasHashPar
        //# } // isTry
        //# if (isSym) {
        /// <summary>
        /// Adds the supplied item to the __type__. Note that this incurs the
        /// overhead of converting the string key to a symbol.
        /// </summary>
        public void Add(string key/*# if (hasValue) { */, __tvalue__ value/*# } */)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                m_dict.Add(key.ToSymbol()/*# if (hasValue) { */, value/*# } */);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            Add(key.ToSymbol()/*# if (hasValue) { */, value/*# } */);
            //# } // !wrapped
        }

        //# if (hasValue) {
        /// <summary>
        /// Add the supplied item to the __type__. Note that this incurs the
        /// overhead of converting the string key to a symbol.
        /// </summary>
        public void Add(KeyValuePair<string, TValue> item)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                m_dict.Add(item.Key.ToSymbol(), item.Value);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            Add(item.Key.ToSymbol(), item.Value);
            //# } // !wrapped
        }

        //# } // hasValue
        //# } // isSym
        //# if (hasValue) {
        /// <summary>
        /// Add the supplied item to the __type__.
        /// </summary>
        public void Add(KeyValuePair<__tkey__, TValue> item)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                m_dict.Add(item.Key, item.Value);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            Add(item.Key, item.Value);
            //# } // !wrapped
        }

        /// <summary>
        /// Add the supplied items to the __type__.
        /// </summary>
        public void AddRange(IEnumerable<KeyValuePair<__tkey__, TValue>> items)
        {
            foreach (var item in items) Add(item);
        }

        //# } else { // !hasValue
        /// <summary>
        /// Add the supplied keys to the __type__.
        /// </summary>
        public void AddRange(IEnumerable<__tkey__> keys)
        {
            foreach (var key in keys) Add(key);
        }

        //# } // !hasValue
        //# foreach (var keyStr in new string[] { "", "Key" }) { if (keyStr != "" && !hasValue) continue;
        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Returns true if the __type__ contains the item with the supplied
        /// key.
        /// </summary>
        public bool Contains__keyStr__(__tkey__ key/*# if (hasHashPar) { */, __itype__ hash/*# } */)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try { return m_dict.Contains(key); } finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (!hasHashPar) {
            var hash = __HashFun__key__GetHash__;
            //# }
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0) return false;
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
                return true;
            while (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                    return true;
                ei = m_extraArray[ei].Next;
            }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            return false;
            //# } // !wrapped
        }

        //# } // hasHashPar
        //# } // keyStr
        //# if (hasValue) {
        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Returns true if the __type__ contains the item with the supplied
        /// key and the supplied value.
        /// </summary>
        public bool Contains(__tkey__ key/*# if (hasHashPar) { */, __itype__ hash/*# } */, TValue value)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try { return m_dict.Contains(key, value); } finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (!hasHashPar) {
            var hash = __HashFun__key__GetHash__;
            //# }
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0) return false;
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */
                && value.Equals(m_firstArray[fi].Item.Value))
                return true;
            while (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */
                    && value.Equals(m_firstArray[fi].Item.Value))
                    return true;
                ei = m_extraArray[ei].Next;
            }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            return false;
            //# } // !wrapped
        }

        //# } // hasHashPar
        /// <summary>
        /// Returns true if the __type__ contains the item with the supplied
        /// KeyValuePair.
        /// </summary>
        public bool Contains(KeyValuePair<__tkey__, TValue> item)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.Contains(item.Key, item.Value);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            return Contains(item.Key, item.Value);
            //# } // !wrapped
        }

        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        //# foreach (var hasDefault in new[] { false, true }) {
        //# foreach (var isCreate in new[] { false, true }) {
        //# if (hasDefault && isCreate) continue;
        //# var orCreate = isCreate ? "OrCreate" : "";
        /// <summary>
        /// Get the element with the supplied key.
        //# if (isCreate) {
        /// If the element is not found, the supplied creator is called to create
        /// a new element that is added to the __type__ and returned.
        //# } else if (hasDefault) {
        /// Returns the supplied default value if the element is not found.
        //# } else {
        /// Throws an exception if the element is not found. This is an alias of the indexer.
        //# }
        /// </summary>
        public TValue Get__orCreate__(__tkey__ key/*#
                if (hasHashPar) { */, __itype__ hash/*# }
                if (isCreate) { */, Func<__tkey__, TValue> creator/*# }
                if (hasDefault) { */, TValue defaultValue/*# } */)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.Get__orCreate__(key/*#
                            if (hasHashPar) { */, hash/*# } if (isCreate) { */, creator/*# } */);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (!hasHashPar) {
            var hash = __HashFun__key__GetHash__;
            //# }
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0)
            //# if (isCreate) {
            {
                if (m_count >= m_increaseThreshold)
                {
                    IncreaseCapacity();
                    var v0 = creator(key);
                    AddCreated(key, hash, v0);
                    return v0;
                }
                ++m_count;
                //# if (concurrent) {
                ++m_version;
                //# }
                m_firstArray[fi].Next = -1;
                //# if (hasKey) {
                m_firstArray[fi].Item.Hash = hash;
                //# }
                m_firstArray[fi].Item__Key__ = key;
                var value = creator(key);
                m_firstArray[fi].Item.Value = value;
                return value;
            }
            //# } else if (hasDefault) {
                return defaultValue;
            //# } else { // !isCreate && ! isDefault
                throw new KeyNotFoundException();
            //# }
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
                return m_firstArray[fi].Item.Value;
            while (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                    return m_extraArray[ei].Item.Value;
                ei = m_extraArray[ei].Next;
            }
            //# if (isCreate) {
            if (m_count >= m_increaseThreshold)
            {
                IncreaseCapacity();
                var v1 = creator(key);
                AddCreated(key, hash, v1);
                return v1;
            }
            ei = m_firstArray[fi].Next;
            ++m_count;
            //# if (concurrent) {
            ++m_version;
            //# }
            ++m_extraCount;
            if (m_freeIndex < 0)
            {
                var length = m_extraArray.Length;
                m_extraArray = m_extraArray.Resized(length * 2);
                m_freeIndex = AddSlotsToFreeList(m_extraArray, length);
            }
            var ni = m_freeIndex;
            m_freeIndex = m_extraArray[ni].Next;
            m_extraArray[ni].Item = m_firstArray[fi].Item;
            m_extraArray[ni].Next = ei;
            m_firstArray[fi].Next = ni;
            //# if (hasKey) {
            m_firstArray[fi].Item.Hash = hash;
            //# }
            m_firstArray[fi].Item__Key__ = key;
            var val = creator(key);
            m_firstArray[fi].Item.Value = val;
            return val;
            //# } else if (hasDefault) {
            return defaultValue;
            //# }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            //# if (!isCreate && !hasDefault) {
            throw new KeyNotFoundException();
            //# }
            //# } // !wrapped
        }

        //# if (!wrapped && isCreate && !hasHashPar) {
        void AddCreated(__tkey__ key, __itype__ hash, __tvalue__ value)
        {
            ++m_count;
            //# if (concurrent) {
            ++m_version;
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0)
            {
                m_firstArray[fi].Next = -1;
                //# if (hasKey) {
                m_firstArray[fi].Item.Hash = hash;
                //# }
                m_firstArray[fi].Item__Key__ = key;
                m_firstArray[fi].Item.Value = value;
                return;
            }
            ++m_extraCount;
            if (m_freeIndex < 0)
            {
                var length = m_extraArray.Length;
                m_extraArray = m_extraArray.Resized(length * 2);
                m_freeIndex = AddSlotsToFreeList(m_extraArray, length);
            }
            var ni = m_freeIndex;
            m_freeIndex = m_extraArray[ni].Next;
            m_extraArray[ni].Item = m_firstArray[fi].Item;
            m_extraArray[ni].Next = ei;
            m_firstArray[fi].Next = ni;
            //# if (hasKey) {
            m_firstArray[fi].Item.Hash = hash;
            //# }
            m_firstArray[fi].Item__Key__ = key;
            m_firstArray[fi].Item.Value = value;
        }

        //# } // !wrapped && isCreate && hasHashPar
        //# } // isCreate
        //# } // hasDefault
        //# } // hasHashPar
        /// <summary>
        /// Get the element with the supplied key. Returns the default
        /// value of the value type if the element is not found.
        /// </summary>
        public TValue GetOrDefault(__tkey__ key)
        {
            return Get(key, default(TValue));
        }

        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Try to retrieve the value with the supplied key, return true if
        /// successful and return the value via the out parameter.
        /// </summary>
        public bool TryGetValue(__tkey__ key, /*#
                if (hasHashPar) { */__itype__ hash, /*# } */out TValue value)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.TryGetValue(key, /*#
                        if (hasHashPar) { */hash, /*# } */out value);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (!hasHashPar) {
            var hash = __HashFun__key__GetHash__;
            //# }
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0) { value = default(TValue); return false; }
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
            {
                value = m_firstArray[fi].Item.Value;
                return true;
            }
            while (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                {
                    value = m_extraArray[ei].Item.Value;
                    return true;
                }
                ei = m_extraArray[ei].Next;
            }
            value = default(TValue);
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            return false;
            //# } // !wrapped
        }

        //# }
        //# if (!wrapped) {
        /// <summary>
        /// Return all the value with the given key. This method is only
        /// useful if multiple item with the same key are allowed.
        /// </summary>
        public IEnumerable<TValue> ValuesWithKey(__tkey__ key)
        {
            var hash = __HashFun__key__GetHash__;
            //# if (concurrent) {
            Monitor.Enter(this); try { var version = m_version;
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0) yield break;
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
                //# if (!concurrent) {
                yield return m_firstArray[fi].Item.Value;
            //# } else {
            {
                var v = m_firstArray[fi].Item.Value;
                Monitor.Exit(this); yield return v; Monitor.Enter(this);
                if (version != m_version)
                    throw new ConcurrentDataModifiedException();
            }
            //# }
            while (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                    //# if (!concurrent) {
                    yield return m_extraArray[ei].Item.Value;
                //# } else {
                {
                    var v = m_extraArray[ei].Item.Value;
                    Monitor.Exit(this); yield return v; Monitor.Enter(this);
                    if (version != m_version)
                        throw new ConcurrentDataModifiedException();
                }
                //# }
                ei = m_extraArray[ei].Next;
            }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
        }

        //# if (!concurrent) {
        /// <summary>
        /// Gets an enumerator for values with key. It is only useful
        /// if multiple item with the same key are allowed.
        /// Should be preferred over ValuesWithKey enumeration in 
        /// performance critical code.
        /// </summary>
        public ValuesWithKeyEnumerator GetValuesWithKeyEnumerator(__tkey__ key)
        {
            return new ValuesWithKeyEnumerator(this, key);
        }

        public struct ValuesWithKeyEnumerator : IEnumerator<TValue>
        {
            __type__/*# if (isGeneric) { */</*# if (hasKey) { */TKey/*# } if (hasValue) { if (hasKey) { */, /*# } */TValue/*# } */>/*# } */ m_dict;/*# if (hasKey) { */
            TKey m_key;/*# } */
            __itype__ m_hash;
            __itype__ m_extraIndex;
            TValue m_current;

            public ValuesWithKeyEnumerator(__type__/*# if (isGeneric) { */</*# if (hasKey) { */TKey/*# } if (hasValue) { if (hasKey) { */, /*# } */TValue/*# } */>/*# } */ dict, __tkey__ key)
            {
                m_dict = dict;/*# if (hasKey) { */
                m_key = key;/*# } */
                m_hash = /*# if (fun) {*/dict./*# } */__HashFun__key__GetHash__;
                m_extraIndex = int.MaxValue;
                m_current = default(TValue);
            }

            public TValue Current => m_current;

            object IEnumerator.Current => m_current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (m_extraIndex == int.MaxValue)
                {
                    var fa = m_dict.m_firstArray;
                    var fi = ((uint)m_hash) % m_dict.m_capacity;
                    m_extraIndex = fa[fi].Next;
                    if (m_extraIndex == 0) return false;
                    if (fa[fi].Item__Hash__ == m_hash/*# if (hasKey) { */
                        && m_key.Equals(fa[fi].Item__Key__)/*# } */)
                    {
                        m_current = fa[fi].Item.Value;
                        return true;
                    }
                }
                if (m_extraIndex > 0)
                {
                    var ea = m_dict.m_extraArray;
                    do
                    {
                        var valid = ea[m_extraIndex].Item__Hash__ == m_hash/*# if (hasKey) { */
                                    && m_key.Equals(ea[m_extraIndex].Item__Key__)/*# } */;
                        if (valid) m_current = ea[m_extraIndex].Item.Value;
                        m_extraIndex = ea[m_extraIndex].Next;
                        if (valid) return true;
                    } while (m_extraIndex > 0);
                }

                return false;
            }

            public void Reset()
            {
                m_extraIndex = int.MaxValue;
                m_current = default(TValue);
            }
        }
        //# } // !concurrent

        //# } // !wrapped
        /// <summary>
        /// Return the value with the given key, but skip a supplied number
        /// of entries with this key. This method is only useful if multiple
        /// item with the same key are allowed.
        /// </summary>
        public TValue ValueWithKeySkip(__tkey__ key, __itype__ skip)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.ValueWithKeySkip(key, skip);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            var hash = __HashFun__key__GetHash__;
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0)
                throw new KeyNotFoundException();
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
            {
                if (skip == 0) return m_firstArray[fi].Item.Value;
                --skip;
            }
            while (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                {
                    if (skip == 0) return m_extraArray[ei].Item.Value;
                    --skip;
                }
                ei = m_extraArray[ei].Next;
            }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            throw new KeyNotFoundException();
            //# } // !wrapped
        }

        /// <summary>
        /// Remove the item with the supplied key from the __type__ and
        /// return it. If multipe entries have the same key, the one that
        /// was inserted last is removed.
        /// If the item is not found, a KeyNotFoundException is thrown.
        /// </summary>
        public TValue GetAndRemove(__tkey__ key)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.GetAndRemove(key);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            TValue value;
            if (!TryRemove(key, out value))
                throw new KeyNotFoundException();
            return value;
            //# } // !wrapped
        }

        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Try to remove the item with the supplied key from the __type__.
        /// and return true if it was succesfully removed. If multipe
        /// entries have the same key, the one that was inserted last is
        /// removed. If the item is not found, false is returned.
        /// </summary>
        public bool Remove(__tkey__ key/*# if (hasHashPar) { */, __itype__ hash/*# } */)
        {
            TValue value;
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.TryRemove(key/*# if (hasHashPar) { */, hash/*# } */, out value);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            return TryRemove(key/*# if (hasHashPar) { */, hash/*# } */, out value);
            //# } // !wrapped
        }

        //# } // hasHashPar
        //# }
        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Remove the item with the supplied key/*# if (hasValue) { */ and the supplied value/*# } */
        /// from the __type__. Returns true if the value was removed.
        /// </summary>
        public bool Remove(__tkey__ key/*# if (hasHashPar) { */, __itype__ hash/*# } if (hasValue) { */, TValue value/*# } */)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.TryRemove(key/*# if (hasHashPar) { */, hash/*# } if (hasValue) { */, value/*# } */);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            return TryRemove(key/*# if (hasHashPar) { */, hash/*# } if (hasValue) { */, value/*# } */);
            //# } // !wrapped
        }

        //# } // hasHashPar
        //# if (hasValue) {
        /// <summary>
        /// Remove the item with the supplied KeyValuePair from the __type__.
        /// If the item is not found, a KeyNotFoundException is thrown.
        /// </summary>
        public bool Remove(KeyValuePair<__tkey__, TValue> item)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.TryRemove(item.Key, item.Value);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            return TryRemove(item.Key, item.Value);
            //# } // !wrapped
        }

        //# } // hasValue
        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Try to reomve the item with the supplied key. If multipe entries
        /// have the same key, the one that was inserted last is removed.
        /// Returns true if the item was found/*# if (hasValue) { */, which is returned via the out
        /// parameter/*# } */.
        /// </summary>
        public bool TryRemove(__tkey__ key/*# if (hasHashPar) { */, __itype__ hash/*# } if (hasValue) { */, out TValue value/*# } */)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.TryRemove(key/*# if (hasHashPar) { */, hash/*# } if (hasValue) { */, out value/*# } */);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (!hasHashPar) {
            var hash = __HashFun__key__GetHash__;
            //# }
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0)
            {
                /*# if (hasValue) { */value = default(TValue); /*# } */return false;
            }
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */)
            {
                //# if (hasValue) {
                value = m_firstArray[fi].Item.Value;
                //# }
                if (ei < 0)
                {
                    m_firstArray[fi].Next = 0;
                    m_firstArray[fi].Item = default(__HashItem__);
                }
                else
                {
                    m_firstArray[fi].Next = m_extraArray[ei].Next;
                    m_firstArray[fi].Item = m_extraArray[ei].Item;
                    m_extraArray[ei].Next = m_freeIndex;
                    m_extraArray[ei].Item = default(__HashItem__);
                    m_freeIndex = ei;
                    --m_extraCount;
                }
                if (--m_count < m_decreaseThreshold) DecreaseCapacity();
                //# if (concurrent) {
                ++m_version;
                //# }
                return true;
            }
            if (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */)
                {
                    //# if (hasValue) {
                    value = m_extraArray[ei].Item.Value;
                    //# }
                    m_firstArray[fi].Next = m_extraArray[ei].Next;
                    m_extraArray[ei].Next = m_freeIndex;
                    m_extraArray[ei].Item = default(__HashItem__);
                    m_freeIndex = ei;
                    --m_extraCount;
                    if (--m_count < m_decreaseThreshold) DecreaseCapacity();
                    //# if (concurrent) {
                    ++m_version;
                    //# }
                    return true;
                }
                for (var ni = m_extraArray[ei].Next; ni > 0;
                     ei = ni, ni = m_extraArray[ei].Next)
                {
                    if (m_extraArray[ni].Item__Hash__ == hash/*# if (hasKey) { */
                        && key.Equals(m_extraArray[ni].Item__Key__)/*# } */)
                    {
                        //# if (hasValue) {
                        value = m_extraArray[ni].Item.Value;
                        //# }
                        m_extraArray[ei].Next = m_extraArray[ni].Next;
                        m_extraArray[ni].Next = m_freeIndex;
                        m_extraArray[ni].Item = default(__HashItem__);
                        m_freeIndex = ni;
                        --m_extraCount;
                        if (--m_count < m_decreaseThreshold) DecreaseCapacity();
                        //# if (concurrent) {
                        ++m_version;
                        //# }
                        return true;
                    }
                }
            }
            //# if (hasValue) {
            value = default(TValue);
            //# }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            return false;
            //# } // !wrapped
        }

        //# } // hasHashPar
        //# if (hasValue) {
        //# foreach (var hasHashPar in new[] { false, true }) { if (hasHashPar && !hasKey) continue;
        /// <summary>
        /// Try to reomve the item with the supplied key and the supplied
        /// value. If multipe entries match, the one that was inserted last
        /// is removed. Returns true if the item was found.
        /// </summary>
        public bool TryRemove(__tkey__ key/*# if (hasHashPar) { */, __itype__ hash/*# } */, TValue value)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                return m_dict.TryRemove(key/*# if (hasHashPar) { */, hash/*# } */, value);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (!hasHashPar) {
            var hash = __HashFun__key__GetHash__;
            //# }
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            var fi = ((__uitype__)hash) % m_capacity;
            var ei = m_firstArray[fi].Next;
            if (ei == 0) return false;
            if (m_firstArray[fi].Item__Hash__ == hash/*# if (hasKey) { */
                && key.Equals(m_firstArray[fi].Item__Key__)/*# } */
                && value.Equals(m_firstArray[fi].Item.Value))
            {
                if (ei < 0)
                {
                    m_firstArray[fi].Next = 0;
                    m_firstArray[fi].Item = default(__HashItem__);
                }
                else
                {
                    m_firstArray[fi].Next = m_extraArray[ei].Next;
                    m_firstArray[fi].Item = m_extraArray[ei].Item;
                    m_extraArray[ei].Next = m_freeIndex;
                    m_extraArray[ei].Item = default(__HashItem__);
                    m_freeIndex = ei;
                    --m_extraCount;
                }
                if (--m_count < m_decreaseThreshold) DecreaseCapacity();
                //# if (concurrent) {
                ++m_version;
                //# }
                return true;
            }
            if (ei > 0)
            {
                if (m_extraArray[ei].Item__Hash__ == hash/*# if (hasKey) { */
                    && key.Equals(m_extraArray[ei].Item__Key__)/*# } */
                    && value.Equals(m_extraArray[ei].Item.Value))
                {
                    m_firstArray[fi].Next = m_extraArray[ei].Next;
                    m_extraArray[ei].Next = m_freeIndex;
                    m_extraArray[ei].Item = default(__HashItem__);
                    m_freeIndex = ei;
                    --m_extraCount;
                    if (--m_count < m_decreaseThreshold) DecreaseCapacity();
                    //# if (concurrent) {
                    ++m_version;
                    //# }
                    return true;
                }
                for (var ni = m_extraArray[ei].Next; ni > 0;
                     ei = ni, ni = m_extraArray[ei].Next)
                {
                    if (m_extraArray[ni].Item__Hash__ == hash/*# if (hasKey) { */
                        && key.Equals(m_extraArray[ni].Item__Key__)/*# } */
                        && value.Equals(m_extraArray[ni].Item.Value))
                    {
                        m_extraArray[ei].Next = m_extraArray[ni].Next;
                        m_extraArray[ni].Next = m_freeIndex;
                        m_extraArray[ni].Item = default(__HashItem__);
                        m_freeIndex = ni;
                        --m_extraCount;
                        if (--m_count < m_decreaseThreshold) DecreaseCapacity();
                        //# if (concurrent) {
                        ++m_version;
                        //# }
                        return true;
                    }
                }
            }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            return false;
            //# } // !wrapped
        }

        //# } // hasHashPar
        //# } // hasValue
        //# foreach (var toaKeys in new[] { true, false }) {
        //# if (!hasValue && !toaKeys) continue;
        //# var KeysValues = hasValue ? (toaKeys ? "Keys" : "Values") : "";
        //# var tkeyvalue = toaKeys ? tkey : "TValue";
        //# var KeyValue = toaKeys ? Key : ".Value";
        //# var keysvalues = toaKeys ? "keys" : "values";
        /// <summary>
        /// Returns all __keysvalues__ in the dictionary as an array.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the operation.
        //# }
        /// </summary>
        public __tkeyvalue__[] __KeysValues__ToArray()
        {
            //# if (wrapped) {
            return m_dict.__KeysValues__ToArray();
            //# } else {
            var array = new __tkeyvalue__[m_count];
            Copy__KeysValues__To(array, __izero__);
            return array;
            //# }
        }

        //# } // toaKeys
        //# if (hasValue) {
        /// <summary>
        /// Returns all KeyValuePairs in the dictionary as an array.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the operation.
        //# }
        /// </summary>
        public KeyValuePair<__tkey__, TValue>[] ToArray()
        {
            //# if (wrapped) {
            return m_dict.ToArray();
            //# } else {
            var array = new KeyValuePair<__tkey__, TValue>[m_count];
            CopyTo(array, __izero__);
            return array;
            //# }
        }

        //# } // hasValue
        /// <summary>
        /// Remove all items. Capacity remains unchanged.
        /// </summary>
        public void Clear()
        {
            //# if (wrapped) {
            Monitor.Enter(this); try { m_dict.Clear(); } finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (concurrent) {
            Monitor.Enter(this); try { ++m_version;
            //# }
            m_firstArray.Set(default(__NextHashItem__));
            m_extraArray.Set(default(__HashItemNext__));
            m_freeIndex = AddSlotsToFreeList(m_extraArray, 1);
            m_count = 0;
            m_extraCount = 0;
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            //# } // !wrapped
        }

        //# foreach (var toaKeys in new[] { true, false }) {
        //# if (!hasValue && !toaKeys) continue;
        //# var KeysValues = hasValue ? (toaKeys ? "Keys" : "Values") : "";
        //# var tkeyvalue = toaKeys ? tkey : "TValue";
        //# var KeyValue = toaKeys ? Key : ".Value";
        //# var keysvalues = toaKeys ? "keys" : "values";
        /// <summary>
        /// Copies all __keysvalues__ in the dictionary to the supplied
        /// array starting at the supplied index.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the operation.
        //# }
        /// </summary>
        public void Copy__KeysValues__To(__tkeyvalue__[] array, __itype__ index)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try
            {
                m_dict.Copy__KeysValues__To(array, index);
            }
            finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            for (__uitype__ fi = 0; fi < m_capacity; fi++)
            {
                var ei = m_firstArray[fi].Next;
                if (ei == 0) continue;
                array[index++] = m_firstArray[fi].Item__KeyValue__;
                while (ei > 0)
                {
                    array[index++] = m_extraArray[ei].Item__KeyValue__;
                    ei = m_extraArray[ei].Next;
                }
            }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            //# } // !wrapped
        }

        //# } // toaKeys
        //# if (hasValue) {
        /// <summary>
        /// Copies all KeyValuePairs in the dictionary into the supplied
        /// array starting at the supplied index.
        //# if (concurrent) {
        /// This may throw a ConcurrentDataModifiedException if data is
        /// modified by another task during the operation.
        //# }
        /// </summary>
        public void CopyTo(KeyValuePair<__tkey__, TValue>[] array, __itype__ index)
        {
            //# if (wrapped) {
            Monitor.Enter(this); try { m_dict.CopyTo(array, index); } finally { Monitor.Exit(this); }
            //# } else { // !wrapped
            //# if (concurrent) {
            Monitor.Enter(this); try {
            //# }
            for (__uitype__ fi = 0; fi < m_capacity; fi++)
            {
                var ei = m_firstArray[fi].Next;
                if (ei == 0) continue;
                array[index++] = new KeyValuePair<__tkey__, TValue>(
                                m_firstArray[fi].Item__Key__, m_firstArray[fi].Item.Value);
                while (ei > 0)
                {
                    array[index++] = new KeyValuePair<__tkey__, TValue>(
                                    m_extraArray[ei].Item__Key__, m_extraArray[ei].Item.Value);
                    ei = m_extraArray[ei].Next;
                }
            }
            //# if (concurrent) {
            } finally { Monitor.Exit(this); }
            //# }
            //# } // !wrapped
        }

        //# } // hasValue
        //# if (!concurrent) {
        /// <summary>
        /// Copy items into supplied array starting at supplied index.
        /// </summary>
        public void CopyTo(Array array, int index)
        {
            var typedArray = array as /*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */[];
            if (typedArray != null)
                CopyTo(typedArray, __icast__index);
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Retuns a concurrent wrapper around the __type__ to enable
        /// concurrent modifications.
        /// </summary>
        public __ctype____tpar__ AsConcurrent()
        {
            return new __ctype____tpar__(this);
        }
        
        //# }
        //# if (isSym && hasValue) {
        public void Add<TType>(TypedSymbol<TType> key, TType value)
            where TType : TValue
        {
            this.Add(key.Symbol, value);
        }

        public bool Contains<TType>(TypedSymbol<TType> key)
            where TType : TValue
        {
            return Contains(key.Symbol);
        }

        public bool ContainsKey<TType>(TypedSymbol<TType> key)
            where TType : TValue
        {
            return ContainsKey(key.Symbol);
        }

        public TType Get<TType>(TypedSymbol<TType> key)
            where TType : TValue
        {
            return (TType)this[key.Symbol];
        }

        public TType Get<TType>(TypedSymbol<TType> key, TType defaultValue)
            where TType : TValue
        {
            TType value;
            if (TryGetValue(key, out value)) return value;
            return defaultValue;
        }

        public TType GetOrDefault<TType>(TypedSymbol<TType> key)
            where TType : TValue
        {
            TType value;
            if (TryGetValue(key, out value)) return value;
            return default(TType);
        }

        public bool Remove<TType>(TypedSymbol<TType> key)
            where TType : TValue
        {
            return Remove(key.Symbol);
        }

        public void Set<TType>(TypedSymbol<TType> key, TType value)
            where TType : TValue
        {
            this[key.Symbol] = value;
        }

        public bool TryGetValue<TType>(TypedSymbol<TType> key, out TType value)
            where TType : TValue
        {
            TValue val;
            if (TryGetValue(key.Symbol, out val)) { value = (TType)val; return true; }
            value = default(TType);
            return false;
        }

        public TType GetAs<TType>(__tkey__ key)
            where TType : TValue
        {
            return (TType)this[key];
        }

        public TType GetAs<TType>(__tkey__ key, TType defaultValue)
            where TType : TValue
        {
            TValue value;
            if (TryGetValue(key, out value)) return (TType)value;
            return defaultValue;
        }

        public TType GetAsOrDefault<TType>(__tkey__ key)
            where TType : TValue
        {
            TValue value;
            if (TryGetValue(key, out value)) return (TType)value;
            return default(TType);
        }

        //# } // isSym
        #endregion
        //# if (!wrapped) {

        #region IEnumerable</*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */> Members

        IEnumerator</*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */> IEnumerable</*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */>.GetEnumerator()
        {
            /*# if (!concurrent) { */
            return new Enumerator(this);
            /*# } else { */
            return Key__ValuePair__s.GetEnumerator();
            /*# } */
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            /*# if (!concurrent) { */
            return new Enumerator(this);
            /*# } else { */
            return Key__ValuePair__s.GetEnumerator();
            /*# } */
        }

        #endregion
        /*# if (!concurrent) { */
        #region Enumerator

        public Enumerator GetEnumerator() { return new Enumerator(this); }

        public struct Enumerator : IEnumerator</*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */>
        {
            __type__/*# if (isGeneric) { */</*# if (hasKey) { */TKey/*# } if (hasValue) { if (hasKey) { */, /*# } */TValue/*# } */>/*# } */ m_dict;
            __itype__ m_index;
            __itype__ m_extraIndex;
            /*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */ m_current;

            public Enumerator(__type__/*# if (isGeneric) { */</*# if (hasKey) { */TKey/*# } if (hasValue) { if (hasKey) { */, /*# } */TValue/*# } */>/*# } */ dict)
            {
                m_dict = dict;
                m_index = 0;
                m_extraIndex = -1;
                m_current = default(/*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */);
            }

            public /*# if (hasValue) { */KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */ Current => m_current;

            object IEnumerator.Current => m_current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (m_extraIndex < 0)
                {
                    var fa = m_dict.m_firstArray;
                    var cap = fa.Length;
                    while (m_index < cap)
                    {
                        var nxt = fa[m_index].Next;
                        if (nxt != 0)
                        {
                            m_current = /*# if (hasValue) { */new KeyValuePair<__tkey__, TValue>(/*# } */fa[m_index].Item__Key__/*# if (hasValue) { */, fa[m_index].Item.Value)/*# } */;
                            m_extraIndex = nxt;
                            m_index++;
                            return true;
                        }
                        m_index++;
                    }
                    return false;
                }
                else
                {
                    var ea = m_dict.m_extraArray;
                    m_current = /*# if (hasValue) { */new KeyValuePair<__tkey__, TValue>(/*# } */ea[m_extraIndex].Item__Key__/*# if (hasValue) { */, ea[m_extraIndex].Item.Value)/*# } */;
                    m_extraIndex = ea[m_extraIndex].Next;
                    return true;
                }
            }

            public void Reset()
            {
                m_index = 0;
                m_extraIndex = -1;
                m_current = default(/*# if (hasValue) { */ KeyValuePair</*# } */__tkey__/*# if (hasValue) { */, TValue>/*# } */);
            }
        }

        #endregion
        /*# } */
        #region Private Helper Methods

        private __uitype__ ComputeIncreaseThreshold(__uitype__ capacity)
        {
            return (__uitype__)(capacity * (double)m_maxFillFactor);
        }

        private __uitype__ ComputeDecreaseThreshold(__uitype__ capacity)
        {
            return (__uitype__)(capacity * (double)m_minFillFactor);
        }

        private static __itype__ AddSlotsToFreeList(
                __HashItemNext__[] extraArray, __itype__ start)
        {
            var length = extraArray.Length - 1;
            for (__itype__ i = start; i < length; i++)
                extraArray[i].Next = i + 1;
            extraArray[length].Next = -1;
            return start;
        }

        private void IncreaseCapacity()
        {
            Resize(DictConstant.PrimeSizes__ext__[++m_capacityIndex], m_capacity);
        }

        private void DecreaseCapacity()
        {
            if (m_capacityIndex > 0)
                Resize(DictConstant.PrimeSizes__ext__[--m_capacityIndex], m_capacity);
        }

        /// <summary>
        /// Add item to the hashtable supplied in the parameters.
        /// This is used in resizing, therefore no size check is done.
        /// </summary>
        private static void Add(
                __HashItem__ item,
                __NextHashItem__[] firstArray,
                __uitype__ capacity,
                ref __HashItemNext__[] extraArray,
                ref __itype__ freeIndex,
                ref __uitype__ extraCount)
        {
            var fi = ((__uitype__)item__Hash__) % capacity;
            var ei = firstArray[fi].Next;
            if (ei == 0)
            {
                firstArray[fi].Next = -1;
                firstArray[fi].Item = item;
                return;
            }
            if (freeIndex < 0)
            {
                var length = extraArray.Length;
                extraArray = extraArray.Resized(length * 2);
                freeIndex = AddSlotsToFreeList(extraArray, length);
            }
            var ni = freeIndex;
            freeIndex = extraArray[ni].Next;

            extraArray[ni].Item = firstArray[fi].Item;
            extraArray[ni].Next = ei;

            firstArray[fi].Next = ni;
            firstArray[fi].Item = item;
            ++extraCount;
        }

        /// <summary>
        /// The resize method has to maintain the stack order of the lists in
        /// the extra array. Therefore one list reversal is necessary.
        /// </summary>
        private void Resize(__uitype__ newCapacity, __uitype__ oldCapacity)
        {
            m_increaseThreshold = ComputeIncreaseThreshold(newCapacity);
            m_decreaseThreshold = ComputeDecreaseThreshold(newCapacity);
            var firstArray = new __NextHashItem__[newCapacity];
            var extraArray = new __HashItemNext__[
                                    System.Math.Max((__itype__)DictConstant.MinExtraCapacity,
                                                    m_extraArray.__ext__Length / 2)];
            var freeIndex = AddSlotsToFreeList(extraArray, 1);
            __uitype__ newExtraCount = 0;
            if ((m_capacityIndex & 1) != 0)
            {
                for (__uitype__ fi = 0; fi < m_capacity; fi++)
                {
                    var ei = m_firstArray[fi].Next;
                    if (ei == 0) continue;
                    if (ei > 0)
                    {
                        // reverse the extra list
                        __itype__ head = -1;
                        do
                        {
                            __itype__ next = m_extraArray[ei].Next;
                            m_extraArray[ei].Next = head;
                            head = ei; ei = next;
                        }
                        while (ei > 0);

                        // insert the extras into the new table
                        do
                        {
                            Add(m_extraArray[head].Item, firstArray, newCapacity,
                                ref extraArray, ref freeIndex, ref newExtraCount);
                            head = m_extraArray[head].Next;
                        }
                        while (head > 0);
                    }
                    Add(m_firstArray[fi].Item, firstArray, newCapacity,
                        ref extraArray, ref freeIndex, ref newExtraCount);
                }
            }
            else
            {
                __uitype__ fi = m_capacity;
                while (fi-- > 0)
                {
                    var ei = m_firstArray[fi].Next;
                    if (ei == 0) continue;
                    if (ei > 0)
                    {
                        // reverse the extra list
                        __itype__ head = -1;
                        do
                        {
                            __itype__ next = m_extraArray[ei].Next;
                            m_extraArray[ei].Next = head;
                            head = ei; ei = next;
                        }
                        while (ei > 0);

                        // insert the extras into the new table
                        do
                        {
                            Add(m_extraArray[head].Item, firstArray, newCapacity,
                                ref extraArray, ref freeIndex, ref newExtraCount);
                            head = m_extraArray[head].Next;
                        }
                        while (head > 0);
                    }
                    Add(m_firstArray[fi].Item, firstArray, newCapacity,
                        ref extraArray, ref freeIndex, ref newExtraCount);
                }
            }

            if ((Report & DictReport.Resize) != 0)
                ReportStats((__uitype__)m_count - m_extraCount, oldCapacity,
                            m_extraCount, (__uitype__)m_extraArray.Length,
                            (__uitype__)m_count - newExtraCount, newCapacity,
                            newExtraCount, (__uitype__)extraArray.Length);

            m_firstArray = firstArray;
            m_capacity = newCapacity;
            m_extraArray = extraArray;
            m_freeIndex = freeIndex;
            m_extraCount = newExtraCount;
        }

        private void ReportStats(
                    __uitype__ oldFirstCount, __uitype__ oldCapacity,
                    __uitype__ oldExtraCount, __uitype__ oldExtraCapacity,
                    __uitype__ newFirstCount, __uitype__ newCapacity,
                    __uitype__ newExtraCount, __uitype__ newExtraCapacity)
        {
            Base.Report.Line("\nresize at {0}:", m_count);
            Base.Report.Line("  old: first {0,9}/{1,-9} [{2:00.0}%] - extra {3,9}/{4,-9} [{5:00.0}%]",
                    oldFirstCount, oldCapacity, (100.0 * oldFirstCount) / (double)oldCapacity,
                    oldExtraCount, oldExtraCapacity, (100.0 * oldExtraCount) / (double)oldExtraCapacity);
            Base.Report.Line("  new: first {0,9}/{1,-9} [{2:00.0}%] - extra {3,9}/{4,-9} [{5:00.0}%]",
                    newFirstCount, newCapacity, (100.0 * oldFirstCount) / (double)newCapacity,
                    newExtraCount, newExtraCapacity, (100.0 * newExtraCount) / (double)newExtraCapacity);
        }

        #endregion
        //# } // !wrapped
    }

    #endregion

    //# } // ti
    //# } // concurrent
    //# } // fast
    //# } // equatable
    #region Support Data Structures

    [Flags]
    public enum DictReport
    {
        Resize = 0x0001,
    }

    public class ConcurrentDataModifiedException : Exception
    {
        public ConcurrentDataModifiedException()
        { }
    }

    #endregion

    #region Internal Helper Structures

    //# foreach (var itype in new[] { "int", "long" }) {
    //#   var Long = itype == "int" ? "" : "Long";
    //#   var Int = itype == "int" ? "Int" : "Long";
    [StructLayout(LayoutKind.Sequential)]
    internal struct HashKeyValue__Long__<TKey, Tvalue>
    {
        public __itype__ Hash;
        public TKey Key;
        public Tvalue Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NextHashKeyValue__Long__<TKey, Tvalue>
    {
        public __itype__ Next;
        public HashKeyValue__Long__<TKey, Tvalue> Item;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HashKeyValueNext__Long__<TKey, Tvalue>
    {
        public HashKeyValue__Long__<TKey, Tvalue> Item;
        public __itype__ Next;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HashKey__Long__<TKey>
    {
        public __itype__ Hash;
        public TKey Key;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NextHashKey__Long__<TKey>
    {
        public __itype__ Next;
        public HashKey__Long__<TKey> Item;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HashKeyNext__Long__<TKey>
    {
        public HashKey__Long__<TKey> Item;
        public __itype__ Next;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct __Int__Value<Tvalue>
    {
        public __itype__ Key;
        public Tvalue Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Next__Int__Value<Tvalue>
    {
        public __itype__ Next;
        public __Int__Value<Tvalue> Item;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct __Int__ValueNext<Tvalue>
    {
        public __Int__Value<Tvalue> Item;
        public __itype__ Next;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Next__Int__
    {
        public __itype__ Next;
        public __itype__ Item;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct __Int__Next
    {
        public __itype__ Item;
        public __itype__ Next;
    }

    //# } // itype
    [StructLayout(LayoutKind.Sequential)]
    internal struct SymbolValue<Tvalue>
    {
        public Symbol Key;
        public Tvalue Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NextSymbolValue<Tvalue>
    {
        public int Next;
        public SymbolValue<Tvalue> Item;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SymbolValueNext<Tvalue>
    {
        public SymbolValue<Tvalue> Item;
        public int Next;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NextSymbol
    {
        public int Next;
        public Symbol Item;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SymbolNext
    {
        public Symbol Item;
        public int Next;
    }

    public static class DictConstant
    {
        #region Constants

        public const float MaxFillFactorDefault = 0.8f; // growing avg: (0.8 + 0.4)/2 = 0.6
        public const float MinFillFactorDefault = 0.2f; // shrinking avg: (0.4 + 0.2)/2 = 0.3
        public const int MinExtraCapacity = 4;

        public readonly static uint[] PrimeSizes =
        {
            /*    prime no.           prime */
            /*           2                3,       +  1 = 2^2 */
            /*           4 */             7,    // +  1 = 2^3, minimal size
            /*           6 */            13,    // +  3 = 2^4
            /*          11 */            31,    // +  1 = 2^5
            /*          18 */            61,    // +  3 = 2^6
            /*          31 */           127,    // +  1 = 2^7
            /*          54 */           251,    // +  5 = 2^8
            /*          97 */           509,    // +  3 = 2^9
            /*         172 */          1021,    // +  3 = 2^10
            /*         309 */          2039,    // +  9 = 2^11
            /*         564 */          4093,    // +  3 = 2^12
            /*        1028 */          8191,    // +  1 = 2^13
            /*        1900 */         16381,    // +  3 = 2^14
            /*        3512 */         32749,    // + 19 = 2^15
            /*        6542 */         65521,    // + 15 = 2^16
            /*       12251 */        131071,    // +  1 = 2^17
            /*       23000 */        262139,    // +  5 = 2^18
            /*       43390 */        524287,    // +  1 = 2^19
            /*       82025 */       1048573,    // +  3 = 2^20
            /*      155611 */       2097143,    // +  9 = 2^21
            /*      295947 */       4194301,    // +  3 = 2^22
            /*      564163 */       8388593,    // + 15 = 2^23
            /*     1077871 */      16777213,    // +  3 = 2^24
            /*     2063689 */      33554393,    // + 39 = 2^25
            /*     3957809 */      67108859,    // +  5 = 2^26
            /*     7603553 */     134217689,    // + 39 = 2^27
            /*    14630843 */     268435399,    // + 57 = 2^28
            /*    28192750 */     536870909,    // +  3 = 2^29
            /*    54400028 */    1073741789,    // + 35 = 2^30
            /*   105097565 */    2147483647,    // +  1 = 2^31
            /*   203280221 */    4294967291,    // +  5 = 2^32
        };

        public readonly static ulong[] PrimeSizesLong =
        {
            /*        prime no.               prime */
            /*               2                    3,       +  1 = 2^2 */
            /*               4 */                 7,    // +  1 = 2^3, minimal size
            /*               6 */                13,    // +  3 = 2^4
            /*              11 */                31,    // +  1 = 2^5
            /*              18 */                61,    // +  3 = 2^6
            /*              31 */               127,    // +  1 = 2^7
            /*              54 */               251,    // +  5 = 2^8
            /*              97 */               509,    // +  3 = 2^9
            /*             172 */              1021,    // +  3 = 2^10
            /*             309 */              2039,    // +  9 = 2^11
            /*             564 */              4093,    // +  3 = 2^12
            /*            1028 */              8191,    // +  1 = 2^13
            /*            1900 */             16381,    // +  3 = 2^14
            /*            3512 */             32749,    // + 19 = 2^15
            /*            6542 */             65521,    // + 15 = 2^16
            /*           12251 */            131071,    // +  1 = 2^17
            /*           23000 */            262139,    // +  5 = 2^18
            /*           43390 */            524287,    // +  1 = 2^19
            /*           82025 */           1048573,    // +  3 = 2^20
            /*          155611 */           2097143,    // +  9 = 2^21
            /*          295947 */           4194301,    // +  3 = 2^22
            /*          564163 */           8388593,    // + 15 = 2^23
            /*         1077871 */          16777213,    // +  3 = 2^24
            /*         2063689 */          33554393,    // + 39 = 2^25
            /*         3957809 */          67108859,    // +  5 = 2^26
            /*         7603553 */         134217689,    // + 39 = 2^27
            /*        14630843 */         268435399,    // + 57 = 2^28
            /*        28192750 */         536870909,    // +  3 = 2^29
            /*        54400028 */        1073741789,    // + 35 = 2^30
            /*       105097565 */        2147483647,    // +  1 = 2^31
            /*       203280221 */        4294967291,    // +  5 = 2^32
            /*       393615806 */        8589934583,    // +  9 = 2^33
            /*       762939111 */       17179869143,    // + 41 = 2^34
            /*      1480206279 */       34359738337,    // + 31 = 2^35
            /*      2874398515 */       68719476731,    // +  5 = 2^36
            /*      5586502348 */      137438953447,    // + 25 = 2^37
            /*     10866266172 */      274877906899,    // + 45 = 2^38
            /*     21151907950 */      549755813881,    // +  7 = 2^39
            /*     41203088796 */     1099511627689,    // + 87 = 2^40
            /*     80316571436 */     2199023255531,    // + 21 = 2^41
            /*    156661034233 */     4398046511093,    // + 11 = 2^42
            /*    305761713237 */     8796093022151,    // + 57 = 2^43
            /*    597116381732 */    17592186044399,    // + 17 = 2^44
        };

        #endregion

    }

    #endregion
}
