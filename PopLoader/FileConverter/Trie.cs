using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using PopLoader.BinaryHelper;

namespace PopLoader.FileConverter;

public class AsciiUint24Trie<TValue> : IReadable<AsciiUint24Trie<TValue>>, IWritable where TValue : IReadable<TValue>, IWritable
{
    public class TrieNode
    {
        public SortedList<byte, TrieNode> Children = [];
        public TValue? Item;
    }
    
    public TrieNode Root;
    public static AsciiUint24Trie<TValue> Read(BinaryReader reader)
    {
        TrieNode root = new TrieNode();
        List<TrieNode> currentNodes = [root];
        // List<byte> characters = [0];
        List<int> offsets = [0];
        do
        {
            (byte c, int o) = AsciiUint24.Read(reader);

            if (c == 0x00)
            {
                int last = offsets.Count - 1;
                currentNodes[last].Item = TValue.Read(reader);
                while (offsets[last] == 0)
                {
                    offsets.RemoveAt(last);
                    // characters.RemoveAt(last);
                    currentNodes.RemoveAt(last);
                    last--;
                }
                if (offsets.Count > 0)
                {
                    offsets.RemoveAt(last);
                    // characters.RemoveAt(last);
                    currentNodes.RemoveAt(last);
                }
                continue;
            }
            
            // characters.Add(c);
            offsets.Add(o);
            TrieNode node = new TrieNode();
            currentNodes[^1].Children.Add(c, node);
            
        } while (offsets.Count > 0);
        return new AsciiUint24Trie<TValue>(){Root = root};
    }

    public void Write(BinaryWriter writer)
    {
        List<TrieNode> currentNodes = [Root];
        List<int> offsets = [0];
        do
        {
            (byte c, int o) = AsciiUint24.Write(reader);

            if (c == 0x00)
            {
                int last = offsets.Count - 1;
                currentNodes[last].Item = TValue.Read(writer);
                while (offsets[last] == 0)
                {
                    offsets.RemoveAt(last);
                    // characters.RemoveAt(last);
                    currentNodes.RemoveAt(last);
                    last--;
                }
                if (offsets.Count > 0)
                {
                    offsets.RemoveAt(last);
                    // characters.RemoveAt(last);
                    currentNodes.RemoveAt(last);
                }
                continue;
            }
            
            // characters.Add(c);
            offsets.Add(o);
            TrieNode node = new TrieNode();
            currentNodes[^1].Children.Add(c, node);
            
        } while (offsets.Count > 0);
    }
}
