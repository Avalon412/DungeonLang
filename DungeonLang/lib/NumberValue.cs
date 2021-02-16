using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class NumberValue : IValue
    {
        private readonly double _value;
        public static readonly NumberValue ZERO = new NumberValue(0);

        public NumberValue(double value)
        {
            this._value = value;
        }

        public NumberValue(bool value)
        {
            this._value = value ? 1 : 0;
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
