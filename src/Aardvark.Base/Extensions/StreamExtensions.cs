using System.IO;

namespace Aardvark.Base
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads <paramref name="count"/> number of bytes from the current stream and advances the position within the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="buffer">The buffer to read into.</param>
        /// <param name="offset">The byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The number of bytes to be read from the current stream. Defaults to <paramref name="buffer"/>.Length - <paramref name="offset"/> if negative.</param>
        /// <exception cref="EndOfStreamException"></exception>
        public static void ReadBytes(this Stream stream, byte[] buffer, int offset = 0, int count = -1)
        {
            if (count < 0) count = buffer.Length - offset;
#if NET8_0_OR_GREATER
            stream.ReadExactly(buffer, offset, count);
#else
            int totalRead = 0;

            while (totalRead < count)
            {
                int read = stream.Read(buffer, offset + totalRead, count - totalRead);
                if (read == 0)
                {
                    throw new EndOfStreamException();
                }

                totalRead += read;
            }
#endif
        }
    }
}
