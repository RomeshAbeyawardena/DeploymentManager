using DeploymentManager.Services.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Tests
{
    [TestFixture]
    public class InputParserTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new InputParser();
        }

        [TestCase("Test test monday", new [] { "Test", "test", "monday" })]
        [TestCase("Test \"test monday\"", new [] { "Test", "test, monday" })]
        public void Parse(string input, string[] expected)
        {
            var result = sut.Parse(input);
            foreach(var expect in expected)
            {
                Assert.Contains(expect, result.ParsedValues.ToArray());
            }
        }

        private InputParser sut;
    }
}
