
namespace GDShrapt.TypesMap
{
    public class GodotAssemblyData
    {
        public GlobalData? GlobalData { get; set; }
        public Dictionary<string, Dictionary<string, TypeData>>? TypeDatas { get; set; }

        public GodotAssemblyData()
        {

        }

        internal GodotAssemblyData(GlobalData globalData, Dictionary<string, Dictionary<string, TypeData>> typeDatas)
        {
            GlobalData = globalData;
            TypeDatas = typeDatas;
        }
    }
}