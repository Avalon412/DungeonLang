using DungeonLang.Parser.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.visitors
{
    public sealed class VariablePrinter : AbstractVisitor
    {
        public void Visit(ArrayAccessExpression s)
        {
            base.Visit(s);
            Console.WriteLine(s._variable);
        }

        public void Visit(AssignmentStatement s)
        {
            base.Visit(s);
            Console.WriteLine(s._variable);
        }

        public void Visit(VariableExpression s)
        {
            base.Visit(s);
            Console.WriteLine(s._name);
        }
    }
}
