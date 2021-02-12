using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ConditionalExpression : Expression
    {
        private readonly Expression _expression1;
        private readonly Expression _expression2;
        private readonly char _operation;

        public ConditionalExpression(char operation, Expression expression1, Expression expression2)
        {
            this._expression1 = expression1;
            this._expression2 = expression2;
            this._operation = operation;
        }

        public Value Evaluate()
        {
            Value value1 = _expression1.Evaluate();
            Value value2 = _expression2.Evaluate();
            if (value1 is StringValue)
            {
                string str1 = value1.AsString();
                string str2 = value2.AsString();
                switch (_operation)
                {
                    case '<': return new NumberValue(str1.CompareTo(str2) < 0);
                    case '>': return new NumberValue(str1.CompareTo(str2) > 0);
                    case '=':
                    default:
                        return new NumberValue(str1.Equals(str2));
                }
            }

            double num1 = value1.AsNumber();
            double num2 = value2.AsNumber();
            switch (_operation)
            {
                case '<': return new NumberValue(num1 < num2);
                case '>': return new NumberValue(num1 > num2);
                case '=':
                default:
                    return new NumberValue(num1 == num2);
            }
        }

        public override string ToString()
        {
            return $"{_expression1} {_operation} {_expression2}";
        }
    }
}
