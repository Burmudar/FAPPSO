using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.FAPModel.Interfaces
{
    public interface ICellInterferer
    {
        int getCellIndex();
        ICellRelation getCellRelation();
    }
}
