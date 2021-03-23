using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class ArrayValue : IValue
    {
        private readonly IValue[] _elements;

        public ArrayValue(int size)
        {
            this._elements = new IValue[size];
        }

        public ArrayValue(IValue[] elements)
        {
            this._elements = new IValue[elements.Length];
            Array.Copy(elements, 0, this._elements, 0, elements.Length);
        }

        public ArrayValue(ArrayValue array) : this(array._elements) { }

        public IValue Get(int index)
        {
            return _elements[index];
        }

        public void Set(int index, IValue value)
        {
            _elements[index] = value;
        }

        public double AsNumber()
        {
            throw new RuntimeException("Cannot cast array to number");
        }

        public string AsString()
        {
            return Arrays.ToString(_elements);
        }

        public override string ToString()
        {
            return AsString();
        }
    }
}
