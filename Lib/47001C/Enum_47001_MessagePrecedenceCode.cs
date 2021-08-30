using System.ComponentModel;

namespace IMAS.Core.Parser.VMF.Lib
{
    public enum Enum_47001_MessagePrecedenceCode
    {
        [Description("Routine")]
        ROUTINE = 0,

        [Description("Priority")]
        PRIORITY = 1,

        [Description("Immediate")]
        IMMEDIATE = 2,

        [Description("Flash")]
        FLASH = 3,

        [Description("Flash Override")]
        FLASH_OVERRIDE = 4,

        [Description("CRITIC/ECP")]
        CRITIC_ECP = 5,
    }
}
