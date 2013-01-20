using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSOFAPConsole.FAP
{
    public class RandomCollisionResolver : AbstractCollisionResolver
    {

        public RandomCollisionResolver(int[] Channels) : base(Channels) { }

        public override void ResolveCollisions(FrequencyHandler freqHandler)
        {
            if (hasLBC(freqHandler))
            {
                ResolveLBCCollisions(freqHandler);
            }
            if (freqHandler is FrequencyHandlerWithTabu)
            {
                FrequencyHandlerWithTabu frequencyHandler = freqHandler as FrequencyHandlerWithTabu;

                foreach (TRXTabu tabu in frequencyHandler.TabuList)
                {
                    var collisions = FindCollisionsForTabuItem(frequencyHandler, tabu);

                    HandleCollisions(collisions, frequencyHandler);
                }
            }
            else
                return;

            
        }

        private bool hasLBC(FrequencyHandler freqHandler)
        {
            return freqHandler.GetParentCell().HasLocallyBlockedChannels();
        }

        private void ResolveLBCCollisions(FrequencyHandler freqHandler)
        {
            Random r = new Random();
            int count = 0;
            for (int i = 0; i < freqHandler.Length; i++)
            {
                int channelValue = Channels[freqHandler[i]];
                if (freqHandler.GetParentCell().LocallyBlocked.Contains(channelValue))
                {
                    freqHandler[i] = r.Next(0, Channels.Length);
                    count++;
                    if (count <= 10)
                    {
                        i--;
                    }
                }
                else
                {
                    count = 0;
                }
            }
        }

        private static IEnumerable<int> FindCollisionsForTabuItem(FrequencyHandlerWithTabu frequencyHandler, TRXTabu tabu)
        {
            var collisions = frequencyHandler.GetFrequencyArray().Select((freq, freqIndex) =>
            {
                if ((freq.Value == tabu.TRX.Value) && (freqIndex == tabu.Index))
                {
                    return freqIndex;
                }
                return -1;
            }
            );
            return collisions;
        }

        private void HandleCollisions(IEnumerable<int> collisions, FrequencyHandlerWithTabu frequencyHandler)
        {
            TRX[] frequencies = frequencyHandler.GetFrequencyArray();
            Parallel.ForEach(collisions, collisionIndex =>
            {
                if (collisionIndex > -1)
                {
                    int newFrequency = RandomizeChannelIndex(frequencies[collisionIndex], frequencyHandler.TabuList);
                    frequencies[collisionIndex] = new TRX(newFrequency);
                }
            }
            );
        }

        private int RandomizeChannelIndex(TRX tRX, Queue<TRXTabu> tabuList)
        {
            bool isUnique = false;
            Random r = new Random();
            int uniqueIndex = 0;
            int count = 0;
            while (isUnique == false)
            {
                int randomIndex = r.Next(0, Channels.Length);
                if ((randomIndex != tRX.Value) && (tabuList.Contains(new TRXTabu(new TRX(randomIndex), -1)) == false))
                {
                    uniqueIndex = randomIndex;
                    isUnique = true;
                }
                count++;
                if (count > 50)
                {
                    uniqueIndex = randomIndex;
                    isUnique = true;
                }
            }
            System.Threading.Thread.Sleep(10);
            return uniqueIndex;
        }
    }
    
}
