namespace IMAS.Core.Parser.VMF.Lib.Bitwise
{
    public class BinaryByteSwapHelper
    {
        #region Public Static Methods
        public static long SwapLong(long value)
        {
            long b1 = (value >> 0) & 0xff;
            long b2 = (value >> 8) & 0xff;
            long b3 = (value >> 16) & 0xff;
            long b4 = (value >> 24) & 0xff;

            return b4 << 0 | b3 << 8 | b2 << 16 | b1 << 24;
        }

        public static long SwapShort(long value)
        {
            long b1 = (value >> 0) & 0xff;
            long b2 = (value >> 8) & 0xff;

            return b2 << 0 | b1 << 8;
        }

        public static float SwapFloat(long value)
        {
            int b1 = ((int)value >> 0) & 0xff;
            int b2 = ((int)value >> 8) & 0xff;
            int b3 = ((int)value >> 16) & 0xff;
            int b4 = ((int)value >> 24) & 0xff;

            return (float)(b4 << 0 | (int)b3 << 8 | b2 << 16 | b1 << 24);
        }
        #endregion
    }
}
