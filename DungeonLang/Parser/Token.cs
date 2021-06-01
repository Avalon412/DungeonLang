using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser
{
    public sealed class Token
    {
        private readonly TokenType _type;
        private readonly string _text;
        private readonly int _row;
        private readonly int _col;

        public Token(TokenType type, string text, int row, int col)
        {
            this._type = type;
            this._text = text;
            this._row = row;
            this._col = col;
        }

        public TokenType Type {
            get {
                return this._type;
            }
        }

        public string Text {
            get {
                return this._text;
            }
        }

        public int Row {
            get {
                return this._row;
            }
        }

        public int Col {
            get {
                return this._col;
            }
        }

        public string Position()
        {
            return "[" + this._row + " " + this._col + "]";
        }

        public override string ToString()
        {
            return Type + " " + Position() + " " + Text;
        }
    }
}
