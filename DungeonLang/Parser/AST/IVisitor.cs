using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public interface IVisitor
    {
        void Visit(ArrayAccessExpression s);
        void Visit(ArrayAssignmentStatement s);
        void Visit(ArrayExpression s);
        void Visit(AssignmentStatement s);
        void Visit(BinaryExpression s);
        void Visit(BlockStatement s);
        void Visit(BreakStatement s);
        void Visit(ConditionalExpression s);
        void Visit(ContinueStatement s);
        void Visit(DoWhileStatement s);
        void Visit(ForStatement s);
        void Visit(FunctionDefineStatement s);
        void Visit(FunctionStatement s);
        void Visit(FunctionalExpression s);
        void Visit(IfStatement s);
        void Visit(PrintStatement s);
        void Visit(ReturnStatement s);
        void Visit(UnaryExpression s);
        void Visit(ValueExpression s);
        void Visit(VariableExpression s);
        void Visit(WhileStatement s);
    }
}
