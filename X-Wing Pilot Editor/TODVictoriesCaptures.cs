using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X_Wing_Pilot_Editor
{
    public class TODVictoriesCaptures
    {
        public NPCShipType VictoryCaptureNPC { get; set; }
        public ushort Victories { get; set; }
        public ushort Captures { get; set; }

        public TODVictoriesCaptures(NPCShipType victoryCaptureNPC)
        {
            VictoryCaptureNPC = victoryCaptureNPC;
            Victories = 0;
            Captures = 0;
        }

        public TODVictoriesCaptures(NPCShipType victoryCaptureNPC, byte[] bytes, int victoriesOffset, int capturesOffset)
        {
            VictoryCaptureNPC = victoryCaptureNPC;
            Victories = BitConverter.ToUInt16(bytes, victoriesOffset);
            Captures = BitConverter.ToUInt16(bytes, capturesOffset);
        }
    }
}
