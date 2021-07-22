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
    public class RenderContextTests
    {
        [TestMethod()]
        public void AddComponentTest()
        {
            var renderContext = new RenderContext();
            var html = "<div>{{msg}}</div>";
            var js = @"
() => ({
   data() {
    return{
        msg: 'Hello';
    }
  },
});
";
            renderContext.AddComponent("Home", html, js, string.Empty);
        }

        [TestMethod()]
        public void RenderTest()
        {
            var renderContext = new RenderContext();
            var html = "<div>{{msg}}</div>";
            var js = @"
() => ({
  data() {
    return{
        msg: 'Hello';
    }
  },
});
";
            var result = renderContext.Render(html, js, string.Empty);
        }
    }
}