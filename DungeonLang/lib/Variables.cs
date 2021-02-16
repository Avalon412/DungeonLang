using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class Variables
    {
        private static Dictionary<string, IValue> _variables;
        private static readonly Stack<Dictionary<string, IValue>> _stack;

        static Variables()
        {
            _stack = new Stack<Dictionary<string, IValue>>();
            _variables = new Dictionary<string, IValue>();
            _variables.Add("PI", new NumberValue(Math.PI));
            _variables.Add("ПИ", new NumberValue(Math.PI));
            _variables.Add("E", new NumberValue(Math.E));
            _variables.Add("GOLDEN_RATIO", new NumberValue(1.618));
        }

        public static void Push()
        {
            _stack.Push(new Dictionary<string, IValue>(_variables));
        }

        public static void Pop()
        {
            _variables = _stack.Pop();
        }

        public static bool IsExist(string key)
        {
            return _variables.ContainsKey(key);
        }

        public static IValue Get(string key)
        {
            if (!IsExist(key)) return NumberValue.ZERO;
            IValue value;
            _variables.TryGetValue(key, out value);
            return value;
        }

        public static void Set(string key, IValue value)
        {
            _variables[key] = value;
        }
    }
}
