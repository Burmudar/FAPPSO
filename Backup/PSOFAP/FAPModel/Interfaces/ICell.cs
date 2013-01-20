using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PSOFAP.FAPModel.Interfaces
{
    public interface ICell : ICloneable
    {
        String CellID { get; set; }
        String SiteName { get; set; }
        int Sector { get; set; }
        double Traffic { get; set; }
        Coordinate Location { get; set; }
        int[] LocallyBlocked { get; set; }
        int CoSiteSeperation { get; set; }
        int[] Frequencies { get; set; }
        int getFrequencyAt(int i);
        void setFrequencyAt(int i,int value);
        Boolean hasLocallyBlockedChannels();
    }
}
