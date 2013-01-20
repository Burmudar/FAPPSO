using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAPConsole.FAP.Interfaces;

namespace PSOFAPConsole.FAP
{
    public class FrequencyHandler : IEnumerable<int>
    {
        protected BasicCell parentCell;
        protected TRX[] frequencies;

        public FrequencyHandler(BasicCell parent)
        {
            parentCell = parent;
            frequencies = new TRX[parent.Frequencies.Length];
            parent.Frequencies.CopyTo(frequencies, 0);
        }

        public FrequencyHandler(FrequencyHandler handler)
        {
            parentCell = handler.parentCell;
            frequencies = new TRX[handler.frequencies.Length];
            handler.frequencies.CopyTo(frequencies,0);
        }

        public virtual void MigrateFrequenciesToParent()
        {
            for (int i = 0; i < frequencies.Length; i++)
            {
                TRX trx = (TRX)frequencies[i].Clone();
                parentCell.Frequencies[i] = trx;
            }
        }

        public ICell GetParentCell()
        {
            return parentCell;
        }

        public TRX[] GetFrequencyArray()
        {
            return frequencies;
        }

        public void SetSingleTrxInterference(int index, double interference)
        {
            frequencies[index].Interference += interference;
        }

        public int this[int index]
        {
            get { return frequencies[index].Value; }
            set 
            {
               frequencies[index].Value = value;
            }
        }

        public int Length { get { return frequencies.Length; } }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (TRX i in frequencies)
            {
                yield return i.Value;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return frequencies.GetEnumerator();
        }
    }
}
