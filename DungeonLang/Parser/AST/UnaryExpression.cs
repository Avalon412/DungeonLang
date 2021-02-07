using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    class UnaryExpression : Expression
    {
        private readonly Expression _expr1;
        private readonly char _operation;

        public UnaryExpression(char operation, Expression expr)
        {
            this._operation = operation;
            this._expr1 = expr;
        }

        public double Evaluate()
        {
            switch (_operation)
            {
                case '-': return -_expr1.Evaluate();
                case '+':
                default:
                    return _expr1.Evaluate();
            }
        }

        public override string ToString()
        {
            return $"{_operation}{_expr1}";
        }
    }
}
