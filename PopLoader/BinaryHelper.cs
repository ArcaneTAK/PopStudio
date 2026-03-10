using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace PopLoader.BinaryHelper;

public static class Zlib
{
    public static MemoryStream Decompress(Stream stream)
    {
        MemoryStream temp = new();
        using ZLibStream zLibStream = new(stream, CompressionMode.Decompress, false);
        zLibStream.CopyTo(temp);
        return temp;
    }   
}

public static class BinaryReaderHelper
{
    public static string ReadUTF8StringEndWithNull(this BinaryReader br)
    {
        List<byte> str = [];
        byte a;
        while ((a = br.ReadByte()) != 0x00)
            str.Add(a);
        return Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(str));
    }
    public static string ListByteToString(List<byte> buffer)
    {
        return Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(buffer));
    }
}
