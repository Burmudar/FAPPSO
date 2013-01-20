using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.PSO.Interfaces;
using PSOFAP.FAPModel;
using PSOFAP.PSO;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPPSO
{
    public class ParticlePerTrxFunction : IMoveFunction<Particle<ICell[]>>
    {
        public int[] Spectrum { get; set; }
        public int[] GBC { get; set; }

        public ParticlePerTrxFunction(int[] spectrum, int[] gbc)
        {
            Spectrum = spectrum;
            GBC = gbc;
        }

        #region IMoveFunction<Particle<Cell[]>> Members

        public Particle<ICell[]> MoveTowards(Particle<ICell[]> from, Particle<ICell[]> to)
        {
            ICell[] pbest = from.PersonalBest.Position;

            ICell[] a = Multiply(0.41,true,Subtract(pbest,from.Position));
            ICell[] b = Multiply(0.52,true,Subtract(to.Position, from.Position));
            if (from.Velocity != null)
            {
                from.Velocity = Multiply(0.7,false, Add(a, b));
            }
            else
            {
                from.Velocity = Add(a, b);
            }
            from.Position = Add(from.Position, from.Velocity);
            return from;
        }

        private Cell[] Subtract(ICell[] a, ICell[] b)
        {
            Cell[] answ = new Cell[a.Length];
            for(int i = 0; i < a.Length;i++)
            {
                answ[i] = a[i].Clone() as Cell;
                for (int j = 0; j < a[i].Frequencies.Length; j++)
                {
                    answ[i].Frequencies[j] = Math.Abs(a[i].Frequencies[j] - b[i].Frequencies[j]);
                }
            }
            return answ;
        }

        private ICell[] Multiply(double x,Boolean random, ICell[] a)
        {
            Random r = new Random();
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].Frequencies.Length; j++)
                {
                    if (random)
                    {
                        a[i].Frequencies[j] = (int)(a[i].Frequencies[j] * x * r.NextDouble());
                    }
                    else
                    {
                        a[i].Frequencies[j] = (int)(a[i].Frequencies[j] * x);
                    }
                }
            }
            return a;
        }

        private ICell[] Add(ICell[] a, ICell[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].Frequencies.Length; j++)
                {
                    a[i].Frequencies[j] += b[i].Frequencies[j];
                    a[i].Frequencies[j] = BoundFrequency(a[i].Frequencies[j], Spectrum[0],Spectrum[1]);
                }
            }
            return a;
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

        private int BoundFrequency(int frequency, int lower, int upper)
        {
            if(frequency > upper)
            {
                return frequency % upper;
            }
            else if (frequency < lower)
            {
                return frequency % lower;
            }
            return frequency;
        }

        #endregion
    }
}
