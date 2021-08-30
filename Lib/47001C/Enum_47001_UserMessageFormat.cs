using System.ComponentModel;

namespace IMAS.Core.Parser.VMF.Lib
{
    public enum Enum_47001_UserMessageFormat
    {
        [Description("LINK 16 (J-series message)")]
        LINK_16 = 1,

        [Description("Variable Message Format (VMF) (K-series message)")]
        VMF = 2,

        [Description("National Imagery Transmission Format System (NITFS)")]
        NITFS = 3,

        [Description("Forwarded Message (FWD MSG)")]
        FWD_MSG = 4,

        [Description("United States Message Text Format (USMTF)")]
        USMTF = 5,

        [Description("DOI-103")]
        DOI_103 = 6,

        [Description("eXtensible Markup Language (XML) - Message Text Format (MTF)")]
        XML_MTF = 7,

        [Description("eXtensible Markup Language (XML) - Variable Message Format (VMF)")]
        XML_VMF = 8,
    }
}
