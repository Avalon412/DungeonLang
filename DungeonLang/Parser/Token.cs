using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser
{
    public sealed class Token
    {
        private TokenType _type;
        private string _text;

        public Token(){}

        public Token(TokenType type, string text)
        {
            this._type = type;
            this._text = text;
        }

        public TokenType Type {
            get {
                return this._type;
            }
            set {
                this._type = value;
            }
        }

        public string Text {
            get {
                return this._text;
            }
            set {
                this._text = value;
            }
        }

        public override string ToString()
        {
            return Type + " " + Text;
        }
    }
}
