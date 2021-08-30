using System.ComponentModel;

namespace IMAS.Core.Parser.VMF.Lib
{
    public enum Enum_47001_CantproReason
    {
        [Description("Field content invalid")]
        FIELD_CONTENT_INVALID = 1,
        
        [Description("Message incorrectly routed")]
        MESSAGE_INCORRECTLY_ROUTED = 2, 
        
        [Description("Address inactive")]
        ADDRESS_INACTIVE = 3, 
        
        [Description("Reference point unknown to receiving agency")]
        REFERENCE_POINT_UNKNOWN_TO_RECEIVING_AGENCY = 4,
        
        [Description("Fire units shall be controlled by receiving agency")]
        FIRE_UNITS_CONTROLLED_BY_RECEIVING_AGENCY = 5,
        
        [Description("Mission shall be controlled by receiving agency")]
        MISSION_CONTROLLED_BY_RECEIVING_AGENCY = 6,
        
        [Description("Mission number unknown by receiving agency")]
        MISSION_NUMBER_UNKNOWN_BY_RECEIVING_AGENCY = 7,
        
        [Description("Target number unknown by receiving agency")]
        TARGET_NUMBER_UNKNOWN_BY_RECEIVING_AGENCY = 8,
            
        [Description("Schedule number unknown by receiving agency")]
        SCHEDULE_NUMBER_UNKNOWN_BY_RECEIVING_AGENCY = 9, 
        
        [Description("Incorrect controlling address for a given track number")]
        INCORRECT_CONTROLLING_ADDRESS_FOR_TRACK_NUMBER = 10,

        [Description("Track number not in own track file")]
        TRACK_NUMBER_NOT_IN_TRACK_FILE = 11, 
        
        [Description("Invalid according to given field")]
        INVALID_ACCORDING_TO_FIELD = 12,

        [Description("Message cannot be converted")]
        MESSAGE_CANNOT_BE_CONVERTED = 13, 

        [Description("Agency file full")]
        AGENCY_FILE_FULL = 14,
        
        [Description("Agency does not recognize this message number")] 
        AGENCY_DOES_NOT_RECOGNIZE_MESSAGE_NUMBER = 15,
        
        [Description("Agency cannot correlate message to current file content")] 
        AGENCY_CANNOT_CORRELATE_MESSAGE_TO_CURRENT_FILE_CONTENT = 16,

        [Description("Agency limit exceeded on repeated fields or groups")]
        AGENCY_LIMIT_EXCEEDED_ON_REPEATED_FIELDS_OR_GROUPS = 17,
        
        [Description("Agency computer system inactive")] 
        AGENCY_COMPUTER_SYSTEM_INACTIVE = 18,
        
        [Description("Addressee unknown")] 
        ADDRESSEE_UNKNOWN = 19,
        
        [Description("Can’t forward (agency failure)")] 
        CANNOT_FORWARD_AGENCY_FAILURE = 20,

        [Description("Can’t forward (link failure)")] 
        CANNOT_FORWARD_LINK_FAILURE = 21,
        
        [Description("Illogical juxtaposition of header fields")] 
        ILLOGICAL_JUXTAPOSITION_HEADER_FIELDS = 22,

        [Description("Cannot uncompress Unix (LZW) compressed data")] 
        CANNOT_UNCOMPRESS_LZW_DATA = 23,
        
        [Description("Cannot uncompress LZ-77 compressed data")] 
        CANNOT_UNCOMPRESS_LZ77_DATA = 24,
        
        [Description("Message too old, based on Perishability")] 
        MESSAGE_TOO_OLD_PERISHABILITY = 25,
        
        [Description("Security level restriction")]
        SECURITY_LEVEL_RESTRICTION = 26,

        [Description("Authentication Failure")]
        AUTHENTICATION_FAILURE = 27,
        
        [Description("Certificate not found")]
        CERTIFICATE_NOT_FOUND = 28,

        [Description("Certificate invalid")]
        CERTIFICATE_INVALID = 29,
        
        [Description("Do not support this SPI value")]
        DO_NOT_SUPPORT_SPI_VALUE = 30,
        
        [Description("Can not generate a signed acknowledgement")]
        CANNOT_GENERATE_SIGNED_ACKNOWLEDGEMENT_ERROR = 31,

        [Description("Response not available for retransmission")]
        RESPONSE_NOT_AVAILABLE_FOR_RETRANSMISSION = 32,
    }
}
