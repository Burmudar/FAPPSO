using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP
{
    public class TRX : ICloneable,IEquatable<TRX>
    {
        public int Value { get; set; }
        public double Interference { get; set; }

        public TRX()
        {
            Value = 0;
            Interference = 0;
        }

        public TRX(int value)
        {
            Value = value;
        }

        public TRX(int value, double interference)
        {
            Value = value;
            Interference = interference;
        }

        public object Clone()
        {
            return new TRX(Value, Interference);
        }

        public bool Equals(TRX other)
        {
            return Value == other.Value;
        }
    }
}
