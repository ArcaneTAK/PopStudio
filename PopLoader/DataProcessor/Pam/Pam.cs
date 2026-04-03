using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using PopLoader.DataProcessor.BinaryHelper;

namespace PopLoader.DataProcessor.Pam;

public class ImageInfo
{
    public string Name;
    public Matrix2 Transform;
    public Vector2i Translation;
}

public class PopAnimation
{
    public int Version;
    public byte FrameRate;
    public Vector2i Position;
    public Vector2i Size;
    public ImageInfo[] ImageInfos;
    public const int PamHeaderMagic = -1158669996; // 0xbaf01954
    public PopAnimation(BinaryReader br)
    {
        br.ReadMagicInt32(PamHeaderMagic);
        Version = br.ReadInt32();
        if (Version <= 6) throw new NotSupportedException();

        FrameRate = br.ReadByte();
        // Position.X = br.ReadInt16() / 20d;
        // Position.Y = br.ReadInt16() / 20d;
        // Size.X = br.ReadInt16() / 20d;
        // Size.Y = br.ReadInt16() / 20d;
        // int imagesCount = br.ReadInt16();
        // ImageInfos = new ImageInfo[imagesCount];
        // for (int i = 0; i < imagesCount; i++)
        // {
        //     image[i] = new ImageInfo().Read(br, version);
        // }
        // int spritesCount = br.ReadInt16();
        // sprite = new SpriteInfo[spritesCount];
        // for (int i = 0; i < spritesCount; i++)
        // {
        //     sprite[i] = new SpriteInfo().Read(br, version);
        //     if (version < 4)
        //     {
        //         sprite[i].frame_rate = frame_rate;
        //     }
        // }
        // if (version <= 3 || br.ReadBoolean())
        // {
        //     main_sprite = new SpriteInfo().Read(br, version);
        //     if (version < 4)
        //     {
        //         main_sprite.frame_rate = frame_rate;
        //     }
        // }
        // return this;
    }
}