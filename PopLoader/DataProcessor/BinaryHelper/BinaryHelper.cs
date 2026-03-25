using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

namespace PopLoader.DataProcessor.BinaryHelper;
public static class BHelper
{
    public static int Utf8ToInt(string s) => BitConverter.ToInt32(Encoding.UTF8.GetBytes(s));
}

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
public delegate T ReadBinary<T>(BinaryReader br);

public static class BinaryReaderHelper
{
    public static int ReadInt32(BinaryReader br) => br.ReadInt32();
    public static string ReadUTF8StringEndWithNull(this BinaryReader br)
    {
        List<byte> str = [];
        byte a;
        while ((a = br.ReadByte()) != 0x00)
            str.Add(a);
        return Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(str));
    }

    public static string ReadUTF8ShortLengthPrefix(this BinaryReader br)
    {
        short Length = br.ReadInt16();
        return Encoding.UTF8.GetString(br.ReadBytes(Length));
    }

    public static void ReadMagicInt32(this BinaryReader br, int magic)
    {
        if (br.ReadInt32() != magic) throw new InvalidDataException("Wrong magic header! The file is not of the expected type or is corrupted");
    }
}

public static class BinaryWriterHelper
{
    
}