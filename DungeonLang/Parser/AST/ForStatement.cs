using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ForStatement : Statement
    {
        private readonly Statement _initialization;
        private readonly Expression _termination;
        private readonly Statement _increment;
        private readonly Statement _statement;

        public ForStatement(Statement initialization, Expression termination, Statement increment, Statement statement)
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
                _statement.Execute();
            }
        }

        public override string ToString()
        {
            return $"for {_initialization}, {_termination}, {_increment}  {_statement}";
        }
    }   
}
