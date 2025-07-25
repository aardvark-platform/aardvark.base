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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Text;

// AUTO-GENERATED CODE - DO NOT EDIT!

namespace Aardvark.Base;

/*#
for (int Dim = 2; Dim <= 4; Dim++)
{
foreach (var TPrimitive in new string[] { "int", "long", "float", "double" })
{
    var TVector = "V" + Dim + TPrimitive[0];
*/    
/////////////////////////////////////////////////////////////////////
//   dimension is __Dim__
//   primitive type is __TPrimitive__
//   vector type is __TVector__

[TypeConverter(typeof(__TVector__Converter))]
public partial struct __TVector__ { }

public class __TVector__Converter : TypeConverter
{

    /// <summary>
    /// Can the framework call CreateInstance?
    /// </summary>
    public override bool GetCreateInstanceSupported(
        ITypeDescriptorContext context)
    {
        return true;
    }

    /// <summary>
    /// Satisfy the CreateInstance call by reading data from the
    /// propertyValues dictionary
    /// </summary>
    public override object CreateInstance(
        ITypeDescriptorContext context, IDictionary propertyValues
        )
    {
        if (propertyValues.Contains("Value"))
        {
            var v = new __TVector__((__TVector__)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new __TVector__(
//# for (int i = 0; i < Dim; i++) {
                (__TPrimitive__)propertyValues["X__i__"]__(i < Dim-1)?", ":""__
//# }
                );
        }
    }

    /// <summary>
    /// Does this struct expose properties?
    /// </summary>
    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
        return true;
    }

    /// <summary>
    /// Return the properties of this struct
    /// </summary>
    public override PropertyDescriptorCollection GetProperties(
        ITypeDescriptorContext context, object value, Attribute[] attributes
        )
    {
        return TypeDescriptor.GetProperties(value, attributes);
    }

    /// <summary>
    /// Check what this type can be created from
    /// </summary>
    public override bool CanConvertFrom(
        ITypeDescriptorContext context, System.Type sourceType
        )
    {
        // Just strings for now
        bool canConvert = (sourceType == typeof(string));

        if (!canConvert)
            canConvert = base.CanConvertFrom(context, sourceType);

        return canConvert;
    }

    /// <summary>
    /// Convert from a specified type to a __TVector__, if possible
    /// </summary>
    public override object ConvertFrom(
        ITypeDescriptorContext context,
        CultureInfo culture, object value
        )
    {
        object retVal = null;

        if (value is string sValue)
        {
            // Check that the string actually has something in it...
            sValue = sValue.Trim();

            if (sValue.Length != 0)
            {
                // Parse the string
                if (null == culture)
                    culture = CultureInfo.CurrentCulture;

                // Split the string based on the cultures list separator
                var parms = sValue.Split(
                    [culture.TextInfo.ListSeparator[0]]
                    );

                if (parms.Length == __Dim__)
                {
                    // Should have an integer and a string.
//# for (int i = 0; i < Dim; i++) {
                    var x__i__ = (__TPrimitive__)Convert.ToDouble(parms[__i__]);
//# }

                    // And finally create the object
/*#
var elements = new List<string>();
for (int i = 0; i < Dim; i++) elements.Add("x" + i);
*/
                    retVal = new __TVector__(__elements.Join(", ")__);
                }
            }
        }
        else
        {
            retVal = base.ConvertFrom(context, culture, value);
        }

        return retVal;
    }

    /// <summary>
    /// Check what the type can be converted to
    /// </summary>
    public override bool CanConvertTo(
        ITypeDescriptorContext context, Type destinationType
        )
    {
        // InstanceDescriptor is used in the code behind
        bool canConvert = (destinationType == typeof(InstanceDescriptor));

        if (!canConvert)
            canConvert = base.CanConvertFrom(context, destinationType);

        return canConvert;
    }

    /// <summary>
    /// Convert to a specified type
    /// </summary>
    public override object ConvertTo(
        ITypeDescriptorContext context,
        CultureInfo culture, object value,
        Type destinationType
        )
    {
        object retVal;
        var v = (__TVector__)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[__Dim__];

//# for (int i = 0; i < Dim; i++) {
            argTypes[__i__] = typeof(__TPrimitive__);
//# }

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(__TVector__).GetConstructor(argTypes);

            var arguments = new object[__Dim__];

//# for (int i = 0; i < Dim; i++) {
            arguments[__i__] = v[__i__];
//# }

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[__Dim__];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(__TPrimitive__));
//# for (int i = 0; i < Dim; i++) {
            values[__i__] = numberConverter.ConvertToString(context, culture, v[__i__]);
//# }

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

//#     }
//# }