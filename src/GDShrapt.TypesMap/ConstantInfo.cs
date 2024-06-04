using System.Reflection.Metadata.Ecma335;

namespace GDShrapt.TypesMap
{
    public class ConstantInfo
    {

        public string Name { get; }
        public string Value { get; }
        public Type ValueType { get; }

        public ConstantInfo(string name, string value, Type valueType)
        {
            Name = name;
            Value = value;
            ValueType = valueType;
        }
    }
}