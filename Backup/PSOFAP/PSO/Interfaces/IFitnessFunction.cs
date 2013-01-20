using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.PSO.Interfaces
{
    public interface IFitnessFunction<T>
    {
        double Evaluate(T position);
    }
}
