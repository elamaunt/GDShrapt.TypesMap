
namespace GDShrapt.TypesMap
{
    public class GlobalData
    {
        public Dictionary<string, List<MethodData>> MethodDatas { get; set; } = new Dictionary<string, List<MethodData>>();
        public Dictionary<string, PropertyData> PropertyDatas { get; set; } = new Dictionary<string, PropertyData>();
        public Dictionary<string, ConstantInfo> Constants { get; set; } = new Dictionary<string, ConstantInfo>();
        public Dictionary<string, List<EnumTypeInfo>> Enums { get; set; } = new Dictionary<string, List<EnumTypeInfo>>();
        public Dictionary<string, List<EnumTypeInfo>> EnumsConstants { get; set; } = new Dictionary<string, List<EnumTypeInfo>>();
        public Dictionary<string, GlobalTypeProxyInfo> GlobalTypes { get; set; } = new Dictionary<string, GlobalTypeProxyInfo>();

        public GlobalData()
        {

        }

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