using System.Buffers.Text;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using PopLoader.DataProcessor.BinaryHelper;

namespace PopLoader.DataProcessor.Rton;
/// <summary>
/// RTON is a binary format similar to BSON in which zero or more ordered key/value pairs are stored as a single entity.
/// P/S: Pls don't flame me for the name
/// </summary>
public class RefTextObjectNotation
{
    List<string> AsciiCache = [];
    List<string> Utf8Cache = [];
    public const int RtonHeaderMagic = 0x4e4f5452; // RTON
    public const int RtonFooterMagic = 0x454e4f44; // DONE
    public int Version;
    public StreamWriter fs;
    public RtonObject Body;
    public RefTextObjectNotation(BinaryReader br, string outPath)
    {
        fs = new StreamWriter(outPath);
        br.ReadMagicInt32(RtonHeaderMagic);
        Version = br.ReadInt32();
        Body = ReadObject(br);
        br.ReadMagicInt32(RtonFooterMagic);
        foreach (var item in Utf8Cache)
        {
            fs.WriteLine(item);
        }
    }
    
    public RtonObject ReadObject(BinaryReader br)
    {
        fs.Write("{\n");
        RtonByteCode code;
        while ((code = (RtonByteCode)br.ReadByte()) != RtonByteCode.EndObject)
        {
            ReadString(br, code);
            fs.Write(":");
            code = (RtonByteCode)br.ReadByte();
            var found = (ReadValue(br, code) is null) || ReadString(br, code) is null;
            
            fs.Write(",\n");
        }
        fs.Write("}\n");
    }

    public string? ReadString(BinaryReader br, RtonByteCode code)
    {
        switch (code)
        {
            case RtonByteCode.StringEmpty:
                return "";
            case RtonByteCode.AsciiString:
                return br.ReadString();
            case RtonByteCode.Utf8String:
                br.Read7BitEncodedInt();
                return br.ReadString();
                
            case RtonByteCode.CacheAsciiString:
                var str = br.ReadString();
                AsciiCache.Add(str);
                return str;
            case RtonByteCode.CacheUtf8String:
                br.Read7BitEncodedInt();
                str = br.ReadString();
                Utf8Cache.Add(str);
                return str;
            case RtonByteCode.RecallAsciiString:
                int index = br.Read7BitEncodedInt();
                return AsciiCache[index];
            case RtonByteCode.RecallUtf8String:
                index = br.Read7BitEncodedInt();
                return AsciiCache[index];
                
            case RtonByteCode.RTID:
                byte subCode = br.ReadByte();
                switch (subCode)
                {
                    case 0x00:
                        return "RTON()";
                    case 0x02:
                        br.Read7BitEncodedInt();
                        str = br.ReadString();
                        var u2 = br.ReadByte();
                        var u1 = br.ReadByte();
                        var id = br.ReadInt32();
                        return $"RTID({u1}.{u2}.{id}@{str})";
                    case 0x03:
                        br.Read7BitEncodedInt();
                        str = br.ReadString();
                        br.Read7BitEncodedInt();
                        var minor = br.ReadString();
                        return $"RTID({minor}@{str})";
                    default:
                        throw new NotImplementedException();
                }
            default:
                return null;
        }
    }

    public object? ReadValue(BinaryReader br, RtonByteCode code)
    {
        return code switch
        {
            RtonByteCode.False => false,
            RtonByteCode.True => true,

            RtonByteCode.Int8 => br.ReadSByte(),
            RtonByteCode.Int8Zero => (sbyte)0,
            RtonByteCode.UInt8 => br.ReadByte(),
            RtonByteCode.UInt8Zero => (byte)0,

            RtonByteCode.Int16 => br.ReadInt16(),
            RtonByteCode.Int16Zero => (short)0,
            RtonByteCode.UInt16 => br.ReadUInt16(),
            RtonByteCode.UInt16Zero => (ushort)0,

            RtonByteCode.Int32 => br.ReadInt32(),
            RtonByteCode.Int32Zero => (int)0,
            RtonByteCode.UInt32 => br.ReadUInt32(),
            RtonByteCode.UInt32Zero => (uint)0,

            RtonByteCode.Single => br.ReadSingle(),
            RtonByteCode.SingleZero => (float)0,
            RtonByteCode.Double => br.ReadDouble(),
            RtonByteCode.DoubleZero => (double)0,

            RtonByteCode.VarInt32 => br.Read7BitEncodedInt(),
            RtonByteCode.VarUInt32 => br.Read7BitEncodedUInt(),
            RtonByteCode.VarInt64 => br.Read7BitEncodedInt64(),
            RtonByteCode.VarUInt64 => br.Read7BitEncodedUInt64(),

            RtonByteCode.Zigzag32 => br.ReadVarZigZag32(),
            RtonByteCode.Zigzag64 => br.ReadVarZigZag64(),

            _ => null,
        };
    }
}
public class RtonObject
{
    public Dictionary<string, object> keyValuePairs;
    
}
public class RtonArray
{
    
}