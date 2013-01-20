using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole.FAP
{
    public class FAPObjectiveFunction
    {
        public ICell[] Cells { get; set; }
        public LinkedList<ICellRelation> InterferenceMatrix { get; set; }
        public FAPObjectiveFunction(ICell[] cells, LinkedList<ICellRelation> interferenceMatrix)
        {
            Cells = cells;
            InterferenceMatrix = interferenceMatrix;
        }

        public double calculate()
        {
            int diff = 0;
            double value = 0;
            foreach (ICellRelation cellRelation in InterferenceMatrix)
            {
                ICell cellA = Cells[cellRelation.CellIndex[0]];
                ICell cellB = Cells[cellRelation.CellIndex[1]];

                if (cellA.FrequencyHandler.Length <= cellB.FrequencyHandler.Length)
                {
                    for (int i = 0; i < cellA.FrequencyHandler.Length;i++ )
                    {
                        diff = Math.Abs(cellA.FrequencyHandler[i] - cellB.FrequencyHandler[i]);
                        if (diff == 1)
                        {
                            value += cellRelation.DA[1];
                        }
                        else if (diff == 0)
                        {
                            value += cellRelation.DA[0];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cellB.FrequencyHandler.Length; i++)
                    {
                        diff = Math.Abs(cellA.FrequencyHandler[i] - cellB.FrequencyHandler[i]);
                        if (diff == 1)
                        {
                            value += cellRelation.DA[1];
                        }
                        else if (diff == 0)
                        {
                            value += cellRelation.DA[0];
                        }
                    }
                }
            }
            return value;
        }
    }
}
