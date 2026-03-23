using System.Runtime.InteropServices.JavaScript;

namespace PopLoader.FileConverter.Rton;

public class Rton
{
    
}

public enum RtonByteCode
{
    False = 0x00,
    True = 0x01,
    Int8 = 0x08,
    Int8Zero = 0x09,
    UInt8 = 0x0a,
    UInt8Zero = 0x0b,
    Int16 = 0x10,
    Int16Zero = 0x11,
    UInt16 = 0x12,
    UInt16Zero = 0x13,
    Int32 = 0x20,
    Int32Zero = 0x21,
    Single = 0x22,
    SingleZero = 0x23,
    VarInt32 = 0x24,
    Zigzag32 = 0x25,
    UInt32 = 0x26,
    UInt32Zero = 0x27,
    VarUInt32 = 0x28,
    Int64 = 0x40,
    Int64Zero = 0x41,
    Double = 0x42,
    DoubleZero = 0x43,
    VarInt64 = 0x44,
    Zigzag64 = 0x45,
    UInt64 = 0x46,
    UInt64Zero = 0x47,
    VarUInt64 = 0x48,
    String = 0x02,
    AsciiString = 0x81,
    Utf8String = 0x82,
    RTID = 0x83,
    RTIDZero = 0x84,
    BinaryString = 0x87,
    CacheByteString = 0x90,
    RecallByteString = 0x91,
    CacheUtf8String = 0x92,
    RecallUtf8String = 0x93,
    Object = 0x85,
    Array = 0x86
}