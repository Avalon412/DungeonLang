using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class FunctionalExpression : Expression
    {
        private readonly string _name;
        private readonly List<Expression> _arguments;

        public FunctionalExpression(string name)
        {
            this._name = name;
            this._arguments = new List<Expression>();
        }

        public FunctionalExpression(string name, List<Expression> arguments)
        {
            this._name = name;
            this._arguments = arguments;
        }

        public void AddArgument(Expression arg)
        {
            _arguments.Add(arg);
        }

        public Value Evaluate()
        {
            int size = _arguments.Count;
            Value[] values = new Value[size];
            for (int i = 0; i < size; i++)
            {
                values[i] = _arguments[i].Evaluate();
            }
            return Functions.GetFunction(_name).Invoke(values);
        }

        public override string ToString()
        {
            return $"{_name} ({_arguments.ToString()})";
        }
    }
}
