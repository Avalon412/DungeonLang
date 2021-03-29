using DungeonLang.Parser.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.visitors
{
    public sealed class FunctionAdder : AbstractVisitor
    {
        public void Visit(FunctionDefineStatement s)
        {
            base.Visit(s);
            s.Execute();
        }
    }
}
