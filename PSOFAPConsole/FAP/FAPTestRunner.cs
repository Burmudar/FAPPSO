using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAPPSO;

namespace PSOFAPConsole.FAP
{
    public class FAPTestRunner
    {
        public FAPTestRunner() 
        {
        }

        public void RunTest(String problemPath, String assignmentPath)
        {
            FAPModelFactory factory = new FAPModelFactory();
            Console.Write("Creating model from problem file given at : {0}", problemPath);
            FAPModel model = factory.CreateModel(problemPath);
            Console.WriteLine(" ... Done");
            Console.Write("Loading assignment file given at: {0}", assignmentPath);
            model.LoadTestData(assignmentPath);
            Console.WriteLine(" ... Done");
            FAPCostFunction costFunction = new FAPCostFunction(model);
            Console.Write("Calculating total amount of interference");
            double interference = costFunction.Evaluate(model.Cells);
            Console.WriteLine(" ... Done");
            Console.WriteLine("Total Interference: {0}", interference);
        }

        public void RunSiemens1Test()
        {
            FAPModelFactory factory = new FAPModelFactory();
            Console.Write("Creating model siemens1 problem");
            FAPModel model = factory.CreateSiemens1Model();
            Console.WriteLine(" ... Done");
            Console.Write("Loading assignment for siemens1");
            model.LoadTestData(@"..\..\FAP\Problems\siemens1.ass");
            Console.WriteLine(" ... Done");
            FAPCostFunction costFunction = new FAPCostFunction(model);
            Console.Write("Calculating total amount of interference");
            double interference = costFunction.Evaluate(model.Cells);
            Console.WriteLine(" ... Done");
            Console.WriteLine("Total Interference: {0}", interference);
            Console.WriteLine("Projected Interference from assignment file: {0}", 2.200);
        }

        public void RunSiemens2Test()
        {
            FAPModelFactory factory = new FAPModelFactory();
            Console.Write("Creating model siemens2 problem");
            FAPModel model = factory.CreateSiemens2Model();
            Console.WriteLine(" ... Done");
            Console.Write("Loading assignment for siemens2");
            model.LoadTestData(@"..\..\FAP\Problems\siemens2.ass");
            Console.WriteLine(" ... Done");
            FAPCostFunction costFunction = new FAPCostFunction(model);
            Console.Write("Calculating total amount of interference");
            double interference = costFunction.Evaluate(model.Cells);
            Console.WriteLine(" ... Done");
            Console.WriteLine("Total Interference: {0}", interference);
            Console.WriteLine("Projected Interference from assignment file: {0}", 2.200);
        }

        public void RunSiemens3Test()
        {
            FAPModelFactory factory = new FAPModelFactory();
            Console.Write("Creating model siemens3 problem");
            FAPModel model = factory.CreateSiemens3Model();
            Console.WriteLine(" ... Done");
            Console.Write("Loading assignment for siemens3");
            model.LoadTestData(@"..\..\FAP\Problems\siemens3.ass");
            Console.WriteLine(" ... Done");
            FAPCostFunction costFunction = new FAPCostFunction(model);
            Console.Write("Calculating total amount of interference");
            double interference = costFunction.Evaluate(model.Cells);
            Console.WriteLine(" ... Done");
            Console.WriteLine("Total Interference: {0}", interference);
            Console.WriteLine("Projected Interference from assignment file: {0}", 2.200);
        }

        public void RunSiemens4Test()
        {
            FAPModelFactory factory = new FAPModelFactory();
            Console.Write("Creating model siemens4 problem");
            FAPModel model = factory.CreateSiemens4Model();
            Console.WriteLine(" ... Done");
            Console.Write("Loading assignment for siemens4");
            model.LoadTestData(@"..\..\FAP\Problems\siemens4.ass");
            Console.WriteLine(" ... Done");
            FAPCostFunction costFunction = new FAPCostFunction(model);
            Console.Write("Calculating total amount of interference");
            double interference = costFunction.Evaluate(model.Cells);
            Console.WriteLine(" ... Done");
            Console.WriteLine("Total Interference: {0}", interference);
            Console.WriteLine("Projected Interference from assignment file: {0}", 2.200);
        }
    }
}
