using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.PSO.Interfaces;
using PSOFAP.FAPModel.Interfaces;
using PSOFAP.FAPModel;

namespace PSOFAP.FAPPSO
{
    public class FAPPositionGenerator : IPositionGenerator<ICell[]>
    {
        private ICell[] Cells;
        private int[] Spectrum;
        public int[] Channels { get; set;}
        public FAPPositionGenerator(FAPModel.FAPModel model)
        {
            Spectrum = model.GeneralInformation.Spectrum;
            Cells = model.Cells;
            if (model.GeneralInformation.GloballyBlockedChannels.Length > 0)
            {
                generateChannelsWithGBC(model.GeneralInformation.GloballyBlockedChannels);
            }
            else
                generateChannels();
        }

        private void generateChannelsWithGBC(int[] GBC)
        {
            if(GBC == null)
                throw new ArgumentNullException();
            if(GBC.Length < 1)
                throw new ArgumentException("If there are no GloballyBlocked Channels then rather use the normal generateCHannels()");
            int channel = Spectrum[0];
            int channelIndex = 0;
            int gbcIndex = 0;
            Channels = new int[(Spectrum[1] - Spectrum[0])+1 - GBC.Length];
            while (channel <= Spectrum[1])
            {
                if (channel == GBC[gbcIndex])
                {
                    if (gbcIndex + 1 < GBC.Length)
                    {
                        gbcIndex++;
                    }
                }
                else
                {
                    Channels[channelIndex] = channel;
                    channelIndex++;
                }
                channel++;
            }
        }

        private void generateChannels()
        {
            Channels = new int[Spectrum[1] - Spectrum[0]];
            int channel = Spectrum[0];
            for(int i = 0 ; i < Channels.Length; i++)
            {
                Channels[i] = channel++;
            }
        }

        #region IPositionGenerator<ICell> Members

        public ICell[] GeneratePosition()
        {
            Random random = new Random();
            ICell[] position = new ICell[Cells.Length];
            for (int i = 0; i < Cells.Length; i++ )
            {
                position[i] = new Cell(Cells[i]);
                InsertUniqueChannels(position[i].Frequencies, random);
            }
            return position;
        }

        

        #endregion

        private void InsertUniqueChannels(int[] frequencies, Random random)
        {
            int index = random.Next(0,Channels.Length);
            for (int i = 0; i < frequencies.Length; i++)
            {
                while (ContainsNumber(Channels[index], frequencies))
                {
                    index = random.Next(0, Channels.Length);
                }
                frequencies[i] = Channels[index];
            }
        }

        private bool ContainsNumber(int number, int[] array)
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
