using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotnetVueSsr
{
    internal class ScriptResource
    {
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
    }
}
