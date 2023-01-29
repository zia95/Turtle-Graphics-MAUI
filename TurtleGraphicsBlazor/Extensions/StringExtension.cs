using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleGraphicsBlazor
{
    public static class StringExtension
    {
        public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
        {
            if(string.IsNullOrEmpty(value)) return value;

            return value?.Length > maxLength
                ? value.Substring(0, maxLength) + truncationSuffix
                : value;
        }
    }
}
