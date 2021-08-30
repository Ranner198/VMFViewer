using System.ComponentModel;

namespace IMAS.Core.Parser.VMF.Lib
{
    public enum Enum_47001_FunctionAreaDesignator
    {
        [Description("Network Control (K00 Series)")]
        NETWORK_CONTROL = 0,

        [Description("General Information Exchange (K01 Series")]
        GENERAL_INFORMATION_EXCHANGE = 1,

        [Description("Fire Support Operations (K02 Series)")]
        FIRE_SUPPORT_OPERATIONS = 2,

        [Description("Air Operations (K03 Series)")]
        AIR_OPERATIONS = 3,

        [Description("Intelligence Operations (K04 Series)")]
        INTELLIGENCE_OPERATIONS = 4,

        [Description("Land Combat Operations (K05 Series)")]
        LAND_COMBAT_OPERATIONS = 5,

        [Description("Maritime Operations (K06 Series)")]
        MARITIME_OPERATIONS = 6,

        [Description("Combat Service Support (K07 Series)")]
        COMBAT_SERVICE_SUPPORT = 7,

        [Description("Special Operations (K08 Series)")]
        SPECIAL_OPERATIONS = 8,

        [Description("JTF Operations Control (K09 Series)")]
        JTF_OPERATIONS_CONTROL = 9,

        [Description("Air Defense/Air Space Control (K10 Series)")]
        AIR_DEFENSE_AIR_SPACE_CONTROL = 10,
    }
}
