using PopLoader.Texture;

namespace PopLoader.FileConverter.Rsb
{
    public class PtxInfo
    {
        public int Width;
        public int Height;
        public int Check;
        public PTXFormat Format;

        public PtxInfo(BinaryReader bs)
        {
            Width = bs.ReadInt32();
            Height = bs.ReadInt32();
            Check = bs.ReadInt32();
            Format = (PTXFormat)bs.ReadInt32();
        }
    }
}