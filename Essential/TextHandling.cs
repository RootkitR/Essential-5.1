using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential
{
    class TextHandling
    {
        internal static string GetString(double double_)
        {
            return double_.ToString(CustomCultureInfo.GetCustomCultureInfo());
        }
    }
}
