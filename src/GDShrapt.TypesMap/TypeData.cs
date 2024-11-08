namespace GDShrapt.TypesMap
{
    public class TypeData
    {
        public string? Name { get; set; }
        public string? CSharpName { get; set; }
        public string? CSharpNamespace { get; set; }
        public bool IsEnum { get; set; }
        public bool IsStatic { get; set; }
        public Dictionary<string, List<MethodData>>? MethodDatas { get; set; }
        public Dictionary<string, PropertyData>? PropertyDatas { get; set; }
        public Dictionary<string, SignalData>? SignalDatas { get; set; }

        public Dictionary<string, ConstantInfo>? Constants { get; set; }
        public Dictionary<string, EnumTypeInfo>? Enums { get; set; }
        public Dictionary<string, EnumTypeInfo>? EnumsConstants { get; set; }

        public string? BaseTypeName { get; set; }
        public string? CSharpBaseTypeName { get; set; }
        public string? CSharpBaseTypeNameSpace { get; set; }

        public TypeData()
        {
        }

        internal TypeData(string name, Type type,
            Dictionary<string, List<MethodData>> methodDatas,
            Dictionary<string, PropertyData> propertyDatas, 
            Dictionary<string, SignalData> signalDatas,
            Dictionary<string, EnumTypeInfo> enumDatas,
            Dictionary<string, ConstantInfo> constants)
        {
            Name = name;

            MethodDatas = methodDatas;
            PropertyDatas = propertyDatas;
            SignalDatas = signalDatas;
            Enums = enumDatas;
            Constants = constants;

            CSharpName = type.Name;
            CSharpNamespace = type.Namespace;

            CSharpBaseTypeName = type.BaseType?.Name;
            CSharpBaseTypeNameSpace = type.BaseType?.Namespace;

            if (type.BaseType != null)
                BaseTypeName = GodotTypeHelper.GetGodotTypeName(type.BaseType);

            IsEnum = type.IsEnum;
            IsStatic = type.IsStatic();

            EnumsConstants = enumDatas.SelectMany(x => x.Value.Values!.Keys.Select(y => (y, x.Value))).ToDictionary(x => x.y, x => x.Value);
        }
    }
}
