﻿using DeploymentManager.Contracts;
using DeploymentManager.Domains;
using DNI.Core.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Shared.Extensions
{
    public static class InputParserOptionsExtensions
    {
        public static IInputParserOptions Combine(this IInputParserOptions value, IInputParserOptions options)
        {
            var inputQuoteGroupList = value.InputQuoteGroups == null 
                ? new List<char>() 
                : value.InputQuoteGroups.ToList();

            var inputSeparatorGroupList = value.InputSeparatorGroups == null 
                ? new List<char>() 
                : value.InputSeparatorGroups.ToList();


            Action<IEnumerable<char>, IList<char>> Combine = (array, list) =>
            {
                array.ForEach(optionQuoteGroupItem =>  {
                if(!list.Contains(optionQuoteGroupItem)) 
                    {  
                        list.Add(optionQuoteGroupItem); 
                    } 
                });
            };

            Combine(options.InputQuoteGroups, inputQuoteGroupList);
            Combine(options.InputSeparatorGroups, inputSeparatorGroupList);

            return new InputParserOptions {
                InputQuoteGroups = inputQuoteGroupList.ToArray(),
                InputSeparatorGroups = inputSeparatorGroupList.ToArray()
            };
        }
    }
}
