using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    class NumberExpression : Expression
    {
        private readonly double _value;

        public NumberExpression(double value)
        {
            this._value = value;
        }

        public double Evaluate()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
