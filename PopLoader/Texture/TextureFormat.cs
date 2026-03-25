using PopLoader.DataProcessor.Rsb;

namespace PopLoader.Texture
{
    public enum PTXFormat
    {
        Abgr32,
        Rgba16,
        Rgb565,
        Rgba5551,
        Dxt5RgbaMortonBlock = 5,
        // RGBA4444_Block = 21,
        // RGB565_Block,
        // RGBA5551_Block,
        // PVRTC_4BPP_RGBA = 30,
        // PVRTC_2BPP_RGBA,
        // Rgb8Etc2,
        // DXT1_RGB = 35,
        // DXT3_RGBA,
        // DXT5_RGBA,
        // ATC_RGB,
        // ATC_RGBA4,

        /// <summary>
        /// CompressedRgb8Etc2 texture with its pixels' alpha value (in byte) appended behind the texture.
        /// <para>
        /// Size: <br/>
        /// Ceil ( <see cref="PtxInfo.Width"/> / 4 ) * ( <see cref="PtxInfo.Height"/> / 4 ) * 8 + <see cref="PtxInfo.Width"/> * <see cref="PtxInfo.Height"/>
        /// </para>
        /// </summary>
        Rgb8Etc2Alpha8 = 147,
        // PVRTC_4BPP_RGBA_A8,
        // Argb32Alpha8, // ???
        // Rgb8Etc2AlphaPalette
    }
}