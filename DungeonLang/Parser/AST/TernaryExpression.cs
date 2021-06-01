using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class TernaryExpression : IExpression
    {
        public readonly IExpression _condition;
        public readonly IExpression _trueExpr;
        public readonly IExpression _falseExpr;

        public TernaryExpression(IExpression condition, IExpression trueExpr, IExpression falseExpr)
        {
            this._condition = condition;
            this._trueExpr = trueExpr;
            this._falseExpr = falseExpr;
        }

        public IValue Evaluate()
        {
            if (_condition.Evaluate().AsNumber() != 0)
            {
                return _trueExpr.Evaluate();
            }
            else
            {
                return _falseExpr.Evaluate();
            }
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{_condition} ? {_trueExpr} : {_falseExpr}";
        }
    }
}
