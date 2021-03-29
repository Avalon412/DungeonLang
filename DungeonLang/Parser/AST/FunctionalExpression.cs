using DungeonLang.lib;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class FunctionalExpression : IExpression
    {
        public readonly string _name;
        public readonly List<IExpression> _arguments;

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

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IValue Evaluate()
        {
            int size = _arguments.Count;
            IValue[] values = new IValue[size];
            for (int i = 0; i < size; i++)
            {
                values[i] = _arguments[i].Evaluate();
            }
            Function function = Functions.GetFunction(_name);
            if (function.Target is UserDefinedFunction)
            {
                var invokeInstance = function.Target as UserDefinedFunction;
                if (size != invokeInstance.GetArgsCount()) throw new RuntimeException("Args count mismatch");

                Variables.Push();
                for (int i = 0; i < size; i++)
                {
                    Variables.Set(invokeInstance.GetArgNames(i), values[i]);
                }
                IValue result = function.Invoke(values);
                Variables.Pop();
                return result;
            }
            return function.Invoke(values);
        }

        public override string ToString()
        {
            return $"{_name} ({_arguments.ToString()})";
        }
    }
}
