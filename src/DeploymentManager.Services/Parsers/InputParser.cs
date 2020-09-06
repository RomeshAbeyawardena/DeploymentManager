using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Parsers;
using DeploymentManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Parsers
{
    public class InputParser : IInputParser
    {
        public IInputGroup Parse(string input)
        {
            bool withinQuotes = false;
            var currentWord = string.Empty;
            var parsedInputList = new List<string>();
            for (var index = 1; index <= input.Length; index++)
            {
                var inputCharacter = input[index - 1];

                if(inputCharacter == '\'' || inputCharacter == '"')
                {
                    withinQuotes = !withinQuotes;
                    continue;
                }

                if(!withinQuotes && inputCharacter == ' ')
                {
                    parsedInputList.Add(currentWord);
                    currentWord =  string.Empty;
                    continue;
                }

                currentWord += inputCharacter;
            }

            parsedInputList.Add(currentWord);

            return new InputGroup(parsedInputList.ToArray());
        }
    }
}
