using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PSOFAPConsole.FAPPSO;
using PSOFAPConsole.FAP;
using PSOFAPConsole.PSO;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread((ThreadStart)delegate
            {
                BenchRunner benchmarkRunner = new BenchRunner();
                benchmarkRunner.StartBenchmark();
            });           
            t1.Start();
        }
    }
}
