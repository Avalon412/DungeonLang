using System;
using NPOI.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ContinueStatement : RuntimeException, IStatement
    {
        public void Execute()
        {
            throw this;
        }

        public override string ToString()
        {
            return "continue";
        }
    }
}
