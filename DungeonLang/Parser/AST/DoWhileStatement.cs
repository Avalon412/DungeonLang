using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class DoWhileStatement : Statement
    {
        private readonly Expression _condition;
        private readonly Statement _statement;

        public DoWhileStatement(Expression condition, Statement statement)
        {
            this._condition = condition;
            this._statement = statement;
        }

        public void Execute()
        {
            do
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
            } while (_condition.Evaluate().AsNumber() != 0);
        }

        public override string ToString()
        {
            return $"do {_statement} while {_condition}";
        }
    }
}
