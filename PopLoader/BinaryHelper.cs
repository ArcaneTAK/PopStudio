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
        using ZLibStream zLibStream = new(stream, CompressionMode.Decompress);
        zLibStream.CopyTo(temp);
        stream.Dispose();
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
    public static int ReadInt24LittleEndian(this BinaryReader br)
    {
        return br.ReadByte() | (br.ReadByte() << 8) | (br.ReadByte() << 16);
    }
    /// <summary>
    /// Good luck ¯\_(ツ)_/¯.
    /// </summary>
    /// <param name="br"></param>
    /// <returns></returns>
    public static string ReadUTF8StringSkip3EndWithNull(this BinaryReader br)
    {
        List<byte> str = [];
        byte a;
        int unknown;
        while ((a = br.ReadByte()) != 0)
        {
            str.Add(a);
            unknown = br.ReadInt24LittleEndian(); // some kind of length, not sure if neccessary.
        }
        unknown = br.ReadInt24LittleEndian();
        return Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(str));
    }
}
