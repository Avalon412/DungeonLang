﻿using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class UnaryExpression : IExpression
    {
        private readonly IExpression _expr1;
        private readonly char _operation;

        public UnaryExpression(char operation, IExpression expr)
        {
            this._operation = operation;
            this._expr1 = expr;
        }

        public IValue Evaluate()
        {
            switch (_operation)
            {
                case '-': return new NumberValue(-_expr1.Evaluate().AsNumber());
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
