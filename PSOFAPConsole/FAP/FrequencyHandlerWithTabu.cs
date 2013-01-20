using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole.FAP
{
    public class FrequencyHandlerWithTabu : FrequencyHandler
    {
        public Queue<TRXTabu> TabuList {get; set;}
        public FrequencyHandlerWithTabu(BasicCell parent)
            : base(parent)
        {
            TabuList = new Queue<TRXTabu>();
        }

        public FrequencyHandlerWithTabu(FrequencyHandlerWithTabu frequencyHandler)
            : base(frequencyHandler)
        {
            TabuList = new Queue<TRXTabu>(frequencyHandler.TabuList);
        }

        public override void MigrateFrequenciesToParent()
        {
            UpdateTabuList();
            for (int i = 0; i < frequencies.Length; i++)
            {
                TRX trx = (TRX)frequencies[i].Clone();
                TabuList.Enqueue(new TRXTabu(trx,i));
                parentCell.Frequencies[i] = trx;
            }
        }

        private void UpdateTabuList()
        {
            if (TabuList.Count > 20)
            {
                TabuList.Dequeue();
            }
        }
    }
}
