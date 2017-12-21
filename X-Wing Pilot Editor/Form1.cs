using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace X_Wing_Pilot_Editor
{
    public partial class Form1 : Form
    {
        private string fileName = "";
        private byte[] bytes = null;
        private Pilot pilot;
        private decimal TotalSpaceVictories = 0;
        private decimal TotalSpaceCaptures = 0;

        public Form1()
        {
            InitializeComponent();

            pilot = new Pilot();
            bytes = new byte[1705];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0x00;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusHealth.DataSource = Enum.GetValues(typeof(Health));
            statusRank.DataSource = Enum.GetValues(typeof(Rank));
            cbKalidorCrescent.DataSource = Enum.GetValues(typeof(KalidorCrescentLevel));

            UpdateForm();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Pilot Files (*.plt)|*.plt";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                bytes = File.ReadAllBytes(fileName);

                if ((bytes.Length == 1705) || (bytes.Length == 3410))
                {
                    pilot.GetData(fileName, bytes);
                    UpdateForm();
                }
                else
                {
                    MessageBox.Show("The pilot file: " + fileName + " is not in the correct format for X-Wing 98.", "Wrong File Format");
                }
            }
            
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            pilot = new Pilot();
            bytes = new byte[1705];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0x00;
            }
            fileName = "";
            UpdateForm();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = fileName;
            saveFileDialog1.Filter = "Pilot Files (*.plt)|*.plt";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                pilot.WriteData(fileName, bytes);
                File.WriteAllBytes(fileName, bytes);
                pilot.PltName = Path.GetFileNameWithoutExtension(fileName);
            }
            UpdateForm();
        }

        private void UpdateForm()
        {
            if (fileName != "")
            {
                toolStripStatusPilotName.Text = pilot.PltRank + " " + pilot.PltName + " is " + pilot.PltHealth + " with " + pilot.PltTODScore + " Tour of Duty points";
            }
            else 
            {
                toolStripStatusPilotName.Text = "No Pilot File Loaded";
            }

            statusPilotName.Text = pilot.PltName;
            statusHealth.SelectedItem = pilot.PltHealth;
            statusRank.SelectedItem = pilot.PltRank;
            nudCurrentTour.Value = pilot.CurrentTour + 1;
            tbCurrentTourOpsComp.Text = pilot.CurrentTourOpsComp.ToString();

            upDownStatusTODPoints.Value = pilot.PltTODScore;
            upDownStatusExperience.Value = pilot.PltRookieNum;

            foreach (FlyableShipType playerShip in Enum.GetValues(typeof(FlyableShipType)))
            {
                int trainingCompleted = pilot.TrainingRecordList[(int)playerShip].TrainingCompleted;
                uint trainingScore = pilot.TrainingRecordList[(int)playerShip].TrainingScore;

                switch (playerShip)
                {
                    case FlyableShipType.XWing:
                        upDownTrainingLevelXWing.Value = trainingCompleted;
                        upDownTrainingBestScoreXWing.Value = trainingScore;
                        break;
                    case FlyableShipType.YWing:
                        upDownTrainingLevelYWing.Value = trainingCompleted;
                        upDownTrainingBestScoreYWing.Value = trainingScore;
                        break;
                    case FlyableShipType.AWing:
                        upDownTrainingLevelAWing.Value = trainingCompleted;
                        upDownTrainingBestScoreAWing.Value = trainingScore;
                        break;
                    case FlyableShipType.BWing:
                        upDownTrainingLevelBWing.Value = trainingCompleted;
                        upDownTrainingBestScoreBWing.Value = trainingScore;
                        break;
                }
            }

            foreach (HistoricMissionType missionType in Enum.GetValues(typeof(HistoricMissionType)))
            {
                switch (missionType)
                {
                    case HistoricMissionType.XWing:
                        XWingHistComplete1.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[0];
                        XWingHistComplete2.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[1];
                        XWingHistComplete3.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[2];
                        XWingHistComplete4.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[3];
                        XWingHistComplete5.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[4];
                        XWingHistComplete6.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[5];

                        XWingHistScore1.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[0];
                        XWingHistScore2.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[1];
                        XWingHistScore3.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[2];
                        XWingHistScore4.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[3];
                        XWingHistScore5.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[4];
                        XWingHistScore6.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[5];
                        break;
                    case HistoricMissionType.YWing:
                        YWingHistComplete1.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[0];
                        YWingHistComplete2.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[1];
                        YWingHistComplete3.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[2];
                        YWingHistComplete4.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[3];
                        YWingHistComplete5.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[4];
                        YWingHistComplete6.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[5];

                        YWingHistScore1.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[0];
                        YWingHistScore2.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[1];
                        YWingHistScore3.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[2];
                        YWingHistScore4.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[3];
                        YWingHistScore5.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[4];
                        YWingHistScore6.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[5];
                        break;
                    case HistoricMissionType.AWing:
                        AWingHistComplete1.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[0];
                        AWingHistComplete2.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[1];
                        AWingHistComplete3.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[2];
                        AWingHistComplete4.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[3];
                        AWingHistComplete5.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[4];
                        AWingHistComplete6.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[5];

                        AWingHistScore1.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[0];
                        AWingHistScore2.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[1];
                        AWingHistScore3.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[2];
                        AWingHistScore4.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[3];
                        AWingHistScore5.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[4];
                        AWingHistScore6.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[5];
                        break;
                    case HistoricMissionType.BWing:
                        BWingHistComplete1.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[0];
                        BWingHistComplete2.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[1];
                        BWingHistComplete3.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[2];
                        BWingHistComplete4.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[3];
                        BWingHistComplete5.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[4];
                        BWingHistComplete6.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[5];

                        BWingHistScore1.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[0];
                        BWingHistScore2.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[1];
                        BWingHistScore3.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[2];
                        BWingHistScore4.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[3];
                        BWingHistScore5.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[4];
                        BWingHistScore6.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[5];
                        break;
                    case HistoricMissionType.Bonus:
                        BonusHistComplete1.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[0];
                        BonusHistComplete2.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[1];
                        BonusHistComplete3.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[2];
                        BonusHistComplete4.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[3];
                        BonusHistComplete5.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[4];
                        BonusHistComplete6.Checked = pilot.HistoricCombatRecordList[(int)missionType].MissionComplete[5];

                        BonusHistScore1.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[0];
                        BonusHistScore2.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[1];
                        BonusHistScore3.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[2];
                        BonusHistScore4.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[3];
                        BonusHistScore5.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[4];
                        BonusHistScore6.Value = pilot.HistoricCombatRecordList[(int)missionType].MissionScore[5];
                        break;
                }

                TotalSpaceVictories = VicXWing.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.XWing].Victories;
                TotalSpaceVictories += VicYWing.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.YWing].Victories;
                TotalSpaceVictories += VicAWing.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.AWing].Victories;
                TotalSpaceVictories += VicTIEF.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEFighter].Victories;
                TotalSpaceVictories += VicTIEI.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEInterceptor].Victories;
                TotalSpaceVictories += VicTIEB.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEBomber].Victories;
                TotalSpaceVictories += VicGunB.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.GunBoat].Victories;
                TotalSpaceVictories += VicTran.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Transport].Victories;
                TotalSpaceVictories += VicShuttle.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Shuttle].Victories;
                TotalSpaceVictories += VicTug.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Tug].Victories;
                TotalSpaceVictories += VicCont.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Container].Victories;
                TotalSpaceVictories += VicFreight.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Freighter].Victories;
                TotalSpaceVictories += VicCalamari.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Calamari].Victories;
                TotalSpaceVictories += VicNebB.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.NebulonB].Victories;
                TotalSpaceVictories += VicCorvette.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Corvette].Victories;
                TotalSpaceVictories += VicStarD.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.StarDestroyer].Victories;
                TotalSpaceVictories += VicTIEA.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEAdvanced].Victories;
                TotalSpaceVictories += VicMine1.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine1].Victories;
                TotalSpaceVictories += VicMine2.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine2].Victories;
                TotalSpaceVictories += VicMine3.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine3].Victories;
                TotalSpaceVictories += VicMine4.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine4].Victories;
                TotalSpaceVictories += VicCommSat1.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat1].Victories;
                TotalSpaceVictories += VicCommSat2.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat2].Victories;
                TotalSpaceVictories += VicProbe.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.SpaceProbe].Victories;
                TODSpaceVictories.Text = TotalSpaceVictories.ToString();

                TotalSpaceCaptures = CapXWing.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.XWing].Captures;
                TotalSpaceCaptures += CapYWing.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.YWing].Captures;
                TotalSpaceCaptures += CapAWing.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.AWing].Captures;
                TotalSpaceCaptures += CapTIEF.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEFighter].Captures;
                TotalSpaceCaptures += CapTIEI.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEInterceptor].Captures;
                TotalSpaceCaptures += CapTIEB.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEBomber].Captures;
                TotalSpaceCaptures += CapGunB.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.GunBoat].Captures;
                TotalSpaceCaptures += CapTran.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Transport].Captures;
                TotalSpaceCaptures += CapShuttle.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Shuttle].Captures;
                TotalSpaceCaptures += CapTug.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Tug].Captures;
                TotalSpaceCaptures += CapCont.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Container].Captures;
                TotalSpaceCaptures += CapFreight.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Freighter].Captures;
                TotalSpaceCaptures += CapCalamari.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Calamari].Captures;
                TotalSpaceCaptures += CapNebB.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.NebulonB].Captures;
                TotalSpaceCaptures += CapCorvette.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Corvette].Captures;
                TotalSpaceCaptures += CapStarD.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.StarDestroyer].Captures;
                TotalSpaceCaptures += CapTIEA.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEAdvanced].Captures;
                TotalSpaceCaptures += CapMine1.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine1].Captures;
                TotalSpaceCaptures += CapMine2.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine2].Captures;
                TotalSpaceCaptures += CapMine3.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine3].Captures;
                TotalSpaceCaptures += CapMine4.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine4].Captures;
                TotalSpaceCaptures += CapCommSat1.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat1].Captures;
                TotalSpaceCaptures += CapCommSat2.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat2].Captures;
                TotalSpaceCaptures += CapProbe.Value = pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.SpaceProbe].Captures;
                TODSpaceCaptures.Text = TotalSpaceCaptures.ToString();

                SurfaceVictories.Value = pilot.TODStats.TotalSurfaceVictories;
                LasersFired.Value = pilot.TODStats.LasersFired;
                LaserCraftHits.Value = pilot.TODStats.LaserCraftHits;
                LaserGroundHits.Value = pilot.TODStats.LaserGroundHits;
                MisslesFired.Value = pilot.TODStats.MisslesFired;
                MissleCraftHits.Value = pilot.TODStats.MissleCraftHits;
                MissleGroundHits.Value = pilot.TODStats.MissleGroundHits;
                CraftLost.Value = pilot.TODStats.CraftLost;
                if (LasersFired.Value != 0)
                {
                    LaserCraftHitsPercent.Text = (int)((LaserCraftHits.Value / LasersFired.Value) * 100) + "%";
                    LaserGroundHitsPercent.Text = (int)((LaserGroundHits.Value / LasersFired.Value) * 100) + "%";
                }
                else
                {
                    LaserCraftHitsPercent.Text = "n/a";
                    LaserGroundHitsPercent.Text = "n/a";
                }

                if (MisslesFired.Value != 0)
                {
                    MissleCraftHitsPercent.Text = (int)((MissleCraftHits.Value / MisslesFired.Value) * 100) + "%";
                    MissleGroundHitsPercent.Text = (int)((MissleGroundHits.Value / MisslesFired.Value) * 100) + "%";
                }
                else
                {
                    MissleCraftHitsPercent.Text = "n/a";
                    MissleGroundHitsPercent.Text = "n/a";
                }
            }

            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        switch (pilot.TourRecordList[i].TourStatus)
                        {
                            case TourStatus.Inactive:
                                rbTOD1Inactive.Checked = true;
                                break;
                            case TourStatus.Active:
                                rbTOD1Active.Checked = true;
                                break;
                            case TourStatus.Incomplete:
                                rbTOD1Incomplete.Checked = true;
                                break;
                            case TourStatus.Complete:
                                rbTOD1Complete.Checked = true;
                                break;
                        }
                        nudTOD1OpsComplete.Value = (int)pilot.TourRecordList[i].OperationsComplete;
                        nudPointsTOD1Op1.Value = (uint)pilot.TourRecordList[i].OperationScore[0];
                        nudPointsTOD1Op2.Value = (uint)pilot.TourRecordList[i].OperationScore[1];
                        nudPointsTOD1Op3.Value = (uint)pilot.TourRecordList[i].OperationScore[2];
                        nudPointsTOD1Op4.Value = (uint)pilot.TourRecordList[i].OperationScore[3];
                        nudPointsTOD1Op5.Value = (uint)pilot.TourRecordList[i].OperationScore[4];
                        nudPointsTOD1Op6.Value = (uint)pilot.TourRecordList[i].OperationScore[5];
                        nudPointsTOD1Op7.Value = (uint)pilot.TourRecordList[i].OperationScore[6];
                        nudPointsTOD1Op8.Value = (uint)pilot.TourRecordList[i].OperationScore[7];
                        nudPointsTOD1Op9.Value = (uint)pilot.TourRecordList[i].OperationScore[8];
                        nudPointsTOD1Op10.Value = (uint)pilot.TourRecordList[i].OperationScore[9];
                        nudPointsTOD1Op11.Value = (uint)pilot.TourRecordList[i].OperationScore[10];
                        nudPointsTOD1Op12.Value = (uint)pilot.TourRecordList[i].OperationScore[11];
                        break;
                    case 1:
                        switch (pilot.TourRecordList[i].TourStatus)
                        {
                            case TourStatus.Inactive:
                                rbTOD2Inactive.Checked = true;
                                break;
                            case TourStatus.Active:
                                rbTOD2Active.Checked = true;
                                break;
                            case TourStatus.Incomplete:
                                rbTOD2Incomplete.Checked = true;
                                break;
                            case TourStatus.Complete:
                                rbTOD2Complete.Checked = true;
                                break;
                        }
                        nudTOD2OpsComplete.Value = (int)pilot.TourRecordList[i].OperationsComplete;
                        nudPointsTOD2Op1.Value = (uint)pilot.TourRecordList[i].OperationScore[0];
                        nudPointsTOD2Op2.Value = (uint)pilot.TourRecordList[i].OperationScore[1];
                        nudPointsTOD2Op3.Value = (uint)pilot.TourRecordList[i].OperationScore[2];
                        nudPointsTOD2Op4.Value = (uint)pilot.TourRecordList[i].OperationScore[3];
                        nudPointsTOD2Op5.Value = (uint)pilot.TourRecordList[i].OperationScore[4];
                        nudPointsTOD2Op6.Value = (uint)pilot.TourRecordList[i].OperationScore[5];
                        nudPointsTOD2Op7.Value = (uint)pilot.TourRecordList[i].OperationScore[6];
                        nudPointsTOD2Op8.Value = (uint)pilot.TourRecordList[i].OperationScore[7];
                        nudPointsTOD2Op9.Value = (uint)pilot.TourRecordList[i].OperationScore[8];
                        nudPointsTOD2Op10.Value = (uint)pilot.TourRecordList[i].OperationScore[9];
                        nudPointsTOD2Op11.Value = (uint)pilot.TourRecordList[i].OperationScore[10];
                        nudPointsTOD2Op12.Value = (uint)pilot.TourRecordList[i].OperationScore[11];
                        break;
                    case 2:
                        switch (pilot.TourRecordList[i].TourStatus)
                        {
                            case TourStatus.Inactive:
                                rbTOD3Inactive.Checked = true;
                                break;
                            case TourStatus.Active:
                                rbTOD3Active.Checked = true;
                                break;
                            case TourStatus.Incomplete:
                                rbTOD3Incomplete.Checked = true;
                                break;
                            case TourStatus.Complete:
                                rbTOD3Complete.Checked = true;
                                break;
                        }
                        nudTOD3OpsComplete.Value = (int)pilot.TourRecordList[i].OperationsComplete;
                        nudPointsTOD3Op1.Value = (uint)pilot.TourRecordList[i].OperationScore[0];
                        nudPointsTOD3Op2.Value = (uint)pilot.TourRecordList[i].OperationScore[1];
                        nudPointsTOD3Op3.Value = (uint)pilot.TourRecordList[i].OperationScore[2];
                        nudPointsTOD3Op4.Value = (uint)pilot.TourRecordList[i].OperationScore[3];
                        nudPointsTOD3Op5.Value = (uint)pilot.TourRecordList[i].OperationScore[4];
                        nudPointsTOD3Op6.Value = (uint)pilot.TourRecordList[i].OperationScore[5];
                        nudPointsTOD3Op7.Value = (uint)pilot.TourRecordList[i].OperationScore[6];
                        nudPointsTOD3Op8.Value = (uint)pilot.TourRecordList[i].OperationScore[7];
                        nudPointsTOD3Op9.Value = (uint)pilot.TourRecordList[i].OperationScore[8];
                        nudPointsTOD3Op10.Value = (uint)pilot.TourRecordList[i].OperationScore[9];
                        nudPointsTOD3Op11.Value = (uint)pilot.TourRecordList[i].OperationScore[10];
                        nudPointsTOD3Op12.Value = (uint)pilot.TourRecordList[i].OperationScore[11];
                        nudPointsTOD3Op13.Value = (uint)pilot.TourRecordList[i].OperationScore[12];
                        nudPointsTOD3Op14.Value = (uint)pilot.TourRecordList[i].OperationScore[13];
                        break;
                    case 3:
                        switch (pilot.TourRecordList[i].TourStatus)
                        {
                            case TourStatus.Inactive:
                                rbTOD4Inactive.Checked = true;
                                break;
                            case TourStatus.Active:
                                rbTOD4Active.Checked = true;
                                break;
                            case TourStatus.Incomplete:
                                rbTOD4Incomplete.Checked = true;
                                break;
                            case TourStatus.Complete:
                                rbTOD4Complete.Checked = true;
                                break;
                        }
                        nudTOD4OpsComplete.Value = (int)pilot.TourRecordList[i].OperationsComplete;
                        nudPointsTOD4Op1.Value = (uint)pilot.TourRecordList[i].OperationScore[0];
                        nudPointsTOD4Op2.Value = (uint)pilot.TourRecordList[i].OperationScore[1];
                        nudPointsTOD4Op3.Value = (uint)pilot.TourRecordList[i].OperationScore[2];
                        nudPointsTOD4Op4.Value = (uint)pilot.TourRecordList[i].OperationScore[3];
                        nudPointsTOD4Op5.Value = (uint)pilot.TourRecordList[i].OperationScore[4];
                        nudPointsTOD4Op6.Value = (uint)pilot.TourRecordList[i].OperationScore[5];
                        nudPointsTOD4Op7.Value = (uint)pilot.TourRecordList[i].OperationScore[6];
                        nudPointsTOD4Op8.Value = (uint)pilot.TourRecordList[i].OperationScore[7];
                        nudPointsTOD4Op9.Value = (uint)pilot.TourRecordList[i].OperationScore[8];
                        nudPointsTOD4Op10.Value = (uint)pilot.TourRecordList[i].OperationScore[9];
                        nudPointsTOD4Op11.Value = (uint)pilot.TourRecordList[i].OperationScore[10];
                        nudPointsTOD4Op12.Value = (uint)pilot.TourRecordList[i].OperationScore[11];
                        nudPointsTOD4Op13.Value = (uint)pilot.TourRecordList[i].OperationScore[12];
                        nudPointsTOD4Op14.Value = (uint)pilot.TourRecordList[i].OperationScore[13];
                        nudPointsTOD4Op15.Value = (uint)pilot.TourRecordList[i].OperationScore[14];
                        nudPointsTOD4Op16.Value = (uint)pilot.TourRecordList[i].OperationScore[15];
                        nudPointsTOD4Op17.Value = (uint)pilot.TourRecordList[i].OperationScore[16];
                        nudPointsTOD4Op18.Value = (uint)pilot.TourRecordList[i].OperationScore[17];
                        nudPointsTOD4Op19.Value = (uint)pilot.TourRecordList[i].OperationScore[18];
                        nudPointsTOD4Op20.Value = (uint)pilot.TourRecordList[i].OperationScore[19];
                        nudPointsTOD4Op21.Value = (uint)pilot.TourRecordList[i].OperationScore[20];
                        nudPointsTOD4Op22.Value = (uint)pilot.TourRecordList[i].OperationScore[21];
                        nudPointsTOD4Op23.Value = (uint)pilot.TourRecordList[i].OperationScore[22];
                        nudPointsTOD4Op24.Value = (uint)pilot.TourRecordList[i].OperationScore[23];
                        break;
                    case 4:
                        switch (pilot.TourRecordList[i].TourStatus)
                        {
                            case TourStatus.Inactive:
                                rbTOD5Inactive.Checked = true;
                                break;
                            case TourStatus.Active:
                                rbTOD5Active.Checked = true;
                                break;
                            case TourStatus.Incomplete:
                                rbTOD5Incomplete.Checked = true;
                                break;
                            case TourStatus.Complete:
                                rbTOD5Complete.Checked = true;
                                break;
                        }
                        nudTOD5OpsComplete.Value = (int)pilot.TourRecordList[i].OperationsComplete;
                        nudPointsTOD5Op1.Value = (uint)pilot.TourRecordList[i].OperationScore[0];
                        nudPointsTOD5Op2.Value = (uint)pilot.TourRecordList[i].OperationScore[1];
                        nudPointsTOD5Op3.Value = (uint)pilot.TourRecordList[i].OperationScore[2];
                        nudPointsTOD5Op4.Value = (uint)pilot.TourRecordList[i].OperationScore[3];
                        nudPointsTOD5Op5.Value = (uint)pilot.TourRecordList[i].OperationScore[4];
                        nudPointsTOD5Op6.Value = (uint)pilot.TourRecordList[i].OperationScore[5];
                        nudPointsTOD5Op7.Value = (uint)pilot.TourRecordList[i].OperationScore[6];
                        nudPointsTOD5Op8.Value = (uint)pilot.TourRecordList[i].OperationScore[7];
                        nudPointsTOD5Op9.Value = (uint)pilot.TourRecordList[i].OperationScore[8];
                        nudPointsTOD5Op10.Value = (uint)pilot.TourRecordList[i].OperationScore[9];
                        nudPointsTOD5Op11.Value = (uint)pilot.TourRecordList[i].OperationScore[10];
                        nudPointsTOD5Op12.Value = (uint)pilot.TourRecordList[i].OperationScore[11];
                        nudPointsTOD5Op13.Value = (uint)pilot.TourRecordList[i].OperationScore[12];
                        nudPointsTOD5Op14.Value = (uint)pilot.TourRecordList[i].OperationScore[13];
                        nudPointsTOD5Op15.Value = (uint)pilot.TourRecordList[i].OperationScore[14];
                        nudPointsTOD5Op16.Value = (uint)pilot.TourRecordList[i].OperationScore[15];
                        nudPointsTOD5Op17.Value = (uint)pilot.TourRecordList[i].OperationScore[16];
                        nudPointsTOD5Op18.Value = (uint)pilot.TourRecordList[i].OperationScore[17];
                        nudPointsTOD5Op19.Value = (uint)pilot.TourRecordList[i].OperationScore[18];
                        nudPointsTOD5Op20.Value = (uint)pilot.TourRecordList[i].OperationScore[19];
                        nudPointsTOD5Op21.Value = (uint)pilot.TourRecordList[i].OperationScore[20];
                        nudPointsTOD5Op22.Value = (uint)pilot.TourRecordList[i].OperationScore[21];
                        nudPointsTOD5Op23.Value = (uint)pilot.TourRecordList[i].OperationScore[22];
                        nudPointsTOD5Op24.Value = (uint)pilot.TourRecordList[i].OperationScore[23];
                        break;
                }
            }

            checkCorellianCross.Checked = pilot.Medals.CorellianCross;
            checkMantooineMedallion.Checked = pilot.Medals.MantooineMedallion;
            checkStarOfAlderaan.Checked = pilot.Medals.StarOfAlderaan;
            checkShieldOfYavin.Checked = pilot.Medals.ShieldOfYavin;
            checkTalonsOfHoth.Checked = pilot.Medals.TalonsOfHoth;

            cbKalidorCrescent.SelectedItem = pilot.Medals.KalidorCrescent;
        }

        // Form Control Changed Methods

        // Pilot Status
        private void statusHealth_SelectedIndexChanged(object sender, EventArgs e)
        {
            pilot.PltHealth = (Health)statusHealth.SelectedItem;
            UpdateForm();
        }

        private void statusRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            pilot.PltRank = (Rank)statusRank.SelectedItem;
            UpdateForm();
        }

        private void upDownStatusTODPoints_ValueChanged(object sender, EventArgs e)
        {
            pilot.PltTODScore = (uint)upDownStatusTODPoints.Value;
            UpdateForm();
        }

        private void upDownStatusExperience_ValueChanged(object sender, EventArgs e)
        {
            pilot.PltRookieNum = (uint)upDownStatusExperience.Value;
        }

        // Pilot Status - Auto select on gaining focus
        private void upDownStatusTODPoints_Enter(object sender, EventArgs e)
        {
            upDownStatusTODPoints.Select(0, upDownStatusTODPoints.Text.Length);
        }

        private void upDownStatusExperience_Enter(object sender, EventArgs e)
        {
            upDownStatusExperience.Select(0, upDownStatusExperience.Text.Length);
        }

        // Completed Training Levels
        private void upDownTrainingLevelXWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[0].TrainingCompleted = (int)upDownTrainingLevelXWing.Value;
        }

        private void upDownTrainingLevelYWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[1].TrainingCompleted = (int)upDownTrainingLevelYWing.Value;
        }

        private void upDownTrainingLevelAWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[2].TrainingCompleted = (int)upDownTrainingLevelAWing.Value;
        }

        private void upDownTrainingLevelBWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[3].TrainingCompleted = (int)upDownTrainingLevelBWing.Value;
        }

        // Training Best Scores
        private void upDownTrainingBestScoreXWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[0].TrainingScore = (uint)upDownTrainingBestScoreXWing.Value;
        }

        private void upDownTrainingBestScoreYWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[1].TrainingScore = (uint)upDownTrainingBestScoreYWing.Value;
        }

        private void upDownTrainingBestScoreAWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[2].TrainingScore = (uint)upDownTrainingBestScoreAWing.Value;
        }

        private void upDownTrainingBestScoreBWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TrainingRecordList[3].TrainingScore = (uint)upDownTrainingBestScoreBWing.Value;
        }

        // Training - Auto select on gaining focus 
        private void upDownTrainingLevelXWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingLevelXWing.Select(0, upDownTrainingLevelXWing.Text.Length);
        }

        private void upDownTrainingLevelYWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingLevelYWing.Select(0, upDownTrainingLevelYWing.Text.Length);
        }

        private void upDownTrainingLevelAWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingLevelAWing.Select(0, upDownTrainingLevelAWing.Text.Length);
        }

        private void upDownTrainingLevelBWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingLevelBWing.Select(0, upDownTrainingLevelBWing.Text.Length);
        }

        private void upDownTrainingBestScoreXWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingBestScoreXWing.Select(0, upDownTrainingBestScoreXWing.Text.Length);
        }

        private void upDownTrainingBestScoreYWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingBestScoreYWing.Select(0, upDownTrainingBestScoreYWing.Text.Length);
        }

        private void upDownTrainingBestScoreAWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingBestScoreAWing.Select(0, upDownTrainingBestScoreAWing.Text.Length);
        }

        private void upDownTrainingBestScoreBWing_Enter(object sender, EventArgs e)
        {
            upDownTrainingBestScoreBWing.Select(0, upDownTrainingBestScoreBWing.Text.Length);
        }

        // Historic Missions - X-Wing - Scores
        private void XWingHistScore1_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionScore[0] = (uint)XWingHistScore1.Value;
        }

        private void XWingHistScore2_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionScore[1] = (uint)XWingHistScore2.Value;
        }

        private void XWingHistScore3_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionScore[2] = (uint)XWingHistScore3.Value;
        }

        private void XWingHistScore4_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionScore[3] = (uint)XWingHistScore4.Value;
        }

        private void XWingHistScore5_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionScore[4] = (uint)XWingHistScore5.Value;
        }

        private void XWingHistScore6_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionScore[5] = (uint)XWingHistScore6.Value;
        }

        // Historic Missions - X-Wing - Auto Select
        private void XWingHistScore1_Enter(object sender, EventArgs e)
        {
            XWingHistScore1.Select(0, XWingHistScore1.Text.Length);
        }

        private void XWingHistScore2_Enter(object sender, EventArgs e)
        {
            XWingHistScore2.Select(0, XWingHistScore2.Text.Length);
        }

        private void XWingHistScore3_Enter(object sender, EventArgs e)
        {
            XWingHistScore3.Select(0, XWingHistScore3.Text.Length);
        }

        private void XWingHistScore4_Enter(object sender, EventArgs e)
        {
            XWingHistScore4.Select(0, XWingHistScore4.Text.Length);
        }

        private void XWingHistScore5_Enter(object sender, EventArgs e)
        {
            XWingHistScore5.Select(0, XWingHistScore5.Text.Length);
        }

        private void XWingHistScore6_Enter(object sender, EventArgs e)
        {
            XWingHistScore6.Select(0, XWingHistScore6.Text.Length);
        }

        // Historic Missions - X-Wing - Mission Completed
        private void XWingHistComplete1_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionComplete[0] = XWingHistComplete1.Checked;
        }

        private void XWingHistComplete2_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionComplete[1] = XWingHistComplete2.Checked;
        }

        private void XWingHistComplete3_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionComplete[2] = XWingHistComplete3.Checked;
        }

        private void XWingHistComplete4_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionComplete[3] = XWingHistComplete4.Checked;
        }

        private void XWingHistComplete5_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionComplete[4] = XWingHistComplete5.Checked;
        }

        private void XWingHistComplete6_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[0].MissionComplete[5] = XWingHistComplete6.Checked;
        }

        // Historic Missions - Y-Wing - Scores
        private void YWingHistScore1_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionScore[0] = (uint)YWingHistScore1.Value;
        }

        private void YWingHistScore2_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionScore[1] = (uint)YWingHistScore2.Value;
        }

        private void YWingHistScore3_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionScore[2] = (uint)YWingHistScore3.Value;
        }

        private void YWingHistScore4_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionScore[3] = (uint)YWingHistScore4.Value;
        }

        private void YWingHistScore5_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionScore[4] = (uint)YWingHistScore5.Value;
        }

        private void YWingHistScore6_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionScore[5] = (uint)YWingHistScore6.Value;
        }

        // Historic Missions - Y-Wing - Auto Select
        private void YWingHistScore1_Enter(object sender, EventArgs e)
        {
            YWingHistScore1.Select(0, YWingHistScore1.Text.Length);
        }

        private void YWingHistScore2_Enter(object sender, EventArgs e)
        {
            YWingHistScore2.Select(0, YWingHistScore2.Text.Length);
        }

        private void YWingHistScore3_Enter(object sender, EventArgs e)
        {
            YWingHistScore3.Select(0, YWingHistScore3.Text.Length);
        }

        private void YWingHistScore4_Enter(object sender, EventArgs e)
        {
            YWingHistScore4.Select(0, YWingHistScore4.Text.Length);
        }

        private void YWingHistScore5_Enter(object sender, EventArgs e)
        {
            YWingHistScore5.Select(0, YWingHistScore5.Text.Length);
        }

        private void YWingHistScore6_Enter(object sender, EventArgs e)
        {
            YWingHistScore6.Select(0, YWingHistScore6.Text.Length);
        }

        // Historic Missions - Y-Wing - Completed
        private void YWingHistComplete1_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionComplete[0] = YWingHistComplete1.Checked;
        }

        private void YWingHistComplete2_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionComplete[1] = YWingHistComplete2.Checked;
        }

        private void YWingHistComplete3_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionComplete[2] = YWingHistComplete3.Checked;
        }

        private void YWingHistComplete4_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionComplete[3] = YWingHistComplete4.Checked;
        }

        private void YWingHistComplete5_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[1].MissionComplete[4] = YWingHistComplete5.Checked;
        }

        private void YWingHistComplete6_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionComplete[5] = YWingHistComplete6.Checked;
        }

        // Historic Missions - A-Wing - Scores
        private void AWingHistScore1_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionScore[0] = (uint)AWingHistScore1.Value;
        }

        private void AWingHistScore2_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionScore[1] = (uint)AWingHistScore2.Value;
        }

        private void AWingHistScore3_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionScore[2] = (uint)AWingHistScore3.Value;
        }

        private void AWingHistScore4_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionScore[3] = (uint)AWingHistScore4.Value;
        }

        private void AWingHistScore5_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionScore[4] = (uint)AWingHistScore5.Value;
        }

        private void AWingHistScore6_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionScore[5] = (uint)AWingHistScore6.Value;
        }

        // Historic Missions - A-Wing - Mission Completed
        private void AWingHistComplete1_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionComplete[0] = AWingHistComplete1.Checked;
        }

        private void AWingHistComplete2_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionComplete[1] = AWingHistComplete2.Checked;
        }

        private void AWingHistComplete3_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionComplete[2] = AWingHistComplete3.Checked;
        }

        private void AWingHistComplete4_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionComplete[3] = AWingHistComplete4.Checked;
        }

        private void AWingHistComplete5_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionComplete[4] = AWingHistComplete5.Checked;
        }

        private void AWingHistComplete6_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[2].MissionComplete[5] = AWingHistComplete6.Checked;
        }

        // Historic Missions - A-Wing - Auto Select
        private void AWingHistScore1_Enter(object sender, EventArgs e)
        {
            AWingHistScore1.Select(0, AWingHistScore1.Text.Length);
        }

        private void AWingHistScore2_Enter(object sender, EventArgs e)
        {
            AWingHistScore2.Select(0, AWingHistScore2.Text.Length);
        }

        private void AWingHistScore3_Enter(object sender, EventArgs e)
        {
            AWingHistScore3.Select(0, AWingHistScore3.Text.Length);
        }

        private void AWingHistScore4_Enter(object sender, EventArgs e)
        {
            AWingHistScore4.Select(0, AWingHistScore4.Text.Length);
        }

        private void AWingHistScore5_Enter(object sender, EventArgs e)
        {
            AWingHistScore5.Select(0, AWingHistScore5.Text.Length);
        }

        private void AWingHistScore6_Enter(object sender, EventArgs e)
        {
            AWingHistScore6.Select(0, AWingHistScore6.Text.Length);
        }

        // Historic Missions - B-Wing - Scores
        private void BWingHistScore1_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionScore[0] = (uint)BWingHistScore1.Value;
        }

        private void BWingHistScore2_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionScore[1] = (uint)BWingHistScore2.Value;
        }

        private void BWingHistScore3_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionScore[2] = (uint)BWingHistScore3.Value;
        }

        private void BWingHistScore4_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionScore[3] = (uint)BWingHistScore4.Value;
        }

        private void BWingHistScore5_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionScore[4] = (uint)BWingHistScore5.Value;
        }

        private void BWingHistScore6_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionScore[5] = (uint)BWingHistScore6.Value;
        }

        // Historic Missions - B-Wing - Mission Completed
        private void BWingHistComplete1_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionComplete[0] = BWingHistComplete1.Checked;
        }

        private void BWingHistComplete2_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionComplete[1] = BWingHistComplete2.Checked;
        }

        private void BWingHistComplete3_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionComplete[2] = BWingHistComplete3.Checked;
        }

        private void BWingHistComplete4_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionComplete[3] = BWingHistComplete4.Checked;
        }

        private void BWingHistComplete5_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionComplete[4] = BWingHistComplete5.Checked;
        }

        private void BWingHistComplete6_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[3].MissionComplete[5] = BWingHistComplete6.Checked;
        }

        // Historic Missions - B-Wing - Auto Select
        private void BWingHistScore1_Enter(object sender, EventArgs e)
        {
            BWingHistScore1.Select(0, BWingHistScore1.Text.Length);
        }

        private void BWingHistScore2_Enter(object sender, EventArgs e)
        {
            BWingHistScore2.Select(0, BWingHistScore2.Text.Length);
        }

        private void BWingHistScore3_Enter(object sender, EventArgs e)
        {
            BWingHistScore3.Select(0, BWingHistScore3.Text.Length);
        }

        private void BWingHistScore4_Enter(object sender, EventArgs e)
        {
            BWingHistScore4.Select(0, BWingHistScore4.Text.Length);
        }

        private void BWingHistScore5_Enter(object sender, EventArgs e)
        {
            BWingHistScore5.Select(0, BWingHistScore5.Text.Length);
        }

        private void BWingHistScore6_Enter(object sender, EventArgs e)
        {
            BWingHistScore6.Select(0, BWingHistScore6.Text.Length);
        }

        // Historic Missions - Bonus Missions - Score
        private void BonusHistScore1_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionScore[0] = (uint)BonusHistScore1.Value;
        }

        private void BonusHistScore2_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionScore[1] = (uint)BonusHistScore2.Value;
        }

        private void BonusHistScore3_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionScore[2] = (uint)BonusHistScore3.Value;
        }

        private void BonusHistScore4_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionScore[3] = (uint)BonusHistScore4.Value;
        }

        private void BonusHistScore5_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionScore[4] = (uint)BonusHistScore5.Value;
        }

        private void BonusHistScore6_ValueChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionScore[5] = (uint)BonusHistScore6.Value;
        }

        // Historic Missions - Bonus Missions - Mission Completed
        private void BonusHistComplete1_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionComplete[0] = BonusHistComplete1.Checked;
        }

        private void BonusHistComplete2_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionComplete[1] = BonusHistComplete2.Checked;
        }

        private void BonusHistComplete3_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionComplete[2] = BonusHistComplete3.Checked;
        }

        private void BonusHistComplete4_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionComplete[3] = BonusHistComplete4.Checked;
        }

        private void BonusHistComplete5_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionComplete[4] = BonusHistComplete5.Checked;
        }

        private void BonusHistComplete6_CheckedChanged(object sender, EventArgs e)
        {
            pilot.HistoricCombatRecordList[4].MissionComplete[5] = BonusHistComplete6.Checked;
        }

        // Historic Missions - Bonus Missions - Auto Select
        private void BonusHistScore1_Enter(object sender, EventArgs e)
        {
            BonusHistScore1.Select(0, BonusHistScore1.Text.Length);
        }

        private void BonusHistScore2_Enter(object sender, EventArgs e)
        {
            BonusHistScore2.Select(0, BonusHistScore2.Text.Length);
        }

        private void BonusHistScore3_Enter(object sender, EventArgs e)
        {
            BonusHistScore3.Select(0, BonusHistScore3.Text.Length);
        }

        private void BonusHistScore4_Enter(object sender, EventArgs e)
        {
            BonusHistScore4.Select(0, BonusHistScore4.Text.Length);
        }

        private void BonusHistScore5_Enter(object sender, EventArgs e)
        {
            BonusHistScore5.Select(0, BonusHistScore5.Text.Length);
        }

        private void BonusHistScore6_Enter(object sender, EventArgs e)
        {
            BonusHistScore6.Select(0, BonusHistScore6.Text.Length);
        }

        //Tour Stats - Page 1
        private void VicXWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.XWing].Victories = (ushort)VicXWing.Value;
            UpdateForm();
        }

        private void CapXWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.XWing].Captures = (ushort)CapXWing.Value;
            UpdateForm();
        }

        private void VicYWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.YWing].Victories = (ushort)VicYWing.Value;
            UpdateForm();
        }

        private void CapYWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.YWing].Captures = (ushort)CapYWing.Value;
            UpdateForm();
        }

        private void VicAWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.AWing].Victories = (ushort)VicAWing.Value;
            UpdateForm();
        }

        private void CapAWing_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.AWing].Captures = (ushort)CapAWing.Value;
            UpdateForm();
        }

        private void VicTIEF_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEFighter].Victories = (ushort)VicTIEF.Value;
            UpdateForm();
        }

        private void CapTIEF_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEFighter].Captures = (ushort)CapTIEF.Value;
            UpdateForm();
        }

        private void VicTIEI_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEInterceptor].Victories = (ushort)VicTIEI.Value;
            UpdateForm();
        }

        private void CapTIEI_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEInterceptor].Captures = (ushort)CapTIEI.Value;
            UpdateForm();
        }

        private void VicTIEB_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEBomber].Victories = (ushort)VicTIEB.Value;
            UpdateForm();
        }

        private void CapTIEB_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEBomber].Captures = (ushort)CapTIEB.Value;
            UpdateForm();
        }

        private void VicGunB_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.GunBoat].Victories = (ushort)VicGunB.Value;
            UpdateForm();
        }

        private void CapGunB_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.GunBoat].Captures = (ushort)CapGunB.Value;
            UpdateForm();
        }

        private void VicTran_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Transport].Victories = (ushort)VicTran.Value;
            UpdateForm();
        }

        private void CapTran_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Transport].Captures = (ushort)CapTran.Value;
            UpdateForm();
        }

        private void VicShuttle_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Shuttle].Victories = (ushort)VicShuttle.Value;
            UpdateForm();
        }

        private void CapShuttle_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Shuttle].Captures = (ushort)CapShuttle.Value;
            UpdateForm();
        }

        private void VicTug_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Tug].Victories = (ushort)VicTug.Value;
            UpdateForm();
        }

        private void CapTug_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Tug].Captures = (ushort)CapTug.Value;
            UpdateForm();
        }

        private void VicCont_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Container].Victories = (ushort)VicCont.Value;
            UpdateForm();
        }

        private void CapCont_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Container].Captures = (ushort)CapCont.Value;
            UpdateForm();
        }

        private void VicFreight_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Freighter].Victories = (ushort)VicFreight.Value;
            UpdateForm();
        }

        private void CapFreight_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Freighter].Captures = (ushort)CapFreight.Value;
            UpdateForm();
        }

        private void VicCalamari_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Calamari].Victories = (ushort)VicCalamari.Value;
            UpdateForm();
        }

        private void CapCalamari_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Calamari].Captures = (ushort)CapCalamari.Value;
            UpdateForm();
        }

        private void VicNebB_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.NebulonB].Victories = (ushort)VicNebB.Value;
            UpdateForm();
        }

        private void CapNebB_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.NebulonB].Captures = (ushort)CapNebB.Value;
            UpdateForm();
        }

        private void VicCorvette_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Corvette].Victories = (ushort)VicCorvette.Value;
            UpdateForm();
        }

        private void CapCorvette_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Corvette].Captures = (ushort)CapCorvette.Value;
            UpdateForm();
        }

        private void VicStarD_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.StarDestroyer].Victories = (ushort)VicStarD.Value;
            UpdateForm();
        }

        private void CapStarD_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.StarDestroyer].Captures = (ushort)CapStarD.Value;
            UpdateForm();
        }

        private void VicTIEA_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEAdvanced].Victories = (ushort)VicTIEA.Value;
            UpdateForm();
        }

        private void CapTIEA_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.TIEAdvanced].Captures = (ushort)CapTIEA.Value;
            UpdateForm();
        }

        private void VicMine1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine1].Victories = (ushort)VicMine1.Value;
            UpdateForm();
        }

        private void CapMine1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine1].Captures = (ushort)CapMine1.Value;
            UpdateForm();
        }

        private void VicMine2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine2].Victories = (ushort)VicMine2.Value;
            UpdateForm();
        }

        private void CapMine2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine2].Captures = (ushort)CapMine2.Value;
            UpdateForm();
        }

        private void VicMine3_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine3].Victories = (ushort)VicMine3.Value;
            UpdateForm();
        }

        private void CapMine3_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine3].Captures = (ushort)CapMine3.Value;
            UpdateForm();
        }

        private void VicMine4_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine4].Victories = (ushort)VicMine4.Value;
            UpdateForm();
        }

        private void CapMine4_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.Mine4].Captures = (ushort)CapMine4.Value;
            UpdateForm();
        }

        private void VicCommSat1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat1].Victories = (ushort)VicCommSat1.Value;
            UpdateForm();
        }

        private void CapCommSat1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat1].Captures = (ushort)CapCommSat1.Value;
            UpdateForm();
        }

        private void VicCommSat2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat2].Victories = (ushort)VicCommSat2.Value;
            UpdateForm();
        }

        private void CapCommSat2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.CommSat2].Captures = (ushort)CapCommSat2.Value;
            UpdateForm();
        }

        private void VicProbe_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.SpaceProbe].Victories = (ushort)VicProbe.Value;
            UpdateForm();
        }

        private void CapProbe_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TODVictoriesCapturesList[(int)NPCShipType.SpaceProbe].Captures = (ushort)CapProbe.Value;
            UpdateForm();
        }

        // Tour Stats - Page 2
        private void SurfaceVictories_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.TotalSurfaceVictories = (ushort)SurfaceVictories.Value;
        }

        private void LasersFired_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.LasersFired = (uint)LasersFired.Value;
            UpdateForm();
        }

        private void LaserCraftHits_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.LaserCraftHits = (uint)LaserCraftHits.Value;
            UpdateForm();
        }

        private void LaserGroundHits_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.LaserGroundHits = (uint)LaserGroundHits.Value;
            UpdateForm();
        }

        private void MisslesFired_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.MisslesFired = (ushort)MisslesFired.Value;
            UpdateForm();
        }

        private void MissleCraftHits_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.MissleCraftHits = (ushort)MissleCraftHits.Value;
            UpdateForm();
        }

        private void MissleGroundHits_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.MissleGroundHits = (ushort)MissleGroundHits.Value;
            UpdateForm();
        }

        private void CraftLost_ValueChanged(object sender, EventArgs e)
        {
            pilot.TODStats.CraftLost = (ushort)CraftLost.Value;
        }

        //TOD 1 Status / Complete
        private void rbTOD1Inactive_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[0].TourStatus = TourStatus.Inactive;
        }

        private void rbTOD1Active_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                pilot.TourRecordList[0].TourStatus = TourStatus.Active;
                pilot.CurrentTour = 0;
            }
        }

        private void rbTOD1Incomplete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[0].TourStatus = TourStatus.Incomplete;
        }

        private void rbTOD1Complete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[0].TourStatus = TourStatus.Complete;
        }

        private void nudTOD1OpsComplete_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationsComplete = (ushort)nudTOD1OpsComplete.Value;

            if (pilot.TourRecordList[0].TourStatus == TourStatus.Active)
                pilot.CurrentTourOpsComp = (uint)nudTOD1OpsComplete.Value;
            UpdateForm();
        }

        //TOD 2 Status / Complete
        private void rbTOD2Inactive_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[1].TourStatus = TourStatus.Inactive;
        }

        private void rbTOD2Active_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                pilot.TourRecordList[1].TourStatus = TourStatus.Active;
                pilot.CurrentTour = 1;
            }
        }

        private void rbTOD2Incomplete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[1].TourStatus = TourStatus.Incomplete;
        }

        private void rbTOD2Complete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[1].TourStatus = TourStatus.Complete;
        }

        private void nudTOD2OpsComplete_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationsComplete = (ushort)nudTOD2OpsComplete.Value;

            if (pilot.TourRecordList[1].TourStatus == TourStatus.Active)
                pilot.CurrentTourOpsComp = (uint)nudTOD2OpsComplete.Value;
            UpdateForm();
        }

        //TOD 3 Status / Complete
        private void rbTOD3Inactive_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[2].TourStatus = TourStatus.Inactive;
        }

        private void rbTOD3Active_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                pilot.TourRecordList[2].TourStatus = TourStatus.Active;
                pilot.CurrentTour = 2;
            }
        }

        private void rbTOD3Incomplete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[2].TourStatus = TourStatus.Incomplete;
        }

        private void rbTOD3Complete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[2].TourStatus = TourStatus.Complete;
        }

        private void nudTOD3OpsComplete_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationsComplete = (ushort)nudTOD3OpsComplete.Value;

            if (pilot.TourRecordList[2].TourStatus == TourStatus.Active)
                pilot.CurrentTourOpsComp = (uint)nudTOD3OpsComplete.Value;
            UpdateForm();
        }

        //TOD 4 Status / Complete
        private void rbTOD4Inactive_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[3].TourStatus = TourStatus.Inactive;
        }

        private void rbTOD4Active_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                pilot.TourRecordList[3].TourStatus = TourStatus.Active;
                pilot.CurrentTour = 3;
            }
        }

        private void rbTOD4Incomplete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[3].TourStatus = TourStatus.Incomplete;
        }

        private void rbTOD4Complete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[3].TourStatus = TourStatus.Complete;
        }

        private void nudTOD4OpsComplete_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationsComplete = (ushort)nudTOD4OpsComplete.Value;

            if (pilot.TourRecordList[3].TourStatus == TourStatus.Active)
                pilot.CurrentTourOpsComp = (uint)nudTOD4OpsComplete.Value;
            UpdateForm();
        }

        //TOD 5 Status / Complete
        private void rbTOD5Inactive_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[4].TourStatus = TourStatus.Inactive;
        }

        private void rbTOD5Active_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                pilot.TourRecordList[4].TourStatus = TourStatus.Active;
                pilot.CurrentTour = 4;
            }
        }

        private void rbTOD5Incomplete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[4].TourStatus = TourStatus.Incomplete;
        }

        private void rbTOD5Complete_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                pilot.TourRecordList[4].TourStatus = TourStatus.Complete;
        }

        private void nudTOD5OpsComplete_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationsComplete = (ushort)nudTOD5OpsComplete.Value;

            if (pilot.TourRecordList[4].TourStatus == TourStatus.Active)
                pilot.CurrentTourOpsComp = (uint)nudTOD5OpsComplete.Value;
            UpdateForm();
        }

        //TOD 1 Ops Scores
        private void nudPointsTOD1Op1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[0] = (uint)nudPointsTOD1Op1.Value;
        }

        private void nudPointsTOD1Op2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[1] = (uint)nudPointsTOD1Op2.Value;
        }

        private void nudPointsTOD1Op3_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[2] = (uint)nudPointsTOD1Op3.Value;
        }

        private void nudPointsTOD1Op4_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[3] = (uint)nudPointsTOD1Op4.Value;
        }

        private void nudPointsTOD1Op5_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[4] = (uint)nudPointsTOD1Op5.Value;
        }

        private void nudPointsTOD1Op6_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[5] = (uint)nudPointsTOD1Op6.Value;
        }

        private void nudPointsTOD1Op7_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[6] = (uint)nudPointsTOD1Op7.Value;
        }

        private void nudPointsTOD1Op8_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[7] = (uint)nudPointsTOD1Op8.Value;
        }

        private void nudPointsTOD1Op9_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[8] = (uint)nudPointsTOD1Op9.Value;
        }

        private void nudPointsTOD1Op10_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[9] = (uint)nudPointsTOD1Op10.Value;
        }

        private void nudPointsTOD1Op11_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[10] = (uint)nudPointsTOD1Op11.Value;
        }

        private void nudPointsTOD1Op12_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[0].OperationScore[11] = (uint)nudPointsTOD1Op12.Value;
        }

        //TOD 2 Ops Scores
        private void nudPointsTOD2Op1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[0] = (uint)nudPointsTOD2Op1.Value;
        }

        private void nudPointsTOD2Op2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[1] = (uint)nudPointsTOD2Op2.Value;
        }

        private void nudPointsTOD2Op3_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[2] = (uint)nudPointsTOD2Op3.Value;
        }

        private void nudPointsTOD2Op4_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[3] = (uint)nudPointsTOD2Op4.Value;
        }

        private void nudPointsTOD2Op5_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[4] = (uint)nudPointsTOD2Op5.Value;
        }

        private void nudPointsTOD2Op6_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[5] = (uint)nudPointsTOD2Op6.Value;
        }

        private void nudPointsTOD2Op7_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[6] = (uint)nudPointsTOD2Op7.Value;
        }

        private void nudPointsTOD2Op8_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[7] = (uint)nudPointsTOD2Op8.Value;
        }

        private void nudPointsTOD2Op9_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[8] = (uint)nudPointsTOD2Op9.Value;
        }

        private void nudPointsTOD2Op10_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[9] = (uint)nudPointsTOD2Op10.Value;
        }

        private void nudPointsTOD2Op11_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[10] = (uint)nudPointsTOD2Op11.Value;
        }

        private void nudPointsTOD2Op12_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[1].OperationScore[11] = (uint)nudPointsTOD2Op12.Value;
        }

        //TOD 3 Ops Scores
        private void nudPointsTOD3Op1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[0] = (uint)nudPointsTOD3Op1.Value;
        }

        private void nudPointsTOD3Op2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[1] = (uint)nudPointsTOD3Op2.Value;
        }

        private void nudPointsTOD3Op3_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[2] = (uint)nudPointsTOD3Op3.Value;
        }

        private void nudPointsTOD3Op4_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[3] = (uint)nudPointsTOD3Op4.Value;
        }

        private void nudPointsTOD3Op5_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[4] = (uint)nudPointsTOD3Op5.Value;
        }

        private void nudPointsTOD3Op6_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[5] = (uint)nudPointsTOD3Op6.Value;
        }

        private void nudPointsTOD3Op7_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[6] = (uint)nudPointsTOD3Op7.Value;
        }

        private void nudPointsTOD3Op8_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[7] = (uint)nudPointsTOD3Op8.Value;
        }

        private void nudPointsTOD3Op9_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[8] = (uint)nudPointsTOD3Op9.Value;
        }

        private void nudPointsTOD3Op10_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[9] = (uint)nudPointsTOD3Op10.Value;
        }

        private void nudPointsTOD3Op11_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[10] = (uint)nudPointsTOD3Op11.Value;
        }

        private void nudPointsTOD3Op12_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[11] = (uint)nudPointsTOD3Op12.Value;
        }

        private void nudPointsTOD3Op13_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[12] = (uint)nudPointsTOD3Op13.Value;
        }

        private void nudPointsTOD3Op14_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[2].OperationScore[13] = (uint)nudPointsTOD3Op14.Value;
        }

        //TOD 4 Ops Scores
        private void nudPointsTOD4Op1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[0] = (uint)nudPointsTOD4Op1.Value;
        }

        private void nudPointsTOD4Op2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[1] = (uint)nudPointsTOD4Op2.Value;
        }

        private void nudPointsTOD4Op3_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[2] = (uint)nudPointsTOD4Op3.Value;
        }

        private void nudPointsTOD4Op4_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[3] = (uint)nudPointsTOD4Op4.Value;
        }

        private void nudPointsTOD4Op5_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[4] = (uint)nudPointsTOD4Op5.Value;
        }

        private void nudPointsTOD4Op6_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[5] = (uint)nudPointsTOD4Op6.Value;
        }

        private void nudPointsTOD4Op7_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[6] = (uint)nudPointsTOD4Op7.Value;
        }

        private void nudPointsTOD4Op8_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[7] = (uint)nudPointsTOD4Op8.Value;
        }

        private void nudPointsTOD4Op9_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[8] = (uint)nudPointsTOD4Op9.Value;
        }

        private void nudPointsTOD4Op10_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[9] = (uint)nudPointsTOD4Op10.Value;
        }

        private void nudPointsTOD4Op11_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[10] = (uint)nudPointsTOD4Op11.Value;
        }

        private void nudPointsTOD4Op12_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[11] = (uint)nudPointsTOD4Op12.Value;
        }

        private void nudPointsTOD4Op13_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[12] = (uint)nudPointsTOD4Op13.Value;
        }

        private void nudPointsTOD4Op14_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[13] = (uint)nudPointsTOD4Op14.Value;
        }

        private void nudPointsTOD4Op15_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[14] = (uint)nudPointsTOD4Op15.Value;
        }

        private void nudPointsTOD4Op16_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[15] = (uint)nudPointsTOD4Op16.Value;
        }

        private void nudPointsTOD4Op17_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[16] = (uint)nudPointsTOD4Op17.Value;
        }

        private void nudPointsTOD4Op18_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[17] = (uint)nudPointsTOD4Op18.Value;
        }

        private void nudPointsTOD4Op19_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[18] = (uint)nudPointsTOD4Op19.Value;
        }

        private void nudPointsTOD4Op20_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[19] = (uint)nudPointsTOD4Op20.Value;
        }

        private void nudPointsTOD4Op21_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[20] = (uint)nudPointsTOD4Op21.Value;
        }

        private void nudPointsTOD4Op22_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[21] = (uint)nudPointsTOD4Op22.Value;
        }

        private void nudPointsTOD4Op23_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[22] = (uint)nudPointsTOD4Op23.Value;
        }

        private void nudPointsTOD4Op24_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[3].OperationScore[23] = (uint)nudPointsTOD4Op24.Value;
        }

        //TOD 5 Ops Scores
        private void nudPointsTOD5Op1_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[0] = (uint)nudPointsTOD5Op1.Value;
        }

        private void nudPointsTOD5Op2_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[1] = (uint)nudPointsTOD5Op2.Value;
        }

        private void nudPointsTOD5Op3_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[2] = (uint)nudPointsTOD5Op3.Value;
        }

        private void nudPointsTOD5Op4_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[3] = (uint)nudPointsTOD5Op4.Value;
        }

        private void nudPointsTOD5Op5_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[4] = (uint)nudPointsTOD5Op5.Value;
        }

        private void nudPointsTOD5Op6_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[5] = (uint)nudPointsTOD5Op6.Value;
        }

        private void nudPointsTOD5Op7_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[6] = (uint)nudPointsTOD5Op7.Value;
        }

        private void nudPointsTOD5Op8_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[7] = (uint)nudPointsTOD5Op8.Value;
        }

        private void nudPointsTOD5Op9_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[8] = (uint)nudPointsTOD5Op9.Value;
        }

        private void nudPointsTOD5Op10_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[9] = (uint)nudPointsTOD5Op10.Value;
        }

        private void nudPointsTOD5Op11_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[10] = (uint)nudPointsTOD5Op11.Value;
        }

        private void nudPointsTOD5Op12_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[11] = (uint)nudPointsTOD5Op12.Value;
        }

        private void nudPointsTOD5Op13_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[12] = (uint)nudPointsTOD5Op13.Value;
        }

        private void nudPointsTOD5Op14_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[13] = (uint)nudPointsTOD5Op14.Value;
        }

        private void nudPointsTOD5Op15_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[14] = (uint)nudPointsTOD5Op15.Value;
        }

        private void nudPointsTOD5Op16_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[15] = (uint)nudPointsTOD5Op16.Value;
        }

        private void nudPointsTOD5Op17_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[16] = (uint)nudPointsTOD5Op17.Value;
        }

        private void nudPointsTOD5Op18_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[17] = (uint)nudPointsTOD5Op18.Value;
        }

        private void nudPointsTOD5Op19_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[18] = (uint)nudPointsTOD5Op19.Value;
        }

        private void nudPointsTOD5Op20_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[19] = (uint)nudPointsTOD5Op20.Value;
        }

        private void nudPointsTOD5Op21_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[20] = (uint)nudPointsTOD5Op21.Value;
        }

        private void nudPointsTOD5Op22_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[21] = (uint)nudPointsTOD5Op22.Value;
        }

        private void nudPointsTOD5Op23_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[22] = (uint)nudPointsTOD5Op23.Value;
        }

        private void nudPointsTOD5Op24_ValueChanged(object sender, EventArgs e)
        {
            pilot.TourRecordList[4].OperationScore[23] = (uint)nudPointsTOD5Op24.Value;
        }

        // Pilot Medals

        private void checkCorellianCross_CheckedChanged(object sender, EventArgs e)
        {
            pilot.Medals.CorellianCross = checkCorellianCross.Checked;
        }

        private void checkMantooineMedallion_CheckedChanged(object sender, EventArgs e)
        {
            pilot.Medals.MantooineMedallion = checkMantooineMedallion.Checked;
        }

        private void checkStarOfAlderaan_CheckedChanged(object sender, EventArgs e)
        {
            pilot.Medals.StarOfAlderaan = checkStarOfAlderaan.Checked;
        }

        private void checkShieldOfYavin_CheckedChanged(object sender, EventArgs e)
        {
            pilot.Medals.ShieldOfYavin = checkShieldOfYavin.Checked;
        }

        private void checkTalonsOfHoth_CheckedChanged(object sender, EventArgs e)
        {
            pilot.Medals.TalonsOfHoth = checkTalonsOfHoth.Checked;
        }

        private void cbKalidorCrescent_SelectedIndexChanged(object sender, EventArgs e)
        {
            pilot.Medals.KalidorCrescent = (KalidorCrescentLevel)cbKalidorCrescent.SelectedItem;
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("X-Wing Pilot Editor v1.2" + Environment.NewLine + Environment.NewLine + "email: retrotek@shaw.ca", "About X-Wing Pilot Editor");
        }

        private void nudCurrentTour_ValueChanged(object sender, EventArgs e)
        {
            pilot.CurrentTour = (uint)nudCurrentTour.Value - 1;

            switch (pilot.CurrentTour)
            {
                case 0:
                    pilot.CurrentTourOpsComp = (uint)nudTOD1OpsComplete.Value;
                    break;

                case 1:
                    pilot.CurrentTourOpsComp = (uint)nudTOD2OpsComplete.Value;
                    break;

                case 2:
                    pilot.CurrentTourOpsComp = (uint)nudTOD3OpsComplete.Value;
                    break;

                case 3:
                    pilot.CurrentTourOpsComp = (uint)nudTOD4OpsComplete.Value;
                    break;

                case 4:
                    pilot.CurrentTourOpsComp = (uint)nudTOD5OpsComplete.Value;
                    break;
            }
            UpdateForm();
        }
    }
}
