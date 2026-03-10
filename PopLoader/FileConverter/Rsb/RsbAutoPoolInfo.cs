namespace PopLoader.FileConverter.Rsb
{
    public class RsbAutoPoolInfo
    {
        public string ID;
        public int part1_MaxOffset_InDecompress;
        public int part1_MaxSize;
        public int type = 1;

        public RsbAutoPoolInfo(BinaryReader br)
        {
            ID = new string(br.ReadChars(128));
            part1_MaxOffset_InDecompress = br.ReadInt32();
            part1_MaxSize = br.ReadInt32();
            type = br.ReadInt32();
            _ = br.ReadInt32();
            _ = br.ReadInt32();
            _ = br.ReadInt32();
        }
    }
}
