using DungeonLang.lib;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class BinaryExpression : IExpression
    {
        public enum Operator
        {
            ADD,
            SUBSTRACT,
            MULTIPLY,
            DIVIDE,
            REMINDER,
            // Bitwise
            AND,
            OR,
            XOR,
            LSHIFT,
            RSHIFT,
        }

        public readonly IExpression _expr1;
        public readonly IExpression _expr2;
        public readonly Operator _operation;
        public string _operator;

        public BinaryExpression(Operator operation, IExpression expr1, IExpression expr2)
        {
            this._operation = operation;
            this._expr1 = expr1;
            this._expr2 = expr2;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IValue Evaluate()
        {
            IValue value1 = _expr1.Evaluate();
            IValue value2 = _expr2.Evaluate();
            if (value1 is StringValue || value1 is ArrayValue)
            {
                string string1 = value1.AsString();
                switch(_operation)
                {
                    case Operator.MULTIPLY:
                        {
                            _operator = "*";
                            int iterations = (int)value2.AsNumber();
                            StringBuilder builder = new StringBuilder();
                            for (int i = 0; i < iterations; i++)
                            {
                                builder.Append(value1);
                            }
                            return new StringValue(builder.ToString());
                        }
                    case Operator.ADD:
                    default:
                        _operator = "+"; return new StringValue(string1 + value2.AsString());
                }
            }

            double number1 = value1.AsNumber();
            double number2 = value2.AsNumber();
            double result;
            switch (_operation)
            {
                case Operator.ADD: result = number1 + number2; break;
                case Operator.SUBSTRACT: result = number1 - number2; break;
                case Operator.MULTIPLY: result = number1 * number2; break;
                case Operator.DIVIDE: result = number1 / number2; break;

                //Bitwise
                case Operator.AND: result = (int)number1 & (int)number2; break;
                case Operator.XOR: result = (int)number1 ^ (int)number2; break;
                case Operator.OR: result = (int)number1 | (int)number2; break;
                case Operator.LSHIFT: result = (int)number1 << (int)number2; break;
                case Operator.RSHIFT: result = (int)number1 >> (int)number2; break;
                default:
                    throw new RuntimeException("Operation " + _operator + " is not supported");
            }
            return new NumberValue(result);
        }

        public override string ToString()
        {
            return $"{_expr1} {_operator} {_expr2}";
        }
    }
}
