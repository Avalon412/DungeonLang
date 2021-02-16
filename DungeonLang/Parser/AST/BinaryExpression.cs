using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class BinaryExpression : IExpression
    {
        private readonly IExpression _expr1;
        private readonly IExpression _expr2;
        private readonly char _operation;

        public BinaryExpression(char operation, IExpression expr1, IExpression expr2)
        {
            this._operation = operation;
            this._expr1 = expr1;
            this._expr2 = expr2;
        }

        public IValue Evaluate()
        {
            IValue value1 = _expr1.Evaluate();
            IValue value2 = _expr2.Evaluate();
            if (value1 is StringValue)
            {
                string string1 = value1.AsString();
                switch(_operation)
                {
                    case '*':
                        {
                            int iterations = (int)value2.AsNumber();
                            StringBuilder builder = new StringBuilder();
                            for (int i = 0; i < iterations; i++)
                            {
                                builder.Append(value1);
                            }
                            return new StringValue(builder.ToString());
                        }
                    case '+':
                    default:
                        return new StringValue(string1 + value2.AsString());
                }
            }

            double number1 = value1.AsNumber();
            double number2 = value2.AsNumber();
            switch (_operation)
            {
                case '-': return new NumberValue(number1 - number2);
                case '*': return new NumberValue(number1 * number2);
                case '/': return new NumberValue(number1 / number2);
                case '+':
                default:
                    return new NumberValue(number1 + number2);
            }
        }

        public override string ToString()
        {
            return $"{_expr1} {_operation} {_expr2}";
        }
    }
}
