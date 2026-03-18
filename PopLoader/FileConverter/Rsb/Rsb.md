# Rsb (ResourceBinary)



> The name "ResourceBinary" is not the official name. I came up with it to make developement easier by avoiding excessive use of strange acronym. Many of the components making up it will also be "deabbrivated".

## Basic

__ResourceBinary__ file contain information require for fast lookup of files. __ResourceBinary__ contains:
```csharp
public class Rsb{
    // Not original order
    Header header; //0
    IndexTrie FileTrie; //1
    IndexTrie PackageTrie; //2
    IndexTrie GroupTrie; //4
    PackageInfo[] PackageInfo; //5
    GroupInfo[] GroupInfo; //3 also called composite shell
    AutopoolInfo[] AutopoolInfo; //6
    TextureInfo[] TextureInfo; //7
    Package[] Packages; //8
}
```

## Rsb Header

```
Offset  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F

0x00    31 62 73 72 04 00 00 00 00 00 00 00 00 B0 28 00
        1  b  s  r  ·  ·  ·  ·  ·  ·  ·  ·  ·  °  (  ·  
0x10    6C 98 06 00 70 00 00 00 00 00 00 00 00 00 00 00
        l  ·  ·  ·  p  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  
0x20    B8 68 01 00 DC 98 06 00 14 09 00 00 4C A7 1B 00
        ¸  h  ·  ·  Ü  ·  ·  ·  ·  ·  ·  ·  L  §  ·  ·  
0x30    CC 00 00 00 1F 04 00 00 94 01 08 00 84 04 00 00
        Ì  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  
0x40    BC 09 01 00 90 9D 1A 00 14 09 00 00 3C E3 22 00
        ¼  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  <  ã  "  ·  
0x50    98 00 00 00 C4 05 00 00 1C 47 28 00 10 00 00 00
        ·  ·  ·  ·  Ä  ·  ·  ·  ·  G  (  ·  ·  ·  ·  ·  
0x60    00 00 00 00 00 00 00 00 00 00 00 00 00 B0 28 00
        ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  °  (  ·
```

Offset | Size | Usage | Example or Comment
---|---|---|---
0x00 | 4 | header magic | must be "1bsr"
_ | 4 | version number | 4
_ | 4 | reserved | 0
_ | 4 | rsgp offset | 2666496
0x10 | 4 | file trie size | 432236
_ | 4 | file trie offset | 112
_ | 4 | reserved | 0
_ | 4 | reserved | 0
0x20 | 4 | rsgp trie size | 92344
_ | 4 | rsgp trie offset | 432348
_ | 4 | rsgp count | 2324
_ | 4 | rsgp info list offset | 1812300
0x30 | 4 | rsgp info size | 204
_ | 4 | comp count | 1055
_ | 4 | comp info list offset | 524692
_ | 4 | comp info size | 1156
0x40 | 4 | comp trie size | 68028
_ | 4 | comp trie offset | 1744272
_ | 4 | autopool count | 2324
_ | 4 | autopool offset | 2286396
0x50 | 4 | autopool size | 152
_ | 4 | ptx count | 1476
_ | 4 | ptx info offset | 2639644
_ | 4 | ptx size | 16
0x60 | 4 | reserve | 0
_ | 4 | reserve | 0
104 | 4 | reserve | 0
108 | 4 | rsgp offset | 2666496


# Trie<FileInfoType>
[ASCII_Int24_Int32]