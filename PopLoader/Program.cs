using PopLoader.DataProcessor.Rsb;
Directory.SetCurrentDirectory(@"..\..\..\..");
// FileStream fs = new FileStream(@"Temp\Obb\ATLASES\ALWAYSLOADED_1536_00.PTX", FileMode.Open, FileAccess.Read);
// ResourceBinary.Unpack(@"main.675.com.ea.game.pvz2_sol.obb", @"Temp\");

// var myPath = @"Temp\PACKAGES\LEVELS\WARP\PARTY142.RTON";
// var outPath = @"Temp\PARTY155.RTON";

object a = "string";
int b = (int)a;
Console.WriteLine(b);

// var myPath = @"Temp\PACKAGES";
// CheckAll(myPath);
// static void CheckAll(string path)
// {
//     foreach (var pathDir in Directory.GetDirectories(path))
//     {
//         CheckAll(pathDir);
//     }
//     foreach (var pathFile in Directory.GetFiles(path))
//     {
//         byte[] data = File.ReadAllBytes(pathFile);
//         for (int i = 1; i < data.Length; i++)
//         {
//             if (data[i] == 0x83 && data[i + 1] == 0x00)
//             {
//                 Console.WriteLine(pathFile + "    " + i.ToString());
//                 break;
//             }
//         }
//     }
// }