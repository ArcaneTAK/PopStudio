using System.Text;
using PopLoader.Texture;

namespace PopLoader.FileConverter.Ptx;

public class PopTexture
{
    public void ConvertPtx1ToImage(string inFilePath, string outFilePath)
    {
        FileStream fs = new FileStream(inFilePath, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        PtxHeader ptx1Header = new(br);

        TextureConverter.ConvertDataToImage(br.ReadBytes((int)(br.BaseStream.Length - 32)), ptx1Header.Width, ptx1Header.Height, ptx1Header.Format, outFilePath);
    }
}
