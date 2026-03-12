using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK.Windowing.Common.Input;
using PopLoader.BinaryHelper;
using PopLoader.FileConverter.Rsb;
using PopLoader.Texture;

namespace PopLoader.FileConverter.Rsgp;

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

        List<byte> currentname = []; List<int> offset = [];
        var val = new AsciiUint24(br);
        if (val.Character == 0x00) return;
        do
        {
            if (val.Character != 0x00)
            {
                currentname.Add(val.Character);
                offset.Add(val.Offset << 2);
                continue;
            }

            RsgpFileInfo fileInfo = new(br);
            switch (fileInfo.FileType)
            {
                case RsgInfoType.Data:
                    PackageFileInfo.Add(BinaryReaderHelper.ListByteToString(currentname), fileInfo);
                    break;
                case RsgInfoType.Image:
                    PackageFileInfo.Add(BinaryReaderHelper.ListByteToString(currentname), fileInfo);
                    ImageInfo.Add(BinaryReaderHelper.ListByteToString(currentname), new RsgpImageInfo(br));
                    break;
                default:
                    break;
            }
            if (currentname.Count == 0) throw new NotImplementedException();

            int last = offset.Count - 1;
            while (last >= 0 && offset[last] == 0x00)
            {
                offset.RemoveAt(last);
                currentname.RemoveAt(last);
                last--;
            }

            if (last < 0) break;

            br.BaseStream.Position = Pos + Header.TrieOffset + offset[last];
            offset.RemoveAt(last);
            currentname.RemoveAt(last);
            val = new AsciiUint24(br);

        } while (offset.Count > 0);

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