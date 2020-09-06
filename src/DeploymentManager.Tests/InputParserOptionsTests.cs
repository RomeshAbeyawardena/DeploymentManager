using DeploymentManager.Domains;
using DeploymentManager.Shared.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Tests
{
    [TestFixture]
    public class InputParserOptionsTests
    {
        [Test]
        public void Combine()
        {
            sut.InputSeparatorGroups = new [] { '_' };

            var result = sut.Combine(InputParserOptions.Default);
            Assert.Contains('"', result.InputQuoteGroups.ToArray());
            Assert.Contains('\'', result.InputQuoteGroups.ToArray());
            Assert.Contains(' ', result.InputSeparatorGroups.ToArray());
            Assert.Contains('_', result.InputSeparatorGroups.ToArray());
        }

        [SetUp]
        public void SetUp()
        {
            sut = new InputParserOptions();
        }

        private InputParserOptions sut;
    }
}
