using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace X_Wing_Pilot_Editor
{
    public class Pilot
    {
        // Pilot data fields

        public string PltName { get; set; }
        public Health PltHealth { get; set; }
        public Rank PltRank { get; set; }
        public uint PltTODScore { get; set; }
        public uint PltRookieNum { get; set; } //experience level - rookie to top ace
        public uint CurrentTour { get; set; }  //is value + 1, so a value of 0 means current tour is tour 1
        public uint CurrentTourOpsComp { get; set; }

        public List<TrainingRecord> TrainingRecordList = new List<TrainingRecord>();
        public List<HistoricCombatRecord> HistoricCombatRecordList = new List<HistoricCombatRecord>();
        public TODStats TODStats;
        public List<TODOperationScore> TourRecordList = new List<TODOperationScore>();
        public Medals Medals;

        public Pilot() // all values should be initialized here before a pilot file is loaded
        {
            PltName = "No Pilot File Loaded";
            PltHealth = Health.Alive;
            PltRank = Rank.Cadet;
            CurrentTour = 0;
            CurrentTourOpsComp = 0;

            foreach (FlyableShipType playerShip in Enum.GetValues(typeof(FlyableShipType)))
            {
                TrainingRecordList.Add(new TrainingRecord(playerShip));
            }

            foreach (HistoricMissionType missionType in Enum.GetValues(typeof(HistoricMissionType)))
            {
                switch (missionType)
                {
                    case HistoricMissionType.XWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, 6));
                        break;
                    case HistoricMissionType.YWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, 6));
                        break;
                    case HistoricMissionType.AWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, 6));
                        break;
                    case HistoricMissionType.BWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, 6));
                        break;
                    case HistoricMissionType.Bonus:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, 6));
                        break;
                }
            }

            TODStats = new TODStats();

            int totalTours = 5;

            for (int i = 1; i < totalTours + 1; i++)
            {
                switch (i)
                {
                    case 1:
                        TourRecordList.Add(new TODOperationScore(i, 12));
                        break;
                    case 2:
                        TourRecordList.Add(new TODOperationScore(i, 12));
                        break;
                    case 3:
                        TourRecordList.Add(new TODOperationScore(i, 14));
                        break;
                    case 4:
                        TourRecordList.Add(new TODOperationScore(i, 24));
                        break;
                    case 5:
                        TourRecordList.Add(new TODOperationScore(i, 24));
                        break;
                }
            }

            Medals = new Medals();

            Medals.KalidorCrescent = (int)KalidorCrescentLevel.None;
        }

        public void GetData(string fileName, byte[] bytes)
        {
            PltName = Path.GetFileNameWithoutExtension(fileName);
            PltHealth = (Health)bytes[2];
            PltRank = (Rank)bytes[3];
            PltTODScore = BitConverter.ToUInt32(bytes, 4);
            PltRookieNum = BitConverter.ToUInt16(bytes, 8);
            CurrentTour = bytes[640];
            CurrentTourOpsComp = BitConverter.ToUInt16(bytes, 643);

            int trainingScoreOffset = 38;
            int trainingCompletedOffset = 134;
            int scoreMultiplier = 0;
            int completedMultuplier = 0;

            int totalTours = 5;

            TrainingRecordList.Clear();

            foreach (FlyableShipType playerShip in Enum.GetValues(typeof(FlyableShipType)))
            {
                TrainingRecordList.Add(new TrainingRecord(playerShip, bytes, trainingScoreOffset + scoreMultiplier, trainingCompletedOffset + completedMultuplier));
                scoreMultiplier += 4; // increments offset to read score
                completedMultuplier += 1; // increments offset to read how many training levels were completed
            }

            HistoricCombatRecordList.Clear();

            foreach (HistoricMissionType missionType in Enum.GetValues(typeof(HistoricMissionType)))
            {
                switch (missionType)
                {
                    case HistoricMissionType.XWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, bytes, 6, 544, 160));
                        break;
                    case HistoricMissionType.YWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, bytes, 6, 560, 224));
                        break;
                    case HistoricMissionType.AWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, bytes, 6, 576, 288));
                        break;
                    case HistoricMissionType.BWing:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, bytes, 6, 592, 352));
                        break;
                    case HistoricMissionType.Bonus:
                        HistoricCombatRecordList.Add(new HistoricCombatRecord(missionType, bytes, 6, 608, 416));
                        break;
                }
            }

            TODStats = new TODStats(bytes);

            TourRecordList.Clear();

            for (int i = 1; i < totalTours + 1; i++)
            {
                switch (i)
                {
                    case 1:
                        TourRecordList.Add(new TODOperationScore(i, bytes, 735, 12, 751, 759));
                        break;
                    case 2:
                        TourRecordList.Add(new TODOperationScore(i, bytes, 736, 12, 752, 859));
                        break;
                    case 3:
                        TourRecordList.Add(new TODOperationScore(i, bytes, 737, 14, 753, 959));
                        break;
                    case 4:
                        TourRecordList.Add(new TODOperationScore(i, bytes, 738, 24, 754, 1059));
                        break;
                    case 5:
                        TourRecordList.Add(new TODOperationScore(i, bytes, 739, 24, 755, 1159));
                        break;
                }
            }

            Medals = new Medals(bytes);
        }

        public void WriteData(string fileName, byte[] oldbytes)
        {
            byte[] bytes = oldbytes;
            // General Pilot Information

            bytes[2] = (byte)PltHealth;
            bytes[3] = (byte)PltRank;
            PltTODScore.CopyToByteArrayLE(bytes, 4);
            PltRookieNum.CopyToByteArrayLE(bytes, 8);
            CurrentTour.CopyToByteArrayLE(bytes, 640);
            CurrentTourOpsComp.CopyToByteArrayLE(bytes, 643);
            CurrentTourOpsComp.CopyToByteArrayLE(bytes, 647);

            // Training Record
            foreach (FlyableShipType playerShip in Enum.GetValues(typeof(FlyableShipType)))
            {
                TrainingRecordList[(int)playerShip].WriteData(playerShip, bytes);

                foreach (HistoricMissionType missionType in Enum.GetValues(typeof(HistoricMissionType)))
                {
                    switch (missionType)
                    {

                        case HistoricMissionType.XWing:
                            HistoricCombatRecordList[(int)missionType].WriteData(bytes, 6, 544, 160);
                            break;

                        case HistoricMissionType.YWing:
                            HistoricCombatRecordList[(int)missionType].WriteData(bytes, 6, 560, 224);
                            break;

                        case HistoricMissionType.AWing:
                            HistoricCombatRecordList[(int)missionType].WriteData(bytes, 6, 576, 288);
                            break;

                        case HistoricMissionType.BWing:
                            HistoricCombatRecordList[(int)missionType].WriteData(bytes, 6, 592, 352);
                            break;

                        case HistoricMissionType.Bonus:
                            HistoricCombatRecordList[(int)missionType].WriteData(bytes, 6, 608, 416);
                            break;
                    }
                }
            }
            // TOD Stats

            TODStats.WriteData(bytes);

            // TOD Record List
            int totalTours = 5;
            for (int i = 0; i < totalTours; i++)
            {
                switch (i)
                {
                    case 0:
                        TourRecordList[i].WriteData(bytes, 735, 12, 751, 759);
                        break;
                    case 1:
                        TourRecordList[i].WriteData(bytes, 736, 12, 752, 859);
                        break;
                    case 2:
                        TourRecordList[i].WriteData(bytes, 737, 14, 753, 959);
                        break;
                    case 3:
                        TourRecordList[i].WriteData(bytes, 738, 24, 754, 1059);
                        break;
                    case 4:
                        TourRecordList[i].WriteData(bytes, 739, 24, 755, 1159);
                        break;
                }
            }

            // Medals
            Medals.WriteData(bytes);
        }
    }
}

