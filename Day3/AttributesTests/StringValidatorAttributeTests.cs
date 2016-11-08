using Microsoft.VisualStudio.TestTools.UnitTesting;
using Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Attributes.Tests
{
    [TestClass]
    public class StringValidatorAttributeTests
    {
        [TestMethod]
        public void StringValidatorAttributeTest()
        {
            var us = new User(10);
            Trace.Write(@"1231");

            us.FirstName = "123123213213123123123213123213213213444";
            Assert.AreEqual(true, true);
        }

    }
}