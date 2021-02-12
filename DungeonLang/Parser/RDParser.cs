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

        public List<Statement> Parse() // Рекурсивный спуск
        {
            List<Statement> result = new List<Statement>();
            while (!IsMatch(TokenType.EOF))
            {
                result.Add(StatementParse());
            }
            return result;
        }

        private Statement StatementParse()
        {
            if (IsMatch(TokenType.PRINT))
            {
                return new PrintStatement(ExpressionParse());
            }
            if (IsMatch(TokenType.IF))
            {
                return IfElse();
            }
            return AssignmentStatement();
        }

        private Statement AssignmentStatement()
        {
            Token current = GetToken(0);
            if (IsMatch(TokenType.WORD) && GetToken(0).Type == TokenType.EQ)
            {
                string variable = current.Text;
                Consume(TokenType.EQ);
                return new AssignmentSatement(variable, ExpressionParse());
            }
            throw new RuntimeException("Unknown statement");
        }

        private Token Consume(TokenType type)
        {
            Token current = GetToken(0);
            if (type != current.Type) throw new RuntimeException($"Token {current} doesn`t match {type}");
            _pos++;
            return current;
        }

        private Statement IfElse()
        {
            Expression condition = ExpressionParse();
            Statement ifStatement = StatementParse();
            Statement elseStatement;
            if (IsMatch(TokenType.ELSE))
            {
                elseStatement = StatementParse();
            }
            else
            {
                elseStatement = null;
            }
            return new IfStatement(condition, ifStatement, elseStatement);
        }

        private AST.Expression ExpressionParse()
        {
            return Conditional();
        }

        private AST.Expression Conditional()
        {
            Expression result = Additive();

            while (true)
            {
                if (IsMatch(TokenType.EQ))
                {
                    result = new ConditionalExpression('=', result, Additive());
                    continue;
                }
                if (IsMatch(TokenType.LT))
                {
                    result = new ConditionalExpression('<', result, Additive());
                    continue;
                }
                if (IsMatch(TokenType.GT))
                {
                    result = new ConditionalExpression('>', result, Additive());
                    continue;
                }
                break;
            }
            return result;
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
                return new ValueExpression(Double.Parse(current.Text, CultureInfo.InvariantCulture));
            }
            if (IsMatch(TokenType.HEX_NUMBER))
            {
                return new ValueExpression(Int64.Parse(current.Text, NumberStyles.HexNumber));
            }
            if (IsMatch(TokenType.WORD))
            {
                return new VariableExpression(current.Text);
            }
            if (IsMatch(TokenType.TEXT))
            {
                return new ValueExpression(current.Text);
            }
            if (IsMatch(TokenType.LPAERN))
            {
                Expression result = ExpressionParse();
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
