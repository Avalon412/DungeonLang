using DungeonLang.Parser.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class UserDefinedFunction : IFunction
    {
        private readonly List<string> _argNames;
        private readonly IStatement _body;

        public UserDefinedFunction(List<string> args, IStatement body)
        {
            this._argNames = args;
            this._body = body;
        }

        public int GetArgsCount()
        {
            return _argNames.Count;
        }

        public string GetArgNames(int index)
        {
            return _argNames[index];
        }

        public IValue Execute(params IValue[] args)
        {
            try
            {
                _body.Execute();
                return NumberValue.ZERO;
            }
            catch (ReturnStatement rt)
            {
                return rt.GetResult();
            }
        }
    }
}
