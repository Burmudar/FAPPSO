using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.PSO.Interfaces
{
    public interface IMoveFunction<T>
    {
        T MoveTowards(T from, T to);
    }
}
