# Rsb (ResourceBinary)



> The name "ResourceBinary" is not the official name. I came up with it to make developement easier by avoiding excessive use of strange acronym. Many of the components making up it will also be "deabbrivated".

## Basic

__ResourceBinary__ file contain information require for fast lookup of files. __ResourceBinary__ contains:
> Rsb Records:
>
> Rsb Header (size 112 = 0x70)
>
> File Name to Package ID Trie
> Package Name to Package ID
> Composite Info
> Composite Trie
> Package Info : each has 
> Autopool Info
> Ptx Info
>
> Packages:
>
>
>

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
0 | 4 | header magic | must be "1bsr"
4 | 4 | version number |         4
8 | 8 | reserved |
12 | 4 | rsgp offset | 2666496

16          4           file trie size          432236
20          4           file trie offset        112

24          4           reserved                0
28          4           reserved                0

32          4           rsgp trie size          92344
36          4           rsgp trie offset        432348

40          4           rsgp count              2324
44          4           rsgp info list offset   1812300
48          4           rsgp info size          204

52          4           comp count              1055
56          4           comp info list offset   524692
60          4           comp info size          1156

64          4           comp trie size          68028
68          4           comp trie offset        1744272

72          4           autopool count          2324
76          4           autopool offset         2286396
80          4           autopool size           152

84          4           ptx count               1476
88          4           ptx info offset         2639644
92          4           ptx size                16

96          4           reserve                 0
100         4           reserve                 0
104         4           reserve                 0
108         4           rsgp offset             2666496


# Trie<FileInfoType>
[ASCII_Int24_Int32]


# Rsgp : ReSource Group Package
An Rsgp contain multiple file

Rsgp Header:
Offset      Size        Usage                   Example/ Comment
0           4           header magic            must be "pgsr"
4           4           version                 4
8           4           reserved                0
12          4           reserved                0

16          4           data flags              3
12          4           reserved                0
12          4           reserved                0
28          4           reserved                0