using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    class BinaryExpression : Expression
    {
        private readonly Expression _expr1;
        private readonly Expression _expr2;
        private readonly char _operation;

        public BinaryExpression(char operation, Expression expr1, Expression expr2)
        {
            this._operation = operation;
            this._expr1 = expr1;
            this._expr2 = expr2;
        }

        public double Evaluate()
        {
            switch (_operation)
            {
                case '-': return _expr1.Evaluate() - _expr2.Evaluate();
                case '*': return _expr1.Evaluate() * _expr2.Evaluate();
                case '/': return _expr1.Evaluate() / _expr2.Evaluate();
                case '+':
                default:
                    return _expr1.Evaluate() + _expr2.Evaluate();
            }
        }

        public override string ToString()
        {
            return $"{_expr1} {_operation} {_expr2}";
        }
    }
}
