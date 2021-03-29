using DungeonLang.lib;
using DungeonLang.Parser.AST;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.visitors
{
    public sealed class AssignValidator : AbstractVisitor
    {
        public void Visit(AssignmentStatement s)
        {
            base.Visit(s);
            if (Variables.IsExist(s._variable))
            {
                throw new RuntimeException("Cannot asign value to constant");
            }
        }
    }
}
