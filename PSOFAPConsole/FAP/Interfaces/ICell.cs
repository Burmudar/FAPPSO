using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PSOFAPConsole.FAP.Interfaces
{
    public interface ICell : ICloneable
    {
        String CellID { get; }
        String SiteName { get; }
        int Sector { get; }
        double Traffic { get; }
        Coordinate Location { get;}
        int[] LocallyBlocked { get; }
        int CoSiteSeperation { get; }
        TRX[] Frequencies { get; set; }
        FrequencyHandler FrequencyHandler{ get; set; }
        double Interference { get; set; }
        Boolean HasLocallyBlockedChannels();
    }
}
