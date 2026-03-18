namespace PopLoader.FileConverter.Rsb
{
    /// <summary>
    /// 8 + 8 + 4 = 20 bytes
    /// </summary>
    public class ChildRsgpInfo
    {
        public int index;
        public int ratio;
        public char[] language;

        public ChildRsgpInfo(BinaryReader br)
        {
            index = br.ReadInt32();
            ratio = br.ReadInt32();
            language = br.ReadChars(4);
            _ = br.ReadInt32();
        }
    }

    /// <summary>
    /// 128 * 2 + 64 * 20 + 4 = 1156 bytes
    /// </summary>
    public class CompositeInfo
    {
        public char[] ID;
        public ChildRsgpInfo[] child_Info;
        public int child_Number;
        public CompositeInfo(BinaryReader br)
        {
            ID = br.ReadChars(128);
            child_Info = new ChildRsgpInfo[64];
            for (int i = 0; i < 64; i++)
            {
                child_Info[i] = new(br);
            }
            child_Number = br.ReadInt32();
        }
    }
}
