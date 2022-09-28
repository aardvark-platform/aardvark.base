using System.Drawing;

namespace Aardvark.Base.SystemDrawingInterop
{
    /// <summary>
    /// Extensions for converting between System.Drawing and Aardvark data types.
    /// </summary>
    public static class SystemDrawingExtensions
    {
        #region System.Drawing.Point

        /// <summary>
        /// System.Drawing.Point to V2i.
        /// </summary>
        public static V2i ToV2i(this Point p) => new V2i(p.X, p.Y);

        /// <summary>
        /// V2i to System.Drawing.Point.
        /// </summary>
        public static Point ToPoint(this V2i p) => new Point(p.X, p.Y);

        #endregion

        #region System.Drawing.Size

        /// <summary>
        /// System.Drawing.Size to V2i.
        /// </summary>
        public static V2i ToV2i(this Size s) => new V2i(s.Width, s.Height);

        /// <summary>
        /// V2i to System.Drawing.Size.
        /// </summary>
        public static Size ToSize(this V2i v) => new Size(v.X, v.Y);

        #endregion

        #region System.Drawing.Rectangle

        /// <summary>
        /// System.Drawing.Rectangle to Box2i.
        /// </summary>
        public static Box2i ToBox2i(this Rectangle r)
            => new Box2i(r.Left, r.Top, r.Right, r.Bottom);

        /// <summary>
        /// Box2i to System.Drawing.Rectangle.
        /// </summary>
        public static Rectangle ToRectangle(this Box2i p)
            => new Rectangle(p.Min.X, p.Min.Y, p.SizeX, p.SizeY);

        #endregion

        #region System.Drawing.Color

        /// <summary>
        /// C3b to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C3b color)
            => Color.FromArgb(color.R, color.G, color.B);

        /// <summary>
        /// System.Drawing.Color to C3b.
        /// </summary>
        public static C3b ToC3b(this Color color)
            => new C3b(color.R, color.G, color.B);

        /// <summary>
        /// C3us to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C3us color) => Color.FromArgb(
            Col.UShortToByte(color.R),
            Col.UShortToByte(color.G),
            Col.UShortToByte(color.B));

        /// <summary>
        /// System.Drawing.Color to C3us.
        /// </summary>
        public static C3us ToC3us(this Color color) => new C3us(
            Col.ByteToUShort(color.R),
            Col.ByteToUShort(color.G),
            Col.ByteToUShort(color.B));

        /// <summary>
        /// C3ui to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C3ui color) => Color.FromArgb(
            Col.UIntToByte(color.R),
            Col.UIntToByte(color.G),
            Col.UIntToByte(color.B));

        /// <summary>
        /// System.Drawing.Color to C3ui.
        /// </summary>
        public static C3ui ToC3ui(this Color color) => new C3ui(
            Col.ByteToUInt(color.R),
            Col.ByteToUInt(color.G),
            Col.ByteToUInt(color.B));

        /// <summary>
        /// C3f to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C3f color) => Color.FromArgb(
            Col.FloatToByteClamped(color.R),
            Col.FloatToByteClamped(color.G),
            Col.FloatToByteClamped(color.B));

        /// <summary>
        /// System.Drawing.Color to C3f.
        /// </summary>
        public static C3f ToC3f(this Color color) => new C3f(
            Col.ByteToFloat(color.R),
            Col.ByteToFloat(color.G),
            Col.ByteToFloat(color.B));

        /// <summary>
        /// C3d to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C3d color) => Color.FromArgb(
            Col.DoubleToByteClamped(color.R),
            Col.DoubleToByteClamped(color.G),
            Col.DoubleToByteClamped(color.B));

        /// <summary>
        /// System.Drawing.Color to C3d.
        /// </summary>
        public static C3d ToC3d(this Color color) => new C3d(
            Col.ByteToDouble(color.R),
            Col.ByteToDouble(color.G),
            Col.ByteToDouble(color.B));

        /// <summary>
        /// C4b to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C4b color)
            => Color.FromArgb(color.A, color.R, color.G, color.B);

        /// <summary>
        /// System.Drawing.Color to C4b.
        /// </summary>
        public static C4b ToC4b(this Color color)
            => new C4b(color.R, color.G, color.B, color.A);

        /// <summary>
        /// C4us to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C4us color) => Color.FromArgb(
            Col.UShortToByte(color.A),
            Col.UShortToByte(color.R),
            Col.UShortToByte(color.G),
            Col.UShortToByte(color.B));

        /// <summary>
        /// System.Drawing.Color to C4us.
        /// </summary>
        public static C4us ToC4us(this Color color) => new C4us(
            Col.ByteToUShort(color.R),
            Col.ByteToUShort(color.G),
            Col.ByteToUShort(color.B),
            Col.ByteToUShort(color.A));

        /// <summary>
        /// C4ui to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C4ui color) => Color.FromArgb(
            Col.UIntToByte(color.A),
            Col.UIntToByte(color.R),
            Col.UIntToByte(color.G),
            Col.UIntToByte(color.B));

        /// <summary>
        /// System.Drawing.Color to C4ui.
        /// </summary>
        public static C4ui ToC4ui(this Color color) => new C4ui(
            Col.ByteToUInt(color.R),
            Col.ByteToUInt(color.G),
            Col.ByteToUInt(color.B),
            Col.ByteToUInt(color.A));

        /// <summary>
        /// C4f to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C4f color) => Color.FromArgb(
            Col.FloatToByteClamped(color.A),
            Col.FloatToByteClamped(color.R),
            Col.FloatToByteClamped(color.G),
            Col.FloatToByteClamped(color.B));

        /// <summary>
        /// System.Drawing.Color to C4f.
        /// </summary>
        public static C4f ToC4f(this Color color) => new C4f(
            Col.ByteToFloat(color.R),
            Col.ByteToFloat(color.G),
            Col.ByteToFloat(color.B),
            Col.ByteToFloat(color.A));

        /// <summary>
        /// C4d to System.Drawing.Color.
        /// </summary>
        public static Color ToColor(this C4d color) => Color.FromArgb(
            Col.DoubleToByteClamped(color.A),
            Col.DoubleToByteClamped(color.R),
            Col.DoubleToByteClamped(color.G),
            Col.DoubleToByteClamped(color.B));

        /// <summary>
        /// System.Drawing.Color to C4d.
        /// </summary>
        public static C4d ToC4d(this Color color) => new C4d(
            Col.ByteToDouble(color.R),
            Col.ByteToDouble(color.G),
            Col.ByteToDouble(color.B),
            Col.ByteToDouble(color.A));

        #endregion
    }
}
