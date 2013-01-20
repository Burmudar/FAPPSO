using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.PSO.Interfaces
{
    public interface IFitnessFunction<T>
    {
        double Evaluate(T position);
    }
}
