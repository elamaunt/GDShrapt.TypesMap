using System.Reflection;

namespace GDShrapt.TypesMap
{
    public class SignalData
    {
        public string Name { get; }
        public EventInfo Event { get; }
        public string DelegateReturnTypeName { get; }
        public string[] DelegateParameterTypeNames { get; }
        public SignalData(string name, EventInfo info)
        {
            Name = name;
            Event = info;
            DelegateReturnTypeName = info.AddMethod.ReturnParameter.Name;
            DelegateParameterTypeNames = info.AddMethod.GetParameters().Select(x => x.ParameterType.Name).ToArray();
        }
    }
}
