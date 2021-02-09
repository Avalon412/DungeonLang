using DungeonLang.lib;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ConstantExpression : Expression
    {
        private readonly string _name;

        public ConstantExpression(string name)
        {
            this._name = name;
        }

        public double Evaluate()
        {
            if (!Constants.IsExist(_name)) throw new RuntimeException("Constant does not exist");
            return Constants.Get(_name);
        }

        public override string ToString()
        {
            return $"{_name}";
        }
    }
}
