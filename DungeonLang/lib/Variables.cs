using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class Variables
    {
        private static readonly Dictionary<string, double> _variables;

        static Variables()
        {
            _variables = new Dictionary<string, double>();
            _variables.Add("PI", Math.PI);
            _variables.Add("ПИ", Math.PI);
            _variables.Add("E", Math.E);
            _variables.Add("GOLDEN_RATIO", 1.618);
        }

        public static bool IsExist(string key)
        {
            return _variables.ContainsKey(key);
        }

        public static double Get(string key)
        {
            if (!IsExist(key)) return 0;
            double value;
            _variables.TryGetValue(key, out value);
            return value;
        }

        public static void Set(string key, double value)
        {
            _variables.Add(key, value);
        }
    }
}
