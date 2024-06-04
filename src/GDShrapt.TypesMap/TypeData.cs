namespace GDShrapt.TypesMap
{
    public class TypeData
    {
        public Type Type { get; }
        public bool IsEnum { get; }
        public Dictionary<string, List<MethodData>> MethodDatas { get; }
        public Dictionary<string, PropertyData> PropertyDatas { get; }
        public Dictionary<string, SignalData> SignalDatas { get; }

        public Dictionary<string, ConstantInfo> Constants { get; }
        public Dictionary<string, EnumTypeInfo> Enums { get; }
        public Dictionary<string, EnumTypeInfo> EnumsConstants { get; }

        public TypeData(Type type,
            Dictionary<string, List<MethodData>> methodDatas,
            Dictionary<string, PropertyData> propertyDatas, 
            Dictionary<string, SignalData> signalDatas,
            Dictionary<string, EnumTypeInfo> enumDatas,
            Dictionary<string, ConstantInfo> constants)
        {
            MethodDatas = methodDatas;
            PropertyDatas = propertyDatas;
            SignalDatas = signalDatas;
            Enums = enumDatas;
            Constants = constants;

            Type = type;
            IsEnum = type.IsEnum;

            EnumsConstants = enumDatas.SelectMany(x => x.Value.Values!.Keys.Select(y => (y, x.Value))).ToDictionary(x => x.y, x => x.Value);
        }
    }
}
