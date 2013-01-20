using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP.Interfaces
{
    public interface ICellInterferer
    {
        int getCellIndex();
        ICellRelation getCellRelation();
    }
}
