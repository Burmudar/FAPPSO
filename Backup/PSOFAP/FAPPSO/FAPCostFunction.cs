using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.PSO.Interfaces;
using PSOFAP.FAPModel.Interfaces;
using PSOFAP.FAPModel;

namespace PSOFAP.FAPPSO
{
    public class FAPCostFunction : IFitnessFunction<ICell[]>
    {
        public ICellRelation[] InterferenceMatrix { get; set; }
        public GeneralInformation GeneralInformation { get; set; }

        public FAPCostFunction(ICellRelation[] interferenxMatrix,GeneralInformation generalInformation)
        {
            InterferenceMatrix = interferenxMatrix;
            GeneralInformation = generalInformation;
        }

        #region IFitnessFunction<ICell[]> Members

        public double Evaluate(ICell[] position)
        {
            double costValue = calculateInterfernece(position);;
            return costValue;
        }

        private double calculateInterfernece(ICell[] position)
        {
            double totalInterference = 0;
            foreach (ICellRelation cellRelation in InterferenceMatrix)
            {
                ICell cell = position[cellRelation.CellIndex[0]];
                ICell interferingCell = position[cellRelation.CellIndex[1]];
                double interference = 0;
                for (int i = 0; i < interferingCell.Frequencies.Length; i++)
                {
                    for (int j = 0; j < cell.Frequencies.Length; j++)
                    {
                        if (SameFrequency(interferingCell.Frequencies[i],cell.Frequencies[j]))
                        {
                            interference += ZeroIfOusideInterferneceThreshold(cellRelation.DA[0]);
                        }
                        else if (frequenciesDifferByOne(interferingCell.Frequencies[i],cell.Frequencies[j]))
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

        private double ZeroIfOusideInterferneceThreshold(double value)
        {
            if (value <= GeneralInformation.MinTolerableInterference)
            {
                return 0;
            }
            return value;
        }


        private bool SameFrequency(int freqA, int freqB)
        {
            return freqA == freqB;
        }

        private bool frequenciesDifferByOne(int freqA, int freqB)
        {
            return Math.Abs(freqA - freqB) == 1;
        }
    }
}
