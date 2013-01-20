using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.PSO.Interfaces;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPPSO
{
    public class MateFunction : IMoveFunction<ICell[]>
    {
        FAPPositionGenerator Generator;
        public MateFunction(FAPPositionGenerator gen)
        {
            Generator = gen;
        }

        #region IMoveFunction<ICell[]> Members

        public ICell[] MoveTowards(ICell[] from, ICell[] to)
        {
            
            int c2 = (int)(to.Length * 0.6);
            ICell[] random = Generator.GeneratePosition();
            for (int c1 = (int)(from.Length * 0.4); c1 < from.Length; c1++)
            {
                from[c1] = to[c1];
            }
            for (int i = 0; i < from.Length / 2; i++)
            {
                from[i] = random[i];
            }
            return from;
        }


        #endregion
    }
}
