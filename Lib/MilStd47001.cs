using IMAS.Core.Parser.VMF.Lib.Bitwise;
using IMAS.Core.Parser.VMF.Lib;
using IMAS.Core.Parser.VMF.Lib.Compression;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IMAS.Core.Parser.VMF.Lib
{
    public class MilStd47001
    {
        #region Private Variables
        private BinaryBitReaderHelper m_bitReader;
        private MilStd47001Data m_data;
        private int currentBit = 0;
        #endregion

        #region Public Properties
        public MilStd47001Data Data 
        {
            get
            {
                return m_data;
            }
            set
            {
                m_data = value;
            }
        }

        public BinaryBitReaderHelper BitReader
        {
            get
            {
                return m_bitReader;
            }
            set
            {
                m_bitReader = value;
            }
        }
        #endregion

        #region Constructors
        public MilStd47001(byte[] inputBuffer)
        {
            m_bitReader = new IMAS.Core.Parser.VMF.Lib.Bitwise.BinaryBitReaderHelper(inputBuffer, false);
            m_data = new MilStd47001Data();
        }
        #endregion

        #region Parse Methods
        public int Parse()
        {
            // Get the protocol version
            m_data.Version = readDatalong("VERSION", 4);

            // Determine which parsing method to run
            switch (m_data.Version)
            {
                case 1: // MIL-STD-47001-B
                    readVersionB();
                    break;
                case 2: // MIL-STD-47001-C
                    readVersionC();
                    break;
                case 3: // MIL-STD-47001-D
                    readVersionD();
                    break;
                case 4: // MIL-STD-47001-D CHANGE 1
                    readVersionDChange1();
                    break;
                default:
                    break;
            }

            if (m_data.HeaderSize > 0)
            {
                currentBit = (int) m_data.HeaderSize * 8;
            }
            else
            {
                // Calculate the bit padding between the header and data
                int newCurrentBit = align8Bit(currentBit);

                m_bitReader.GetDataAsLong(currentBit, newCurrentBit);

                currentBit = newCurrentBit;
            }

            if (m_data.IsDataCompressionSet)
            {
                Enum_47001_DataCompression dataCompression = (Enum_47001_DataCompression)m_data.DataCompression;

                switch (dataCompression)
                {
                    case Enum_47001_DataCompression.LZW:
                        {
                            byte[] data = m_bitReader.GetDataAsByteArray(currentBit, m_bitReader.GetBitCount());

                            List<long> listOfIntegers = new List<long>();
                            for (long i = 0; i < data.Length; i++)
                            {
                                listOfIntegers.Add(data[i]);
                            }

                            string decompressedString = decompress_lzw(listOfIntegers);

                            byte[] decompressedData = new byte[decompressedString.Length];
                            for (int i = 0; i < decompressedData.Length; i++)
                            {
                                decompressedData[i] = (byte)decompressedString[i];
                            }

                            if (decompressedData != null)
                            {
                                m_bitReader.OverwriteBytesToBits(decompressedData, currentBit);
                            }
                        }
                        break;
                    case Enum_47001_DataCompression.LZ77:
                        {
                            byte[] data = m_bitReader.GetDataAsByteArray(currentBit, m_bitReader.GetBitCount());

                            // GZIP data has a magic number
                            if (data[0] == 0x1F && data[1] == 0x8B)
                            {
                                byte[] decompressedData = decompress_lz77(data);

                                if (decompressedData != null)
                                {
                                    m_bitReader.OverwriteBytesToBits(decompressedData, currentBit);
                                }
                            }
                        }
                        break;
                    default:
                        {
                        }
                        break;
                }
            }


            return currentBit;
        }
        #endregion

        #region Utility Methods
        public int GetCurrentBit()
        {
            return currentBit;
        }

        public long readDatalong(string name, int size)
        {
            int currentBitStart = currentBit;
            int currentBitStop = currentBit + size - 1;

            long result = m_bitReader.GetDataAsLong(currentBitStart, currentBitStop);

            Debug.Print(name + " => " + result.ToString());

            // Start the next field on the following bit
            currentBit = currentBitStop + 1;

            return result;
        }

        public byte[] readDataArray(string name, int size)
        {
            int currentBitStart = currentBit;
            int currentBitStop = currentBit + size - 1;

            byte[] result = m_bitReader.GetDataAsByteArray(currentBitStart, currentBitStop);

            if (result != null)
            {
                Debug.Print(name + ": ");
                result.Select(p => p.ToString("x2")).ToArray().ToList().ForEach(x => Debug.Print(x));
            }

            // Start the next field on the following bit
            currentBit = currentBitStop + 1;

            return result;
        }

        public string readDataString(string name, int size)
        {
            String vmfString = "";

            for (int i = 0; i < size; i += 7)
            {
                long vmfCharField = readDatalong(name, 7);

                char c = (char)vmfCharField;

                if ((vmfCharField >= 32 && vmfCharField < 127) || (vmfCharField == 10) || (vmfCharField == 13))
                {
                    vmfString += c;
                }
                else
                {
                    break;
                }
            }

            Debug.Print(name + ": " + vmfString);

            return vmfString;
        }

        public int align8Bit(int value)
        {
            // Calculate the bit padding between the header and data
            int newCurrentBit = currentBit;
            int newCurrentBitInBytes = newCurrentBit / 8;

            if (newCurrentBit % 8 > 0)
            {
                newCurrentBitInBytes++;
            }

            newCurrentBit = newCurrentBitInBytes * 8;

            return newCurrentBit;
        }
        #endregion

        #region MIL-STD-47001 Parsing Methods
        private bool readVersionB()
        {
            long dataCompressionTypeFpi = readDatalong("DATA COMPRESSION FPI", 1);

            if (dataCompressionTypeFpi == 1)
            {
                long dataCompressionType = readDatalong("DATA COMPRESSION TYPE", 2);
            } // dataCompressionTypeFpi

            long originatorAddressGpi = readDatalong("ORIGINATOR ADDRESS GPI", 1);

            if (originatorAddressGpi == 1)
            {
                long originatorAddressUrnFpi = readDatalong("ORIGINATOR ADDRESS URN FPI", 1);

                if (originatorAddressUrnFpi == 1)
                {
                    long originatorAddressUrn = readDatalong("ORIGINATOR ADDRESS URN", 24);
                } // originatorAddressUrnFpi

                long originatorAddressUnitNameFpi = readDatalong("ORIGINATOR ADDRESS UNIT NAME FPI", 1);

                if (originatorAddressUnitNameFpi == 1)
                {
                    String originatorAddressUnitName = readDataString("ORIGINATOR ADDRESS UNIT NAME", 448);
                } // originatorAddressUnitNameFpi
            } // originatorAddressGpi

            long recipientAddressGpi = readDatalong("RECIPIENT ADDRESS GPI", 1);

            if (recipientAddressGpi == 1)
            {
                bool recipientAddressGriRepeat = false;
                do
                {
                    long recipientAddressGri = readDatalong("RECIPIENT ADDRESS GRI", 1);
                    recipientAddressGriRepeat = (recipientAddressGri == 1);

                    long recipientAddressUrnFpi = readDatalong("RECIPIENT ADDRESS URN FPI", 1);

                    if (recipientAddressUrnFpi == 1)
                    {
                        long recipientAddressUrn = readDatalong("RECIPIENT ADDRESS URN", 24);
                    }

                    long recipientAddressUnitNameFpi = readDatalong("RECIPIENT ADDRESS UNIT NAME FPI", 1);

                    if (recipientAddressUnitNameFpi == 1)
                    {
                        String recipientAddressUnitName = readDataString("RECIPIENT ADDRESS UNIT NAME", 448);
                    } // recipientAddressUnitNameFpi

                } while (recipientAddressGriRepeat);
            } // recipientAddressGpi

            long informationAddressGpi = readDatalong("INFORMATION ADDRESS GPI", 1);

            if (informationAddressGpi == 1)
            {
                bool informationAddressGriRepeat = false;
                do
                {
                    long informationAddressGri = readDatalong("INFORMATION ADDRESS GRI", 1);
                    informationAddressGriRepeat = (informationAddressGri == 1);

                    long informationAddressUrnFpi = readDatalong("INFORMATION ADDRESS URN FPI", 1);

                    if (informationAddressUrnFpi == 1)
                    {
                        long informationAddressUrn = readDatalong("INFORMATION ADDRESS URN", 24);
                    }

                    long informationAddressUnitNameFpi = readDatalong("INFORMATION ADDRESS UNIT NAME FPI", 1);

                    if (informationAddressUnitNameFpi == 1)
                    {
                        String informationAddressUnitName = readDataString("INFORMATION ADDRESS UNIT NAME", 448);
                    } // informationAddressUnitNameFpi

                } while (informationAddressGriRepeat);
            } // informationAddressGpi

            bool messageHandlingGriRepeat = false;
            do
            {
                long messageHandlingGri = readDatalong("MESSAGE HANDLING GRI", 1);
                messageHandlingGriRepeat = (messageHandlingGri == 1);

                long userMessageFormat = readDatalong("USER MESSAGE FORMAT", 4);

                long messageIdentificationGpi = readDatalong("MESSAGE IDENTIFICATION GPI", 1);

                if (messageIdentificationGpi == 1)
                {
                    long functionalAreaDesignator = readDatalong("FUNCTION AREA DESIGNATOR", 4);

                    long messageNumber = readDatalong("MESSAGE NUMBER", 7);

                    long messageSubTypeFpi = readDatalong("MESSAGE SUBTYPE FPI", 1);

                    if (messageSubTypeFpi == 1)
                    {
                        long messageSubType = readDatalong("MESSAGE SUBTYPE", 7);
                    }
                } // messageIdentificationGpi

                long fileNameFpi = readDatalong("FILENAME FPI", 1);

                if (fileNameFpi == 1)
                {
                    String fileName = readDataString("FILENAME", 448);
                } // fileNameFpi

                long messageSizeFpi = readDatalong("MESSAGE SIZE FPI", 1);

                if (messageSizeFpi == 1)
                {
                    long messageSize = readDatalong("MESSAGE SIZE", 20);
                } // messageSizeFpi

                long operationalIndicator = readDatalong("OPERATIONAL INDICATOR", 2);

                long retransmitIndicator = readDatalong("RETRANSMIT INDICATOR", 1);

                long messagePrecedenceCode = readDatalong("MESSAGE PRECEDENCE CODE", 3);

                long messageSecurityClassification = readDatalong("MESSAGE SECURITY CLASSIFICATION", 2);

                long controlReleaseMarkingFpi = readDatalong("CONTROL RELEASE MARKING FPI", 1);

                if (controlReleaseMarkingFpi == 1)
                {
                    String controlReleaseMarking = readDataString("CONTROL RELEASE MARKING", 14);
                } // controlReleaseMarkingFpi

                long originatorDtgGpi = readDatalong("ORIGINATOR DTG GPI", 1);

                if (originatorAddressGpi == 1)
                {
                    long originatorYear = readDatalong("ORIGINATOR YEAR", 7);

                    long originatorMonth = readDatalong("ORIGINATOR MONTH", 4);

                    long originatorDay = readDatalong("ORIGINATOR DAY", 5);

                    long originatorHour = readDatalong("ORIGINATOR HOUR", 5);

                    long originatorMinute = readDatalong("ORIGINATOR MINUTE", 6);

                    long originatorSecond = readDatalong("ORIGINATOR SECOND", 6);

                    long originatorDtgExtensionFpi = readDatalong("ORIGINATOR DTG EXTENSION FPI", 1);

                    if (originatorDtgExtensionFpi == 1)
                    {
                        long originatorDtgExtension = readDatalong("ORIGINATOR DTG EXTENSION", 12);
                    }

                } // originatorDtgGpi

                long perishabilityDtgGpi = readDatalong("PERISHABILITY DTG GPI", 1);

                if (perishabilityDtgGpi == 1)
                {
                    long perishabilityDtgYear = readDatalong("PERISHABILITY DTG YEAR", 7);

                    long perishabilityDtgMonth = readDatalong("PERISHABILITY DTG MONTH", 4);

                    long perishabilityDtgDay = readDatalong("PERISHABILITY DTG DAY", 5);

                    long perishabilityDtgHour = readDatalong("PERISHABILITY DTG HOUR", 5);

                    long perishabilityDtgMinute = readDatalong("PERISHABILITY DTG MINUTE", 6);

                    long perishabilityDtgSecond = readDatalong("PERISHABILITY DTG SECOND", 6);
                } // perishabilityDtgGpi

                long acknowledgementRequestGroupGpi = readDatalong("ACKNOWLEDGEMENT REQUEST GROUP GPI", 1);

                if (acknowledgementRequestGroupGpi == 1)
                {
                    long machineAcknowledgementRequestIndicator = readDatalong("MACHINE ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    long operatorAcknowledgementRequestIndicator = readDatalong("OPERATOR ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    long operatorReplyRequestIndicator = readDatalong("OPERATOR REPLY REQUEST INDICATOR", 1);
                } // acknowledgementRequestGroupGpi

                long responseDataGroupGpi = readDatalong("RESPONSE DATA GROUP GPI", 1);

                if (responseDataGroupGpi == 1)
                {
                    long responseDataYear = readDatalong("RESPONSE DATA YEAR", 7);

                    long responseDataMonth = readDatalong("RESPONSE DATA MONTH", 4);

                    long responseDataDay = readDatalong("RESPONSE DATA DAY", 5);

                    long responseDataHour = readDatalong("RESPONSE DATA HOUR", 5);

                    long responseDataMinute = readDatalong("RESPONSE DATA MINUTE", 6);

                    long responseDataSecond = readDatalong("RESPONSE DATA SECOND", 6);

                    long responseDataDtgExtensionFpi = readDatalong("RESPONSE DATA DTG EXTENSION FPI", 1);

                    if (responseDataDtgExtensionFpi == 1)
                    {
                        long responseDataDtgExtension = readDatalong("RESPONSE DATA DTG EXTENSION", 12);
                    }

                    long receiptCompliance = readDatalong("RECIPT COMPLIANCE", 1);

                    long cantcoReasonCodeFpi = readDatalong("CANTCO REASON CODE FPI", 1);

                    if (cantcoReasonCodeFpi == 1)
                    {
                        long cantcoReasonCode = readDatalong("CANTCO REASON CODE", 3);
                    } // cantcoReasonCodeFpi

                    long cantproReasonCodeFpi = readDatalong("CANTPRO REASON CODE FPI", 1);

                    if (cantproReasonCodeFpi == 1)
                    {
                        long cantproReasonCode = readDatalong("CANTPRO REASON CODE", 6);
                    }

                    long replyAmplificationFpi = readDatalong("REPLY AMPLIFICATION FPI", 1);

                    if (replyAmplificationFpi == 1)
                    {
                        String replyAmplification = readDataString("REPLY AMPLIFICATION", 350);
                    }
                } // responseDataGroupGpi

                long referenceMessageDataGpi = readDatalong("REFERENCE MESSAGE DATA GPI", 1);

                if (referenceMessageDataGpi == 1)
                {
                    bool referenceMessageDataGriRepeat = false;
                    do
                    {
                        long referenceMessageDataGri = readDatalong("REFERENCE MESSAGE DATA GRI", 1);
                        referenceMessageDataGriRepeat = (referenceMessageDataGri == 1);

                        long referenceMessageDataUrnFpi = readDatalong("REFERENCE MESSAGE DATA URN FPI", 1);

                        if (referenceMessageDataUrnFpi == 1)
                        {
                            long referenceMessageDataUrn = readDatalong("REFERENCE MESSAGE DATA URN", 24);
                        }

                        long referenceMessageDataUnitNameFpi = readDatalong("REFERENCE MESSAGE DATA UNIT NAME FPI", 1);

                        if (referenceMessageDataUnitNameFpi == 1)
                        {
                            String referenceMessageDataUnitName = readDataString("REFERENCE MESSAGE DATA UNIT NAME", 350);
                        } // referenceMessageDataUnitNameFpi

                        long referenceDataYear = readDatalong("REFERENCE DATA YEAR", 7);

                        long referenceDataMonth = readDatalong("REFERENCE DATA MONTH", 4);

                        long referenceDataDay = readDatalong("REFERENCE DATA DAY", 5);

                        long referenceDataHour = readDatalong("REFERENCE DATA HOUR", 5);

                        long referenceDataMinute = readDatalong("REFERENCE DATA MINUTE", 6);

                        long referenceDataSecond = readDatalong("REFERENCE DATA SECOND", 6);

                        long referenceDataDtgExtensionFpi = readDatalong("REFERENCE DATA DTG EXTENSION FPI", 1);

                        if (referenceDataDtgExtensionFpi == 1)
                        {
                            long referenceDataDtgExtension = readDatalong("REFERENCE DATA DTG EXTENSION", 12);
                        }

                        long referenceMessageDataFAD = readDatalong("REFERENCE DATA FAD", 4);

                        long referenceMessageDataMessageNumber = readDatalong("REFERENCE DATA MESSAGE NUMBER", 7);

                    } while (referenceMessageDataGriRepeat);
                }  // referenceMessageDataGpi

            } while (messageHandlingGriRepeat);
                
            return true;
        }

        private bool readVersionC()
        {
            long dataCompressionTypeFpi = readDatalong("DATA COMPRESSION FPI", 1);

            if (dataCompressionTypeFpi == 1)
            {
                m_data.DataCompression = readDatalong("DATA COMPRESSION TYPE", 2);
            } // dataCompressionTypeFpi

            long originatorAddressGpi = readDatalong("ORIGINATOR ADDRESS GPI", 1);

            if (originatorAddressGpi == 1)
            {
                long originatorAddressUrnFpi = readDatalong("ORIGINATOR ADDRESS URN FPI", 1);

                if (originatorAddressUrnFpi == 1)
                {
                    m_data.OriginatorAddress.Urn = readDatalong("ORIGINATOR ADDRESS URN", 24);
                } // originatorAddressUrnFpi

                long originatorAddressUnitNameFpi = readDatalong("ORIGINATOR ADDRESS UNIT NAME FPI", 1);

                if (originatorAddressUnitNameFpi == 1)
                {
                    m_data.OriginatorAddress.UnitName = readDataString("ORIGINATOR ADDRESS UNIT NAME", 448);
                } // originatorAddressUnitNameFpi
            } // originatorAddressGpi

            long recipientAddressGpi = readDatalong("RECIPIENT ADDRESS GPI", 1);

            if (recipientAddressGpi == 1)
            {
                bool recipientAddressGriRepeat = false;
                do
                {
                    MilStd47001_Address address = new MilStd47001_Address();

                    long recipientAddressGri = readDatalong("RECIPIENT ADDRESS GRI", 1);
                    recipientAddressGriRepeat = (recipientAddressGri == 1);

                    long recipientAddressUrnFpi = readDatalong("RECIPIENT ADDRESS URN FPI", 1);

                    if (recipientAddressUrnFpi == 1)
                    {
                        address.Urn = readDatalong("RECIPIENT ADDRESS URN", 24);
                    }

                    long recipientAddressUnitNameFpi = readDatalong("RECIPIENT ADDRESS UNIT NAME FPI", 1);

                    if (recipientAddressUnitNameFpi == 1)
                    {
                        address.UnitName = readDataString("RECIPIENT ADDRESS UNIT NAME", 448);
                    } // recipientAddressUnitNameFpi

                    m_data.RecipientAddress.Add(address);
                } while (recipientAddressGriRepeat);
            } // recipientAddressGpi

            long informationAddressGpi = readDatalong("INFORMATION ADDRESS GPI", 1);

            if (informationAddressGpi == 1)
            {
                bool informationAddressGriRepeat = false;
                do
                {
                    MilStd47001_Address address = new MilStd47001_Address();

                    long informationAddressGri = readDatalong("INFORMATION ADDRESS GRI", 1);
                    informationAddressGriRepeat = (informationAddressGri == 1);

                    long informationAddressUrnFpi = readDatalong("INFORMATION ADDRESS URN FPI", 1);

                    if (informationAddressUrnFpi == 1)
                    {
                        address.Urn = readDatalong("INFORMATION ADDRESS URN", 24);
                    }

                    long informationAddressUnitNameFpi = readDatalong("INFORMATION ADDRESS UNIT NAME FPI", 1);

                    if (informationAddressUnitNameFpi == 1)
                    {
                        address.UnitName = readDataString("INFORMATION ADDRESS UNIT NAME", 448);
                    } // informationAddressUnitNameFpi

                    m_data.InformationAddress.Add(address);
                } while (informationAddressGriRepeat);
            } // informationAddressGpi

            long headerSizeFpi = readDatalong("HEADER SIZE FPI", 1);

            if (headerSizeFpi == 1)
            {
                m_data.HeaderSize = readDatalong("HEADER SIZE", 16);
            }

            bool messageHandlingGriRepeat = false;
            do
            {
                MilStd47001_Message message = new MilStd47001_Message();

                long messageHandlingGri = readDatalong("MESSAGE HANDLING GRI", 1);
                messageHandlingGriRepeat = (messageHandlingGri == 1);

                message.UserMessageFormat = readDatalong("USER MESSAGE FORMAT", 4);

                long messageStandardVersionFpi = readDatalong("MESSAGE STANDARD VERSION FPI", 1);

                if (messageStandardVersionFpi == 1)
                {
                    message.MessageStandardVersion = readDatalong("MESSAGE STANDARD VERSION", 4);
                }

                long messageIdentificationGpi = readDatalong("MESSAGE IDENTIFICATION GPI", 1);

                if (messageIdentificationGpi == 1)
                {
                    message.FunctionAreaDesignator = readDatalong("FUNCTION AREA DESIGNATOR", 4);

                    message.MessageNumber = readDatalong("MESSAGE NUMBER", 7);

                    long messageSubTypeFpi = readDatalong("MESSAGE SUBTYPE FPI", 1);

                    if (messageSubTypeFpi == 1)
                    {
                        message.MessageSubType = readDatalong("MESSAGE SUBTYPE", 7);
                    }
                } // messageIdentificationGpi

                long fileNameFpi = readDatalong("FILENAME FPI", 1);

                if (fileNameFpi == 1)
                {
                    message.FileName = readDataString("FILENAME", 448);
                } // fileNameFpi

                long messageSizeFpi = readDatalong("MESSAGE SIZE FPI", 1);

                if (messageSizeFpi == 1)
                {
                    message.MessageSize = readDatalong("MESSAGE SIZE", 20);
                } // messageSizeFpi

                message.OperationalIndicator = readDatalong("OPERATIONAL INDICATOR", 2);

                message.RetransmitIndicator = readDatalong("RETRANSMIT INDICATOR", 1);

                message.MessagePrecedenceCode = readDatalong("MESSAGE PRECEDENCE CODE", 3);

                message.MessageSecurityClassification = readDatalong("MESSAGE SECURITY CLASSIFICATION", 2);

                long controlReleaseMarkingFpi = readDatalong("CONTROL RELEASE MARKING FPI", 1);

                if (controlReleaseMarkingFpi == 1)
                {
                    message.ControlReleaseMarking = readDataString("CONTROL RELEASE MARKING", 14);
                } // controlReleaseMarkingFpi

                long originatorDtgGpi = readDatalong("ORIGINATOR DTG GPI", 1);

                if (originatorDtgGpi == 1)
                {
                    message.OriginatorDtg.Year = readDatalong("ORIGINATOR YEAR", 7);

                    message.OriginatorDtg.Month = readDatalong("ORIGINATOR MONTH", 4);

                    message.OriginatorDtg.Day = readDatalong("ORIGINATOR DAY", 5);

                    message.OriginatorDtg.Hour = readDatalong("ORIGINATOR HOUR", 5);

                    message.OriginatorDtg.Minute = readDatalong("ORIGINATOR MINUTE", 6);

                    message.OriginatorDtg.Second = readDatalong("ORIGINATOR SECOND", 6);

                    long originatorDtgExtensionFpi = readDatalong("ORIGINATOR DTG EXTENSION FPI", 1);

                    if (originatorDtgExtensionFpi == 1)
                    {
                        message.OriginatorDtg.DtgExtension = readDatalong("ORIGINATOR DTG EXTENSION", 12);
                    }

                } // originatorDtgGpi

                long perishabilityDtgGpi = readDatalong("PERISHABILITY DTG GPI", 1);

                if (perishabilityDtgGpi == 1)
                {
                    message.PerishabilityDtg.Year = readDatalong("PERISHABILITY DTG YEAR", 7);

                    message.PerishabilityDtg.Month = readDatalong("PERISHABILITY DTG MONTH", 4);

                    message.PerishabilityDtg.Day = readDatalong("PERISHABILITY DTG DAY", 5);

                    message.PerishabilityDtg.Hour = readDatalong("PERISHABILITY DTG HOUR", 5);

                    message.PerishabilityDtg.Minute = readDatalong("PERISHABILITY DTG MINUTE", 6);

                    message.PerishabilityDtg.Second = readDatalong("PERISHABILITY DTG SECOND", 6);
                } // perishabilityDtgGpi

                long acknowledgementRequestGroupGpi = readDatalong("ACKNOWLEDGEMENT REQUEST GROUP GPI", 1);

                if (acknowledgementRequestGroupGpi == 1)
                {
                     message.MachineAcknowledgementRequestIndicator = readDatalong("MACHINE ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    message.OperatorAcknowledgementRequestIndicator = readDatalong("OPERATOR ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    message.OperatorReplyRequestIndicator = readDatalong("OPERATOR REPLY REQUEST INDICATOR", 1);
                } // acknowledgementRequestGroupGpi

                long responseDataGroupGpi = readDatalong("RESPONSE DATA GROUP GPI", 1);

                if (responseDataGroupGpi == 1)
                {
                    message.ResponseData.Year = readDatalong("RESPONSE DATA YEAR", 7);

                    message.ResponseData.Month = readDatalong("RESPONSE DATA MONTH", 4);

                    message.ResponseData.Day = readDatalong("RESPONSE DATA DAY", 5);

                    message.ResponseData.Hour = readDatalong("RESPONSE DATA HOUR", 5);

                    message.ResponseData.Minute = readDatalong("RESPONSE DATA MINUTE", 6);

                    message.ResponseData.Second = readDatalong("RESPONSE DATA SECOND", 6);

                    long responseDataDtgExtensionFpi = readDatalong("RESPONSE DATA DTG EXTENSION FPI", 1);

                    if (responseDataDtgExtensionFpi == 1)
                    {
                        message.ResponseData.DtgExtension = readDatalong("RESPONSE DATA DTG EXTENSION", 12);
                    }

                    message.ReceiptCompliance = readDatalong("RECIPT COMPLIANCE", 1);

                    long cantcoReasonCodeFpi = readDatalong("CANTCO REASON CODE FPI", 1);

                    if (cantcoReasonCodeFpi == 1)
                    {
                        message.CantcoReasonCode = readDatalong("CANTCO REASON CODE", 3);
                    } // cantcoReasonCodeFpi

                    long cantproReasonCodeFpi = readDatalong("CANTPRO REASON CODE FPI", 1);

                    if (cantproReasonCodeFpi == 1)
                    {
                        message.CantproReasonCode = readDatalong("CANTPRO REASON CODE", 6);
                    }

                    long replyAmplificationFpi = readDatalong("REPLY AMPLIFICATION FPI", 1);

                    if (replyAmplificationFpi == 1)
                    {
                        message.ReplyAmplification = readDataString("REPLY AMPLIFICATION", 350);
                    }
                } // responseDataGroupGpi

                long referenceMessageDataGpi = readDatalong("REFERENCE MESSAGE DATA GPI", 1);

                if (referenceMessageDataGpi == 1)
                {
                    bool referenceMessageDataGriRepeat = false;
                    do
                    {
                        long referenceMessageDataGri = readDatalong("REFERENCE MESSAGE DATA GRI", 1);
                        referenceMessageDataGriRepeat = (referenceMessageDataGri == 1);

                        long referenceMessageDataUrnFpi = readDatalong("REFERENCE MESSAGE DATA URN FPI", 1);

                        if (referenceMessageDataUrnFpi == 1)
                        {
                            message.ReferenceMessageDataAddress.Urn = readDatalong("REFERENCE MESSAGE DATA URN", 24);
                        }

                        long referenceMessageDataUnitNameFpi = readDatalong("REFERENCE MESSAGE DATA UNIT NAME FPI", 1);

                        if (referenceMessageDataUnitNameFpi == 1)
                        {
                            message.ReferenceMessageDataAddress.UnitName = readDataString("REFERENCE MESSAGE DATA UNIT NAME", 448);
                        } // referenceMessageDataUnitNameFpi

                        message.ReferenceMessageDataDate.Year = readDatalong("REFERENCE DATA YEAR", 7);

                        message.ReferenceMessageDataDate.Month = readDatalong("REFERENCE DATA MONTH", 4);

                        message.ReferenceMessageDataDate.Day = readDatalong("REFERENCE DATA DAY", 5);

                        message.ReferenceMessageDataDate.Hour = readDatalong("REFERENCE DATA HOUR", 5);

                        message.ReferenceMessageDataDate.Minute = readDatalong("REFERENCE DATA MINUTE", 6);

                        message.ReferenceMessageDataDate.Second = readDatalong("REFERENCE DATA SECOND", 6);

                        long referenceDataDtgExtensionFpi = readDatalong("REFERENCE DATA DTG EXTENSION FPI", 1);

                        if (referenceDataDtgExtensionFpi == 1)
                        {
                            message.ReferenceMessageDataDate.DtgExtension = readDatalong("REFERENCE DATA DTG EXTENSION", 12);
                        }
                    } while (referenceMessageDataGriRepeat);
                }  // referenceMessageDataGpi

                long messageSecurityGpi = readDatalong("MESSAGE SECURITY GPI", 1);

                if (messageSecurityGpi == 1)
                {
                    long messageSecurityParameters = readDatalong("MESSAGE SECURITY PARAMETERS", 4);

                    long keyingMaterialGpi = readDatalong("KEYING MATERIAL GPI", 1);

                    if (keyingMaterialGpi == 1)
                    {
                        long keyingMaterialIdLength = readDatalong("KEYING MATERIAL ID LENGTH", 3);

                        byte[] keyingMaterialId = readDataArray("KEYING MATERIAL ID", 64);

                        long cryptographicInitializationGpi = readDatalong("CRYPTOGRAPHIC INITIALIZATION GPI", 1);

                        if (cryptographicInitializationGpi == 1)
                        {
                            long cryptographicInitializationLength = readDatalong("CRYPTOGRAPHIC INITIALIZATION LENGTH", 4);

                            byte[] cryptographicInitialization = readDataArray("CRYPTOGRAPHIC INITIALIZATION", 1024);
                        }
                    } // keyingMaterialGpi

                    long keyTokenGpi = readDatalong("KEY TOKEN GPI", 1);

                    if (keyTokenGpi == 1)
                    {
                        bool keyTokenFriRepeat = false;
                        do
                        {
                            long keyTokenFri = readDatalong("KEY TOKEN FRI", 1);
                            keyTokenFriRepeat = (keyTokenFri == 1);

                            byte[] keyToken = readDataArray("KEY TOKEN", 16384);

                        } while (keyTokenFriRepeat);
                    } // keyTokenGpi

                    long authentication_A_Gpi = readDatalong("AUTHENTICATION (A) GPI", 1);

                    if (authentication_A_Gpi == 1)
                    {
                        long authentication_A_Length = readDatalong("AUTHENTICATION (A) LENGTH", 7);

                        byte[] authentication_A_Data = readDataArray("AUTHENTICATION (A) DATA", 8192);
                    } // authentication_A_Gpi

                    long authentication_B_Gpi = readDatalong("AUTHENTICATION (B) GPI", 1);

                    if (authentication_B_Gpi == 1)
                    {
                        long authentication_A_Length = readDatalong("AUTHENTICATION (B) LENGTH", 7);

                        byte[] authentication_A_Data = readDataArray("AUTHENTICATION (B) DATA", 8192);

                        long authentication_B_SignedAcknowledgeRequestIndicator = readDatalong("AUTHENTICATION (B) SIGNED ACKNOWLEDGE REQUEST INDICATOR", 1);
                    } // authentication_B_Gpi

                    long messageSecurityPaddingGpi = readDatalong("MESSAGE SECURITY PADDING GPI", 1);

                    if (messageSecurityPaddingGpi == 1)
                    {
                        long messageSecurityPaddingLength = readDatalong("MESSAGE SECURITY PADDING LENGTH", 8);

                        long messageSecurityPaddingFpi = readDatalong("MESSAGE SECURITY PADDING FPI", 1);

                        if (messageSecurityPaddingFpi == 1)
                        {
                            byte[] messageSecurityPadding = readDataArray("MESSAGE SECURITY PADDING", 2040);
                        }
                    }
                } // messageSecurityGpi

                m_data.Messages.Add(message);
            } while (messageHandlingGriRepeat);

            return true;
        }

        private bool readVersionD()
        {
            long dataCompressionTypeFpi = readDatalong("DATA COMPRESSION FPI", 1);

            if (dataCompressionTypeFpi == 1)
            {
                m_data.Version = readDatalong("DATA COMPRESSION TYPE", 2);
            } // dataCompressionTypeFpi

            long originatorAddressGpi = readDatalong("ORIGINATOR ADDRESS GPI", 1);

            if (originatorAddressGpi == 1)
            {
                long originatorAddressUrnFpi = readDatalong("ORIGINATOR ADDRESS URN FPI", 1);

                if (originatorAddressUrnFpi == 1)
                {
                    m_data.OriginatorAddress.Urn = readDatalong("ORIGINATOR ADDRESS URN", 24);
                } // originatorAddressUrnFpi

                long originatorAddressUnitNameFpi = readDatalong("ORIGINATOR ADDRESS UNIT NAME FPI", 1);

                if (originatorAddressUnitNameFpi == 1)
                {
                    m_data.OriginatorAddress.UnitName = readDataString("ORIGINATOR ADDRESS UNIT NAME", 448);
                } // originatorAddressUnitNameFpi
            } // originatorAddressGpi

            long recipientAddressGpi = readDatalong("RECIPIENT ADDRESS GPI", 1);

            if (recipientAddressGpi == 1)
            {
                bool recipientAddressGriRepeat = false;
                do
                {
                    MilStd47001_Address address = new MilStd47001_Address();

                    long recipientAddressGri = readDatalong("RECIPIENT ADDRESS GRI", 1);
                    recipientAddressGriRepeat = (recipientAddressGri == 1);

                    long recipientAddressUrnFpi = readDatalong("RECIPIENT ADDRESS URN FPI", 1);

                    if (recipientAddressUrnFpi == 1)
                    {
                        address.Urn = readDatalong("RECIPIENT ADDRESS URN", 24);
                    }

                    long recipientAddressUnitNameFpi = readDatalong("RECIPIENT ADDRESS UNIT NAME FPI", 1);

                    if (recipientAddressUnitNameFpi == 1)
                    {
                        address.UnitName = readDataString("RECIPIENT ADDRESS UNIT NAME", 448);
                    } // recipientAddressUnitNameFpi

                    m_data.RecipientAddress.Add(address);
                } while (recipientAddressGriRepeat);
            } // recipientAddressGpi

            long informationAddressGpi = readDatalong("INFORMATION ADDRESS GPI", 1);

            if (informationAddressGpi == 1)
            {
                bool informationAddressGriRepeat = false;
                do
                {
                    MilStd47001_Address address = new MilStd47001_Address();

                    long informationAddressGri = readDatalong("INFORMATION ADDRESS GRI", 1);
                    informationAddressGriRepeat = (informationAddressGri == 1);

                    long informationAddressUrnFpi = readDatalong("INFORMATION ADDRESS URN FPI", 1);

                    if (informationAddressUrnFpi == 1)
                    {
                        address.Urn = readDatalong("INFORMATION ADDRESS URN", 24);
                    }

                    long informationAddressUnitNameFpi = readDatalong("INFORMATION ADDRESS UNIT NAME FPI", 1);

                    if (informationAddressUnitNameFpi == 1)
                    {
                        address.UnitName = readDataString("INFORMATION ADDRESS UNIT NAME", 448);
                    } // informationAddressUnitNameFpi

                    m_data.InformationAddress.Add(address);
                } while (informationAddressGriRepeat);
            } // informationAddressGpi

            long headerSizeFpi = readDatalong("HEADER SIZE FPI", 1);

            if (headerSizeFpi == 1)
            {
                m_data.HeaderSize = readDatalong("HEADER SIZE", 16);
            }

            long futureUse1Gpi = readDatalong("FUTURE USE 1 GPI", 1);

            if (futureUse1Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 1 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 1 GROUP DATA", (int) groupSize);

            } // futureUse1Gpi

            long futureUse2Gpi = readDatalong("FUTURE USE 2 GPI", 1);

            if (futureUse2Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 2 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 2 GROUP DATA", (int) groupSize);

            } // futureUse2Gpi

            long futureUse3Gpi = readDatalong("FUTURE USE 3 GPI", 1);

            if (futureUse3Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 3 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 3 GROUP DATA", (int) groupSize);

            } // futureUse3Gpi

            long futureUse4Gpi = readDatalong("FUTURE USE 4 GPI", 1);

            if (futureUse4Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 4 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 4 GROUP DATA", (int) groupSize);

            } // futureUse4Gpi

            long futureUse5Gpi = readDatalong("FUTURE USE 5 GPI", 1);

            if (futureUse5Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 5 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 5 GROUP DATA", (int) groupSize);

            } // futureUse5Gpi

            bool messageHandlingGriRepeat = false;
            do
            {
                MilStd47001_Message message = new MilStd47001_Message();

                long messageHandlingGri = readDatalong("MESSAGE HANDLING GRI", 1);
                messageHandlingGriRepeat = (messageHandlingGri == 1);

                message.UserMessageFormat = readDatalong("USER MESSAGE FORMAT", 4);

                long messageStandardVersionFpi = readDatalong("MESSAGE STANDARD VERSION FPI", 1);

                if (messageStandardVersionFpi == 1)
                {
                    message.MessageStandardVersion = readDatalong("MESSAGE STANDARD VERSION", 4);
                }

                long messageIdentificationGpi = readDatalong("MESSAGE IDENTIFICATION GPI", 1);

                if (messageIdentificationGpi == 1)
                {
                    message.FunctionAreaDesignator = readDatalong("FUNCTION AREA DESIGNATOR", 4);

                    message.MessageNumber = readDatalong("MESSAGE NUMBER", 7);

                    long messageSubTypeFpi = readDatalong("MESSAGE SUBTYPE FPI", 1);

                    if (messageSubTypeFpi == 1)
                    {
                        message.MessageSubType = readDatalong("MESSAGE SUBTYPE", 7);
                    }
                } // messageIdentificationGpi

                long fileNameFpi = readDatalong("FILENAME FPI", 1);

                if (fileNameFpi == 1)
                {
                    message.FileName = readDataString("FILENAME", 448);
                } // fileNameFpi

                long messageSizeFpi = readDatalong("MESSAGE SIZE FPI", 1);

                if (messageSizeFpi == 1)
                {
                    message.MessageSize = readDatalong("MESSAGE SIZE", 20);
                } // messageSizeFpi

                message.OperationalIndicator = readDatalong("OPERATIONAL INDICATOR", 2);

                message.RetransmitIndicator = readDatalong("RETRANSMIT INDICATOR", 1);

                message.MessagePrecedenceCode = readDatalong("MESSAGE PRECEDENCE CODE", 3);

                message.MessageSecurityClassification = readDatalong("MESSAGE SECURITY CLASSIFICATION", 2);

                long controlReleaseMarkingFpi = readDatalong("CONTROL RELEASE MARKING FPI", 1);

                if (controlReleaseMarkingFpi == 1)
                {
                    bool controlReleaseMarkingFriRepeat = false;
                    do
                    {
                        long controlReleaseMarkingFri = readDatalong("CONTROL RELEASE MARKING FRI", 1);
                        controlReleaseMarkingFriRepeat = (controlReleaseMarkingFri == 1);

                        long controlReleaseMarking = readDatalong("CONTROL RELEASE MARKING", 9);
                        message.ControlReleaseMarkingList.Add(controlReleaseMarking);

                    } while (controlReleaseMarkingFriRepeat);
                } // controlReleaseMarkingFpi

                long originatorDtgGpi = readDatalong("ORIGINATOR DTG GPI", 1);

                if (originatorAddressGpi == 1)
                {
                    message.OriginatorDtg.Year = readDatalong("ORIGINATOR YEAR", 7);

                    message.OriginatorDtg.Month = readDatalong("ORIGINATOR MONTH", 4);

                    message.OriginatorDtg.Day = readDatalong("ORIGINATOR DAY", 5);

                    message.OriginatorDtg.Hour = readDatalong("ORIGINATOR HOUR", 5);

                    message.OriginatorDtg.Minute = readDatalong("ORIGINATOR MINUTE", 6);

                    message.OriginatorDtg.Second = readDatalong("ORIGINATOR SECOND", 6);

                    long originatorDtgExtensionFpi = readDatalong("ORIGINATOR DTG EXTENSION FPI", 1);

                    if (originatorDtgExtensionFpi == 1)
                    {
                        message.OriginatorDtg.DtgExtension = readDatalong("ORIGINATOR DTG EXTENSION", 12);
                    }

                } // originatorDtgGpi

                long perishabilityDtgGpi = readDatalong("PERISHABILITY DTG GPI", 1);

                if (perishabilityDtgGpi == 1)
                {

                    message.PerishabilityDtg.Year = readDatalong("PERISHABILITY DTG YEAR", 7);

                    message.PerishabilityDtg.Month = readDatalong("PERISHABILITY DTG MONTH", 4);

                    message.PerishabilityDtg.Day = readDatalong("PERISHABILITY DTG DAY", 5);

                    message.PerishabilityDtg.Hour = readDatalong("PERISHABILITY DTG HOUR", 5);

                    message.PerishabilityDtg.Minute = readDatalong("PERISHABILITY DTG MINUTE", 6);

                    message.PerishabilityDtg.Second = readDatalong("PERISHABILITY DTG SECOND", 6);
                } // perishabilityDtgGpi

                long acknowledgementRequestGroupGpi = readDatalong("ACKNOWLEDGEMENT REQUEST GROUP GPI", 1);

                if (acknowledgementRequestGroupGpi == 1)
                {
                    message.MachineAcknowledgementRequestIndicator = readDatalong("MACHINE ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    message.OperatorAcknowledgementRequestIndicator = readDatalong("OPERATOR ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    message.OperatorReplyRequestIndicator = readDatalong("OPERATOR REPLY REQUEST INDICATOR", 1);
                } // acknowledgementRequestGroupGpi

                long responseDataGroupGpi = readDatalong("RESPONSE DATA GROUP GPI", 1);

                if (responseDataGroupGpi == 1)
                {
                    message.ResponseData.Year = readDatalong("RESPONSE DATA YEAR", 7);

                    message.ResponseData.Month = readDatalong("RESPONSE DATA MONTH", 4);

                    message.ResponseData.Day = readDatalong("RESPONSE DATA DAY", 5);

                    message.ResponseData.Hour = readDatalong("RESPONSE DATA HOUR", 5);

                    message.ResponseData.Minute = readDatalong("RESPONSE DATA MINUTE", 6);

                    message.ResponseData.Second = readDatalong("RESPONSE DATA SECOND", 6);

                    long responseDataDtgExtensionFpi = readDatalong("RESPONSE DATA DTG EXTENSION FPI", 1);

                    if (responseDataDtgExtensionFpi == 1)
                    {
                        message.ResponseData.DtgExtension = readDatalong("RESPONSE DATA DTG EXTENSION", 12);
                    }

                    message.ReceiptCompliance = readDatalong("RECIPT COMPLIANCE", 3);

                    long cantcoReasonCodeFpi = readDatalong("CANTCO REASON CODE FPI", 1);

                    if (cantcoReasonCodeFpi == 1)
                    {
                        message.CantcoReasonCode = readDatalong("CANTCO REASON CODE", 3);
                    } // cantcoReasonCodeFpi

                    long cantproReasonCodeFpi = readDatalong("CANTPRO REASON CODE FPI", 1);

                    if (cantproReasonCodeFpi == 1)
                    {
                        message.CantproReasonCode = readDatalong("CANTPRO REASON CODE", 6);
                    }

                    long replyAmplificationFpi = readDatalong("REPLY AMPLIFICATION FPI", 1);

                    if (replyAmplificationFpi == 1)
                    {
                        message.ReplyAmplification = readDataString("REPLY AMPLIFICATION", 350);
                    }
                } // responseDataGroupGpi

                long referenceMessageDataGpi = readDatalong("REFERENCE MESSAGE DATA GPI", 1);

                if (referenceMessageDataGpi == 1)
                {
                    bool referenceMessageDataGriRepeat = false;
                    do
                    {
                        long referenceMessageDataGri = readDatalong("REFERENCE MESSAGE DATA GRI", 1);
                        referenceMessageDataGriRepeat = (referenceMessageDataGri == 1);

                        long referenceMessageDataUrnFpi = readDatalong("REFERENCE MESSAGE DATA URN FPI", 1);

                        if (referenceMessageDataUrnFpi == 1)
                        {
                            message.ReferenceMessageDataAddress.Urn = readDatalong("REFERENCE MESSAGE DATA URN", 24);
                        }

                        long referenceMessageDataUnitNameFpi = readDatalong("REFERENCE MESSAGE DATA UNIT NAME FPI", 1);

                        if (referenceMessageDataUnitNameFpi == 1)
                        {
                            message.ReferenceMessageDataAddress.UnitName = readDataString("REFERENCE MESSAGE DATA UNIT NAME", 448);
                        } // referenceMessageDataUnitNameFpi

                        message.ReferenceMessageDataDate.Year = readDatalong("REFERENCE DATA YEAR", 7);

                        message.ReferenceMessageDataDate.Month = readDatalong("REFERENCE DATA MONTH", 4);

                        message.ReferenceMessageDataDate.Day = readDatalong("REFERENCE DATA DAY", 5);

                        message.ReferenceMessageDataDate.Hour = readDatalong("REFERENCE DATA HOUR", 5);

                        message.ReferenceMessageDataDate.Minute = readDatalong("REFERENCE DATA MINUTE", 6);

                        message.ReferenceMessageDataDate.Second = readDatalong("REFERENCE DATA SECOND", 6);

                        long referenceDataDtgExtensionFpi = readDatalong("REFERENCE DATA DTG EXTENSION FPI", 1);

                        if (referenceDataDtgExtensionFpi == 1)
                        {
                            message.ReferenceMessageDataDate.DtgExtension = readDatalong("REFERENCE DATA DTG EXTENSION", 12);
                        }
                    } while (referenceMessageDataGriRepeat);
                }  // referenceMessageDataGpi

                long futureUse6Gpi = readDatalong("FUTURE USE 6 GPI", 1);

                if (futureUse6Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 6 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 6 GROUP DATA", (int) groupSize);

                } // futureUse6Gpi

                long futureUse7Gpi = readDatalong("FUTURE USE 7 GPI", 1);

                if (futureUse7Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 7 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 7 GROUP DATA", (int) groupSize);

                } // futureUse7Gpi

                long futureUse8Gpi = readDatalong("FUTURE USE 8 GPI", 1);

                if (futureUse8Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 8 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 8 GROUP DATA", (int) groupSize);
                } // futureUse8Gpi

                long futureUse9Gpi = readDatalong("FUTURE USE 9 GPI", 1);

                if (futureUse9Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 9 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 9 GROUP DATA", (int) groupSize);

                } // futureUse9Gpi

                long futureUse10Gpi = readDatalong("FUTURE USE 10 GPI", 1);

                if (futureUse10Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 10 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 10 GROUP DATA", (int) groupSize);
                } // futureUse10Gpi

                long messageSecurityGpi = readDatalong("MESSAGE SECURITY GPI", 1);

                if (messageSecurityGpi == 1)
                {
                    long messageSecurityParameters = readDatalong("MESSAGE SECURITY PARAMETERS", 4);

                    long keyingMaterialGpi = readDatalong("KEYING MATERIAL GPI", 1);

                    if (keyingMaterialGpi == 1)
                    {
                        long keyingMaterialIdLength = readDatalong("KEYING MATERIAL ID LENGTH", 3);

                        byte[] keyingMaterialId = readDataArray("KEYING MATERIAL ID", 64);

                        long cryptographicInitializationGpi = readDatalong("CRYPTOGRAPHIC INITIALIZATION GPI", 1);

                        if (cryptographicInitializationGpi == 1)
                        {
                            long cryptographicInitializationLength = readDatalong("CRYPTOGRAPHIC INITIALIZATION LENGTH", 4);

                            byte[] cryptographicInitialization = readDataArray("CRYPTOGRAPHIC INITIALIZATION", 1024);
                        }
                    } // keyingMaterialGpi

                    long keyTokenGpi = readDatalong("KEY TOKEN GPI", 1);

                    if (keyTokenGpi == 1)
                    {
                        bool keyTokenFriRepeat = false;
                        do
                        {
                            long keyTokenFri = readDatalong("KEY TOKEN FRI", 1);
                            keyTokenFriRepeat = (keyTokenFri == 1);

                            byte[] keyToken = readDataArray("KEY TOKEN", 16384);

                        } while (keyTokenFriRepeat);
                    } // keyTokenGpi

                    long authentication_A_Gpi = readDatalong("AUTHENTICATION (A) GPI", 1);

                    if (authentication_A_Gpi == 1)
                    {
                        long authentication_A_Length = readDatalong("AUTHENTICATION (A) LENGTH", 7);

                        byte[] authentication_A_Data = readDataArray("AUTHENTICATION (A) DATA", 8192);
                    } // authentication_A_Gpi

                    long authentication_B_Gpi = readDatalong("AUTHENTICATION (B) GPI", 1);

                    if (authentication_B_Gpi == 1)
                    {
                        long authentication_A_Length = readDatalong("AUTHENTICATION (B) LENGTH", 7);

                        byte[] authentication_A_Data = readDataArray("AUTHENTICATION (B) DATA", 8192);

                        long authentication_B_SignedAcknowledgeRequestIndicator = readDatalong("AUTHENTICATION (B) SIGNED ACKNOWLEDGE REQUEST INDICATOR", 1);
                    } // authentication_B_Gpi

                    long messageSecurityPaddingGpi = readDatalong("MESSAGE SECURITY PADDING GPI", 1);

                    if (messageSecurityPaddingGpi == 1)
                    {
                        long messageSecurityPaddingLength = readDatalong("MESSAGE SECURITY PADDING LENGTH", 8);

                        long messageSecurityPaddingFpi = readDatalong("MESSAGE SECURITY PADDING FPI", 1);

                        if (messageSecurityPaddingFpi == 1)
                        {
                            byte[] messageSecurityPadding = readDataArray("MESSAGE SECURITY PADDING", 2040);
                        }
                    }
                } // messageSecurityGpi

                m_data.Messages.Add(message);
            } while (messageHandlingGriRepeat);

            return true;
        }

        private bool readVersionDChange1()
        {
            long dataCompressionTypeFpi = readDatalong("DATA COMPRESSION FPI", 1);

            if (dataCompressionTypeFpi == 1)
            {
                m_data.IsDataCompressionSet = true;
                m_data.DataCompression = readDatalong("DATA COMPRESSION TYPE", 2);
            } // dataCompressionTypeFpi

            long originatorAddressGpi = readDatalong("ORIGINATOR ADDRESS GPI", 1);

            if (originatorAddressGpi == 1)
            {
                long originatorAddressUrnFpi = readDatalong("ORIGINATOR ADDRESS URN FPI", 1);

                if (originatorAddressUrnFpi == 1)
                {
                    m_data.OriginatorAddress.Urn = readDatalong("ORIGINATOR ADDRESS URN", 24);
                } // originatorAddressUrnFpi

                long originatorAddressUnitNameFpi = readDatalong("ORIGINATOR ADDRESS UNIT NAME FPI", 1);

                if (originatorAddressUnitNameFpi == 1)
                {
                    m_data.OriginatorAddress.UnitName = readDataString("ORIGINATOR ADDRESS UNIT NAME", 448);
                } // originatorAddressUnitNameFpi
            } // originatorAddressGpi

            long recipientAddressGpi = readDatalong("RECIPIENT ADDRESS GPI", 1);

            if (recipientAddressGpi == 1)
            {
                bool recipientAddressGriRepeat = false;
                do
                {
                    MilStd47001_Address address = new MilStd47001_Address();

                    long recipientAddressGri = readDatalong("RECIPIENT ADDRESS GRI", 1);
                    recipientAddressGriRepeat = (recipientAddressGri == 1);

                    long recipientAddressUrnFpi = readDatalong("RECIPIENT ADDRESS URN FPI", 1);

                    if (recipientAddressUrnFpi == 1)
                    {
                        address.Urn = readDatalong("RECIPIENT ADDRESS URN", 24);
                    }

                    long recipientAddressUnitNameFpi = readDatalong("RECIPIENT ADDRESS UNIT NAME FPI", 1);

                    if (recipientAddressUnitNameFpi == 1)
                    {
                        address.UnitName = readDataString("RECIPIENT ADDRESS UNIT NAME", 448);
                    } // recipientAddressUnitNameFpi

                    m_data.RecipientAddress.Add(address);
                } while (recipientAddressGriRepeat);
            } // recipientAddressGpi

            long informationAddressGpi = readDatalong("INFORMATION ADDRESS GPI", 1);

            if (informationAddressGpi == 1)
            {
                bool informationAddressGriRepeat = false;
                do
                {
                    MilStd47001_Address address = new MilStd47001_Address();

                    long informationAddressGri = readDatalong("INFORMATION ADDRESS GRI", 1);
                    informationAddressGriRepeat = (informationAddressGri == 1);

                    long informationAddressUrnFpi = readDatalong("INFORMATION ADDRESS URN FPI", 1);

                    if (informationAddressUrnFpi == 1)
                    {
                        address.Urn = readDatalong("INFORMATION ADDRESS URN", 24);
                    }

                    long informationAddressUnitNameFpi = readDatalong("INFORMATION ADDRESS UNIT NAME FPI", 1);

                    if (informationAddressUnitNameFpi == 1)
                    {
                        address.UnitName = readDataString("INFORMATION ADDRESS UNIT NAME", 448);
                    } // informationAddressUnitNameFpi

                    m_data.InformationAddress.Add(address);
                } while (informationAddressGriRepeat);
            } // informationAddressGpi

            long headerSizeFpi = readDatalong("HEADER SIZE FPI", 1);

            if (headerSizeFpi == 1)
            {
                m_data.HeaderSize = readDatalong("HEADER SIZE", 16);
            }

            long futureUse1Gpi = readDatalong("FUTURE USE 1 GPI", 1);

            if (futureUse1Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 1 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 1 GROUP DATA", (int) groupSize);

            } // futureUse1Gpi

            long futureUse2Gpi = readDatalong("FUTURE USE 2 GPI", 1);

            if (futureUse2Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 2 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 2 GROUP DATA", (int) groupSize);

            } // futureUse2Gpi

            long futureUse3Gpi = readDatalong("FUTURE USE 3 GPI", 1);

            if (futureUse3Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 3 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 3 GROUP DATA", (int) groupSize);

            } // futureUse3Gpi

            long futureUse4Gpi = readDatalong("FUTURE USE 4 GPI", 1);

            if (futureUse4Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 4 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 4 GROUP DATA", (int) groupSize);

            } // futureUse4Gpi

            long futureUse5Gpi = readDatalong("FUTURE USE 5 GPI", 1);

            if (futureUse5Gpi == 1)
            {
                long groupSize = readDatalong("FUTURE USE 5 GROUP SIZE", 12);

                byte[] futureUse10Data = readDataArray("FUTURE 5 GROUP DATA", (int) groupSize);

            } // futureUse5Gpi

            bool messageHandlingGriRepeat = false;
            do
            {
                MilStd47001_Message message = new MilStd47001_Message();

                long messageHandlingGri = readDatalong("MESSAGE HANDLING GRI", 1);
                messageHandlingGriRepeat = (messageHandlingGri == 1);
                
                message.UserMessageFormat = readDatalong("USER MESSAGE FORMAT", 4);

                long messageStandardVersionFpi = readDatalong("MESSAGE STANDARD VERSION FPI", 1);

                if (messageStandardVersionFpi == 1)
                {
                    message.MessageStandardVersion = readDatalong("MESSAGE STANDARD VERSION", 4);
                }

                long messageIdentificationGpi = readDatalong("MESSAGE IDENTIFICATION GPI", 1);

                if (messageIdentificationGpi == 1)
                {
                    message.FunctionAreaDesignator = readDatalong("FUNCTION AREA DESIGNATOR", 4);

                    message.MessageNumber = readDatalong("MESSAGE NUMBER", 7);

                    long messageSubTypeFpi = readDatalong("MESSAGE SUBTYPE FPI", 1);

                    if (messageSubTypeFpi == 1)
                    {
                        message.MessageSubType = readDatalong("MESSAGE SUBTYPE", 7);
                    }
                } // messageIdentificationGpi

                long fileNameFpi = readDatalong("FILENAME FPI", 1);

                if (fileNameFpi == 1)
                {
                    message.FileName = readDataString("FILENAME", 448);
                } // fileNameFpi

                long messageSizeFpi = readDatalong("MESSAGE SIZE FPI", 1);

                if (messageSizeFpi == 1)
                {
                    message.MessageSize = readDatalong("MESSAGE SIZE", 20);
                } // messageSizeFpi

                message.OperationalIndicator = readDatalong("OPERATIONAL INDICATOR", 2);

                message.RetransmitIndicator = readDatalong("RETRANSMIT INDICATOR", 1);

                message.MessagePrecedenceCode = readDatalong("MESSAGE PRECEDENCE CODE", 3);

                message.MessageSecurityClassification = readDatalong("MESSAGE SECURITY CLASSIFICATION", 2);

                long controlReleaseMarkingFpi = readDatalong("CONTROL RELEASE MARKING FPI", 1);

                if (controlReleaseMarkingFpi == 1)
                {
                    bool controlReleaseMarkingFriRepeat = false;
                    do
                    {
                        long controlReleaseMarkingFri = readDatalong("CONTROL RELEASE MARKING FRI", 1);
                        controlReleaseMarkingFriRepeat = (controlReleaseMarkingFri == 1);

                        long controlReleaseMarking = readDatalong("CONTROL RELEASE MARKING", 9);
                        message.ControlReleaseMarkingList.Add(controlReleaseMarking);

                    } while (controlReleaseMarkingFriRepeat);
                } // controlReleaseMarkingFpi

                long originatorDtgGpi = readDatalong("ORIGINATOR DTG GPI", 1);

                if (originatorAddressGpi == 1)
                {
                    message.OriginatorDtg.Year = readDatalong("ORIGINATOR YEAR", 7);

                    message.OriginatorDtg.Month = readDatalong("ORIGINATOR MONTH", 4);

                    message.OriginatorDtg.Day = readDatalong("ORIGINATOR DAY", 5);

                    message.OriginatorDtg.Hour = readDatalong("ORIGINATOR HOUR", 5);

                    message.OriginatorDtg.Minute = readDatalong("ORIGINATOR MINUTE", 6);

                    message.OriginatorDtg.Second = readDatalong("ORIGINATOR SECOND", 6);

                    long originatorDtgExtensionFpi = readDatalong("ORIGINATOR DTG EXTENSION FPI", 1);

                    if (originatorDtgExtensionFpi == 1)
                    {
                        message.OriginatorDtg.DtgExtension = readDatalong("ORIGINATOR DTG EXTENSION", 12);
                    }

                } // originatorDtgGpi

                long perishabilityDtgGpi = readDatalong("PERISHABILITY DTG GPI", 1);

                if (perishabilityDtgGpi == 1)
                {

                    message.PerishabilityDtg.Year = readDatalong("PERISHABILITY DTG YEAR", 7);

                    message.PerishabilityDtg.Month = readDatalong("PERISHABILITY DTG MONTH", 4);

                    message.PerishabilityDtg.Day = readDatalong("PERISHABILITY DTG DAY", 5);

                    message.PerishabilityDtg.Hour = readDatalong("PERISHABILITY DTG HOUR", 5);

                    message.PerishabilityDtg.Minute = readDatalong("PERISHABILITY DTG MINUTE", 6);

                    message.PerishabilityDtg.Second = readDatalong("PERISHABILITY DTG SECOND", 6);
                } // perishabilityDtgGpi

                long acknowledgementRequestGroupGpi = readDatalong("ACKNOWLEDGEMENT REQUEST GROUP GPI", 1);

                if (acknowledgementRequestGroupGpi == 1)
                {
                    message.MachineAcknowledgementRequestIndicator = readDatalong("MACHINE ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    message.OperatorAcknowledgementRequestIndicator = readDatalong("OPERATOR ACKNOWLEDGEMENT REQUEST INDICATOR", 1);

                    message.OperatorReplyRequestIndicator = readDatalong("OPERATOR REPLY REQUEST INDICATOR", 1);
                } // acknowledgementRequestGroupGpi

                long responseDataGroupGpi = readDatalong("RESPONSE DATA GROUP GPI", 1);

                if (responseDataGroupGpi == 1)
                {
                    message.ResponseData.Year = readDatalong("RESPONSE DATA YEAR", 7);

                    message.ResponseData.Month = readDatalong("RESPONSE DATA MONTH", 4);

                    message.ResponseData.Day = readDatalong("RESPONSE DATA DAY", 5);

                    message.ResponseData.Hour = readDatalong("RESPONSE DATA HOUR", 5);

                    message.ResponseData.Minute = readDatalong("RESPONSE DATA MINUTE", 6);

                    message.ResponseData.Second = readDatalong("RESPONSE DATA SECOND", 6);

                    long responseDataDtgExtensionFpi = readDatalong("RESPONSE DATA DTG EXTENSION FPI", 1);

                    if (responseDataDtgExtensionFpi == 1)
                    {
                        message.ResponseData.DtgExtension = readDatalong("RESPONSE DATA DTG EXTENSION", 12);
                    }

                    message.ReceiptCompliance = readDatalong("RECIPT COMPLIANCE", 3);

                    long cantcoReasonCodeFpi = readDatalong("CANTCO REASON CODE FPI", 1);

                    if (cantcoReasonCodeFpi == 1)
                    {
                        message.CantcoReasonCode = readDatalong("CANTCO REASON CODE", 3);
                    } // cantcoReasonCodeFpi

                    long cantproReasonCodeFpi = readDatalong("CANTPRO REASON CODE FPI", 1);

                    if (cantproReasonCodeFpi == 1)
                    {
                        message.CantproReasonCode = readDatalong("CANTPRO REASON CODE", 6);
                    }

                    long replyAmplificationFpi = readDatalong("REPLY AMPLIFICATION FPI", 1);

                    if (replyAmplificationFpi == 1)
                    {
                        message.ReplyAmplification = readDataString("REPLY AMPLIFICATION", 350);
                    }
                } // responseDataGroupGpi

                long referenceMessageDataGpi = readDatalong("REFERENCE MESSAGE DATA GPI", 1);

                if (referenceMessageDataGpi == 1)
                {
                    bool referenceMessageDataGriRepeat = false;
                    do
                    {
                        long referenceMessageDataGri = readDatalong("REFERENCE MESSAGE DATA GRI", 1);
                        referenceMessageDataGriRepeat = (referenceMessageDataGri == 1);

                        long referenceMessageDataUrnFpi = readDatalong("REFERENCE MESSAGE DATA URN FPI", 1);

                        if (referenceMessageDataUrnFpi == 1)
                        {
                            message.ReferenceMessageDataAddress.Urn = readDatalong("REFERENCE MESSAGE DATA URN", 24);
                        }

                        long referenceMessageDataUnitNameFpi = readDatalong("REFERENCE MESSAGE DATA UNIT NAME FPI", 1);

                        if (referenceMessageDataUnitNameFpi == 1)
                        {
                            message.ReferenceMessageDataAddress.UnitName = readDataString("REFERENCE MESSAGE DATA UNIT NAME", 448);
                        } // referenceMessageDataUnitNameFpi

                        message.ReferenceMessageDataDate.Year = readDatalong("REFERENCE DATA YEAR", 7);

                        message.ReferenceMessageDataDate.Month = readDatalong("REFERENCE DATA MONTH", 4);

                        message.ReferenceMessageDataDate.Day = readDatalong("REFERENCE DATA DAY", 5);

                        message.ReferenceMessageDataDate.Hour = readDatalong("REFERENCE DATA HOUR", 5);

                        message.ReferenceMessageDataDate.Minute = readDatalong("REFERENCE DATA MINUTE", 6);

                        message.ReferenceMessageDataDate.Second = readDatalong("REFERENCE DATA SECOND", 6);

                        long referenceDataDtgExtensionFpi = readDatalong("REFERENCE DATA DTG EXTENSION FPI", 1);

                        if (referenceDataDtgExtensionFpi == 1)
                        {
                            message.ReferenceMessageDataDate.DtgExtension = readDatalong("REFERENCE DATA DTG EXTENSION", 12);
                        }
                    } while (referenceMessageDataGriRepeat);
                }  // referenceMessageDataGpi

                long futureUse6Gpi = readDatalong("FUTURE USE 6 GPI", 1);

                if (futureUse6Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 6 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 6 GROUP DATA", (int) groupSize);

                } // futureUse6Gpi

                long futureUse7Gpi = readDatalong("FUTURE USE 7 GPI", 1);

                if (futureUse7Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 7 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 7 GROUP DATA", (int) groupSize);

                } // futureUse7Gpi

                long futureUse8Gpi = readDatalong("FUTURE USE 8 GPI", 1);

                if (futureUse8Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 8 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 8 GROUP DATA", (int) groupSize);
                } // futureUse8Gpi

                long futureUse9Gpi = readDatalong("FUTURE USE 9 GPI", 1);

                if (futureUse9Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 9 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 9 GROUP DATA", (int) groupSize);

                } // futureUse9Gpi

                long futureUse10Gpi = readDatalong("FUTURE USE 10 GPI", 1);

                if (futureUse10Gpi == 1)
                {
                    long groupSize = readDatalong("FUTURE USE 10 GROUP SIZE", 12);

                    byte[] futureUse10Data = readDataArray("FUTURE 10 GROUP DATA", (int) groupSize);
                } // futureUse10Gpi

                long messageSecurityGpi = readDatalong("MESSAGE SECURITY GPI", 1);

                if (messageSecurityGpi == 1)
                {
                    long messageSecurityParameters = readDatalong("MESSAGE SECURITY PARAMETERS", 4);

                    long keyingMaterialGpi = readDatalong("KEYING MATERIAL GPI", 1);

                    if (keyingMaterialGpi == 1)
                    {
                        long keyingMaterialIdLength = readDatalong("KEYING MATERIAL ID LENGTH", 3);

                        byte[] keyingMaterialId = readDataArray("KEYING MATERIAL ID", 64);

                        long cryptographicInitializationGpi = readDatalong("CRYPTOGRAPHIC INITIALIZATION GPI", 1);

                        if (cryptographicInitializationGpi == 1)
                        {
                            long cryptographicInitializationLength = readDatalong("CRYPTOGRAPHIC INITIALIZATION LENGTH", 4);

                            byte[] cryptographicInitialization = readDataArray("CRYPTOGRAPHIC INITIALIZATION", 1024);
                        }
                    } // keyingMaterialGpi

                    long keyTokenGpi = readDatalong("KEY TOKEN GPI", 1);

                    if (keyTokenGpi == 1)
                    {
                        bool keyTokenFriRepeat = false;
                        do
                        {
                            long keyTokenFri = readDatalong("KEY TOKEN FRI", 1);
                            keyTokenFriRepeat = (keyTokenFri == 1);

                            byte[] keyToken = readDataArray("KEY TOKEN", 16384);

                        } while (keyTokenFriRepeat);
                    } // keyTokenGpi

                    long authentication_A_Gpi = readDatalong("AUTHENTICATION (A) GPI", 1);

                    if (authentication_A_Gpi == 1)
                    {
                        long authentication_A_Length = readDatalong("AUTHENTICATION (A) LENGTH", 7);

                        byte[] authentication_A_Data = readDataArray("AUTHENTICATION (A) DATA", 8192);
                    } // authentication_A_Gpi

                    long authentication_B_Gpi = readDatalong("AUTHENTICATION (B) GPI", 1);

                    if (authentication_B_Gpi == 1)
                    {
                        long authentication_A_Length = readDatalong("AUTHENTICATION (B) LENGTH", 7);

                        byte[] authentication_A_Data = readDataArray("AUTHENTICATION (B) DATA", 8192);

                        long authentication_B_SignedAcknowledgeRequestIndicator = readDatalong("AUTHENTICATION (B) SIGNED ACKNOWLEDGE REQUEST INDICATOR", 1);
                    } // authentication_B_Gpi

                    long messageSecurityPaddingGpi = readDatalong("MESSAGE SECURITY PADDING GPI", 1);

                    if (messageSecurityPaddingGpi == 1)
                    {
                        long messageSecurityPaddingLength = readDatalong("MESSAGE SECURITY PADDING LENGTH", 8);

                        long messageSecurityPaddingFpi = readDatalong("MESSAGE SECURITY PADDING FPI", 1);

                        if (messageSecurityPaddingFpi == 1)
                        {
                            byte[] messageSecurityPadding = readDataArray("MESSAGE SECURITY PADDING", 2040);
                        }
                    }
                } // messageSecurityGpi

                m_data.Messages.Add(message);
            } while (messageHandlingGriRepeat);

            return true;
        }
        #endregion

        #region Compression/Decompression Methods
        public byte[] compress_lz77(byte[] data)
        {
            return LZ77.Compress(data);
        }
    
        public byte[] decompress_lz77(byte[] data)
        {
            return LZ77.Decompress(data);
        }

        public List<long> compress_lzw(string data)
        {
            return LZW.Compress(data);
        }

        public string decompress_lzw(List<long> data)
        {
            return LZW.Decompress(data);
        }
        #endregion
    }
}
