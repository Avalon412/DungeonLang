using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class FunctionalExpression : IExpression
    {
        private readonly string _name;
        private readonly List<IExpression> _arguments;

        public FunctionalExpression(string name)
        {
            this._name = name;
            this._arguments = new List<IExpression>();
        }

        public FunctionalExpression(string name, List<IExpression> arguments)
        {
            this._name = name;
            this._arguments = arguments;
        }

        public void AddArgument(IExpression arg)
        {
            _arguments.Add(arg);
        }

        public IValue Evaluate()
        {
            int size = _arguments.Count;
            IValue[] values = new IValue[size];
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
