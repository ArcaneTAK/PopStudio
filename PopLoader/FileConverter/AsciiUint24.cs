using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PopLoader.BinaryHelper;

namespace PopLoader.FileConverter;

[StructLayout(LayoutKind.Explicit)]
public struct AsciiUint24 : IReadable<(byte, int)>, IWritable
{
    [FieldOffset(0)]
    public byte Character;
    [FieldOffset(0)]
    private readonly int _offset;
    /// <summary>
    /// = (Offset from the start of trie to the next sibling) / 4
    /// </summary>
    public readonly int Offset => _offset & 0xffffff >> 8;

    public static (byte, int) Read(BinaryReader reader)
    {
        AsciiUint24 my = Unsafe.BitCast<int, AsciiUint24>(reader.ReadInt32());
        return (my.Character, my.Offset);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(_offset);
    }
}
