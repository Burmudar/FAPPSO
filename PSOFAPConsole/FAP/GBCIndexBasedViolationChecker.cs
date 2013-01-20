using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole.FAP
{
    public class GBCIndexBasedViolationChecker : ICellIntegrityChecker
    {
        private readonly int[] channels;
        private readonly int[] gbc;

        public GBCIndexBasedViolationChecker(FAPModel model)
        {
            channels = model.Channels;
            gbc = model.GeneralInformation.GloballyBlockedChannels;
        }

        public int CountViolations(ICell[] cells)
        {
            int violations = 0;
            foreach (ICell cell in cells)
            {
                violations += CountViolations(cell);
            }
            return violations;
        }

        public int CountViolations(ICell cell)
        {
            int violations = 0;
            foreach (int channelIndex in cell.FrequencyHandler)
            {
                if (IsGloballyBlocked(channels[channelIndex]))
                {
                    violations++;
                }
            }

            return violations;
        }

        private bool IsGloballyBlocked(int frequency)
        {
            foreach (int blockedFrequency in gbc)
            {
                if (frequency == blockedFrequency)
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}
