using DungeonLang.lib;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class VariableExpression : IExpression
    {
        private readonly string _name;

        public VariableExpression(string name)
        {
            this._name = name;
        }

        public IValue Evaluate()
        {
            if (!Variables.IsExist(_name)) throw new RuntimeException("Constant does not exist");
            return Variables.Get(_name);
        }

        public override string ToString()
        {
            return $"{_name}";
        }
    }
}
