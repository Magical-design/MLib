using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLib
{
    public class IConvert
    {
        public static bool[] ShortToBoolArray(short[] _short)
        {
            return _short.SelectMany(s => Enumerable.Range(0, 16).Select(i => (s & (1 << i)) != 0)).ToArray();
        }
        public static short[] BoolToShortArray(bool[] _bool)
        {
            return Enumerable.Range(0, _bool.Length / 16).Select(i => (short)_bool.Select(b => b ? 1 : 0).Skip(i * 16).Take(16).ToArray().Reverse().Aggregate((k, j) => 2 * k + j)).ToArray();
        }

    }
}
