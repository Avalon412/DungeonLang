using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class WhileStatement : IStatement
    {
        private readonly IExpression _condition;
        private readonly IStatement _statement;

        public WhileStatement(IExpression condition, IStatement statement)
        {
            this._condition = condition;
            this._statement = statement;
        }

        public void Execute()
        {
            while (_condition.Evaluate().AsNumber() != 0)
            {
                try
                {
                    _statement.Execute();
                }
                catch (BreakStatement bx)
                {
                    break;
                }
                catch (ContinueStatement cx)
                {
                    continue;
                }
            }
        }

        public override string ToString()
        {
            return $"while {_condition}  {_statement}";
        }
    }
}
