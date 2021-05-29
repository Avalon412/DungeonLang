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
        }
    }
}
