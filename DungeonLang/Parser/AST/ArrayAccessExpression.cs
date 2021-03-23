using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ArrayAccessExpression : IExpression
    {
        private readonly string _variable;
        private readonly IExpression _index;

        public ArrayAccessExpression(string variable, IExpression index)
        {
            this._variable = variable;
            this._index = index;
        }

        public IValue Evaluate()
        {
            IValue var = Variables.Get(_variable);
            if (var is ArrayValue)
            {
                ArrayValue array = (ArrayValue)var;
                return array.Get((int)_index.Evaluate().AsNumber());
            }
            else
            {
                throw new RuntimeExpression("Array expected");
            }
        }

        public override string ToString()
        {
            return $"{_variable}[{_index}]";
        }
    }
}
