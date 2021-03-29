using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class BlockStatement : IStatement
    {
        public readonly List<IStatement> _statements;

        public BlockStatement()
        {
            this._statements = new List<IStatement>();
        }

        public void Add(IStatement statement)
        {
            _statements.Add(statement);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
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
