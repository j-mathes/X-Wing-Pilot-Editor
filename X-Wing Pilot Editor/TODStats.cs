using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X_Wing_Pilot_Editor
{
    public class TODStats
    {
        public ushort TotalSurfaceVictories { get; set; }
        public List<TODVictoriesCaptures> TODVictoriesCapturesList { get; set;}
        public uint LasersFired { get; set; }
        public uint LaserCraftHits { get; set; }
        public uint LaserGroundHits { get; set; }
        public ushort MisslesFired { get; set; }
        public ushort MissleCraftHits { get; set; }
        public ushort MissleGroundHits { get; set; }
        public ushort CraftLost { get; set; }
        
        public TODStats()
        {
            TotalSurfaceVictories = 0;
            TODVictoriesCapturesList = new List<TODVictoriesCaptures>();
            LasersFired = 0;
            LaserCraftHits = 0;
            LaserGroundHits = 0;
            MisslesFired = 0;
            MissleCraftHits = 0;
            MissleGroundHits = 0;
            CraftLost = 0;

            TODVictoriesCapturesList.Clear();

            foreach (NPCShipType npcShip in Enum.GetValues(typeof(NPCShipType)))
            {
                TODVictoriesCapturesList.Add(new TODVictoriesCaptures(npcShip));
            }
        }
        
        public TODStats(byte[] bytes)
        {
            TotalSurfaceVictories = BitConverter.ToUInt16(bytes, 1587); // TODO: this may not be reading in correct
            TODVictoriesCapturesList = new List<TODVictoriesCaptures>();
            LasersFired = BitConverter.ToUInt32(bytes, 1685);
            LaserCraftHits = BitConverter.ToUInt32(bytes, 1689);
            LaserGroundHits = BitConverter.ToUInt32(bytes, 1693);
            MisslesFired = BitConverter.ToUInt16(bytes, 1697);
            MissleCraftHits = BitConverter.ToUInt16(bytes, 1699);
            MissleGroundHits = BitConverter.ToUInt16(bytes, 1701);
            CraftLost = BitConverter.ToUInt16(bytes, 1703);

            TODVictoriesCapturesList.Clear();
            int firstKillsOffset = 1589; // TODO: this may not be reading in correct
            int firstCapturesOffset = 1637;
            int countMultiplier = 0;

            foreach(NPCShipType npcShip in Enum.GetValues(typeof(NPCShipType)))
            {
                TODVictoriesCapturesList.Add(new TODVictoriesCaptures(npcShip, bytes, firstKillsOffset + countMultiplier, firstCapturesOffset + countMultiplier));
                countMultiplier += 2;
            }
        }

        public void WriteData(byte[] bytes)
        {
            TotalSurfaceVictories.CopyToByteArrayLE(bytes, 1587);

            LasersFired.CopyToByteArrayLE(bytes, 1685);
            LaserCraftHits.CopyToByteArrayLE(bytes, 1689);
            LaserGroundHits.CopyToByteArrayLE(bytes, 1693);
            MisslesFired.CopyToByteArrayLE(bytes, 1697);
            MissleCraftHits.CopyToByteArrayLE(bytes, 1699);
            MissleGroundHits.CopyToByteArrayLE(bytes, 1701);
            CraftLost.CopyToByteArrayLE(bytes, 1703);

            int firstKillsOffset = 1589;
            int firstCapturesOffset = 1637;
            int countMultiplier = 0;

            foreach (NPCShipType npcShip in Enum.GetValues(typeof(NPCShipType)))
            {
                TODVictoriesCapturesList[(int)npcShip].Victories.CopyToByteArrayLE(bytes, firstKillsOffset + countMultiplier);
                TODVictoriesCapturesList[(int)npcShip].Captures.CopyToByteArrayLE(bytes, firstCapturesOffset + countMultiplier);
                countMultiplier += 2;
            }
        }
    }
}
