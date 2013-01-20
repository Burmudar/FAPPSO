using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP
{
    class ChannelGenerator
    {
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
        public int[] BlockedChannels { get; set; }

        public ChannelGenerator(int low, int high,int[] blockedChannels)
        {
            LowerBound = low;
            UpperBound = high;
            BlockedChannels = blockedChannels;
        }

        public int[] Generate()
        {
            int[] Channels = new int[(UpperBound - LowerBound) - BlockedChannels.Length];
            int blockedArrayIndex = 0;
            int channelIndex = 0;
            int channelValue = LowerBound;
            while(channelIndex != Channels.Length)
            {
                if (IsBlocked(channelValue, blockedArrayIndex))
                {
                    blockedArrayIndex++;
                }
                else
                {
                    Channels[channelIndex] = channelValue;
                    channelIndex++;
                }
                channelValue++;
            }
            return Channels;
        }

        protected bool IsBlocked(int channelValue, int blockedArrayIndex)
        {
            if (blockedArrayIndex < BlockedChannels.Length)
            {
                return channelValue == BlockedChannels[blockedArrayIndex];
            }
            return false;
        }
    }
}
