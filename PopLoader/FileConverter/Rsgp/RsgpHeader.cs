using PopLoader.BinaryManager;
using PopLoader.FileConverter.Rsb;

namespace PopLoader.FileConverter.Rsgp;

/// <summary>
/// Not to be confused with <see cref="ResoureGroupPackageInfo"/> 
/// </summary>
public class RsgpHeader
{
    public const int RsgpHeaderMagic = 1920165744;  //  pgsr
    public RsgpHeader(BinaryReader br)
    {
        br.ReadMagicInt32(RsgpHeaderMagic);
        Version = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();

        DataFlags = (DataFlags)br.ReadInt32();
        RsgHeaderSize = br.ReadInt32();
        DataOffset = br.ReadInt32();
        DataBlobSize = br.ReadInt32();

        DecompressedDataSize = br.ReadInt32();
        _ = br.ReadInt32();
        ImageOffset = br.ReadInt32();
        CompressedImageSize = br.ReadInt32();

        DecompressedImageSize = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();

        _ = br.ReadInt32();
        _ = br.ReadInt32();
        TrieSize = br.ReadInt32();
        TrieOffset = br.ReadInt32();
    }
    public int Version;
    public DataFlags DataFlags;
    public int RsgHeaderSize;

    public int DataOffset;
    public int DataBlobSize;
    /// <summary>
    /// Assigned but doesn't seem to do anything.
    /// </summary>
    public int DecompressedDataSize;

    /// <summary>
    /// Offset relative to <see cref="GroupInfo.Offset"/>.
    /// </summary>
    public int ImageOffset;
    public int CompressedImageSize;
    /// <summary>
    /// For verifying the length. Is Set. Unused.
    /// </summary>
    public int DecompressedImageSize;
    public int TrieSize;
    /// <summary>
    /// Offset relative to the start of the RSGP file.
    /// </summary>
    public int TrieOffset;

}