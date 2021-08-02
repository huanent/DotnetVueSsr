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
            var aa = engine.Evaluate(@"
({
    template:`<div @click='aa'>{{msg}}</div>`,
    data:{
        msg:`hello`
    },
    mounted(){
      aaaaa=`bbb`
    },
    methods:{
        aa(){
            
        }
    }
})
") as ObjectInstance;
            var result = Vue.RenderVueComponentToString(engine, aa);
        }
    }
}