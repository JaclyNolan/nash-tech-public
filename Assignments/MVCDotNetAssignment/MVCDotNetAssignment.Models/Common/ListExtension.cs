using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDotNetAssignment.Domain.Common
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty(this IEnumerable<Object> list)
        {
            return list?.ToList().Count == 0;
        }
        public static bool IsNullOrEmpty(this List<Object> list)
        {
            return list?.ToList().Count == 0;
        }
    }
}
