using System.Runtime.InteropServices;

namespace PopLoader.DataProcessor.BinaryHelper;

[StructLayout(LayoutKind.Explicit)]
public struct AsciiUint24
{
    [FieldOffset(0)]
    public byte Character;
    [FieldOffset(0)]
    private readonly int _offset;
    /// <summary>
    /// = (Offset from the start of trie to the next sibling) / 4
    /// </summary>
    public readonly int Offset => _offset >> 8;
    
    public AsciiUint24(BinaryReader br)
    {
        _offset = br.ReadInt32();
    }
    public readonly void Write(BinaryWriter bw)
    {
        bw.Write(_offset);
    }
}
