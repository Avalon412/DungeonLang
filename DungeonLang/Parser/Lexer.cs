using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser
{
    public sealed class Lexer
    {
        private readonly string _input;
        private readonly List<Token> _tokens;
        private readonly int _length;
        private int _pos;

        private static readonly string OPERATOR_CHARS = "+-*/()";
        private static readonly TokenType[] OPERATOR_TOKENS =
        {
            TokenType.PLUS, TokenType.MINUS, TokenType.STAR, TokenType.SLASH, TokenType.LPAERN, TokenType.RPAREN
        };

        public Lexer(string input)
        {
            this._input = input;
            this._length = input.Length;
            this._tokens = new List<Token>();
        }

        public List<Token> Tokenize()
        {
            while (_pos < _length)
            {
                char current = Peek(0);
                if (Char.IsDigit(current)) TokenizeNumber();
                else if (Char.IsLetter(current)) TokenizeWord();
                else if (current == '#')
                {
                    Next();
                    TokenizeHexNumber();
                }
                else if (OPERATOR_CHARS.IndexOf(current) != -1)
                {
                    TokenizeOperator();
                }
                else
                {
                    // Possible whitespace
                    Next();
                }
            }
            return _tokens;
        }

        private void TokenizeOperator()
        {
            int position = OPERATOR_CHARS.IndexOf(Peek(0));
            AddToken(OPERATOR_TOKENS[position]);
            Next();
        }

        private void TokenizeHexNumber()
        {
            StringBuilder buffer = new StringBuilder();
            char current = Peek(0);
            while (Char.IsDigit(current) || IsHexNumber(current))
            {
                buffer.Append(current);
                current = Next();
            }
            AddToken(TokenType.HEX_NUMBER, buffer.ToString());
        }


        private void TokenizeNumber()
        {
            StringBuilder buffer = new StringBuilder();
            char current = Peek(0);
            while (true)
            {
                if (current == '.')
                {
                    if (buffer.ToString().IndexOf(".") != -1) throw new RuntimeException("Invalid float number");
                }
                else if (!Char.IsDigit(current))
                {
                    break;
                }
                buffer.Append(current);
                current = Next();
            }
            AddToken(TokenType.NUMBER, buffer.ToString());
        }

        private void TokenizeWord()
        {
            StringBuilder buffer = new StringBuilder();
            char current = Peek(0);
            while (true)
            {
                if(!Char.IsLetterOrDigit(current) && (current != '_') && (current != '$'))
                {
                    break;
                }
                buffer.Append(current);
                current = Next();
            }
            AddToken(TokenType.WORD, buffer.ToString());
        }

        private bool IsHexNumber(char current)
        {
            return "abcdef".IndexOf(Char.ToLower(current)) != -1;
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, "");
        }

        private void AddToken(TokenType type, string text)
        {
            _tokens.Add(new Token(type, text));
        }

        private char Peek(int relativePos)
        {
            int position = _pos + relativePos;
            if (position >= _length) return '\0';
            return _input[position];
        }

        private char Next()
        {
            _pos++;
            return Peek(0);
        }
    }
}
