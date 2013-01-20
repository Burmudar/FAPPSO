using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.PSO
{
    public interface PSOAlgorithm<T>
    {
         void Initialize();
         void PerformCostTest();
         Particle<T> GetGlobalBest();
         void Start();
         void UpdateSwarmMovement();
         void EvaluateParticle(Particle<T> particle);
    }
}
