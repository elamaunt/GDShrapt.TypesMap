
namespace GDShrapt.TypesMap
{
    public class GlobalTypeData
    {
        public Dictionary<string, List<MethodData>> MethodDatas { get; } = new Dictionary<string, List<MethodData>>();
        public Dictionary<string, PropertyData> PropertyDatas { get; } = new Dictionary<string, PropertyData>();
        public Dictionary<string, (string, string)> Constants { get; } = new Dictionary<string, (string, string)>();
        public Dictionary<string, EnumTypeInfo> Enums { get; } = new Dictionary<string, EnumTypeInfo>();
        public Dictionary<string, EnumTypeInfo> EnumsConstants { get; private set; } = new Dictionary<string, EnumTypeInfo>();

        public void BuildEnumsConstants()
        {
            EnumsConstants = Enums.SelectMany(x => x.Value.Values!.Keys.Select(y => (y, x.Value))).ToDictionary(x => x.y, x => x.Value);
        }
    }
}