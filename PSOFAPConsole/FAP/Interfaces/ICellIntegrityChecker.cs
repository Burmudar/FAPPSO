using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP.Interfaces
{
    public interface ICellIntegrityChecker
    {
        int CountViolations(ICell cell);
        int CountViolations(ICell[] cells);
        
    }
}
