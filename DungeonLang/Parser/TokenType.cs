using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser
{
    public enum TokenType
    {
        NUMBER,
        HEX_NUMBER,
        WORD,

        PLUS,
        MINUS,
        STAR,
        SLASH,

        LPAERN,
        RPAREN,

        EOF
    }
}
