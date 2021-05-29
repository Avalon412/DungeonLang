using DungeonLang.lib.modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class UseStatement : IStatement
    {
        private readonly string PACKAGE = "DungeonLang.lib.modules.";

        public readonly IExpression _expression;

        public UseStatement(IExpression expression)
        {
            this._expression = expression;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Execute()
        {
            try
            {
                string moduleName = _expression.Evaluate().AsString();
                IModule module = (IModule)Activator.CreateInstance(Type.GetType(PACKAGE + moduleName));
                module.Init();
            } catch (Exception ex) { }
        }

        public override string ToString()
        {
            return "use " + _expression;
        }
    }
}
