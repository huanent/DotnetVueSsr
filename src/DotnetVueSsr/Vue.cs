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
using System.Threading.Tasks;

namespace DotnetVueSsr
{
    public static class Vue
    {
        static readonly Engine engine = new();
        static readonly ScriptFunctionInstance renderVueComponentToString;
        static readonly ScriptFunctionInstance createVueRoot;
        internal static string Load()
        {
            var assembly = Assembly.GetAssembly(typeof(RenderContext));
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

        static Vue()
        {
            engine.Execute(Load());
            renderVueComponentToString = engine.GetValue("renderVueComponentToString") as ScriptFunctionInstance;
            createVueRoot = engine.Evaluate("(vm)=>new Vue(vm)") as ScriptFunctionInstance;
        }

        public static string RenderVueComponentToString(Engine scopeEngine, ObjectInstance rootComponent)
        {
            var vm = createVueRoot.Call(scopeEngine.Realm.GlobalObject, new[] { rootComponent });
            var result = string.Empty;

            var callBack = new Action<JsValue, JsValue>((err, res) =>
            {
                if (!err.IsNull()) throw new JavaScriptException(err);
                result = res?.AsString();
            });

            renderVueComponentToString.Call(scopeEngine.Realm.GlobalObject, new[] { vm, JsValue.FromObject(scopeEngine, callBack) });
            return result;
        }
    }
}
