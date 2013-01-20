using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPModel
{
    public class Cell : ICell
    {
        private string cellID;
        private string siteName;
        private int sector;
        private double traffic;
        private int[] frequencies;
        private Coordinate location;
        private int[] lbc; // Locally Blocked
        private int css; // Co Site Seperation

        public Cell(string cellID)
        {
            CellID = cellID;
        }

        public Cell(string cellID,string siteName,int sector,double traffic,Coordinate location)
        {
            CellID = cellID;
            SiteName = siteName;
            Sector = sector;
            Traffic = traffic;
            Location = location;
        }

        public Cell(ICell cell)
        {
            CellID = cell.CellID;
            SiteName = cell.SiteName;
            Sector = cell.Sector;
            Traffic = cell.Traffic;
            Location = cell.Location;
            LocallyBlocked = cell.LocallyBlocked;
            CoSiteSeperation = cell.CoSiteSeperation;
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

        public double Traffic
        {
            get
            {
                return traffic;
            }
            set
            {
                traffic = value;
                frequencies = new int[(int)traffic];
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

        public bool hasLocallyBlockedChannels()
        {
            if (lbc != null)
            {
                if (lbc.Length > 0)
                    return true;
            }
            return false;
        }

        public int[] Frequencies
        {
            get
            {
                return frequencies;
            }
            set
            {
                frequencies = value;
            }
        }

        public int getFrequencyAt(int i)
        {
            return frequencies[i];
        }

        public void setFrequencyAt(int i,int value)
        {
            frequencies[i] = value;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            Cell cloned = new Cell(this.CellID, this.SiteName, this.Sector, this.Traffic, this.Location);
            if (this.LocallyBlocked != null)
            {
                this.LocallyBlocked.CopyTo(cloned.LocallyBlocked, 0);
            }
            this.Frequencies.CopyTo(cloned.Frequencies, 0);
            return cloned;
        }

        #endregion
    }
}
