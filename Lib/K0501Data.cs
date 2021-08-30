using System;

namespace IMAS.Core.Parser.VMF.Lib
{
    [Serializable]
    public class K0501Data
    {
        public K0501Data()
        {
            ReportTime = new MilStd47001_Date();
        }

        public long Urn { get; set; }
        public long UnitLatitude { get; set; }
        
        public double UnitLatitudeAsDegrees { get; set; }
        
        public long UnitLongitude { get; set; }
        
        public double UnitLongitudeAsDegrees { get; set; }
        
        public long LocationDerivation { get; set; }
        
        public long LocationQuality { get; set; }
        
        public long ExerciseIndicator { get; set; }
        
        public long Course { get; set; }
        
        public long UnitSpeedKph { get; set; }
        
        public long ElevationFeet { get; set; }
        
        public long Altitude25ft { get; set; }
        
        public long IffMode1Code { get; set; }
        
        public long IffMode2Code { get; set; }
        
        public long IffMode3Code { get; set; }
        
        public MilStd47001_Date ReportTime { get; set; }
        
        public long OriginatorEnvironment { get; set; }
        
        public long AirSpecificType { get; set; }
        
        public long LandSpecificType { get; set; }
        
        public long SurfaceSpecificType { get; set; }
        
        public long SubSurfaceSpecificType { get; set; }
    }
}
