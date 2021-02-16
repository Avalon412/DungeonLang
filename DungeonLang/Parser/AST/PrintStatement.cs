using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class PrintStatement : IStatement
    {
        public readonly IExpression _expression;

        public PrintStatement(IExpression expression)
        {
            this._expression = expression;
        }

        public void Execute()
        {
            Console.WriteLine(_expression.Evaluate());
        }

        public override string ToString()
        {
            return $"print {_expression}";
        }
    }
}
