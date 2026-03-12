using System.Runtime.InteropServices;
using System.Text;
using PopLoader.FileConverter.Rsgp;
using PopLoader.Texture;
namespace PopLoader.FileConverter.Rsb;

public class ResourceBinary
{
    
    public static void Unpack(string filePath, string outFolderPath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        using BinaryReader br = new(fileStream);

        RsbHeader rsbHeaderInfo = new(br);

        if (rsbHeaderInfo.PtxInfoSize != 16)
        {
            throw new NotImplementedException("Unsupported PTX info encoding");
        }

        br.BaseStream.Seek(rsbHeaderInfo.PtxInfoOffset, SeekOrigin.Begin);
        PtxInfo[] ptxInfos = new PtxInfo[rsbHeaderInfo.PtxCount];
        br.Read(MemoryMarshal.AsBytes(ptxInfos.AsSpan()));

        // ResoureGroupPackageInfo[] resourceGroups = new ResoureGroupPackageInfo[rsbHeaderInfo.PackageCount];
        br.BaseStream.Seek(rsbHeaderInfo.PackageInfoOffset, SeekOrigin.Begin);
        for (int i = 0; i < rsbHeaderInfo.PackageCount; i++)
        // for (int i = 0; i < 40; i++)
        {
            var resourceGroup = new ResoureGroupPackageInfo(br);
            long startPos = br.BaseStream.Position;
            br.BaseStream.Seek(resourceGroup.Offset, SeekOrigin.Begin);
            var package = new ResourceGroupPackage(br);
            foreach ((string filename, RsgpFileInfo fileinfo) in package.PackageFileInfo)
            {
                switch (fileinfo.FileType)
                {
                    case RsgInfoType.Data:
                        package.DynamicDataStream.Seek(fileinfo.FileOffset, SeekOrigin.Begin);
                        byte[] file = new byte[fileinfo.FileSize];
                        package.DynamicDataStream.ReadExactly(file);
                        string output = outFolderPath + filename;
                        Directory.CreateDirectory(Path.GetDirectoryName(output) ?? "");
                        File.WriteAllBytes(output, file);
                        break;
                    case RsgInfoType.Image:
                        // PtxInfo imageInfo = ptxInfos[resourceGroup.StartImageId + package.ImageInfo[filename].ImageIndexInPackage];
                        // package.ImageStream.Seek(fileinfo.FileOffset, SeekOrigin.Begin);
                        // file = new byte[fileinfo.FileSize];
                        // package.ImageStream.ReadExactly(file);
                        // output = outFolderPath + filename.Replace(".PTX", ".png");
                        // Directory.CreateDirectory(Path.GetDirectoryName(output) ?? "");
                        // TextureConverter.ConvertDataToImage(file, imageInfo.Width, imageInfo.Height, imageInfo.Format, output);
                        break;
                    default:
                        break;
                }


            }
            br.BaseStream.Seek(startPos, SeekOrigin.Begin);
        }

        // var groupInfoArray = new CompositeInfo[rsbHeaderInfo.GroupCount];
        // br.BaseStream.Seek(rsbHeaderInfo.GroupInfoOffset, SeekOrigin.Begin);
        // for (int i = 0; i < groupInfoArray.Length; i++)
        // {
        //     ref CompositeInfo k = ref groupInfoArray[i];
        //     k = new CompositeInfo().Read(br);
        // }

        // var autopoolInfo = new RsbAutoPoolInfo[rsbHeaderInfo.AutopoolCount];
        // br.BaseStream.Seek(rsbHeaderInfo.AutopoolInfoOffset, SeekOrigin.Begin);
        // for (int i = 0; i < autopoolInfo.Length; i++)
        // {
        //     autopoolInfo[i] = new RsbAutoPoolInfo().Read(br);
        // }

        // if (xmlPart1_BeginOffset != 0)
        // {
        // //     XmlPack xmlPack = new XmlPack((int)rsb.head.xmlPart1_BeginOffset, (int)rsb.head.xmlPart2_BeginOffset, (int)rsb.head.xmlPart3_BeginOffset).Read(br); // read xml
        // }
    }
}