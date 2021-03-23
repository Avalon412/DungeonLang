using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ArrayExpression : IExpression
    {
        private readonly List<IExpression> _elements;

        public ArrayExpression(List<IExpression> arguments)
        {
            this._elements = arguments;
        }

        public IValue Evaluate()
        {
            int size = _elements.Count;
            ArrayValue array = new ArrayValue(size);
            for (int i = 0; i < size; i++)
            {
                array.Set(i, _elements[i].Evaluate());
            }
            return array;
        }

        public override string ToString()
        {
            return _elements.ToString();
        }
    }
}
