using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PopLoader.DataProcessor.BinaryHelper;

namespace PopLoader.DataProcessor.Rsb
{
    /// <summary>
    /// 128 + 64 * 16 + 4 = 1156 bytes
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GroupInfo
    {
        public CString128 ID;
        public GroupPackage child_Info;
        public int child_Number;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class GroupPackageInfo
    {
        public int PackageIndex;
        public int ImportanceRatio;
        public int language; // Optional
        private int _filler;
    }

    [InlineArray(64)]
    public struct GroupPackage
    {
        public GroupPackageInfo PackageInfo;
    }
}
