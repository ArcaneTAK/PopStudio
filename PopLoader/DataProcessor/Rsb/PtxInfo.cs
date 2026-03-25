using PopLoader.Texture;

namespace PopLoader.DataProcessor.Rsb
{
    public struct PtxInfo
    {
        public int Width;
        public int Height;
        /// <summary>
        /// The number of bytes in one row of the image;
        /// </summary>
        public int Stride;
        public PTXFormat Format;

        public PtxInfo(BinaryReader bs)
        {
            Width = bs.ReadInt32();
            Height = bs.ReadInt32();
            Stride = bs.ReadInt32();
            Format = (PTXFormat)bs.ReadInt32();
        }
    }
}