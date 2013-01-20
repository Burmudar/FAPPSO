using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.PSO.Interfaces;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP;

namespace PSOFAPConsole.FAPPSO
{
    public class FrequencyPositionGenerator : IPositionGenerator<ICell[]>
    {
        private ICell[] Cells;
        private int[] Spectrum;
        private int[] Channels;

        public FrequencyPositionGenerator(FAPModel model)
        {
            Spectrum = model.GeneralInformation.Spectrum;
            Cells = model.Cells;
            Channels = model.Channels;
        }

        #region IPositionGenerator<ICell> Members

        public ICell[] GeneratePosition()
        {
            ICell[] position = new ICell[Cells.Length];
            for (int i = 0; i < Cells.Length; i++)
            {
                position[i] = new BasicCell(Cells[i]);
                InsertUniqueChannels(position[i].FrequencyHandler);
            }
            return position;
        }



        #endregion

        private void InsertUniqueChannels(FrequencyHandler frequencies)
        {
            Random random = new Random();
            int index = random.Next(0, Channels.Length);
            for (int i = 0; i < frequencies.Length; i++)
            {
                while (ContainsNumber(Channels[index], frequencies))
                {
                    index = random.Next(0, Channels.Length);
                }
                frequencies[i] = Channels[index];
            }
        }

        private bool ContainsNumber(int number, FrequencyHandler array)
        {
            foreach (int i in array)
            {
                if (number == i)
                    return true;
            }
            return false;
        }
    }
}
