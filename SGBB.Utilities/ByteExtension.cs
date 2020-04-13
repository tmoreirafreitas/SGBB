using System.IO;

namespace SGBB.Utilities
{
    public static class ByteExtension
    {
        public static Stream ToStream(this byte[] data)
        {
            var ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            return ms;
        }
    }
}
