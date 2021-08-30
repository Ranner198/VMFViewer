using System;

namespace IMAS.Core.Parser.VMF.Lib.Bitwise
{
    public class BinaryBitReaderHelper
    {
        #region Private Variables
        // Member variables
        private bool m_reverseBits;
        private long[] m_bitArray;
        private int m_bitArrayCount;
        private int m_byteCount;
        #endregion

        #region Constructors
        // Constructor
        public BinaryBitReaderHelper(byte[] byteArray, bool reverseBits)
        {
            // Allocate enough space for the bit array
            m_bitArray = new long[byteArray.Length * 8];
            m_bitArrayCount = 0;
            m_byteCount = byteArray.Length;
            m_reverseBits = reverseBits;

            // Convert all the binary data to binary bits
            int bitIndex = 0;
            for (int i = 0; i < byteArray.Length;)
            {
                long val;

                if (reverseBits)
                {
                    val = (byteArray[i] >> (7 - bitIndex)) & 0x1;
                }
                else
                {
                    val = (byteArray[i] >> bitIndex) & 0x1;
                }

                bitIndex++;

                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    i++;
                }

                m_bitArray[m_bitArrayCount++] = val;
            }
        }
        #endregion

        #region Private Methods
        private byte ConvertBitsToByte(long startbit, long stopbit, long[] value)
        {
            byte v = 0;
            int count = 0;

            if (!m_reverseBits)
            {
                for (long i = startbit; i <= stopbit; i++, count++)
                {
                    v |= (byte)(value[i] << count);
                }
            }
            else
            {
                for (long i = stopbit; i >= startbit; i--, count++)
                {
                    v |= (byte)(value[i] << count);
                }
            }

            return v;
        }
        #endregion

        #region Public Information Methods
        // Get the number of bits converted
        public int GetBitCount()
        {
            return m_bitArrayCount;
        }

        public int GetByteCount()
        {
            return m_byteCount;
        }

        public void OverwriteBytesToBits(byte[] byteArray, int startbit)
        {
            int startByte = startbit / 8;

            m_bitArrayCount = startByte;

            // Convert all the binary data to binary bits
            int bitIndex = 0;

            for (int i = 0; i < byteArray.Length;)
            {
                long val;

                if (m_reverseBits)
                {
                    val = (byteArray[i] >> (7 - bitIndex)) & 0x1;
                }
                else
                {
                    val = (byteArray[i] >> bitIndex) & 0x1;
                }

                bitIndex++;

                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    i++;
                }

                m_bitArray[m_bitArrayCount++] = val;
            }
        }
        #endregion

        #region Data Append Methods
        // Read the data from startbit to the stopbit
        public byte GetDataAsByte(int startbit, int stopbit)
        {
            byte value = 0;
            int count = 0;

            if (!m_reverseBits)
            {
                for (int i = startbit; i <= stopbit; i++, count++)
                {
                    value |= (byte)(m_bitArray[i] << count);
                }
            }
            else
            {
                for (int i = stopbit; i >= stopbit; i--, count++)
                {
                    value |= (byte)(m_bitArray[i] << count);
                }
            }

            return value;
        }

        public long GetDataAsLong(int startbit, int stopbit)
        {
            long value = 0;

            int count = 0;

            if (!m_reverseBits)
            {
                for (int i = startbit; i <= stopbit; i++, count++)
                {
                    value |= (long)(m_bitArray[i] << count);
                }
            }
            else
            {
                for (int i = stopbit; i >= startbit; i--, count++)
                {
                    value |= (long)(m_bitArray[i] << count);
                }
            }

            return value;
        }
        #endregion

        #region Data Extraction Methods
        public byte[] GetDataAsByteArray(int startbit, int stopbit)
        {
            float fSize = (float)(stopbit - startbit) / 8.0f;
            int size = (int)Math.Round(fSize);

            if (size <= 0)
            {
                size = 1;
            }

            int s1 = startbit;
            int resultCount = 0;
            byte[] result = new byte[size];

            for (int i = 0; i < size; i++)
            {
                int tempStartBit = s1;
                int tempStopbit = s1 + 8 - 1;

                result[resultCount++] = ConvertBitsToByte(tempStartBit, tempStopbit, m_bitArray);

                s1 += 8;
            }

            return result;
        }
        #endregion
    }
}
