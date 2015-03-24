using System;

namespace Aardvark.Base
{
    /// <summary>
    /// Specifies an image boder, all entries should be positive values.
    /// </summary>
    public struct Border2i
    {
        public V2i Min;
        public V2i Max;

        #region Constructors

        public Border2i(V2i min, V2i max)
        {
            Min = min; Max = max;
        }

        public Border2i(int minx, int miny, int maxx, int maxy)
            : this(new V2i(minx, miny), new V2i(maxx, maxy))
        { }

        public Border2i(int val)
            : this(new V2i(val, val), new V2i(val, val))
        { }

        public Border2i Flipped { get { return new Border2i(Max, Min); } }

        #endregion
    }

    /// <summary>
    /// Specifies an image boder, all entries should be positive values.
    /// </summary>
    public struct Border2l
    {
        public V2l Min;
        public V2l Max;

        #region Constructors

        public Border2l(V2l min, V2l max)
        {
            Min = min; Max = max;
        }

        public Border2l(long minx, long miny, long maxx, long maxy)
            : this(new V2l(minx, miny), new V2l(maxx, maxy))
        { }

        public Border2l(long val)
            : this(new V2l(val, val), new V2l(val, val))
        { }

        public Border2l Flipped { get { return new Border2l(Max, Min); } }

        #endregion
    }

    public static class BorderMatrixAndVolumeExtensions
    {
        #region Matrix Parts

        /// <summary>
        /// Get the minimal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMinX<T>(this Matrix<T> m, long b)
        {
            return m.SubMatrixWindow(m.FX, m.FY, b, m.SY);
        }

        /// <summary>
        /// Get the minimal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMinX<Td, Tv>(this Matrix<Td, Tv> m, long b)
        {
            return m.SubMatrixWindow(m.FX, m.FY, b, m.SY);
        }

        /// <summary>
        /// Get the maximal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMaxX<T>(this Matrix<T> m, long b)
        {
            return m.SubMatrixWindow(m.EX - b, m.FY, b, m.SY);
        }

        /// <summary>
        /// Get the maximal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMaxX<Td, Tv>(this Matrix<Td, Tv> m, long b)
        {
            return m.SubMatrixWindow(m.EX - b, m.FY, b, m.SY);
        }

        /// <summary>
        /// Get the minimal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        /// <summary>
        /// Get the minimal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMinY<T>(this Matrix<T> m, long b)
        {
            return m.SubMatrixWindow(m.FX, m.FY, m.SX, b);
        }

        /// <summary>
        /// Get the minimal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMinY<Td, Tv>(this Matrix<Td, Tv> m, long b)
        {
            return m.SubMatrixWindow(m.FX, m.FY, m.SX, b);
        }

        /// <summary>
        /// Get the maximal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMaxY<T>(this Matrix<T> m, long b)
        {
            return m.SubMatrixWindow(m.FX, m.EY - b, m.SX, b);
        }

        /// <summary>
        /// Get the maximal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMaxY<Td, Tv>(this Matrix<Td, Tv> m, long b)
        {
            return m.SubMatrixWindow(m.FX, m.EY - b, m.SX, b);
        }

