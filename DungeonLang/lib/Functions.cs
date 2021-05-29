using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public delegate IValue Function(params IValue[] args);

    public sealed class Functions
    {
        public static readonly Dictionary<string, Function> _functions;
        
        static Functions()
        {
            _functions = new Dictionary<string, Function>();
        }

        public static bool IsExist(string key)
        {
            return _functions.ContainsKey(key);
        }

        public static Function GetFunction(string key)
        {
            if (!IsExist(key)) throw new RuntimeException("Unknown function " + key);
            Function func;
            _functions.TryGetValue(key, out func);
            return func;
        }

        public static void SetFunction(string key, Function function)
        {
            _functions.Add(key, function);
        }
    }
}
