using System.Runtime.InteropServices;

namespace PopLoader.FileConverter.Rsb
{
    /// <summary>
    /// 8 + 8 + 4 = 20 bytes
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildRsgpInfo
    {
        public int index;
        public int ratio;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] language;

        public ChildRsgpInfo Read(BinaryReader br)
        {
            index = br.ReadInt32();
            ratio = br.ReadInt32();
            language = br.ReadChars(4);
            _ = br.ReadInt32();
            return this;
        }
    }

    /// <summary>
    /// 128 * 2 + 64 * 20 + 4 = 1156 bytes
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CompositeInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public ChildRsgpInfo[] child_Info;
        [MarshalAs(UnmanagedType.U4)]
        public int child_Number;
        public CompositeInfo Read(BinaryReader br)
        {
            ID = br.ReadChars(128);
            for (int i = 0; i < 64; i++)
            {
                child_Info[i].Read(br);
            }
            child_Number = br.ReadInt32();
            return this;
        }
    }
}
