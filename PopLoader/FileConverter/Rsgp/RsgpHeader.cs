using PopLoader.FileConverter.Rsb;

namespace PopLoader.FileConverter.Rsgp;

/// <summary>
/// Not to be confused with <see cref="ResoureGroupHeader"/> 
/// </summary>
public class RsgpHeader
{
    public int Version;
    public ResourceGroupInfo SubgroupInfo;
    public int InfoSize;
    /// <summary>
    /// Offset relative to the start of the RSGP file.
    /// </summary>
    public int InfoOffset;

    public RsgpHeader(BinaryReader br)
    {
        int headerMagic = br.ReadInt32();
        if (headerMagic != HeaderMagic.RsgpHeaderMagic)
        {
            throw new InvalidDataException($"Unsupported file format. Expected RSGP file magic: {HeaderMagic.RsgpHeaderMagic}, but got: {headerMagic}");
        }

        Version = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();

        SubgroupInfo = new ResourceGroupInfo(br);

        InfoSize = br.ReadInt32();
        InfoOffset = br.ReadInt32();
    }
}