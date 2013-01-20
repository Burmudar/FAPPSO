using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP;
using PSOFAPConsole.PSO;
using System.Threading.Tasks;

namespace PSOFAPConsole.FAPPSO.Functions
{
    public class PerTRXChannelIndexFunction : IMoveFunction<Particle<ICell[]>>
    {
        public AbstractCollisionResolver CollisionResolver { get; set; }
        public double LocalCoefficient { get; set; }
        public double GlobalCoefficient { get; set; }
        public int[] Channels { get; set; }
        private int coSite;
        private int coCell;

        public PerTRXChannelIndexFunction(FAPModel model,double localCoefficient, double globalCoefficient,AbstractCollisionResolver collisionResolver)
        {
            CollisionResolver = collisionResolver;
            Channels = model.Channels;
            LocalCoefficient = localCoefficient;
            GlobalCoefficient = globalCoefficient;
            coSite = model.GeneralInformation.CoSiteSeperation;
            coCell = model.GeneralInformation.DefaultCoCellSeperation;
        }

        public Particle<ICell[]> MoveTowards(Particle<ICell[]> from, Particle<ICell[]> to)
        {
            ICell[] velocity = from.Velocity;
            for (int i = 0; i < from.Position.Length; i++)
            {
                FrequencyHandler pbest = from.PersonalBest.Position[i].FrequencyHandler;
                FrequencyHandler fromIndex = from.Position[i].FrequencyHandler;
                FrequencyHandler toIndex = to.Position[i].FrequencyHandler;
                FrequencyHandler movedIndexes = MoveIndexes(pbest, fromIndex, toIndex);
                if (velocity == null)
                {
                    velocity = new ICell[from.Position.Length];
                    from.Position.CopyTo(velocity, 0);
                }
                FrequencyHandler indexVelocity = CalculateIndexVelocity(velocity[i].FrequencyHandler, movedIndexes);
                ApplyVelocity(indexVelocity, fromIndex);
            }
            MigrateFrequencies(from.Position);
            return from;
        }

        private void MigrateFrequencies(ICell[] iCell)
        {
            Parallel.ForEach(iCell, cell => {
                CollisionResolver.ResolveCollisions(cell.FrequencyHandler);
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
                    if (frequencyHandler[i] - coCell < 0)
                    {
                        frequencyHandler[i] = Channels.Length - coCell;
                    }
                    else
                    {
                        frequencyHandler[i] = frequencyHandler[i] - coCell;
                    }
                }
            }
        }

        private bool ViolatesSeperation(int i, FrequencyHandler handler, int seperation)
        {
            for (int j = 0; j < handler.Length; j++)
            {
                if (handler[i] == handler[j] && i != j)
                {
                    return true;
                }
                //handler[j] and [i] contain channel indexes
                if (Math.Abs(Channels[handler[j]] - Channels[handler[i]]) < seperation)
                {
                    return true;
                }
            }
            return false;
        }

        private void ApplyVelocity(FrequencyHandler indexVelocity, FrequencyHandler fromIndex)
        {
            for (int i = 0; i < indexVelocity.Length; i++)
            {
                fromIndex[i] = Math.Abs((fromIndex[i] + indexVelocity[i])) % Channels.Length;
            }
            
        }

        private bool IsDuplicate(int i, FrequencyHandler fromIndex)
        {
            for (int j = 0; j < fromIndex.Length; j++)
            {
                if (j == i)
                    continue;
                else if (fromIndex[j] == fromIndex[i])
                {
                    return true;
                }
            }
            return false;
        }


        private FrequencyHandler CalculateIndexVelocity(FrequencyHandler velocity, FrequencyHandler movedIndexes)
        {
            for (int i = 0; i < velocity.Length; i++)
            {
                velocity[i] = velocity[i] + (int)(0.5 * movedIndexes[i]);
            }
            return velocity;
        }


        private FrequencyHandler MoveIndexes(FrequencyHandler pbest, FrequencyHandler from, FrequencyHandler to)
        {
            Random random = new Random();
            for (int i = 0; i < from.Length; i++)
            {
                double r1 = random.NextDouble();
                double r2 = random.NextDouble();
                double a1 = (LocalCoefficient * r1 * (pbest[i] - from[i]));
                double a2 = (GlobalCoefficient * r2 * (pbest[i] - to[i]));
                from[i] = (int)(a1 + a2);
            }
            return from;
        }

        private bool hasFrequencyIndex(FrequencyHandler frequencies, int frequency)
        {
            return frequencies.Any(el => el == frequency);
        }


    }
}
