using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class PrintStatement : Statement
    {
        public readonly Expression _expression;

        public PrintStatement(Expression expression)
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
