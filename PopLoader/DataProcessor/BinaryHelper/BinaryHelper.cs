using System.Diagnostics.CodeAnalysis;
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

public static class ClassHelper
{
    public static bool TryCast<T>(object? input, [MaybeNullWhen(false)] out T result)
    {
        if (input is T r)
        {
            result = r;
            return true;
        }
        result = default(T);
        return false;
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

    public static string ReadUTF8ShortLengthPrefix(this BinaryReader br)
    {
        short Length = br.ReadInt16();
        return Encoding.UTF8.GetString(br.ReadBytes(Length));
    }

    public static void ReadMagicInt32(this BinaryReader br, int magic)
    {
        if (br.ReadInt32() != magic) throw new InvalidDataException("Wrong magic header! The file is not of the expected type or is corrupted");
    }

    public static uint Read7BitEncodedUInt(this BinaryReader br) => (uint)br.Read7BitEncodedInt();
    public static ulong Read7BitEncodedUInt64(this BinaryReader br) => (ulong)br.Read7BitEncodedInt64();
    public static int ReadVarZigZag32(this BinaryReader br) => ZigZagDecodeInt32(br.Read7BitEncodedInt());
    public static long ReadVarZigZag64(this BinaryReader br) => ZigZagDecodeInt64(br.Read7BitEncodedInt64());

    private static int ZigZagDecodeInt32(int value)
    {
        return (value >>> 1) ^ ((value << 31) >> 31);
    }
    private static long ZigZagDecodeInt64(long value)
    {
        return (value >>> 1) ^ ((value << 63) >> 63);
    }
}

public static class BinaryWriterHelper
{
    private static int ZigZagEncodeInt32(int value)
    {
        return (value << 1) ^ (value >> 31);
    }
    private static long ZigZagDecodeInt64(long value)
    {
        return (value << 1) ^ (value >> 63);
    }
}

