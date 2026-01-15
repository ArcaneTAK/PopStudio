using PopLoader.Texture;

namespace PopLoader.FileConverter.Ptx;

public class PtxHeader
{
    public int Version;
    public int Width;
    public int Height;
    public int Check; // No idea what this does.
    public PTXFormat Format;
    public int AlphaSize;
    public int AlphaFormat;
    public PtxHeader(BinaryReader br)
    {
        int headerMagic = br.ReadInt32();
        if (headerMagic != HeaderMagic.Ptx1HeaderMagic)
        {
            throw new InvalidDataException("Unsupported file format. Expected RSB file magic: " + HeaderMagic.Rsb1HeaderMagic + ", but got: " + headerMagic);
        }
        Version = br.ReadInt32();
        Width = br.ReadInt32();
        Height = br.ReadInt32();
        Check = br.ReadInt32();
        Format = (PTXFormat)br.ReadInt32();
        AlphaSize = br.ReadInt32();
        AlphaFormat = br.ReadInt32();
    }
}