using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.PSO
{
    public interface PSOAlgorithm<T>
    {
         void Initialize();
         Particle<T> GetGlobalBest();
         void Start();
         void UpdateSwarmMovement();
         void EvaluateParticle(Particle<T> particle);
    }
}
