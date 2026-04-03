using System.Runtime.InteropServices;
using System.Text;

namespace PopLoader.DataProcessor.BinaryHelper;
public class TrieNode<T>
{
    public byte Character;
    public T? Value;
    public List<TrieNode<T>> Children = [];
    public void ReadDescendants(BinaryReader br, long StartOffset, Func<BinaryReader, T> reader)
    {
        while (true)
        {
            var val = new AsciiUint24(br);
            if (val.Character == 0x00)
            {
                Value = reader(br);
            }
            else
            {
                var child = new TrieNode<T>{Character = val.Character};
                child.ReadDescendants(br, StartOffset, reader);
                Children.Add(child);
            }
            if (val.Offset == 0) return;
            br.BaseStream.Seek(StartOffset + val.Offset, SeekOrigin.Begin);
        }
    }
}

public class ByteUInt24Trie<T>
{
    public TrieNode<T> RootNode;
    public ByteUInt24Trie(BinaryReader br, Func<BinaryReader, T> reader){
        RootNode = new TrieNode<T>();
        if (br.PeekChar() == 0x00) return;
        RootNode.ReadDescendants(br, br.BaseStream.Position, reader);
    }
}

public class ByteUInt24Trie
{
    public static void ReadWithAction(BinaryReader br, Action<string, BinaryReader> readAction)
    {
        long start = br.BaseStream.Position;
        var val = new AsciiUint24(br);
        if (val.Character == 0x00) return;

        List<byte> currentname = []; List<int> offset = [];
        currentname.Add(val.Character);
        offset.Add(val.Offset << 2);

        while (currentname.Count > 0)
        {
            val = new AsciiUint24(br);
            currentname.Add(val.Character);
            offset.Add(val.Offset << 2);
            if (val.Character == 0x00)
            {
                int last = offset.Count - 1;
                string name = Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(currentname).Slice(0, last));

                readAction(name, br);

                while (last >= 0 && offset[last] == 0)
                {
                    offset.RemoveAt(last);
                    currentname.RemoveAt(last);
                    last--;
                }

                if (last < 0) break;

                br.BaseStream.Position = start + offset[last];
                offset.RemoveAt(last);
                currentname.RemoveAt(last);

                val = new AsciiUint24(br);
                currentname.Add(val.Character);
                offset.Add(val.Offset << 2);
            }
        }
        return;
    }
}