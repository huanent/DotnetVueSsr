using Esprima;
using Esprima.Ast;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotnetVueSsr
{
    public class RenderContext
    {
        static readonly Script libs = new JavaScriptParser(ScriptResource.Load()).ParseScript();

        public Engine Engine { get; }

        public RenderContext() : this(new Engine())
        {

        }

        public RenderContext(Engine engine)
        {
            Engine = engine;
            Engine.Execute(libs);
        }


        public void AddComponent(string name, string html, string js, string css)
        {
            var func = Engine.Evaluate(js) as ScriptFunctionInstance;
            var vm = func.Call(null, Array.Empty<JsValue>());
            vm.AsObject().Set("template", html);
            var Vue = Engine.GetValue("Vue");
            var vueCompoment = Vue.Get("component") as ScriptFunctionInstance;
            vueCompoment.Call(Vue, new[] { name, vm });
        }

        public string Render(string html, string js, string css)
        {
            var vm = (Engine.Evaluate(js) as ScriptFunctionInstance).Call(null, Array.Empty<JsValue>());
            vm.AsObject().Set("template", html);
            var renderVueComponentToString = Engine.GetValue("renderVueComponentToString") as ScriptFunctionInstance;
            var result = string.Empty;
            var vue = Engine.Evaluate("(vm)=>new Vue(vm)") as ScriptFunctionInstance;
            vm = vue.Call(null, new[] { vm });

            var callBack = new Action<JsValue, JsValue>((err, res) =>
            {
                result = res?.AsString();
            });

            renderVueComponentToString.Call(null, new[] { vm, JsValue.FromObject(Engine, callBack) });
            return result;
        }
    }
}
