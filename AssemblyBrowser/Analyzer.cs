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
            } catch
            {
                throw new Exception("Failed to load assembly");
            }
            AnalyzerResult assemblyInfo = new AnalyzerResult(currAssembly.GetName().FullName);
            Type[] types = currAssembly.GetTypes();
            foreach (Type type in types)
            {
                string nSpace = type.Namespace;
                if (nSpace == null)
                {
                    nSpace = "Not presented";
                }
                try
                {   
                    if (type.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                    {
                        assemblyInfo.AddType(type, nSpace);
                    }
                } catch (FileNotFoundException ex)
                {
                    assemblyInfo.AddType(type, nSpace);
                }
            }
            return assemblyInfo;
        }
    }

    [Serializable]
    public class AnalyzerException: Exception 
    {
        public AnalyzerException() { }

        public AnalyzerException(string message) : base(message)
        {

        }

    }

}
