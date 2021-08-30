using System.Collections.Generic;

namespace IMAS.Core.Parser.VMF.Lib
{
    public class K0519_EntityDescription
    {
        public long IdentityVmf { get; set; }
        public long SymbolDimension { get; set; }
        public long EntityType { get; set; }
        public long EntitySubType { get; set; }
        public long EntitySizeMobility { get; set; }
        public long EntityStatus { get; set; }
        public long Nationality { get; set; }
    }

    public class K0519_EntityInformation
    {
        public K0519_EntityInformation()
        {
            EntityDescription = new K0519_EntityDescription();
            AdditionalInformationList = new List<string>();
            UniqueDesignationList = new List<string>();
        }

        public string SymbolCode { get; set; }
        public K0519_EntityDescription EntityDescription { get; set; }
        public List<string> AdditionalInformationList { get; set; }
        public List<string> UniqueDesignationList { get; set; }
        public string StaffComments { get; set; }
    }

    public class K0519_FireMission
    {
        public long FireMissionType { get; set; }

        public string TargetNumber { get; set; }

        public long EngagementType { get; set; }

        public long ObserverUrn { get; set; }

        public long TargetLatitude { get; set; }
        public double TargetLatitudeAsDegree { get; set; }

        public long TargetLongitude { get; set; }
        public double TargetLongitudeAsDegree { get; set; }

        public long DayOnTarget { get; set; }

        public long HourOnTarget { get; set; }

        public long MinuteOnTarget { get; set; }
    }

    public class K0519_ObstacleDtg
    {
        public long TimeFunction { get; set; }

        public long Year { get; set; }

        public long Month { get; set; }

        public long Day { get; set; }

        public long Hour { get; set; }

        public long Minute { get; set; }

        public long Second { get; set; }

    }

    public class K0519_ObstacleLocation
    {
        public long Latitude { get; set; }
        public double LatitudeAsDegree { get; set; }
        public long Longitude { get; set; }
        public double LongitudeAsDegree { get; set; }
    }

    public class K0519_SafeLaneStatus
    {
        public long SafeLaneLatitude { get; set; }
        public double SafeLaneLatitudeAsDegree { get; set; }
        public long SafeLaneLongitude { get; set; }
        public double SafeLaneLongitudeAsDegree { get; set; }
        public long SafeLaneWidth { get; set; }
    }

    public class K0519_ObstacleGroup
    {
        public K0519_ObstacleGroup()
        {
            TimeParameterList = new List<K0519_ObstacleDtg>();
            ObstacleLocationList = new List<K0519_ObstacleLocation>();
            SafeLaneStatusList = new List<K0519_SafeLaneStatus>();
        }

        public long IdentityVmf { get; set; }
        public long SymbolDimension { get; set; }
        public long EntityType { get; set; }
        public long EntitySubType { get; set; }
        public string UniqueSymbolDesignation { get; set; }
        public string AdditionalInformation { get; set; }
        public long EntityAxisOrientation { get; set; }

        public List<K0519_ObstacleDtg> TimeParameterList { get; set; }

        public long ImpactOnMovement { get; set; }
        public long ObstacleHeight { get; set; }
        public long ObstacleDepth { get; set; }
        public long ObstacleLength { get; set; }
        public long ObstacleWidth { get; set; }

        public long EnemyActivity { get; set; }
        public long ZoneMarking { get; set; }
        public long ControllerUrn { get; set; }
        public List<K0519_ObstacleLocation> ObstacleLocationList { get; set; }
        public long ObstacleType { get; set; }
        public long ObstacleStatus { get; set; }
    
        public List<K0519_SafeLaneStatus> SafeLaneStatusList { get; set; }
        public string SafeLaneSymbolCode { get; set; }
    }

    public class K0519_StrikeWarning
    {

    }

    public class K0519_ThreatWarning
    {

    }

    public class K0519_NbcAlert
    {

    }

    public class K0519_Bridge
    {

    }

    public class K0519_SupplyPolong
    {

    }

    public class K0519_ObservedPositions
    {

    }

    public class K0519_EntityCombatStatus
    {

    }

    public class K0519_IEDReport
    {

    }

    public class K0519_BypassGroup
    {

    }

    public class K0519Data
    {
        public long DataOriginatorUrn { get; set; }

        public long DataOriginatorYear { get; set; }
        public long DataOriginatorMonth { get; set; }
        public long DataOriginatorDay { get; set; }
        public long DataOriginatorHour { get; set; }
        public long DataOriginatorMinute { get; set; }
        public long DataOriginatorSecond { get; set; }

        public long GraphicalReferenceEntityType { get; set; }

        public long EntityIdSerialNumber { get; set; }

        public long ActionDesignator { get; set; }

        public long SecurityClassification { get; set; }

        public K0519_EntityInformation EntityInformation { get; set; }

        public K0519_FireMission FireMission { get; set; }

        public K0519_ObstacleGroup ObstacleGroup { get; set; }

        public K0519_StrikeWarning StrikeWarning { get; set; }

        public K0519_ThreatWarning ThreatWarning { get; set; }

        public K0519_NbcAlert NbcAlert { get; set; }

        public K0519_Bridge Bridge { get; set; }

        public K0519_SupplyPolong SupplyPolong { get; set; }

        public K0519_ObservedPositions ObservedPositions { get; set; }

        public K0519_EntityCombatStatus EntityCombatStatus { get; set; }

        public K0519_IEDReport IEDReport { get; set; }

        public K0519_BypassGroup BypassGroup { get; set; }
    }
}
