using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP;
using PSOFAPConsole.PSO;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole.FAPPSO
{
    using ParticleCellArray = Particle<ICell[]>;
    using FitnessFuncCellArray = IFitnessFunction<ICell[]>;
    using ParticleMoveFunction = IMoveFunction<Particle<ICell[]>>;
    using PositionGenCellArray = IPositionGenerator<ICell[]>;
    using PSOFAPConsole.FAPPSO.Functions;

    public class FAPPSOFactory
    {


        public FAPPSOFactory()
        {
        }

        public PSOAlgorithm<ICell[]> CreateFrequencyValueBased(int Population,FAPModel Model,double localCoefficient, double globalCoefficient)
        {
            PositionGenCellArray generator = new FrequencyPositionGenerator(Model);
            FitnessFuncCellArray evalFunction = new FAPCostFunction(Model);
            ParticleMoveFunction moveFunction = new ParticlePerTrxFunction(Model, Model.GeneralInformation.Spectrum[0], Model.GeneralInformation.Spectrum[1],
                 localCoefficient, globalCoefficient, CreateCollisionResolver(Model));
            ICellIntegrityChecker checker = new GBCViolationChecker(Model.GeneralInformation.GloballyBlockedChannels);
            String benchName = Model.GeneralInformation.ScenarioID;
            return new FAPPSOAlgorithm(benchName,Population, evalFunction, moveFunction, generator, checker,GBestFactory.GetStandardSelector());
        }

        public PSOAlgorithm<ICell[]> CreateFrequencyIndexBased(int Population, FAPModel Model, double localCoefficient, double globalCoefficient)
        {
            PositionGenCellArray generator = new FrequencyIndexPositionGenerator(Model);
            FitnessFuncCellArray evalFunction = new FAPIndexCostFunction(Model);
            ParticleMoveFunction moveFunction = new ParticlePerTrxFunction(Model, 0, Model.Channels.Length - 1, localCoefficient, globalCoefficient, CreateCollisionResolver(Model));
            ICellIntegrityChecker checker = new GBCIndexBasedViolationChecker(Model);
            String benchName = Model.GeneralInformation.ScenarioID;
            return new FAPPSOAlgorithm(benchName, Population, evalFunction, moveFunction, generator, checker, GBestFactory.GetStandardSelector());
        }

        public PSOAlgorithm<ICell[]> CreateIndexMovementBased(int Population, FAPModel Model, double localCoefficient, double globalCoefficient)
        {
            PositionGenCellArray generator = new FrequencyIndexPositionGenerator(Model);
            FitnessFuncCellArray evalFunction = new FAPIndexCostFunction(Model);
            ParticleMoveFunction moveFunction = new PerTRXChannelIndexFunction(Model, localCoefficient, globalCoefficient, CreateCollisionResolver(Model));
            ICellIntegrityChecker checker = new GBCIndexBasedViolationChecker(Model);
            String benchName = Model.GeneralInformation.ScenarioID;
            return new FAPPSOAlgorithm(benchName, Population, evalFunction, moveFunction, generator, checker, GBestFactory.GetStandardSelector());
        }

        public PSOAlgorithm<ICell[]> CreateIndexBasedFAPPSOWithGlobalBestCellBuilder(int Population, FAPModel Model, double localCoefficient, double globalCoefficient)
        {
            PositionGenCellArray generator = new FrequencyIndexPositionGenerator(Model);
            FitnessFuncCellArray evalFunction = new FAPIndexCostFunction(Model);
            ParticleMoveFunction moveFunction = new ParticlePerTrxFunction(Model, 0, Model.Channels.Length - 1, localCoefficient, globalCoefficient, CreateCollisionResolver(Model));
            ICellIntegrityChecker checker = new GBCIndexBasedViolationChecker(Model);
            String benchName = Model.GeneralInformation.ScenarioID;
            return new FAPPSOAlgorithm(benchName, Population, evalFunction, moveFunction, generator, checker, GBestFactory.GetGlobalBestCellBuilderSelector());
        }

        public PSOAlgorithm<ICell[]> CreateIndexMovementBasedWithGlobalBestCellBuilder(int Population, FAPModel Model, double localCoefficient, double globalCoefficient)
        {
            PositionGenCellArray generator = new FrequencyIndexPositionGenerator(Model);
            FitnessFuncCellArray evalFunction = new FAPIndexCostFunction(Model);
            ParticleMoveFunction moveFunction = new PerTRXChannelIndexFunction(Model, localCoefficient, globalCoefficient, CreateCollisionResolver(Model));
            ICellIntegrityChecker checker = new GBCIndexBasedViolationChecker(Model);
            String benchName = Model.GeneralInformation.ScenarioID;
            return new FAPPSOAlgorithm(benchName, Population, evalFunction, moveFunction, generator, checker, GBestFactory.GetGlobalBestCellBuilderSelector());
        }

        public PSOAlgorithm<ICell[]> CreateIndexBasedFAPPSOWithGlobalBestTRXBuilder(int Population, FAPModel Model, double localCoefficient, double globalCoefficient)
        {
            PositionGenCellArray generator = new FrequencyIndexPositionGenerator(Model);
            FitnessFuncCellArray evalFunction = new FAPIndexCostFunction(Model);
            ParticleMoveFunction moveFunction = new ParticlePerTrxFunction(Model, 0, Model.Channels.Length - 1, localCoefficient, globalCoefficient, CreateCollisionResolver(Model));
            ICellIntegrityChecker checker = new GBCIndexBasedViolationChecker(Model);
            String benchName = Model.GeneralInformation.ScenarioID;
            return new FAPPSOAlgorithm(benchName, Population, evalFunction, moveFunction, generator, checker, GBestFactory.GetGlobalBestTRXBuilderSelector());
        }

        public PSOAlgorithm<ICell[]> CreateIndexMovementBasedWithGlobalBestTRXBuilder(int Population, FAPModel Model, double localCoefficient, double globalCoefficient)
        {
            PositionGenCellArray generator = new FrequencyIndexPositionGenerator(Model);
            FitnessFuncCellArray evalFunction = new FAPIndexCostFunction(Model);
            ParticleMoveFunction moveFunction = new PerTRXChannelIndexFunction(Model, localCoefficient,globalCoefficient,CreateCollisionResolver(Model));
            ICellIntegrityChecker checker = new GBCIndexBasedViolationChecker(Model);
            String benchName = Model.GeneralInformation.ScenarioID;
            return new FAPPSOAlgorithm(benchName, Population, evalFunction, moveFunction, generator, checker, GBestFactory.GetGlobalBestTRXBuilderSelector());
        }

        protected AbstractCollisionResolver CreateCollisionResolver(FAPModel model)
        {
            return new RandomCollisionResolver(model.Channels);
        }

    }
}
