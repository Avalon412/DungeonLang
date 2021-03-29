using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class IfStatement : IStatement
    {
        public readonly IExpression _expression;
        public readonly IStatement _ifStatement;
        public readonly IStatement _elseStatement;

        public IfStatement(IExpression expression, IStatement ifStatement, IStatement elseStatement)
        {
            this._expression = expression;
            this._ifStatement = ifStatement;
            this._elseStatement = elseStatement;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
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
