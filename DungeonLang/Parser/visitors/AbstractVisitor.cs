using DungeonLang.Parser.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.visitors
{
    public abstract class AbstractVisitor : IVisitor
    {
        public void Visit(ArrayAccessExpression s) //print arr[4]
        {
            foreach (var index in s._indices)
            {
                index.Accept(this);
            }
        }

        public void Visit(ArrayAssignmentStatement s)
        {
            s._array.Accept(this);
            s._expression.Accept(this);
        }

        public void Visit(ArrayExpression s)
        {
            foreach (var index in s._elements)
            {
                index.Accept(this);
            }
        }

        public void Visit(AssignmentStatement s)
        {
            s._expression.Accept(this);
        }

        public void Visit(BinaryExpression s)
        {
            s._expr1.Accept(this);
            s._expr2.Accept(this);
        }

        public void Visit(BlockStatement s)
        {
            foreach (var statement in s._statements)
            {
                statement.Accept(this);
            }
        }

        public void Visit(BreakStatement s)
        {
        }

        public void Visit(ConditionalExpression s)
        {
            s._expression1.Accept(this);
            s._expression2.Accept(this);
        }

        public void Visit(ContinueStatement s)
        {   
        }

        public void Visit(DoWhileStatement s)
        {
            s._condition.Accept(this);
            s._statement.Accept(this);
        }

        public void Visit(ForStatement s)
        {
            s._initialization.Accept(this);
            s._termination.Accept(this);
            s._increment.Accept(this);
            s._statement.Accept(this);
        }

        public void Visit(FunctionDefineStatement s)
        {
            s._body.Accept(this);
        }

        public void Visit(FunctionStatement s)
        {
            s._function.Accept(this);
        }

        public void Visit(FunctionalExpression s)
        {
            foreach (var arg in s._arguments)
            {
                arg.Accept(this);
            }
        }

        public void Visit(IfStatement s)
        {
            s._expression.Accept(this);
            s._ifStatement.Accept(this);
            if (s._elseStatement != null)
            {
                s._elseStatement.Accept(this);
            }
        }

        public void Visit(PrintStatement s)
        {
            s._expression.Accept(this);
        }

        public void Visit(ReturnStatement s)
        {
            s._expression.Accept(this);
        }

        public void Visit(TernaryExpression s)
        {
            s._condition.Accept(this);
            s._trueExpr.Accept(this);
            s._falseExpr.Accept(this);
        }

        public void Visit(UnaryExpression s)
        {
            s._expr1.Accept(this);
        }

        public void Visit(ValueExpression s)
        {
            
        }

        public void Visit(VariableExpression s)
        {
            
        }

        public void Visit(WhileStatement s)
        {
            s._condition.Accept(this);
            s._statement.Accept(this);
        }

        public void Visit(UseStatement s)
        {
            s._expression.Accept(this);
        }
    }
}
