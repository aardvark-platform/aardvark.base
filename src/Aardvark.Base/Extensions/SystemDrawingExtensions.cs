namespace Aardvark.Base
{
    public static class SystemDrawingExtensions
    {
        #region System.Drawing.Point

        /// <summary>
        /// Casts the System.Drawing.Point to an Aardvark integer vector.
        /// </summary>
        public static V2i ToV2i(this System.Drawing.Point p)
        {
            return new V2i(p.X, p.Y);
        }

        public static System.Drawing.Point ToPoint(this V2i p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        #endregion

        #region System.Drawing.Size

        /// <summary>
        /// Casts the System.Drawing.Size to an Aardvark integer vector.
        /// </summary>
        public static V2i ToV2i(this System.Drawing.Size s)
        {
            return new V2i(s.Width, s.Height);
        }

        public static System.Drawing.Size ToSize(this V2i p)
        {
            return new System.Drawing.Size(p.X, p.Y);
        }

        #endregion

        #region System.Drawing.Rectangle

        /// <summary>
        /// Casts the System.Drawing.Rectangle to an Aardvark box.
        /// </summary>
        public static Box2i ToBox2i(this System.Drawing.Rectangle s)
        {
            return new Box2i(s.Location.ToV2i(), s.Size.ToV2i());
        }

        public static System.Drawing.Rectangle ToRectangle(this Box2i p)
        {
            return new System.Drawing.Rectangle(p.Min.ToPoint(), p.Size.ToSize());
        }

        #endregion

        #region System.Drawing.Color

        public static System.Drawing.Color ToColor(this C3b color)
        {
            return System.Drawing.Color.FromArgb(
                            color.R, color.G, color.B);
        }

        public static C3b ToC3b(this System.Drawing.Color color)
        {
            return new C3b(color.R, color.G, color.B);
        }

        public static System.Drawing.Color ToColor(this C3us color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromUShort(color.R),
                            Col.ByteFromUShort(color.G),
                            Col.ByteFromUShort(color.B));
        }

        public static C3us ToC3us(this System.Drawing.Color color)
        {
            return new C3us(Col.UShortFromByte(color.R),
                            Col.UShortFromByte(color.G),
                            Col.UShortFromByte(color.B));
        }

        public static System.Drawing.Color ToColor(this C3ui color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromUInt(color.R),
                            Col.ByteFromUInt(color.G),
                            Col.ByteFromUInt(color.B));
        }

        public static C3ui ToC3ui(this System.Drawing.Color color)
        {
            return new C3ui(Col.UIntFromByte(color.R),
                            Col.UIntFromByte(color.G),
                            Col.UIntFromByte(color.B));
        }

        public static System.Drawing.Color ToColor(this C3f color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromFloatClamped(color.R),
                            Col.ByteFromFloatClamped(color.G),
                            Col.ByteFromFloatClamped(color.B));
        }

        public static C3f ToC3f(this System.Drawing.Color color)
        {
            return new C3f(Col.FloatFromByte(color.R),
                            Col.FloatFromByte(color.G),
                            Col.FloatFromByte(color.B));
        }

        public static System.Drawing.Color ToColor(this C3d color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromDoubleClamped(color.R),
                            Col.ByteFromDoubleClamped(color.G),
                            Col.ByteFromDoubleClamped(color.B));
        }

        public static C3d ToC3d(this System.Drawing.Color color)
        {
            return new C3d(Col.DoubleFromByte(color.R),
                            Col.DoubleFromByte(color.G),
                            Col.DoubleFromByte(color.B));
        }

        public static System.Drawing.Color ToColor(this C4b color)
        {
            return System.Drawing.Color.FromArgb(
                            color.A, color.R, color.G, color.B);
        }

        public static C4b ToC4b(this System.Drawing.Color color)
        {
            return new C4b(color.R, color.G, color.B, color.A);
        }

        public static System.Drawing.Color ToColor(this C4us color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromUShort(color.A),
                            Col.ByteFromUShort(color.R),
                            Col.ByteFromUShort(color.G),
                            Col.ByteFromUShort(color.B));
        }

        public static C4us ToC4us(this System.Drawing.Color color)
        {
            return new C4us(Col.UShortFromByte(color.R),
                            Col.UShortFromByte(color.G),
                            Col.UShortFromByte(color.B),
                            Col.UShortFromByte(color.A));
        }

        public static System.Drawing.Color ToColor(this C4ui color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromUInt(color.A),
                            Col.ByteFromUInt(color.R),
                            Col.ByteFromUInt(color.G),
                            Col.ByteFromUInt(color.B));
        }

        public static C4ui ToC4ui(this System.Drawing.Color color)
        {
            return new C4ui(Col.UIntFromByte(color.R),
                            Col.UIntFromByte(color.G),
                            Col.UIntFromByte(color.B),
                            Col.UIntFromByte(color.A));
        }

        public static System.Drawing.Color ToColor(this C4f color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromFloatClamped(color.A),
                            Col.ByteFromFloatClamped(color.R),
                            Col.ByteFromFloatClamped(color.G),
                            Col.ByteFromFloatClamped(color.B));
        }

        public static C4f ToC4f(this System.Drawing.Color color)
        {
            return new C4f(Col.FloatFromByte(color.R),
                            Col.FloatFromByte(color.G),
                            Col.FloatFromByte(color.B),
                            Col.FloatFromByte(color.A));
        }

        public static System.Drawing.Color ToColor(this C4d color)
        {
            return System.Drawing.Color.FromArgb(
                            Col.ByteFromDoubleClamped(color.A),
                            Col.ByteFromDoubleClamped(color.R),
                            Col.ByteFromDoubleClamped(color.G),
                            Col.ByteFromDoubleClamped(color.B));
        }

        public static C4d ToC4d(this System.Drawing.Color color)
        {
            return new C4d(Col.DoubleFromByte(color.R),
                            Col.DoubleFromByte(color.G),
                            Col.DoubleFromByte(color.B),
                            Col.DoubleFromByte(color.A));
        }

        #endregion
    }
}
