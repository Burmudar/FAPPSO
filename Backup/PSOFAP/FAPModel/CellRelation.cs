using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPModel
{
    public class CellRelation : ICellRelation
    {
        private int[] cellIndex;
        private int handover;
        private int separation;
        private double[] downlinkArea;
        private double[] uplinkArea;
        private double[] downlinkTraffic;
        private double[] uplinkTraffic;

        public CellRelation(int cellIndex1, int cellIndex2)
        {
            CellIndex = new int[2] { cellIndex1, cellIndex2 };
        }

        public CellRelation(int[] cellIndex)
        {
            CellIndex = cellIndex;
        }

        public CellRelation(int cellIndex1, int cellIndex2, int handover, int separation)
        {
            CellIndex = new int[2] { cellIndex1, cellIndex2 };
            Handover = handover;
            Separation = separation;
        }

        #region ICellRelation Members

        public int[] CellIndex
        {
            get
            {
                return cellIndex;
            }
            set
            {
                cellIndex = value;
            }
        }

        public int Handover
        {
            get
            {
                return handover;
            }
            set
            {
                handover = value;
            }
        }

        public int Separation
        {
            get
            {
                return separation;
            }
            set
            {
                separation = value;
            }
        }

        public double[] DA
        {
            get
            {
                return downlinkArea;
            }
            set
            {
                downlinkArea = value;
            }
        }

        public double[] UA
        {
            get
            {
                return uplinkArea;
            }
            set
            {
                uplinkArea = value;
            }
        }

        public double[] DT
        {
            get
            {
                return downlinkTraffic;
            }
            set
            {
                downlinkTraffic = value;
            }
        }

        public double[] UT
        {
            get
            {
                return uplinkArea;
            }
            set
            {
                uplinkArea = value;
            }
        }

        #endregion
    }
}
