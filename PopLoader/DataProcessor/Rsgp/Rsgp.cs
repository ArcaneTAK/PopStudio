using System.Runtime.InteropServices;
using System.Text;
using PopLoader;
using PopLoader.DataProcessor.BinaryHelper;
using PopLoader.DataProcessor.Rsb;

namespace PopLoader.DataProcessor.Rsgp;

public class RsgpFileInfo
{
    public RsgInfoType FileType;
    public int FileOffset;
    public uint FileSize;

    public RsgpFileInfo(BinaryReader br)
    {
        FileType = (RsgInfoType)br.ReadInt32();
        FileOffset = br.ReadInt32();
        FileSize = br.ReadUInt32();
    }
}

public class RsgpImageInfo
{
    public int ImageIndexInPackage;
    public int Width;
    public int Height;

    public RsgpImageInfo(BinaryReader br)
    {
        ImageIndexInPackage = br.ReadInt32(); // This is suppposed to be image id but is not set
        _ = br.ReadInt32();
        _ = br.ReadInt32();
        Width = br.ReadInt32();
        Height = br.ReadInt32();
    }
}

public class ResourceGroupPackage : IDisposable
{
    public RsgpHeader Header;
    public Dictionary<string, RsgpFileInfo> PackageFileInfo;
    public Dictionary<string, RsgpImageInfo> ImageInfo;
    public MemoryStream DynamicDataStream;
    public MemoryStream ImageStream;
    public ResourceGroupPackage(BinaryReader br)
    {
        long Pos = br.BaseStream.Position;
        Header = new(br);

        ImageInfo = [];
        PackageFileInfo = [];

        br.BaseStream.Seek(Pos + Header.TrieOffset, SeekOrigin.Begin);

        // Read File Info Trie
        
        ByteUInt24Trie.ReadWithAction(br, (name, subbr) =>
        {
            RsgpFileInfo fileInfo = new(subbr);
            switch (fileInfo.FileType)
            {
                case RsgInfoType.Data:
                    PackageFileInfo.Add(name, fileInfo);
                    break;
                case RsgInfoType.Image:
                    PackageFileInfo.Add(name, fileInfo);
                    ImageInfo.Add(name, new RsgpImageInfo(br));
                    break;
                default:
                    break;
            } 
        });

        br.BaseStream.Seek(Pos + Header.DataOffset, SeekOrigin.Begin);
        DynamicDataStream = new(br.ReadBytes(Header.DataBlobSize));
        if (Header.DataFlags.HasFlag(DataFlags.CompressedData) && Header.DecompressedDataSize != 0)
            DynamicDataStream = Zlib.Decompress(DynamicDataStream);

        br.BaseStream.Seek(Pos + Header.ImageOffset, SeekOrigin.Begin);
        ImageStream = new(br.ReadBytes(Header.CompressedImageSize));
        if (Header.DataFlags.HasFlag(DataFlags.CompressedImage) && Header.DecompressedImageSize != 0)
            ImageStream = Zlib.Decompress(ImageStream);
    }

    public void Dispose()
    {
        DynamicDataStream.Dispose();
        ImageStream.Dispose();
        GC.SuppressFinalize(this);
    }
}
public enum RsgInfoType
{
    Data,
    Image,
}