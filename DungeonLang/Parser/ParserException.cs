using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser
{
    public sealed class ParserException : RuntimeException
    {
        public ParserException() : base() { }

        public ParserException(string message) : base(message) { }
    }
}
