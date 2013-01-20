using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP;
using PSOFAPConsole.FAPPSO;
using System.Threading.Tasks;

namespace PSOFAPConsole.PSO
{
    public class BenchRunner
    {
        public  delegate PSOAlgorithm<ICell[]> pso(int pop,FAPModel model,double c1,double c2);
        public delegate FAPModel ModelCreators();
        public List<int> Populations { get; set; }
        public List<ModelCreators> Models { get; set; }
        public List<pso> PSOCreate { get; set; }

        public BenchRunner()
        {
            Populations = new List<int>();
            Models = new List<ModelCreators>();
            PSOCreate = new List<pso>();
            FAPModelFactory modelFactory = new FAPModelFactory();
            Models.Add(modelFactory.CreateSiemens1Model);
            Models.Add(modelFactory.CreateSiemens2Model);
            Models.Add(modelFactory.CreateSiemens3Model);
            Models.Add(modelFactory.CreateSiemens4Model);
            Populations.Add(20);
            Populations.Add(50);
            Populations.Add(100);
            Populations.Add(500);
            FAPPSOFactory factory = new FAPPSOFactory();
            PSOCreate.Add(factory.CreateFrequencyIndexBased);
            PSOCreate.Add(factory.CreateIndexBasedFAPPSOWithGlobalBestCellBuilder);
            PSOCreate.Add(factory.CreateIndexBasedFAPPSOWithGlobalBestTRXBuilder);
            PSOCreate.Add(factory.CreateIndexMovementBased);
            PSOCreate.Add(factory.CreateIndexMovementBasedWithGlobalBestCellBuilder);
            PSOCreate.Add(factory.CreateIndexMovementBasedWithGlobalBestTRXBuilder);
        }

        public void StartBenchmark()
        {
            foreach (int population in Populations)
            {
                Console.WriteLine("Starting benchmark with population size <{0}>", population);
                foreach (ModelCreators modelcreator in Models)
                {
                    FAPModel model = modelcreator();
                    foreach (pso psocreator in PSOCreate)
                    {
                        PSOAlgorithm<ICell[]> fapPso = psocreator(population, model, 0.41, 0.59);
                        fapPso.Start();
                    }
                }
            }
        }
    }
}
