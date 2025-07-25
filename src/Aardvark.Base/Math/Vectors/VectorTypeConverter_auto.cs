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

    
/////////////////////////////////////////////////////////////////////
//   dimension is 2
//   primitive type is int
//   vector type is V2i

[TypeConverter(typeof(V2iConverter))]
public partial struct V2i { }

public class V2iConverter : TypeConverter
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
            var v = new V2i((V2i)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V2i(
                (int)propertyValues["X0"], 
                (int)propertyValues["X1"]
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
    /// Convert from a specified type to a V2i, if possible
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

                if (parms.Length == 2)
                {
                    // Should have an integer and a string.
                    var x0 = (int)Convert.ToDouble(parms[0]);
                    var x1 = (int)Convert.ToDouble(parms[1]);

                    // And finally create the object

                    retVal = new V2i(x0, x1);
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
        var v = (V2i)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[2];

            argTypes[0] = typeof(int);
            argTypes[1] = typeof(int);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V2i).GetConstructor(argTypes);

            var arguments = new object[2];

            arguments[0] = v[0];
            arguments[1] = v[1];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[2];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(int));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 2
//   primitive type is long
//   vector type is V2l

[TypeConverter(typeof(V2lConverter))]
public partial struct V2l { }

public class V2lConverter : TypeConverter
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
            var v = new V2l((V2l)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V2l(
                (long)propertyValues["X0"], 
                (long)propertyValues["X1"]
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
    /// Convert from a specified type to a V2l, if possible
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

                if (parms.Length == 2)
                {
                    // Should have an integer and a string.
                    var x0 = (long)Convert.ToDouble(parms[0]);
                    var x1 = (long)Convert.ToDouble(parms[1]);

                    // And finally create the object

                    retVal = new V2l(x0, x1);
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
        var v = (V2l)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[2];

            argTypes[0] = typeof(long);
            argTypes[1] = typeof(long);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V2l).GetConstructor(argTypes);

            var arguments = new object[2];

            arguments[0] = v[0];
            arguments[1] = v[1];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[2];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(long));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 2
//   primitive type is float
//   vector type is V2f

[TypeConverter(typeof(V2fConverter))]
public partial struct V2f { }

public class V2fConverter : TypeConverter
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
            var v = new V2f((V2f)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V2f(
                (float)propertyValues["X0"], 
                (float)propertyValues["X1"]
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
    /// Convert from a specified type to a V2f, if possible
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

                if (parms.Length == 2)
                {
                    // Should have an integer and a string.
                    var x0 = (float)Convert.ToDouble(parms[0]);
                    var x1 = (float)Convert.ToDouble(parms[1]);

                    // And finally create the object

                    retVal = new V2f(x0, x1);
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
        var v = (V2f)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[2];

            argTypes[0] = typeof(float);
            argTypes[1] = typeof(float);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V2f).GetConstructor(argTypes);

            var arguments = new object[2];

            arguments[0] = v[0];
            arguments[1] = v[1];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[2];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(float));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 2
//   primitive type is double
//   vector type is V2d

[TypeConverter(typeof(V2dConverter))]
public partial struct V2d { }

public class V2dConverter : TypeConverter
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
            var v = new V2d((V2d)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V2d(
                (double)propertyValues["X0"], 
                (double)propertyValues["X1"]
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
    /// Convert from a specified type to a V2d, if possible
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

                if (parms.Length == 2)
                {
                    // Should have an integer and a string.
                    var x0 = (double)Convert.ToDouble(parms[0]);
                    var x1 = (double)Convert.ToDouble(parms[1]);

                    // And finally create the object

                    retVal = new V2d(x0, x1);
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
        var v = (V2d)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[2];

            argTypes[0] = typeof(double);
            argTypes[1] = typeof(double);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V2d).GetConstructor(argTypes);

            var arguments = new object[2];

            arguments[0] = v[0];
            arguments[1] = v[1];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[2];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(double));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 3
//   primitive type is int
//   vector type is V3i

[TypeConverter(typeof(V3iConverter))]
public partial struct V3i { }

public class V3iConverter : TypeConverter
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
            var v = new V3i((V3i)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V3i(
                (int)propertyValues["X0"], 
                (int)propertyValues["X1"], 
                (int)propertyValues["X2"]
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
    /// Convert from a specified type to a V3i, if possible
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

                if (parms.Length == 3)
                {
                    // Should have an integer and a string.
                    var x0 = (int)Convert.ToDouble(parms[0]);
                    var x1 = (int)Convert.ToDouble(parms[1]);
                    var x2 = (int)Convert.ToDouble(parms[2]);

                    // And finally create the object

                    retVal = new V3i(x0, x1, x2);
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
        var v = (V3i)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[3];

            argTypes[0] = typeof(int);
            argTypes[1] = typeof(int);
            argTypes[2] = typeof(int);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V3i).GetConstructor(argTypes);

            var arguments = new object[3];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[3];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(int));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 3
//   primitive type is long
//   vector type is V3l

[TypeConverter(typeof(V3lConverter))]
public partial struct V3l { }

public class V3lConverter : TypeConverter
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
            var v = new V3l((V3l)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V3l(
                (long)propertyValues["X0"], 
                (long)propertyValues["X1"], 
                (long)propertyValues["X2"]
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
    /// Convert from a specified type to a V3l, if possible
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

                if (parms.Length == 3)
                {
                    // Should have an integer and a string.
                    var x0 = (long)Convert.ToDouble(parms[0]);
                    var x1 = (long)Convert.ToDouble(parms[1]);
                    var x2 = (long)Convert.ToDouble(parms[2]);

                    // And finally create the object

                    retVal = new V3l(x0, x1, x2);
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
        var v = (V3l)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[3];

            argTypes[0] = typeof(long);
            argTypes[1] = typeof(long);
            argTypes[2] = typeof(long);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V3l).GetConstructor(argTypes);

            var arguments = new object[3];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[3];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(long));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 3
//   primitive type is float
//   vector type is V3f

[TypeConverter(typeof(V3fConverter))]
public partial struct V3f { }

public class V3fConverter : TypeConverter
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
            var v = new V3f((V3f)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V3f(
                (float)propertyValues["X0"], 
                (float)propertyValues["X1"], 
                (float)propertyValues["X2"]
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
    /// Convert from a specified type to a V3f, if possible
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

                if (parms.Length == 3)
                {
                    // Should have an integer and a string.
                    var x0 = (float)Convert.ToDouble(parms[0]);
                    var x1 = (float)Convert.ToDouble(parms[1]);
                    var x2 = (float)Convert.ToDouble(parms[2]);

                    // And finally create the object

                    retVal = new V3f(x0, x1, x2);
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
        var v = (V3f)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[3];

            argTypes[0] = typeof(float);
            argTypes[1] = typeof(float);
            argTypes[2] = typeof(float);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V3f).GetConstructor(argTypes);

            var arguments = new object[3];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[3];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(float));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 3
//   primitive type is double
//   vector type is V3d

[TypeConverter(typeof(V3dConverter))]
public partial struct V3d { }

public class V3dConverter : TypeConverter
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
            var v = new V3d((V3d)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V3d(
                (double)propertyValues["X0"], 
                (double)propertyValues["X1"], 
                (double)propertyValues["X2"]
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
    /// Convert from a specified type to a V3d, if possible
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

                if (parms.Length == 3)
                {
                    // Should have an integer and a string.
                    var x0 = (double)Convert.ToDouble(parms[0]);
                    var x1 = (double)Convert.ToDouble(parms[1]);
                    var x2 = (double)Convert.ToDouble(parms[2]);

                    // And finally create the object

                    retVal = new V3d(x0, x1, x2);
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
        var v = (V3d)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[3];

            argTypes[0] = typeof(double);
            argTypes[1] = typeof(double);
            argTypes[2] = typeof(double);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V3d).GetConstructor(argTypes);

            var arguments = new object[3];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[3];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(double));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 4
//   primitive type is int
//   vector type is V4i

[TypeConverter(typeof(V4iConverter))]
public partial struct V4i { }

public class V4iConverter : TypeConverter
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
            var v = new V4i((V4i)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V4i(
                (int)propertyValues["X0"], 
                (int)propertyValues["X1"], 
                (int)propertyValues["X2"], 
                (int)propertyValues["X3"]
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
    /// Convert from a specified type to a V4i, if possible
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

                if (parms.Length == 4)
                {
                    // Should have an integer and a string.
                    var x0 = (int)Convert.ToDouble(parms[0]);
                    var x1 = (int)Convert.ToDouble(parms[1]);
                    var x2 = (int)Convert.ToDouble(parms[2]);
                    var x3 = (int)Convert.ToDouble(parms[3]);

                    // And finally create the object

                    retVal = new V4i(x0, x1, x2, x3);
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
        var v = (V4i)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[4];

            argTypes[0] = typeof(int);
            argTypes[1] = typeof(int);
            argTypes[2] = typeof(int);
            argTypes[3] = typeof(int);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V4i).GetConstructor(argTypes);

            var arguments = new object[4];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];
            arguments[3] = v[3];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[4];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(int));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);
            values[3] = numberConverter.ConvertToString(context, culture, v[3]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 4
//   primitive type is long
//   vector type is V4l

[TypeConverter(typeof(V4lConverter))]
public partial struct V4l { }

public class V4lConverter : TypeConverter
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
            var v = new V4l((V4l)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V4l(
                (long)propertyValues["X0"], 
                (long)propertyValues["X1"], 
                (long)propertyValues["X2"], 
                (long)propertyValues["X3"]
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
    /// Convert from a specified type to a V4l, if possible
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

                if (parms.Length == 4)
                {
                    // Should have an integer and a string.
                    var x0 = (long)Convert.ToDouble(parms[0]);
                    var x1 = (long)Convert.ToDouble(parms[1]);
                    var x2 = (long)Convert.ToDouble(parms[2]);
                    var x3 = (long)Convert.ToDouble(parms[3]);

                    // And finally create the object

                    retVal = new V4l(x0, x1, x2, x3);
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
        var v = (V4l)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[4];

            argTypes[0] = typeof(long);
            argTypes[1] = typeof(long);
            argTypes[2] = typeof(long);
            argTypes[3] = typeof(long);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V4l).GetConstructor(argTypes);

            var arguments = new object[4];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];
            arguments[3] = v[3];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[4];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(long));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);
            values[3] = numberConverter.ConvertToString(context, culture, v[3]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 4
//   primitive type is float
//   vector type is V4f

[TypeConverter(typeof(V4fConverter))]
public partial struct V4f { }

public class V4fConverter : TypeConverter
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
            var v = new V4f((V4f)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V4f(
                (float)propertyValues["X0"], 
                (float)propertyValues["X1"], 
                (float)propertyValues["X2"], 
                (float)propertyValues["X3"]
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
    /// Convert from a specified type to a V4f, if possible
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

                if (parms.Length == 4)
                {
                    // Should have an integer and a string.
                    var x0 = (float)Convert.ToDouble(parms[0]);
                    var x1 = (float)Convert.ToDouble(parms[1]);
                    var x2 = (float)Convert.ToDouble(parms[2]);
                    var x3 = (float)Convert.ToDouble(parms[3]);

                    // And finally create the object

                    retVal = new V4f(x0, x1, x2, x3);
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
        var v = (V4f)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[4];

            argTypes[0] = typeof(float);
            argTypes[1] = typeof(float);
            argTypes[2] = typeof(float);
            argTypes[3] = typeof(float);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V4f).GetConstructor(argTypes);

            var arguments = new object[4];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];
            arguments[3] = v[3];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[4];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(float));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);
            values[3] = numberConverter.ConvertToString(context, culture, v[3]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

    
/////////////////////////////////////////////////////////////////////
//   dimension is 4
//   primitive type is double
//   vector type is V4d

[TypeConverter(typeof(V4dConverter))]
public partial struct V4d { }

public class V4dConverter : TypeConverter
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
            var v = new V4d((V4d)(propertyValues["Value"]));
            return v;
        }
        else
        {
            return new V4d(
                (double)propertyValues["X0"], 
                (double)propertyValues["X1"], 
                (double)propertyValues["X2"], 
                (double)propertyValues["X3"]
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
    /// Convert from a specified type to a V4d, if possible
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

                if (parms.Length == 4)
                {
                    // Should have an integer and a string.
                    var x0 = (double)Convert.ToDouble(parms[0]);
                    var x1 = (double)Convert.ToDouble(parms[1]);
                    var x2 = (double)Convert.ToDouble(parms[2]);
                    var x3 = (double)Convert.ToDouble(parms[3]);

                    // And finally create the object

                    retVal = new V4d(x0, x1, x2, x3);
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
        var v = (V4d)value;

        // If this is an instance descriptor...
        if (destinationType == typeof(InstanceDescriptor))
        {
            var argTypes = new System.Type[4];

            argTypes[0] = typeof(double);
            argTypes[1] = typeof(double);
            argTypes[2] = typeof(double);
            argTypes[3] = typeof(double);

            // Lookup the appropriate $TVector$ constructor
            ConstructorInfo constructor = typeof(V4d).GetConstructor(argTypes);

            var arguments = new object[4];

            arguments[0] = v[0];
            arguments[1] = v[1];
            arguments[2] = v[2];
            arguments[3] = v[3];

            // And return an instance descriptor to the caller.
            // Will fill in the CodeBehind stuff in VS.Net
            retVal = new InstanceDescriptor(constructor, arguments);
        }
        else if (destinationType == typeof(string))
        {
            // If it's a string, return one to the caller
            if (null == culture)
                culture = CultureInfo.CurrentCulture;

            var values = new string[4];

            // I'm a bit of a culture vulture - do it properly!
            TypeConverter numberConverter =
                TypeDescriptor.GetConverter(typeof(double));
            values[0] = numberConverter.ConvertToString(context, culture, v[0]);
            values[1] = numberConverter.ConvertToString(context, culture, v[1]);
            values[2] = numberConverter.ConvertToString(context, culture, v[2]);
            values[3] = numberConverter.ConvertToString(context, culture, v[3]);

            // A useful method - join an array of strings using a separator, in this instance the culture specific one
            retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
        }
        else
            retVal = base.ConvertTo(context, culture, value, destinationType);

        return retVal;
    }

}

