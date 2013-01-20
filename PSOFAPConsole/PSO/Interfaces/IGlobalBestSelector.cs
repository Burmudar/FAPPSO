using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.PSO.Interfaces
{
    public interface IGlobalBestSelector<T>
    {
        T FindGlobalBest(List<T> population);
        String GetConstructionMethodName();
    }
}
