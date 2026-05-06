## Foreword

This document is copied from [h3x4n1um's RETON](https://github.com/h3x4n1um/RETON) and modified with information from [PopStudio_Old](https://github.com/YingFengTingYu/PopStudio_Old). Types that are not used in 

This documentation takes great inspiration from [BSON's specification](https://bsonspec.org/spec.html).

# RTON v1

* RTON is a binary format in which zero or more ordered key/value pairs are stored as a single entity. This entity is called a Document.
* A document conatins Elements, which can be divided into 3 catergories: Block, Numeral and String.
  * [Block Element](#block-element)s are elements which contain nested Elements.
  * [Numeral Elements](#numeral-element)s are used to store numeric values. All numeral types ares stored in little endian.
  * [String Elements](#string-element)s are used to store strings and ids, and used as the key in key/value pair.
* Elements has a bytecode indicating its type and a body. Some byte code indicate that the Element has a predefined value and does not have a body. 
* Note that the `*` operator is shorthand for repetition. E.g. `byte*2` is byte byte, `byte*any` is any amount of byte.

## Document
The container for all data in a RTON file.

Definition: <br>
`$MagicHeader $Version (KeyValuePair)*any $MagicFooter`
* `MagicHeader` (uint)= 0x4e4f5452 (`RTON` in ASCII)
* the RTON version (uint)= 1
* MagicFooter (uint)= 0x454e4f44 (`DONE` in ASCII)
Block
### KeyValuePair
Definition: `StringElement Element`

- [RTON v1](#rton-v1)
  - [Document](#document)
    - [KeyValuePair](#keyvaluepair)
  - [Table of Element ByteCode](#table-of-element-bytecode)
  - [Block Element](#block-element)
    - [Object](#object)
    - [Array](#array)
  - [Numeral Element](#numeral-element)
    - [VarInt](#varint)
    - [VarZigZag](#varzigzag)
  - [String Element](#string-element)
    - [AsciiString](#asciistring)
    - [Utf8String](#utf8string)
  - [Others Element](#others-element)
    - [RTID](#rtid)
      - [`0x03` Subcode](#0x03-subcode)
  - [String Caching](#string-caching)
  - [Cached UTF-8 String](#cached-utf-8-string)
    - [`0x92` and `0x93`](#0x92-and-0x93)


## Table of Element ByteCode

Bytecode | Type | Value if predefined | Notes
---|---|---|---
---|**NumeralElement**
`0x00` | Boolean | false
`0x01` | Boolean | true
`0x08` | Int8 |
`0x09` | Int8 | 0
`0x0a` | UInt8 |
`0x0b` | UInt8 | 0
`0x10` | Int16 |
`0x11` | Int16 | 0
`0x12` | UInt16 |
`0x13` | UInt16 | 0
`0x20` | Int32 |
`0x21` | Int32 | 0
`0x22` | Single |
`0x23` | Single | 0.0f
`0x24` | [VarInt32](#varint) |
`0x25` | [Zigzag32](#varzigzag) |
`0x26` | UInt32 |
`0x27` | UInt32 | 0
`0x28` | [VarUInt32](#varint) |
`0x40` | Int64 |
`0x41` | Int64 | 0
`0x42` | Double |
`0x43` | Double | 0
`0x44` | [VarInt64](#varint) |
`0x45` | [Zigzag64](#varzigzag) |
`0x46` | UInt64 |
`0x47` | UInt64 | 0
`0x48` | [VarUInt64](#varint) |
---|[**StringElement**](#string-element)
`0x02` | String | EmptyString
`0x81` | [AsciiString](#asciistring) |
`0x82` | [Utf8String](#utf8string) |
`0x90` | cache [AsciiString](#string-caching) |
`0x91` | recall [AsciiString](#string-caching) |
`0x92` | cache [Utf8String](#string-caching) |
`0x93` | recall [Utf8String](#string-caching) |
---|**BlockElement**
`0x85` | Object | 
`0x86` | Array |
---|**Others**
`0x83` | [RTID](#rtid) |
`0x84` | [RTID](#rtid) | RTID(0)
`0x87` | [BinaryString]() |

## Block Element
[Block Element](#block-element)s are elements which contain nested Elements. Elements of this catergory is: Object, Array.

### Object
Bytecode: `0x85` <br>
Body: `KeyValuePair*any 0xff`
* Example:
```
52 54 4F 4E 01 00 00 00 {
    90 07 54 65 73 74 69 6E 67
    "Testing":
    85 {
        90 05 48 65 6C 6C 6F 90 02 48 69
        "Hello": "Hi"
    FF }
FF }
44 4F 4E 45
```
### Array
Bytecode: `0x86` <br>
Body: `0xfd $Count Element*Count 0xfe` <br>
where `Count` is [VarInt32](#varint)
* Example:
```
52 54 4F 4E 01 00 00 00 {
    90 0E 41 6E 45 78 61 6D 70 6C 65 41 72 72 61 79
    "AnExampleArray": [
    86 FD 03
        90 0A 31 73 74 45 6C 65 6D 65 6E 74
            "1stElement",
        90 0A 32 6E 64 45 6C 65 6D 65 6E 74
            "2ndElement",
        90 0A 33 72 64 45 6C 65 6D 65 6E 74
            "3rdElement"
    FE ]
FF }
44 4F 4E 45
```

## Numeral Element
[Numeral Elements](#numeral-element) are used to store numeric values. <br>
The body of numeral element are the number themselves.  All numeral types are stored in little endian. <br>
This section only presents some notable elements. See [Table](#table-of-element-bytecode) for others.
### [VarInt](https://protobuf.dev/programming-guides/encoding/#varints)
Varint encoding reduces the size of many integer by omitting the most significant bits in ints which do not need them. The codes for its members are: `0x24` VarInt32 | `0x28` VarUInt32 | `0x44` VarInt64 | `0x48` VarUInt64.

### [VarZigZag](https://protobuf.dev/programming-guides/encoding/#signed-ints)
Negative number are stored using two's complement. I.E. -2 becomes ~0 + (-2) + 1 and hence use the upper bytes.
Zigzag encoding use the least significant bit as the sign bit  and the other bits are use to store the absolute value (negative values -1 before storing). The codes for its members are:
`0x25` VarZigZag32 | `0x45` VarZigZag64

## String Element
String Elements are used to store strings and ids, and used as the key in key/value pair. Bytecode `0x02` is used for empty string.
### AsciiString
Bytecode: `0x81` <br>
Body: `$Count Byte*Count` <br>
Where `Count` is [VarInt32](#varint)
* Example
```
81 06 6C 65 6D 65 6E 74
"Element"
```
### Utf8String
ByteCode: `0x82` <br>
Body: `$Utf8Count $ByteCount Byte*Count` <br>
where `Utf8Count`, `ByteCount` are [VarInt32](#varint)
```
81 06 06 6C 65 6D 65 6E 74
"Element"
```
## Others Element
### RTID
Based on [PopStudio_Old](https://github.com/YingFengTingYu/PopStudio_Old)

Bytecode: `0x83`
RTID references to other resources in the PACKAGE directory. Has a one-byte subcode to indicate its type
#### `0x03` Subcode
Body: `$InitRtonName $PropertyName`
where `InitRtonName`, `PropertyName` are [Utf8String](#utf8string).
```
83 03 0c 0c 4c 65 76 65 6c 4d 6f 64 75 6c 65 73
0b 0b 44 65 66 61 75 6c 74 4c 6f 6f 74
RTID(LevelModules@DefaultLoot)
```

> [!WARNING] Not Used, Unsure, DO NOT USE
> #### `0x0` Subcode
> ```
> 83 00
> "RTID()"
> ```
> #### `0x1` Subcode
> Body: `01 $MajorVersion $MinorVersion $Id`<br>
> where `MajorVersion`, `MinorVersion` are UInt8, `Id` is UInt32.
> ```
> 83 01 41 63 74 69 76 65
> "RTID(1.0.6d7ba77d@)"
> ```
> #### `0x02` Subcode
> Body: `02 $PropertyName $MajorVersion $MinorVersion $Id`<br>
> where `PropertyName` is [Utf8String](#utf8string), `MajorVersion`, `MinorVersion` are UInt8, `Id` is UInt32.
> ```
> 83 02 0C 0C 51 75 65 73 74 73 41 63 74 69 76 65
> 00 01 7D A7 7B 6D
> "RTID(1.0.6d7ba77d@QuestsActive)"
> ```

### ByteString
> [!WARNING] Not Used, Unsure, DO NOT USE
> Body: `00 $Count Byte*Count $Something`<br>
> where `Count`, `Something` is [VarInt32](#varint).
> 87 00 0C 51 75 65 73 74 73 41 63 74 69 76 65 6D
> ```
> "BINARY(QuestsActive@110)"
> ```

## String Caching

Cache the string into a list. Recall string with index.
`0x90` cache [AsciiString](#asciistring) | `0x91` recall [AsciiString](#asciistring) | `0x92` cache [Utf8String](#utf8string) | `0x93` recall [Utf8String](#utf8string)

* `90 xx [string]`, the `xx [string]` is just like `0x81`

* By using `0x90`, the string is cached then it can be recalled by `91 xx`, `xx` is **unsigned RTON number**-th element in the cached (starting from 0)

* Let's call it ASCII_CACHE

* Example: the following dump contain 2 objects:

    ```
    52 54 4F 4E 01 00 00 00
        90 08 23 63 6F 6D 6D 65 6E 74 90 33 50 6C 61 6E 74 20 6C 65 76 65 6C 69 6E 67 20 64 61 74 61 21 20 20 42 65 77 61 72 65 20 79 65 20 61 6C 6C 20 77 68 6F 20 65 6E 74 65 72 20 68 65 72 65 21
        90 07 54 65 73 74 69 6E 67 91 00
    FF
    44 4F 4E 45
    ```

    * The 1st object creates a 8-byte string key `23 63 6F 6D 6D 65 6E 74` (`#comment` in ASCII) (1st in ASCII_CACHE), value inside it is 51-byte long string (`0x33`) `Plant leveling data!  Beware ye all who enter here!` (2nd in ASCII_CACHE)

    * The 2nd object creates 7-byte string key `54 65 73 74 69 6E 67` (`Testing` in ASCII), value inside it is `91 00` which mean recalls the 1st string in ASCII_CACHE

* JSON decode:

    ```JSON
    {
        "#comment": "Plant leveling data!  Beware ye all who enter here!",
        "Testing": "#comment"
    }
    ```

## Cached UTF-8 String

### `0x92` and `0x93`

* Very much like the **Cached String**, `0x92` and `0x93` different is `0x93` use UTF-8 encode

* `92 [L1] [L2] [string]`, the `[L1] [L2] [string]` same as `0x82`

* Example:

    ```
    52 54 4F 4E 01 00 00 00
        90 05 48 65 6C 6C 6F 92 0B 0E C4 90 C3 A2 79 20 6C C3 A0 20 75 74 66 38
        90 04 54 65 73 74 92 0A 0E 54 68 E1 BB AD 20 6E 67 68 69 E1 BB 87 6D
        93 01 93 00
    FF 44 4F 4E 45
    ```

    * The 1st object creates a 5-byte string key `48 65 6C 6C 6F` (`Hello` in ASCII) (1st in ASCII_CACHE), value inside it is 11 characters (`0x0B`), 14-byte long utf-8 string (`0x0E`) `Đây là utf8` (1st in UTF8_CACHE)

    * The 2nd object creates 4-byte string key `54 65 73 74` (`Test` in ASCII) (2nd in ASCII_CACHE), value inside it is 10 characters (`0x0A`), 14-byte long utf-8 string (`0x0E`) `Thử nghiệm` (2nd in UTF8_CACHE)

    * The 3rd object key using `93 01` recalls 2nd string in UTF8_CACHE `Thử nghiệm` and value `93 00` recalls the 1st string `Đây là utf8`

* JSON decode:

    ```JSON
    {
        "Hello": "Đây là utf8",
        "Test": "Thử nghiệm",
        "Thử nghiệm": "Đây là utf8"
    }
    ```
