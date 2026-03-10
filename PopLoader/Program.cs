using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using PopLoader.FileConverter.Rsb;
using PopLoader.Texture;
Directory.SetCurrentDirectory(@"..\..\..");
// // FileStream fs = new FileStream(@"Temp\Obb\ATLASES\ALWAYSLOADED_1536_00.PTX", FileMode.Open, FileAccess.Read);

ResourceBinary.Unpack(@"main.675.com.ea.game.pvz2_sol.obb", @"Temp\New\");
