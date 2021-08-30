using System;
using System.Collections.Generic;

namespace IMAS.Core.Parser.VMF.Lib
{
    [Serializable]
    public class MessageGroupData
    {
        public MessageGroupData()
        {
        }

        public long PayGrade { get; set; }
        public string OccupationalSpecialty { get; set; }
        public long CasualtyType { get; set; }
        public long BodyPartAffected { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public long SocialSecurityNumber { get; set; }
        public long ArmedServiceDesignator { get; set; }
        public long ActualOrExpectedDisposition { get; set; }
        public long BurialSiteLatitude { get; set; }
        public double BurialSiteLatitudeAsDegree { get; set; }
        public long BurialSiteLongitude { get; set; }
        public double BurialSiteLongitudeAsDegree { get; set; }
        public string Comment { get; set; }
    }
    
    [Serializable]
    public class K0702Data
    {
        public K0702Data()
        {
            MessageGroupList = new List<MessageGroupData>();
        }
        public long NumberOfCasualties { get; set; }

        public List<MessageGroupData> MessageGroupList { get; set; } 
    }
}
