using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP;

namespace PSOFAPConsole.FAPPSO
{
    public class FAPCostFunction : IFitnessFunction<ICell[]>
    {
        public ICellRelation[] InterferenceMatrix { get; private set; }
        public GeneralInformation GeneralInformation { get; private set; }

        public FAPCostFunction(FAPModel model)
        {
            InterferenceMatrix = model.InterferenceMatrix.ToArray();
            GeneralInformation = model.GeneralInformation;
        }

        #region IFitnessFunction<ICell[]> Members

        public virtual double Evaluate(ICell[] position)
        {
            return CalculateInterfernece(position);
        }

        protected virtual double CalculateInterfernece(ICell[] position)
        {
            double totalInterference = 0;
            
            foreach (ICellRelation cellRelation in InterferenceMatrix)
            {
                ICell cell = position[cellRelation.CellIndex[0]];
                ICell interferingCell = position[cellRelation.CellIndex[1]];
                double interference = 0;
                for (int i = 0; i < interferingCell.FrequencyHandler.Length; i++)
                {
                    for (int j = 0; j < cell.FrequencyHandler.Length; j++)
                    {
                        if (SameFrequency(interferingCell.FrequencyHandler[i],cell.FrequencyHandler[j]))
                        {
                            interference += ZeroIfOusideInterferneceThreshold(cellRelation.DA[0]);
                        }
                        else if (FrequenciesDifferByOne(interferingCell.FrequencyHandler[i],cell.FrequencyHandler[j]))
                        {
                            interference += ZeroIfOusideInterferneceThreshold(cellRelation.DA[1]);
                        }
                    }
                }
                totalInterference += interference;

            }
            return totalInterference;
        }

        #endregion

        protected double ZeroIfOusideInterferneceThreshold(double value)
        {
            if (value <= GeneralInformation.MinTolerableInterference)
            {
                return 0;
            }
            return value;
        }


        protected bool SameFrequency(int freqA, int freqB)
        {
            return freqA == freqB;
        }

        protected bool FrequenciesDifferByOne(int freqA, int freqB)
        {
            return Math.Abs(freqA - freqB) == 1;
        }
    }
}
