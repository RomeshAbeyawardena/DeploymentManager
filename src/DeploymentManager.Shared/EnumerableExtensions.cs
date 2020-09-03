using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Shared
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> values, int index)
        {
            var valueList = values.ToList();
            valueList.RemoveAt(index);
            return valueList;
        }
    }
}
