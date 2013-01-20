using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.PSO;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP;

namespace PSOFAPConsole.FAPPSO
{
    
    public class GBestBuilder : IGlobalBestSelector<Particle<ICell[]>>
    {
       
        public delegate void GlobalBestConstructionMethod(Particle<ICell[]> particle,ref Particle<ICell[]> gbest);
        private Particle<ICell[]> globalBest;
        private GlobalBestConstructionMethod BuildGBest;


        public GBestBuilder(GlobalBestConstructionMethod globalBestBuildMethod)
        {
            BuildGBest = globalBestBuildMethod;
        }

        public Particle<ICell[]> FindGlobalBest(List<Particle<ICell[]>> population)
        {
            return BuildGlobalBest(population);
        }

        private Particle<ICell[]> BuildGlobalBest(List<Particle<ICell[]>> population)
        {
            foreach (Particle<ICell[]> particle in population)
            {
                if (globalBest == null)
                {
                    globalBest = (Particle<ICell[]>)particle.Clone();
                }
                else
                {
                    BuildGBest(particle,ref globalBest);
                }
            }
            return globalBest;

        }





        public string GetConstructionMethodName()
        {
            return BuildGBest.Method.Name;
        }
    }
}
