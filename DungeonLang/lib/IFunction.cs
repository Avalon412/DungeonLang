using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public interface IFunction
    {
        IValue Execute(params IValue[] args);
    }
}
