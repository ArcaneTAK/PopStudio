using PopLoader.DataProcessor.Rsb;
Directory.SetCurrentDirectory(@"..\..\..\..");
// FileStream fs = new FileStream(@"Temp\Obb\ATLASES\ALWAYSLOADED_1536_00.PTX", FileMode.Open, FileAccess.Read);
ResourceBinary.Unpack(@"main.675.com.ea.game.pvz2_sol.obb", @"Temp\");