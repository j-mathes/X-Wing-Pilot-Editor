using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X_Wing_Pilot_Editor
{
    public class Medals
    {
        public KalidorCrescentLevel KalidorCrescent { get; set; }
        public bool CorellianCross { get; set; }
        public bool MantooineMedallion { get; set; }
        public bool StarOfAlderaan { get; set; }
        public bool ShieldOfYavin { get; set; }
        public bool TalonsOfHoth { get; set; }

        public Medals()
        {
            CorellianCross = false;
            MantooineMedallion = false;
            StarOfAlderaan = false;
            ShieldOfYavin = false;
            TalonsOfHoth = false;

            KalidorCrescent = (int)KalidorCrescentLevel.None;
        }

        public Medals(byte[] bytes)
        {
            CorellianCross = BitConverter.ToBoolean(bytes, 10);
            MantooineMedallion = BitConverter.ToBoolean(bytes, 11);
            StarOfAlderaan = BitConverter.ToBoolean(bytes, 12);
            ShieldOfYavin = BitConverter.ToBoolean(bytes, 13);
            TalonsOfHoth = BitConverter.ToBoolean(bytes, 14);

            KalidorCrescent = (KalidorCrescentLevel)bytes[17];
        }

        public void WriteData(byte[] bytes)
        {
            bytes[10] = Convert.ToByte(CorellianCross);
            bytes[11] = Convert.ToByte(MantooineMedallion);
            bytes[12] = Convert.ToByte(StarOfAlderaan);
            bytes[13] = Convert.ToByte(ShieldOfYavin);
            bytes[14] = Convert.ToByte(TalonsOfHoth);
            bytes[17] = Convert.ToByte(KalidorCrescent);
        }
    }
}
