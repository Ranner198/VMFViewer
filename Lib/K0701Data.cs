using System;
using System.Collections.Generic;

namespace IMAS.Core.Parser.VMF.Lib
{
    [Serializable]
    public class CasualtyData
    {
        public CasualtyData()
        {
            SpecialMedevacEquipment = new List<long>();
        }

        public long MedevacMissionPriority { get; set; }

        public long NumberLitterPatients { get; set; }

        public long NumberAmbulatoryPatients { get; set; }

        public long MedicRequiredIndicator { get; set; }

        public long CasualtyType { get; set; }
        
        public long BodyPartAffected { get; set; }
        
        public long NbcContaminationType { get; set; }
        
        public List<long> SpecialMedevacEquipment { get; set; }
        
        public long PatientNationality { get; set; }
    }

    
    [Serializable]
    public class PickupLocationData
    {
        
        public long Latitude0051Minute { get; set; }
        
        public double Latitude0051MinuteAsDegree { get; set; }
        
        public long Longitude0051Minute { get; set; }
        
        public double Longitude0051MinuteAsDegree { get; set; }
        
        public long ElevationFeet { get; set; }
    }

    
    [Serializable]
    public class EnemyData
    {
        
        public long DirectionToEnemy { get; set; }
        
        public long HostileFireTypeReceived { get; set; }
    }

    
    [Serializable]
    public class K0701Data
    {
        public K0701Data()
        {
            RequestResponseDate = new MilStd47001_Date();
            CasualtyDataList = new List<CasualtyData>();
            PickupTime = new MilStd47001_Date();
            PickupLocation = new PickupLocationData();
            EnemyDataList = new List<EnemyData>();
        }

        public string RequestorCallSign { get; set; }
        
        /// <summary>
        /// Requesting Unit Identification
        /// </summary>
        public long Urn { get; set; }
        
        public MilStd47001_Date RequestResponseDate { get; set; }

        public long MedevacRequestNumberFpi { get; set; }

        /// <summary>
        /// THE ALPHANUMERIC IDENTIFIER ASSIGNED TO A MEDEVAC REQUEST BY THE CONTROLLING AGENCY.
        /// </summary>
        public string MedevacRequestNumber { get; set; }

        /// <summary>
        /// DESCRIBES THE TYPE OF MEDICAL EVACUATION MISSION.
        /// NOT SPECIFIED 0-0
        /// AIR 1-1
        /// GROUND 2-2
        /// SURFACE 3-3 (WATERCRAFT)
        /// </summary>
        public long MedevacMissionType { get; set; }
        
        public long NumberOfFriendlyKiaFpi { get; set; }
        
        public long NumberOfFriendlyKia { get; set; }
        
        public long NumberOfFriendlyWiaFpi { get; set; }
        
        public long NumberOfFriendlyWia { get; set; }
        
        public List<CasualtyData> CasualtyDataList;
        
        public MilStd47001_Date PickupTime { get; set; }

        public string ZoneName { get; set; }
        
        public PickupLocationData PickupLocation { get; set; }
        public string AgencyContactFrequencyDesignator { get; set; }
        public string ZoneControllerCallSign { get; set; }

        public long ZoneMarking { get; set; }
        
        public long ZoneHot { get; set; }
        
        public List<EnemyData> EnemyDataList { get; set; }

        public long ZoneSecurity { get; set; }

        public long ZoneMarkingColor { get; set; }

        public long TerrainDescription { get; set; }

        public long WeatherConditions { get; set; }
        
        public long CloudBaseAltitudeFeet { get; set; }
        
        public long WindSpeed { get; set; }
        
        public string Comments { get; set; }
    }
}
