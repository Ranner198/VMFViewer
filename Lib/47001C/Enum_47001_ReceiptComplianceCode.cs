using System.ComponentModel;

namespace IMAS.Core.Parser.VMF.Lib
{
    public enum Enum_47001_ReceiptComplianceCode
    {
        [Description("Machine Receipt")]
        MACHINE_RECEIPT = 1,

        [Description("Cannot Process (CANTPRO)")]
        CANTPRO = 2,

        [Description("Operator Acknowledge (OPRACK")]
        OPRACK = 3,

        [Description("Will Comply (WILCO)")]
        WILCO,

        [Description("Have Complied")]
        HAVCO,

        [Description("Cannot Comply (CANTCO)")]
        CANTCO,
    }
}
