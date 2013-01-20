using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.FAPModel.Interfaces;
using PSOFAP.PSO;
using PSOFAP.PSO.Interfaces;

namespace PSOFAP.FAPPSO
{
    public class FAPPSOAlgorithm : PSOAlgorithm<ICell[]>
    {
        public List<Particle<ICell[]>> Particles { get; set;}
        public Particle<ICell[]> GlobalBest { get; set; }
        public int Population { get; set; }
        public IFitnessFunction<ICell[]> EvaluationFunction { get; set; }
        public IMoveFunction<Particle<ICell[]>> VelocityFunction { get; set; }
        public FAPPositionGenerator Generator { get; set; }
        private Boolean fakeMode = false;
        public FAPPSOAlgorithm(int amount)
        {
            Particles = new List<Particle<ICell[]>>();
            Population = amount;
            
        }

        #region PSOAlgorithm<T> Members

        public void Initialize()
        {
            FAPModel.FAPModel model = InitializeFAPModel();
            Generator = new FAPPositionGenerator(model);
            for (int i = 0; i < Population; i++)
            {
                Particles.Add(new Particle<ICell[]>(Generator.GeneratePosition()));
            }
            EvaluationFunction = new FAPCostFunction(model.InterferenceMatrix.ToArray(),model.GeneralInformation);
            //VelocityFunction = new PerTRXFunction(model.GeneralInformation.Spectrum,model.GeneralInformation.GloballyBlockedChannels);
            //VelocityFunction = new MateFunction((FAPPositionGenerator) generator);
            VelocityFunction = new ParticlePerTrxFunction(model.GeneralInformation.Spectrum, model.GeneralInformation.GloballyBlockedChannels);
        }

        private FAPModel.FAPModel InitializeFAPModel()
        {
            FAPModel.FAPModel model = new PSOFAP.FAPModel.FAPModel(@"..\..\FAPModel\Problems\siemens4");
            model.createModel();
            return model;
        }

        public void PerformCostTest()
        {
            Console.WriteLine("Performing Cost Test");
            Console.WriteLine("====================");
            Console.Write("Initializing FAP Model ... ");
            FAPModel.FAPModel model = InitializeFAPModel();
            Console.WriteLine("DONE");
            Console.Write("Loading Test assignment data ... ");
            model.LoadTestData(@"..\..\FAPModel\Problems\siemens4.ass");
            Console.WriteLine("DONE");
            Console.Write("Initializing FAP Cost function ... ");
            EvaluationFunction = new FAPCostFunction(model.InterferenceMatrix.ToArray(),model.GeneralInformation);
            Console.WriteLine("DONE");
            Console.Write("Calculating cost ... ");
            double cost = EvaluationFunction.Evaluate(model.Cells);
            Console.WriteLine("DONE");
            Console.WriteLine("Calculated Cost : {0}", cost);
            Console.WriteLine("Target Cost: 2.200");
            Console.WriteLine("Missed Target by: {0}", cost - 2.200);
            
        }

        public Particle<ICell[]> GetGlobalBest()
        {
            return GlobalBest;
        }

        public void Start()
        {
            int i = 0;
            double fitness = 0.0;
            int j = 0;
            while (true)
            {
                foreach (Particle<ICell[]> particle in Particles)
                {
                    EvaluateParticle(particle);
                }
                UpdateSwarmMovement();
                i++;
                if (GlobalBest.Fitness != fitness)
                {
                        fitness = GlobalBest.Fitness;
                }
                else
                {
                    j++;
                    if (j == 20)
                    {
                        fakeMode = !fakeMode;
                        j = 0;
                    }
                }
                Console.WriteLine("Hashcode: {0} Fitness: {1} Using FakeGlobalBest: {2}",this.GetHashCode(),GlobalBest.Fitness,fakeMode);
            }
        }

        public void UpdateSwarmMovement()
        {
            foreach (Particle<ICell[]> particle in Particles)
            {
                VelocityFunction.MoveTowards(particle, GlobalBest);
            }
        }

        public void EvaluateParticle(Particle<ICell[]> particle)
        {
            double cost = particle.Evaluate(EvaluationFunction);
            if(IsGlobalBest(cost) && fakeMode == false)
            {
                GlobalBest = (Particle<ICell[]>)particle.Clone();
            }
        }

        #endregion

        public bool IsGlobalBest(double cost)
        {
            if (GlobalBest == null)
            {
                return true;
            }
            if (cost < GlobalBest.Fitness)
            {
                return true;
            }
            return false;
        }
    }
}
