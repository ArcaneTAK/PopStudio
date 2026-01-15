namespace PopLoader.FileConverter.Rsb
{
    public class RsbAutoPoolInfo
    {
        
        public string ID;
        public int part1_MaxOffset_InDecompress;
        public int part1_MaxSize;
        public int type = 1;

        public RsbAutoPoolInfo Read(BinaryReader bs)
        {
            ID = new string(bs.ReadChars(128));
            part1_MaxOffset_InDecompress = bs.ReadInt32();
            part1_MaxSize = bs.ReadInt32();
            type = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            return this;
        }
    }
}
