
namespace GDShrapt.TypesMap
{
    public class GodotAssemblyData
    {
        public GlobalData GlobalData { get; }
        public Dictionary<string, Dictionary<string, TypeData>> TypeDatas { get; }

        public GodotAssemblyData(GlobalData globalData, Dictionary<string, Dictionary<string, TypeData>> typeDatas)
        {
            GlobalData = globalData;
            TypeDatas = typeDatas;
        }
    }
}