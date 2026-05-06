using System.Runtime.CompilerServices;
using System.Text;

namespace PopLoader.DataProcessor.BinaryHelper;

[InlineArray(128)]
public struct CString128
{
    public byte data;
    public override string ToString()
    {
        return Encoding.UTF8.GetString(this);
    }
}
