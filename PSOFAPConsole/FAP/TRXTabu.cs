using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP
{
    public class TRXTabu
    {
        public TRX TRX { get; set; }
        public int Index { get; set;}

        public TRXTabu(TRX trx, int index)
        {
            TRX = trx;
            Index = index;
        }

        public override bool Equals(object obj)
        {
            if (obj is int)
            {
                return ((int)obj) == TRX.Value;
            }
            else if (obj is TRXTabu)
            {
                return ((TRXTabu)obj).TRX.Value == TRX.Value;
            }
            else
                return false;
        }
    }
}
