using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser
{
    public sealed class LexerException : RuntimeException
    {
        public LexerException(string message) : base(message) { }

        public LexerException(int row, int col, string message) : base("[" + row + ":" + col + "]" + message) { }
    }
}