        /// <summary>
        /// Get the center part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubCenter<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX + b.Min.X, m.FY + b.Min.Y,
                                     m.SX - b.Max.X - b.Min.X, m.SY - b.Max.Y - b.Min.Y);
        }

        /// <summary>
        /// Get the center part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubCenter<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX + b.Min.X, m.FY + b.Min.Y,
                                     m.SX - b.Max.X - b.Min.X, m.SY - b.Max.Y - b.Min.Y);
        }

        /// <summary>
        /// Get the minimal x/minmal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMinXMinY<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX, m.FY,
                                     b.Min.X, b.Min.Y);
        }

        /// <summary>
        /// Get the minimal x/minmal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMinXMinY<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX, m.FY,
                                     b.Min.X, b.Min.Y);
        }

        /// <summary>
        /// Get the minimal x/maximal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMinXMaxY<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX, m.EY - b.Max.Y,
                                     b.Min.X, b.Max.Y);
        }

        /// <summary>
        /// Get the minimal x/maximal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMinXMaxY<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX, m.EY - b.Max.Y,
                                     b.Min.X, b.Max.Y);
        }

        /// <summary>
        /// Get the maximal x/minimal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMaxXMinY<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.EX - b.Max.X, m.FY,
                                     b.Max.X, b.Min.Y);
        }

        /// <summary>
        /// Get the maximal x/minimal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMaxXMinY<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.EX - b.Max.X, m.FY,
                                     b.Max.X, b.Min.Y);
        }

        /// <summary>
        /// Get the maximal x/maximal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMaxXMaxY<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.EX - b.Max.X, m.EY - b.Max.Y,
                                     b.Max.X, b.Max.Y);
        }

        /// <summary>
        /// Get the maximal x/maximal y corner part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMaxXMaxY<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.EX - b.Max.X, m.EY - b.Max.Y,
                                     b.Max.X, b.Max.Y);
        }

        /// <summary>
        /// Get the minimal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMinX<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX, m.FY + b.Min.Y,
                                     b.Min.X, m.SY - b.Min.Y - b.Max.Y);
        }

        /// <summary>
        /// Get the minimal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMinX<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX, m.FY + b.Min.Y,
                                     b.Min.X, m.SY - b.Min.Y - b.Max.Y);
        }

        /// <summary>
        /// Get the maximal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMaxX<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.EX - b.Max.X, m.FY + b.Min.Y,
                                     b.Max.X, m.SY - b.Min.Y - b.Max.Y);
        }

        /// <summary>
        /// Get the maximal x edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMaxX<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.EX - b.Max.X, m.FY + b.Min.Y,
                                     b.Max.X, m.SY - b.Min.Y - b.Max.Y);
        }

        public static Matrix<T> SubMinY<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX + b.Min.X, m.FY,
                                     m.SX - b.Min.X - b.Max.X, b.Min.Y);
        }

        /// <summary>
        /// Get the minimal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMinY<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX + b.Min.X, m.FY,
                                     m.SX - b.Min.X - b.Max.X, b.Min.Y);
        }

        /// <summary>
        /// Get the maximal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> SubMaxY<T>(this Matrix<T> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX + b.Min.X, m.EY - b.Max.Y,
                                     m.SX - b.Min.X - b.Max.X, b.Max.Y);
        }

        /// <summary>
        /// Get the maximal y edge part of a matrix with the specified border.
        /// Note that the part retains the pixel coordinates of the original matrix.
        /// </summary>
        public static Matrix<Td, Tv> SubMaxY<Td, Tv>(this Matrix<Td, Tv> m, Border2l b)
        {
            return m.SubMatrixWindow(m.FX + b.Min.X, m.EY - b.Max.Y,
                                     m.SX - b.Min.X - b.Max.X, b.Max.Y);
        }

        #endregion

        #region Volume Parts

        /// <summary>
        /// Get the minimal x edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMinX<T>(this Volume<T> v, long b)
        {
            return v.SubVolumeWindow(v.FX, v.FY, v.FZ, b, v.SY, v.SZ);
        }

        /// <summary>
        /// Get the maximal x edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMaxX<T>(this Volume<T> v, long b)
        {
            return v.SubVolumeWindow(v.EX - b, v.FY, v.FZ, b, v.SY, v.SZ);
        }

        /// <summary>
        /// Get the minimal y edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMinY<T>(this Volume<T> v, long b)
        {
            return v.SubVolumeWindow(v.FX, v.FY, v.FZ, v.SX, b, v.SZ);
        }

        /// <summary>
        /// Get the maximal y edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMaxY<T>(this Volume<T> v, long b)
        {
            return v.SubVolumeWindow(v.FX, v.EY - b, v.FZ, v.SX, b, v.SZ);
        }

        /// <summary>
        /// Get the center part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubCenter<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.FX + b.Min.X, v.FY + b.Min.Y, v.FZ,
                                     v.SX - b.Max.X - b.Min.X, v.SY - b.Max.Y - b.Min.Y, v.SZ);
        }

        /// <summary>
        /// Get the minimal x/minmal y corner part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMinXMinY<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.FX, v.FY, v.FZ,
                                     b.Min.X, b.Min.Y, v.SZ);
        }

        /// <summary>
        /// Get the minimal x/maximal y corner part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMinXMaxY<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.FX, v.EY - b.Max.Y, v.FZ,
                                     b.Min.X, b.Max.Y, v.SZ);
        }

        /// <summary>
        /// Get the maximal x/minimal y corner part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMaxXMinY<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.EX - b.Max.X, v.FY, v.FZ,
                                     b.Max.X, b.Min.Y, v.SZ);
        }

        /// <summary>
        /// Get the maximal x/maximal y corner part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMaxXMaxY<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.EX - b.Max.X, v.EY - b.Max.Y, v.FZ,
                                     b.Max.X, b.Max.Y, v.SZ);
        }

        /// <summary>
        /// Get the minimal x edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMinX<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.FX, v.FY + b.Min.Y, v.FZ,
                                     b.Min.X, v.SY - b.Min.Y - b.Max.Y, v.SZ);
        }

        /// <summary>
        /// Get the maximal x edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMaxX<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.EX - b.Max.X, v.FY + b.Min.Y, v.FZ,
                                     b.Max.X, v.SY - b.Min.Y - b.Max.Y, v.SZ);
        }

        /// <summary>
        /// Get the minimal y edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMinY<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.FX + b.Min.X, v.FY, v.FZ,
                                     v.SX - b.Min.X - b.Max.X, b.Min.Y, v.SZ);
        }

        /// <summary>
        /// Get the maximal y edge part of an image volume with the specified border.
        /// Note that the part retains the pixel coordinates of the original volume.
        /// </summary>
        public static Volume<T> SubMaxY<T>(this Volume<T> v, Border2l b)
        {
            return v.SubVolumeWindow(v.FX + b.Min.X, v.EY - b.Max.Y, v.FZ,
                                     v.SX - b.Min.X - b.Max.X, b.Max.Y, v.SZ);
        }

        #endregion

        #region Generic Matrix Processing

        /// <summary>
        /// Process an image volume with specific actions for the center and border
        /// parts.
        /// </summary>
        public static Matrix<T1> ProcessCenterBordersAndCorners<T, T1>(
                this Matrix<T> source, Border2l border,
                Action<Matrix<T>, Matrix<T1>> actCenter,
                Action<Matrix<T>, Matrix<T1>> actMinX, Action<Matrix<T>, Matrix<T1>> actMaxX,
                Action<Matrix<T>, Matrix<T1>> actMinY, Action<Matrix<T>, Matrix<T1>> actMaxY,
                Action<Matrix<T>, Matrix<T1>> actMinXMinY, Action<Matrix<T>, Matrix<T1>> actMaxXMinY,
                Action<Matrix<T>, Matrix<T1>> actMinXMaxY, Action<Matrix<T>, Matrix<T1>> actMaxXMaxY)
        {
            var target = new Matrix<T1>(source.Size);
            target.F = source.F;

            actCenter(source.SubCenter(border), target.SubCenter(border));
            actMinX(source.SubMinX(border), target.SubMinX(border));
            actMaxX(source.SubMaxX(border), target.SubMaxX(border));
            actMinY(source.SubMinY(border), target.SubMinY(border));
            actMaxY(source.SubMaxY(border), target.SubMaxY(border));
            actMinXMinY(source.SubMinXMinY(border), target.SubMinXMinY(border));
            actMaxXMinY(source.SubMaxXMinY(border), target.SubMaxXMinY(border));
            actMinXMaxY(source.SubMinXMaxY(border), target.SubMinXMaxY(border));
            actMaxXMaxY(source.SubMaxXMaxY(border), target.SubMaxXMaxY(border));

            return target;
        }

        /// <summary>
        /// Process an image volume with specific actions for the center and border
        /// parts.
        /// </summary>
        public static void ApplyCenterBordersAndCorners<T>(
                this Matrix<T> source, Border2l border,
                Action<Matrix<T>> actCenter,
                Action<Matrix<T>> actMinX, Action<Matrix<T>> actMaxX,
                Action<Matrix<T>> actMinY, Action<Matrix<T>> actMaxY,
                Action<Matrix<T>> actMinXMinY, Action<Matrix<T>> actMaxXMinY,
                Action<Matrix<T>> actMinXMaxY, Action<Matrix<T>> actMaxXMaxY)
        {

            actCenter(source.SubCenter(border));
            actMinX(source.SubMinX(border)); actMaxX(source.SubMaxX(border));
            actMinY(source.SubMinY(border)); actMaxY(source.SubMaxY(border));
            actMinXMinY(source.SubMinXMinY(border)); actMaxXMinY(source.SubMaxXMinY(border));
            actMinXMaxY(source.SubMinXMaxY(border)); actMaxXMaxY(source.SubMaxXMaxY(border));
        }

        /// <summary>
        /// Process an image volume with specific actions for the center and border
        /// parts.
        /// </summary>
        public static void ApplyCenterBordersAndCorners<Td, Tv>(
                this Matrix<Td, Tv> source, Border2l border,
                Action<Matrix<Td, Tv>> actCenter,
                Action<Matrix<Td, Tv>> actMinX, Action<Matrix<Td, Tv>> actMaxX,
                Action<Matrix<Td, Tv>> actMinY, Action<Matrix<Td, Tv>> actMaxY,
                Action<Matrix<Td, Tv>> actMinXMinY, Action<Matrix<Td, Tv>> actMaxXMinY,
                Action<Matrix<Td, Tv>> actMinXMaxY, Action<Matrix<Td, Tv>> actMaxXMaxY)
        {

            actCenter(source.SubCenter(border));
            actMinX(source.SubMinX(border)); actMaxX(source.SubMaxX(border));
            actMinY(source.SubMinY(border)); actMaxY(source.SubMaxY(border));
            actMinXMinY(source.SubMinXMinY(border)); actMaxXMinY(source.SubMaxXMinY(border));
            actMinXMaxY(source.SubMinXMaxY(border)); actMaxXMaxY(source.SubMaxXMaxY(border));
        }

        #endregion

        #region Generic Volume Processing

        /// <summary>
        /// Process an image volume with specific actions for the center and
        /// border parts.
        /// </summary>
        public static Volume<T1> WithProcessedCenterBordersAndCorners<T, T1>(
                this Volume<T> source, Border2l border,
                Func<Volume<T>, Volume<T1>> funCenter,
                Func<Volume<T>, Volume<T1>> funMinX, Func<Volume<T>, Volume<T1>> funMaxX,
                Func<Volume<T>, Volume<T1>> funMinY, Func<Volume<T>, Volume<T1>> funMaxY,
                Func<Volume<T>, Volume<T1>> funMinXMinY, Func<Volume<T>, Volume<T1>> funMaxXMinY,
                Func<Volume<T>, Volume<T1>> funMinXMaxY, Func<Volume<T>, Volume<T1>> funMaxXMaxY)
        {
            var target = source.Size.CreateImageVolume<T1>();
            target.F = source.F;
            target.SubCenter(border).Set(funCenter(source.SubCenter(border)));
            target.SubMinX(border).Set(funMinX(source.SubMinX(border)));
            target.SubMaxX(border).Set(funMaxX(source.SubMaxX(border)));
            target.SubMinY(border).Set(funMinY(source.SubMinY(border)));
            target.SubMaxY(border).Set(funMaxY(source.SubMaxY(border)));
            target.SubMinXMinY(border).Set(funMinXMinY(source.SubMinXMinY(border)));
            target.SubMaxXMinY(border).Set(funMaxXMinY(source.SubMaxXMinY(border)));
            target.SubMinXMaxY(border).Set(funMinXMaxY(source.SubMinXMaxY(border)));
            target.SubMaxXMaxY(border).Set(funMaxXMaxY(source.SubMaxXMaxY(border)));
            return target;
        }

        /// <summary>
        /// Process an image volume with specific actions for the center and border
        /// parts.
        /// </summary>
        public static void ApplyCenterBordersAndCorners<T>(
                this Volume<T> source, Border2l border,
                Action<Volume<T>> actCenter,
                Action<Volume<T>> actMinX, Action<Volume<T>> actMaxX,
                Action<Volume<T>> actMinY, Action<Volume<T>> actMaxY,
                Action<Volume<T>> actMinXMinY, Action<Volume<T>> actMaxXMinY,
                Action<Volume<T>> actMinXMaxY, Action<Volume<T>> actMaxXMaxY)
        {

            actCenter(source.SubCenter(border));
            actMinX(source.SubMinX(border)); actMaxX(source.SubMaxX(border));
            actMinY(source.SubMinY(border)); actMaxY(source.SubMaxY(border));
            actMinXMinY(source.SubMinXMinY(border)); actMaxXMinY(source.SubMaxXMinY(border));
            actMinXMaxY(source.SubMinXMaxY(border)); actMaxXMaxY(source.SubMaxXMaxY(border));
        }

        #endregion

        #region Specific Matrix Border Manipulations

        /// <summary>
        /// Creates a new matrix with a border of the supplied size around the supplied matrix.
        /// The resulting matrix retains the coordinates of the original matrix.
        /// </summary>
        public static Matrix<T> CopyWithBorderWindow<T>(this Matrix<T> matrix, Border2l border)
        {
            var bm = new Matrix<T>(matrix.SX + border.Min.X + border.Max.X,
                                   matrix.SY + border.Min.Y + border.Max.Y)
                        { F = new V2l(matrix.FX - border.Min.X, matrix.FY - border.Min.Y) };
            bm.SubCenter(border).Set(matrix);
            return bm;
        }

        /// <summary>
        /// Creates a new matrix with a border of the supplied size around the supplied matrix.
        /// the resulting matrix starts at zero coordinates.
        /// </summary>
        public static Matrix<T> CopyWithBorder<T>(this Matrix<T> matrix, Border2l border)
        {
            var bm = new Matrix<T>(matrix.SX + border.Min.X + border.Max.X,
                                   matrix.SY + border.Min.Y + border.Max.Y);
            bm.SubCenter(border).Set(matrix);
            return bm;
        }


        /// <summary>
        /// Set the border of a matrix to a specific value.
        /// </summary>
        public static void SetBorder<T>(this Matrix<T> matrix, Border2l border, T value)
        {
            matrix.ApplyCenterBordersAndCorners(border,
                m => { },
                m => m.Set(value), m => m.Set(value), m => m.Set(value), m => m.Set(value),
                m => m.Set(value), m => m.Set(value), m => m.Set(value), m => m.Set(value));
        }

        /// <summary>
        /// Replicate the border pixels of the center region outward.
        /// </summary>
        public static void ReplicateBorder<T>(this Matrix<T> matrix, Border2l border)
        {
            matrix.ApplyCenterBordersAndCorners(new Border2l(border.Min, border.Max),
            m => { },
            m => m.SetByCoord(y => m[m.EX, y], (y, x, vy) => vy),
            m => m.SetByCoord(y => m[m.FX - 1, y], (y, x, vy) => vy),
            m =>
            {
                var tm = m.SubMatrixWindow(m.F, m.S.YX, m.D.YX, m.F.YX);
                tm.SetByCoord(y => tm[tm.EX, y], (y, x, vy) => vy);
            },
            m =>
            {
                var tm = m.SubMatrixWindow(m.F, m.S.YX, m.D.YX, m.F.YX);
                tm.SetByCoord(y => tm[tm.FX - 1, y], (y, x, vy) => vy);
            },
            m => { var v = m[m.EX, m.EY]; m.SetByCoord((x, y) => v); },
            m => { var v = m[m.FX - 1, m.EY]; m.SetByCoord((x, y) => v); },
            m => { var v = m[m.EX, m.FY - 1]; m.SetByCoord((x, y) => v); },
            m => { var v = m[m.FX - 1, m.FY - 1]; m.SetByCoord((x, y) => v); });
        }

        public static Matrix<T> GetMatrixFromTiles<T>(
                this Border2l border, long x, long y, Func<long, long, Box.Flags, Matrix<T>> x_y_loadFun)
        {
            var flipped = border.Flipped;
            var center = x_y_loadFun(x, y, Box.Flags.All);
            if (center.IsInvalid) return center;
            var result = center.CopyWithBorderWindow(border);

            if (border.Min.Y > 0)
            {
                if (border.Min.X > 0)
                {
                    var tnn = x_y_loadFun(x - 1, y - 1, Box.Flags.MaxXMaxY);
                    if (tnn.IsValid) result.SubMinXMinY(border).Set(tnn.SubMaxXMaxY(flipped));
                }
                var t0n = x_y_loadFun(x, y - 1, Box.Flags.MaxY);
                if (t0n.IsValid) result.SubMinY(border).Set(t0n.SubMaxY(border.Min.Y));
                if (border.Max.X > 0)
                {
                    var tpn = x_y_loadFun(x + 1, y - 1, Box.Flags.MinXMaxY);
                    if (tpn.IsValid) result.SubMaxXMinY(border).Set(tpn.SubMinXMaxY(flipped));
                }
            }
            if (border.Min.X > 0)
            {
                var tn0 = x_y_loadFun(x - 1, y, Box.Flags.MaxX);
                if (tn0.IsValid) result.SubMinX(border).Set(tn0.SubMaxX(border.Min.X));
            }
            if (border.Max.X > 0)
            {
                var tp0 = x_y_loadFun(x + 1, y, Box.Flags.MinX);
                if (tp0.IsValid) result.SubMaxX(border).Set(tp0.SubMinX(border.Max.X));
            }
            if (border.Max.Y > 0)
            {
                if (border.Min.X > 0)
                {
                    var tnp = x_y_loadFun(x - 1, y + 1, Box.Flags.MaxXMinY);
                    if (tnp.IsValid) result.SubMinXMaxY(border).Set(tnp.SubMaxXMinY(flipped));
                }
                var t0p = x_y_loadFun(x, y + 1, Box.Flags.MinY);
                if (t0p.IsValid) result.SubMaxY(border).Set(t0p.SubMinY(border.Max.Y));
                if (border.Max.X > 0)
                {
                    var tpp = x_y_loadFun(x + 1, y + 1, Box.Flags.MinXMinY);
                    if (tpp.IsValid) result.SubMaxXMaxY(border).Set(tpp.SubMinXMinY(flipped));
                }
            }
            return result;
        }


        #endregion

        #region Specific Volume Border Manipulations

        /// <summary>
        /// Creates a new image volume with a border of the supplied size
        /// around the supplied image volume.
        /// </summary>
        public static Volume<T> CopyWithBorderWindow<T>(this Volume<T> volume, Border2l border)
        {
            var iv = new V3l(volume.SX + border.Min.X + border.Max.X,
                             volume.SY + border.Min.Y + border.Max.Y,
                             volume.SZ).CreateImageVolume<T>();
            iv.F = new V3l(volume.FX - border.Min.X, volume.FY - border.Min.Y, volume.FZ);
            iv.SubCenter(border).Set(volume);
            return iv;
        }

        /// <summary>
        /// Creates a new image volume with a border of the supplied size
        /// around the supplied image volume.
        /// </summary>
        public static Volume<T> CopyWithBorder<T>(this Volume<T> volume, Border2l border)
        {
            var iv = new V3l(volume.SX + border.Min.X + border.Max.X,
                             volume.SY + border.Min.Y + border.Max.Y,
                             volume.SZ).CreateImageVolume<T>();
            iv.SubCenter(border).Set(volume);
            return iv;
        }

        /// <summary>
        /// Set the border of an image volume to a specific value.
        /// </summary>
        public static void SetBorder<T>(this Volume<T> volume, Border2l border, T value)
        {
            volume.ApplyCenterBordersAndCorners(border,
                v => { },
                v => v.Set(value), v => v.Set(value), v => v.Set(value), v => v.Set(value),
                v => v.Set(value), v => v.Set(value), v => v.Set(value), v => v.Set(value));
        }

        /// <summary>
        /// Replicate the border pixels of the center region outward.
        /// </summary>
        public static void ReplicateBorder<T>(this Volume<T> volume, Border2l border)
        {
            volume.ApplyCenterBordersAndCorners(border,
            v => { },
            v => v.SetByCoord(z => false, (z, y, vz) => v[v.EX, y, z], (z, y, x, vz, vy) => vy),
            v => v.SetByCoord(z => false, (z, y, vz) => v[v.FX - 1, y, z], (z, y, x, vz, vy) => vy),
            v =>
            {
                var tv = v.SubVolumeWindow(v.F, v.S.YXZ, v.D.YXZ, v.F.YXZ);
                tv.SetByCoord(z => false, (z, y, vz) => tv[tv.EX, y, z], (z, y, x, vz, vy) => vy);
            },
            v =>
            {
                var tv = v.SubVolumeWindow(v.F, v.S.YXZ, v.D.YXZ, v.F.YXZ);
                tv.SetByCoord(z => false, (z, y, vz) => tv[tv.FX - 1, y, z], (z, y, x, vz, vy) => vy);
            },
            v => v.SetByCoord(z => v[v.EX, v.EY, z], (z, y, vz) => false, (z, y, x, vz, vy) => vz),
            v => v.SetByCoord(z => v[v.FX - 1, v.EY, z], (z, y, vz) => false, (z, y, x, vz, vy) => vz),
            v => v.SetByCoord(z => v[v.EX, v.FY - 1, z], (z, y, vz) => false, (z, y, x, vz, vy) => vz),
            v => v.SetByCoord(z => v[v.FX - 1, v.FY - 1, z], (z, y, vz) => false, (z, y, x, vz, vy) => vz));
        }

        public static Volume<T> GetMatrixFromTiles<T>(
                this Border2l border, long x, long y, Func<long, long, Box.Flags, Volume<T>> x_y_loadFun)
        {
            var flipped = border.Flipped;
            var center = x_y_loadFun(x, y, Box.Flags.All);
            if (center.IsInvalid) return center;
            var result = center.CopyWithBorderWindow(border);

            if (border.Min.Y > 0)
            {
                if (border.Min.X > 0)
                {
                    var tnn = x_y_loadFun(x - 1, y - 1, Box.Flags.MaxXMaxY);
                    if (tnn.IsValid) result.SubMinXMinY(border).Set(tnn.SubMaxXMaxY(flipped));
                }
                var t0n = x_y_loadFun(x, y - 1, Box.Flags.MaxY);
                if (t0n.IsValid) result.SubMinY(border).Set(t0n.SubMaxY(border.Min.Y));
                if (border.Max.X > 0)
                {
                    var tpn = x_y_loadFun(x + 1, y - 1, Box.Flags.MinXMaxY);
                    if (tpn.IsValid) result.SubMaxXMinY(border).Set(tpn.SubMinXMaxY(flipped));
                }
            }
            if (border.Min.X > 0)
            {
                var tn0 = x_y_loadFun(x - 1, y, Box.Flags.MaxX);
                if (tn0.IsValid) result.SubMinX(border).Set(tn0.SubMaxX(border.Min.X));
            }
            if (border.Max.X > 0)
            {
                var tp0 = x_y_loadFun(x + 1, y, Box.Flags.MinX);
                if (tp0.IsValid) result.SubMaxX(border).Set(tp0.SubMinX(border.Max.X));
            }
            if (border.Max.Y > 0)
            {
                if (border.Min.X > 0)
                {
                    var tnp = x_y_loadFun(x - 1, y + 1, Box.Flags.MaxXMinY);
                    if (tnp.IsValid) result.SubMinXMaxY(border).Set(tnp.SubMaxXMinY(flipped));
                }
                var t0p = x_y_loadFun(x, y + 1, Box.Flags.MinY);
                if (t0p.IsValid) result.SubMaxY(border).Set(t0p.SubMinY(border.Max.Y));
                if (border.Max.X > 0)
                {
                    var tpp = x_y_loadFun(x + 1, y + 1, Box.Flags.MinXMinY);
                    if (tpp.IsValid) result.SubMaxXMaxY(border).Set(tpp.SubMinXMinY(flipped));
                }
            }
            return result;
        }

        #endregion

    }
}
