using DungeonLang.lib;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class UnaryExpression : IExpression
    {
        public enum Operator
        {
            NEGATE,
            NOT,
            COMPLEMENT
        }

        public readonly IExpression _expr1;
        public readonly Operator _operation;
        public string _operator;

        public UnaryExpression(Operator operation, IExpression expr)
        {
            this._operation = operation;
            this._expr1 = expr;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IValue Evaluate()
        {
            IValue value = _expr1.Evaluate();
            switch (_operation)
            {
                case Operator.NEGATE: _operator = "-"; return new NumberValue(-value.AsNumber());
                case Operator.COMPLEMENT: _operator = "~"; return new NumberValue(~(int)value.AsNumber());
                case Operator.NOT: _operator = "!"; return new NumberValue(value.AsNumber() != 0 ? 0 : 1);
                default:
                    throw new RuntimeException("Operation " + _operator + " is not supported");
            }
        }

        public override string ToString()
        {
            return $"{_operator}{_expr1}";
        }
    }
}
