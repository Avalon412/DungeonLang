using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class Variables
    {
        private static readonly NumberValue ZERO = new NumberValue(0);
        private static readonly Dictionary<string, Value> _variables;

        static Variables()
        {
            _variables = new Dictionary<string, Value>();
            _variables.Add("PI", new NumberValue(Math.PI));
            _variables.Add("ПИ", new NumberValue(Math.PI));
            _variables.Add("E", new NumberValue(Math.E));
            _variables.Add("GOLDEN_RATIO", new NumberValue(1.618));
        }

        public static bool IsExist(string key)
        {
            return _variables.ContainsKey(key);
        }

        public static Value Get(string key)
        {
            if (!IsExist(key)) return ZERO;
            Value value;
            _variables.TryGetValue(key, out value);
            return value;
        }

        public static void Set(string key, Value value)
        {
            _variables[key] = value;
        }
    }
}
