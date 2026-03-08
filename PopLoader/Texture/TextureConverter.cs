using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace PopLoader.Texture;

public static class TextureConverter
{
    /// <summary>
    /// Use to <see cref="GL.LoadBindings(IBindingsContext)"/> because I can't think of a better way.
    /// </summary>
    public static NativeWindow? BlankWindow;
    /// <summary>
    /// Use to <see cref="GL.LoadBindings(IBindingsContext)"/> because I can't think of a better way.
    /// </summary>
    public static void LoadBindings()
    {
        BlankWindow ??= new NativeWindow(new NativeWindowSettings()
        {
            ClientSize = new OpenTK.Mathematics.Vector2i(1, 1),
            StartVisible = false,
            Flags = ContextFlags.Offscreen
        });
    }
    public static void PixelDataToImage<TPixel>(TPixel[] data, int width, int height, string filePath) where TPixel : unmanaged, IPixel<TPixel>
    {
        Image.LoadPixelData<TPixel>(data, width, height).Save(filePath);
    }
    /// <summary>
    /// Convert pixel data to the specified image type provided in the file extension.
    /// </summary>
    /// <typeparam name="TPixel">The pixel type</typeparam>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="filePath"></param>
    public static void PixelDataToImage<TPixel>(byte[] data, int width, int height, string filePath) where TPixel : unmanaged, IPixel<TPixel>
    {
        Image.LoadPixelData<TPixel>(MemoryMarshal.Cast<byte, TPixel>(data), width, height).Save(filePath);
    }

    /// <summary>
    /// Convert CompressedRgb8Etc2 to the specified image type provided in the file extension.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="filePath"></param>
    public static void Rgb8Etc2Alpha8ToImage(byte[] data, int width, int height, string filePath)
    {
        LoadBindings();
        int glTex = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, glTex);

        int texSize = 8 * (width / 4) * (height / 4);
        GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, InternalFormat.CompressedRgb8Etc2, width, height, 0, texSize, data);

        byte[] processedImage = new byte[height * width * 4]; // Rgba32

        GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, processedImage);

        int alphaIndex = 3;
        for (int i = texSize; i < data.Length && alphaIndex < processedImage.Length; i++)
        {
            processedImage[alphaIndex] = data[i];
            alphaIndex += 4;
        }

        GL.BindTexture(TextureTarget.Texture2D, 0);
        GL.DeleteTexture(glTex);

        PixelDataToImage<Rgba32>(processedImage, width, height, filePath);
    }

    public static void ConvertDataToImage(byte[] data, int width, int height, PTXFormat format, string filePath)
    {
        switch (format)
        {
            case PTXFormat.Abgr32:
                PixelDataToImage<Argb32>(data, width, height, filePath);
                break;
            case PTXFormat.Rgb8Etc2Alpha8:
                Rgb8Etc2Alpha8ToImage(data, width, height, filePath);
                break;
            default:
                break;
        }
    }
}