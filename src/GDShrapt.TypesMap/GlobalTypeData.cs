
namespace GDShrapt.TypesMap
{
    public class GlobalTypeData
    {
        public Dictionary<string, List<MethodData>> MethodDatas { get; } = new Dictionary<string, List<MethodData>>();
        public Dictionary<string, PropertyData> PropertyDatas { get; } = new Dictionary<string, PropertyData>();
        public Dictionary<string, ConstantInfo> Constants { get; } = new Dictionary<string, ConstantInfo>();
        public Dictionary<string, List<EnumTypeInfo>> Enums { get; } = new Dictionary<string, List<EnumTypeInfo>>();
        public Dictionary<string, List<EnumTypeInfo>> EnumsConstants { get; private set; } = new Dictionary<string, List<EnumTypeInfo>>();

        public void BuildEnumsConstants()
        {
            EnumsConstants = Enums
                .SelectMany(x => x.Value.SelectMany(y => y.Values!.Keys.Select(y => (y, x.Value))))
                .GroupBy(x => x.y)
                .ToDictionary(
                    x => x.Key, 
                    x => x.SelectMany(y => y.Value).ToList()
                );
        }
    }
}