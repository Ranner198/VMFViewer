using System.ComponentModel;

namespace IMAS.Core.Parser.VMF.Lib
{
    public enum Enum_47001_CantcoReason
    {
        [Description("Communications problem")]
        COMMUNICATIONS_PROBLEM = 0,

        [Description("Ammunition problem")]
        AMMUNITION_PROBLEM = 1,

        [Description("Personnel problem")]
        PERSONNEL_PROBLEM = 2,

        [Description("Fuel problem")]
        FUEL_PROBLEM = 3,

        [Description("Terrain/Environment problem")]
        TERRAIN_ENVIRONMENT_PROBLEM = 4,

        [Description("Equipment problem")]
        EQUIPMENT_PROBLEM = 5,

        [Description("Tactical Situation problem")]
        TACTICAL_SITUATION_PROBLEM = 6, 

        [Description("Other")]
        OTHER = 7,
    }
}
