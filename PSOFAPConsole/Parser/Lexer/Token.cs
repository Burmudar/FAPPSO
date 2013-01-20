using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.Parser.Lexer
{

    public enum Tag
    {
        //General Information
        FORMAT_START,FORMAT_TYPE, FORMAT_VERSION,FORMAT_END,GENERATION_INFORMATION_START, SCENARIO_ID,NAME, ANNOTATION, NETWORK_TYPE, SPECTRUM, GLOBALLY_BLOCKED_CHANNELS,
        CO_SITE_SEPARATION, DEFAULT_CO_CELL_SEPARATION, HANDOVER_SEPARATION, MINIMAL_SIGNIFICANT_INTERFERENCE, MAXIMUM_SIGNIFICANT_INTERFERENCE, DEMAND_MODEL, SITE_LOCATIONS, GENERAL_INFORMATION_END, 
        //Cells
        CELLS_START,CELL_INNER_START, CELL_ID, CELL_SITENAME,CELL_SECTOR,CELL_TRAFFIC,CELL_LOCATION,CELL_LOCALLY_BLOCKED,CELL_CO_SITE_SEPARATION,CELLS_END,CELL_INNER_END,CELL_ASSIGNMENT,
        //Cell_RELATION
        CELL_RELATION_START, CELL_RELATION_IDS, DA, UA, DT, UT, CELLRELATION_HANDOVER, CELLRELATION_SEPARATION, CELL_RELATION_INNER_END, CELL_RELATION_END,
        //
        END
    }

    public class Token
    {
        public Tag Tag { get; set; }

        public Token(Tag tag)
        {
            Tag = tag;
        }

        public override String ToString()
        {
            return Tag.ToString();
        }

    }

    public class StringBlock : Token
    {
        public String value { get; set; }

        public StringBlock(Tag tag,String  value) : base(tag)
        {
            this.value = value;
        }
    }

    public class Num : Token
    {
        public int Value { get; set; }

        public Num(Tag tag, int value) : base (tag)
        {
            this.Value = value;
        }

        public Num(Tag tag, String value)
            : base(tag)
        {
            this.Value = int.Parse(value);
        }
    }

    public class NumArray : Token
    {
        public int[] Value { get; set; }

        public NumArray(Tag tag,int[] value) : base(tag)
        {
            this.Value = value;
        }

        public NumArray(Tag tag, String value, char splitChar)
            : base(tag)
        {
            String[] values = value.Split(splitChar);
            this.Value = values.Select(x => int.Parse(x)).ToArray();
        }
    }

    public class Real : Token
    {
        public double Value { get; set; }

        public Real(Tag tag, String value)
            : base(tag)
        {
            this.Value = double.Parse(value);
        }

        public Real(Tag tag,double value) : base(tag)
        {
            this.Value = value;
        }
    }

    public class RealArray : Token
    {
        public double[] Value { get; set; }

        public RealArray(Tag tag, String value, char splitChar):base(tag)
        {
            String[] values = value.Split(splitChar);
            this.Value = values.Select(x => double.Parse(x)).ToArray();
        }

        public RealArray(Tag tag, double[] value) : base(tag)
        {
            this.Value = value;
        }
    }
}
