using System.Runtime.InteropServices;
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

        public PtxInfo(BinaryReader br)
        {
            Width = br.ReadInt32();
            Height = br.ReadInt32();
            Stride = br.ReadInt32();
            Format = (PTXFormat)br.ReadInt32();
        }
    }
}