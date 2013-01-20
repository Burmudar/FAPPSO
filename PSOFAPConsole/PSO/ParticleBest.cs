using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.PSO
{
    public class ParticleBest<T> : ICloneable
    {
        public T Position { get; set; }
        public double Fitness { get; set; }

        public ParticleBest(T Pos, double fitness)
        {
            T clone = (T)((ICloneable)Pos).Clone();
            Position = Pos;
            Fitness = fitness;
        }



        public object Clone()
        {
            ParticleBest<T> clone = new ParticleBest<T>(Position, Fitness);
            return clone;
        }
    }
}
