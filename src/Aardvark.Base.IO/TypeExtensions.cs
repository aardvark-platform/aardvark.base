using System;
using System.Linq;

namespace Aardvark.Base.Coder
{
    public static class TypeFun
    {
        static readonly string[] c_genericSeparators = new[] { "<", ",", ">" };
        public static string GetCanonicalName(this Type self)
        {
            return GetCanonicalName(self, c_genericSeparators, false);
        }

        static readonly string[] c_xmlGenericSeparators = new[] { "_of_", "_", "" };
        public static string GetCanonicalXmlName(this Type self)
        {
            return GetCanonicalName(self, c_xmlGenericSeparators, true);
        }

        public static string GetCanonicalName(
                this Type self, string[] separators, bool xml)
        {
            var typeName = self.Name;
            if (self.IsGenericType)
            {
                var subTypeNames = (from t in self.GetGenericArguments()
                                    select t.GetCanonicalName(separators,
                                                              xml)
                                    ).ToArray();

                int quote = typeName.LastIndexOf("`");

                string numberString = (xml && subTypeNames.Length > 1)
                        ? subTypeNames.Length.ToString() + separators[1]
                        : "";

                return typeName.Substring(0, quote)
                        + separators[0]
                        + numberString
                        + String.Join(separators[1], subTypeNames)
                        + separators[2];
            }
            if (self.IsArray)
            {
                if (xml)
                {
                    var d = self.GetArrayRank();
                    return self.GetElementType().GetCanonicalName(separators, true)
                            + "_Array" + (d == 1 ? "" : d.ToString() + "d");
                }
                return typeName;
            }
            return typeName;
        }
    }
}
