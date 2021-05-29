using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonLang.lib.modules
{
    public class std : IModule
    {
        public void Init()
        {
            Functions.SetFunction("echonl", args =>
            {
                foreach (IValue arg in args)
                {
                    Console.WriteLine(arg.AsString());
                }
                return NumberValue.ZERO;
            });
            Functions.SetFunction("echo", args =>
            {
                foreach (IValue arg in args)
                {
                    Console.Write(arg.AsString());
                }
                return NumberValue.ZERO;
            });
            Functions.SetFunction("cls", args =>
            {
                Console.Clear();
                return NumberValue.ZERO;
            });
            Functions.SetFunction("padd", args =>
            {
                if (args.Length == 1)
                {
                    return new StringValue("".PadLeft((int)args[0].AsNumber()));
                }
                return NumberValue.ZERO;
            });
            Functions.SetFunction("newarray", args =>
            {
                return CreateArray(args, 0);
            });
            Functions.SetFunction("rand", new Rand().Execute);
            Functions.SetFunction("sleep", args =>
            {
                if (args.Length == 1)
                {
                    try
                    {
                        Thread.Sleep((int)args[0].AsNumber());
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Thread.CurrentThread.Interrupt();
                    }
                }
                return NumberValue.ZERO;
            });
            Functions.SetFunction("thread", args =>
            {
                if (args.Length == 1)
                {
                    new Thread(() =>
                    {
                        Functions.GetFunction(args[0].AsString()).Invoke();
                    }).Start();
                }
                return NumberValue.ZERO;
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

        private class Rand : IFunction
        {
            private static readonly Random _rnd = new Random();

            public IValue Execute(params IValue[] args)
            {
                int from = 0;
                int to = 100;
                if (args.Length == 1)
                {
                    to = (int)args[0].AsNumber();
                }
                else if (args.Length == 2)
                {
                    from = (int)args[0].AsNumber();
                    to = (int)args[1].AsNumber();
                }
                return new NumberValue(_rnd.Next(to - from) + from);
            }
        }
    }
}
