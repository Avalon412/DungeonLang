﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class BlockStatement : Statement
    {
        private readonly List<Statement> _statements;

        public BlockStatement()
        {
            this._statements = new List<Statement>();
        }

        public void Add(Statement statement)
        {
            _statements.Add(statement);
        }

        public void Execute()
        {
            foreach (var statement in _statements)
            {
                statement.Execute();
            }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (var statement in _statements)
            {
                buffer.Append(statement.ToString()).Append(Environment.NewLine);
            }
            return buffer.ToString();
        }
    }
}
