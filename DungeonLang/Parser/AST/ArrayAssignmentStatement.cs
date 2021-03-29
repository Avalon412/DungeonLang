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
        public readonly IExpression _expression;
        public readonly ArrayAccessExpression _array;

        public ArrayAssignmentStatement(ArrayAccessExpression array, IExpression expression)
        {
            this._expression = expression;
            this._array = array;
        }

        public void Execute()
        {
            _array.GetArray().Set(_array.LastIndex(), _expression.Evaluate());
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{_array} = {_expression}";
        }
    }
}
