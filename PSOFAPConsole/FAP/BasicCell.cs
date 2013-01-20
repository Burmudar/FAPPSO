using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole.FAP
{
    public class BasicCell : ICell
    {
        private string cellID;
        private string siteName;
        private int sector;
        private double traffic;
        private Coordinate location;
        private int[] lbc; // Locally Blocked
        private int css; // Co Site Seperation
        private TRX[] frequencies;
        private FrequencyHandler frequencyHandler;
        private double interference;

        public BasicCell(string cellID)
        {
            this.cellID = cellID;
        }

        public BasicCell(string cellID,string siteName,int sector,double traffic,Coordinate location)
        {
            CellID = cellID;
            SiteName = siteName;
            Sector = sector;
            Traffic = traffic;
            Location = location;
            interference = -1;
        }

        public BasicCell(ICell cell)
        {
            BasicCell basicCell = cell as BasicCell;
            CellID = cell.CellID;
            SiteName = cell.SiteName;
            Sector = cell.Sector;
            Traffic = cell.Traffic;
            Location = cell.Location;
            CopyLocallyBlockedIfNotNull(cell);
            CopyFrequenciesIfNotNull(basicCell);
            InitializeFrequencyHandlerIfNotNullFromCell(basicCell);
            CoSiteSeperation = cell.CoSiteSeperation;
            interference = cell.Interference;
        }

        private void InitializeFrequencyHandlerIfNotNullFromCell(BasicCell basicCell)
        {
            if (basicCell.frequencyHandler != null)
            {
                frequencyHandler = CreateFrequencyHandler();
            }
        }

        private void CopyFrequenciesIfNotNull(BasicCell basicCell)
        {
            if (basicCell.Frequencies != null)
            {
                basicCell.frequencies.CopyTo(frequencies, 0);
                for (int i = 0; i < frequencies.Length; i++)
                {
                    frequencies[i] = (TRX)basicCell.frequencies[i].Clone();
                }
            }
        }

        private void CopyLocallyBlockedIfNotNull(ICell cell)
        {
            if (cell.LocallyBlocked != null)
            {
                LocallyBlocked = new int[cell.LocallyBlocked.Length];
                cell.LocallyBlocked.CopyTo(this.LocallyBlocked, 0);
            }
        }

        #region ICell Members

        public string CellID
        {
            get
            {
                return cellID;
            }
            set
            {
                cellID = value;
            }
        }

        public string SiteName
        {
            get
            {
                return siteName;
            }
            set
            {
                siteName = value;
            }
        }

        public int Sector
        {
            get
            {
                return sector;
            }
            set
            {
                sector = value;
            }
        }

        public virtual double Traffic
        {
            get
            {
                return traffic;
            }
            set
            {
                if (value != Traffic)
                {
                    frequencies = new TRX[(int)value];
                    InitializeFrequenciesTRXObjects(frequencies);
                    frequencyHandler = CreateFrequencyHandler();
                }
                traffic = value;
            }
        }

        protected FrequencyHandler CreateFrequencyHandler()
        {
            return new FrequencyHandlerWithTabu(this);
            //return new FrequencyHandler(this);
        }

        private void InitializeFrequenciesTRXObjects(TRX[] frequencies)
        {
            for (int i = 0; i < frequencies.Length; i++)
            {
                frequencies[i] = new TRX(-1,-1);
            }
        }

        public Coordinate Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        public int[] LocallyBlocked
        {
            get
            {
                return lbc;
            }
            set
            {
                lbc = value;
            }
        }

        public int CoSiteSeperation
        {
            get
            {
                return css;
            }
            set
            {
                css = value;
            }
        }

        public TRX[] Frequencies
        {
            get { return frequencies; }
            set { frequencies = value; }
        }

        public FrequencyHandler FrequencyHandler
        {
            get
            {
                return frequencyHandler;
            }
            set
            {
                frequencyHandler = value;
            }
        }

        public bool HasLocallyBlockedChannels()
        {
            if (lbc != null)
            {
                if (lbc.Length > 0)
                    return true;
            }
            return false;
        }

        public double Interference
        {
            get { return interference; }
            set { interference = value; }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new BasicCell((ICell)this);
        }

        #endregion
    }
}
