using System.Buffers.Text;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32.SafeHandles;
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
    public Dictionary<string, object> Body;
    public RefTextObjectNotation(BinaryReader br)
    {
        br.ReadMagicInt32(RtonHeaderMagic);
        Version = br.ReadInt32();
        Body = ReadObject(br);
        br.ReadMagicInt32(RtonFooterMagic);
    }

    public void WriteJson(string outPath)
    {
        using StreamWriter sw = new StreamWriter(outPath);
        sw.Write(JsonSerializer.Serialize(Body));
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
                var readstring = br.ReadString();
                AsciiCache.Add(readstring);
                return readstring;
            case RtonByteCode.CacheUtf8String:
                br.Read7BitEncodedInt();
                readstring = br.ReadString();
                Utf8Cache.Add(readstring);
                return readstring;
            case RtonByteCode.RecallAsciiString:
                int index = br.Read7BitEncodedInt();
                return AsciiCache[index];
            case RtonByteCode.RecallUtf8String:
                index = br.Read7BitEncodedInt();
                return AsciiCache[index];
            
            default:
                return null;
        }
    }

    public object? ReadElement(BinaryReader br, RtonByteCode code)
    {
        switch (code)
        {
            case RtonByteCode.False : return false;
            case RtonByteCode.True : return true;

            case RtonByteCode.Int8 : return br.ReadSByte();
            case RtonByteCode.Int8Zero : return (sbyte)0;
            case RtonByteCode.UInt8 : return br.ReadByte();
            case RtonByteCode.UInt8Zero : return (byte)0;

            case RtonByteCode.Int16 : return br.ReadInt16();
            case RtonByteCode.Int16Zero : return (short)0;
            case RtonByteCode.UInt16 : return br.ReadUInt16();
            case RtonByteCode.UInt16Zero : return (ushort)0;

            case RtonByteCode.Int32 : return br.ReadInt32();
            case RtonByteCode.Int32Zero : return (int)0;
            case RtonByteCode.UInt32 : return br.ReadUInt32();
            case RtonByteCode.UInt32Zero : return (uint)0;

            case RtonByteCode.Single : return br.ReadSingle();
            case RtonByteCode.SingleZero : return (float)0;
            case RtonByteCode.Double : return br.ReadDouble();
            case RtonByteCode.DoubleZero : return (double)0;

            case RtonByteCode.VarInt32 : return br.Read7BitEncodedInt();
            case RtonByteCode.VarUInt32 : return br.Read7BitEncodedUInt();
            case RtonByteCode.VarInt64 : return br.Read7BitEncodedInt64();
            case RtonByteCode.VarUInt64 : return br.Read7BitEncodedUInt64();

            case RtonByteCode.Zigzag32 : return br.ReadVarZigZag32();
            case RtonByteCode.Zigzag64 : return br.ReadVarZigZag64();
            
            case RtonByteCode.Array : return ReadArray(br);
            case RtonByteCode.Object : return ReadObject(br);

            case RtonByteCode.StringEmpty:
            case RtonByteCode.AsciiString:
            case RtonByteCode.Utf8String:
            case RtonByteCode.CacheAsciiString:
            case RtonByteCode.CacheUtf8String:
            case RtonByteCode.RecallAsciiString:
            case RtonByteCode.RecallUtf8String:
                return ReadString(br, code);

            case RtonByteCode.BinaryString:
                br.ReadByte();
                string s = br.ReadString();
                int i = br.Read7BitEncodedInt();
                return $"$BINARY(\"{s}\", {i})";
            case RtonByteCode.RTIDZero:
                return "RTID()";
            case RtonByteCode.RTID:
                byte subCode = br.ReadByte();
                switch (subCode)
                {
                    case 0x00:
                        return "RTID()";
                    case 0x03:
                        br.Read7BitEncodedInt();
                        var package = br.ReadString();
                        br.Read7BitEncodedInt();
                        var property = br.ReadString();
                        return $"RTID({property}@{package})";
                    case 0x02:
                        br.Read7BitEncodedInt();
                        property = br.ReadString();
                        var u2 = br.ReadByte();
                        var u1 = br.ReadByte();
                        var id = br.ReadInt32();
                        return $"RTID({u1}.{u2}.{id}@{property})";
                    case 0x01:
                        u2 = br.ReadByte();
                        u1 = br.ReadByte();
                        id = br.ReadInt32();
                        br.Read7BitEncodedInt();
                        return $"RTID({u1}.{u2}.{id}@)";
                    default:
                        throw new NotImplementedException();
                }
            default: return null;
        };
    }

    public Dictionary<string, object> ReadObject(BinaryReader br)
    {
        Dictionary<string, object> res = [];
        RtonByteCode code;
        while ((code = (RtonByteCode)br.ReadByte()) != RtonByteCode.EndObject)
        {
            var key = ReadString(br, code);
            code = (RtonByteCode)br.ReadByte();
            var value = ReadElement(br, code);
            if (key is null || value is null) throw new Exception();
            res.Add(key, value);
        }
        return res;
    }
    public object[] ReadArray(BinaryReader br)
    {
        int index = 0;
        if ((RtonByteCode)br.ReadByte() != RtonByteCode.StartArray) throw new Exception();
        int length = br.Read7BitEncodedInt();
        object[] res = new object[length];
        RtonByteCode code;
        while ((code = (RtonByteCode)br.ReadByte()) != RtonByteCode.EndArray)
        {
            var value = ReadElement(br, code);
            if (value is null) throw new Exception();
            res[index] = value;
            index++;
        }
        return res;
    }
}