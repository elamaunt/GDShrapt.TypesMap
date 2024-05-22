
namespace GDShrapt.TypesMap
{
    public class GodotAssemblyData
    {
        public GlobalTypeData GlobalData { get; }
        public Dictionary<string, Dictionary<string, TypeData>> TypeDatas { get; }

        public GodotAssemblyData(GlobalTypeData globalData, Dictionary<string, Dictionary<string, TypeData>> typeDatas)
        {
            GlobalData = globalData;
            TypeDatas = typeDatas;
        }
    }
}