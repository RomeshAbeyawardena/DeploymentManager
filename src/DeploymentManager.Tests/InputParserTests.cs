using DeploymentManager.Domains;
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
            sut = new InputParser(InputParserOptions.Default);
        }

        
        [TestCase("Test test monday", new [] { "Test", "test", "monday" }),
            TestCase("Test \"test monday\"", new [] { "Test", "test monday" }),
            TestCase("Test \"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus malesuada sapien in eleifend euismod. Cras volutpat tortor ac erat lacinia posuere. Nulla eu sollicitudin tortor, et dignissim augue. Praesent commodo, tellus id laoreet rutrum, metus nibh venenatis sapien, sed dignissim lorem lacus et velit. Nullam interdum eu ex eu fringilla. Curabitur nec sagittis mauris, a venenatis risus. Sed id ligula eu turpis maximus fermentum. Duis at enim malesuada, dapibus magna feugiat, molestie mauris. Donec mollis dignissim risus ut varius. \"", new [] { "Test", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus malesuada sapien in eleifend euismod. Cras volutpat tortor ac erat lacinia posuere. Nulla eu sollicitudin tortor, et dignissim augue. Praesent commodo, tellus id laoreet rutrum, metus nibh venenatis sapien, sed dignissim lorem lacus et velit. Nullam interdum eu ex eu fringilla. Curabitur nec sagittis mauris, a venenatis risus. Sed id ligula eu turpis maximus fermentum. Duis at enim malesuada, dapibus magna feugiat, molestie mauris. Donec mollis dignissim risus ut varius. " })]
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
