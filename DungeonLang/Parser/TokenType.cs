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
        IF,
        ELSE,

        PLUS,
        MINUS,
        STAR,
        SLASH,
        EQ,
        LT,
        GT,

        LPAERN,
        RPAREN,

        EOF
    }
}
