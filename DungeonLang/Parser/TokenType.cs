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
        TEXT,

        // Keywords
        PRINT,

        PLUS,
        MINUS,
        STAR,
        SLASH,
        EQ,

        LPAERN,
        RPAREN,

        EOF
    }
}
