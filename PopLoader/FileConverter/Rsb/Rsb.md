# Rsb (ResourceBinary)

> The name "ResourceBinary" is not the official name. I came up with it to make developement "easier" by avoiding excessive use of strange acronym. Many of the components making up it will also be "deabbrivated".

## Basics

**ResourceBinary** (**Rsb**) file contain information required for fast lookup of files, packages and groups. All offsets except in **Packages** and **IndexTrie**s are relative to the start of the **ResourceBinary** file.

**ResourceBinary** contains:
- ***Header*** contains the size and offset other components in the file.
- ***FileTrie*** is an index trie mapping file name to the index of the package containing this file.
- ***PackageTrie*** is an index trie mapping package name to the index of the package.
- ***PackageInfos*** is an array of package information. The 0-index position of the package info in the file is the package index.
- ***GroupInfos*** is an array of group info. The 0-index position of the group info in the array is the group index.
- ***GroupTrie*** is an indexTrie mapping group name to the index of the group.
- ***AutopoolInfos*** is an Array of group each with fixed size, containing the Name of the group, packages' indexes, and more.  
- ***TextureInfo*** is an Array of texture informations. The 0-index position of the texture info in the file is the texture index.
- ***Packages*** are the packages containing actual game assets.

### Group, Package and File
In **ResourceBinary**, only **Package**s are stored; access to a **File** require opening the package containing it and access to a **Group** is simply opening multiple **Package**s.
- A **Group** is collection of packages. A group of audio packages is also called a **CompositeShell**.
- A **Package** is collection of files.
- A **File** is one of the game's assets.
### Pool (AutoPool)
**Disclaimer: Very Unsure**

Each **Package** is matched with an **Pool**, which has the name of PackageName + "_AutoPool". A **Pool** indicate how much memory should the game allocate for its matching **Package**. The position of the matching file in **AutoPoolInfos** is the same as that of its matching **Package** in **PackageInfos**

## Header
Contains information about RsbComponents in the Binary file
```
Offset  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
________General
0x00    31 62 73 72 04 00 00 00 00 00 00 00 00 B0 28 00
        1  b  s  r  ·  ·  ·  ·  ·  ·  ·  ·  ·  °  (  · 
________FileTrie
0x10    6C 98 06 00 70 00 00 00 00 00 00 00 00 00 00 00
        l  ·  ·  ·  p  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  · 
________PackageTrie
0x20    B8 68 01 00 DC 98 06 00
        ¸  h  ·  ·  Ü  ·  ·  · 
________PackageInfo
0x28    CC 00 00 00 14 09 00 00 4C A7 1B 00
        Ì  ·  ·  ·  ·  ·  ·  ·  L  §  ·  ·  
________Group
0x34    1F 04 00 00 94 01 08 00 84 04 00 00
        ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  
________GroupTrie
0x40    BC 09 01 00 90 9D 1A 00
        ¼  ·  ·  ·  ·  ·  ·  · 
________Autopool
0x48    14 09 00 00 3C E3 22 00 98 00 00 00
        ·  ·  ·  ·  <  ã  "  ·  ·  ·  ·  · 
________TextureDetail
0x54    C4 05 00 00 1C 47 28 00 10 00 00 00
        Ä  ·  ·  ·  ·  G  (  ·  ·  ·  ·  · 
________Reserved and Package Offset
0x60    00 00 00 00 00 00 00 00 00 00 00 00 00 B0 28 00
        ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  °  (  ·
```

Name | Data (in order)
--- | ---
General | Magic string "1bsr", Version Number<br> Reserved, Package Offset
FileTrie | Size, Offset
PackageTrie | Size, Offset
PackageInfo | Count, Offset, SizeOfInfo
GroupInfo | Count, Offset, SizeOfInfo
GroupTrie | Size, Offset
PoolInfo | Count, Offset, SizeOfInfo
Ending | Reserved x 3, Package Offset

## 

## Note
Due to Package Offset field in Package being 32 bit, the Rsb file has a max size of at most uint.MaxValue 4GiB, however, it is safer to keep it under 2 GiB.