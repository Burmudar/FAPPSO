using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.FAPModel.Interfaces
{
    public interface ICellRelation
    {
        int[] CellIndex { get; set; }
        int Handover { get; set; }
        int Separation { get; set; }
        double[] DA { get; set; }
        double[] UA { get; set; }
        double[] DT { get; set; }
        double[] UT { get; set; }
    }
}
