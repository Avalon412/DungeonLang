﻿using DungeonLang.lib;
using NPOI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ReturnStatement : RuntimeException, IStatement
    {
        private readonly IExpression _expression;
        private IValue _result;

        public ReturnStatement(IExpression expression)
        {
            this._expression = expression;
        }

        public void Execute()
        {
            _result = _expression.Evaluate();
            throw this;
        }

        public IValue GetResult()
        {
            return _result;
        }

        public override string ToString()
        {
            return "return";
        }
    }
}