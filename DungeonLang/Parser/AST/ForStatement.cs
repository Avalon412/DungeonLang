using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ForStatement : IStatement
    {
        private readonly IStatement _initialization;
        private readonly IExpression _termination;
        private readonly IStatement _increment;
        private readonly IStatement _statement;

        public ForStatement(IStatement initialization, IExpression termination, IStatement increment, IStatement statement)
        {
            this._initialization = initialization;
            this._termination = termination;
            this._increment = increment;
            this._statement = statement;
        }

        public void Execute()
        {
            for (_initialization.Execute(); _termination.Evaluate().AsNumber() != 0; _increment.Execute())
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
            return $"for {_initialization}, {_termination}, {_increment}  {_statement}";
        }
    }   
}
