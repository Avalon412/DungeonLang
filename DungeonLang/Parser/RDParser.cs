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

        public IStatement Parse() // Рекурсивный спуск
        {
            BlockStatement result = new BlockStatement();
            while (!IsMatch(TokenType.EOF))
            {
                result.Add(StatementParse());
            }
            return result;
        }

        private IStatement Block()
        {
            BlockStatement block = new BlockStatement();
            Consume(TokenType.LBRACE);
            while (!IsMatch(TokenType.RBRACE))
            {
                block.Add(StatementParse());
            }
            return block;
        }

        private IStatement StatementOrBlock()
        {
            if (GetToken(0).Type == TokenType.LBRACE) return Block();
            return StatementParse();
        }

        private IStatement StatementParse()
        {
            if (IsMatch(TokenType.PRINT))
            {
                return new PrintStatement(ExpressionParse());
            }
            if (IsMatch(TokenType.IF))
            {
                return IfElse();
            }
            if (IsMatch(TokenType.WHILE))
            {
                return WhileStatement();
            }
            if (IsMatch(TokenType.FOR))
            {
                return ForStatement();
            }
            if (IsMatch(TokenType.DO))
            {
                return DoWhileStatement();
            }
            if (IsMatch(TokenType.BREAK))
            {
                return new BreakStatement();
            }
            if (IsMatch(TokenType.CONTINUE))
            {
                return new ContinueStatement();
            }
            if (GetToken(0).Type == TokenType.WORD && GetToken(1).Type == TokenType.LPAREN)
            {
                return new FunctionStatement(Function());
            }
            return AssignmentStatement();
        }

        private IStatement AssignmentStatement()
        {
            Token current = GetToken(0);
            if (IsMatch(TokenType.WORD) && GetToken(0).Type == TokenType.EQ)
            {
                string variable = current.Text;
                Consume(TokenType.EQ);
                return new AssignmentStatement(variable, ExpressionParse());
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

        private IStatement IfElse()
        {
            IExpression condition = ExpressionParse();
            IStatement ifStatement = StatementOrBlock();
            IStatement elseStatement;
            if (IsMatch(TokenType.ELSE))
            {
                elseStatement = StatementOrBlock();
            }
            else
            {
                elseStatement = null;
            }
            return new IfStatement(condition, ifStatement, elseStatement);
        }

        private IStatement WhileStatement()
        {
            IExpression condition = ExpressionParse();
            IStatement statement = StatementOrBlock();
            return new WhileStatement(condition, statement);
        }

        private IStatement DoWhileStatement()
        {
            IStatement statement = StatementOrBlock();
            Consume(TokenType.WHILE);
            IExpression condition = ExpressionParse();
            return new DoWhileStatement(condition, statement);
        }

        public IStatement ForStatement()
        {
            IStatement initialization = AssignmentStatement();
            Consume(TokenType.COMMA);
            IExpression termination = ExpressionParse();
            Consume(TokenType.COMMA);
            IStatement increment = AssignmentStatement();
            IStatement statement = StatementOrBlock();
            return new ForStatement(initialization, termination, increment, statement);
        }

        private FunctionalExpression Function()
        {
            string name = Consume(TokenType.WORD).Text;
            Consume(TokenType.LPAREN);
            FunctionalExpression function = new FunctionalExpression(name);
            while (!IsMatch(TokenType.RPAREN))
            {
                function.AddArgument(ExpressionParse());
                IsMatch(TokenType.COMMA);
            }
            return function;
        }

        private AST.IExpression ExpressionParse()
        {
            return LogicOr();
        }

        private AST.IExpression LogicOr()
        {
            IExpression result = LogicAnd();

            while (true)
            {
                if (IsMatch(TokenType.BARBAR))
                {
                    result = new ConditionalExpression(ConditionalExpression.Operator.OR, result, LogicAnd());
                    continue;
                }
                break;
            }
            return result;
        }

        private AST.IExpression LogicAnd()
        {
            IExpression result = Equality();

            while (true)
            {
                if (IsMatch(TokenType.AMPAMP))
                {
                    result = new ConditionalExpression(ConditionalExpression.Operator.AND, result, Equality());
                    continue;
                }
                break;
            }
            return result;
        }

        private AST.IExpression Equality()
        {
            IExpression result = Conditional();

            if (IsMatch(TokenType.EQEQ))
            {
                return new ConditionalExpression(ConditionalExpression.Operator.EQUALS, result, Conditional());
            }
            if (IsMatch(TokenType.EXCLEQ))
            {
                return new ConditionalExpression(ConditionalExpression.Operator.NOT_EQUALS, result, Conditional());
            }
            return result;
        }

        private AST.IExpression Conditional()
        {
            IExpression result = Additive();

            while (true)
            {
                if (IsMatch(TokenType.LT))
                {
                    result = new ConditionalExpression(ConditionalExpression.Operator.LT, result, Additive());
                    continue;
                }
                if (IsMatch(TokenType.LTEQ))
                {
                    result = new ConditionalExpression(ConditionalExpression.Operator.LTEQ, result, Additive());
                    continue;
                }
                if (IsMatch(TokenType.GT))
                {
                    result = new ConditionalExpression(ConditionalExpression.Operator.GT, result, Additive());
                    continue;
                }
                if (IsMatch(TokenType.GTEQ))
                {
                    result = new ConditionalExpression(ConditionalExpression.Operator.GTEQ, result, Additive());
                    continue;
                }
                break;
            }
            return result;
        }

        private AST.IExpression Additive()
        {
            AST.IExpression result = Multiplicative();

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

        private AST.IExpression Multiplicative()
        {
            AST.IExpression result = Unary();
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

        private AST.IExpression Unary()
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

        private AST.IExpression Primary()
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
            if (GetToken(0).Type == TokenType.WORD && GetToken(1).Type == TokenType.LPAREN)
            {
                return Function();
            }
            if (IsMatch(TokenType.WORD))
            {
                return new VariableExpression(current.Text);
            }
            if (IsMatch(TokenType.TEXT))
            {
                return new ValueExpression(current.Text);
            }
            if (IsMatch(TokenType.LPAREN))
            {
                IExpression result = ExpressionParse();
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
