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
        public enum Operator
        {
            PLUS,
            MINUS,
            MULTIPLY,
            DIVIDE,

            EQUALS,
            NOT_EQUALS,

            LT,
            LTEQ,
            GT,
            GTEQ,

            AND,
            OR
        }

        private readonly Expression _expression1;
        private readonly Expression _expression2;
        private readonly Operator _operation;
        private string _operator;

        public ConditionalExpression(Operator operation, Expression expression1, Expression expression2)
        {
            this._expression1 = expression1;
            this._expression2 = expression2;
            this._operation = operation;
        }

        public Value Evaluate()
        {
            Value value1 = _expression1.Evaluate();
            Value value2 = _expression2.Evaluate();

            double number1;
            double number2;
            if (value1 is StringValue)
            {
                number1 = value1.AsString().CompareTo(value2.AsString());
                number2 = 0;
            }
            else
            {
                number1 = value1.AsNumber();
                number2 = value2.AsNumber();
            }

            bool result;
            switch (_operation)
            {
                case Operator.LT: result = number1 < number2; _operator = "<"; break;
                case Operator.LTEQ: result = number1 <= number2; _operator = "<="; break;
                case Operator.GT: result = number1 > number2; _operator = ">"; break;
                case Operator.GTEQ: result = number1 >= number2; _operator = ">="; break;
                case Operator.NOT_EQUALS: result = number1 != number2; _operator = "!="; break;

                case Operator.AND: result = (number1 != 0) && (number2 != 0); _operator = "&&"; break;
                case Operator.OR: result = (number1 != 0) || (number2 != 0); _operator = "||"; break;

                case Operator.EQUALS:
                default:
                    result = number1 == number2; _operator = "=="; break;
            }
            return new NumberValue(result);
        }

        public override string ToString()
        {
            return $"{_expression1} {_operator} {_expression2}";
        }
    }
}
