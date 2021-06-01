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
        private int _row;
        private int _col;

        private static readonly string OPERATOR_CHARS = "+-*/%()[]{}=<>!&|,^~?:";
        private static readonly Dictionary<string, TokenType> OPERATORS;

        static Lexer()
        {
            OPERATORS = new Dictionary<string, TokenType>();
            OPERATORS.Add("+", TokenType.PLUS);
            OPERATORS.Add("-", TokenType.MINUS);
            OPERATORS.Add("*", TokenType.STAR);
            OPERATORS.Add("/", TokenType.SLASH);
            OPERATORS.Add("%", TokenType.PERCENT);
            OPERATORS.Add("(", TokenType.LPAREN);
            OPERATORS.Add(")", TokenType.RPAREN);
            OPERATORS.Add("[", TokenType.LBRACKET);
            OPERATORS.Add("]", TokenType.RBRACKET);
            OPERATORS.Add("{", TokenType.LBRACE);
            OPERATORS.Add("}", TokenType.RBRACE);
            OPERATORS.Add("=", TokenType.EQ);
            OPERATORS.Add("<", TokenType.LT);
            OPERATORS.Add(">", TokenType.GT);
            OPERATORS.Add(",", TokenType.COMMA);
            OPERATORS.Add("^", TokenType.CARET);
            OPERATORS.Add("~", TokenType.TILDE);
            OPERATORS.Add("?", TokenType.QUESTION);
            OPERATORS.Add(":", TokenType.COLON);

            OPERATORS.Add("!", TokenType.EXCL);
            OPERATORS.Add("&", TokenType.AMP);
            OPERATORS.Add("|", TokenType.BAR);

            OPERATORS.Add("==", TokenType.EQEQ);
            OPERATORS.Add("!=", TokenType.EXCLEQ);
            OPERATORS.Add("<=", TokenType.LTEQ);
            OPERATORS.Add(">=", TokenType.GTEQ);

            OPERATORS.Add("&&", TokenType.AMPAMP);
            OPERATORS.Add("||", TokenType.BARBAR);

            OPERATORS.Add("<<", TokenType.LTLT);
            OPERATORS.Add(">>", TokenType.GTGT);
        }

        public Lexer(string input)
        {
            this._input = input;
            this._length = input.Length;
            this._tokens = new List<Token>();
            this._row = this._col = 1;
        }

        public List<Token> Tokenize()
        {
            while (_pos < _length)
            {
                char current = Peek(0);
                if (Char.IsDigit(current)) TokenizeNumber();
                else if (Char.IsLetter(current)) TokenizeWord();
                else if (current == '"') TokenizeText();
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
            char current = Peek(0);

            if (current == '/')
            {
                if (Peek(1) == '/')
                {
                    Next();
                    Next();
                    TokenizeComment();
                    return;
                }
                else if (Peek(1) == '*')
                {
                    Next();
                    Next();
                    TokenizeMultilineComment();
                    return;
                }
            }
            StringBuilder buffer = new StringBuilder();
            while (true)
            {
                string text = buffer.ToString();
                if (!OPERATORS.ContainsKey(text + current) && (!String.IsNullOrEmpty(text)))
                {
                    TokenType token;
                    OPERATORS.TryGetValue(text, out token);
                    AddToken(token);
                    return;
                }
                buffer.Append(current);
                current = Next();
            }
        }

        private void TokenizeMultilineComment()
        {
            char current = Peek(0);
            while (true)
            {
                if (current == '\0') throw Error("Reached end of file while parsing multiline comment");
                if (current == '*' && Peek(1) == '/') break;
                current = Next();
            }
            Next();
            Next();
        }

        private void TokenizeComment()
        {
            char current = Peek(0);
            while ("\r\n\0".IndexOf(current) != -1)
            {
                current = Next();
            }
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
                    if (buffer.ToString().IndexOf(".") != -1) throw Error("Invalid float number");
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
            string word = buffer.ToString();
            switch (word)
            {
                case "print": AddToken(TokenType.PRINT); break;
                case "if": AddToken(TokenType.IF); break;
                case "else": AddToken(TokenType.ELSE); break;
                case "while": AddToken(TokenType.WHILE); break;
                case "for": AddToken(TokenType.FOR); break;
                case "do": AddToken(TokenType.DO); break;
                case "break": AddToken(TokenType.BREAK); break;
                case "continue": AddToken(TokenType.CONTINUE); break;
                case "def": AddToken(TokenType.DEF); break;
                case "return": AddToken(TokenType.RETURN); break;
                case "use": AddToken(TokenType.USE); break;
                default:
                    AddToken(TokenType.WORD, word); break;
            }
        }

        private void TokenizeText()
        {
            Next();
            StringBuilder buffer = new StringBuilder();
            char current = Peek(0);

            while (true)
            {
                if (current == '\0') throw Error("Reached end of file while parsing text string");
                if (current == '\\')
                {
                    current = Next();
                    switch (current)
                    {
                        case '"': current = Next(); buffer.Append('"'); continue;
                        case 'n': current = Next(); buffer.Append('\n'); continue;
                        case 't': current = Next(); buffer.Append('\t'); continue;
                    }
                    buffer.Append('\\');
                    continue;
                }
                if (current == '"') break;
                buffer.Append(current);
                current = Next();
            }
            Next();
            AddToken(TokenType.TEXT, buffer.ToString());
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
            _tokens.Add(new Token(type, text, _row, _col));
        }

        private LexerException Error(string text)
        {
            return new LexerException(_row, _col, text);
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
            char result = Peek(0);
            if (result == '\n')
            {
                _row++;
                _col = 1;
            }
            else _col++;
            return result;
        }
    }
}
