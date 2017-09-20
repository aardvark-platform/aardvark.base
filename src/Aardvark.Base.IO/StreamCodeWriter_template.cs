namespace Aardvark.Base.Coder
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# IEnumerable<string> ignoreTypes = null;
    public partial class StreamCodeWriter
    {

        #region Vectors

        //# foreach (var t in Meta.VecTypes) {
        public void Write(__t.Name__ x)
        {
            /*# t.Fields.ForEach(f => { */Write(x.__f__); /*# }); */
        }

        //# }
        #endregion

        #region Matrices

        //# foreach (var t in Meta.MatTypes) {
        public void Write(__t.Name__ x)
        {
            //# t.Rows.ForEach(i => {
            /*# t.Cols.ForEach(j => { */Write(x.M__i____j__); /*# }); */
            //# });
        }

        //# }
        #endregion

        #region Ranges and Boxes

        //# foreach (var t in Meta.RangeAndBoxTypes) {
        public void Write(__t.Name__ x)
        {
            Write(x.Min); Write(x.Max);
        }

        //# }
        #endregion

        #region Colors

        //# foreach (var t in Meta.ColorTypes) {
        public void Write(__t.Name__ c)
        {
            /*# t.Fields.ForEach(f => { */Write(c.__f__); /*# }); */
        }

        //# }
        #endregion

        //# if (ignoreTypes != null) {
        #region Arrays

        //# ignoreTypes.ForEach(t => {
        public void WriteBig(__t__[] array, long index, long count)
        {
            unsafe
            {
                int itemsPerBlock = c_bufferSize / sizeof(__t__);
                while (count > 0)
                {
                    int blockSize = count > (long)itemsPerBlock
                                        ? itemsPerBlock : (int)count;
                    // array.CopyIntoBuffer(index, blockSize, m_buffer, 0);
                    // index += blockSize;
                    fixed (byte* p = m_buffer)
                    {
                        __t__* v = (__t__*)p;
                        for (int i = 0; i < blockSize; i++)
                            v[i] = array[index++];
                    }
                    Write(m_buffer, 0, blockSize * sizeof(__t__));
                    count -= (long)blockSize;
                }
            }
        }

        //# });
        #endregion

        #region Multi-Dimensional Arrays

        //# ignoreTypes.ForEach(t => {
        public void WriteBig(__t__[,] array, long count)
        {
            unsafe
            {
                int itemsPerBlock = c_bufferSize / sizeof(__t__);
                long index = 0;
                fixed (__t__* a = array)
                {
                    while (count > 0)
                    {
                        int blockSize = count > (long)itemsPerBlock
                                            ? itemsPerBlock : (int)count;
                        fixed (byte* p = m_buffer)
                        {
                            __t__* v = (__t__*)p;
                            for (int i = 0; i < blockSize; i++)
                                v[i] = a[index++];
                        }
                        Write(m_buffer, 0, blockSize * sizeof(__t__));
                        count -= (long)blockSize;
                    }
                }
            }
        }

        public void WriteBig(__t__[, ,] array, long count)
        {
            unsafe
            {
                int itemsPerBlock = c_bufferSize / sizeof(__t__);
                long index = 0;
                fixed (__t__* a = array)
                {
                    while (count > 0)
                    {
                        int blockSize = count > (long)itemsPerBlock
                                            ? itemsPerBlock : (int)count;
                        fixed (byte* p = m_buffer)
                        {
                            __t__* v = (__t__*)p;
                            for (int i = 0; i < blockSize; i++)
                                v[i] = a[index++];
                        }
                        Write(m_buffer, 0, blockSize * sizeof(__t__));
                        count -= (long)blockSize;
                    }
                }
            }
        }

        //# });
        #endregion

        #region Lists

        //# ignoreTypes.ForEach(t => {
        public void Write(List<__t__> buffer, int index, int count)
        {
            unsafe
            {
                var itemsPerBlock = c_bufferSize / sizeof(__t__);
                while (count > 0)
                {
                    int blockSize = Fun.Min(count, itemsPerBlock);
                    fixed (byte* p = m_buffer)
                    {
                        __t__* v = (__t__*)p;
                        for (int i = 0; i < blockSize; i++)
                            v[i] = buffer[index++];
                    }
                    Write(m_buffer, 0, blockSize * sizeof(__t__));
                    count -= blockSize;
                }
            }
        }

        //# });
        #endregion

        //# } // if (ignoreTypes != null)
    }
}
