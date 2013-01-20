using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP;
using PSOFAPConsole.FAP.Interfaces;
using System.Threading.Tasks;

namespace PSOFAPConsole.FAPPSO
{
    public class FrequencyIndexPositionGenerator: IPositionGenerator<ICell[]>
    {
        private ICell[] Cells;
        private int[] Channels;

        public FrequencyIndexPositionGenerator(FAPModel model)
        {
            Cells = model.Cells;
            Channels = model.Channels;
        }

        public ICell[] GeneratePosition()
        {
            ICell[] position = new ICell[Cells.Length];
            for (int i = 0; i < position.Length; i++)
            {
                position[i] = new BasicCell(Cells[i]);
                GenerateUniqueFrequencyIndexes(position[i].FrequencyHandler);
            }
            return position;
        }

        private void GenerateUniqueFrequencyIndexes(FrequencyHandler FrequencyHandler)
        {
            Random random = new Random();
            for (int i = 0; i < FrequencyHandler.Length; i++)
            {
                int channelIndex = random.Next(Channels.Length);
                if (FrequencyHandler.Contains(channelIndex))
                {
                    i--;
                    continue;
                }
                else
                {
                    FrequencyHandler[i] = channelIndex;
                }
            }
            FrequencyHandler.MigrateFrequenciesToParent();
        }

    }
}
