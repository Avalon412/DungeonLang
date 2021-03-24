using DungeonLang.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.Parser.AST
{
    public sealed class ArrayAccessExpression : IExpression
    {
        private readonly string _variable;
        private readonly List<IExpression> _indices;

        public ArrayAccessExpression(string variable, List<IExpression> indices)
        {
            this._variable = variable;
            this._indices = indices;
        }

        public IValue Evaluate()
        {
            return GetArray().Get(LastIndex());
        }

        public ArrayValue GetArray()
        {
            ArrayValue array = ConsumeArray(Variables.Get(_variable));
            int last = _indices.Count - 1;
            for (int i = 0; i < last; i++)
            {
                array = ConsumeArray(array.Get(Index(i)));
            }
            return array;
        }

        private int Index(int index)
        {
            return (int)_indices[index].Evaluate().AsNumber();
        }

        public int LastIndex()
        {
            return Index(_indices.Count - 1);
        }

        private ArrayValue ConsumeArray(IValue value)
        {
            if (value is ArrayValue)
            {
                return (ArrayValue)value;
            }
            else
            {
                throw new RuntimeExpression("Array expected");
            }
        }

        public override string ToString()
        {
            return _variable + _indices;
        }
    }
}
