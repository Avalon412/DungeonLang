﻿using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ValueExpression : IExpression
    {
        public readonly IValue _value;

        public ValueExpression(double value)
        {
            this._value = new NumberValue(value);
        }

        public ValueExpression(string value)
        {
            this._value = new StringValue(value);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IValue Evaluate()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.AsString();
        }
    }
}
