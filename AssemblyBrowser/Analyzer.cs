using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyAnalyzer
{
    public class Analyzer
    {
        public static AnalyzerResult Analyze(string path)
        {
            Assembly currAssembly = null;
            try
            {
                currAssembly = Assembly.LoadFrom(path);
            } catch (Exception ex)
            {
                throw new Exception("Failed to load assembly");
            }
            AnalyzerResult assemblyInfo = new AnalyzerResult(currAssembly.GetName().FullName);
            Type[] types = currAssembly.GetTypes();
            foreach (Type type in types)
            {
                if (!(type.Namespace == null)) {
                /*if (!type.IsDefined(typeof(CompilerGeneratedAttribute), false))*/
                    if (type.Namespace != "System.Runtime.CompilerServices" && type.Namespace != "Microsoft.CodeAnalysis")
                    {
                        string Namespace = type.Namespace;
                        NamespaceInfo info = assemblyInfo.namespaces.GetOrAdd(Namespace, new NamespaceInfo(Namespace));
                        info.AddType(new TypeInfo(type));
                    }
                }
            }
            return assemblyInfo;
        }
    }
}
