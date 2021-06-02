using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib.modules
{
    public class math : IModule
    {
        public void Init()
        {
            Functions.SetFunction("sin", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Sin(args[0].AsNumber()));
            });
            Functions.SetFunction("cos", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Cos(args[0].AsNumber()));
            });
            Functions.SetFunction("tan", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Tan(args[0].AsNumber()));
            });
            Functions.SetFunction("ctan", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(1 / Math.Tan(args[0].AsNumber()));
            });
            Functions.SetFunction("asin", args => {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Asin(args[0].AsNumber()));
            });
            Functions.SetFunction("acos", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Acos(args[0].AsNumber()));
            });
            Functions.SetFunction("atan", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Atan(args[0].AsNumber()));
            });
            Functions.SetFunction("actan", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue((Math.PI / 2) - Math.Atan(args[0].AsNumber()));
            });
            Functions.SetFunction("abs", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Abs(args[0].AsNumber()));
            });
            Functions.SetFunction("sqrt", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(Math.Sqrt(args[0].AsNumber()));
            });
            Functions.SetFunction("pow", args =>
            {
                if (args.Length != 2) throw new RuntimeException("Two arg exprected");
                return new NumberValue(Math.Pow(args[0].AsNumber(), args[1].AsNumber()));
            });
            Functions.SetFunction("toDegrees", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(args[0].AsNumber() * (180 / Math.PI));
            });
            Functions.SetFunction("toRadians", args =>
            {
                if (args.Length != 1) throw new RuntimeException("One arg exprected");
                return new NumberValue(args[0].AsNumber() * (Math.PI / 180));
            });
        }
    }
}
