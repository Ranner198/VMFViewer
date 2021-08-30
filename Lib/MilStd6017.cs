
using System;
using System.Collections.Generic;

namespace IMAS.Core.Parser.VMF.Lib
{
    public class MilStd6017
    {
        #region Private Variables
        private MilStd47001 m_header;
        private int currentBit = 0;
        #endregion

        #region Constructors
        public MilStd6017(byte[] inputBuffer)
        {
            m_header = new MilStd47001(inputBuffer);
            currentBit = m_header.Parse();
        }
        #endregion

        #region Utility Methods
        public long readDataInt(string name, int size)
        {
            int currentBitStart = currentBit;
            int currentBitStop = currentBit + size - 1;

            long result = m_header.BitReader.GetDataAsLong(currentBitStart, currentBitStop);

            //Debug.Prlong(name + " => " + result.ToString());

            // Start the next field on the following bit
            currentBit = currentBitStop + 1;

            return result;
        }

        public byte[] readDataArray(string name, int size)
        {
            int currentBitStart = currentBit;
            int currentBitStop = currentBit + size - 1;

            byte[] result = m_header.BitReader.GetDataAsByteArray(currentBitStart, currentBitStop);

            // Start the next field on the following bit
            currentBit = currentBitStop + 1;

            return result;
        }

