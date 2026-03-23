## ByteInt24Trie\<TypeT\>

A string to **TypeT** dictionary stored using a trie. A node is composed of a byte character and an offset to the next neighbor node. 

The offset is the offset in int32; to convert to byte, multiply it by 4. Offset equal to zero (0) means that it does not have a neighbor after it.

Byte character 0x00 indicate that the node is a leaf node and hence is followed by **TypeT**. Note that leaf node may have neighbor (i.e. offset != 0). 

Forexample, the process of looking up in the trie is:
```csharp
byte nodeByte = ReadByte();
Int24 offsetNextCousin = ReadInt24() * 4;
if (nodeByte == 0x00) {
    if (index == lookUpString.Length) return Read<TypeT>();
    else goto notFound;
}
else if (nodeByte == lookUpString[index]) {
    index++;
    goto repeat;
}
else {
    if (offsetNextCousin != 0) {
        Stream.Position = Trie.Start + offsetNextCousin;
        goto repeat;
    }
    else goto notFound;
}
```

## IndexTrie

A lookup trie for indexes. IndexTrie = ByteInt24Trie\<int\>.

