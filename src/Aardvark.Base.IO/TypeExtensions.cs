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
using System.Linq;

namespace Aardvark.Base.Coder;

public static class TypeFun
{
    static readonly string[] c_genericSeparators = ["<", ",", ">"];
    public static string GetCanonicalName(this Type self)
    {
        return GetCanonicalName(self, c_genericSeparators, false);
    }

    static readonly string[] c_xmlGenericSeparators = ["_of_", "_", ""];
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

            int quote = typeName.LastIndexOf('`');

            string numberString = (xml && subTypeNames.Length > 1)
                    ? subTypeNames.Length.ToString() + separators[1]
                    : "";

#if NETSTANDARD2_0
            return typeName.Substring(0, quote)
#else
            return typeName[..quote]
#endif
                    + separators[0]
                    + numberString
                    + string.Join(separators[1], subTypeNames)
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
