using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aardvark.Base
{
    #region IXml Interfaces

    public interface IXml
    {
        void WriteTo(StreamWriter writer, int indent = 0);
    }

    public interface IXmlText
    {
        Text GetText();
    }

    public interface IXmlItemProperty
    {
        XmlItem XmlItem { get; set; }
    }

    public interface IXmlDocProperty
    {
        XmlDoc XmlDoc { get; set; }
    }

    #endregion

    #region XmlAtt

    public struct XmlAtt : IXml
    {
        public Symbol Key;
        public Text Value;

        public XmlAtt(Symbol key, string value)
        {
            Key = key;
            Value = value.ToText();
        }

        public XmlAtt(Text key, Text value)
        {
            Key = Symbol.Create(key.ToString()); Value = value;
        }

        public readonly void WriteTo(StreamWriter writer, int indent = 0)
        {
            writer.Write(Key.ToString()); writer.Write("=\"");
            writer.Write(Value.ToString()); writer.Write('"');
        }
    }

    #endregion

    #region XmlItem

    public class XmlItem : IXml
    {
        public Symbol Name;
        public List<XmlAtt> AttList;
        public List<IXml> SubList;

        #region Constructors

        public XmlItem()
            : this(default(Symbol))
        { }

        public XmlItem(Symbol name, params XmlAtt[] atts)
        {
            Name = name;
            AttList = new List<XmlAtt>(atts);
            SubList = new List<IXml>();
        }

        public XmlItem(Symbol name, string str)
            : this(name, str.ToText())
        { }

        public XmlItem(Symbol name, Text text)
            : this(name)
        {
            SubList.Add(new XmlDirectText(text));
        }

        public XmlItem(Symbol name, object obj)
            : this(name, obj.ToString())
        { }

        public XmlItem(Symbol name, IXml xmlItem, params IXml[] xmlItems)
            : this(name)
        {
            Add(xmlItem);
            foreach (var item in xmlItems) Add(item);
        }

        #endregion

        #region Properties

        public void Add(XmlAtt att)
        {
            AttList.Add(att);
        }

        public int Count
        {
            get { return SubList.Count; }
        }

        public IXml this[int index]
        {
            get { return SubList[index]; }
            set { SubList[index] = value; }
        }

        #endregion

        #region Query Methods

        public int SubCount<T>()
            where T : class
        {
            int count = 0;
            foreach (var xml in SubList) if (xml is T) ++count;
            return count;
        }

        public Text Attribute(Symbol symbol)
        {
            foreach (var att in AttList)
                if (att.Key == symbol)
                    return att.Value;
            return default(Text);
        }

        public T FirstSub<T>()
            where T : class
        {
            T item;
            if (NextSubIndex(out item) < 0)
                return null;
            return item;
        }

        public T FirstSub<T>(out int index)
            where T : class
        {
            T item;
            if ((index = NextSubIndex(out item)) < 0) return null;
            return item;
        }

        public T NextSub<T>(ref int index)
            where T : class
        {
            T item;
            if ((index = NextSubIndex(out item, index)) < 0) return null;
            return item;
        }

        public XmlItem FirstSubItemWithName(string name)
        {
            return FirstSubItemWithName(Symbol.Create(name));
        }

        public XmlItem FirstSubItemWithName(Symbol name)
        {
            return FirstSub<XmlItem>(i => i.Name == name);
        }

        public T FirstSub<T>(Func<T, bool> predicate)
            where T : class
        {
            T item;
            int index;
            if ((index = NextSubIndex(out item)) < 0) return null;
            while (!predicate(item))
            {
                if ((index = NextSubIndex(out item, index)) < 0) return null;
            }
            return item;
        }

        public XmlItem FirstSubItemWithName(string name, out int index)
        {
            return FirstSubItemWithName(Symbol.Create(name), out index);
        }

        public XmlItem FirstSubItemWithName(Symbol name, out int index)
        {
            return FirstSub<XmlItem>(i => i.Name == name, out index);
        }

        public T FirstSub<T>(Func<T, bool> predicate, out int index)
            where T : class
        {
            T item;
            if ((index = NextSubIndex(out item)) < 0) return null;
            while (!predicate(item))
            {
                if ((index = NextSubIndex(out item, index)) < 0) return null;
            }
            return item;
        }

        public XmlItem NextSubItemWithName(string name, ref int index)
        {
            return NextSubItemWithName(Symbol.Create(name), ref index);
        }

        public XmlItem NextSubItemWithName(Symbol name, ref int index)
        {
            return NextSub<XmlItem>(i => i.Name == name, ref index);
        }

        public IEnumerable<XmlItem> SubItemsWithName(Symbol name)
        {
            XmlItem item; int index = -1;
            while ((item = NextSub<XmlItem>(i => i.Name == name, ref index)) != null)
                yield return item;
        }

        public T NextSub<T>(Func<T, bool> predicate, ref int index)
            where T : class
        {
            T item;
            do
            {
                if ((index = NextSubIndex(out item, index)) < 0) return null;
            }
            while (!predicate(item));
            return item;
        }

        public XmlItem RequiredSubItemWithName(string name)
        {
            return RequiredSubItemWithName(Symbol.Create(name));
        }

        public string RequiredSubItemStringOnlyWithName(Symbol name)
        {
            return RequiredSubItemWithName(name).RequiredOnlySubString;
        }

        public XmlItem RequiredSubItemWithName(Symbol name)
        {
            return RequiredSub<XmlItem>(i => i.Name == name);
        }

        public T RequiredSub<T>(Func<T, bool> predicate)
            where T : class
        {
            int index = -1;
            T item;
            do
            {
                if ((index = NextSubIndex(out item, index)) < 0)
                    throw new ArgumentException();
            }
            while (!predicate(item));
            return item;
        }

        public XmlItem SubItemWithName(string name)
        {
            return SubItemWithName(Symbol.Create(name));
        }

        public XmlItem SubItemWithName(Symbol name)
        {
            return Sub<XmlItem>(i => i.Name == name);
        }

        public T Sub<T>(Func<T, bool> predicate)
            where T : class
        {
            int index = -1;
            T item;
            do
            {
                if ((index = NextSubIndex(out item, index)) < 0) return null;
            }
            while (!predicate(item));
            return item;
        }

        public XmlItem RequiredSubItemWithName(string name, out int index)
        {
            return RequiredSubItemWithName(Symbol.Create(name), out index);
        }

        public XmlItem RequiredSubItemWithName(Symbol name, out int index)
        {
            return RequiredSub<XmlItem>(i => i.Name == name, out index);
        }

        public T RequiredSub<T>(Func<T, bool> predicate, out int index)
            where T : class
        {
            index = -1;
            T item;
            do
            {
                if ((index = NextSubIndex(out item, index)) < 0)
                    throw new ArgumentException();
            }
            while (!predicate(item));
            return item;
        }

        public XmlItem SubItemWithName(string name, out int index)
        {
            return SubItemWithName(Symbol.Create(name), out index);
        }

        public XmlItem SubItemWithName(Symbol name, out int index)
        {
            return Sub<XmlItem>(i => i.Name == name, out index);
        }

        public T Sub<T>(Func<T, bool> predicate, out int index)
            where T : class
        {
            index = -1;
            T item;
            do
            {
                if ((index = NextSubIndex(out item, index)) < 0) return null;
            }
            while (!predicate(item));
            return item;
        }

        #endregion

        #region Required Sub

        public string RequiredOnlySubString
        {
            get
            {
                return RequiredOnlySubText.ToString();
            }
        }

        public Text RequiredOnlySubText
        {
            get
            {
                var text = RequiredOnlySub<IXmlText>();
                return text.GetText();
            }
        }

        public T RequiredOnlySub<T>()
            where T : class
        {
            if (SubList.Count != 1) throw new ArgumentException();
            var sub = SubList[0] as T;
            if (sub == null) throw new ArgumentException();
            return sub;
        }

        #endregion

        #region SubNodes by Index

        public int NextSubIndex<T>(int i = -1)
            where T : class
        {
            ++i;
            for (int c = SubList.Count; i < c; i++)
                if (SubList[i] is T) return i;
            return -1;
        }

        public int NextSubIndex<T>(out T item, int i = -1)
            where T : class
        {
            ++i;
            for (int c = SubList.Count; i < c; i++)
            {
                var node = SubList[i] as T;
                if (node != null) { item = node; return i; }
            }
            item = default(T);
            return -1;
        }

        #endregion

        #region Manipulation Methods

        public void Add(Text text)
        {
            SubList.Add(new XmlDirectText(text));
        }

        public void Add(IXml node)
        {
            if (node != null) SubList.Add(node);
        }

        public IXml Remove()
        {
            var last = SubList.Count - 1;
            var xml = SubList[last];
            SubList.RemoveAt(last);
            return xml;
        }

        #endregion

        #region IXml Members

        public void WriteTo(StreamWriter writer, int indent = 0)
        {
            var name = Name.ToString();
            var subItemCount = SubCount<XmlItem>();
            if (indent > 0) for (int i = 0; i < indent; i++) writer.Write("  ");
            writer.Write('<'); writer.Write(name);
            if (AttList.Count > 0)
                foreach (var att in AttList)
                {
                    writer.Write(' '); att.WriteTo(writer);
                }
            if (SubList.Count == 0)
            {
                if (AttList.Count > 0)
                    writer.WriteLine(" />");
                else
                    writer.WriteLine("/>");
            }
            else
            {
                writer.Write('>');
                if (subItemCount == 0)
                {
                    foreach (var xml in SubList) xml.WriteTo(writer);
                }
                else
                {
                    writer.WriteLine();
                    foreach (var xml in SubList)
                        xml.WriteTo(writer, indent + 1);
                    if (indent > 0)
                        for (int i = 0; i < indent; i++) writer.Write("  ");
                }
                writer.Write("</");
                writer.Write(name); writer.WriteLine('>');
            }
        }

        #endregion
    }

    #endregion

    #region XmlDirectText

    public class XmlDirectText : IXml, IXmlText
    {
        public Text Text;

        #region Constructors

        public XmlDirectText(Text text)
        {
            Text = text;
        }

        public XmlDirectText(string str)
            : this(str.ToText())
        { }

        #endregion

        #region IXml Members

        public void WriteTo(StreamWriter writer, int indent = 0)
        {
            if (indent == 0)
                writer.Write(Text.ToString());
            else if (Text.SkipWhiteSpace() < Text.Count)
            {
                for (int i = 0; i < indent; i++) writer.Write("  ");
                writer.Write(Text.ToString());
                writer.WriteLine();
            }
        }

        #endregion

        #region IXmlText Members

        public Text GetText() { return Text; }

        #endregion
    }

    #endregion

    #region XmlLazyText

    public class XmlLazyText : IXml, IXmlText
    {
        public Action<StreamWriter> WriteAct;

        public XmlLazyText(Action<StreamWriter> writeAct)
        {
            WriteAct = writeAct;
        }

        #region IXml Members

        public void WriteTo(StreamWriter writer, int indent = 0)
        {
            if (indent > 0) for (int i = 0; i < indent; i++) writer.Write("  ");
            WriteAct(writer);
            if (indent > 0) writer.WriteLine();
        }

        #endregion

        #region IXmlText Members

        public Text GetText()
        {
            var stream = new MemoryStream();
            WriteAct(new StreamWriter(stream));
            return new StreamReader(stream).ReadToEnd().ToText();
        }

        #endregion
    }

    #endregion

    #region XmlDoc

    public class XmlDoc : XmlItem, IXml
    {
        #region Constructor

        public XmlDoc(string version = null, string encoding = null)
            : base(XmlSymbol)
        {
            if (version != null) Add(new XmlAtt("version", version));
            if (encoding != null) Add(new XmlAtt("encoding", encoding));
        }

        public XmlDoc(XmlItem item, string version = null, string encoding = null)
            : this(version, encoding)
        {
            Add(item);
        }

        #endregion

        #region Constants

        public static readonly Symbol XmlSymbol = Symbol.Create("xml");

        #endregion

        #region IO

        public void SaveTo(string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
                WriteTo(streamWriter, 0);
        }

        public static XmlDoc LoadFrom(string filePath)
        {
            return LoadFrom(filePath, Encoding.Default);
        }

        public static XmlDoc LoadFrom(string filePath, Encoding encoding)
        {
            var fileString = File.ReadAllText(filePath, encoding);

            var parser = new XmlParser(); var doc = new XmlDoc();
            try
            {
                parser.Parse(fileString, doc);
            }
            catch (ParserException<XmlParser> e)
            {
                Console.WriteLine("(line {0}, col {1}): {2}",
                                  e.Parser.UserLine, e.Parser.UserColumn, e.Message);
            }

            return doc;
        }

        #endregion

        #region IXml Members

        public new void WriteTo(StreamWriter writer, int indent = 0)
        {
            if (AttList.Count > 0)
            {
                writer.Write("<?xml");
                foreach (var att in AttList)
                {
                    writer.Write(' '); att.WriteTo(writer);
                }
                writer.WriteLine("?>");
            }
            foreach (var xml in SubList)
            {
                var text = xml as XmlDirectText;
                if (text != null && text.Text.IsWhiteSpace) continue;
                xml.WriteTo(writer, indent);
            }
        }

        #endregion
    }

    #endregion

    #region XmlParser

    public class XmlParser : TextParser<XmlParser>
    {
        public bool SeenName;
        public bool SeenSlash;
        public bool PreserveWhiteSpace;
        public bool SimpleNames;
        public char Separator;

        public void Parse(string text, XmlItem node)
        {
            SeenName = false;
            SeenSlash = false;
            PreserveWhiteSpace = true;
            SimpleNames = true;
            Separator = ':';
            Parse(new Text(text), this, TextState, node);
        }

        /// <summary>
        /// A parser state is initialized with an array of cases which
        /// represent the state transitions. A state transition is chosen if
        /// its specified regular expression is the first one (closest to the
        /// current position) that matches. The specified function is
        /// performed when the transition is chosen, and the function needs
        /// to return the next state. If the 'null' state is returned, or the
        /// whole input is consumed, the parsing function that performs all
        /// transitions terminates. Since recursive descent also calls this
        /// parsing function, returning the 'null' state also signals return
        /// from recursive descent.
        /// </summary>
        public static readonly State<XmlParser, XmlItem> TextState
                            = new State<XmlParser, XmlItem>(new[] {
                new Case<XmlParser, XmlItem>(@"<\?xml", (p, n) =>
                    {   if (!(n is XmlDoc))
                        {
                            p.Skip(1);
                            throw new ParserException<XmlParser>(p, "illegal character");
                        }
                        p.Skip(); p.SeenName = true; p.SeenSlash = false;
                        return TagState; }),
                new Case<XmlParser, XmlItem>(@"<!DOCTYPE", (p, n) =>
                    { n.Add(p.GetToEndOf('>')); return TextState; }),
                new Case<XmlParser, XmlItem>(@"<!--", (p, n) =>
                    { p.SkipToEndOf("-->"); return TextState; }),
                new Case<XmlParser, XmlItem>(@"<", (p, n) =>
                    { p.Skip(); p.SeenName = p.SeenSlash = false;
                        return TagState; }),
                new Case<XmlParser, XmlItem>(@"&", (p, n) =>
                    { n.Add(p.GetToEndOf(';')); return TextState; }),
            },
            // If the following action is specified, all cases are search
            // operations that skip text which is supplied to the action.
            // In the Xml case, this is used to add the text between xml
            // tags to the node.
            (p, n, t) =>  { if (p.PreserveWhiteSpace || !t.IsWhiteSpace) n.Add(t); }
        );

        public static readonly Rx SymbolRx = new Rx(@"[A-Za-z_][0-9A-Za-z_]*");

        public static readonly State<XmlParser, XmlItem> TagState = new State<XmlParser, XmlItem>(new[] {
                new Case<XmlParser, XmlItem>(SymbolRx, (p, n) =>
                    {
                        var namePos = p.Pos;
                        Text nameText;
                        if (p.SimpleNames)
                            nameText = p.Get();
                        else
                        {
                            p.Skip();
                            while (p.TrySkip(p.Separator))
                                p.Skip(SymbolRx);
                            nameText = p.GetFrom(namePos);
                        }
                        if (p.EndOfText)
                            throw new ParserException<XmlParser>(p,
                                        "end of file after tag name");
                        var name = Symbol.Create(nameText.ToString());
                        if (p.SeenName) // its an attribute
                        {
                            if (namePos != p.LastWhiteSpace)
                            {
                                p.Pos = namePos; // adjust for error msg
                                throw new ParserException<XmlParser>(p,
                                            "missing required whitespace");
                            }
                            p.SkipWhiteSpace();
                            if (!p.TryGet('='))
                                throw new ParserException<XmlParser>(p,
                                            "tag attribute missing '='");
                            p.SkipWhiteSpace();
                            var quote = p.Peek;
                            if (quote != '"' && quote != '\'')
                                throw new ParserException<XmlParser>(p,
                                            "tag attribute not quoted");
                            p.Skip(1);  // skip quote
                            var value = p.GetToStartOf(quote);
                            p.Skip();   // skip quote
                            n.AttList.Add(new XmlAtt(nameText, value));
                            return TagState;
                        }
                        p.SeenName = true;
                        if (n.Name == null)
                        {
                            n.Name = name;
                            return TagState;
                        }
                        if (p.SeenSlash)
                        {
                            if (name != n.Name)
                                throw new ParserException<XmlParser>(p,
                                            "ending tag does not match");
                            return TagState;
                        }
                        var subNode = new XmlItem(name);    // node to parse into
                        Parse(p, TagState, subNode);        // recurse to parse xml-tree
                        n.Add(subNode);                     // add parsed node
                        return TextState;
                    }),
                new Case<XmlParser, XmlItem>(@"\?>", (p, n) =>
                    {
                        if (!(n is XmlDoc))
                            throw new ParserException<XmlParser>(p,  "illegal character");
                        p.Skip();
                        return TextState;
                    }),
                new Case<XmlParser, XmlItem>(@">", (p, n) =>
                    {
                        if (p.SeenSlash)
                        {
                            if (!p.SeenName)
                                throw new ParserException<XmlParser>(p,  "ending tag without name");
                            p.Skip();
                            return null; // no next state == recursion end
                        }
                        p.Skip();
                        return TextState;
                    }),
                new Case<XmlParser, XmlItem>(@"/", (p, n) =>
                    {
                        if (p.SeenSlash)
                            throw new ParserException<XmlParser>(p, "second slash in tag");
                        p.SeenSlash = true;
                        p.Skip();
                        return TagState;
                    }),
                // If a state has a default case, all cases represent
                // non-searching patterns, that need to match at the
                // start of the non-consumed input. If non of the other
                // cases match at this point, the default case is taken.
                // In the xml case the state inside tags is non-searching,
                // and its default case handles white space and illegal
                // chraracters insided tags.
                new Case<XmlParser, XmlItem>((p, n) =>  // default used for eating white space
                    {   
                        p.SkipWhiteSpaceAndCheckProgress(); return TagState;
                    }),
            });
    }

    #endregion
}
