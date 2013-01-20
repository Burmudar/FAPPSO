using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP;

namespace PSOFAPConsole.FAPPSO
{
    public class GBCViolationChecker : ICellIntegrityChecker
    {
        protected int[] GBC { get; set; }

        public GBCViolationChecker(int[] gbc)
        {
            GBC = gbc;
        }

        public int CountViolations(ICell[] cells)
        {
            int violations = 0;
            foreach (ICell item in cells)
            {
                violations += CountViolations(item);
            }
            return violations;
        }

        public int CountViolations(ICell cell)
        {
            int violations = 0;
            foreach (int trx in cell.FrequencyHandler)
            {
                if (IsGloballyBlocked(trx))
                    violations++;
            }
            return violations;
        }

        private bool IsGloballyBlocked(int frequency)
        {
            foreach (int gbc in GBC)
            {
                if (frequency == gbc)
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}
