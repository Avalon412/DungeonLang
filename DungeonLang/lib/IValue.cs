using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public interface IValue
    {
        double AsNumber();
        string AsString();
    }
}
