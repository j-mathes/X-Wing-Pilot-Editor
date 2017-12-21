using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X_Wing_Pilot_Editor
{
    public class TrainingRecord
    {
        public FlyableShipType TrainingShipType { get; set; }
        public uint TrainingScore { get; set; }
        public int TrainingCompleted { get; set; }

        public TrainingRecord(FlyableShipType trainingShipType)
        {
            TrainingShipType = trainingShipType;
            TrainingScore = 0;
            TrainingCompleted = 0;
        }

        public TrainingRecord(FlyableShipType trainingShipType, byte[] bytes, int trainingScoreOffset, int trainingCompletedOffset)
        {
            TrainingShipType = trainingShipType;
            TrainingScore = BitConverter.ToUInt32(bytes, trainingScoreOffset);
            TrainingCompleted = bytes[trainingCompletedOffset];
        }

        public void WriteData(FlyableShipType playerShip, byte[] bytes)
        {
            switch (playerShip)
            {
                case FlyableShipType.XWing:
                    bytes[134] = (byte)TrainingCompleted;
                    TrainingScore.CopyToByteArrayLE(bytes, 38);
                    break;
                case FlyableShipType.YWing:
                    bytes[135] = (byte)TrainingCompleted;
                    TrainingScore.CopyToByteArrayLE(bytes, 42);
                    break;
                case FlyableShipType.AWing:
                    bytes[136] = (byte)TrainingCompleted;
                    TrainingScore.CopyToByteArrayLE(bytes, 46);
                    break;
                case FlyableShipType.BWing:
                    bytes[137] = (byte)TrainingCompleted;
                    TrainingScore.CopyToByteArrayLE(bytes, 50);
                    break;
            }
        }
    }
}