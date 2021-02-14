using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class WhileStatement : Statement
    {
        private readonly Expression _condition;
        private readonly Statement _statement;

        public WhileStatement(Expression condition, Statement statement)
        {
            this._condition = condition;
            this._statement = statement;
        }

        public void Execute()
        {
            while (_condition.Evaluate().AsNumber() != 0)
            {
                _statement.Execute();
            }
        }

        public override string ToString()
        {
            return $"while {_condition}  {_statement}";
        }
    }
}
