## ByteInt24Trie\<TypeT\>
A string to **TypeT** dictionary stored using a trie. A node is composed of a byte character and an offset to the next neighbor node.

Definition: <br>
Node = `$char $offset` <br>
where char is `Byte` and offset is `Int24` from the start of the trie to the next neighbor in Int32(= offset*4 bytes).

Byte character 0x00 indicate that the node is a leaf node and hence is followed by **TypeT**. Note that leaf node may have neighbor (i.e. offset != 0). 

Example:
```
41 76 64 00 54 00 00 00 4C 00 00 00 41 00 00 00
53 00 00 00 45 00 00 00 53 00 00 00 5C 00 00 00
41 2E 00 00 4C 00 00 00 57 00 00 00 41 00 00 00
59 00 00 00 53 00 00 00 4C 00 00 00 4F 00 00 00
41 00 00 00 44 00 00 00 45 00 00 00 44 00 00 00
5F 00 00 00 31 22 00 00 35 00 00 00 33 00 00 00
36 00 00 00 5F 00 00 00 30 00 00 00 30 00 00 00
2E 00 00 00 50 00 00 00 54 00 00 00 58 00 00 00
00 00 00 00 00 00 00 00 37 00 00 00 36 00 00 00
38 00 00 00 5F 00 00 00 30 00 00 00 30 00 00 00
2E 00 00 00 50 00 00 00 54 00 00 00 58 00 00 00
00 00 00 00 01 00 00 00 42 75 01 00 45 C5 00 00
```

For example, the process of looking up in the trie is:
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

