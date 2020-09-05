using DNI.Core.Contracts.Builders;
using DNI.Core.Services.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<IDictionaryBuilder<TKey, TValue>> builderAction)
        {
            var dictionaryBuilder = DictionaryBuilder<TKey, TValue>.Create();
            builderAction(dictionaryBuilder);
            foreach(var(key, value) in dictionaryBuilder.Dictionary)
            {
                dictionary.TryAdd(key, value);
            }
            
            return dictionary;
        }
    }
}
