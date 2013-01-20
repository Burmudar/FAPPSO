using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP;
using PSOFAPConsole.FAPPSO.Functions;

namespace PSOFAPConsole.FAPPSO
{
    public class PerTRXFunction : IMoveFunction<ICell[]>
    {
        public int[] Spectrum { get; set; }
        public int[] GBC { get; set; }
        public AbstractCollisionResolver CollisionResolver { get; set; }
        public double LocalCoefficient { get; set; }
        public double GlobalCoefficient { get; set; }
        private int coSite;
        private int coCell;

        public PerTRXFunction(FAPModel model, double localCoefficient, double globalCoefficient, AbstractCollisionResolver collisionResolver)
        {
            Spectrum = model.GeneralInformation.Spectrum;
            GBC = model.GeneralInformation.GloballyBlockedChannels;
            CollisionResolver = collisionResolver;
            LocalCoefficient = localCoefficient;
            GlobalCoefficient = globalCoefficient;
            coSite = model.GeneralInformation.CoSiteSeperation;
            coCell = model.GeneralInformation.DefaultCoCellSeperation;
        }

        #region IMoveFunction<ICell[]> Members

        public ICell[] MoveTowards(ICell[] from, ICell[] to)
        {
            for (int i = 0; i < to.Length; i++)
            {
                FrequencyHandler newFrequencies = MoveTowardsChannelArray(from[i].FrequencyHandler, to[i].FrequencyHandler);
                CollisionResolver.ResolveCollisions(newFrequencies);
                newFrequencies.MigrateFrequenciesToParent();
            }
            return from;
        }

        private FrequencyHandler MoveTowardsChannelArray(FrequencyHandler from, FrequencyHandler to)
        {
            for (int i = 0; i < to.Length; i++)
            {
                from[i] = MoveTowardsChannel(from[i], to[i]);
            }
            return from;
        }

        private int MoveTowardsChannel(int from, int to)
        {
            Random random = new Random();
            int newChannel = 0;
            while (true)
            {
                newChannel = Math.Abs((int)(((LocalCoefficient * random.Next() * from) + (GlobalCoefficient * random.Next() * to)) % Spectrum[1]));
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
