using System;

namespace IMAS.Core.Parser.VMF.Lib.Bitwise
{
    public class BinaryBitWriterHelper
    {
        #region Private Variables
        // Member variables
        private Boolean m_reverseBits;
        private readonly long[] m_bitArray;
        private long m_bitArrayCount;
        #endregion

        #region Constructors
        // Constructor
        public BinaryBitWriterHelper(Boolean reverseBits)
        {
            // Allocate enough space for the bit array
            m_bitArray = new long[1024];
            m_bitArrayCount = 0;
            m_reverseBits = reverseBits;
        }

        public BinaryBitWriterHelper(int size, Boolean reverseBits)
        {
            // Allocate enough space for the bit array
            m_bitArray = new long[size];
            m_bitArrayCount = 0;
            m_reverseBits = reverseBits;
        }
        #endregion

        #region Private Methods
        private long[] ConvertInt32ToBits(long value)
        {
            long[] buffer = new long[32];

            if (!m_reverseBits)
            {
                for (int i = 0; i < 32; i++)
                {
                    buffer[i] = (value >> i) & 0x1;
                }
            }
            else
            {
                for (int i = 31; i >= 0; i--)
                {
                    buffer[i] = (value >> i) & 0x1;
                }
            }

            return buffer;
        }

        private byte ConvertBitsToByte(int startbit, int stopbit, long[] value)
        {
            byte result = 0;
            int count = 0;

            if (!m_reverseBits)
            {
                for (int i = startbit; i <= stopbit; i++, count++)
                {
                    result |= (byte)(value[i] << count);
                }
            }
            else
            {
                for (int i = stopbit; i >= startbit; i--, count++)
                {
                    result |= (byte)(value[i] << count);
                }
            }

            return result;
        }

        private long[] ConvertByteToBits(int startbit, int stopbit, byte value)
        {
            long[] buffer = new long[8];

            if (!m_reverseBits)
            {
                for (int i = startbit; i <= stopbit; i++)
                {
                    buffer[i] = (value >> i) & 0x1;
                }
            }
            else
            {
                for (int i = stopbit; i >= startbit; i--)
                {
                    buffer[i] = (value >> i) & 0x1;
                }
            }

            return buffer;
        }
        #endregion

        #region Write Data Methods
        public byte[] WriteLong(int startbit, int stopbit, long data)
        {
            int numberOfBits = stopbit - startbit + 1;
            long[] dataValueToBits = ConvertInt32ToBits(data);
            long[] concatDataValueBits = new long[numberOfBits];

            int count = 0;
            if (!m_reverseBits)
            {
                for (int i = startbit; i < stopbit; i++)
                {
                    concatDataValueBits[count++] = dataValueToBits[i];
                }
            }
            else
            {
                for (int i = stopbit; i >= startbit; i--)
                {
                    concatDataValueBits[count++] = dataValueToBits[i];
                }
            }

            int size = numberOfBits / 8;
            if (size <= 0)
            {
                size = 1;
            }

            int resultCount = 0;
            byte[] result = new byte[size];

            int s1 = 0;
            for (int i = 0; i < size; i++)
            {
                startbit = s1;
                stopbit = s1 + 8 - 1;
                result[resultCount++] = ConvertBitsToByte(startbit, stopbit, concatDataValueBits);

                s1 += 8;
            }

            return result;
        }
        #endregion

        #region Append Data Methods
        public void AppendLong(int startbit, int stopbit, long data)
        {
            long[] dataValueToBits = ConvertInt32ToBits(data);

            int count = 0;

            if (!m_reverseBits)
            {
                for (int i = startbit; i <= stopbit; i++)
                {
                    m_bitArray[i] = dataValueToBits[count++];
                }
            }
            else
            {
                for (int i = stopbit; i >= startbit; i--)
                {
                    m_bitArray[i] = dataValueToBits[count++];
                }
            }

            m_bitArrayCount = stopbit + 1;
        }

        public void AppendByteArray(int startbit, int stopbit, byte[] array)
        {
            bool terminateLoop = false;
            int currentBitOutArray = startbit;
            for (int i = 0; i < array.Length; i++)
            {

                int startBit = 0;
                int stopBit = 7;

                if (stopBit > stopbit)
                {
                    stopBit = stopbit;
                    terminateLoop = true;
                }

                long[] dataValueToBits = ConvertByteToBits(startBit, stopBit, array[i]);

                if (!m_reverseBits)
                {
                    int count = 0;
                    for (int j = currentBitOutArray; j <= currentBitOutArray + stopBit; j++)
                    {
                        m_bitArray[j] = dataValueToBits[count++];
                    }
                }
                else
                {
                    int count = dataValueToBits.Length - 1;
                    for (int j = currentBitOutArray; j <= currentBitOutArray + stopBit; j++)
                    {
                        m_bitArray[j] = dataValueToBits[count--];
                    }
                }

                currentBitOutArray += (stopBit + 1);

                if (terminateLoop)
                {
                    break;
                }
            }

            m_bitArrayCount = stopbit + 1;
        }
        #endregion

        #region Output Data Methods
        public byte[] GetByteArray()
        {
            int size = (int)(Math.Round((float)m_bitArrayCount / 8.0));
            int resultCount = 0;
            byte[] result = new byte[size];

            int s1 = 0;
            for (int i = 0; i < size; i++)
            {
                int startbit = s1;
                int stopbit = s1 + 8 - 1;

                result[resultCount++] = ConvertBitsToByte(startbit, stopbit, m_bitArray);

                s1 += 8;
            }

            return result;
        }
        #endregion

    }
}
