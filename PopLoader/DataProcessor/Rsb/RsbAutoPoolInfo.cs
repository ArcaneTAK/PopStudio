using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PopLoader.DataProcessor.Rsb
{
    [InlineArray(128)]
    public struct String128
    {
        public byte buffer;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RsbAutoPool
    {
        public String128 ID;
        public int DecompressedData;
        public int DecompressedImage;
        public int type; // always 1 for some reason
        public int reserve0;
        public int reserve1;
        public int reserve2;
    }
}
