using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class AssignmentStatement : IStatement
    {
        private readonly string _variable;
        private readonly IExpression _expression;

        public AssignmentStatement(string variable, IExpression expression)
        {
            this._variable = variable;
            this._expression = expression;
        }

        public void Execute()
        {
            IValue result = _expression.Evaluate();
            Variables.Set(_variable, result);
        }

        public override string ToString()
        {
            return $"{_variable} = {_expression}";
        }
    }
}
