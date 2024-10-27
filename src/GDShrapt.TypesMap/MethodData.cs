using Godot;
using System.Reflection;

namespace GDShrapt.TypesMap
{
    public class MethodData
    {
        public string Name { get; }
        public MethodInfo Method { get; }
        public string ReturnTypeName { get; }
        public string[] ParameterTypeNames { get; }
        public bool IsOverridable { get; }
        public bool IsStatic { get; }
        public MethodData(string name, MethodInfo method)
        {
            Name = name;
            Method = method;

            ReturnTypeName = method.ReturnParameter.ParameterType.Name;
            ParameterTypeNames = method.GetParameters().Select(x => x.ParameterType.Name).ToArray();
            IsOverridable = (method.IsVirtual || method.IsAbstract) && !method.IsFinal;
            IsStatic = method.IsStatic;
        }
    }
}
