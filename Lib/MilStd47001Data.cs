using System;
using System.Collections.Generic;

namespace IMAS.Core.Parser.VMF.Lib
{

    [Serializable]
    public class MilStd47001_Address
    {
        public long Urn { get; set; }
        public string UnitName { get; set; }
    }

    
    [Serializable]
    public class MilStd47001_Date
    {
        public long Year { get; set; }
        public long Month { get; set; }
        public long Day { get; set; }
        public long Hour { get; set; }
        public long Minute { get; set; }
        public long Second { get; set; }
        public long DtgExtension { get; set; }
    }
    
    [Serializable]
    public class MilStd47001_Message
    {
        public MilStd47001_Message()
        {
            OriginatorDtg = new MilStd47001_Date();
            PerishabilityDtg = new MilStd47001_Date();
            ResponseData = new MilStd47001_Date();
            ControlReleaseMarkingList = new List<long>();
            ReferenceMessageDataAddress = new MilStd47001_Address();
            ReferenceMessageDataDate = new MilStd47001_Date();
        }
        
        public long UserMessageFormat { get; set; }
        public long MessageStandardVersion { get; set; }
        public long FunctionAreaDesignator { get; set; }
        public long MessageNumber { get; set; }
        public long MessageSubType { get; set; }
        
        public string FileName { get; set; }
        
        public long MessageSize { get; set; }
        
        public long OperationalIndicator { get; set; }
        public long RetransmitIndicator { get; set; }
        public long MessagePrecedenceCode { get; set; }
        public long MessageSecurityClassification { get; set; }
        
        public string ControlReleaseMarking { get; set; }
        public List<long> ControlReleaseMarkingList { get; set; }
        
        public MilStd47001_Date OriginatorDtg { get; set; }
        public MilStd47001_Date PerishabilityDtg { get; set; }

        public long MachineAcknowledgementRequestIndicator { get; set; }
        public long OperatorAcknowledgementRequestIndicator { get; set; }
        public long OperatorReplyRequestIndicator { get; set; }

        public MilStd47001_Date ResponseData { get; set; }

        public long ReceiptCompliance { get; set; }
        public long CantcoReasonCode { get; set; }
        public long CantproReasonCode { get; set; }
        public string ReplyAmplification { get; set; }

        public MilStd47001_Address ReferenceMessageDataAddressSet { get; set; }
        public MilStd47001_Address ReferenceMessageDataAddress { get; set; }

        public bool ReferenceMessageDataDateSet { get; set; }
        public MilStd47001_Date ReferenceMessageDataDate { get; set; }
    }

    
    [Serializable]
    public class MilStd47001Data
    {
        public MilStd47001Data()
        {
            OriginatorAddress = new MilStd47001_Address();
            RecipientAddress = new List<MilStd47001_Address>();
            InformationAddress = new List<MilStd47001_Address>();
            Messages = new List<MilStd47001_Message>();
        }
        public long Version { get; set; }

        public bool IsDataCompressionSet { get; set; }
        public long DataCompression { get; set; }

        public MilStd47001_Address OriginatorAddress { get; set; }

        public List<MilStd47001_Address> RecipientAddress { get; set; }

        public List<MilStd47001_Address> InformationAddress { get; set; }

        public long HeaderSize { get; set; }

        public List<MilStd47001_Message> Messages { get; set; }
    }
}
