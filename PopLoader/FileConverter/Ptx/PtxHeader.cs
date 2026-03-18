using PopLoader.BinaryManager;
using PopLoader.Texture;

namespace PopLoader.FileConverter.Ptx;

public class PtxHeader
{
    public const int Ptx1HeaderMagic = 1886681137;  //  1xtp
    public PtxHeader(BinaryReader br)
    {
        br.ReadMagicInt32(Ptx1HeaderMagic);
        Version = br.ReadInt32();
        Width = br.ReadInt32();
        Height = br.ReadInt32();
        Check = br.ReadInt32();
        Format = (PTXFormat)br.ReadInt32();
        AlphaSize = br.ReadInt32();
        AlphaFormat = br.ReadInt32();
    }

    public int Version;
    public int Width;
    public int Height;
    public int Check; // No idea what this does.
    public PTXFormat Format;
    public int AlphaSize;
    public int AlphaFormat;
}