namespace PopLoader.FileConverter.Rsb;

public class RsbHeader
{
    public int Version;
    public int HeaderLength;
    public int FileCount;
    public int FileListOffset;

    public int SubgroupListCount;
    public int SubgroupListInfoOffset;
    public int SubgroupCount;
    public int SubgroupInfoOffset;
    public int RsgpInfoSize;

    public int GroupCount;
    public int GroupInfoOffset;
    public int GroupInfoSize;
    public int GroupListCount;
    public int GroupListOffset;

    public int AutopoolCount;
    public int AutopoolInfoOffset;
    public int AutopoolInfoSize;

    public int PtxCount;
    public int PtxInfoOffset;
    /// <summary>
    /// The size in byte of the PtxInfo, use to indentify the ptx info type:
    /// <list type="bullet">
    /// <item>16 for International version</item>
    /// <item>20</item>
    /// <item>24 for Chinese version</item>
    /// </list>
    /// </summary>
    public int PtxInfoSize;

    // public int XmlPart1BeginOffset;
    // public int XmlPart2BeginOffset;
    // public int XmlPart3BeginOffset;

    public int RsbInfoLength;
    public RsbHeader(BinaryReader br)
    {
        int headerMagic = br.ReadInt32();
        if (headerMagic != HeaderMagic.Rsb1HeaderMagic)
        {
            throw new InvalidDataException("Unsupported file format. Expected RSB file magic: " + HeaderMagic.Rsb1HeaderMagic + ", but got: " + headerMagic);
        }
        
        Version = br.ReadInt32();
        _ = br.ReadInt32();
        HeaderLength = br.ReadInt32();
        
        FileCount = br.ReadInt32();
        FileListOffset = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();

        SubgroupListCount = br.ReadInt32();
        SubgroupListInfoOffset = br.ReadInt32();
        SubgroupCount = br.ReadInt32();
        SubgroupInfoOffset = br.ReadInt32();
        RsgpInfoSize = br.ReadInt32();

        GroupCount = br.ReadInt32();
        GroupInfoOffset = br.ReadInt32();
        GroupInfoSize = br.ReadInt32();
        GroupListCount = br.ReadInt32();
        GroupListOffset = br.ReadInt32();

        AutopoolCount = br.ReadInt32();
        AutopoolInfoOffset = br.ReadInt32();
        AutopoolInfoSize = br.ReadInt32();

        PtxCount = br.ReadInt32();
        PtxInfoOffset = br.ReadInt32();
        PtxInfoSize = br.ReadInt32();

        _ = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();

        if (Version == 4) RsbInfoLength = br.ReadInt32();
    }
}