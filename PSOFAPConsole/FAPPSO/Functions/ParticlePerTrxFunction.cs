using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP;
using PSOFAPConsole.PSO;
using PSOFAPConsole.FAP.Interfaces;
using System.Threading.Tasks;

namespace PSOFAPConsole.FAPPSO
{
    public class ParticlePerTrxFunction : IMoveFunction<Particle<ICell[]>>
    {
        private int upperBound;
        private int lowerBound;
        private double localCoefficient;
        private double globalCoefficient;
        private AbstractCollisionResolver collisionResolver;
        private int coSite;
        private int coCell;

        public ParticlePerTrxFunction(FAPModel model,int lowerBound, int upperBound, double localCoef, double globalCoef,AbstractCollisionResolver collisionResolver)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
            localCoefficient = localCoef;
            globalCoefficient = globalCoef;
            this.collisionResolver = collisionResolver;
            coSite = model.GeneralInformation.CoSiteSeperation;
            coCell = model.GeneralInformation.DefaultCoCellSeperation;

        }

        #region IMoveFunction<Particle<Cell[]>> Members

        public Particle<ICell[]> MoveTowards(Particle<ICell[]> from, Particle<ICell[]> to)
        {
            ICell[] pbest = from.PersonalBest.Position;

            ICell[] a = Multiply(localCoefficient,true,Subtract(pbest,from.Position));
            ICell[] b = Multiply(globalCoefficient,true,Subtract(to.Position, from.Position));
            if (from.Velocity != null)
            {
                from.Velocity = Multiply(0.5,false, Add(from.Velocity,Add(a, b)));
            }
            else
            {
                from.Velocity = Add(a, b);
                
            }
            //EnsureUnique(from.Velocity);
            EnsureUnique(Add(from.Position, from.Velocity));
            MigrateFrequencies(from.Position);
            return from;
        }

        private void MigrateFrequencies(ICell[] iCell)
        {
            Parallel.ForEach(iCell, cell =>
                {
                    collisionResolver.ResolveCollisions(cell.FrequencyHandler);
                    AdhereToSeperation(cell.FrequencyHandler);
                    cell.FrequencyHandler.MigrateFrequenciesToParent();
                });
        }

        private void AdhereToSeperation(FrequencyHandler frequencyHandler)
        {
            for (int i = 0; i < frequencyHandler.Length; i++)
            {
                if (ViolatesSeperation(i, frequencyHandler, coCell))
                {
                    frequencyHandler[i] = BoundValue(frequencyHandler[i] - coCell);
                }
            }
        }

        private void EnsureUnique(ICell[] iCells)
        {
            Parallel.ForEach(iCells, MakeUnique);
        }

        private void MakeUnique(ICell cell)
        {
            Random r = new Random();
            FrequencyHandler handler = cell.FrequencyHandler;
            for (int i = 0; i < handler.Length; i++)
            {
                if (hasValue(i, handler))
                {
                    int newFrequency = r.Next(upperBound);
                    newFrequency = BoundValue(newFrequency);
                    handler[i] = newFrequency;
                    i--;
                }
            }
        }

        private bool hasValue(int i, FrequencyHandler handler)
        {
            for (int j = 0; j < handler.Length; j++)
            {
                if (handler[i] == handler[j] && i != j)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ViolatesSeperation(int i, FrequencyHandler handler,int seperation)
        {
            for (int j = 0; j < handler.Length; j++)
            {
                if (i == j)
                    continue;
                if (handler[i] == handler[j])
                {
                    return true;
                }
                if (Math.Abs(handler[j] - handler[i]) < seperation)
                {
                    return true;
                }
            }
            return false;
        }

        //Loops through each cell and then each trx that a cell contains. 
        //It takes each trx and subtracts it from the same cell trx found in the other cell array
        private BasicCell[] Subtract(ICell[] a, ICell[] b)
        {
            BasicCell[] answ = new BasicCell[a.Length];
            for(int i = 0; i < a.Length;i++)
            {
                answ[i] = a[i].Clone() as BasicCell;
                for (int j = 0; j < a[i].FrequencyHandler.Length; j++)
                {
                    answ[i].FrequencyHandler[j] = (a[i].FrequencyHandler[j] - b[i].FrequencyHandler[j]);
                }
            }
            return answ;
        }

        //Loops through the given cell array and then it loops through each trx that a cell contains and multiplies it with x.
        //If random is true, the product of the trx multiplied by x is also multiplied with a random value in range [0.0,1.0]
        private ICell[] Multiply(double x,Boolean random, ICell[] a)
        {
            Random r = new Random();
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].FrequencyHandler.Length; j++)
                {
                    if (random)
                    {
                        a[i].FrequencyHandler[j] = (int)(a[i].FrequencyHandler[j] * x * r.NextDouble());
                    }
                    else
                    {
                        a[i].FrequencyHandler[j] = (int)(a[i].FrequencyHandler[j] * x);
                    }
                }
            }
            return a;
        }

        //Takes each corresponding trx found in a cell and adds it together.
        //The result of two trx's being added is then bounded to ensure it lies within the spectrum bounds
        private ICell[] Add(ICell[] a, ICell[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].FrequencyHandler.Length; j++)
                {
                    a[i].FrequencyHandler[j] += b[i].FrequencyHandler[j];
                    a[i].FrequencyHandler[j] = BoundValue(a[i].FrequencyHandler[j]);
                }
            }
            return a;
        }

        private int BoundValue(int value)
        {
            if (value < 0)
                return BoundValue(Math.Abs(value));
            if(value > upperBound)
            {
                return lowerBound + (value % upperBound);
            }
            else if (value < lowerBound)
            {
                return BoundValue(value + lowerBound) ;
            }
            return value;
        }

        #endregion
    }
}
