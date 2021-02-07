using DungeonLang.Parser.AST;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DungeonLang.Parser
{
    class RDParser
    {
        private static readonly Token EOF = new Token(TokenType.EOF, "");

        private readonly List<Token> _tokens;
        private readonly int _size;

        private int _pos;

        public RDParser(List<Token> tokens)
        {
            this._tokens = tokens;
            this._size = tokens.Count;
        }

        public List<AST.Expression> Parse() // Рекурсивный спуск
        {
            List<AST.Expression> result = new List<AST.Expression>();
            while (!IsMatch(TokenType.EOF))
            {
                result.Add(Expression());
            }
            return result;
        }

        private AST.Expression Expression()
        {
            return Additive();
        }

        private AST.Expression Additive()
        {
            AST.Expression result = Multiplicative();

            while (true)
            {
                if (IsMatch(TokenType.PLUS))
                {
                    result = new AST.BinaryExpression('+', result, Multiplicative());
                    continue;
                }
                if (IsMatch(TokenType.MINUS))
                {
                    result = new AST.BinaryExpression('-', result, Multiplicative());
                    continue;
                }
                break;
            }

            return result;
        }

        private AST.Expression Multiplicative()
        {
            AST.Expression result = Unary();
            while (true)
            {
                if (IsMatch(TokenType.STAR))
                {
                    result = new AST.BinaryExpression('*', result, Unary());
                    continue;
                }
                if (IsMatch(TokenType.SLASH))
                {
                    result = new AST.BinaryExpression('/', result, Unary());
                    continue;
                }
                break;
            }
            return result;
        }

        private AST.Expression Unary()
        {
            if (IsMatch(TokenType.MINUS))
            {
                return new UnaryExpression('-', Primary());
            }
            if (IsMatch(TokenType.PLUS))
            {
                return Primary();
            }
            return Primary();
        }

        private AST.Expression Primary()
        {
            Token current = GetToken(0);
            if (IsMatch(TokenType.NUMBER))
            {
                return new NumberExpression(Double.Parse(current.Text));
            }
            if (IsMatch(TokenType.HEX_NUMBER))
            {
                return new NumberExpression(Int64.Parse(current.Text, NumberStyles.HexNumber));
            }
            if (IsMatch(TokenType.LPAERN))
            {
                Expression result = Expression();
                IsMatch(TokenType.RPAREN);
                return result;
            }
            throw new RuntimeException("Unknown Expression");
        }

        private bool IsMatch(TokenType type)
        {
            Token current = GetToken(0);
            if (type != current.Type) return false;
            _pos++;
            return true;
        }

        private Token GetToken(int relativePosition)
        {
            int position = _pos + relativePosition;
            if (position >= _size) return EOF;
            return _tokens[position];
        }
    }
}
