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
            throw new NotImplementedException("Unsupported PTX info encoding");

        // br.BaseStream.Seek(rsbHeaderInfo.PtxInfoOffset, SeekOrigin.Begin);
        // PtxInfo[] ptxInfos = new PtxInfo[rsbHeaderInfo.PtxCount];
        // br.Read(MemoryMarshal.AsBytes(ptxInfos.AsSpan()));

        // br.BaseStream.Seek(rsbHeaderInfo.AutopoolInfoOffset, SeekOrigin.Begin);
        // RsbAutoPool[] autopoolInfo = new RsbAutoPool[rsbHeaderInfo.AutopoolCount];
        // br.Read(MemoryMarshal.AsBytes(autopoolInfo.AsSpan()));
        
        // using FileStream fi = File.OpenWrite(outFolderPath + "Autopool.txt");
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

        //         br.BaseStream.Position = rsbHeaderInfo.GroupTrieOffset + offset[last];
        //         offset.RemoveAt(last);
        //         currentname.RemoveAt(last);

        //         val = new AsciiUint24(br);
        //         currentname.Add(val.Character);
        //         offset.Add(val.Offset << 2);
        //     }
        // } while (currentname.Count > 0);
        #endregion

        #region Package
        ResoureGroupPackageInfo[] resourceGroups = new ResoureGroupPackageInfo[rsbHeaderInfo.PackageCount];
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
        #endregion

        // var groupInfoArray = new CompositeInfo[rsbHeaderInfo.GroupCount];
        // br.BaseStream.Seek(rsbHeaderInfo.GroupInfoOffset, SeekOrigin.Begin);
        // for (int i = 0; i < groupInfoArray.Length; i++)
        // {
        //     ref CompositeInfo k = ref groupInfoArray[i];
        //     k = new CompositeInfo().Read(br);
        // }

        
    }
}