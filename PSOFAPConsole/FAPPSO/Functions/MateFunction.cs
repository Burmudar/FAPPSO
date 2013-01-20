using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.PSO;

namespace PSOFAPConsole.FAPPSO
{
    public class MateFunction : IMoveFunction<Particle<ICell[]>>
    {
        FrequencyPositionGenerator Generator;
        public MateFunction(FrequencyPositionGenerator gen)
        {
            Generator = gen;
        }

        #region IMoveFunction<Particle<ICell[]>> Members

        public Particle<ICell[]> MoveTowards(Particle<ICell[]> from, Particle<ICell[]> to)
        {
            ICell[] fromPosition = from.Position;
            ICell[] toPosition = to.Position;
            
            int c2 = (int)(toPosition.Length * 0.6);
            ICell[] random = Generator.GeneratePosition();
            for (int c1 = (int)(fromPosition.Length * 0.4); c1 < fromPosition.Length; c1++)
            {
                fromPosition[c1] = toPosition[c1];
            }
            for (int i = 0; i < fromPosition.Length / 2; i++)
            {
                fromPosition[i] = random[i];
            }
            return from;
        }


        #endregion
    }
}
