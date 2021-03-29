using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class FunctionStatement : IStatement
    {
        public readonly FunctionalExpression _function;

        public FunctionStatement(FunctionalExpression function)
        {
            this._function = function;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this); ;
        }

        public void Execute()
        {
            _function.Evaluate();
        }

        public override string ToString()
        {
            return _function.ToString();
        }
    }
}
