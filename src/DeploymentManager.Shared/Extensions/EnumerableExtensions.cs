using DeploymentManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> values, int index)
        {
            var valueList = values.ToList();
            valueList.RemoveAt(index);
            return valueList;
        }
        
        public static IDictionary<string, string> ToDictionary(this IEnumerable<IParameter> parameters)
        {
            return parameters.ToDictionary(parameter => parameter.Name, parameter => parameter.Value);
        }
    }
}
