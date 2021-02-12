using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class IfStatement : Statement
    {
        private readonly Expression _expression;
        private readonly Statement _ifStatement;
        private readonly Statement _elseStatement;

        public IfStatement(Expression expression, Statement ifStatement, Statement elseStatement)
        {
            this._expression = expression;
            this._ifStatement = ifStatement;
            this._elseStatement = elseStatement;
        }

        public void Execute()
        {
            double result = _expression.Evaluate().AsNumber();
            if (result != 0)
            {
                _ifStatement.Execute();
            }
            else if (_elseStatement != null)
            {
                _elseStatement.Execute();
            }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("if ").Append(_expression).Append(_ifStatement);
            if (_elseStatement != null)
            {
                buffer.Append("\nelse").Append(_elseStatement);
            }
            return buffer.ToString();
        }
    }
}
