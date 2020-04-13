using System;
using System.IO;

namespace SGBB.Utilities
{
    public static class StringExtension
    {
        public static byte[] ToBytes(this string texto)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(texto);
                        writer.Flush();
                        stream.Position = 0;
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Stream ToStream(this string texto)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(texto);
                        writer.Flush();
                        stream.Position = 0;
                        return stream;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
