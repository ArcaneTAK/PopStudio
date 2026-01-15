namespace PopLoader.FileConverter.Rsgp;
/// <summary>
/// 14 bytes
/// </summary>
public class ResourceGroupInfo
{
    public DataFlags DataFlags;
    public int RsgHeaderSize;

    public int DataOffset;
    public int CompressedDataSize;
    /// <summary>
    /// Assigned but doesn't seem to do anything.
    /// </summary>
    public int DecompressedDataSize;
    public int Unknown1;

    /// <summary>
    /// Offset relative to <see cref="GroupInfo.Offset"/>.
    /// </summary>
    public int ImageOffset;
    public int CompressedImageSize;
    /// <summary>
    /// For verifying the length. Is Set. Unused.
    /// </summary>
    public int DecompressedImageSize;
    public int Unknown2;

    public ResourceGroupInfo(BinaryReader br)
    {
        DataFlags = (DataFlags)br.ReadInt32();
        RsgHeaderSize = br.ReadInt32();

        DataOffset = br.ReadInt32();
        CompressedDataSize = br.ReadInt32();
        DecompressedDataSize = br.ReadInt32();
        Unknown1 = br.ReadInt32();

        ImageOffset = br.ReadInt32();
        CompressedImageSize = br.ReadInt32();
        DecompressedImageSize = br.ReadInt32();
        Unknown2 = br.ReadInt32();

        _ = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();
        _ = br.ReadInt32();
    }
}
[Flags]
public enum DataFlags : int
{
    None = 0,
    CompressedImage = 1,
    CompressedData = 2,
}
