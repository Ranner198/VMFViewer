using System.ComponentModel;

namespace IMAS.Core.Parser.VMF.Lib
{
    public enum Enum_47001_DataCompression
    {
        [Description("Lempel - Ziv - Welch Compression Algorithm, Welch 1984 (LZW)")]
        LZW = 0,

        [Description("RFC 1951 and RFC 1952 (Lempel-Ziv Compression Algorithm, Lempel-Ziv 1977) (LZ-77)")]
        LZ77 = 1,
    }
}
