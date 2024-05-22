using System.Reflection;

namespace GDShrapt.TypesMap
{
    public class PropertyData
    {
        public string Name { get; }
        public PropertyInfo Property { get; }
        public string PropertyTypeName { get; }
        public PropertyData(string name, PropertyInfo info)
        {
            Name = name;
            Property = info;
            PropertyTypeName = info.PropertyType.Name;
        }
    }
}
