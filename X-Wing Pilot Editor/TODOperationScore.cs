using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X_Wing_Pilot_Editor
{
    public class TODOperationScore
    {
        public int TourNumber { get; set; }
        public TourStatus TourStatus { get; set; }
        public UInt16 OperationsComplete { get; set; }
        public List<uint> OperationScore { get; set; }

        public TODOperationScore(int tourNumber, int numberOfOperations)
        {
            TourNumber = tourNumber;
            TourStatus = TourStatus.Inactive;
            OperationsComplete = 0;
            OperationScore = new List<uint>(numberOfOperations);

            for (int i = 0; i < numberOfOperations; i++)
            {
                OperationScore.Add(0);
            }
        }

        public TODOperationScore(int tourNumber, byte[] bytes, int isTourActiveOffset, int numberOfOperations, int operationsCompleteOffset, int operationScoreOffset)
        {
            TourNumber = tourNumber;
            TourStatus = (TourStatus)bytes[isTourActiveOffset];
            OperationsComplete = bytes[operationsCompleteOffset];
            OperationScore = new List<uint>(numberOfOperations);

            for (int i = 0; i < numberOfOperations; i++)
            {
                OperationScore.Add(BitConverter.ToUInt32(bytes, operationScoreOffset + (i * 4)));
            }
        }

        public void WriteData(byte[] bytes, int isTourActiveOffset, int numberOfOperations, int operationsCompleteOffset, int operationScoreOffset)
        {
            bytes[isTourActiveOffset] = (byte)TourStatus;
            OperationsComplete.CopyToByteArrayLE(bytes, operationsCompleteOffset);
            for (int i = 0; i < numberOfOperations; i++)
            {
                OperationScore[i].CopyToByteArrayLE(bytes, operationScoreOffset + (i * 4));
            }
        }
    }
}
