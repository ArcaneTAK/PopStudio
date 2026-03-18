using PopLoader.BinaryManager;

namespace PopLoader.FileConverter.Rsb;

public class RsbHeader
{
    public const int Rsb1HeaderMagic = 1920164401;  //  1bsr

    public RsbHeader(BinaryReader br)
    {
        // 0x00
        br.ReadMagicInt32(Rsb1HeaderMagic);
        Version = br.ReadInt32();
        _ = br.ReadInt32();
        HeaderLength = br.ReadInt32();
        // 0x10
        FileTrieSizeBytes = br.ReadInt32();
        FileTrieOffset = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();
        // 0x20
            PackageTrieSizeBytes = br.ReadInt32();
            PackageTrieOffset = br.ReadInt32();
            PackageCount = br.ReadInt32();
            PackageInfoOffset = br.ReadInt32();
        // 0x30
        // Composite Shell
            OnePackageInfoSize = br.ReadInt32();
        GroupCount = br.ReadInt32();
        GroupInfoOffset = br.ReadInt32();
        GroupInfoSize = br.ReadInt32();
        // 0x40
        GroupTrieCount = br.ReadInt32();
        GroupTrieOffset = br.ReadInt32();
            AutopoolCount = br.ReadInt32();
            AutopoolInfoOffset = br.ReadInt32();
        // 0x50
            AutopoolInfoSize = br.ReadInt32();
        PtxCount = br.ReadInt32();
        PtxInfoOffset = br.ReadInt32();
        PtxInfoSize = br.ReadInt32();
        // 0x60
        _ = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();
        RsbInfoLength = br.ReadInt32();
    }
    public int Version;
    public int HeaderLength;
    public int FileTrieSizeBytes;
    public int FileTrieOffset;

    public int PackageTrieSizeBytes;
    public int PackageTrieOffset;
    public int PackageCount;
    public int PackageInfoOffset;
    public int OnePackageInfoSize;

    public int GroupCount;
    public int GroupInfoOffset;
    public int GroupInfoSize;
    public int GroupTrieCount;
    public int GroupTrieOffset;

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
}