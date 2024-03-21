using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyAnalyzer
{
    public class AnalyzerResult
    {
        public string AssemblyName { get; set; }
        public ConcurrentDictionary<string, NamespaceInfo> namespaces { get; set; }   
        public AnalyzerResult(string assemblyName)
        {
            this.AssemblyName = assemblyName;
            namespaces = new ConcurrentDictionary<string, NamespaceInfo>();
        }
    }

    public class NamespaceInfo
    {
        public string namespaceName { get; set; }
        public List<TypeInfo> typeInfos { get; set; }
        public NamespaceInfo(string namespaceName)
        {
            this.namespaceName = namespaceName;
            typeInfos = new List<TypeInfo>();
        }   

        public void AddType(TypeInfo type)
        {
            type.fields = new List<FieldInfo>(type.type.GetFields());
            type.properties = new List<PropertyInfo>(type.type.GetProperties());
            type.AddMethods(type.type.GetMethods());
            typeInfos.Add(type);
        }
    }

    public class TypeInfo
    {
        public Type type { get; set; }
        public List<MethodInfo> methods { get; set; }
        public List<FieldInfo> fields { get; set; }
        public List<PropertyInfo> properties { get; set; }

        public TypeInfo(Type type)
        {
            this.type = type;
            methods = new List<MethodInfo>();
            fields = new List<FieldInfo>();
            properties = new List<PropertyInfo>();
        }
        
        public void AddMethods(System.Reflection.MethodInfo[] methods)
        {
            foreach(var method in methods)
            {
                this.methods.Add(new MethodInfo(
                    method.Name,
                    method.ReturnType.Name,
                    method.GetParameters().Select(p => p.ParameterType.Name).ToList()
                ));
            }
        }
    }

    public class MethodInfo
    {
        public string Name { get; set; }
        public string returnType { get; set; }
        public List<string> paramTypes { get; set; }

        public MethodInfo(string name, string returnType, List<string> paramTypes)
        {
            this.Name = name;
            this.returnType = returnType;
            this.paramTypes = paramTypes;
        }
    }
}
