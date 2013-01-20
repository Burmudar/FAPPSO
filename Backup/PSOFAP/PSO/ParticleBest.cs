using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.PSO
{
    public class ParticleBest<T>
    {
        public T Position { get; set; }
        public double Fitness { get; set; }

        public ParticleBest(T Pos, double fitness)
        {
            T clone = (T)((ICloneable)Pos).Clone();
            Position = Pos;
            Fitness = fitness;
        }
    }
}
