# Rsb



### Trie_CharType_OffsetType_InfoType:
A trie is a data structure representing file structure


Trie_ASCII_Int24_Int32


General Structure:
- Rsb Header
- File Name to Package ID Trie
- Package Name to Package ID
- Composite Info
- Composite Trie
- Package Info
- Autopool Info
- Ptx Info
- Packages

Rsb Header:
Offset | Size | Usage | Example/ Comment
---|---|---|---
0 | 4 | header magic | must be "1bsr"
4           4           version number          4
8           4           reserved                0
12          4           header size             2666496

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