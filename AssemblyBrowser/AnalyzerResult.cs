using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AssemblyAnalyzer
{
    public class AnalyzerResult
    {
        public string AssemblyName { get; set; }
        public ICollection<NamespaceInfo> Namespaces {
            get => _namesapces.Values;
        }
        public ConcurrentDictionary<string, NamespaceInfo> NamespacesDictionary { get => _namesapces; }

        private ConcurrentDictionary<string, NamespaceInfo> _namesapces;
        public void AddType(Type type, string nSpace)
        {
            NamespaceInfo nsp = _namesapces.GetOrAdd(nSpace, new NamespaceInfo(nSpace));
            nsp.AddType(new TypeInfo(type));
        }
        public AnalyzerResult(string assemblyName)
        {
            this.AssemblyName = assemblyName;
            _namesapces = new();
        }
    }

    public class NamespaceInfo
    {
        public string namespaceName { get; set; }
        public List<TypeInfo> typeInfos { get; set; }
        public List<TypeInfo> TypeInfos
        {
            get => typeInfos;
        }
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
                    String.Join(" ", method.GetParameters().Select(p => p.ParameterType.Name))
                ));
            }
        }
    }

    public class MethodInfo
    {
        public string Name { get; set; }
        public string returnType { get; set; }
        public string paramTypes { get; set; }

        public MethodInfo(string name, string returnType, string paramTypes)
        {
            this.Name = name;
            this.returnType = returnType;
            this.paramTypes = paramTypes;
        }
    }
}
