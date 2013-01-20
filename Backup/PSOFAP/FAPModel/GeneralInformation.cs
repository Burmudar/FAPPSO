using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPModel
{
    public class GeneralInformation
    {
        private string scenarioID;
        private string networkType;
        private string demandModel;
        private string annotation;
        private int[] spectrum;
        private int[] gbc;//globally blocked channels
        private int defaultCCSeperation;
        private int css;//CoSiteSeperation;
        private int siteLocations;
        private int[] handoverSeperation;
        private double minTInterference;//minimum Tolerable Interference
        private double maxTInterference;//maximum Tolerable Interference

        public GeneralInformation() 
        {
            spectrum = new int[2];
            gbc = new int[0];
            handoverSeperation = new int[0];
        }
        #region IGeneralInformation Members

        public string ScenarioID
        {
            get
            {
                return scenarioID;
            }
            set
            {
                scenarioID = value;
            }
        }

        public string NetworkType
        {
            get
            {
                return networkType;
            }
            set
            {
                networkType = value;
            }
        }

        public String Annotation
        {
            get
            {
                return annotation;
            }
            set
            {
                annotation = value;
            }
        }

        public int[] Spectrum
        {
            get
            {
                return spectrum;
            }
            set
            {
                spectrum = value;
            }
        }

        public int[] GloballyBlockedChannels
        {
            get
            {
                return gbc;
            }
            set
            {
                gbc = value;
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

        public int DefaultCoCellSeperation
        {
            get
            {
                return defaultCCSeperation;
            }
            set
            {
                defaultCCSeperation = value;
            }
        }

        public int SiteLocations
        {
            get
            {
                return siteLocations;
            }
            set
            {
                siteLocations = value;
            }
        }

        public int[] HandoverSeperation
        {
            get
            {
                return handoverSeperation;
            }
            set
            {
                handoverSeperation = value;
            }
        }

        public double MinTolerableInterference
        {
            get
            {
                return minTInterference;
            }
            set
            {
                minTInterference = value;
            }
        }

        public double MaxTolerableInterference
        {
            get
            {
                return maxTInterference;
            }
            set
            {
                maxTInterference = value;
            }
        }

        public string DemandModel
        {
            get
            {
                return demandModel;
            }
            set
            {
                demandModel = value;
            }
        }

        #endregion
    }
}
