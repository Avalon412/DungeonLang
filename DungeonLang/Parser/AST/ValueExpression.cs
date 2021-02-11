using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ValueExpression : Expression
    {
        private readonly Value _value;

        public ValueExpression(double value)
        {
            this._value = new NumberValue(value);
        }

        public ValueExpression(string value)
        {
            this._value = new StringValue(value);
        }

        public Value Evaluate()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.AsString();
        }
    }
}
