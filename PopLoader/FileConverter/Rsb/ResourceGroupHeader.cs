using PopLoader.FileConverter.Rsgp;

namespace PopLoader.FileConverter.Rsb;

/// <summary>
/// Not to be confused with <see cref="RsgpHeader"/>
/// </summary>
public class ResoureGroupHeader
{
    public string Name;
    public int Offset;
    public int Size;
    /// <summary>
    /// The index of this subgroup in the pool.
    /// </summary>
    public int Id;
    public ResourceGroupInfo GroupInfo;

    public int ImageCount;
    /// <summary>
    /// The image Id of the first image (if there exists one) in the package, images after have their Id increament subsequently.
    /// Can be understood as the number of images in packages before this one.
    /// </summary>
    public int StartImageId;
    public ResoureGroupHeader(BinaryReader br)
    {
        Name = new string(br.ReadChars(128)).Replace("\0", null);
        Offset = br.ReadInt32();
        Size = br.ReadInt32();
        Id = br.ReadInt32();

        GroupInfo = new ResourceGroupInfo(br);

        ImageCount = br.ReadInt32();
        StartImageId = br.ReadInt32();
    }
}
