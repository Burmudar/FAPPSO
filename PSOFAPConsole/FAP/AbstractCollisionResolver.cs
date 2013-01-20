using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole.FAP
{
    public abstract class AbstractCollisionResolver
    {
        protected int[] Channels { get; set; }

        public AbstractCollisionResolver(int[] channels)
        {
            Channels = channels;
        }

        public abstract void ResolveCollisions(FrequencyHandler frequencyHandler);

    }
}
