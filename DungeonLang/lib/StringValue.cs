using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class StringValue : IValue
    {
        private readonly string _value;

        public StringValue(string value)
        {
            this._value = value;
        }

        public double AsNumber()
        {
            try
            {
                return double.Parse(_value);
            } catch (FormatException ex)
            {
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }

        public string AsString()
        {
            return _value;
        }

        public override string ToString()
        {
            return AsString();
        }
    }
}
