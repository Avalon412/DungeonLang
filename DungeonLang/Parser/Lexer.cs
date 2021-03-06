﻿using NPOI.Util;
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

        private static readonly string OPERATOR_CHARS = "+-*/(){}=<>!&|,";
        private static readonly Dictionary<string, TokenType> OPERATORS;

        static Lexer()
        {
            OPERATORS = new Dictionary<string, TokenType>();
            OPERATORS.Add("+", TokenType.PLUS);
            OPERATORS.Add("-", TokenType.MINUS);
            OPERATORS.Add("*", TokenType.STAR);
            OPERATORS.Add("/", TokenType.SLASH);
            OPERATORS.Add("(", TokenType.LPAREN);
            OPERATORS.Add(")", TokenType.RPAREN);
            OPERATORS.Add("{", TokenType.LBRACE);
            OPERATORS.Add("}", TokenType.RBRACE);
            OPERATORS.Add("=", TokenType.EQ);
            OPERATORS.Add("<", TokenType.LT);
            OPERATORS.Add(">", TokenType.GT);
            OPERATORS.Add(",", TokenType.COMMA);

            OPERATORS.Add("!", TokenType.EXCL);
            OPERATORS.Add("&", TokenType.AMP);
            OPERATORS.Add("|", TokenType.BAR);

            OPERATORS.Add("==", TokenType.EQEQ);
            OPERATORS.Add("!=", TokenType.EXCLEQ);
            OPERATORS.Add("<=", TokenType.LTEQ);
            OPERATORS.Add(">=", TokenType.GTEQ);

            OPERATORS.Add("&&", TokenType.AMPAMP);
            OPERATORS.Add("||", TokenType.BARBAR);
        }

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
                if (current == '\0') throw new RuntimeException("Missing close tag");
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
            string word = buffer.ToString();
            switch (word)
            {
                case "print": AddToken(TokenType.PRINT); break;
                case "if": AddToken(TokenType.IF); break;
                case "else": AddToken(TokenType.ELSE); break;
                case "while": AddToken(TokenType.WHILE); break;
                case "for": AddToken(TokenType.FOR); break;
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
