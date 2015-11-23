using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSmith.Abp.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string ToDelimitedString(this IEnumerable<string> values, string delimiter, string format = null)
        {
            var sb = new StringBuilder();
            foreach (var i in values)
            {
                if (sb.Length > 0)
                    sb.Append(delimiter);

                if (string.IsNullOrEmpty(format))
                    sb.Append(i);
                else
                    sb.AppendFormat(format, i);
            }

            return sb.ToString();
        }
    }
}
