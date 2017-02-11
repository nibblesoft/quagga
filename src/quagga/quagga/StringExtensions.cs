using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quagga
{
    public static class StringExtensions
    {
        public static bool Contains(this string s, string pattern, StringComparison comparison)
        {
            return s.IndexOf(pattern, comparison) >= 0;
        }
    }
}