        public string readDataString(string name, long size)
        {
            String vmfString = "";

            for (int i = 0; i < size; i += 7)
            {
                long vmfCharField = readDataInt(name, 7);

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

            return vmfString;
        }

        public MilStd47001Data GetHeaderData()
        {
            return m_header.Data;
        }
        #endregion

        #region MIL-STD-6017 Parsing Methods
        public List<object> Parse()
        {
            List<object> result = new List<object>();

            if (m_header.Data.Messages != null)
            {
                foreach (MilStd47001_Message message in m_header.Data.Messages)
                {
                    if (message.FunctionAreaDesignator == 5) {
                        
                        if (message.MessageNumber == 1) {
                            List<K0501Data> data = ParseK0501(message.MessageStandardVersion);

                            if (data != null)
                            {
                                result.AddRange(data);
                            }
                        }
                        
                        else if (message.MessageNumber == 19)
                        {
                            List<K0519Data> data = ParseK0519(message.MessageStandardVersion);

                            if (data != null)
                            {
                                result.Add(data);
                            }
                        }
                    }

                    else if (message.FunctionAreaDesignator == 7)
                    {
                        if (message.MessageNumber == 1)
                        {
                            K0701Data data = ParseK0701(message.MessageStandardVersion);

                            if (data != null)
                            {
                                result.Add(data);
                            }
                        }
                        else if (message.MessageNumber == 2)
                        {
                            K0702Data data = ParseK0702(message.MessageStandardVersion);

                            if (data != null)
                            {
                                result.Add(data);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public List<K0501Data> ParseK0501(long messageStandardVersion)
        {
            switch (messageStandardVersion)
            {
                case 5: // 6017 enumeration for MIL-STD-47001-D
                case 6: // 6017 enumeration for MIL-STD-47001-C
                case 7:
                    return ParseK0501_CS11_12();
            }

            return null;
        }

        public List<K0501Data> ParseK0501_CS11_12()
        { 
            List<K0501Data> listOfPositionReports = new List<K0501Data>();
            bool positionReportGriRepeat = false;

            do
            {
                K0501Data data = new K0501Data();

                long positionReportGri = readDataInt("POSITION REPORT GRI", 1);
                positionReportGriRepeat = (positionReportGri == 1);

                data.Urn = readDataInt("POSITION REPORT URN", 24);

                data.UnitLatitude = readDataInt("UNIT LATITUDE", 25);
                data.UnitLatitudeAsDegrees = DFIDUI.Convert_281_402(data.UnitLatitude);

                data.UnitLongitude = readDataInt("UNIT LONGITUDE", 26);
                data.UnitLongitudeAsDegrees = DFIDUI.Convert_282_402(data.UnitLongitude);

                data.LocationDerivation = readDataInt("LOCATION DERIVATION", 4);

                long locationQualityFpi = readDataInt("LOCATION QUALITY FPI", 1);

                if (locationQualityFpi == 1)
                {
                    data.LocationQuality = readDataInt("LOCATION QUALITY", 4);
                }

                long exerciseIndicator = readDataInt("EXERCISE INDICATOR", 1);
                data.ExerciseIndicator = exerciseIndicator;

                long courseSpeedDataGpi = readDataInt("COURSE SPEED DATA GPI", 1);

                if (courseSpeedDataGpi == 1)
                {
                    data.Course = readDataInt("COURSE", 9);

                    data.UnitSpeedKph = readDataInt("SPEED", 11);
                } // courseSpeedDataGpi - G1

                long elevationFeetFpi = readDataInt("ELEVATION, FEET FPI", 1);

                if (elevationFeetFpi == 1)
                {
                    data.ElevationFeet = readDataInt("ELEVATION, FEET", 17);
                }

                long altitude25FtFpi = readDataInt("ALTITUDE, 25 FT FPI", 1);

                if (altitude25FtFpi == 1)
                {
                    data.Altitude25ft = readDataInt("ALTITUDE, 25 FT", 13);
                }

                long iffModesGpi = readDataInt("IFF MODES GPI", 1);

                if (iffModesGpi == 1)
                {
                    long iffMode1CodeFpi = readDataInt("IFF MODE I Code FPI", 1);
                    if (iffMode1CodeFpi == 1)
                    {
                        data.IffMode1Code = readDataInt("IFF MODE I Code", 5);
                    }

                    long iffMode2CodeFpi = readDataInt("IFF MODE II Code FPI", 1);
                    if (iffMode2CodeFpi == 1)
                    {
                        data.IffMode2Code = readDataInt("IFF MODE II Code", 12);
                    }

                    long iffMode3CodeFpi = readDataInt("IFF MODE III Code FPI", 1);
                    if (iffMode3CodeFpi == 1)
                    {
                        data.IffMode3Code = readDataInt("IFF MODE III Code", 12);
                    }
                } // iffModesGpi - G2

                long reportTimeGpi = readDataInt("REPORT TIME GPI", 1);

                if (reportTimeGpi == 1)
                {
                    //data.ReportTime.Year = readDataInt("REPORT TIME YEAR", 7);
                    //data.ReportTime.Month = readDataInt("REPORT TIME MONTH", 4);
                    data.ReportTime.Day = readDataInt("REPORT TIME DAY OF MONTH", 5);
                    data.ReportTime.Hour = readDataInt("REPORT TIME HOUR", 5);
                    data.ReportTime.Minute = readDataInt("REPORT TIME MINUTE", 6);
                    data.ReportTime.Second = readDataInt("REPORT TIME SECOND", 6);
                } // reportTimeGpi - G3

                data.OriginatorEnvironment = readDataInt("ORIGINATOR ENVIRONMENT", 2);

                long specificTypeGpi = readDataInt("SPECIFIC TYPE GPI", 1);

                if (specificTypeGpi == 1)
                {
                    long airSpecificTypeFpi = readDataInt("AIR SPECIFIC TYPE FPI", 1);
                    if (airSpecificTypeFpi == 1)
                    {
                        data.AirSpecificType = readDataInt("AIR SPECIFIC TYPE", 12);
                    }

                    long surfaceSpecificTypeFpi = readDataInt("SURFACE SPECIFIC TYPE FPI", 1);
                    if (surfaceSpecificTypeFpi == 1)
                    {
                        data.SurfaceSpecificType = readDataInt("SURFACE SPECIFIC TYPE", 12);
                    }

                    long subSurfaceSpecificTypeFpi = readDataInt("SUBSURFACE SPECIFIC TYPE FPI", 1);
                    if (subSurfaceSpecificTypeFpi == 1)
                    {
                        data.SubSurfaceSpecificType = readDataInt("SUBSURFACE SPECIFIC TYPE", 12);
                    }

                    long landSpecificTypeFpi = readDataInt("LAND SPECIFIC TYPE FPI", 1);
                    if (landSpecificTypeFpi == 1)
                    {
                        data.LandSpecificType = readDataInt("LAND SPECIFIC TYPE", 12);
                    }
                } // specificTypeGpi - G4

                listOfPositionReports.Add(data);
            } while (positionReportGriRepeat);

            return listOfPositionReports;
        }

        public List<K0519Data> ParseK0519(long messageStandardVersion)
        {
            switch (messageStandardVersion)
            {
                case 5: // 6017 enumeration for MIL-STD-47001-D
                case 6: // 6017 enumeration for MIL-STD-47001-C
                    return ParseK0519_CS11_12();
                case 7: // 6017 enumeration for the 6017A+ Baseline
                    return ParseK0519_APLUS();
            }

            return null;
        }

        public List<K0519Data> ParseK0519_CS11_12()
        {
            List<K0519Data> data = new List<K0519Data>();

            bool entityDataGriRepeat = false;

            do
            {
                long entityDataGri = readDataInt("ENTIY DATA GRI", 1);
                entityDataGriRepeat = (entityDataGri == 1);

                long originatorUrn = readDataInt("ORIGINATOR URN", 24);

                long originatorDay = readDataInt("ORIGINATOR DAY OF MONTH", 5);

                long originatorHour = readDataInt("ORIGINATOR HOUR", 5);

                long originatorMinute = readDataInt("ORIGINATOR MINUTE", 6);

                long originatorSecond = readDataInt("ORIGINATOR SECOND", 6);

                long graphicalReferenceEntityType = readDataInt("GRAPHICAL REFERENCE ENTITY TYPE", 4);

                long entityIdSerialNumberFpi = readDataInt("ENTITY ID SERIAL NUMBER FPI", 1);

                if (entityIdSerialNumberFpi == 1)
                {
                    long entityIdSerialNumber = readDataInt("ENTITY ID SERIAL NUMBER", 32);
                }

                long actionDesignator = readDataInt("ACTION DESIGNATOR", 2);

                long fireMissionInitiationDeletionFpi = readDataInt("FIRE MISSION INITIATION/DELETION GPI", 1);

                if (fireMissionInitiationDeletionFpi == 1)
                {
                    long fireMissionTypeFpi = readDataInt("FIRE MISSION TYPE FPI", 1);

                    if (fireMissionTypeFpi == 1)
                    {
                        long fireMissionType = readDataInt("FIRE MISSION TYPE", 4);
                    }

                    long targetNumberFpi = readDataInt("TARGET NUMBER FPI", 1);

                    if (targetNumberFpi == 1)
                    {
                        string targetNumber = readDataString("TARGET NUMBER", 28);
                    }

                    long missionEngagementGpi = readDataInt("MISSION ENGAGEMENT GPI", 1);

                    if (missionEngagementGpi == 1)
                    {
                        long engagementType = readDataInt("ENGAGEMENT TYPE", 2);

                        long observerUrnFpi = readDataInt("OBSERVER URN FPI", 1);

                        if (observerUrnFpi == 1)
                        {
                            long observerUrn = readDataInt("OBSERVER URN", 24);
                        }

                        long targetLocationGpi = readDataInt("TARGET LOCATION GPI", 1);

                        if (targetLocationGpi == 1)
                        {
                            long targetLatitude = readDataInt("TARGET LATITUDE", 25);

                            long targetLongitude = readDataInt("TARGET LONGITUDE", 26);
                        } // TARGET LOCATION GPI - G3

                        long timeOnTargetDtgGpi = readDataInt("TIME ON TARGET DTG GPI", 1);

                        if (timeOnTargetDtgGpi == 1)
                        {
                            long dayOnTarget = readDataInt("DAY ON TARGET", 5);

                            long hourOnTarget = readDataInt("HOUR ON TARGET", 5);

                            long minuteOnTarget = readDataInt("MINUTE ON TARGET", 6);
                        } // TIME ON TARGET DTG GPI - G4
                    } // MISSION ENGAGEMENT GPI - G2
                } // FIRE MISSION INITIATION/DELETION GPI - G1

                long obstacleGroupGpi = readDataInt("OBSTACLE GROUP GPI", 1);

                if (obstacleGroupGpi == 1)
                {
                    long identityVmf = readDataInt("IDENTITY, VMF", 4);

                    long symbolDimension = readDataInt("SYMBOL DIMENSION", 5);

                    long entityType = readDataInt("ENTITY TYPE", 6);

                    long entitySubTypeFpi = readDataInt("ENTITY SUBTYPE FPI", 1);

                    if (entitySubTypeFpi == 1)
                    {
                        long entitySubType = readDataInt("ENTITY SUBTYPE", 6);
                    }

                    long iconStatus = readDataInt("ICON STATUS", 1);

                    long uniqueSymbolDesignationFpi = readDataInt("UNIQUE SYMBOL DESIGNATION FPI", 1);

                    if (uniqueSymbolDesignationFpi == 1)
                    {
                        string uniqueSymbolDesignator = readDataString("UNIQUE SYMBOL DESIGNATION", 245);
                    }

                    long additionalInformationFpi = readDataInt("ADDITIONAL INFORMATION FPI", 1);

                    if (additionalInformationFpi == 1)
                    {
                        string additionalInformation = readDataString("ADDITIONAL INFORMATION", 140);
                    }

                    long symbolAxisOrientationFpi = readDataInt("SYMBOL AXIS ORIENTATION FPI", 1);

                    if (symbolAxisOrientationFpi == 1)
                    {
                        long symbolAxisOrientation = readDataInt("SYMBOL AXIS ORIENTATION", 9);
                    }

                    bool obstacleDateTimeParametersGriRepeat = false;

                    do
                    {
                        long obstacleDateTimeParametersGri = readDataInt("OBSTACLE DATE/TIME PARAMETERS GRI", 1);
                        obstacleDateTimeParametersGriRepeat = (obstacleDateTimeParametersGri == 1);

                        long obstacleTimeFunction = readDataInt("OBSTACLE TIME FUNCTION", 3);

                        long obstacleTimeDay = readDataInt("OBSTACLE TIME DAY OF MONTH", 5);

                        long obstacleTimeHour = readDataInt("OBSTACLE TIME HOUR", 5);

                        long obstacleTimeMinute = readDataInt("OBSTACLE TIME MINUTE", 6);

                        long obstacleTimeSecondFpi = readDataInt("OBSTACLE TIME SECOND FPI", 1);

                        if (obstacleTimeSecondFpi == 1)
                        {
                            long obstacleTimeSecond = readDataInt("OBSTACLE TIME SECOND", 6);
                        }
                    } while (obstacleDateTimeParametersGriRepeat);

                    long impactOnMovementFpi = readDataInt("IMPACT ON MOVEMENT FPI", 1);

                    if (impactOnMovementFpi == 1)
                    {
                        long impactOnMovement = readDataInt("IMPACT ON MOVEMENT", 3);
                    }

                    long obstacleHeightFpi = readDataInt("OBSTACLE HEIGHT FPI", 1);

                    if (obstacleHeightFpi == 1)
                    {
                        long obstacleHeight = readDataInt("OBSTACLE HEIGHT", 10);
                    }

                    long obstacleDepthFpi = readDataInt("OBSTACLE DEPTH FPI", 1);

                    if (obstacleDepthFpi == 1)
                    {
                        long obstacleDepth = readDataInt("OBSTACLE DEPTH", 5);
                    }

                    long obstacleLengthFpi = readDataInt("OBSTACLE LENGTH FPI", 1);

                    if (obstacleLengthFpi == 1)
                    {
                        long obstacleLength = readDataInt("OBSTACLE LENGTH", 10);
                    }

                    long obstacleWidthFpi = readDataInt("OBSTACLE WIDTH FPI", 1);

                    if (obstacleWidthFpi == 1)
                    {
                        long obstacleWidth = readDataInt("OBSTACLE WIDTH", 10);
                    }

                    bool obstacleLocationGriRepeat = false;

                    do
                    {
                        long obstacleLocationGri = readDataInt("OBSTACLE LOCATION GRI", 1);
                        obstacleLocationGriRepeat = (obstacleLocationGri == 1);

                        long obstacleLatitude = readDataInt("OBSTACLE LATITUDE", 25);
                        long obstacleLongitude = readDataInt("OBSTACLE LONGITUDE", 26);
                    } while (obstacleLocationGriRepeat);

                    long enemyActivityFpi = readDataInt("ENEMY ACTIVITY FPI", 1);

                    if (enemyActivityFpi == 1)
                    {
                        long enemyActivity = readDataInt("ENEMY ACTIVITY", 6);
                    }

                    long zoneMarkingFpi = readDataInt("ZONE MARKING FPI", 1);

                    if (zoneMarkingFpi == 1)
                    {
                        long zoneMarking = readDataInt("ZONE MARKING", 4);
                    }

                    long controllerUrnFpi = readDataInt("CONTROLLER URN FPI", 1);

                    if (controllerUrnFpi == 1)
                    {
                        long controllerUrn = readDataInt("CONTROLLER URN", 24);
                    }
                } // OBSTACLE GROUP GPI - G5

                long strikeWarningGpi = readDataInt("STRIKE WARNING GPI", 1);

                if (strikeWarningGpi == 1)
                {
                    long conventionalNuclearIndicator = readDataInt("CONVENTIONAL/NUCLEAR INDICATOR", 1);

                    long strikeDay = readDataInt("STRIKE DAY", 5);

                    long strikeHour = readDataInt("STRIKE HOUR", 5);

                    long strikeMinute = readDataInt("STRIKE MINUTE", 6);

                    long troopSafetyGpi = readDataInt("TROOP SAFETY GPI", 1);

                    if (troopSafetyGpi == 1)
                    {
                        long minimumSafeDistance1RadiusFpi = readDataInt("MINIMUM SAFE DISTANCE 1 RADIUS FPI", 1);

                        if (minimumSafeDistance1RadiusFpi == 1)
                        {
                            long minimumSafeDistance1Radius = readDataInt("MINIMUM SAFE DISTANCE 1 RADIUS", 10);
                        }

                        long groundZeroLocationGpi = readDataInt("GROUND ZERO LOCATION GPI", 1);

                        if (groundZeroLocationGpi == 1)
                        {
                            long groundZeroLocationLatitude = readDataInt("GROUND ZERO LOCATION LATITUDE", 25);
                            long groundZeroLocationLongitude = readDataInt("GROUND ZERO LOCATION LONGITUDE", 26);
                        } // GROUND ZERO LOCATION GPI - G8
                    } // TROOP SAFETY GPI - G7
                } // STRIKE WARNING GPI - G6

                long threatWarningGpi = readDataInt("THREAT WARNING GPI", 1);

                if (threatWarningGpi == 1)
                {
                    long threatType = readDataInt("THREAT TYPE", 4);

                    long threatPostureVmf = readDataInt("THREAT POSTURE, VMF", 3);

                    long threatImpactDtgGpi = readDataInt("THREAT IMPACT DTG GPI", 1);

                    if (threatImpactDtgGpi == 1)
                    {
                        long threatImpactHour = readDataInt("THREAT IMPACT HOUR", 5);

                        long threatImpactMinute = readDataInt("THREAT IMPACT MINUTE", 6);

                        long threatImpactSecondFpi = readDataInt("THREAT IMPACT SECOND FPI", 1);

                        if (threatImpactSecondFpi == 1)
                        {
                            long threatImpactSecond = readDataInt("THREAT IMPACT SECOND", 6);
                        }
                    } // THREAT IMPACT DTG GPI - G10

                    long threatLocationGpi = readDataInt("THEAT LOCATION GPI", 1);

                    if (threatLocationGpi == 1)
                    {
                        long threatLocationLatitude0051Minute = readDataInt("THREAT LOCATION LATITUDE 0.0051 MINUTE", 21);

                        long threatLocationLongitude0051Minute = readDataInt("THREAT LOCATION LONGITUDE 0.0051 MINUTE", 22);

                        long threatLocationAltitude25FtFpi = readDataInt("THREAT LOCATION ALTITUDE 25 FT FPI", 1);

                        if (threatLocationAltitude25FtFpi == 1)
                        {
                            long threatLocationAltitude25Ft = readDataInt("THREAT LOCATION ALTITUDE 25 FT", 13);
                        }

                        long strengthFpi = readDataInt("STRENGTH FPI", 1);

                        if (strengthFpi == 1)
                        {
                            long strength = readDataInt("STRENGTH", 4);
                        }

                        long courseFpi = readDataInt("COURSE FPI", 1);

                        if (courseFpi == 1)
                        {
                            long course = readDataInt("COURSE", 9);
                        }

                        long speedFpi = readDataInt("SPEED FPI", 1);

                        if (speedFpi == 1)
                        {
                            long speed = readDataInt("SPEED", 11);
                        }
                    } // THREAT LOCATION GPI - G11

                    long threatImpactLocationGpi = readDataInt("THREAT IMPACT LOCATION GPI", 1);

                    if (threatImpactLocationGpi == 1)
                    {
                        long threatImpactLocationLatitude0103Minute = readDataInt("THREAT IMPACT LOCATION LATITUDE 0.0103 MINUTE", 20);

                        long threatImpactLocationLongitude0103Minute = readDataInt("THREAT IMPACT LOCATION LONGITUDE 0.0103 MINUTE", 21);

                        long ballisticMissileImpactGpi = readDataInt("BALLISTIC MISSILE IMPACT GPI", 1);

                        if (ballisticMissileImpactGpi == 1)
                        {
                            long squareCircleSwitch = readDataInt("SQUARE/CIRCLE SWITCH", 2);

                            long axisOrientation = readDataInt("AXIS ORIENTATION", 8);

                            long areaMajorAxis4 = readDataInt("AREA MAJOR AXIS, 4", 7);

                            long areaMinorAxis4 = readDataInt("AREA MINOR AXIS, 4", 7);
                        } // BALLISTIC MISSILE IMPACT GPI = G13
                    } // THREAT IMPACT LOCATION GPI - G12
                } // THREAT WARNING GPI - G9

                long nbcAlertGpi = readDataInt("NBC ALERT GPI", 1);

                if (nbcAlertGpi == 1)
                {
                    long nbcReportTypeFpi = readDataInt("NBC REPORT TYPE FPI", 1);

                    if (nbcReportTypeFpi == 1)
                    {
                        long nbcReportType = readDataInt("NBC REPORT TYPE", 3);
                    }

                    long nbcCharacteristicsGpi = readDataInt("NBC CHARACTERISTICS GPI", 1);

                    if (nbcCharacteristicsGpi == 1)
                    {
                        long nbcIncidentType = readDataInt("NBC INCIDENT TYPE", 3);

                        long typeOfNuclearBurstFpi = readDataInt("TYPE OF NUCLEAR BURST FPI", 1);

                        if (typeOfNuclearBurstFpi == 1)
                        {
                            long typeOfNuclearBurst = readDataInt("TYPE OF NUCLEAR BURST", 2);
                        }

                        bool nbcAlertDtgGriRepeat = false;

                        do
                        {
                            long nbcAlertDtgGri = readDataInt("NBC ALERT DTG GRI", 1);
                            nbcAlertDtgGriRepeat = (nbcAlertDtgGri == 1);

                            long timeFunctionNbc = readDataInt("TIME FUNCTION, NBC", 4);

                            long nbcAlertDtgDay = readDataInt("NBC ALERT DTG DAY OF MONTH", 5);

                            long nbcAlertDtgHour = readDataInt("NBC ALERT DTG HOUR", 5);

                            long nbcAlertDtgMinute = readDataInt("NBC ALERT DTG MINUTE", 6);
                        } while (nbcAlertDtgGriRepeat);

                        long nbcAlertAttackLocationGpi = readDataInt("NBC ALERT ATTACK LOCATION GPI", 1);

                        if (nbcAlertAttackLocationGpi == 1)
                        {
                            long locationQualifier = readDataInt("LOCATION QUALIFIER", 1);

                            long attackLocationLatitude = readDataInt("ATTACK LOCATION LATITUDE", 25);

                            long attackLocationLongitude = readDataInt("ATTACK LOCATION LONGITUDE", 26);
                        } // NBC ALERT ATTACK LOCATION GPI - G16

                        long locationCharacteristicsGpi = readDataInt("LOCATION CHARACTERISTICS GPI", 1);

                        if (locationCharacteristicsGpi == 1)
                        {
                            long terrainDescriptionFpi = readDataInt("TERRAIN DESCRIPTION FPI", 1);

                            if (terrainDescriptionFpi == 1)
                            {
                                long terrainDescription = readDataInt("TERRAIN DESCRIPTION", 4);
                            }

                            long vegetationTypeFpi = readDataInt("VEGETATION TYPE FPI", 1);

                            if (vegetationTypeFpi == 1)
                            {
                                long vegetationType = readDataInt("VEGETATION TYPE", 3);
                            }

                            bool releaseDataGriRepeat = false;

                            do
                            {
                                long releaseDataGri = readDataInt("RELEASE DATA GRI", 1);
                                releaseDataGriRepeat = (releaseDataGri == 1);

                                long agentTypeFpi = readDataInt("AGENT TYPE FPI", 1);

                                if (agentTypeFpi == 1)
                                {
                                    long agentType = readDataInt("AGENT TYPE", 5);
                                }

                                long agentNameFpi = readDataInt("AGENT NAME FPI", 1);

                                if (agentNameFpi == 1)
                                {
                                    long agentName = readDataInt("AGENT NAME", 6);
                                }

                                long agentPersistencyFpi = readDataInt("AGENT PERSISTENCY FPI", 1);

                                if (agentPersistencyFpi == 1)
                                {
                                    long agentPersistency = readDataInt("AGENT PERSISTENCY", 2);
                                }

                                bool typeOfDetectionFriRepeat = false;

                                do
                                {
                                    long typeOfDetectionFri = readDataInt("TYPE OF DETECTION FRI", 1);
                                    typeOfDetectionFriRepeat = (typeOfDetectionFri == 1);

                                    long typeOfDetection = readDataInt("TYPE OF DETECTION", 4);
                                } while (typeOfDetectionFriRepeat);
                            } while (releaseDataGriRepeat);
                        } // LOCATION CHARACTERISTICS GPI - G17
                    } // NBC CHARACTERISTICS GPI - G15
                } // NBC ALERT GPI - G14

                long bridgeGpi = readDataInt("BRIDGE GPI", 1);

                if (bridgeGpi == 1)
                {
                    long bridgeType = readDataInt("BRIDGE TYPE", 3);

                    bool bridgeOrientationGriRepeat = false;

                    do
                    {
                        long bridgeOrientationGri = readDataInt("BRIDGE ORIENTATION GRI", 1);
                        bridgeOrientationGriRepeat = (bridgeOrientationGri == 1);

                        long bridgeOrientationLatitude0013Minute = readDataInt("BRIDGE ORIENTATION LATITUDE 0.0013 MINUTE", 23);
                        
                        long bridgeOrientationLongitude0013Minute = readDataInt("BRIDGE ORIENTATION LONGITUDE 0.0013 MINUTE", 23);

                    } while (bridgeOrientationGriRepeat);

                    long controllingForce = readDataInt("CONTROLLING FORCE", 2);
                } // BRIDGE GPI - G18

                long supplyPolongGpi = readDataInt("SUPPLY POlong GPI", 1);

                if (supplyPolongGpi == 1)
                {
                    long supplyPolongReferenceNumber = readDataInt("SUPPLY POlong REFERENCE NUMBER", 24);

                    long supplyPolongType = readDataInt("SUPPLY POlong TYPE", 5);

                    long prepositionedLocationGpi = readDataInt("PREPOSITIONED LOCATION GPI", 1);

                    if (prepositionedLocationGpi == 1)
                    {
                        long prepositionedLatitude = readDataInt("PREPOSITIONED LATITUDE", 25);

                        long prepositionedLongitude = readDataInt("PREPOSITIONED LONGITUDE", 26);
                    } // PREPOSITIONED LOCATION GPI - G20

                    long supplyPolongOperationTimelongervalGpi = readDataInt("SUPPLY POlong OPERATION TIME longERVAL GPI", 1);

                    if (supplyPolongOperationTimelongervalGpi == 1)
                    {
                        bool supplyPolongOperationTimelongervalGriRepeat = false;

                        do
                        {
                            long supplyPolongOperationTimelongervalGri = readDataInt("SUPPLY POlong OPERATION TIME longERVAL GRI", 1);
                            supplyPolongOperationTimelongervalGriRepeat = (supplyPolongOperationTimelongervalGri == 1);

                            long operationTimelongervalMonth = readDataInt("OPERATION TIME longERVAL MONTH", 4);

                            long operationTimelongervalDay = readDataInt("OPERATION TIME longERVAL DAY OF MONTH", 5);

                            long operationTimelongervalHour = readDataInt("OPERATION TIME longERVAL HOUR", 5);

                            long operationTimelongervalMinute = readDataInt("OPERATION TIME longERVAL MINUTE", 6);

                            long operationTimelongervalSecond = readDataInt("OPERATION TIME longERVAL SECOND", 6);
                        } while (supplyPolongOperationTimelongervalGriRepeat);
                    } // SUPPLY POlong OPERATION TIME longERVAL GPI - G21
                } // SUPPLY POlong GPI - G19

                long observedPositionsGpi = readDataInt("OBSERVED POSITIONS GPI", 1);

                if (observedPositionsGpi == 1)
                {
                    long locationDerivation = readDataInt("LOCATION DERIVATION", 4);

                    long eplrsRsIdFpi = readDataInt("EPLRS RS ID FPI", 1);

                    if (eplrsRsIdFpi == 1)
                    {
                        long eplrsRsId = readDataInt("EPLRS RS ID", 14);
                    }

                    long observedLatitude0013Minute = readDataInt("OBSERVED LATITUDE 0.0013 MINUTE", 23);

                    long observedLongitude0013Minute = readDataInt("OBSERVED LONGITUDE 0.0013 MINUTE", 24);

                    long locationQualityFpi = readDataInt("LOCATION QUALITY FPI", 1);

                    if (locationQualityFpi == 1)
                    {
                        long locationQuality = readDataInt("LOCATION QUALITY", 4);
                    }

                    long observedCourseAndSpeedGpi = readDataInt("OBSERVED COURSE AND SPEED GPI", 1);

                    if (observedCourseAndSpeedGpi == 1)
                    {
                        long course = readDataInt("COURSE", 9);

                        long unitSpeedKph = readDataInt("UNIT SPEED, KPH", 11);
                    } // OBSERVED COURSE AND SPEED GPI - G23

                    long elevationFeetFpi = readDataInt("ELEVATION, FEET FPI", 1);

                    if (elevationFeetFpi == 1)
                    {
                        long elevationFeet = readDataInt("ELEVATION, FEET", 17);
                    }

                    long altitude25FtFpi = readDataInt("ALTITUDE 25 FT FPI", 1);

                    if (altitude25FtFpi == 1)
                    {
                        long altitude25Ft = readDataInt("ALTITUDE 25 FT", 13);
                    }

                    long quantityOfEquipmentWeaponsObservedFpi = readDataInt("QUANTITY OF EQUIPMENT/WEAPONS OBSERVED FPI", 1);

                    if (quantityOfEquipmentWeaponsObservedFpi == 1)
                    {
                        long quantityOfEquipmentWeaponsObserved = readDataInt("QUANTITY OF EQUIPMENT/WEAPONS OBSERVED", 14);
                    }

                    long staffCommentsFpi = readDataInt("STAFF COMMENTS FPI", 1);

                    if (staffCommentsFpi == 1)
                    {
                        string staffComments = readDataString("STAFF COMMENTS", 140);
                    }

                    long actionTakenFpi = readDataInt("ACTION TAKEN FPI", 1);

                    if (actionTakenFpi == 1)
                    {
                        long actionTaken = readDataInt("ACTION TAKEN", 6);
                    }
                } // OBSERVED POSITIONS GPI - G22

                long safeLaneGroupGpi = readDataInt("SAFE LANE GROUP GPI", 1);

                if (safeLaneGroupGpi == 1)
                {
                    bool safeLaneLocationGriRepeat = false;

                    do
                    {
                        long safeLaneLocationGri = readDataInt("SAFE LANE LOCATION GRI", 1);
                        safeLaneLocationGriRepeat = (safeLaneLocationGri == 1);

                        long safeLaneLatitude = readDataInt("SAFE LANE LATITUDE", 31);

                        long safeLaneLongitude = readDataInt("SAFE LANE LONGITUDE", 32);

                        long safeLaneWidth = readDataInt("SAFE LANE WIDTH", 7);
                    } while (safeLaneLocationGriRepeat);
                } // SAFE LANE GROUP GPI - G24

                long bypassGroupGpi = readDataInt("BYPASS GROUP GPI", 1);

                if (bypassGroupGpi == 1)
                {
                    bool bypassGroupLocationGriRepeat = false;

                    do
                    {
                        long bypassGroupLocationGri = readDataInt("BYPASS GROUP LOCATION GRI", 1);
                        bypassGroupLocationGriRepeat = (bypassGroupLocationGri == 1);

                        long bypassLatitude = readDataInt("BYPASS LATITUDE", 25);

                        long bypassLongitude = readDataInt("BYPASS LONGITUDE", 26);
                    } while (bypassGroupLocationGriRepeat);

                    long bypassPotentialFpi = readDataInt("BYPASS POTENTIAL FPI", 1);

                    if (bypassPotentialFpi == 1)
                    {
                        long bypassPotential = readDataInt("BYPASS POTENTIAL", 2);
                    }
                } // BYPASS GROUP GPI - G25

                long symbologyInformationGpi = readDataInt("SYMBOLOGY INFORMATION GPI", 1);

                if (symbologyInformationGpi == 1)
                {
                    long identityVmf = readDataInt("IDENTITY, VMF", 4);

                    long symbolDimension = readDataInt("SYMBOL DIMENSION", 5);

                    long entityType = readDataInt("ENTITY TYPE", 6);

                    long entitySubTypeFpi = readDataInt("ENTITY SUBTYPE FPI", 1);

                    if (entitySubTypeFpi == 1)
                    {
                        long entitySubType = readDataInt("ENTITY SUBTYPE", 6);
                    }

                    long additionalInformationFpi = readDataInt("ADDITIONAL INFORMATION FPI", 1);

                    if (additionalInformationFpi == 1)
                    {
                        string additionalInformation = readDataString("ADDITIONAL INFORMATION", 140);
                    }

                    long iconSizeMobilityFpi = readDataInt("ICON SIZE/MOBILITY FPI", 1);

                    if (iconSizeMobilityFpi == 1)
                    {
                        long iconSizeMobility = readDataInt("ICON SIZE/MOBILITY", 8);
                    }
                } // SYMBOLOGY INFORMATION GPI - G26

                long entityCombatStatusGpi = readDataInt("ENTITY COMBAT STATUS GPI", 1);

                if (entityCombatStatusGpi == 1)
                {
                    long entityBeingReportUrn = readDataInt("ENTITY BEING REPORTED URN", 24);

                    long entityCombatStatus = readDataInt("ENTITY COMBAT STATUS", 2);
                } // ENTITY COMBAT STATUS GPI - G27
            } while (entityDataGriRepeat);

            return data;
        }
        public List<K0519Data> ParseK0519_APLUS()
        {
            List<K0519Data> dataList = new List<K0519Data>();

            long grapicalReferenceEntityType = readDataInt("GRAPHICAL REFERENCE ENTITY TYPE", 4);

            bool entityGriRepeat = false;

            do
            {
                K0519Data data = new K0519Data();

                long entityGri = readDataInt("R1 GRI", 1);
                entityGriRepeat = (entityGri == 1);

                long dataOriginatorUrn = readDataInt("DATA ORIGINATOR URN", 24);
                data.DataOriginatorUrn = dataOriginatorUrn;

                long dataOriginatorYear = readDataInt("DATA ORIGINATOR YEAR", 7);
                data.DataOriginatorYear = dataOriginatorYear;

                long dataOriginatorMonth = readDataInt("DATA ORIGINATOR MONTH", 4);
                data.DataOriginatorMonth = dataOriginatorMonth;

                long dataOriginatorDay = readDataInt("DATA ORIGINATOR DAY OF MONTH", 5);
                data.DataOriginatorDay = dataOriginatorDay;

                long dataOriginatorHour = readDataInt("DATA ORIGINATOR HOUR", 5);
                data.DataOriginatorHour = dataOriginatorHour;

                long dataOriginatorMinute = readDataInt("DATA ORIGINATOR MINUTE", 6);
                data.DataOriginatorMinute = dataOriginatorMinute;

                long dataOriginatorSecond = readDataInt("DATA ORIGINATOR SECOND", 6);
                data.DataOriginatorSecond = dataOriginatorSecond;

                long entityIdSerialNumberFpi = readDataInt("ENTITY ID SERIAL NUMBER FPI", 1);

                if (entityIdSerialNumberFpi == 1)
                {
                    long entityIdSerialNumber = readDataInt("ENTITY ID SERIAL NUMBER", 32);
                    data.EntityIdSerialNumber = entityIdSerialNumber;
                }

                long actionDesignator = readDataInt("ACTION DESIGNATOR", 2);
                data.ActionDesignator = actionDesignator;

                long securityClassification = readDataInt("SECURITY CLASSIFICATION", 4);
                data.SecurityClassification = securityClassification;

                long entityInformationGpi = readDataInt("ENTITY INFORMATION GPI", 1);

                if (entityInformationGpi == 1)
                {
                    K0519_EntityInformation entityInformation = new K0519_EntityInformation();

                    long symbolCodeFpi = readDataInt("SYMBOL CODE FPI", 1);

                    if (symbolCodeFpi == 1)
                    {
                        string symbolCode = readDataString("SYMBOL CODE", 105);
                        entityInformation.SymbolCode = symbolCode;
                    }

                    long entityDescriptionGpi = readDataInt("ENTITY DESCRIPTION GPI", 1);

                    if (entityDescriptionGpi == 1)
                    {
                        K0519_EntityDescription entityDescription = new K0519_EntityDescription();

                        long identityVmf = readDataInt("IDENTITY, VMF", 4);
                        entityDescription.IdentityVmf = identityVmf;

                        long symbolDimension = readDataInt("SYMBOL DIMENSION", 5);
                        entityDescription.SymbolDimension = symbolDimension;

                        long entityType = readDataInt("ENTITY TYPE", 6);
                        entityDescription.EntityType = entityType;

                        long entitySubTypeFpi = readDataInt("ENTITY SUBTYPE FPI", 1);

                        if (entitySubTypeFpi == 1)
                        {
                            long entitySubType = readDataInt("ENTITY SUBTYPE", 6);
                            entityDescription.EntitySubType = entitySubType;
                        }

                        long entitySizeMobilityFpi = readDataInt("ENTITY SIZE/MOBILITY FPI", 1);

                        if (entitySizeMobilityFpi == 1)
                        {
                            long entitySizeMobility = readDataInt("ENTITY SIZE/MOBILITY", 8);
                            entityDescription.EntitySizeMobility = entitySizeMobility;
                        }

                        long entityStatusFpi = readDataInt("ENTITY STATUS FPI", 1);

                        if (entityStatusFpi == 1)
                        {
                            long entityStatus = readDataInt("ENTITY STATUS", 1);
                            entityDescription.EntityStatus = entityStatus;
                        }

                        long nationalityFpi = readDataInt("NATIONALITY FPI", 1);

                        if (nationalityFpi == 1)
                        {
                            long nationality = readDataInt("NATIONALITY", 9);
                            entityDescription.Nationality = nationality;
                        }

                        entityInformation.EntityDescription = entityDescription;
                    } // ENTITY DESCRIPTION GPI - G2

                    long additionalInformationFpi = readDataInt("ADDITIONAL INFORMATION FPI", 1);

                    if (additionalInformationFpi == 1)
                    {
                        bool additionalInformationFriRepeat = false;

                        do
                        {
                            long additionalInformationFri = readDataInt("ADDITIONAL INFORMATION FRI", 1);
                            additionalInformationFriRepeat = (additionalInformationFri == 1);

                            string additionalInformation = readDataString("ADDITIONAL INFORMATION", 140);

                            entityInformation.AdditionalInformationList.Add(additionalInformation);
                        } while (additionalInformationFriRepeat);
                    } // ADDITIONAL INFORMATION FPI

                    long uniqueDesignationFpi = readDataInt("UNIQUE DESIGNATION FPI", 1);

                    if (uniqueDesignationFpi == 1)
                    {
                        bool uniqueDesignationFriRepeat = false;

                        do
                        {
                            long uniqueDesignationFri = readDataInt("UNIQUE DESIGNATION FRI", 1);
                            uniqueDesignationFriRepeat = (uniqueDesignationFri == 1);

                            string uniqueDesignation = readDataString("UNIQUE DESIGNATION", 245);

                            entityInformation.UniqueDesignationList.Add(uniqueDesignation);
                        } while (uniqueDesignationFriRepeat);
                    } // UNIQUE DESIGNATION FPI

                    long staffCommentsFpi = readDataInt("STAFF COMMENTS FPI", 1);

                    if (staffCommentsFpi == 1)
                    {
                        string staffComments = readDataString("STAFF COMMENTS", 140);
                        entityInformation.StaffComments = staffComments;
                    } // STAFF COMMENTS FPI

                    data.EntityInformation = entityInformation;
                } // ENTITY INFORMATION GPI - G1

                long fireMissionInitiationDeletionGpi = readDataInt("FIRE MISSION INITIATION/DELETION GPI", 1);

                if (fireMissionInitiationDeletionGpi == 1)
                {
                    K0519_FireMission fireMission_Gpi = new K0519_FireMission();

                    long fireMissionTypeFpi = readDataInt("FIRE MISSION TYPE FPI", 1);

                    if (fireMissionTypeFpi == 1)
                    {
                        fireMission_Gpi.FireMissionType = readDataInt("FIRE MISSION TYPE", 4);
                    }

                    long targetNumberFpi = readDataInt("TARGET NUMBER FPI", 1);

                    if (targetNumberFpi == 1)
                    {
                        fireMission_Gpi.TargetNumber = readDataString("TARGET NUMBER", 28);
                    }

                    long missionEngagementGpi = readDataInt("MISSION ENGAGEMENT GPI", 1);

                    if (missionEngagementGpi == 1)
                    {
                        fireMission_Gpi.EngagementType = readDataInt("ENGAGEMENT TYPE", 2);

                        long observerUrnFpi = readDataInt("OBSERVER URN FPI", 1);

                        if (observerUrnFpi == 1)
                        {
                            fireMission_Gpi.ObserverUrn = readDataInt("OBSERVER URN", 24);
                        }

                        long targetLocationGpi = readDataInt("TARGET LOCATION GPI", 1);

                        if (targetLocationGpi == 1)
                        {
                            long targetLatitude = readDataInt("TARGET LOCATION LATITUDE", 25);
                            fireMission_Gpi.TargetLatitude = targetLatitude;
                            fireMission_Gpi.TargetLatitudeAsDegree = DFIDUI.Convert_281_407(targetLatitude);

                            long targetLongitude = readDataInt("TARGET LOCATION LONGITUDE", 26);
                            fireMission_Gpi.TargetLongitude = targetLongitude;
                            fireMission_Gpi.TargetLongitudeAsDegree = DFIDUI.Convert_281_407(targetLongitude);
                        } // TARGET LOCATION GPI - G5

                        long timeOnTargetGpi = readDataInt("TIME ON TARGET GPI", 1);

                        if (timeOnTargetGpi == 1)
                        {
                            fireMission_Gpi.DayOnTarget = readDataInt("DAY ON TARGET", 5);

                            fireMission_Gpi.HourOnTarget = readDataInt("HOUR ON TARGET", 5);

                            fireMission_Gpi.MinuteOnTarget = readDataInt("MINUTE ON TARGET", 6);
                        } // TIME ON TARGET GPI - G6
                    } // MISSION ENGAGEMENT GPI - G4

                    data.FireMission = fireMission_Gpi;
                } // FIRE MISSION INITIATION/DELETION GPI - G3

                long obstacleGroupGpi = readDataInt("OBSTACLE GROUP GPI", 1);

                if (obstacleGroupGpi == 1)
                {
                    K0519_ObstacleGroup obstacleGroup = new K0519_ObstacleGroup();

                    long entityAxisOrientationFpi = readDataInt("ENTITY AXIS ORIENTATION FPI", 1);

                    if (entityAxisOrientationFpi == 1)
                    {
                        obstacleGroup.EntityAxisOrientation = readDataInt("ENTITY AXIS ORIENTATION", 9);
                    }

                    bool obstacleDtgGriRepeat = false;

                    do
                    {
                        K0519_ObstacleDtg timeDateParameters = new K0519_ObstacleDtg();

                        long obstacleDtgGri = readDataInt("OBSTACLE DTG GRI", 1);
                        obstacleDtgGriRepeat = (obstacleDtgGri == 1);

                        long timeFunction = readDataInt("OBSTACLE TIME FUNCTION", 3);
                        timeDateParameters.TimeFunction = timeFunction;

                        long obstacleDtgYear = readDataInt("OBSTACLE DTG YEAR", 7);
                        timeDateParameters.Year = obstacleDtgYear;

                        long obstacleDtgMonth = readDataInt("OBSTACLE DTG MONTH", 4);
                        timeDateParameters.Month = obstacleDtgMonth;

                        long obstacleDtgDay = readDataInt("OBSTACLE DTG DAY OF MONTH", 5);
                        timeDateParameters.Day = obstacleDtgDay;

                        long obstacleDtgHour = readDataInt("OBSTACLE DTG HOUR", 5);
                        timeDateParameters.Hour = obstacleDtgHour;

                        long obstacleDtgMinute = readDataInt("OBSTACLE DTG MINUTE", 6);
                        timeDateParameters.Minute = obstacleDtgMinute;

                        long obstacleDtgSecondFpi = readDataInt("OBSTACLE DTG SECOND FPI", 1);

                        if (obstacleDtgSecondFpi == 1)
                        {
                            long obstacleDtgSecond = readDataInt("OBSTACLE DTG SECOND", 6);
                            timeDateParameters.Second = obstacleDtgSecond;
                        }

                        obstacleGroup.TimeParameterList.Add(timeDateParameters);
                    } while (obstacleDtgGriRepeat);

                    long impactOnMovementFpi = readDataInt("IMPACT ON MOVEMENT FPI", 1);

                    if (impactOnMovementFpi == 1)
                    {
                        long impactOnMovement = readDataInt("IMPACT ON MOVEMENT", 3);
                        obstacleGroup.ImpactOnMovement = impactOnMovement;
                    }

                    long obstacleHeightFpi = readDataInt("OBSTACLE HEIGHT FPI", 1);

                    if (obstacleHeightFpi == 1)
                    {
                        long obstacleHeight = readDataInt("OBSTACLE HEIGHT", 10);
                        obstacleGroup.ObstacleHeight = obstacleHeight;
                    }

                    long obstacleDepthFpi = readDataInt("OBSTACLE DEPTH FPI", 1);

                    if (obstacleDepthFpi == 1)
                    {
                        long obstacleDepth = readDataInt("OBSTACLE DEPTH", 5);
                        obstacleGroup.ObstacleDepth = obstacleDepth;
                    }

                    long obstacleLengthFpi = readDataInt("OBSTACLE LENGTH FPI", 1);

                    if (obstacleLengthFpi == 1)
                    {
                        long obstacleLength = readDataInt("OBSTACLE LENGTH", 10);
                        obstacleGroup.ObstacleLength = obstacleLength;
                    }

                    long obstacleWidthFpi = readDataInt("OBSTACLE WIDTH FPI", 1);

                    if (obstacleWidthFpi == 1)
                    {
                        long obstacleWidth = readDataInt("OBSTACLE WIDTH", 10);
                        obstacleGroup.ObstacleWidth = obstacleWidth;
                    }

                    bool obstaclePerimeterGriRepeat = false;

                    do
                    {
                        K0519_ObstacleLocation obstacleLocation = new K0519_ObstacleLocation();

                        long obstaclePerimeterGri = readDataInt("OBSTACLE PERIMETER GRI", 1);
                        obstaclePerimeterGriRepeat = (obstaclePerimeterGri == 1);

                        long obstacleLatitude = readDataInt("OBSTACLE LATITUDE", 25);
                        obstacleLocation.Latitude = obstacleLatitude;
                        obstacleLocation.LatitudeAsDegree = DFIDUI.Convert_281_461(obstacleLatitude);

                        long obstacleLongitude = readDataInt("OBSTACLE LONGITUDE", 26);
                        obstacleLocation.Longitude = obstacleLongitude;
                        obstacleLocation.LongitudeAsDegree = DFIDUI.Convert_282_461(obstacleLongitude);

                        obstacleGroup.ObstacleLocationList.Add(obstacleLocation);
                    } while (obstaclePerimeterGriRepeat);

                    long enemyActivityFpi = readDataInt("ENEMY ACTIVITY FPI", 1);

                    if (enemyActivityFpi == 1)
                    {
                        long enemyActivity = readDataInt("ENEMY ACTIVITY", 6);
                        obstacleGroup.EnemyActivity = enemyActivity;
                    }

                    long zoneMarkingFpi = readDataInt("ZONE MARKING FPI", 1);

                    if (zoneMarkingFpi == 1)
                    {
                        long zoneMarking = readDataInt("ZONE MARKING", 4);
                        obstacleGroup.ZoneMarking = zoneMarking;
                    }

                    long controllerUnitUrnFpi = readDataInt("CONTROLLER UNIT URN FPI", 1);

                    if (controllerUnitUrnFpi == 1)
                    {
                        long controllerUnitUrn = readDataInt("CONTROLLER UNIT URN", 24);
                        obstacleGroup.ControllerUrn = controllerUnitUrn;
                    }

                    long obstacleTypeFpi = readDataInt("OBSTACLE TYPE FPI", 1);

                    if (obstacleTypeFpi == 1)
                    {
                        long obstacleType = readDataInt("OBSTACLE TYPE", 7);
                        obstacleGroup.ObstacleType = obstacleType;
                    }

                    long obstacleStatusFpi = readDataInt("OBSTACLE STATUS FPI", 1);

                    if (obstacleStatusFpi == 1)
                    {
                        long obstacleStatus = readDataInt("OBSTACLE STATUS", 5);
                        obstacleGroup.ObstacleStatus = obstacleStatus;
                    }

                    long safeLaneStatusGpi = readDataInt("SAFE LANE STATUS GPI", 1);

                    if (safeLaneStatusGpi == 1)
                    {
                        long safeLaneStatus = readDataInt("SAFE LANE STATUS", 1);

                        bool safeLaneGriRepeat = false;

                        do
                        {
                            K0519_SafeLaneStatus safeLaneStatus_Gpi = new K0519_SafeLaneStatus();

                            long safeLaneGri = readDataInt("SAFE LANE GRI", 1);
                            safeLaneGriRepeat = (safeLaneGri == 1);

                            long safeLaneLatitude = readDataInt("SAFE LANE LATITUDE", 31);
                            safeLaneStatus_Gpi.SafeLaneLatitude = safeLaneLatitude;
                            safeLaneStatus_Gpi.SafeLaneLatitudeAsDegree = DFIDUI.Convert_281_426(safeLaneLatitude);

                            long safeLaneLongitude = readDataInt("SAFE LANE LONGITUDE", 32);
                            safeLaneStatus_Gpi.SafeLaneLongitude = safeLaneLongitude;
                            safeLaneStatus_Gpi.SafeLaneLongitudeAsDegree = DFIDUI.Convert_282_426(safeLaneLongitude);

                            long safeLaneWidth = readDataInt("SAFE LANE WIDTH", 7);
                            safeLaneStatus_Gpi.SafeLaneWidth = safeLaneWidth;
                        } while (safeLaneGriRepeat);

                        long safeLaneSymbolCodeFpi = readDataInt("SAFE LANE SYMBOL CODE FPI", 1);

                        if (safeLaneSymbolCodeFpi == 1)
                        {
                            string safeLaneSymbolCode = readDataString("SAFE LANE SYMBOL CODE", 105);
                            obstacleGroup.SafeLaneSymbolCode = safeLaneSymbolCode;
                        }
                    } // SAFE LANE STATUS GPI - G8

                    data.ObstacleGroup = obstacleGroup;
                } // OBSTACLE GROUP GPI - G7

                long strikeWarningGpi = readDataInt("STRIKE WARNING GPI", 1);

                if (strikeWarningGpi == 1)
                {
                    K0519_StrikeWarning strikeWarning_Gpi = new K0519_StrikeWarning();

                    long conventionalNuclearWarning = readDataInt("CONVENTIONAL/NUCLEAR WARNING", 1);

                    long strikeStartYear = readDataInt("STRIKE START YEAR", 7);

                    long strikeStartMonth = readDataInt("STRIKE START MONTH", 4);

                    long strikeStartDay = readDataInt("STRIKE START DAY", 5);

                    long strikeStartHour = readDataInt("STRIKE START HOUR", 5);

                    long strikeStartMinute = readDataInt("STRIKE START MINUTE", 6);

                    long troopSafetyGpi = readDataInt("TROOP SAFETY GPI", 1);

                    if (troopSafetyGpi == 1)
                    {
                        long minimumSafeDistance1RadiusFpi = readDataInt("MINIMUM SAFE DISTANCE 1 RADIUS FPI", 1);

                        if (minimumSafeDistance1RadiusFpi == 1)
                        {
                            long minimumSafeDistance1Radius = readDataInt("MINIMUM SAFE DISTANCE 1 RADIUS", 10);
                        }

                        long minimumSafeDistance2RadiusFpi = readDataInt("MINIMUM SAFE DISTANCE 2 RADIUS FPI", 1);

                        if (minimumSafeDistance2RadiusFpi == 1)
                        {
                            long minimumSafeDistance2Radius = readDataInt("MINIMUM SAFE DISTANCE 2 RADIUS", 10);
                        }

                        long groundZeroLocationGpi = readDataInt("GROUND ZERO LOCATION GPI", 1);

                        if (groundZeroLocationGpi == 1)
                        {
                            bool groundZeroLocationGriRepeat = false;

                            do
                            {
                                long groundZeroLocationGri = readDataInt("GROUND ZERO LOCATION GRI", 1);
                                groundZeroLocationGriRepeat = (groundZeroLocationGri == 1);

                                long groundZeroLocationLatitude = readDataInt("GROUND ZERO LOCATION LATITUDE", 25);

                                long groundZeroLocationLongitude = readDataInt("GROUND ZERO LOCATION LONGITUDE", 26);
                            } while (groundZeroLocationGriRepeat);
                        } // GROUND ZERO LOCATION GPI - G11
                    } // TROOP SAFETY GPI - G10

                    data.StrikeWarning = strikeWarning_Gpi;
                } // STRIKE WARNING GPI - G9

                long threatWarningGpi = readDataInt("THREAT WARNING GPI", 1);

                if (threatWarningGpi == 1)
                {
                    K0519_ThreatWarning threatWarning = new K0519_ThreatWarning();

                    long threatTypeFpi = readDataInt("THREAT TYPE FPI", 1);

                    if (threatTypeFpi == 1)
                    {
                        long threatType = readDataInt("THREAT TYPE", 4);
                    }

                    long threatPostureVmf = readDataInt("THREAT POSTURE, VMF", 3);

                    long threatImpactDtgGpi = readDataInt("THREAT IMPACT DTG GPI", 1);

                    if (threatImpactDtgGpi == 1)
                    {
                        long threatImpactYear = readDataInt("THREAT IMPACT YEAR", 7);

                        long threatImpactMonth = readDataInt("THREAT IMPACT MONTH", 4);

                        long threatImpactDay = readDataInt("THREAT IMPACT DAY OF MONTH", 5);

                        long threatImpactHour = readDataInt("THREAT IMPACT HOUR", 5);

                        long threatImpactMinute = readDataInt("THREAT IMPACT MINUTE", 6);

                        long threatImpactSecondFpi = readDataInt("THREAT IMPACT SECOND FPI", 1);

                        if (threatImpactSecondFpi == 1)
                        {
                            long threatImpactSecond = readDataInt("THREAT IMPACT SECOND", 6);
                        }
                    } // THREAT IMPACT DTG GPI - G13

                    long threatLocationGpi = readDataInt("THREAT LOCATION GPI", 1);

                    if (threatLocationGpi == 1)
                    {
                        long threatLocationLatitude0051Minute = readDataInt("THREAT LOCATION LATITUDE 0.0051 MINUTE", 21);
                        long threatLocationLongitude0051Minute = readDataInt("THREAT LOCATION LONGITUDE 0.0051 MINUTE", 22);

                        long altitude25FtFpi = readDataInt("ALTITUDE, 25 FT FPI", 1);

                        if (altitude25FtFpi == 1)
                        {
                            long altitude25Ft = readDataInt("ALTITUDE, 25 FT", 13);
                        }

                        long strengthFpi = readDataInt("STRENGTH FPI", 1);

                        if (strengthFpi == 1)
                        {
                            long strength = readDataInt("STRENGHT", 4);
                        }

                        long courseFpi = readDataInt("COURSE FPI", 1);

                        if (courseFpi == 1)
                        {
                            long course = readDataInt("COURSE", 9);
                        }

                        long speedFpi = readDataInt("SPEED FPI", 1);

                        if (speedFpi == 1)
                        {
                            long speed = readDataInt("SPEED", 11);
                        }
                    } // THREAT LOCATION GPI - G14

                    long threatImpactLocationGpi = readDataInt("THREAT IMPACT LOCATION GPI", 1);

                    if (threatImpactLocationGpi == 1)
                    {
                        long threatImpactLocationLatitude0103Minute = readDataInt("THREAT IMPACT LOCATION LATITUDE 0.0103 MINUTE", 20);
                        long threatImpactLocationLongitude0103Minute = readDataInt("THREAT IMPACT LOCATION LONGITUDE 0.0103 MINUTE", 21);

                        long ballisticMissileImpactGpi = readDataInt("BALLISTIC MISSILE IMPACT GPI", 1);

                        if (ballisticMissileImpactGpi == 1)
                        {
                            long squareCircleSwitch = readDataInt("SQUARE/CIRCLE SWITCH", 2);

                            long axisOrientation = readDataInt("AXIS ORIENTATION", 8);

                            long areaMajorAxis4 = readDataInt("AREA MAJOR AXIS, 4", 7);

                            long areaMinorAxis4 = readDataInt("AREA MINOR AXIS, 4", 7);
                        } // BALLISTIC MISSILE IMPACT GPI - G16
                    } // THREAT IMPACT LOCATION GPI - G15

                    data.ThreatWarning = threatWarning;
                } // THREAT WARNING GPI - G12

                long nbcAlertGpi = readDataInt("NBC ALERT GPI", 1);

                if (nbcAlertGpi == 1)
                {
                    K0519_NbcAlert nbcAlert = new K0519_NbcAlert();

                    long nbcAlertType = readDataInt("NBC ALERT TYPE", 4);

                    long typeOfNuclearBurstFpi = readDataInt("TYPE OF NUCLEAR BURST FPI", 1);

                    if (typeOfNuclearBurstFpi == 1)
                    {
                        long typeOfNuclearBurst = readDataInt("TYPE OF NUCLEAR BURST", 3);
                    }

                    bool nbcAlertDtgGriRepeat = false;

                    do
                    {
                        long nbcAlertDtgGri = readDataInt("NBC ALERT DTG GRI", 1);
                        nbcAlertDtgGriRepeat = (nbcAlertDtgGri == 1);

                        long timeFunctionNbc = readDataInt("TIME FUNCTION, NBC", 4);

                        long nbcAlertDtgDay = readDataInt("NBC ALERT DTG DAY OF MONTH", 5);

                        long nbcAlertDtgHour = readDataInt("NBC ALERT DTG HOUR", 5);

                        long nbcAlertDtgMinute = readDataInt("NBC ALERT DTG MINUTE", 6);
                    } while (nbcAlertDtgGriRepeat);

                    long nbcAlertLocationGpi = readDataInt("NBC ALERT LOCATION GPI", 1);

                    if (nbcAlertLocationGpi == 1)
                    {
                        long nbcAlertLocationQualifier = readDataInt("NBC ALERT LOCATION QUALIFIER", 1);

                        long attackLocationLatitude = readDataInt("ATTACK LOCATION LATITUDE", 25);

                        long attackLocationLongitude = readDataInt("ATTACK LOCATION LONGITUDE", 26);
                    } // NBC ALERT LOCATION GPI - G18

                    long nbcAlertLocationCharacteristicsGpi = readDataInt("NBC ALERT LOCATION CHARACTERISTICS GPI", 1);

                    if (nbcAlertLocationCharacteristicsGpi == 1)
                    {
                        long terrainDescriptionFpi = readDataInt("TERRAIN DESCRIPTION FPI", 1);

                        if (terrainDescriptionFpi == 1)
                        {
                            long terrainDescription = readDataInt("TERRAIN DESCRIPTION", 4);
                        }

                        long vegetationTypeFpi = readDataInt("VEGETATION TYPE FPI", 1);

                        if (vegetationTypeFpi == 1)
                        {
                            long vegetationType = readDataInt("VEGETATION TYPE", 3);
                        }

                        bool nbcAlertReleaseDataGriRepeat = false;

                        do
                        {
                            long nbcAlertReleaseDataGri = readDataInt("NBC ALERT RELEASE DATA GRI", 1);
                            nbcAlertReleaseDataGriRepeat = (nbcAlertReleaseDataGri == 1);

                            long agentTypeFpi = readDataInt("AGENT TYPE FPI", 1);

                            if (agentTypeFpi == 1)
                            {
                                long agentType = readDataInt("AGENT TYPE", 5);
                            }

                            long agentNameFpi = readDataInt("AGENT NAME FPI", 1);

                            if (agentNameFpi == 1)
                            {
                                long agentName = readDataInt("AGENT NAME", 6);
                            }

                            long agentPersistencyTypeFpi = readDataInt("AGENT PERSISTENCY TYPE FPI", 1);

                            if (agentPersistencyTypeFpi == 1)
                            {
                                long agentPersistency = readDataInt("AGENT PERSISTENCY TYPE", 2);
                            }

                            long typeOfDetectionFpi = readDataInt("TYPE OF DETECTION FPI", 1);

                            if (typeOfDetectionFpi == 1)
                            {
                                bool typeOfDetectionFriRepeat = false;

                                do
                                {
                                    long typeOfDetectionFri = readDataInt("TYPE OF DETECTION FRI", 1);
                                    typeOfDetectionFriRepeat = (typeOfDetectionFri == 1);

                                    long typeOfDetection = readDataInt("TYPE OF DETECTION", 4);
                                } while (typeOfDetectionFriRepeat);
                            }
                        } while (nbcAlertReleaseDataGriRepeat);
                    } // NBC ALERT CHARACTERISTICS GPI - G19

                    data.NbcAlert = nbcAlert;
                } // NBC ALERT GPI - G17

                long bridgeGpi = readDataInt("BRIDGE GPI", 1);

                if (bridgeGpi == 1)
                {
                    K0519_Bridge bridge = new K0519_Bridge();

                    long bridgeTypeFpi = readDataInt("BRIDGE TYPE FPI", 1);

                    if (bridgeTypeFpi == 1)
                    {
                        long bridgeType = readDataInt("BRIDGE TYPE", 3);
                    }

                    bool bridgeLocationGriRepeat = false;

                    do
                    {
                        long bridgeLocationGri = readDataInt("BRIDGE LOCATION GRI", 1);
                        bridgeLocationGriRepeat = (bridgeLocationGri == 1);

                        long bridgeLocationLatitude0013Minute = readDataInt("BRIDGE LOCATION LATITUDE 0.0013 MINUTE", 23);

                        long bridgeLocationLongitude0013Minute = readDataInt("BRIDGE LOCATION LONGITUDE 0.0013 MINUTE", 24);
                    } while (bridgeLocationGriRepeat);

                    long controllingForce = readDataInt("CONTROLLING FORCE", 2);

                    long widthOfBridgeFpi = readDataInt("WIDTH OF BRIDGE FPI", 1);

                    if (widthOfBridgeFpi == 1)
                    {
                        long widthOfBridge = readDataInt("WIDTH OF BRIDGE", 10);
                    }

                    long bridgeWeightClassificationFpi = readDataInt("BRIDGE WEIGHT CLASSIFICATION FPI", 1);

                    if (bridgeWeightClassificationFpi == 1)
                    {
                        long bridgeWeightClassification = readDataInt("BRIDGE WEIGHT CLASSIFICATION", 10);
                    }

                    data.Bridge = bridge;
                } // BRIDGE GPI - G20

                long supplyPolongGpi = readDataInt("SUPPLY POlong GPI", 1);

                if (supplyPolongGpi == 1)
                {
                    K0519_SupplyPolong supplyPolong = new K0519_SupplyPolong();

                    long supplyPolongReferenceNumber = readDataInt("SUPPLY POlong REFERENCE NUMBER", 24);

                    long supplyPolongTypeFpi = readDataInt("SUPPLY POlong TYPE FPI", 1);

                    if (supplyPolongTypeFpi == 1)
                    {
                        long supplyPolongType = readDataInt("SUPPLY POlong TYPE", 5);
                    }

                    long supplyPolongLocationGpi = readDataInt("SUPPLY POlong LOCATION GPI", 1);

                    if (supplyPolongLocationGpi == 1)
                    {
                        bool supplyPolongLocationGriRepeat = false;

                        do
                        {
                            long supplyPolongLocationGri = readDataInt("SUPPLY POlong LOCATION GRI", 1);
                            supplyPolongLocationGriRepeat = (supplyPolongLocationGri == 1);

                            long supplyPolongLatitude = readDataInt("SUPPLY POlong LATITUDE", 25);

                            long supplyPolongLongitude = readDataInt("SUPPLY POlong LONGITUDE", 26);
                        } while (supplyPolongLocationGriRepeat);
                    } // SUPPLY POlong LOCATION GPI - G22

                    long supplyPolongOperationTimeGpi = readDataInt("SUPPLY POlong OPERATION TIME GPI", 1);

                    if (supplyPolongOperationTimeGpi == 1)
                    {
                        bool openAndCloseTimesGriRepeat = false;

                        do
                        {
                            long openAndCloseTimesGri = readDataInt("OPEN AND CLOSE TIMES GRI", 1);
                            openAndCloseTimesGriRepeat = (openAndCloseTimesGri == 1);

                            long openAndCloseTimesMonth = readDataInt("OPEN AND CLOSE TIMES MONTH", 4);

                            long openAndCloseTimesHour = readDataInt("OPEN AND CLOSE TIMES HOUR", 5);

                            long openAndCloseTimesMinute = readDataInt("OPEN AND CLOSE TIMES MINUTE", 6);

                        } while (openAndCloseTimesGriRepeat);
                    } // SUPPLY POlong OPERATION TIME GPI - G23

                    data.SupplyPolong = supplyPolong;
                } // SUPPLY POlong GPI - G21

                long observedPositionsGpi = readDataInt("OBSERVED POSITIONS GPI", 1);

                if (observedPositionsGpi == 1)
                {
                    K0519_ObservedPositions observedPositions = new K0519_ObservedPositions();

                    long locationDerivation = readDataInt("LOCATION DERIVATION", 4);

                    long eplrsRsIdFpi = readDataInt("EPLRS RS ID FPI", 1);

                    if (eplrsRsIdFpi == 1)
                    {
                        long eplrsRsId = readDataInt("EPLRS RS ID", 14);
                    }

                    long observedPositionsLatitude0013Minute = readDataInt("OBSERVED POSITIONS LATITUDE 0.0013 MINUTE", 23);

                    long observedPositionsLongitude0013Minute = readDataInt("OBSERVED POSITIONS LONGITUDE 0.0013 MINUTE", 24);

                    long locationQualityFpi = readDataInt("LOCATION QUALITY FPI", 1);

                    if (locationQualityFpi == 1)
                    {
                        long locationQuality = readDataInt("LOCATION QUALITY", 4);
                    }

                    long observedCourseAndSpeedGpi = readDataInt("OBSERVED COURSE AND SPEED GPI", 1);

                    if (observedCourseAndSpeedGpi == 1)
                    {
                        long course = readDataInt("COURSE", 9);

                        long unitSpeedKph = readDataInt("UNIT SPEED, KPH", 11);
                    } // OBSERVED COURSE AND SPEED GPI - G25

                    long activityFpi = readDataInt("ACTIVITY FPI", 1);

                    if (activityFpi == 1)
                    {
                        long activity = readDataInt("ACTIVITY", 6);
                    }

                    long elevationFeetFpi = readDataInt("ELEVATION, FEET FPI", 1);

                    if (elevationFeetFpi == 1)
                    {
                        long elevationFeet = readDataInt("ELEVATION, FEET", 17);
                    }

                    long altitude25FtFpi = readDataInt("ALTITUDE, 25 FT FPI", 1);

                    if (altitude25FtFpi == 1)
                    {
                        long altitude25Ft = readDataInt("ALTITUDE, 25 FT", 13);
                    }

                    long quantityOfEquipmentWeaponsFpi = readDataInt("QUANTITY OF EQUIPMENT/WEAPONS FPI", 1);

                    if (quantityOfEquipmentWeaponsFpi == 1)
                    {
                        long quantityOfEquipmentWeapons = readDataInt("QUANTITY OF EQUIPMENT/WEAPONS", 14);
                    }

                    long actionTakenFpi = readDataInt("ACTION TAKEN FPI", 1);

                    if (actionTakenFpi == 1)
                    {
                        long actionTaken = readDataInt("ACTION TAKEN", 6);
                    }

                    data.ObservedPositions = observedPositions;
                } // OBSERVED POSITIONS GPI - G24

                long entityCombatStatusGpi = readDataInt("ENTITY COMBAT STATUS GPI", 1);

                if (entityCombatStatusGpi == 1)
                {
                    K0519_EntityCombatStatus entityCombatStatus_Gpi = new K0519_EntityCombatStatus();

                    long entityUrnFpi = readDataInt("ENTITY URN FPI", 1);

                    if (entityUrnFpi == 1)
                    {
                        long entityUrn = readDataInt("ENTITY URN", 24);
                    }

                    long entityCombatStatus = readDataInt("ENTITY COMBAT STATUS", 2);

                    long medevacRequestGpi = readDataInt("MEDEVAC REQUEST GPI", 1);

                    if (medevacRequestGpi == 1)
                    {
                        long medevacMissionType = readDataInt("MEDEVAC MISSION TYPE", 2);

                        long medevacRequestNumberFpi = readDataInt("MEDEVAC REQUEST NUMBER FPI", 1);

                        if (medevacRequestNumberFpi == 1)
                        {
                            string medevacRequestNumber = readDataString("MEDEVAC REQUEST NUMBER", 35);
                        }

                        long casualtyPickupLocationGpi = readDataInt("CASUALTY/PICKUP LOCATION GPI", 1);

                        if (casualtyPickupLocationGpi == 1)
                        {
                            long casualtyPickupLocationLatitude0051Minute = readDataInt("CASUALTY/PICKUP LOCATION LATITUDE 0.0051 MINUTE", 21);

                            long casualtyPickupLocationLongitude0051Minute = readDataInt("CASUALTY/PICKUP LOCATION LONGITUDE 0.0051 MINUTE", 21);

                        } // CASUALTY/PICKUP LOCATION GPI - G28

                        long pickupTimeGpi = readDataInt("PICK-UP TIME GPI", 1);

                        if (pickupTimeGpi == 1)
                        {
                            long pickupDay = readDataInt("PICK UP DAY", 5);

                            long pickupHour = readDataInt("PICK UP HOUR", 5);

                            long pickupMinute = readDataInt("PICK UP MINUTE", 6);
                        } // PICK-UP TIME GPI - G29

                        long zoneMarkingFpi = readDataInt("ZONE MARKING FPI", 1);

                        if (zoneMarkingFpi == 1)
                        {
                            long zoneMarking = readDataInt("ZONE MARKING", 4);
                        }

                        long zoneHot = readDataInt("ZONE HOT", 1);
                    } // MEDEVAC REQUEST GPI - G27

                    data.EntityCombatStatus = entityCombatStatus_Gpi;
                } // ENTITY COMBAT STATUS GPI - G26

                long iedReportGpi = readDataInt("IED REPORT GPI", 1);

                if (iedReportGpi == 1)
                {
                    K0519_IEDReport iedReport = new K0519_IEDReport();

                    long dimension = readDataInt("DIMENSION", 6);

                    long entityType = readDataInt("ENTITY TYPE", 6);

                    long entitySubTypeFpi = readDataInt("ENTITY SUBTYPE FPI", 1);

                    if (entitySubTypeFpi == 1)
                    {
                        long entitySubType = readDataInt("ENTITY SUBTYPE", 6);
                    }

                    long entityLatitude = readDataInt("ENTITY LATITUDE", 25);

                    long entityLongitude = readDataInt("ENTITY LONGITUDE", 26);

                    long iedStatus = readDataInt("IED STATUS", 2);

                    long staffCommentsFpi = readDataInt("STAFF COMMENTS FPI", 1);

                    if (staffCommentsFpi == 1)
                    {
                        string staffComments = readDataString("STAFF COMMENTS", 140);
                    }

                    data.IEDReport = iedReport;
                } // IED REPORT GPI - G30

                long bypassGroupGpi = readDataInt("BYPASS GROUP GPI", 1);

                if (bypassGroupGpi == 1)
                {
                    K0519_BypassGroup bypassGroup = new K0519_BypassGroup();

                    bool bypassGroupGriRepeat = false;

                    do
                    {
                        long bypassGroupGri = readDataInt("BYPASS GROUP GRI", 1);
                        bypassGroupGriRepeat = (bypassGroupGri == 1);

                        long bypassLatitude = readDataInt("BYPASS LATITUDE", 25);

                        long bypassLongitude = readDataInt("BYPASS LONGITUDE", 26);
                    } while (bypassGroupGriRepeat);

                    long bypassPotentialFpi = readDataInt("BYPASS POTENTIAL FPI", 1);

                    if (bypassPotentialFpi == 1)
                    {
                        long bypassPotential = readDataInt("BYPASS POTENTIAL", 2);
                    }

                    long symbolCodeFpi = readDataInt("SYMBOL CODE FPI", 1);

                    if (symbolCodeFpi == 1)
                    {
                        string symbolCode = readDataString("SYMBOL CODE", 105);
                    }

                    data.BypassGroup = bypassGroup;
                } // BYPASS GROUP GPI - G31

                dataList.Add(data);
            } while (entityGriRepeat);

            return dataList;
        }

        public K0701Data ParseK0701(long messageStandardVersion)
        {
            switch (messageStandardVersion)
            {
                case 5: // 6017C enumeration for MIL-STD-47001-D
                case 6: // 6017C enumeration for MIL-STD-47001-C
                case 7:
                    return ParseK0701_CS11_12();
            }

            return null;
        }
        public K0701Data ParseK0701_CS11_12()
        { 
            K0701Data data = new K0701Data();

            data.RequestorCallSign = readDataString("REQUESTOR CALL SIGN", 119);

            data.Urn = readDataInt("URN", 24);
            
            data.RequestResponseDate.Year = readDataInt("REQUEST RESPONSE DATE YEAR", 7);
            data.RequestResponseDate.Month = readDataInt("REQUEST RESPONSE DATE MONTH", 4);
            data.RequestResponseDate.Day = readDataInt("REQUEST RESPONSE DATE DAY OF MONTH", 5);
            data.RequestResponseDate.Hour = readDataInt("REQUEST RESPONSE DATE HOUR", 5);
            data.RequestResponseDate.Minute = readDataInt("REQUEST RESPONSE DATE MINUTE", 6);

            long medevacRequestNumberFpi = readDataInt("MEDEVAC REQUEST NUMBER FPI", 1);

            if (medevacRequestNumberFpi == 1)
            {
                data.MedevacRequestNumber = readDataString("MEDEVAC REQUEST NUMBER", 35);
            }

            data.MedevacMissionType = readDataInt("MEDEVAC MISSION TYPE", 2);

            long numberOfFriendlyKiaFpi = readDataInt("NUMBER OF FRIENDLY KIA FPI", 1);

            if (numberOfFriendlyKiaFpi == 1)
            {
                data.NumberOfFriendlyKia = readDataInt("NUMBER OF FRIENDLY KIA", 14);
            }

            long numberOfFriendlyWiaFpi = readDataInt("NUMBER OF FRIENDLY WIA", 1);

            if (numberOfFriendlyWiaFpi == 1)
            {
                data.NumberOfFriendlyWia = readDataInt("NUMBER OF FRIENDLY WIA", 14);
            }

            bool casualtyDataGriRepeat = false;

            do
            {
                CasualtyData casualtyData = new CasualtyData();

                long casualtyDataGri = readDataInt("CASUALTY DATA GRI", 1);
                casualtyDataGriRepeat = (casualtyDataGri == 1);

                casualtyData.MedevacMissionPriority = readDataInt("MEDEVAC MISSION PRIORITY", 3);

                casualtyData.NumberLitterPatients = readDataInt("NUMBER LITTER PATIENTS", 7);

                casualtyData.NumberAmbulatoryPatients = readDataInt("NUMBER AMBULATORY PATIENTS", 7);

                long casualtyTypeFpi = readDataInt("CASUALTY TYPE FPI", 1);

                if (casualtyTypeFpi == 1)
                {
                    casualtyData.CasualtyType = readDataInt("CASUALTY TYPE", 5);
                }

                long bodyPartAffectedFpi = readDataInt("BODY PART AFFECTED FPI", 1);

                if (bodyPartAffectedFpi == 1)
                {
                    casualtyData.BodyPartAffected = readDataInt("BODY PART AFFECTED", 4);
                }

                casualtyData.MedicRequiredIndicator = readDataInt("MEDIC REQUIRED INDICATOR", 1);

                long specialMedevacEquipmentFpi = readDataInt("SPECIAL MEDEVAC EQUIPMENT FPI", 1);

                if (specialMedevacEquipmentFpi == 1)
                {
                    bool specialMedevacEquipmentRepeat = false;

                    do
                    {
                        long specialMedevacEquipmentFri = readDataInt("SPECIAL MEDEVAC EQUIPMENT FRI", 1);

                        specialMedevacEquipmentRepeat = (specialMedevacEquipmentFri == 1);

                        long specialMedevacEquipment = readDataInt("SPECIAL MEDEVAC EQUIPMENT", 3);

                        casualtyData.SpecialMedevacEquipment.Add(specialMedevacEquipment);
                    } while (specialMedevacEquipmentRepeat);
                }

                long patientNationalityFpi = readDataInt("PATIENT NATIONALITY FPI", 1);

                if (patientNationalityFpi == 1)
                {
                    casualtyData.PatientNationality = readDataInt("PATIENT NATIONALITY", 3);
                }
                
                long nbcContaminationTypeFpi = readDataInt("NBC CONTAMINATION TYPE FPI", 1);

                if (nbcContaminationTypeFpi == 1)
                {
                    casualtyData.NbcContaminationType= readDataInt("NBC CONTAMINATION TYPE", 2);
                }

                data.CasualtyDataList.Add(casualtyData);
            } while (casualtyDataGriRepeat);

            long pickupTimeGpi = readDataInt("PICK UP TIME GPI", 1);

            if (pickupTimeGpi == 1)
            {
                data.PickupTime.Day = readDataInt("PICK UP TIME DAY", 5);

                data.PickupTime.Hour = readDataInt("PICK UP TIME HOUR", 5);

                data.PickupTime.Minute = readDataInt("PICK UP TIME MINUTE", 6);
            }

            long zoneNameFpi = readDataInt("ZONE NAME FPI", 1);

            if (zoneNameFpi == 1)
            {
                data.ZoneName = readDataString("ZONE NAME", 63);
            }

            long pickupLocationGpi = readDataInt("PICK UP LOCATION GPI", 1);

            if (pickupLocationGpi == 1)
            {
                data.PickupLocation.Latitude0051Minute = readDataInt("PICK UP LOCATION LATITUDE 0.0051 MINUTE", 21);

                data.PickupLocation.Latitude0051MinuteAsDegree = DFIDUI.Convert_281_14(data.PickupLocation.Latitude0051Minute);

                data.PickupLocation.Longitude0051Minute = readDataInt("PICK UP LOCATION LONGITUDE 0.0051 MINUTE", 22);

                data.PickupLocation.Longitude0051MinuteAsDegree = DFIDUI.Convert_282_14(data.PickupLocation.Longitude0051Minute);
            }

            long elevationFeetFpi = readDataInt("ELEVATION, FEET FPI", 1);

            if (elevationFeetFpi == 1)
            {
                data.PickupLocation.ElevationFeet = readDataInt("ELEVATION, FEET", 17);
            }

            long agencyContactFrequencyDesignatorFpi = readDataInt("AGENCY CONTACT FREQUENCY DESIGNATOR FPI", 1);

            if (agencyContactFrequencyDesignatorFpi == 1)
            {
                data.AgencyContactFrequencyDesignator = readDataString("AGENCY CONTACT FREQUENCY DESIGNATOR", 56);
            }

            long zoneControllerCallSignFpi = readDataInt("ZONE CONTROLLER CALL SIGN FPI", 1);

            if (zoneControllerCallSignFpi == 1)
            {
                data.ZoneControllerCallSign = readDataString("ZONE CONTROLLER CALL SIGN", 119);
            }

            long zoneMarkingFpi = readDataInt("ZONE MARKING FPI", 1);

            if (zoneMarkingFpi == 1)
            {
                data.ZoneMarking = readDataInt("ZONE MARKING", 4);
            }

            data.ZoneHot = readDataInt("ZONE HOT", 1);

            long enemyDataGpi = readDataInt("ENEMY DATA GPI", 1);

            if (enemyDataGpi == 1)
            {
                bool enemyDataGriRepeat = false;

                do
                {
                    EnemyData enemyData = new EnemyData();

                    long enemyDataGri = readDataInt("ENEMY DATA GRI", 1);

                    enemyDataGriRepeat = (enemyDataGri == 1);

                    enemyData.DirectionToEnemy = readDataInt("DIRECTION TO ENEMY", 4);

                    long hostileFireTypeReceivedFpi = readDataInt("HOSTILE FIRE TYPE RECEIVED FPI", 1);

                    if (hostileFireTypeReceivedFpi == 1)
                    {
                        enemyData.HostileFireTypeReceived = readDataInt("HOSTILE FIRE TYPE RECEIVED", 2);
                    }

                    data.EnemyDataList.Add(enemyData);
                } while (enemyDataGriRepeat);
            }

            long zoneSecurityFpi = readDataInt("ZONE SECURITY FPI", 1);

            if (zoneSecurityFpi == 1)
            {
                data.ZoneSecurity = readDataInt("ZONE SECURITY", 3);
            }

            long zoneMarkingColorFpi = readDataInt("ZONE MARKING COLOR FPI", 1);

            if (zoneMarkingColorFpi == 1)
            {
                data.ZoneMarkingColor = readDataInt("ZONE MARKING COLOR", 4);
            }

            long terrainDescriptionFpi = readDataInt("TERRAIN DESCRIPTION FPI", 1);

            if (terrainDescriptionFpi == 1)
            {
                data.TerrainDescription = readDataInt("TERRAIN DESCRIPTION", 4);
            }

            long weatherConditionsFpi = readDataInt("WEATHER CONDITIONS FPI", 1);

            if (weatherConditionsFpi == 1)
            {
                data.WeatherConditions = readDataInt("WEATHER CONDITIONS", 5);
            }

            long cloudBaseAltitudeFeetFpi = readDataInt("CLOUD BASE ALTITUDE, FEET FPI", 1);

            if (cloudBaseAltitudeFeetFpi == 1)
            {
                data.CloudBaseAltitudeFeet = readDataInt("CLOUD BASE ALTITUDE, FEET", 9);
            }

            long windSpeedFpi = readDataInt("WIND SPEED FPI", 1);

            if (windSpeedFpi == 1)
            {
                data.WindSpeed = readDataInt("WIND SPEED", 7);
            }

            long commentsFpi = readDataInt("COMMENTS FPI", 1);

            if (commentsFpi == 1)
            {
                data.Comments = readDataString("COMMENTS", 1400);
            }

            return data;
        }

        public K0702Data ParseK0702(long messageStandardVersion)
        {
            switch (messageStandardVersion)
            {
                case 5: // 6017C enumeration for MIL-STD-47001-D
                case 6: // 6017C enumeration for MIL-STD-47001
                case 7:
                    return ParseK0702_CS11_12();
            }

            return null;
        }

        public K0702Data ParseK0702_CS11_12()
        {
            K0702Data data = new K0702Data();

            data.NumberOfCasualties = readDataInt("NUMBER OF CASUALTIES", 8);

            long messageGroupGpi = readDataInt("MESSAGE GROUP GPI", 1);

            if (messageGroupGpi == 1)
            {
                bool messageGroupGriRepeat = false;

                do
                {
                    MessageGroupData messageGroupData = new MessageGroupData();

                    long messageGroupGri = readDataInt("MESSAGE GROUP GRI", 1);
                    messageGroupGriRepeat = (messageGroupGri == 1);

                    long payGradeFpi = readDataInt("PAY GRADE FPI", 1);

                    if (payGradeFpi == 1)
                    {
                        messageGroupData.PayGrade = readDataInt("PAY GRADE", 5);
                    }

                    long occupationalSpecialtyFpi = readDataInt("OCCUPATIONAL SPECIALTY FPI", 1);

                    if (occupationalSpecialtyFpi == 1)
                    {
                        messageGroupData.OccupationalSpecialty = readDataString("OCCUPATIONAL SPECIALTY", 35);
                    }

                    messageGroupData.CasualtyType = readDataInt("CASUALTY TYPE", 5);

                    long bodyPartAffectedFpi = readDataInt("BODY PART AFFECTED FPI", 1);

                    if (bodyPartAffectedFpi == 1)
                    {
                        messageGroupData.BodyPartAffected = readDataInt("BODY PART AFFECTED", 4);
                    }

                    long casualtyPersonnelDataGpi = readDataInt("CASUALY PERSONNEL DATA GPI", 1);

                    if (casualtyPersonnelDataGpi == 1)
                    {
                        long lastNameFpi = readDataInt("LAST NAME FPI", 1);

                        if (lastNameFpi == 1)
                        {
                            string lastName = readDataString("LAST NAME", 140);

                            messageGroupData.LastName = lastName;
                        }

                        long initialsFpi = readDataInt("INITIALS FPI", 1);

                        if (initialsFpi == 1)
                        {
                            string initials = readDataString("INITIALS", 14);

                            messageGroupData.Initials = initials;
                        }

                        long socialSecurityNumberFpi = readDataInt("SOCIAL SECURITY NUMBER FPI", 1);

                        if (socialSecurityNumberFpi == 1)
                        {
                            long socialSecurityNumber = readDataInt("SOCIAL SECURITY NUMBER", 30);

                            messageGroupData.SocialSecurityNumber = socialSecurityNumber;
                        }

                        long armedServiceDesignatorFpi = readDataInt("ARMED SERVICE DESIGNATOR", 1);

                        if (armedServiceDesignatorFpi == 1)
                        {
                            long armedServiceDesignator = readDataInt("ARMED SERVICE DESIGNATOR", 3);

                            messageGroupData.ArmedServiceDesignator = armedServiceDesignator;
                        }

                        long actualOrExpectedDispositionFpi = readDataInt("ACTUAL OR EXPECTED DISPOSITION FPI", 1);

                        if (actualOrExpectedDispositionFpi == 1)
                        {
                            long actualOrExpectedDisposition = readDataInt("ACTUAL OR EXPECTED DISPOSITION", 3);

                            messageGroupData.ActualOrExpectedDisposition = actualOrExpectedDisposition;
                        }

                        long burialLocationGpi = readDataInt("BURIAL LOCATION GPI", 1);

                        if (burialLocationGpi == 1)
                        {
                            messageGroupData.BurialSiteLatitude = readDataInt("BURIAL SITE LATITUDE", 25);
                            messageGroupData.BurialSiteLatitudeAsDegree = DFIDUI.Convert_281_423(messageGroupData.BurialSiteLatitude);

                            messageGroupData.BurialSiteLongitude = readDataInt("BURIAL SITE LONGITUDE", 26);
                            messageGroupData.BurialSiteLongitudeAsDegree = DFIDUI.Convert_282_423(messageGroupData.BurialSiteLongitude);
                        }

                        long commentsFpi = readDataInt("COMMENTS FPI", 1);

                        if (commentsFpi == 1)
                        {
                            messageGroupData.Comment = readDataString("COMMENTS", 1400);
                        }
                    }

                    data.MessageGroupList.Add(messageGroupData);
                } while (messageGroupGriRepeat);
            }  // messageGroupGpi - G1

            return data;
        }
        #endregion
    }
}