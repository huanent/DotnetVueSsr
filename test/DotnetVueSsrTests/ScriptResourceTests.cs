using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotnetVueSsr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetVueSsr.Tests
{
    [TestClass()]
    public class ScriptResourceTests
    {
        [TestMethod()]
        public void LoadTest()
        {
            var result = ScriptResource.Load();
            Assert.IsNotNull(result);
        }
    }
}