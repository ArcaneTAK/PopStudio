using PopLoader.FileConverter.Rsgp;
namespace PopLoader.FileConverter.Rsb;

public static class ResourceBinary
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
        PtxInfo[] ptxInfos = new PtxInfo[rsbHeaderInfo.PtxCount];
        br.BaseStream.Seek(rsbHeaderInfo.PtxInfoOffset, SeekOrigin.Begin);
        for (int i = 0; i < ptxInfos.Length; i++)
        {
            ptxInfos[i] = new PtxInfo(br);
        }

        ResoureGroupHeader[] resourceGroups = new ResoureGroupHeader[rsbHeaderInfo.SubgroupCount];
        br.BaseStream.Seek(rsbHeaderInfo.SubgroupInfoOffset, SeekOrigin.Begin);
        for (int i = 0; i < rsbHeaderInfo.SubgroupCount; i++)
        {
            resourceGroups[i] = new ResoureGroupHeader(br);
            long startPos = br.BaseStream.Position;
            br.BaseStream.Seek(resourceGroups[i].Offset, SeekOrigin.Begin);
            ResourceGroupPackage.Unpack(br, ptxInfos, resourceGroups[i].StartImageId, outFolderPath);
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