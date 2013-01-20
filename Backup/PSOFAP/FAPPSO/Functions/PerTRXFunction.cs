using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.PSO.Interfaces;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPPSO
{
    public class PerTRXFunction : IMoveFunction<ICell[]>
    {
        public int[] Spectrum { get; set; }
        public int[] GBC { get; set; }

        public PerTRXFunction(int[] spectrum, int[] gbc)
        {
            Spectrum = spectrum;
            GBC = gbc;
        }
        #region IMoveFunction<ICell[]> Members

        public ICell[] MoveTowards(ICell[] from, ICell[] to)
        {
            for (int i = 0; i < to.Length; i++)
            {
                MoveTowardsChannelArray(from[i].Frequencies, to[i].Frequencies);
            }
            return from;
        }

        private void MoveTowardsChannelArray(int[] from, int[] to)
        {
            for (int i = 0; i < to.Length; i++)
            {
                from[i] = MoveTowardsChannel(from[i], to[i]);
            }
        }

        private int MoveTowardsChannel(int from, int to)
        {
            Random random = new Random();
            int newChannel = 0;
            while (true)
            {
                newChannel = Math.Abs((int)(((0.4 * random.Next() * from) + (0.6 * random.Next() * to)) % Spectrum[1]));
                if (NotInGBC(newChannel))
                    break;
            }
            return newChannel;
        }

        private bool NotInGBC(int channel)
        {
            if (channel < GBC[0] || channel > GBC[GBC.Length - 1])
                return true;
            foreach (int i in GBC)
            {
                if (channel == i)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
