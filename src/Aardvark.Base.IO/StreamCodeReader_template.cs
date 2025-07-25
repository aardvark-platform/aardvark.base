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
namespace Aardvark.Base.Coder;

// AUTO GENERATED CODE - DO NOT CHANGE!

//# Action comma = () => Out(", ");
//# IEnumerable<string> ignoreTypes = null;
public partial class StreamCodeReader
{

    #region Vectors

    //# foreach (var t in Meta.VecTypes) {
    public __t.Name__ __t.Read__()
        => new(/*# t.Len.ForEach(i => {
            */__t.FieldType.Read__()/*# }, comma); */);

    //# }
    #endregion

    #region Matrices

    //# foreach (var t in Meta.MatTypes) {
    public __t.Name__ __t.Read__()
        => new(/*# t.Rows.ForEach(i => { */
            /*# t.Cols.ForEach(j => { */__t.FieldType.Read__()/*# }, comma); }, comma); */
            );

    //# }
    #endregion

    #region Ranges and Boxes

    //# foreach (var t in Meta.RangeAndBoxTypes) {
    public __t.Name__ __t.Read__()
        => new(__t.LimitType.Read__(), __t.LimitType.Read__());

    //# }
    #endregion

    #region Colors

    //# foreach (var t in Meta.ColorTypes) {
    public __t.Name__ __t.Read__()
        => new(/*# t.Len.ForEach(i => {
                               */__t.FieldType.Read__()/*# }, comma); */);

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
