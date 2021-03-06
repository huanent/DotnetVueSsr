using Esprima.Ast;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotnetVueSsr
{
    public static class Vue
    {
        static readonly Engine engine = new();
        static readonly ScriptFunctionInstance renderVueComponentToString;
        static readonly ScriptFunctionInstance createVueRoot;

        static Vue()
        {
            engine.Execute(Load());

            engine.SetValue("process", new Dictionary<string, object> {
                {"env",new Dictionary<string,object>{
                        {"VUE_ENV","server"},
                        {"NODE_ENV","production"},
                    }
                }
            });

            renderVueComponentToString = engine.GetValue("renderVueComponentToString") as ScriptFunctionInstance;
            createVueRoot = engine.Evaluate("(vm)=>new Vue(vm)") as ScriptFunctionInstance;
        }

        public static string RenderVueComponentToString(Engine scopeEngine, ObjectInstance rootComponent)
        {
            var globalObject = scopeEngine.Realm.GlobalObject;
            var vm = createVueRoot.Call(globalObject, new[] { rootComponent });
            var result = string.Empty;

            var callBack = new Action<JsValue, JsValue>((err, res) =>
            {
                if (!err.IsNull()) throw new JavaScriptException(err);
                result = res?.AsString();
            });

            renderVueComponentToString.Call(globalObject, new[] { vm, JsValue.FromObject(scopeEngine, callBack) });
            return result;
        }

        public static string RenderVueComponentToString(Engine scopeEngine, string code)
        {
            var script = scopeEngine.Evaluate(code) as ObjectInstance;
            if (script == null) throw new NotComponentException(code);
            return RenderVueComponentToString(scopeEngine, script);
        }

        public static string RenderVueComponentToString(Engine scopeEngine, Script script)
        {
            var vm = scopeEngine.Evaluate(script) as ObjectInstance;
            return RenderVueComponentToString(scopeEngine, vm);
        }

        internal static string Load()
        {
            var assembly = Assembly.GetAssembly(typeof(Vue));
            var resources = assembly.GetManifestResourceNames();

            var scripts = resources.Where(w => w.StartsWith("DotnetVueSsr.scripts") && w.EndsWith(".js"))
                                   .OrderBy(o => o);

            var scriptBuilder = new StringBuilder();

            foreach (var item in scripts)
            {
                using var stream = assembly.GetManifestResourceStream(item);
                var script = new StreamReader(stream).ReadToEnd();
                scriptBuilder.AppendLine(script);
            }

            return scriptBuilder.ToString();
        }
    }
}
