using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ArrayAssignmentStatement : IStatement
    {
        private readonly string _variable;
        private readonly IExpression _index;
        private readonly IExpression _expression;

        public ArrayAssignmentStatement(string variable, IExpression index, IExpression expression)
        {
            this._variable = variable;
            this._index = index;
            this._expression = expression;
        }

        public void Execute()
        {
            IValue variable = Variables.Get(_variable);
            if (variable is ArrayValue)
            {
                ArrayValue array = (ArrayValue)variable;
                array.Set((int)_index.Evaluate().AsNumber(), _expression.Evaluate());
            }
            else
            {
                throw new RuntimeExpression("Array expected");
            }
        }

        public override string ToString()
        {
            return $"{_variable}[{_index}] = {_expression}";
        }
    }
}
