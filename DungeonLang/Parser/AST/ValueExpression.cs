using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ValueExpression : IExpression
    {
        private readonly IValue _value;

        public ValueExpression(double value)
        {
            this._value = new NumberValue(value);
        }

        public ValueExpression(string value)
        {
            this._value = new StringValue(value);
        }

        public IValue Evaluate()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.AsString();
        }
    }
}
