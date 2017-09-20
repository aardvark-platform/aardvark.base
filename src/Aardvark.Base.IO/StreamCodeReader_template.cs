namespace Aardvark.Base.Coder
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# Action comma = () => Out(", ");
    //# IEnumerable<string> ignoreTypes = null;
    public partial class StreamCodeReader
    {

        #region Vectors

        //# foreach (var t in Meta.VecTypes) {
        public __t.Name__ __t.Read__()
        {
            return new __t.Name__(/*# t.Len.ForEach(i => {
                */__t.FieldType.Read__()/*# }, comma); */);
        }

        //# }
        #endregion

        #region Matrices

        //# foreach (var t in Meta.MatTypes) {
        public __t.Name__ __t.Read__()
        {
            return new __t.Name__(/*# t.Rows.ForEach(i => { */
                /*# t.Cols.ForEach(j => { */__t.FieldType.Read__()/*# }, comma); }, comma); */
                );
        }

        //# }
        #endregion

        #region Ranges and Boxes

        //# foreach (var t in Meta.RangeAndBoxTypes) {
        public __t.Name__ __t.Read__()
        {
            return new __t.Name__(__t.LimitType.Read__(), __t.LimitType.Read__());
        }

        //# }
        #endregion

        #region Colors

        //# foreach (var t in Meta.ColorTypes) {
        public __t.Name__ __t.Read__()
        {
            return new __t.Name__(/*# t.Len.ForEach(i => {
                                   */__t.FieldType.Read__()/*# }, comma); */);
        }

        //# }
        #endregion

        //# if (ignoreTypes != null) {
        #region Arrays

        //# ignoreTypes.ForEach(t => {
        public long ReadBig(__t__[] array, long index, long count)
        {
            int complete = 0;
            unsafe
            {
                int itemsPerBlock = c_bufferSize / sizeof(__t__);
                while (count > 0)
                {
                    int blockSize = count > (long)itemsPerBlock
                                        ? itemsPerBlock : (int)count;
                    int request = blockSize * sizeof(__t__);
                    int total = 0;
                    do
                    {
                        int finished = Read(m_buffer, total, request);
                        if (finished == 0) break;
                        total += finished; request -= finished;
                    }
                    while (request > 0);
                    total /= sizeof(__t__);
                    fixed (byte* p = m_buffer)
                    {
                        __t__* v = (__t__*)p;
                        for (int i = 0; i < total; i++)
                            array[index++] = v[i];
                    }
                    count -= total; complete += total;
                    if (total < blockSize) break;
                }
            }
            return complete;
        }

        //# });
        #endregion

        #region Multi-Dimensional Arrays

        //# ignoreTypes.ForEach(t => {
        public long ReadBig(__t__[,] array, long count)
        {
            long complete = 0;
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
                        int request = blockSize * sizeof(__t__);
                        int total = 0;
                        do
                        {
                            int finished = Read(m_buffer, total, request);
                            if (finished == 0) break;
                            total += finished; request -= finished;
                        }
                        while (request > 0);
                        total /= sizeof(__t__);
                        fixed (byte* p = m_buffer)
                        {
                            __t__* v = (__t__*)p;
                            for (int i = 0; i < total; i++)
                                a[index++] = v[i];
                        }
                        count -= total; complete += total;
                        if (total < blockSize) break;
                    }
                }
            }
            return complete;
        }

        public long ReadBig(__t__[, ,] array, long count)
        {
            long complete = 0;
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
                        int request = blockSize * sizeof(__t__);
                        int total = 0;
                        do
                        {
                            int finished = Read(m_buffer, total, request);
                            if (finished == 0) break;
                            total += finished; request -= finished;
                        }
                        while (request > 0);
                        total /= sizeof(__t__);
                        fixed (byte* p = m_buffer)
                        {
                            __t__* v = (__t__*)p;
                            for (int i = 0; i < total; i++)
                                a[index++] = v[i];
                        }
                        count -= total; complete += total;
                        if (total < blockSize) break;
                    }
                }
            }
            return complete;
        }

        //# });
        #endregion

        #region Lists

        //# ignoreTypes.ForEach(t => {
        public int Read(List<__t__> buffer, int index, int count)
        {
            int end = index + count;
            while (buffer.Count < end) buffer.Add(default(__t__));
            int complete = 0;
            unsafe
            {
                var itemsPerBlock = c_bufferSize / sizeof(__t__);
                while (count > 0)
                {
                    int blockSize = Fun.Min(count, itemsPerBlock);
                    int request = blockSize * sizeof(__t__);
                    int total = 0;
                    do
                    {
                        int finished = Read(m_buffer, total, request);
                        if (finished == 0) break;
                        total += finished; request -= finished;
                    }
                    while (request > 0);
                    total /= sizeof(__t__);
                    fixed (byte* p = m_buffer)
                    {
                        __t__* v = (__t__*)p;
                        for (int i = 0; i < total; i++)
                            buffer[index++] = v[i];
                    }
                    count -= total; complete += total;
                    if (total < blockSize) break;
                }
            }
            return complete;
        }

        //# });
        #endregion

        //# } // if (ignoreTypes != null)
    }
}
