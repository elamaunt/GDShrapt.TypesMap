using System.Reflection;

namespace GDShrapt.TypesMap
{
    public class SignalData
    {
        public string? Name { get; set; }
        public string? CSharpName { get; set; }
        public string? DelegateReturnTypeName { get; set; }
        public string[]? DelegateParameterTypeNames { get; set; }

        public SignalData() { }
        internal SignalData(string name, EventInfo info)
        {
            Name = name;
            CSharpName = info.Name;
            DelegateReturnTypeName = info.AddMethod?.ReturnParameter.Name;
            DelegateParameterTypeNames = info.AddMethod?.GetParameters().Select(x => x.ParameterType.Name).ToArray();
        }
    }
}
