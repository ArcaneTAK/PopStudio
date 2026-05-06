using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PopLoader.DataProcessor.BinaryHelper;

namespace PopLoader.DataProcessor.Rsb;

[StructLayout(LayoutKind.Sequential)]
public struct RsbAutoPool
{
    public CString128 ID;
    public int DecompressedData;
    public int DecompressedImage;
    public int type; // always 1 for some reason
    public int reserve0;
    public int reserve1;
    public int reserve2;
}
