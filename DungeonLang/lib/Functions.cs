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
            _functions.Add("newarray", args =>
            {
                return CreateArray(args, 0);
            });
        }

        private static ArrayValue CreateArray(IValue[] args, int index)
        {
            int size = (int)args[index].AsNumber();
            int last = args.Length - 1;
            ArrayValue array = new ArrayValue(size);
            if (index == last)
            {
                for (int i = 0; i < size; i++)
                {
                    array.Set(i, NumberValue.ZERO);
                }
            }
            else if (index < last)
            {
                for (int i = 0; i < size; i++)
                {
                    array.Set(i, CreateArray(args, index + 1));
                }
            }
            return array;
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
