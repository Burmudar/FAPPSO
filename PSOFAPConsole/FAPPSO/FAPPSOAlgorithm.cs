using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.PSO;
using PSOFAPConsole.PSO.Interfaces;
using System.Diagnostics;
using PSOFAPConsole.FAPPSO;
using PSOFAPConsole.FAP;

namespace PSOFAPConsole.FAPPSO
{
    using ParticleCellArray = Particle<ICell[]>;
    using FitnessFuncCellArray = IFitnessFunction<ICell[]>;
    using ParticleMoveFunction = IMoveFunction<Particle<ICell[]>>;
    using PositionGenCellArray = IPositionGenerator<ICell[]>;
    using GlobalBestSelector = IGlobalBestSelector<Particle<ICell[]>>;
    using System.Threading.Tasks;
    using System.IO;

    public class FAPPSOAlgorithm : PSOAlgorithm<ICell[]>
    {
        public List<ParticleCellArray> Particles { get; set;}
        public ParticleCellArray GlobalBest { get; set; }
        public int Population { get; set; }
        public FitnessFuncCellArray EvaluationFunction { get; set; }
        public ParticleMoveFunction VelocityFunction { get; set; }
        public PositionGenCellArray Generator { get; set; }
        public GlobalBestSelector Selector { get; set; }
        private ICellIntegrityChecker cellIntegrityChecker;
        public String BenchName { get; set; }

        public FAPPSOAlgorithm(string benchName,int population,FitnessFuncCellArray evalFunction, ParticleMoveFunction moveFunction, PositionGenCellArray positionGenerator, ICellIntegrityChecker checker,GlobalBestSelector selector)
        {
            BenchName = benchName;
            Particles = new List<ParticleCellArray>();
            Population = population;
            EvaluationFunction = evalFunction;
            VelocityFunction = moveFunction;
            Generator = positionGenerator;
            cellIntegrityChecker = checker;
            Selector = selector;
            BenchName += "("+moveFunction.GetType().Name+")";
            BenchName += "("+selector.GetConstructionMethodName()+")";
            Console.WriteLine(BenchName);
            Initialize();
        }

        #region PSOAlgorithm<T> Members

        public void Initialize()
        {
            for (int i = 0; i < Population; i++)
            {
                Particles.Add(new Particle<ICell[]>(Generator.GeneratePosition()));
            }
        }

        public Particle<ICell[]> GetGlobalBest()
        {
            return GlobalBest;
        }

        public void Start()
        {
            DateTime start = DateTime.Now;
            String filename = "Bench-"+BenchName+"("+Population+")-" + start.ToLongDateString() + " " + start.ToLongTimeString().Replace(':','-') +"#" + this.GetHashCode() +".csv";
            Console.WriteLine("Outputting results to <{0}>", filename);
            StreamWriter swriter = new StreamWriter(new FileStream(filename, FileMode.CreateNew));
            for (int i = 0; i < 10000; i++)
            {
                Parallel.ForEach(Particles, EvaluateParticle);
                UpdateGlobalBest();
                UpdateSwarmMovement();
                swriter.WriteLine("{0},{1}", i, GlobalBest.Fitness);
                TimeSpan timespan = DateTime.Now - start;
                int duration = timespan.Duration().Minutes;
                if ( duration >= 15)
                {
                    Console.WriteLine(filename + " is at iteration " + i + " after "+duration+" mins");
                }
                if (duration >= 15 && i >= 20)
                {
                    Console.WriteLine(filename + " -- finished -- 15 Minute mark and iteration is greater 20 -- fitness:" + GlobalBest.Fitness + " with " + cellIntegrityChecker.CountViolations(GlobalBest.Position) + " violations");
                    break;
                }
            }
            swriter.Close();
        }

        private void UpdateGlobalBest()
        {
            GlobalBest = Selector.FindGlobalBest(Particles);
            GlobalBest.Evaluate(EvaluationFunction);
        }

        //Applies the velocity function to each particle in the swarm
        public void UpdateSwarmMovement()
        {
            Parallel.ForEach(Particles, particle => particle.MoveTowards(GlobalBest,VelocityFunction));
        }

        public void EvaluateParticle(Particle<ICell[]> particle)
        {
            double cost = particle.Evaluate(EvaluationFunction);
        }

        #endregion
    }
}
