using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLang.lib
{
    public sealed class Constants
    {
        private static readonly Dictionary<string, double> _constants;

        static Constants()
        {
            _constants = new Dictionary<string, double>();
            _constants.Add("PI", Math.PI);
            _constants.Add("ПИ", Math.PI);
            _constants.Add("E", Math.E);
            _constants.Add("GOLDEN_RATIO", 1.618);
        }

        public static bool IsExist(string key)
        {
            return _constants.ContainsKey(key);
        }

        public static double Get(string key)
        {
            if (!IsExist(key)) return 0;
            double value;
            _constants.TryGetValue(key, out value);
            return value;
        }
    }
}
