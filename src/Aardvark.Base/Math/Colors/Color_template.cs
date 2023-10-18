using System;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# var ColorNames
    //#    = new Dictionary<string, V3d>()
    //#    {
    //#        { "AliceBlue", new V3d(0.941176, 0.972549, 1.000000) },
    //#        { "AntiqueWhite", new V3d(0.980392, 0.921569, 0.843137) },
    //#        { "Aqua", new V3d(0.000000, 1.000000, 1.000000) },
    //#        { "Aquamarine", new V3d(0.498039, 1.000000, 0.831373) },
    //#        { "Azure", new V3d(0.941176, 1.000000, 1.000000) },
    //#        { "Beige", new V3d(0.960784, 0.960784, 0.862745) },
    //#        { "Bisque", new V3d(1.000000, 0.894118, 0.768627) },
    //#        { "Black", new V3d(0.000000, 0.000000, 0.000000) },
    //#        { "BlanchedAlmond", new V3d(1.000000, 0.921569, 0.803922) },
    //#        { "Blue", new V3d(0.000000, 0.000000, 1.000000) },
    //#        { "BlueViolet", new V3d(0.541176, 0.168627, 0.886275) },
    //#        { "Brown", new V3d(0.647059, 0.164706, 0.164706) },
    //#        { "BurlyWood", new V3d(0.870588, 0.721569, 0.529412) },
    //#        { "CadetBlue", new V3d(0.372549, 0.619608, 0.627451) },
    //#        { "Chartreuse", new V3d(0.498039, 1.000000, 0.000000) },
    //#        { "Chocolate", new V3d(0.823529, 0.411765, 0.117647) },
    //#        { "Coral", new V3d(1.000000, 0.498039, 0.313725) },
    //#        { "CornflowerBlue", new V3d(0.392157, 0.584314, 0.929412) },
    //#        { "Cornsilk", new V3d(1.000000, 0.972549, 0.862745) },
    //#        { "Crimson", new V3d(0.862745, 0.078431, 0.235294) },
    //#        { "Cyan", new V3d(0.000000, 1.000000, 1.000000) },
    //#        { "DarkBlue", new V3d(0.000000, 0.000000, 0.545098) },
    //#        { "DarkCyan", new V3d(0.000000, 0.545098, 0.545098) },
    //#        { "DarkGoldenRod", new V3d(0.721569, 0.525490, 0.043137) },
    //#        { "DarkGray", new V3d(0.662745, 0.662745, 0.662745) },
    //#        { "DarkGrey", new V3d(0.662745, 0.662745, 0.662745) },
    //#        { "DarkGreen", new V3d(0.000000, 0.392157, 0.000000) },
    //#        { "DarkKhaki", new V3d(0.741176, 0.717647, 0.419608) },
    //#        { "DarkMagenta", new V3d(0.545098, 0.000000, 0.545098) },
    //#        { "DarkOliveGreen", new V3d(0.333333, 0.419608, 0.184314) },
    //#        { "DarkOrange", new V3d(1.000000, 0.549020, 0.000000) },
    //#        { "DarkOrchid", new V3d(0.600000, 0.196078, 0.800000) },
    //#        { "DarkRed", new V3d(0.545098, 0.000000, 0.000000) },
    //#        { "DarkSalmon", new V3d(0.913725, 0.588235, 0.478431) },
    //#        { "DarkSeaGreen", new V3d(0.560784, 0.737255, 0.560784) },
    //#        { "DarkSlateBlue", new V3d(0.282353, 0.239216, 0.545098) },
    //#        { "DarkSlateGray", new V3d(0.184314, 0.309804, 0.309804) },
    //#        { "DarkSlateGrey", new V3d(0.184314, 0.309804, 0.309804) },
    //#        { "DarkTurquoise", new V3d(0.000000, 0.807843, 0.819608) },
    //#        { "DarkViolet", new V3d(0.580392, 0.000000, 0.827451) },
    //#        { "DeepPink", new V3d(1.000000, 0.078431, 0.576471) },
    //#        { "DeepSkyBlue", new V3d(0.000000, 0.749020, 1.000000) },
    //#        { "DimGray", new V3d(0.411765, 0.411765, 0.411765) },
    //#        { "DimGrey", new V3d(0.411765, 0.411765, 0.411765) },
    //#        { "DodgerBlue", new V3d(0.117647, 0.564706, 1.000000) },
    //#        { "FireBrick", new V3d(0.698039, 0.133333, 0.133333) },
    //#        { "FloralWhite", new V3d(1.000000, 0.980392, 0.941176) },
    //#        { "ForestGreen", new V3d(0.133333, 0.545098, 0.133333) },
    //#        { "Fuchsia", new V3d(1.000000, 0.000000, 1.000000) },
    //#        { "Gainsboro", new V3d(0.862745, 0.862745, 0.862745) },
    //#        { "GhostWhite", new V3d(0.972549, 0.972549, 1.000000) },
    //#        { "Gold", new V3d(1.000000, 0.843137, 0.000000) },
    //#        { "GoldenRod", new V3d(0.854902, 0.647059, 0.125490) },
    //#        { "Gray", new V3d(0.501961, 0.501961, 0.501961) },
    //#        { "Grey", new V3d(0.501961, 0.501961, 0.501961) },
    //#        { "Green", new V3d(0.000000, 0.501961, 0.000000) },
    //#        { "GreenYellow", new V3d(0.678431, 1.000000, 0.184314) },
    //#        { "HoneyDew", new V3d(0.941176, 1.000000, 0.941176) },
    //#        { "HotPink", new V3d(1.000000, 0.411765, 0.705882) },
    //#        { "IndianRed ", new V3d(0.803922, 0.360784, 0.360784) },
    //#        { "Indigo ", new V3d(0.294118, 0.000000, 0.509804) },
    //#        { "Ivory", new V3d(1.000000, 1.000000, 0.941176) },
    //#        { "Khaki", new V3d(0.941176, 0.901961, 0.549020) },
    //#        { "Lavender", new V3d(0.901961, 0.901961, 0.980392) },
    //#        { "LavenderBlush", new V3d(1.000000, 0.941176, 0.960784) },
    //#        { "LawnGreen", new V3d(0.486275, 0.988235, 0.000000) },
    //#        { "LemonChiffon", new V3d(1.000000, 0.980392, 0.803922) },
    //#        { "LightBlue", new V3d(0.678431, 0.847059, 0.901961) },
    //#        { "LightCoral", new V3d(0.941176, 0.501961, 0.501961) },
    //#        { "LightCyan", new V3d(0.878431, 1.000000, 1.000000) },
    //#        { "LightGoldenRodYellow", new V3d(0.980392, 0.980392, 0.823529) },
    //#        { "LightGray", new V3d(0.827451, 0.827451, 0.827451) },
    //#        { "LightGrey", new V3d(0.827451, 0.827451, 0.827451) },
    //#        { "LightGreen", new V3d(0.564706, 0.933333, 0.564706) },
    //#        { "LightPink", new V3d(1.000000, 0.713725, 0.756863) },
    //#        { "LightSalmon", new V3d(1.000000, 0.627451, 0.478431) },
    //#        { "LightSeaGreen", new V3d(0.125490, 0.698039, 0.666667) },
    //#        { "LightSkyBlue", new V3d(0.529412, 0.807843, 0.980392) },
    //#        { "LightSlateGray", new V3d(0.466667, 0.533333, 0.600000) },
    //#        { "LightSlateGrey", new V3d(0.466667, 0.533333, 0.600000) },
    //#        { "LightSteelBlue", new V3d(0.690196, 0.768627, 0.870588) },
    //#        { "LightYellow", new V3d(1.000000, 1.000000, 0.878431) },
    //#        { "Lime", new V3d(0.000000, 1.000000, 0.000000) },
    //#        { "LimeGreen", new V3d(0.196078, 0.803922, 0.196078) },
    //#        { "Linen", new V3d(0.980392, 0.941176, 0.901961) },
    //#        { "Magenta", new V3d(1.000000, 0.000000, 1.000000) },
    //#        { "Maroon", new V3d(0.501961, 0.000000, 0.000000) },
    //#        { "MediumAquaMarine", new V3d(0.400000, 0.803922, 0.666667) },
    //#        { "MediumBlue", new V3d(0.000000, 0.000000, 0.803922) },
    //#        { "MediumOrchid", new V3d(0.729412, 0.333333, 0.827451) },
    //#        { "MediumPurple", new V3d(0.576471, 0.439216, 0.847059) },
    //#        { "MediumSeaGreen", new V3d(0.235294, 0.701961, 0.443137) },
    //#        { "MediumSlateBlue", new V3d(0.482353, 0.407843, 0.933333) },
    //#        { "MediumSpringGreen", new V3d(0.000000, 0.980392, 0.603922) },
    //#        { "MediumTurquoise", new V3d(0.282353, 0.819608, 0.800000) },
    //#        { "MediumVioletRed", new V3d(0.780392, 0.082353, 0.521569) },
    //#        { "MidnightBlue", new V3d(0.098039, 0.098039, 0.439216) },
    //#        { "MintCream", new V3d(0.960784, 1.000000, 0.980392) },
    //#        { "MistyRose", new V3d(1.000000, 0.894118, 0.882353) },
    //#        { "Moccasin", new V3d(1.000000, 0.894118, 0.709804) },
    //#        { "NavajoWhite", new V3d(1.000000, 0.870588, 0.678431) },
    //#        { "Navy", new V3d(0.000000, 0.000000, 0.501961) },
    //#        { "OldLace", new V3d(0.992157, 0.960784, 0.901961) },
    //#        { "Olive", new V3d(0.501961, 0.501961, 0.000000) },
    //#        { "OliveDrab", new V3d(0.419608, 0.556863, 0.137255) },
    //#        { "Orange", new V3d(1.000000, 0.647059, 0.000000) },
    //#        { "OrangeRed", new V3d(1.000000, 0.270588, 0.000000) },
    //#        { "Orchid", new V3d(0.854902, 0.439216, 0.839216) },
    //#        { "PaleGoldenRod", new V3d(0.933333, 0.909804, 0.666667) },
    //#        { "PaleGreen", new V3d(0.596078, 0.984314, 0.596078) },
    //#        { "PaleTurquoise", new V3d(0.686275, 0.933333, 0.933333) },
    //#        { "PaleVioletRed", new V3d(0.847059, 0.439216, 0.576471) },
    //#        { "PapayaWhip", new V3d(1.000000, 0.937255, 0.835294) },
    //#        { "PeachPuff", new V3d(1.000000, 0.854902, 0.725490) },
    //#        { "Peru", new V3d(0.803922, 0.521569, 0.247059) },
    //#        { "Pink", new V3d(1.000000, 0.752941, 0.796078) },
    //#        { "Plum", new V3d(0.866667, 0.627451, 0.866667) },
    //#        { "PowderBlue", new V3d(0.690196, 0.878431, 0.901961) },
    //#        { "Purple", new V3d(0.501961, 0.000000, 0.501961) },
    //#        { "Red", new V3d(1.000000, 0.000000, 0.000000) },
    //#        { "RosyBrown", new V3d(0.737255, 0.560784, 0.560784) },
    //#        { "RoyalBlue", new V3d(0.254902, 0.411765, 0.882353) },
    //#        { "SaddleBrown", new V3d(0.545098, 0.270588, 0.074510) },
    //#        { "Salmon", new V3d(0.980392, 0.501961, 0.447059) },
    //#        { "SandyBrown", new V3d(0.956863, 0.643137, 0.376471) },
    //#        { "SeaGreen", new V3d(0.180392, 0.545098, 0.341176) },
    //#        { "SeaShell", new V3d(1.000000, 0.960784, 0.933333) },
    //#        { "Sienna", new V3d(0.627451, 0.321569, 0.176471) },
    //#        { "Silver", new V3d(0.752941, 0.752941, 0.752941) },
    //#        { "SkyBlue", new V3d(0.529412, 0.807843, 0.921569) },
    //#        { "SlateBlue", new V3d(0.415686, 0.352941, 0.803922) },
    //#        { "SlateGray", new V3d(0.439216, 0.501961, 0.564706) },
    //#        { "SlateGrey", new V3d(0.439216, 0.501961, 0.564706) },
    //#        { "Snow", new V3d(1.000000, 0.980392, 0.980392) },
    //#        { "SpringGreen", new V3d(0.000000, 1.000000, 0.498039) },
    //#        { "SteelBlue", new V3d(0.274510, 0.509804, 0.705882) },
    //#        { "Tan", new V3d(0.823529, 0.705882, 0.549020) },
    //#        { "Teal", new V3d(0.000000, 0.501961, 0.501961) },
    //#        { "Thistle", new V3d(0.847059, 0.749020, 0.847059) },
    //#        { "Tomato", new V3d(1.000000, 0.388235, 0.278431) },
    //#        { "Turquoise", new V3d(0.250980, 0.878431, 0.815686) },
    //#        { "Violet", new V3d(0.933333, 0.509804, 0.933333) },
    //#        { "Wheat", new V3d(0.960784, 0.870588, 0.701961) },
    //#        { "White", new V3d(1.000000, 1.000000, 1.000000) },
    //#        { "WhiteSmoke", new V3d(0.960784, 0.960784, 0.960784) },
    //#        { "Yellow", new V3d(1.000000, 1.000000, 0.000000) },
    //#        { "YellowGreen", new V3d(0.603922, 0.803922, 0.196078) },
    //#    };
    //#
    //# Action andand = () => Out(" && ");
    //# Action add = () => Out(" + ");
    //# Action addbetween = () => Out(" + between ");
    //# Action addqcommaspace = () => Out(" + \", \" ");
    //# Action comma = () => Out(", ");
    //# Action oror = () => Out(" || ");
    //# Action semicolon = () => Out("; ");
    //# Action xor = () => Out(" ^ ");
    //#
    //# Func<Meta.SimpleType, Meta.SimpleType, bool> ismapped =
    //#     (t1, t2) => (t1 != t2) && !(t1.IsReal && t2.IsReal);
    //#
    //# Func<Meta.SimpleType, Meta.SimpleType, bool> coltovecsupported =
    //#     (cft, vft) => (!cft.IsReal || vft.IsReal) && (cft != Meta.UIntType || vft != Meta.IntType);
    //#
    //# var ftoftmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "Col.FloatToByteClamped" },
    //#         { Meta.UShortType, "Col.FloatToUShortClamped" },
    //#         { Meta.UIntType, "Col.FloatToUIntClamped" },
    //#         { Meta.FloatType, "" },
    //#         { Meta.DoubleType, "(double)" },
    //#     };
    //# var dtoftmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "Col.DoubleToByteClamped" },
    //#         { Meta.UShortType, "Col.DoubleToUShortClamped" },
    //#         { Meta.UIntType, "Col.DoubleToUIntClamped" },
    //#         { Meta.FloatType, "(float)" },
    //#         { Meta.DoubleType, "" },
    //#     };
    //# var fttodmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "Col.ByteToDouble" },
    //#         { Meta.UShortType, "Col.UShortToDouble" },
    //#         { Meta.UIntType, "Col.UIntToDouble" },
    //#         { Meta.FloatType, "(double)" },
    //#         { Meta.DoubleType, "" },
    //#     };
    //# var btoftmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "" },
    //#         { Meta.UShortType, "Col.ByteToUShort" },
    //#         { Meta.UIntType, "Col.ByteToUInt" },
    //#         { Meta.FloatType, "Col.ByteToFloat" },
    //#         { Meta.DoubleType, "Col.ByteToDouble" },
    //#     };
    //# var fttobmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "" },
    //#         { Meta.UShortType, "Col.UShortToByte" },
    //#         { Meta.UIntType, "Col.UIntToByte" },
    //#         { Meta.FloatType, "Col.FloatToByteClamped" },
    //#         { Meta.DoubleType, "Col.DoubleToByteClamped" },
    //#     };
    //# var maxvalmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "255" },
    //#         { Meta.UShortType, "2^16 - 1" },
    //#         { Meta.UIntType, "2^32 - 1" },
    //#         { Meta.FloatType, "1" },
    //#         { Meta.DoubleType, "1" },
    //#     };
    //# var fdtypes = new[] { Meta.FloatType, Meta.DoubleType };
    //# foreach (var t in Meta.ColorTypes) {
    //#     var type = t.Name;
    //#     var ft = t.FieldType;
    //#     var ht = (ft != Meta.FloatType) ? Meta.HighPrecisionTypeOf(ft) : ft;
    //#     var ct = Meta.ComputationTypeOf(ft);
    //#     var htype = ht.Name;
    //#     var ctype = ct.Name;
    //#     var hnd = ht != Meta.DoubleType; // high not double
    //#     var dblt = Meta.ColorTypeOf(t.Len, Meta.DoubleType);
    //#     var dbltype = dblt.Name;
    //#     var fltt = Meta.ColorTypeOf(t.Len, Meta.FloatType);
    //#     var flttype = fltt.Name;
    //#     var isByte = ft == Meta.ByteType;
    //#     var isUShort = ft == Meta.UShortType;
    //#     var isFloat = ft == Meta.FloatType;
    //#     var isDouble = ft == Meta.DoubleType;
    //#     var isReal = ft.IsReal;
    //#     var ftype = ft.Name;
    //#     var fcaps = ft.Caps;
    //#     var fields = t.Fields;
    //#     var dim = fields.Length;
    //#     var channels = t.Channels;
    //#     var args = fields.ToLower();
    //#     var cargs = channels.ToLower();
    //#     var f_to_ft = ftoftmap[ft];
    //#     var d_to_ft = dtoftmap[ft];
    //#     var ft_to_d = fttodmap[ft];
    //#     var b_to_ft = btoftmap[ft];
    //#     var ft_to_b = fttobmap[ft];
    //#     var fabs_p = isReal ? "Fun.Abs(" : "";
    //#     var q_fabs = isReal ? ")" : "";
    //#     var getptr = "&" + ((ft == Meta.ByteType) ? fields[2] : fields[0]);
    //#     var rgba = t.HasAlpha ? "RGBA" : "RGB";
    //#     var maxval = maxvalmap[ft];
    #region __type__

    /// <summary>
    /// Represents an __rgba__ color with each channel stored as a <see cref="__ftype__"/> value within [0, __maxval__].
    /// </summary>
    [Serializable]
    public partial struct __type__ : IFormattable, IEquatable<__type__>, IRGB/*# if (t.HasAlpha) { */, IOpacity/*# } */
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="__ftype__"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */__ftype__ __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = __a__/*# }, semicolon); */;
        }

        //# if (!isReal) {
        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */int __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = (__ftype__)__a__/*# }, semicolon); */;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */long __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = (__ftype__)__a__/*# }, semicolon); */;
        }

        //# }
        //# if (!isFloat) {
        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        //# if (!isDouble) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */float __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f,a) => {
            __f__ = __f_to_ft__(__a__);
            //# });
        }

        //# }
        //# if (!isDouble) {
        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        //# if (!isFloat) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */double __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f,a) => {
            __f__ = __d_to_ft__(__a__);
            //# });
        }

        //# }
        //# if (t.HasAlpha) {
        /// <summary>
        /// Creates a color from the given <see cref="__ftype__"/> RGB values.
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */__ftype__ __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = __a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# if (!isReal) {
        /// <summary>
        /// Creates a color from the given <see cref="int"/> RGB values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */int __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = (__ftype__)__a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> RGB values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */long __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = (__ftype__)__a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# }
        //# if (!isFloat) {
        /// <summary>
        /// Creates a color from the given <see cref="float"/> RGB values.
        //# if (!isDouble) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */float __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c,a) => { */
            __c__ = __f_to_ft__(__a__)/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# }
        //# if (!isDouble) {
        /// <summary>
        /// Creates a color from the given <see cref="double"/> RGB values.
        //# if (!isFloat) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */double __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c,a) => { */__c__ = __d_to_ft__(__a__)/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# }
        //# } // t.HasAlpha
        /// <summary>
        /// Creates a color from a single <see cref="__ftype__"/> value.
        //# if (t.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ gray)
        {
            /*# channels.ForEach(
                    c => { */__c__ = gray/*# },
                    semicolon); if (t.HasAlpha) {*/; A = __t.MaxValue__/*# } */;
        }

        //# foreach (var ft1 in Meta.RealTypes) { if (ft != ft1) {
        //#     var ftype1 = ft1.Name;
        //#     var convert = (ft1 == Meta.DoubleType) ? d_to_ft : f_to_ft;
        /// <summary>
        /// Creates a color from a single <see cref="__ftype1__"/> value.
        //# if (ismapped(ft, ft1)) {
        /// The value is mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        //# if (t.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype1__ gray)
        {
            var value = __convert__(gray);
            /*# channels.ForEach(
                    c => { */__c__ = value/*# },
                    semicolon); if (t.HasAlpha) {*/; A = __t.MaxValue__/*# } */;
        }

        //# } }
        //# foreach (var t1 in Meta.ColorTypes) {
        //#     var convert = t.FieldType != t1.FieldType
        //#         ? "Col." + t1.FieldType.Caps + "To" + t.FieldType.Caps
        //#         : "";
        /// <summary>
        /// Creates a color from the given <see cref="__t1.Name__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (t.HasAlpha && !t1.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__t1.Name__ color)
        {
            //# channels.ForEach(c => {
            __c__ = __convert__(color.__c__);
            //# });
            //# if (t.HasAlpha) {
            //#     if (t1.HasAlpha) {
            A = __convert__(color.A);
            //#     } else {
            A = __t.MaxValue__;
            //#     }
            //# }
        }

        //#if (t.HasAlpha && !t1.HasAlpha) { // build constructor from Color3 with explicit alpha
        /// <summary>
        /// Creates a color from the given <see cref="__t1.Name__"/> color and an alpha value.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__t1.Name__ color, __ftype__ alpha)
        {
            //# channels.ForEach(Meta.VecFields, (c, vf) => {
            __c__ = __convert__(color.__c__);
            //# });
            A = alpha;
        }

        //# }
        //# } // end For
        //# for (int d = 3; d <= 4; d++) {
        //#     foreach (var vft in Meta.VecFieldTypes) { if (coltovecsupported(ft, vft)) {
        //#         var vt = Meta.VecTypeOf(d, vft);
        //#         var convert = ft != vft
        //#             ? "("+ ft.Name+")"
        //#             : "";
        /// <summary>
        /// Creates a color from the given <see cref="__vt.Name__"/> vector.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (d < dim) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__vt.Name__ vec)
        {
            //# fields.ForEach(Meta.VecFields, (c, vf, i) => {
            __c__ = /*# if (i < d) { */__convert__(vec.__vf__);/*# } else {*/__t.MaxValue__;/*# }*/
            //# });
        }

        //# } } }
        //# if (t.HasAlpha) {
        //# foreach (var vft in Meta.VecFieldTypes) { if (coltovecsupported(ft, vft)) {
        //#     var vt = Meta.VecTypeOf(3, vft);
        //#     var convert = ft != vft
        //#         ? "("+ ft.Name+")"
        //#         : "";
        /// <summary>
        /// Creates a color from the given <see cref="__vt.Name__"/> vector and an alpha value.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__vt.Name__ vec, __ftype__ alpha)
        {
            //# channels.ForEach(Meta.VecFields, (c, vf) => {
            __c__ = __convert__(vec.__vf__);
            //# });
            A = alpha;
        }

        //# } } }
        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(Func<int, __ftype__> index_fun)
        {
            //# fields.ForEach((f, fi) => {
            __f__ = index_fun(__fi__);
            //# });
        }

        //# foreach (var ft1 in Meta.ColorFieldTypes) {
        //#     var ftype1 = ft1.Name;
        //#     var convert = ft != ft1
        //#         ? "Col." + ft1.Caps + "To" + ft.Caps
        //#         : "";
        /// <summary>
        /// Creates a new color from the given <see cref="__ftype1__"/> array.
        //# if (ismapped(ft, ft1)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype1__[] values)
        {
            //# fields.ForEach((f, i) => {
            __f__ = __convert__(values[__i__]);
            //# });
        }

        /// <summary>
        /// Creates a new color from the given <see cref="__ftype1__"/> array, starting at the specified index.
        //# if (ismapped(ft, ft1)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype1__[] values, int start)
        {
            //# fields.ForEach((f, i) => {
            __f__ = __convert__(values[start + __i__]);
            //# });
        }

        //# }
        #endregion

        //# if (t.HasAlpha || isReal) {
        #region Properities

        //# if (t.HasAlpha) {
        //#     var t1 = Meta.ColorTypeOf(3, ft);
        //#     var type1 = t1.Name;
        public __type1__ RGB => (__type1__)this;

        //# }
        //# if (isReal) {
        //# var condArray = new[] { "NaN", "Infinity", "PositiveInfinity", "NegativeInfinity", "Tiny" };
        //# var scopeArray = new[] { ftype, ftype, ftype, ftype, "Fun" };
        //# var quantArray = new[] { "Any", "All" };
        //# var actArray = new[] { oror, andand };
        //# condArray.ForEach(scopeArray, (cond, scope) => {
        //# quantArray.ForEach(actArray, (qant, act) => {
        public bool __qant____cond__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => /*# fields.ForEach((f, i) => { */__scope__.Is__cond__(__f__)/*# }, act); */;
        }

        //# }); // quantArray
        //# }); // condArray
        /// <summary>
        /// Returns true if the absolute value of each component of the color is smaller than Constant&lt;__ftype__&gt;.PositiveTinyValue, false otherwise.
        /// </summary>
        public bool IsTiny
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AllTiny;
        }

        /// <summary>
        /// Returns true if any component of the color is NaN, false otherwise.
        /// </summary>
        public bool IsNaN
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyNaN;
        }

        /// <summary>
        /// Returns true if any component of the color is infinite (positive or negative), false otherwise.
        /// </summary>
        public bool IsInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyInfinity;
        }

        /// <summary>
        /// Returns true if any component of the color is infinite and positive, false otherwise.
        /// </summary>
        public bool IsPositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyPositiveInfinity;
        }

        /// <summary>
        /// Returns true if any component of the color is infinite and negative, false otherwise.
        /// </summary>
        public bool IsNegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyNegativeInfinity;
        }

        /// <summary>
        /// Returns whether all components of the color are finite (i.e. not NaN and not infinity).
        /// </summary>
        public bool IsFinite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !(IsInfinity || IsNaN);
        }

        //# } // isReal
        #endregion

        //# }
        #region Conversions

        //# foreach (var t1 in Meta.ColorTypes) if (t1 != t) {
        //#     var type1 = t1.Name;
        /// <summary>
        /// Converts the given <see cref="__type1__"/> color to a <see cref="__type__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (t.HasAlpha && !t1.HasAlpha) {
        /// The alpha channel is set to __maxvalmap[t.FieldType]__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type1__ color)
            => new __type__(color);

        /// <summary>
        /// Converts the given <see cref="__type__"/> color to a <see cref="__type1__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type1__"/> color range.
        //# }
        //# if (t1.HasAlpha && !t.HasAlpha) {
        /// The alpha channel is set to __maxvalmap[t1.FieldType]__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type1__ To__type1__() => (__type1__)this;

        /// <summary>
        /// Creates a <see cref="__type__"/> color from the given <see cref="__type1__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (t.HasAlpha && !t1.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__type1__(__type1__ c) => new __type__(c);

        //# }
        //# for (int d = 3; d <= 4; d++) {
        //#     foreach (var vft in Meta.VecFieldTypes) { if (coltovecsupported(ft, vft)) {
        //#         var vt = Meta.VecTypeOf(d, vft);
        //#         var vtype = vt.Name;
        //#         var convert = ft != vft ? "("+ vft.Name+")" : "";
        /// <summary>
        /// Converts the given <see cref="__vtype__"/> vector to a <see cref="__type__"/> color.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (d == 3 && t.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__vtype__ v)
            => new __type__(v);

        /// <summary>
        /// Converts the given <see cref="__type__"/> color to a <see cref="__vtype__"/> vector.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped from the <see cref="__type__"/> color range.
        //# }
        //# if (d == 4 && dim == 3) {
        /// W is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__ To__vtype__() => (__vtype__)this;

        /// <summary>
        /// Creates a <see cref="__type__"/> color from a <see cref="__vtype__"/> vector.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (d == 3 && t.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__vtype__(__vtype__ c) => new __type__(c);

        //#     } }
        //# }
        //# foreach (var ft1 in Meta.ColorFieldTypes) {
        //#     var ftype1 = ft1.Name;
        //#     var convert = ft != ft1
        //#         ? "Col." + ft.Caps + "To" + ft1.Caps
        //#         : "";
        /// <summary>
        /// Creates a new color from the given <see cref="__ftype1__"/> array.
        //# if (ismapped(ft, ft1)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__ftype1__[] values)
            => new __type__(values);

        /// <summary>
        /// Creates a new <see cref="__ftype1__"/> array from the given <see cref="__type__"/> color.
        //# if (ismapped(ft, ft1)) {
        /// The values are mapped from the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __ftype1__[](__type__ color)
            => new __ftype1__[] { /*# fields.ForEach(f => {*/__convert__(color.__f__)/*# }, comma); */ };

        //# }
        //# foreach (var t1 in Meta.ColorTypes) {
        //#     if (t.Fields.Length != t1.Fields.Length) continue;
        //#     var type1 = t1.Name;
        //#     var ftype1 = t1.FieldType.Name;
        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type1__ Map(Func<__ftype__, __ftype1__> channel_fun)
        {
            return new __type1__(/*# fields.ForEach(f => { */channel_fun(__f__)/*# }, comma); */);
        }

        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo<T>(T[] array, int start, Func<__ftype__, T> element_fun)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = element_fun(__f__);
            //# });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo<T>(T[] array, int start, Func<__ftype__, int, T> element_index_fun)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = element_index_fun(__f__, __i__);
            //# });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ftype__[] ToArray()
            => (__ftype__[])this;

        #endregion

        #region Indexer

        //# if (ft == Meta.ByteType) {
        // Byte colors have a different byte order (red and blue are swapped)
        private static readonly byte[] IndexMapping = new byte[] { 2, 1, 0, 3 };

        //# }
        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe __ftype__ this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (__ftype__* ptr = __getptr__) { ptr[/*#if (ft == Meta.ByteType) {*/IndexMapping[i]/*# } else {*/i/*#}*/] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (__ftype__* ptr = __getptr__) { return ptr[/*#if (ft == Meta.ByteType) {*/IndexMapping[i]/*# } else {*/i/*#}*/]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// __type__ with all components zero.
        /// </summary>
        public static __type__ Zero => new __type__(/*# fields.ForEach(f => {*/__t.MinValue__/*#}, comma); */);

        // Web colors
        //# foreach(KeyValuePair<string, V3d> entry in ColorNames) {
        //# var name = entry.Key;
        //# var color = new C3d(entry.Value);
        public static __type__ __name__ => new __type__(__d_to_ft__(__color.R__), __d_to_ft__(__color.G__), __d_to_ft__(__color.B__));
        //# }

        public static __type__ DarkYellow => Olive;

        public static __type__ VRVisGreen => new __type__(__d_to_ft__(0.698), __d_to_ft__(0.851), __d_to_ft__(0.008));

        //# for (int i = 1; i < 10; i++) {
        //# var val = (double)(0.1m * i); int percent = 10 * i;
        public static __type__ Gray__percent__ => new __type__(__d_to_ft__(__val__));
        //# }

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ == b.__f__/*# }, andand); */;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ != b.__f__/*# }, oror); */;
        }

        #endregion

        #region Color Arithmetic

        //# fdtypes.ForEach(rt => {
        //# var rtype = rt.Name;
        //# if (!ft.IsReal || ft == rt) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ col, __rtype__ scalar)
        {
            //# if (fdtypes.Contains(ft)) {
            return new __type__(/*# fields.ForEach(f => { */
                col.__f__ * scalar/*# }, comma); */);
            //# } else {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)Fun.Round(col.__f__ * scalar)/*# }, comma); */);
            //# }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__rtype__ scalar, __type__ col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__type__ col, __rtype__ scalar)
        {
            __rtype__ f = 1 / scalar;
            //# if (fdtypes.Contains(ft)) {
            return new __type__(/*# fields.ForEach(f => { */
                col.__f__ * f/*# }, comma); */);
            //# } else {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)Fun.Round(col.__f__ * f)/*# }, comma); */);
            //# }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__rtype__ scalar, __type__ col)
        {
            //# if (fdtypes.Contains(ft)) {
            return new __type__(/*# fields.ForEach(f => { */
                scalar / col.__f__/*# }, comma); */);
            //# } else {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)Fun.Round(scalar / col.__f__)/*# }, comma); */);
            //# }
        }

        //# } });
        //# foreach (var t1 in Meta.ColorTypes) { if (t1.HasAlpha != t.HasAlpha) continue;
        //#
        //#     var type1 = t1.Name; var ft1 = t1.FieldType;
        //#     var ft1_from_ft = t1 != t
        //#         ? (ft.IsReal && ft1.IsReal ? "(" + ftype + ")" : "Col." + ft1.Caps + "To" + ft.Caps)
        //#         : "";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator +(__type__ c0, __type1__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ + __ft1_from_ft__(c1.__f__))/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator -(__type__ c0, __type1__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ - __ft1_from_ft__(c1.__f__))/*# }, comma); */);
        }

        //# } // t1
        //# if (!ft.IsReal) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ col, __ftype__ scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(col.__f__ * scalar)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__ftype__ scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(scalar * col.__f__)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__type__ col, __ftype__ scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(col.__f__ / scalar)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__ftype__ scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(scalar / col.__f__)/*# }, comma); */);
        }

        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ c0, __type__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(c0.__f__ * c1.__f__)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__type__ c0, __type__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(c0.__f__ / c1.__f__)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator +(__type__ col, __ftype__ scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(col.__f__ + scalar)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator +(__ftype__ scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(scalar + col.__f__)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator -(__type__ col, __ftype__ scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(col.__f__ - scalar)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator -(__ftype__ scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(scalar - col.__f__)/*# }, comma); */);
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(__ftype__ min, __ftype__ max)
        {
            //# channels.ForEach(c => {
            __c__ = __c__.Clamp(min, max);
            //# });
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__ Clamped(__ftype__ min, __ftype__ max)
        {
            return new __type__(/*# channels.ForEach(
                c => { */__c__.Clamp(min, max)/*# }, comma);
                if (t.HasAlpha) {*/, A/*# } */);
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __htype__ Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return /*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, add); */; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ctype__ Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(/*# channels.ForEach(c => { */__c__ * __c__/*# }, add); */); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ftype__ NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(/*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, comma); */); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ftype__ NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(/*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, comma); */); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(/*# t.Fields.ForEach(f => { */__f__/*# }, comma); */);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")/*# fields.ForEach(f => {*/
                + __f__.ToString(null, CultureInfo.InvariantCulture) /*# }, addqcommaspace); */
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<__type__, int, __ftype__> Setter =
            (ref __type__ color, int i, __ftype__ value) =>
            {
                switch (i)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: color.__f__ = value; return;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ DivideByInt(__type__ c, int x)
            => c / x;

        #endregion

        #region Parsing

        /// <summary>
        /// Parses a color string with decimal format [R, G, B, A], or hexadecimal formats RRGGBBAA or RGBA.
        /// </summary>
        /// <remarks>
        /// The alpha component in any format is optional/*# if (!t.HasAlpha) {*/ and discarded/*#}*/.
        /// For the single digit hexadecimal RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// Color strings in a hexadecimal format may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="t">The string to be parsed.</param>
        /// <param name="result">Contains the parsed color on success, __type__.Zero otherwise.</param>
        /// <returns>True on success, false otherwise.</returns>
        public static bool TryParse(Text t, out __type__ result)
        {
            //# if (type == "C4b") {
            if (Col.TryParseHex(t, out result))
            {
                return true;
            }
            //# } else {
            if (Col.TryParseHex(t, out C4b tmp))
            {
                result = tmp.To__type__();
                return true;
            }
            //# }
            else
            {
                bool success = true;
                __ftype__[] values = new __ftype__[4] { /*# 4.ForEach(p => { */__t.MaxValue__/*# }, comma);*/ };

                __ftype__ parse(Text t)
                {
                    if (!__ftype__.TryParse(t.ToString(), out __ftype__ value))
                        success = false;

                    return value;
                };

                var count = t.NestedBracketSplitCount2(1);
                if (count == 3 || count == 4)
                    t.NestedBracketSplit(1, parse, () => values);
                else
                    success = false;

                result = success ? new __type__(values) : Zero;
                return success;
            }
        }

        /// <summary>
        /// Parses a color string with decimal format [R, G, B, A], or hexadecimal formats RRGGBBAA or RGBA.
        /// </summary>
        /// <remarks>
        /// The alpha component in any format is optional/*# if (!t.HasAlpha) {*/ and discarded/*#}*/.
        /// For the single digit hexadecimal RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// Color strings in a hexadecimal format may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="s">The string to be parsed.</param>
        /// <param name="result">Contains the parsed color on success, __type__.Zero otherwise.</param>
        /// <returns>True on success, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(string s, out __type__ result)
            => TryParse(new Text(s), out result);

        [Obsolete("Parameter provider is unused.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s, IFormatProvider provider)
            => Parse(s);

        /// <summary>
        /// Parses a color string with decimal format [R, G, B, A], or hexadecimal formats RRGGBBAA or RGBA.
        /// </summary>
        /// <remarks>
        /// The alpha component in any format is optional/*# if (!t.HasAlpha) {*/ and discarded/*#}*/.
        /// For the single digit hexadecimal RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// Color strings in a hexadecimal format may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="s">The string to be parsed.</param>
        /// <returns>The parsed color.</returns>
        /// <exception cref="FormatException">the input does not represent a valid __type__ color.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s)
            => Parse(new Text(s));

        [Obsolete("Weird overload with level, call NestedBracketSplit() manually instead.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(Text t, int bracketLevel = 1)
            => t.NestedBracketSplit(bracketLevel, Text<__ftype__>.Parse, __type__.Setter);

        /// <summary>
        /// Parses a color string with decimal format [R, G, B, A], or hexadecimal formats RRGGBBAA or RGBA.
        /// </summary>
        /// <remarks>
        /// The alpha component in any format is optional/*# if (!t.HasAlpha) {*/ and discarded/*#}*/.
        /// For the single digit hexadecimal RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// Color strings in a hexadecimal format may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="t">The string to be parsed.</param>
        /// <returns>The parsed color.</returns>
        /// <exception cref="FormatException">the input does not represent a valid __type__ color.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(Text t)
            => TryParse(t, out __type__ result) ? result : throw new FormatException($"{t} is not a valid __type__ color.");

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin /*# fields.ForEach(f => {*/+ __f__.ToString(format, fp) /*# }, addbetween); */ + end;
        }

        #endregion

        #region IEquatable<__type__> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
        {
            return /*# fields.ForEach(f => { */__f__.Equals(other.__f__)/*# }, andand); */;
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return __ft_to_d__(R); }
            set { R = __d_to_ft__(value); }
        }

        double IRGB.Green
        {
            get { return __ft_to_d__(G); }
            set { G = __d_to_ft__(value); }
        }

        double IRGB.Blue
        {
            get { return __ft_to_d__(B); }
            set { B = __d_to_ft__(value); }
        }

        #endregion

        //# if (t.HasAlpha) {
        #region IOpacity Members

        [XmlIgnore]
        public double Opacity
        {
            get { return __ft_to_d__(A); }
            set { A = __d_to_ft__(value); }
        }

        #endregion

        //# }
    }

    public static partial class Fun
    {
        #region Interpolation

        //# if (!fdtypes.Contains(ft)) {
        //# fdtypes.ForEach(rt => { var rtype = rt.Name;
        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static __type__ Lerp(this __rtype__ x, __type__ a, __type__ b)
        {
            return new __type__(/*# fields.ForEach(f => {*/Lerp(x, a.__f__, b.__f__)/*#}, comma); */);
        }

        //# });
        //# } else {
        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static __type__ Lerp(this __ftype__ x, __type__ a, __type__ b)
        {
            return new __type__(/*# fields.ForEach(f => {*/Lerp(x, a.__f__, b.__f__)/*#}, comma); */);
        }
        //# }
        #endregion

        #region ApproximateEquals

        //# if (isReal) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b)
        {
            return ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
        }

        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, __ftype__ tolerance)
        {
            return /*# fields.ForEach(f => {*/ApproximateEquals(a.__f__, b.__f__, tolerance)/*# }, andand);*/;
        }

        #endregion

        #region IsTiny

        /// <summary>
        /// Returns whether the absolute value of each component of the given <see cref="__type__"/> is smaller than <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTiny(this __type__ c, __ftype__ epsilon)
            => Col.AllTiny(c, epsilon);

        //# if (ft.IsReal) {
        /// <summary>
        /// Returns whether the absolute value of each component of the given <see cref="__type__"/> is smaller than Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTiny(__type__ c)
            => c.IsTiny;

        //# }
        #endregion
        //# if (isReal) {

        #region Special Floating Point Value Checks

        /// <summary>
        /// Returns whether any component of the given <see cref="__type__"/> is NaN.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(__type__ c)
            => c.IsNaN;

        /// <summary>
        /// Returns whether any component of the the given <see cref="__type__"/> is infinity (positive or negative).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(__type__ c)
            => c.IsInfinity;

        /// <summary>
        /// Returns whether any component of the the given <see cref="__type__"/> is positive infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(__type__ c)
            => c.IsPositiveInfinity;

        /// <summary>
        /// Returns whether any component of the the given <see cref="__type__"/> is negative infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(__type__ c)
            => c.IsNegativeInfinity;

        /// <summary>
        /// Returns whether all components of the the given <see cref="__type__"/> are finite (i.e. not NaN and not infinity).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(__type__ c)
            => c.IsFinite;

        #endregion
        //# }
    }

    public static partial class Col
    {
        #region ToHexString

        /// <summary>
        /// Returns the hexadecimal representation with format RRGGBB/*# if (t.HasAlpha) {*/AA/*#}*/.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHexString(this __type__ c)
            //# if (ft == Meta.ByteType) {
            => /*# if (t.HasAlpha) {*/$"{c.R:X2}{c.G:X2}{c.B:X2}{c.A:X2}"/*# } else {*/$"{c.R:X2}{c.G:X2}{c.B:X2}"/*#}*/;
            //# } else {
            => c.ToC__t.Len__b().ToHexString();
            //# }

        #endregion

        #region Comparisons

        //# var bops = new[,] { { "<",  "Smaller"        }, { ">" , "Greater"},
        //#                     { "<=", "SmallerOrEqual" }, { ">=", "GreaterOrEqual"},
        //#                     { "==", "Equal" },          { "!=", "Different" } };
        //# var attention = "ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).";
        //# for(int o = 0; o < bops.GetLength(0); o++) {
        //#     string bop = " " + bops[o,0] + " ", opName = bops[o,1];
        /// <summary>
        /// Returns whether ALL elements of a are __opName__ the corresponding element of b.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(this __type__ a, __type__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of col are __opName__ s.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(this __type__ col, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */col.__f____bop__s/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ ALL elements of col.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(__ftype__ s, __type__ col)
        {
            return (/*# fields.ForEach(f => { */s__bop__col.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is __opName__ the corresponding element of b.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(this __type__ a, __type__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is __opName__ s.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(this __type__ col, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */col.__f____bop__s/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ AT LEAST ONE element of col.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(__ftype__ s, __type__ col)
        {
            return (/*# fields.ForEach(f => { */s__bop__col.__f__/*# }, oror); */);
        }
        //# }

        #endregion

        #region Linear Combination

        //# for (int tpc = 4; tpc < 7; tpc+=2) {
        //# foreach (var rt in new[] { fltt, dblt }) { var rtype = rt.Name; var wtype = rt.FieldType.Name; var rtc = rt.FieldType.Caps[0];
        //#     var convert = ft.IsReal ? ""
        //#        : "Col." + ft.Caps + "In"
        //#          + (ft.Name == "uint" ? "Double" : rt.FieldType.Caps)
        //#          + "To" + ft.Caps + "Clamped";
        //# if (!isReal || wtype == ftype) {
        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ LinCom(
            /*# tpc.ForEach(i => { */__type__ p__i__/*# }, comma); */, ref Tup__tpc__<__wtype__> w)
        {
            return new __type__(/*# channels.ForEach(ch => { */
                __convert__(/*# tpc.ForEach(i => { */p__i__.__ch__ * w.E__i__/*# }, add); */)/*# }, comma); */);
        }

        //# }
        //# if (!isReal) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ LinComRaw__rtc__(
            /*# tpc.ForEach(i => { */__type__ p__i__/*# }, comma); */, ref Tup__tpc__<__wtype__> w)
        {
            return new __rtype__(/*# channels.ForEach(ch => { */
                /*# tpc.ForEach(i => { */p__i__.__ch__ * w.E__i__/*# }, add); }, comma); */);
        }

        //# } // !isReal
        //# } // rt
        //# } // tpc
        #endregion

        #region AnyTiny, AllTiny

        /// <summary>
        /// Returns whether the absolute value of any component of the given <see cref="__type__"/> is smaller than <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyTiny(this __type__ c, __ftype__ epsilon)
            => /*# fields.ForEach(f => { */c.__f__.IsTiny(epsilon)/*# }, oror);*/;

        /// <summary>
        /// Returns whether the absolute value of each component of the given <see cref="__type__"/> is smaller than <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllTiny(this __type__ c, __ftype__ epsilon)
            => /*# fields.ForEach(f => { */c.__f__.IsTiny(epsilon)/*# }, andand);*/;

        #endregion
        //# if (isReal) {

        #region Special Floating Point Value Checks

        //# var condArray = new[] { "NaN", "Infinity", "PositiveInfinity", "NegativeInfinity", "Tiny" };
        //# var scopeArray = new[] { ftype, ftype, ftype, ftype, "Fun" };
        //# var quantArray = new[] { "Any", "All" };
        //# var actArray = new[] { oror, andand };
        //# condArray.ForEach(scopeArray, (cond, scope) => {
        //# quantArray.ForEach(actArray, (qant, act) => {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool __qant____cond__(__type__ c)
            => c.__qant____cond__;

        //# }); // quantArray
        //# }); // condArray
        #endregion
        //# }
    }

    //# if (ft != Meta.ByteType && ft != Meta.UShortType) {
    public static class IRandomUniform__type__Extensions
    {
        #region IRandomUniform extensions for __type__

        //# string[] variants;
        //# if (ft == Meta.FloatType) {
        //#     variants = new string[] { "", "Closed", "Open" };
        //# } else if (ft == Meta.DoubleType) {
        //#     variants = new string[] { "", "Closed", "Open", "Full", "FullClosed", "FullOpen" };
        //# } else {
        //#     variants = new string[] { "" };
        //# }
        //# foreach (var v in variants) {
        /// <summary>
        /// Uses Uniform__fcaps____v__() to generate the elements of a __type__ color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Uniform__type____v__(this IRandomUniform rnd)
        {
            return new __type__(/*# fields.ForEach(f => { */rnd.Uniform__fcaps____v__()/*#  }, comma); */);
        }

        //# }
        #endregion
    }

    //# }
    #endregion

    //# }
}
