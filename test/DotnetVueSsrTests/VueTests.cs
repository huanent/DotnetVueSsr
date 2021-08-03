using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotnetVueSsr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native.Object;

namespace DotnetVueSsr.Tests
{
    [TestClass()]
    public class VueTests
    {
        [TestMethod()]
        public void RenderVueComponentToStringTest()
        {
            var engine = new Engine();
            var vm = engine.Evaluate(@"
({
    template:`<div @click='aa'>{{msg}}</div>`,
    data:{
        msg:`hello`
    },
    methods:{
        aa(){
            // not render
        }
    }
})
") as ObjectInstance;
            var result = Vue.RenderVueComponentToString(engine, vm);
            Assert.AreEqual(result, "<div data-server-rendered=\"true\">hello</div>");
        }

        [TestMethod()]
        public void RenderVueComponentToStringByCode()
        {
            var engine = new Engine();
            var code = @"
({
    template:`<div @click='aa'>{{msg}}</div>`,
    data:{
        msg:`hello`
    }
})
";
            var result = Vue.RenderVueComponentToString(engine, code);
            Assert.AreEqual(result, "<div data-server-rendered=\"true\">hello</div>");
        }

    }
}