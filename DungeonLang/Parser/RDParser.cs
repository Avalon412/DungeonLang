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
            if (IsMatchToken(0, TokenType.LBRACE)) return Block();
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
            if (IsMatch(TokenType.RETURN))
            {
                return new ReturnStatement(ExpressionParse());
            }
            if (IsMatch(TokenType.DEF))
            {
                return FunctionDefine();
            }
            if (IsMatchToken(0, TokenType.WORD) && IsMatchToken(1, TokenType.LPAREN))
            {
                return new FunctionStatement(Function());
            }
            return AssignmentStatement();
        }

        private IStatement AssignmentStatement()
        {
            if (IsMatchToken(0, TokenType.WORD) && IsMatchToken(1, TokenType.EQ))
            {
                string variable = Consume(TokenType.WORD).Text;
                Consume(TokenType.EQ);
                return new AssignmentStatement(variable, ExpressionParse());
            }
            if (IsMatchToken(0, TokenType.WORD) && IsMatchToken(1, TokenType.LBRACKET))
            {
                string variable = Consume(TokenType.WORD).Text;
                Consume(TokenType.LBRACKET);
                IExpression index = ExpressionParse();
                Consume(TokenType.RBRACKET);
                Consume(TokenType.EQ);
                return new ArrayAssignmentStatement(variable, index, ExpressionParse());
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

        private FunctionDefineStatement FunctionDefine()
        {
            string name = Consume(TokenType.WORD).Text;
            Consume(TokenType.LPAREN);
            List<string> argNames = new List<string>();
            while (!IsMatch(TokenType.RPAREN))
            {
                argNames.Add(Consume(TokenType.WORD).Text);
                IsMatch(TokenType.COMMA);
            }
            IStatement body = StatementOrBlock();
            return new FunctionDefineStatement(name, argNames, body);
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

        private IExpression Array()
        {
            Consume(TokenType.LBRACKET);
            List<IExpression> elements = new List<IExpression>();
            while (!IsMatch(TokenType.RBRACKET))
            {
                elements.Add(ExpressionParse());
                IsMatch(TokenType.COMMA);
            }
            return new ArrayExpression(elements);
        }

        private IExpression Element()
        {
            string variable = Consume(TokenType.WORD).Text;
            Consume(TokenType.LBRACKET);
            IExpression index = ExpressionParse();
            Consume(TokenType.RBRACKET);
            return new ArrayAccessExpression(variable, index);
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
            if (IsMatchToken(0, TokenType.WORD) && IsMatchToken(1, TokenType.LPAREN))
            {
                return Function();
            }
            if (IsMatchToken(0, TokenType.WORD) && IsMatchToken(1, TokenType.LBRACKET))
            {
                return Element();
            }
            if (IsMatchToken(0, TokenType.LBRACKET))
            {
                return Array();
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

        private bool IsMatchToken(int pos, TokenType type)
        {
            return GetToken(pos).Type == type;
        }

        private Token GetToken(int relativePosition)
        {
            int position = _pos + relativePosition;
            if (position >= _size) return EOF;
            return _tokens[position];
        }
    }
}
