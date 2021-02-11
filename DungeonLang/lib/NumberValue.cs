using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class NumberValue : Value
    {
        private readonly double _value;

        public NumberValue(double value)
        {
            this._value = value;
        }

        public double Evaluate()
        {
            return _value;
        }

        public string AsString()
        {
            return _value.ToString();
        }

        public double AsNumber()
        {
            return _value;
        }

        public override string ToString()
        {
            return AsString();
        }
    }
}
