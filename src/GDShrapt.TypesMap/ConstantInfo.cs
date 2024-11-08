namespace GDShrapt.TypesMap
{
    public class ConstantInfo
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? ValueTypeName { get; set; }
        public string? ContainingTypeName { get; set; }

        public ConstantInfo()
        {

        }

        public ConstantInfo(string name, string value, Type valueType, Type containingType)
        {
            Name = name;
            Value = value;
            ValueTypeName = valueType.Name;
            ContainingTypeName = containingType.Name;
        }
    }
}