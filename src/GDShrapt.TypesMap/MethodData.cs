using Godot;
using System.Reflection;

namespace GDShrapt.TypesMap
{
    public class MethodData
    {
        public string? Name { get; set; }
        public string? CSharpName { get; set; }
        public string? ReturnTypeName { get; set; }
        public string[]? ParameterTypeNames { get; set; }
        public bool IsOverridable { get; set; }
        public bool IsStatic { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsAbstract { get; set; }

        public MethodData()
        {

        }

        internal MethodData(string name, MethodInfo method)
        {
            Name = name;
            CSharpName = method.Name;
            
            ReturnTypeName = method.ReturnParameter.ParameterType.Name;
            ParameterTypeNames = method.GetParameters().Select(x => x.ParameterType.Name).ToArray();
            IsOverridable = (method.IsVirtual || method.IsAbstract) && !method.IsFinal;
            IsStatic = method.IsStatic;
            IsGeneric = method.IsGenericMethod;
            IsVirtual = method.IsVirtual;
            IsAbstract = method.IsAbstract;
        }
    }
}
