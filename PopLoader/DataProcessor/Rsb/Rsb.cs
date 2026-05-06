using System.Runtime.InteropServices;
using System.Text;
using PopLoader.DataProcessor.BinaryHelper;
using PopLoader.DataProcessor.Rsgp;
using PopLoader.Texture;
namespace PopLoader.DataProcessor.Rsb;

public class ResourceBinary
{
    public RsbHeader Header;
    public PtxInfo[] TextureInfo;
    public ResourceBinary(BinaryReader br)
    {
        Header = new RsbHeader(br);

        if (Header.PtxInfoSize != 16)
            throw new NotImplementedException("Unsupported PTX info encoding");

        #region PtxInfo
            br.BaseStream.Seek(Header.PtxInfoOffset, SeekOrigin.Begin);
            TextureInfo = new PtxInfo[Header.PtxCount];
            br.Read(MemoryMarshal.AsBytes(TextureInfo.AsSpan()));   
        #endregion
        
        #region PackageTrie
        // br.BaseStream.Seek(rsbHeaderInfo.PackageTrieOffset, SeekOrigin.Begin);
        // using FileStream fi = File.OpenWrite(outFolderPath + "Package.txt");
        // using BinaryWriter sbr = new BinaryWriter(fi);
        // List<byte> currentname = []; List<int> offset = [];
        // AsciiUint24 val;
        // do
        // {
        //     val = new AsciiUint24(br);
        //     currentname.Add(val.Character);
        //     offset.Add(val.Offset << 2);
        //     if (val.Character == 0x00)
        //     {
        //         int last = offset.Count - 1;
        //         int PackageID = br.ReadInt32();
        //         string name = Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(currentname).Slice(0, last));
                
        //         sbr.Write(Encoding.UTF8.GetBytes($"{name} {PackageID}\n"));
                
        //         while (last >= 0 && offset[last] == 0)
        //         {
        //             offset.RemoveAt(last);
        //             currentname.RemoveAt(last);
        //             last--;
        //         }

        //         if (last < 0) break;

        //         br.BaseStream.Position = rsbHeaderInfo.PackageTrieOffset + offset[last];
        //         offset.RemoveAt(last);
        //         currentname.RemoveAt(last);

        //         val = new AsciiUint24(br);
        //         currentname.Add(val.Character);
        //         offset.Add(val.Offset << 2);
        //     }
        // } while (currentname.Count > 0);
        #endregion

        #region GroupTrie
        // br.BaseStream.Seek(rsbHeaderInfo.GroupTrieOffset, SeekOrigin.Begin);
        // using FileStream fi = File.OpenWrite(outFolderPath + "Group.txt");
        // using StreamWriter sw = new StreamWriter(fi);
        // ByteUInt24Trie.ReadWithAction(br, (name, subbr) =>
        // {
        //     sw.Write($"{name} {subbr.ReadInt32()}\n");
        // });
        #endregion

        #region Package
        // ResoureGroupPackageInfo[] resourceGroups = new ResoureGroupPackageInfo[rsbHeaderInfo.PackageCount];
        // br.BaseStream.Seek(rsbHeaderInfo.PackageInfoOffset, SeekOrigin.Begin);
        // for (int i = 0; i < rsbHeaderInfo.PackageCount; i++)
        // {
        //     var resourceGroup = new ResoureGroupPackageInfo(br);
        //     long startPos = br.BaseStream.Position;
        //     br.BaseStream.Seek(resourceGroup.Offset, SeekOrigin.Begin);
        //     var package = new ResourceGroupPackage(br);
        //     foreach ((string filename, RsgpFileInfo fileinfo) in package.PackageFileInfo)
        //     {
        //         System.Console.WriteLine(resourceGroup.Name+" "+filename);
        //         // string output = outFolderPath + filename;
        //         // byte[] file = new byte[fileinfo.FileSize];
        //         // switch (fileinfo.FileType)
        //         // {
        //         //     case RsgInfoType.Data:
        //         //         // package.DynamicDataStream.Seek(fileinfo.FileOffset, SeekOrigin.Begin);
        //         //         // package.DynamicDataStream.ReadExactly(file);
        //         //         // Directory.CreateDirectory(Path.GetDirectoryName(output) ?? "");
        //         //         // File.WriteAllBytes(output, file);
        //         //         break;
        //         //     case RsgInfoType.Image:
        //         //         PtxInfo imageInfo = ptxInfos[resourceGroup.StartImageId + package.ImageInfo[filename].ImageIndexInPackage];
        //         //         package.ImageStream.Seek(fileinfo.FileOffset, SeekOrigin.Begin);
        //         //         package.ImageStream.ReadExactly(file);
        //         //         Directory.CreateDirectory(Path.GetDirectoryName(output) ?? "");
        //         //         output = output.Replace(".PTX", ".png");
        //         //         TextureConverter.ConvertDataToImage(file, imageInfo.Width, imageInfo.Height, imageInfo.Format, output);
        //         //         break;
        //         //     default:
        //         //         break;
        //         // }

        //     }
        //     br.BaseStream.Seek(startPos, SeekOrigin.Begin);
        // }
        #endregion


        #region CompositeInfo
        // var groupInfoArray = new CompositeInfo[Header.GroupCount];
        // br.BaseStream.Seek(Header.GroupInfoOffset, SeekOrigin.Begin);
        // for (int i = 0; i < groupInfoArray.Length; i++)
        // {
        //     ref CompositeInfo k = ref groupInfoArray[i];
        //     k = new CompositeInfo(br);
        // }
        // #endregion

        // #region Autopool
        // br.BaseStream.Seek(Header.AutopoolInfoOffset, SeekOrigin.Begin);
        // RsbAutoPool[] autopoolInfo = new RsbAutoPool[Header.AutopoolCount];
        // br.Read(MemoryMarshal.AsBytes(autopoolInfo.AsSpan()));
        
        // using FileStream fi = File.OpenWrite("Autopool.txt");
        // using BinaryWriter sbr = new BinaryWriter(fi);

        // HashSet<int> maxoffset = [];
        // HashSet<int> maxsize = [];
        // HashSet<int> types = [];

        // for (int i = 0; i < autopoolInfo.Length; i++)
        // {
        //     RsbAutoPool pool = autopoolInfo[i];
        //     var st = Encoding.UTF8.GetString(pool.ID[..]).TrimEnd('\x00');
        //     var str =
        //     st + $" {pool.DecompressedData} {pool.DecompressedImage} {pool.type}\n";
        //     sbr.Write(Encoding.UTF8.GetBytes(str));
        //     types.Add(pool.type);
        //     maxoffset.Add(pool.DecompressedData);
        //     maxsize.Add(pool.DecompressedImage);
        // }
        // sbr.Write(Encoding.UTF8.GetBytes("\n" + nameof(maxoffset) + "\n"));
        // foreach (var item in maxoffset)
        //     sbr.Write(Encoding.UTF8.GetBytes(item.ToString() + " "));
        
        // sbr.Write(Encoding.UTF8.GetBytes("\n" + nameof(maxsize) + "\n"));
        // foreach (var item in maxsize)
        //     sbr.Write(Encoding.UTF8.GetBytes(item.ToString() + " "));
        
        // sbr.Write(Encoding.UTF8.GetBytes("\n" + nameof(types) + "\n"));
        // foreach (var item in types)
        //     sbr.Write(Encoding.UTF8.GetBytes(item.ToString() + " "));
        #endregion
    }
}