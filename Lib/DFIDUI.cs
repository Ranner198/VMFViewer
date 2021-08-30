using System;

namespace IMAS.Core.Parser.VMF.Lib
{
    public class DFIDUI
    {
        #region 6017 DFI/DUI Conversion Methods
        // 6017 Latitude/Longitude Conversions
        public static double Convert_281_14(long value)
        {
            double units = 90.0d / 1048575.0d;

            if (value == 0)
            {
                return 0.0;
            }
            else if (value == 1048575)
            {
                return Double.MaxValue;
            }
            else if (value >= 1 && value <= 1048575)
            {
                return value * units;
            }
            else if (value >= 1048577 && value <= 2097151)
            {
                return (value * units) - 90.0d;
            }

            return Double.MaxValue;
        }
        
        public static double Convert_282_14(long value)
        {
            double units = 180.0d / 2097151.0d;

            if (value == 0)
            {
                return 0.0d;
            }
            else if (value >= 1 && value <= 2097151)
            {
                return (double)(value * units);
            }
            else if (value == 2097152)
            {
                return Double.MaxValue;
            }
            else if (value >= 2097153 && value <= 4194303)
            {
                return (double)(value * units) - 360.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_281_402(long value)
        {
            double units = 90.0d / 16777215.0d;

            if (value == 0)
            {
                return 0.0;
            }
            else if (value == 16777216)
            {
                return Double.MaxValue;
            }
            else if (value >= 1 && value <= 16777215)
            {
                return value * units;
            }
            else if (value >= 16777217 && value <= 33554431)
            {
                return (value * units) - 90.0d;
            }

            return Double.MaxValue;
        }
        
        public static double Convert_282_402(long value)
        {
            double units = 180.0d / 33554431.0d;

            if (value == 0)
            {
                return 0.0d;
            }
            else if (value >= 1 && value <= 33554431)
            {
                return (double)(value * units);
            }
            else if (value == 33554432)
            {
                return Double.MaxValue;
            }
            else if (value >= 33554433 && value <= 67108863)
            {
                return (double)(value * units) - 360.0d;
            }

            return Double.MaxValue;
        }
        
        public static double Convert_281_406(long value)
        {
            double units = 90.0d / 16777215.0d;

            if (value == 0)
            {
                return 0.0;
            }
            else if (value == 16777216)
            {
                return Double.MaxValue;
            }
            else if (value >= 1 && value <= 16777215)
            {
                return value * units;
            }
            else if (value >= 16777217 && value <= 33554431)
            {
                return (value * units) - 90.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_282_406(long value)
        {
            double units = 180.0d / 33554431.0d;

            if (value == 0)
            {
                return 0.0d;
            }
            else if (value >= 1 && value <= 33554431)
            {
                return (double)(value * units);
            }
            else if (value == 33554432)
            {
                return Double.MaxValue;
            }
            else if (value >= 33554433 && value <= 67108863)
            {
                return (double)(value * units) - 360.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_281_407(long value)
        {
            double units = 90.0d / 16777215.0d;

            if (value == 0)
            {
                return 0.0;
            }
            else if (value == 16777216)
            {
                return Double.MaxValue;
            }
            else if (value >= 1 && value <= 16777215)
            {
                return value * units;
            }
            else if (value >= 16777217 && value <= 33554431)
            {
                return (value * units) - 90.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_282_407(long value)
        {
            double units = 180.0d / 33554431.0d;

            if (value == 0)
            {
                return 0.0d;
            }
            else if (value >= 1 && value <= 33554431)
            {
                return (double)(value * units);
            }
            else if (value == 33554432)
            {
                return Double.MaxValue;
            }
            else if (value >= 33554433 && value <= 67108863)
            {
                return (double)(value * units) - 360.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_281_423(long value)
        {
            double units = 90.0d / 16777215.0d;

            if (value == 0)
            {
                return 0.0;
            }
            else if (value == 16777216)
            {
                return Double.MaxValue;
            }
            else if (value >= 1 && value <= 16777215)
            {
                return value * units;
            }
            else if (value >= 16777217 && value <= 33554431)
            {
                return (value * units) - 90.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_282_423(long value)
        {
            double units = 180.0d / 33554431.0d;

            if (value == 0)
            {
                return 0.0d;
            }
            else if (value >= 1 && value <= 33554431)
            {
                return (double)(value * units);
            }
            else if (value == 33554432)
            {
                return Double.MaxValue;
            }
            else if (value >= 33554433 && value <= 67108863)
            {
                return (double)(value * units) - 360.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_281_426(long value)
        {
            double units = 90.0d / 1073741823.0d;

            if (value == 0)
            {
                return 0.0;
            }
            else if (value == 1073741824)
            {
                return Double.MaxValue;
            }
            else if (value >= 1 && value <= 1073741823)
            {
                return value * units;
            }
            else if (value >= 1073741825 && value <= 2147483647)
            {
                return (value * units) - 90.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_282_426(long value)
        {
            double units = 180.0d / 2147483647.0d;

            if (value == 0)
            {
                return 0.0d;
            }
            else if (value >= 1 && value <= 2147483647)
            {
                return (double)(value * units);
            }
            else if (value == 2147483648)
            {
                return Double.MaxValue;
            }
            else if (value >= 2147483649 && value <= 4294967295)
            {
                return (double)(value * units) - 360.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_281_461(long value)
        {
            double units = 90.0d / 16777215.0d;

            if (value == 0)
            {
                return 0.0;
            }
            else if (value == 16777216)
            {
                return Double.MaxValue;
            }
            else if (value >= 1 && value <= 16777215)
            {
                return value * units;
            }
            else if (value >= 16777217 && value <= 33554431)
            {
                return (value * units) - 90.0d;
            }

            return Double.MaxValue;
        }

        public static double Convert_282_461(long value)
        {
            double units = 180.0d / 33554431.0d;

            if (value == 0)
            {
                return 0.0d;
            }
            else if (value >= 1 && value <= 33554431)
            {
                return (double)(value * units);
            }
            else if (value == 33554432)
            {
                return Double.MaxValue;
            }
            else if (value >= 33554433 && value <= 67108863)
            {
                return (double)(value * units) - 360.0d;
            }

            return Double.MaxValue;
        }

        #endregion

    }
}
