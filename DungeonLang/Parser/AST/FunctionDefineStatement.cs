using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class FunctionDefineStatement : IStatement
    {
        public readonly string _name;
        public readonly List<string> _argNames;
        public readonly IStatement _body;

        public FunctionDefineStatement(string name, List<string> argNames, IStatement body)
        {
            this._name = name;
            this._argNames = argNames;
            this._body = body;
        }

        public void Execute()
        {
            Functions.SetFunction(_name, new UserDefinedFunction(_argNames, _body).Execute);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"def ({_argNames.ToString()}) {_body.ToString()}";
        }
    }
}
