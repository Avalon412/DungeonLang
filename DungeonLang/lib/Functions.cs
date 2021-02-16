using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class Functions
    {
        public delegate IValue Function(params IValue[] args);
        private static readonly Dictionary<string, Function> _functions;
        
        static Functions()
        {
            _functions = new Dictionary<string, Function>();
            _functions.Add("sin", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Sin(args[0].AsNumber()));
            });
            _functions.Add("cos", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Cos(args[0].AsNumber()));
            });
            _functions.Add("echo", args =>
            {
                foreach (IValue arg in args)
                {
                    Console.WriteLine(arg.AsString());
                }
                return NumberValue.ZERO;
            });
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
