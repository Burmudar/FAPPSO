using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPModel
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

                if (cellA.Frequencies.Length <= cellB.Frequencies.Length)
                {
                    for (int i = 0; i < cellA.Frequencies.Length;i++ )
                    {
                        diff = Math.Abs(cellA.Frequencies[i] - cellB.Frequencies[i]);
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
                    for (int i = 0; i < cellB.Frequencies.Length; i++)
                    {
                        diff = Math.Abs(cellA.Frequencies[i] - cellB.Frequencies[i]);
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
