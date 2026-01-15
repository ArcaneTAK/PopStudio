using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using PopLoader.BinaryHelper;
using PopLoader.FileConverter.Rsb;
using PopLoader.Texture;

namespace PopLoader.FileConverter.Rsgp;

public static class ResourceGroupPackage
{
    public static void Unpack(BinaryReader br, PtxInfo[] ptx1Infos, int startId, string path)
    {
        long Pos = br.BaseStream.Position; // Allow for use directly from Rsb
        RsgpHeader rsgpHeaderInfo = new(br);
        var info = rsgpHeaderInfo.SubgroupInfo;


        br.BaseStream.Seek(Pos + info.DataOffset, SeekOrigin.Begin);
        MemoryStream dataFile = new(br.ReadBytes(info.CompressedDataSize));
        if ((info.DataFlags & DataFlags.CompressedData) == DataFlags.CompressedData && info.DecompressedDataSize != 0)
        {
            dataFile = Zlib.Decompress(dataFile);
        }


        br.BaseStream.Seek(Pos + info.ImageOffset, SeekOrigin.Begin);
        MemoryStream imageFile = new(br.ReadBytes(info.CompressedImageSize));
        if ((info.DataFlags & DataFlags.CompressedImage) == DataFlags.CompressedImage && info.DecompressedImageSize != 0)
            imageFile = Zlib.Decompress(imageFile);

        br.BaseStream.Seek(Pos + rsgpHeaderInfo.InfoOffset, SeekOrigin.Begin);
        while (br.BaseStream.Position < Pos + rsgpHeaderInfo.InfoOffset + rsgpHeaderInfo.InfoSize)
        {
            string Name = br.ReadUTF8StringSkip3EndWithNull();
            RsgInfoType InfoType = (RsgInfoType)br.ReadInt32();
            int FileOffset = br.ReadInt32();
            uint FileSize = br.ReadUInt32();
            
            switch (InfoType)
            {
                case RsgInfoType.Data:
                    dataFile.Seek(FileOffset, SeekOrigin.Begin);
                    byte[] file = new byte[FileSize];
                    dataFile.ReadExactly(file);

                    Path.GetExtension(Name);
                    string output = path + Name;
                    Directory.CreateDirectory(Path.GetDirectoryName(output) ?? string.Empty);
                    File.WriteAllBytes(output, file);

                    break;
                case RsgInfoType.Image:
                    _ = br.ReadInt32(); // This is suppposed to be image id but is not set
                    _ = br.ReadInt32(); // This is suppposed to be image format but is not set
                    _ = br.ReadInt32();
                    int Width = br.ReadInt32();
                    int Height = br.ReadInt32();

                    PtxInfo imageInfo = ptx1Infos[startId++];

                    imageFile.Seek(FileOffset, SeekOrigin.Begin);
                    file = new byte[FileSize];
                    imageFile.ReadExactly(file);

                    output = path + Name.Replace(".PTX", ".png"); // TEMP
                    Directory.CreateDirectory(Path.GetDirectoryName(output) ?? string.Empty);
                    TextureConverter.ConvertDataToImage(file, imageInfo.Width, imageInfo.Height, imageInfo.Format, output);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported InfoType: {InfoType}");
            }
        }

        dataFile.Dispose();
        imageFile.Dispose();
    }
}
public enum RsgInfoType
{
    Data,
    Image,
}