using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Aardvark.Base.Coder
{
    [RegisterTypeInfo]
    public class Convertible : IFieldCodeable, IEquatable<Convertible>
    {
        public class IllegalSizeException : Exception
        {
            public IllegalSizeException(string message)
                : base(message)
            { }
        };

        private string m_descriptor;
        private object m_data;

        #region Constructors

        public Convertible() { }

        public Convertible(string descriptor)
            : this(descriptor, null)
        { }

        public Convertible(string descriptor, object data)
        {
            m_descriptor = descriptor;
            m_data = data;
        }

        // This resolves a resources leak when loading Bitmaps, but apparently breaks SlimDX10 rendering.
        //~Convertible()
        //{
        //    if (m_data is IDisposable)
        //    {
        //        var d = m_data as IDisposable;
        //        d.Dispose();
        //    }
        //}

        #endregion

        #region Properties

        public string Descriptor { get { return m_descriptor; } }

        public object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }
        #endregion

        #region Static Creator Functions

        /// <summary>
        /// Creates a convertible given its description (a matching creator must be registered).
        /// </summary>
        public static Convertible FromDescriptor(string descriptor)
        {
            return Converter.Global.FromDescriptor(descriptor);
        }

        /// <summary>
        /// Creates a file convertible given a file name (a matching creator must be registered).
        /// </summary>
        public static Convertible FromFile(string fileName)
        {
            return Converter.Global.FindConvertibleByFileName(fileName);
        }

        /// <summary>
        /// Creates a file convertible given a file name and a preferrable target used to
        /// determine the shortest possible path (a matching creator must be registered).
        /// </summary>
        public static Convertible FromFile(string fileName, Convertible preferrableTarget)
        {
            return Converter.Global.FindConvertibleByFileName(fileName, preferrableTarget);
        }

        /// <summary>
        /// Creates a file convertible given a file name and a preferrable target used to the
        /// determine the shortest possible path (a matching creator must be registered).
        /// </summary>
        public static Convertible FromFile(string fileName, string preferrableTargetDescriptor)
        {
            return Converter.Global.FindConvertibleByFileName(fileName, preferrableTargetDescriptor);
        }

        /// <summary>
        /// Creates a matching raw convertible from a given convertible (a matching raw convertible
        /// creator must be registered).
        /// </summary>
        public static Convertible FindMatchingRaw(Convertible c)
        {
            return Converter.Global.FindMatchingRaw(c);
        }

        /// <summary>
        /// Creates a matching raw convertible from a given convertible and a preferrable target
        /// used to the determine the shortest possible path (a matching raw convertible creator
        /// must be registered).
        /// </summary>
        public static Convertible FindMatchingRaw(Convertible c, Convertible preferrableTarget)
        {
            return Converter.Global.FindMatchingRaw(c, preferrableTarget);
        }

        /// <summary>
        /// Creates a matching raw convertible from a given convertible and a preferrable target
        /// used to the determine the shortest possible path (a matching raw convertible creator
        /// must be registered).
        /// </summary>
        public static Convertible FindMatchingRaw(Convertible c, string preferrableTargetDescriptor)
        {
            return Converter.Global.FindMatchingRaw(c, preferrableTargetDescriptor);
        }

        #endregion

        #region Conversion Methods

        /// <summary>
        /// Sets (i.e., replaces) the convertible.
        /// </summary>
        public void SetTo(Convertible convertible)
        {
            m_descriptor = convertible.m_descriptor;
            m_data = convertible.m_data;
        }

        /// <summary>
        /// Transmogrifies the convertible into the target convertible without
        /// changing the target. Returns this.
        /// </summary>
        public Convertible ChangeInto(Convertible target)
        {
            var newConv = new Convertible(target.Descriptor, target.Data);
            ConvertInto(newConv);
            SetTo(newConv);
            return this;
        }

        /// <summary>
        /// Converts the convertible into the target, that is modified.
        /// </summary>
        public void ConvertInto(Convertible target)
        {
            Converter.Global.Convert(this, target);
        }

        /// <summary>
        /// Returns the convertible converted to the target convertible without
        /// changing the target.
        /// </summary>
        public Convertible ConvertedTo(Convertible target)
        {
            var newConv = new Convertible(target.Descriptor, target.Data);
            ConvertInto(newConv);
            return newConv;
        }

        #endregion

        #region Query Methods

        /// <summary>
        /// Returns whether a convertible is directly convertible into the target.
        /// </summary>
        public bool IsDirectConvertibleTo(Convertible target)
        {
            return Converter.Global.IsDirectConversionPossible(this, target);
        }

        /// <summary>
        /// Returns whether a convertible is convertible into the target.
        /// </summary>
        public bool IsConvertibleTo(Convertible target)
        {
            return Converter.Global.IsConversionPossible(this, target);
        }

        /// <summary>
        /// Returns whether a convertible has data assigned.
        /// </summary>
        public bool HasData
        {
            get { return (m_data != null); }
        }

        #endregion

        #region IFieldCodeable Members

        public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        {
            yield return new FieldCoder(0, "Descriptor", (c, o) => c.CodeString(ref ((Convertible)o).m_descriptor));
            yield return new FieldCoder(1, "Data", (c, o) => c.CodeT(ref ((Convertible)o).m_data));
        }

        #endregion

        #region IEquatable<Convertible> Members

        public bool Equals(Convertible other)
        {
            if (!m_descriptor.Equals(other.m_descriptor)) return false;
            if (!m_data.Equals(other.m_data)) return false;
            return true;
        }

        #endregion

        #region HashCode and Equals

        public override int GetHashCode()
        {
            return HashCode.GetCombinedWithDefaultZero(m_descriptor, m_data);
        }

        public override bool Equals(object other)
        {
            var otherConvertible = other as Convertible;
            if (otherConvertible == null) return false;
            return Equals(otherConvertible);
        }

        public static bool operator ==(Convertible a, Convertible b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        public static bool operator !=(Convertible a, Convertible b)
        {
            if (object.ReferenceEquals(a, b)) return false;
            if (object.ReferenceEquals(a, null)) return true;
            if (object.ReferenceEquals(b, null)) return true;

            return !a.Equals(b);
        }

        #endregion
    }

    internal class Converter
    {
        #region Statics and Consts

        /// <summary>
        /// The one global converter.
        /// </summary>
        public static readonly Converter Global = new Converter();

        private const int c_distanceUnreachable = 200;
        private const int c_distanceComplex = 100;

        #endregion

        #region Members

        private struct RoutingEntry
        {
            public string Next;
            public int Distance;
        }

        private bool m_initialized = false;
        private SpinLock m_spinLock = new SpinLock();

        private readonly Dictionary<(string, string), Action<Convertible, Convertible>>
            m_actionMap = new Dictionary<(string, string), Action<Convertible, Convertible>>();

        private readonly Dictionary<(string, string), Annotation[]>
            m_annotationsMap = new Dictionary<(string, string), Annotation[]>();

        private readonly Dictionary<string, Dictionary<string, RoutingEntry>>
            m_routingMap = new Dictionary<string, Dictionary<string, RoutingEntry>>();

        private readonly Dictionary<(string, string), int>
            m_weightMap = new Dictionary<(string, string), int>();

        private readonly Dictionary<string, List<Func<string, Convertible>>>
            m_fileExtensionsMap = new Dictionary<string, List<Func<string, Convertible>>>();

        private readonly Dictionary<string, List<Func<Convertible>>>
            m_rawConvertiblesMap = new Dictionary<string, List<Func<Convertible>>>();

        private readonly Dictionary<string, List<Func<Convertible>>>
            m_creatorsMap = new Dictionary<string, List<Func<Convertible>>>();

        private readonly Dictionary<string, List<string>>
            m_resourcesMap = new Dictionary<string, List<string>>();

        private readonly List<string> m_availableResources = new List<string>();

        #endregion

        #region Constructor, Kernel startup

        static Converter()
        {
            Init();
        }

        /// <summary>
        /// Builds the initial routing table.
        /// </summary>
        public static void Init()
        {
            Global.BuildRoutingTables();
        }

        /// <summary>
        /// Creates a new converter with an empty routing table and no registered conversions.
        /// </summary>
        public Converter()
        {
        }

        /// <summary>
        /// Creates a new converter by copying an existing one.
        /// </summary>
        public Converter(Converter converter)
        {
            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                this.m_initialized = converter.m_initialized;
                this.m_availableResources = new List<string>(converter.m_availableResources);
                this.m_actionMap = converter.m_actionMap.Copy();
                this.m_annotationsMap = converter.m_annotationsMap.Copy();
                this.m_weightMap = converter.m_weightMap.Copy();
                this.m_fileExtensionsMap = Clone(converter.m_fileExtensionsMap);
                this.m_rawConvertiblesMap = Clone(converter.m_rawConvertiblesMap);
                this.m_creatorsMap = Clone(converter.m_creatorsMap);
                this.m_resourcesMap = Clone(converter.m_resourcesMap);
                this.m_routingMap = converter.CloneRoutingTable();
            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Clones this converter and returns the new one.
        /// </summary>
        private Converter Clone()
        {
            return new Converter(this);
        }

        /// <summary>
        /// Internal method for cloning a dictionary which contains a list.
        /// </summary>
        private Dictionary<string, List<T>> Clone<T>(Dictionary<string, List<T>> dictionary)
        {
            var clonedDictionary = new Dictionary<string, List<T>>(dictionary.Count);
            foreach (var kvp in dictionary)
                clonedDictionary[kvp.Key] = new List<T>(kvp.Value);
            return clonedDictionary;
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts the source convertible into the target convertible.
        /// There are 3 possibilities here:
        ///     1) There is a direct conversion possible, this works no matter if the conversion requires user-set parameters.
        ///     2) No direct conversion available. The conversion can be done indirectly in two cases:
        ///             - there is no conversion with user parameters needed.
        ///             - the only conversion with user parameters is the last one.
        ///             ... in all other cases the conversion is not possible.
        ///     3) The conversion would need more than two conversion with user parameters, which can not be done.
        /// </summary>
        /// <param name="source">The source convertible</param>
        /// <param name="target">The target convertible (result will be stored in target.Data)</param>
        public void Convert(Convertible source, Convertible target)
        {
            Convert(source, target, CloneRoutingTable(), new List<Convertible>());
        }

        /// <summary>
        /// Private helper for public Convert method above.
        /// </summary>
        private void Convert(
            Convertible source, Convertible target,
            Dictionary<string, Dictionary<string, RoutingEntry>> routingMap,
            List<Convertible> tempConvertibles)
        {
            var sourceDescr = source.Descriptor;
            var targetDescr = target.Descriptor;

            if (sourceDescr == targetDescr)
            {
                DirectConvert(source, target);
                return;
            }

            try
            {
                int distance = routingMap[sourceDescr][targetDescr].Distance;

                if (distance == 1 || distance == c_distanceComplex)
                {
                    DirectConvert(source, target);

                    if (tempConvertibles != null)
                    {
                        // dispose any temporary convertibles
                        foreach (var temp in tempConvertibles)
                        {
                            if (!temp.Data.Equals(target.Data))
                            {
                                var tempDisposable = temp.Data as IDisposable;
                                if (tempDisposable != null)
                                    tempDisposable.Dispose();
                            }
                        }

                        tempConvertibles.Clear();
                    }
                }
                else if (distance < c_distanceUnreachable)
                {
                    int lastDistance = FindLastDistance(sourceDescr, targetDescr);

                    if ((distance < c_distanceComplex) || (lastDistance == c_distanceComplex))
                    {
                        Convertible tmp = new Convertible(routingMap[sourceDescr][targetDescr].Next, null);

                        DirectConvert(source, tmp);

                        if (tempConvertibles == null)
                            tempConvertibles = new List<Convertible>();
                        tempConvertibles.Add(tmp);

                        Convert(tmp, target, routingMap, tempConvertibles);
                    }
                    else
                    {
                        throw new ConversionNotPossibleException(sourceDescr, targetDescr, "not reachable 1");
                    }
                }
                else
                {
                    throw new ConversionNotPossibleException(sourceDescr, targetDescr, "not reachable 2");
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ConversionNotPossibleException(sourceDescr, targetDescr, "key not found");
            }
        }

        /// <summary>
        /// Direct conversion between two convertibles.
        /// </summary>
        private void DirectConvert(Convertible source, Convertible target)
        {
            if (source.Data == null) throw new ArgumentNullException();

            var conversion = (source.Descriptor, target.Descriptor);

            Action<Convertible, Convertible> procedure;
            if (m_actionMap.TryGetValue(conversion, out procedure))
                procedure(source, target);
            else if (source.Descriptor == target.Descriptor)
            {
                target.Data = source.Data;
            }
            else
                throw new ConversionNotPossibleException(source.Descriptor, target.Descriptor, "convert failed");
        }

        /// <summary>
        /// Returns whether a direct conversion between two convertibles is possible.
        /// </summary>
        public bool IsDirectConversionPossible(Convertible source, Convertible target)
        {
            return IsDirectConversionPossible(source.Descriptor, target.Descriptor);
        }

        /// <summary>
        /// Returns whether a direct conversion between two convertibles is possible.
        /// </summary>
        public bool IsDirectConversionPossible(string source, string target)
        {
            if (source == target) return true;
            int distance = m_routingMap[source][target].Distance;
            return distance == 1 || distance == c_distanceComplex;
        }

        /// <summary>
        /// Returns whether a conversion will work or not.
        /// </summary>
        public bool IsConversionPossible(Convertible source, Convertible target)
        {
            return IsConversionPossible(source.Descriptor, target.Descriptor);
        }

        /// <summary>
        /// Returns whether a conversion will work or not.
        /// </summary>
        public bool IsConversionPossible(string source, string target)
        {
            if (source == target) return true;

            try
            {
                int distance = m_routingMap[source][target].Distance;

                // direct conversion without params || direct conversion with params
                if (distance == 1 || distance == c_distanceComplex)
                {
                    return true;
                }
                // conversion chain with at most one step requiring params
                else if (distance < c_distanceUnreachable)
                {
                    // get weight of last step in chain
                    int lastDistance = FindLastDistance(source, target);

                    // if all steps of chain are steps without params || only last step requires params
                    if ((distance < c_distanceComplex) || (lastDistance == c_distanceComplex))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        #endregion

        #region Registration

        /// <summary>
        /// Registers a parameterless conversion from a
        /// source convertible to a target convertible.
        /// </summary>
        public void Register(
            string sourceDescriptor, string targetDescriptor,
            Action<Convertible, Convertible> procedure)
        {
            if (string.IsNullOrEmpty(sourceDescriptor) ||
                string.IsNullOrEmpty(targetDescriptor) ||
                procedure == null) throw new ArgumentNullException();
            Register(sourceDescriptor, targetDescriptor, procedure, false);
        }

        /// <summary>
        /// Registers a parameterless conversion from a
        /// source convertible to a target convertible,
        /// and a list of annotations for this action.
        /// </summary>
        public void Register(
            string sourceDescriptor, string targetDescriptor,
            Action<Convertible, Convertible> procedure,
            IEnumerable<Annotation> annotations)
        {
            if (string.IsNullOrEmpty(sourceDescriptor) ||
                string.IsNullOrEmpty(targetDescriptor) ||
                procedure == null) throw new ArgumentNullException();
            Register(sourceDescriptor, targetDescriptor, procedure, false);
            Register(sourceDescriptor, targetDescriptor, annotations);
        }

        /// <summary>
        /// This registers a conversion from a source convertible described
        /// by a descriptor to a target convertible.
        /// </summary>
        /// <param name="sourceDescriptor">The source descriptor</param>
        /// <param name="targetDescriptor">The target descriptor</param>
        /// <param name="procedure">The conversion method converting source to target (result is stored in target.Data)</param>
        /// <param name="parameterRequired">
        /// Whether a user-set parameter is required for the conversion. This is important for conversion chaining. If no direct
        /// conversion is possible, the chaining of conversion only works if there is no parameter required or the only conversion
        /// with a parameter is the last one in the chain.
        /// </param>
        public void Register(
            string sourceDescriptor, string targetDescriptor,
            Action<Convertible, Convertible> procedure,
            bool parameterRequired)
        {
            if (string.IsNullOrEmpty(sourceDescriptor) ||
                string.IsNullOrEmpty(targetDescriptor) ||
                procedure == null) throw new ArgumentNullException();

            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                var conversion = (sourceDescriptor, targetDescriptor);
                if (!m_actionMap.ContainsKey(conversion))
                {
                    m_actionMap[conversion] = procedure;

                    if (parameterRequired)
                        m_weightMap[conversion] = c_distanceComplex;
                    else
                        m_weightMap[conversion] = 1;
                }
                else
                    throw new ArgumentException(
                        String.Format(
                            "conversion from \"{0}\" to \"{1}\" "
                            + "already registered",
                            sourceDescriptor, targetDescriptor));

                if (!m_routingMap.ContainsKey(sourceDescriptor))
                    AddNewRoutingObject(sourceDescriptor);

                if (!m_routingMap.ContainsKey(targetDescriptor))
                    AddNewRoutingObject(targetDescriptor);

                if (m_initialized)
                {
                    AddDirectRoute(conversion);
                    UpdateRoutingTable(conversion);
                }
            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Register a creator function for a given convertible, which allows convertibles
        /// to be created using only their descriptor.
        /// </summary>
        public void RegisterCreator(string descriptor, Func<Convertible> creator)
        {
            if (string.IsNullOrEmpty(descriptor) || creator == null)
                throw new ArgumentNullException();

            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                if (!m_creatorsMap.ContainsKey(descriptor))
                    m_creatorsMap[descriptor] = new List<Func<Convertible>>();

                m_creatorsMap[descriptor].Add(creator);
            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Registers a raw convertible for a given convertible descriptor.
        /// </summary>
        public void RegisterRaw(string descriptor, Func<Convertible> creator)
        {
            if (string.IsNullOrEmpty(descriptor) || creator == null)
                throw new ArgumentNullException();

            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                if (!m_rawConvertiblesMap.ContainsKey(descriptor))
                    m_rawConvertiblesMap[descriptor] = new List<Func<Convertible>>();

                m_rawConvertiblesMap[descriptor].Add(creator);
            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Registers a file convertible for a given file extension.
        /// </summary>
        public void RegisterExtension(string fileExtension, Func<string, Convertible> creator)
        {
            if (string.IsNullOrEmpty(fileExtension) || creator == null)
                throw new ArgumentNullException();

            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                fileExtension = fileExtension.ToLower();

                if (!m_fileExtensionsMap.ContainsKey(fileExtension))
                    m_fileExtensionsMap[fileExtension] = new List<Func<string, Convertible>>();

                m_fileExtensionsMap[fileExtension].Add(creator);
            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Registers a resource for a descriptor.
        /// </summary>
        public void RegisterResource(string resource, string descriptor)
        {
            if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(descriptor))
                throw new ArgumentNullException();

            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                if (!m_resourcesMap.ContainsKey(resource))
                    m_resourcesMap[resource] = new List<string>();

                m_resourcesMap[resource].Add(descriptor);

            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Registers a resource for a list of descriptors.
        /// </summary>
        public void RegisterResource(string resource, List<string> descriptors)
        {
            if (string.IsNullOrEmpty(resource) || descriptors == null)
                throw new ArgumentNullException();

            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                if (!m_resourcesMap.ContainsKey(resource))
                    m_resourcesMap[resource] = descriptors;
                else
                    m_resourcesMap[resource].AddRange(descriptors);

            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Registers annotations for the conversion from given source to target.
        /// Throws NullArgumentException if source- or target-descriptor is null.
        /// Does nothing if annotations is null.
        /// </summary>
        public void Register(string sourceDescriptor, string targetDescriptor,
            IEnumerable<Annotation> annotations)
        {
            if (string.IsNullOrEmpty(sourceDescriptor) || string.IsNullOrEmpty(targetDescriptor))
                throw new ArgumentNullException();
            if (annotations == null) return;
            m_annotationsMap[(sourceDescriptor, targetDescriptor)] = annotations.ToArray();
        }

        #endregion

        #region Routing table creation

        private void AddNewRoutingObject(String descriptor)
        {
            // add new routing table
            Dictionary<string, RoutingEntry> newRoutingTable = new Dictionary<string, RoutingEntry>();

            // add existing targets to this one
            foreach (string key in m_routingMap.Keys)
                newRoutingTable[key] = new RoutingEntry() { Distance = 99999999 };

            // add this new target to the existing routing tables
            foreach (Dictionary<string, RoutingEntry> table in m_routingMap.Values)
                table[descriptor] = new RoutingEntry() { Distance = 99999999 };

            m_routingMap[descriptor] = newRoutingTable;
        }

        private void AddDirectRoute((string, string) conversion)
        {
            m_routingMap[conversion.Item1][conversion.Item2] =
                    new RoutingEntry() { Distance = m_weightMap[conversion], Next = conversion.Item2 };
        }

        /// <summary>
        /// Builds the static routing table and stores the shortest distances.
        /// </summary>
        public void BuildRoutingTables()
        {
            var keys = m_routingMap.Keys.ToList();

            // clear all entries
            foreach (var key1 in keys)
                foreach (var key2 in keys)
                    m_routingMap[key1][key2] = new RoutingEntry() { Distance = 99999999 };

            var validConversions = (from conversion in m_actionMap.Keys
                                    where AreAllResourcesAvailable(conversion.Item1) &&
                                          AreAllResourcesAvailable(conversion.Item2)
                                    select conversion).ToList();

            // first add all direct routes
            foreach (var conversion in validConversions)
                AddDirectRoute(conversion);

            // now go and find the indirect routes
            foreach (var conversion in validConversions)
                UpdateRoutingTable(conversion);

            if (!m_initialized)
                m_initialized = true;
        }

        /// <summary>
        /// Clones the current routing table. This is usually done before a conversion is started
        /// because the routing may be altered by a different thread (e.g. by setting invalid
        /// convertibles).
        /// </summary>
        private Dictionary<string, Dictionary<string, RoutingEntry>> CloneRoutingTable()
        {
            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                var clonedTable = new Dictionary<string, Dictionary<string, RoutingEntry>>();
                foreach (var kvp in m_routingMap)
                    clonedTable[kvp.Key] = m_routingMap[kvp.Key].Copy();

                return clonedTable;
            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Updates the routing table with a conversion. If the conversion results
        /// into a shorter path, the updates are propagated.
        /// </summary>
        private void UpdateRoutingTable((string, string) conversion)
        {
            Dictionary<string, RoutingEntry> tableToUpdate = m_routingMap[conversion.Item1];
            Dictionary<string, RoutingEntry> tableWithNewInfo = m_routingMap[conversion.Item2];

            bool updated = false;

            var targetsToOptimize = (
                from target in tableToUpdate.Keys
                where target != conversion.Item2
                select target
            ).ToArray();

            foreach (var target in targetsToOptimize)
            {
                int alternativeDistance = tableWithNewInfo[target].Distance + tableToUpdate[conversion.Item2].Distance;

                // found a better way to the target
                if (alternativeDistance < tableToUpdate[target].Distance)
                {
                    tableToUpdate[target] = new RoutingEntry()
                    {
                        Distance = alternativeDistance,
                        Next = tableToUpdate[conversion.Item2].Next
                    };
                    updated = true;
                }
            }

            if (updated)
            {
                foreach (var conv in m_actionMap.Keys)
                {
                    if (conv.Item2 == conversion.Item1)
                        UpdateRoutingTable(conv);
                }
            }
        }

        #endregion

        #region FindXYZ, FromXYZ, ...

        /// <summary>
        /// Creates a convertible from a descriptor. Only works if a creator function was
        /// registered (see RegisterCreator).
        /// </summary>
        public Convertible FromDescriptor(string descriptor)
        {
            List<Func<Convertible>> creators;
            if (!m_creatorsMap.TryGetValue(descriptor, out creators))
                 return null;

            var func = creators.First();
            Convertible conv = func();

            return conv;
        }

        /// <summary>
        /// Returns a matching raw convertible for a convertible. Only works if a raw convertible
        /// was registered (see RegisterRaw).
        /// </summary>
        public Convertible FindMatchingRaw(Convertible c)
        {
            var desc = c.Descriptor;

            List<Func<Convertible>> creators;
            if (!m_rawConvertiblesMap.TryGetValue(desc, out creators))
                return null;

            var func = creators.First();
            Convertible conv = func();

            return conv;
        }

        /// <summary>
        /// Returns a matching raw convertible by also taking a preferred target into account.
        /// Only works if a raw convertible was registered (see RegisterRaw).
        /// </summary>
        public Convertible FindMatchingRaw(Convertible conv, Convertible preferrableTarget)
        {
            return FindMatchingRaw(conv, preferrableTarget.Descriptor);
        }

        /// <summary>
        /// Returns a matching raw convertible by also taking a preferred target into account.
        /// Only works if a raw convertible was registered (see RegisterRaw).
        /// </summary>
        public Convertible FindMatchingRaw(Convertible conv, string preferrableTargetDescriptor)
        {
            List<Func<Convertible>> creators;
            if (!m_rawConvertiblesMap.TryGetValue(conv.Descriptor, out creators))
                return null;

            var c = (from func in creators
                     let source = func()
                     let distance = m_routingMap[source.Descriptor][preferrableTargetDescriptor].Distance
                     orderby distance ascending
                     select source).First();
            return c;
        }

        /// <summary>
        /// Returns a matching file convertible given a file name.
        /// Only works if a convertible was registered for the file extension (see RegisterExtension).
        /// </summary>
        public Convertible FindConvertibleByFileName(string fileName)
        {
            var fileExt = Path.GetExtension(fileName).ToLower();

            List<Func<string, Convertible>> creators;
            if (!m_fileExtensionsMap.TryGetValue(fileExt, out creators))
                throw new ArgumentException("no convertible available for this file extension", fileExt);

            foreach (var func in creators)
            {
                Convertible conv = func(fileName);
                if (AreAllResourcesAvailable(conv.Descriptor))
                    return conv;
            }

            throw new ArgumentException("could not find a creator for this file extension", fileExt);
        }

        /// <summary>
        /// Returns a matching file convertible given a file name, taking a preferred target into account.
        /// Only works if a convertible was registered for the file extension (see RegisterExtension).
        /// </summary>
        public Convertible FindConvertibleByFileName(string fileName, Convertible preferrableTarget)
        {
            return FindConvertibleByFileName(fileName, preferrableTarget.Descriptor);
        }

        /// <summary>
        /// Returns a matching file convertible given a file name, taking a preferred target into account.
        /// Only works if a convertible was registered for the file extension (see RegisterExtension).
        /// </summary>
        public Convertible FindConvertibleByFileName(string fileName, string preferrableTargetDescriptor)
        {
            var fileExt = Path.GetExtension(fileName).ToLower();

            List<Func<string, Convertible>> creators;
            if (!m_fileExtensionsMap.TryGetValue(fileExt, out creators))
                return null;

            var c = (from func in creators
                     let source = func(fileName)
                     let distance = m_routingMap[source.Descriptor][preferrableTargetDescriptor].Distance
                     orderby distance ascending
                     select source).First();
            return c;
        }

        #endregion

        #region Resources

        /// <summary>
        /// Sets whether a resource is available or not. By default, resources are
        /// not available. If this is done after the routing table has been
        /// initialized once, the routing table will be rebuilt (therefore, use
        /// SetResourceAvailability(List{string} resources, bool available) if you need
        /// to set the availability of multiple resources).
        /// </summary>
        public void SetResourceAvailability(string resource, bool available)
        {
            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                InternalSetAvailability(resource, available);

                if (m_initialized)
                    BuildRoutingTables();

            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Sets whether resources are available or not. By default, resources are
        /// not available. If this is done after the routing table has been
        /// initialized once, the routing table will be rebuilt.
        /// </summary>
        public void SetResourceAvailability(List<string> resources, bool available)
        {
            bool lockTaken = false;
            try
            {
                m_spinLock.Enter(ref lockTaken);

                foreach (var resource in resources)
                    InternalSetAvailability(resource, available);

                if (m_initialized)
                    BuildRoutingTables();
            }
            finally
            {
                if (lockTaken) m_spinLock.Exit();
            }
        }

        /// <summary>
        /// Sets the availability of a resource.
        /// </summary>
        private void InternalSetAvailability(string resource, bool available)
        {
            bool contained = m_availableResources.Contains(resource);

            if (available && !contained)
                m_availableResources.Add(resource);
            else if (!available && contained)
                m_availableResources.Remove(resource);
        }

        /// <summary>
        /// Returns the required resources or null if none required.
        /// </summary>
        private List<string> RequiredResources(string descriptor)
        {
            var resources = new List<string>();

            foreach (var kvp in m_resourcesMap)
            {
                if (kvp.Value.Contains(descriptor))
                    resources.Add(kvp.Key);
            }

            return resources.Count > 0 ? resources : null;
        }

        /// <summary>
        /// Returns whether a convertible can be used (i.e., all needed resources
        /// are available).
        /// </summary>
        private bool AreAllResourcesAvailable(string descriptor)
        {
            var reqResources = RequiredResources(descriptor);

            if (reqResources == null)
                return true;

            foreach (var res in reqResources)
            {
                if (!m_availableResources.Contains(res))
                    return false;
            }

            return true;
        }

        #endregion

        #region Queries

        public struct Entry
        {
            public string SourceDescriptor;
            public string TargetDescriptor;
            public Action<Convertible, Convertible> Action;
            public IEnumerable<Annotation> Annotations;
        }

        /// <summary>
        /// Enumerates all conversions.
        /// </summary>
        public IEnumerable<Entry> Conversions
        {
            get
            {
                foreach (var kv in m_actionMap)
                    yield return GetConversion(kv.Key.Item1, kv.Key.Item2);
            }
        }

        /// <summary>
        /// Gets conversion for given source- and target description.
        /// </summary>
        public Entry GetConversion(string sourceDescription, string targetDescription)
        {
            var key = (sourceDescription, targetDescription);
            if (m_annotationsMap.ContainsKey(key))
            {
                return new Entry
                {
                    SourceDescriptor = sourceDescription,
                    TargetDescriptor = targetDescription,
                    Action = m_actionMap[key],
                    Annotations = GetAnnotations(sourceDescription, targetDescription)
                };
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Enumerates all conversions in the specified conversion chain,
        /// or returns null if no conversion exists.
        /// </summary>
        public IEnumerable<Entry> GetConversionChain(
            string sourceDescription, string targetDescription)
        {
            if (string.IsNullOrEmpty(sourceDescription)) throw new ArgumentNullException();
            if (string.IsNullOrEmpty(targetDescription)) throw new ArgumentNullException();
            if (!m_routingMap.ContainsKey(sourceDescription)) throw new ArgumentException("unknow convertible");
            var tmp = m_routingMap[sourceDescription];
            if (!tmp.ContainsKey(targetDescription)) return null; // no conversion exists

            return EnumerateChain(sourceDescription, targetDescription);
        }

        /// <summary>
        /// Enumerates annotations for given conversion.
        /// </summary>
        public IEnumerable<Annotation> GetAnnotations(string source, string target)
        {
            var key = (source, target);
            if (m_annotationsMap.ContainsKey(key)) return m_annotationsMap[key];
            return Array.Empty<Annotation>();
        }

        /// <summary>
        /// Helper method for GetChain.
        /// Requires that the conversion chain exists.
        /// </summary>
        private IEnumerable<Entry> EnumerateChain(string source, string target)
        {
            var next = m_routingMap[source][target].Next;
            var proc = new[] { GetConversion(source, next) };

            if (next == target) return proc;
            return proc.Concat(EnumerateChain(next, target));
        }

        /// <summary>
        /// Returns the "last" distance of a conversion chain.
        /// Requires that the conversion chain exists.
        /// </summary>
        private int FindLastDistance(string source, string target)
        {
            if (m_routingMap[source][target].Next == target)
                return m_routingMap[source][target].Distance;
            else
                return FindLastDistance(m_routingMap[source][target].Next, target);
        }

        #endregion
    }

    internal class ConversionNotPossibleException : Exception
    {
        public ConversionNotPossibleException(
            string source, string target, string text)
            : base(String.Format("unable to convert from \"{0}\" to \"{1}\": {2}",
                                 source, target, text))
        {
        }
    }
}
