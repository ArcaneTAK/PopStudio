using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace PopLoader.FileConverter;

public class HeaderMagic
{
    public const int Ptx1HeaderMagic = 1886681137;  //  1xtp
    public const int RsgpHeaderMagic = 1920165744;  //  pgsr
    public const int Rsb1HeaderMagic = 1920164401;  //  1bsr
    public static int Utf8ToInt(string s) => BitConverter.ToInt32(Encoding.UTF8.GetBytes(s));
}
