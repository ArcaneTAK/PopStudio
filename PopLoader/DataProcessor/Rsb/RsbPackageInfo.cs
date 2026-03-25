using PopLoader.DataProcessor.Rsgp;

namespace PopLoader.DataProcessor.Rsb;

/// <summary>
/// Not to be confused with <see cref="RsgpHeader"/>
/// </summary>
public class ResoureGroupPackageInfo
{
    public string Name;
    public int Offset;
    public int Size;
    /// <summary>
    /// The index of this subgroup in the pool.
    /// </summary>
    public int Id;
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

    public int ImageCount;
    /// <summary>
    /// The image Id of the first image (if there exists one) in the package, images after have their Id increament subsequently.
    /// Can be understood as the number of images in packages before this one.
    /// </summary>
    public int StartImageId;
    public ResoureGroupPackageInfo(BinaryReader br)
    {
        Name = new string(br.ReadChars(128)).Replace("\0", null);
        Offset = br.ReadInt32();
        Size = br.ReadInt32();
        Id = br.ReadInt32();

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

        ImageCount = br.ReadInt32();
        StartImageId = br.ReadInt32();
    }
}
[Flags]
public enum DataFlags
{
    None = 0,
    CompressedImage = 1,
    CompressedData = 2,
}