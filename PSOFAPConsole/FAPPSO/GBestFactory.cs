using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP;

namespace PSOFAPConsole.FAPPSO
{
    public class GBestFactory
    {
        public static IGlobalBestSelector<Particle<ICell[]>> GetStandardSelector()
        {
            return new GBestBuilder(CloneIfGbest);
        }

        public static IGlobalBestSelector<Particle<ICell[]>> GetGlobalBestCellBuilderSelector()
        {
            return new GBestBuilder(BuildGBestFromCells);
        }

        public static IGlobalBestSelector<Particle<ICell[]>> GetGlobalBestTRXBuilderSelector()
        {
            return new GBestBuilder(BuildGBestFromTrxs);
        }



        //delegate methods
        protected static void BuildGBestFromCells(Particle<ICell[]> particle,ref Particle<ICell[]> gbest)
        {
            for (int i = 0; i < particle.Position.Length; i++)
            {
                if (particle.Position[i].Interference < gbest.Position[i].Interference)
                {
                    gbest.Position[i] = (ICell)particle.Position[i].Clone();
                }
            }
        }


        protected static void BuildGBestFromTrxs(Particle<ICell[]> particle, ref Particle<ICell[]> gbest)
        {
            for (int i = 0; i < particle.Position.Length; i++)
            {
                for (int j = 0; j < particle.Position[i].Frequencies.Length; j++)
                    if (particle.Position[i].Frequencies[j].Interference < gbest.Position[i].Frequencies[j].Interference)
                    {
                        gbest.Position[i].Frequencies[j] = (TRX)particle.Position[i].Frequencies[j].Clone();
                        //particle.Position[i].Frequencies[j].Interference = 0;
                    }
            }
        }

        protected static void CloneIfGbest(Particle<ICell[]> particle,ref Particle<ICell[]> gbest)
        {
            if (particle.Fitness < gbest.Fitness)
                gbest = (Particle<ICell[]>)particle.Clone();
        }

    }
}
