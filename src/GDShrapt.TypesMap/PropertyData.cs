using System.Reflection;

namespace GDShrapt.TypesMap
{
    public class PropertyData
    {
        public string? Name { get; set; }
        public string? CSharpName { get; set; }
        public string? PropertyTypeName { get; set; }
        public bool CanWrite { get; set; }
        public bool CanRead { get; set; }

        public PropertyData()
        {

        }

        internal PropertyData(string name, PropertyInfo info)
        {
            Name = name;
            CSharpName = info.Name;
            PropertyTypeName = info.PropertyType.Name;
            CanWrite = info.CanWrite;
            CanRead = info.CanRead;
        }
    }
}
