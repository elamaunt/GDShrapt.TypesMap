namespace GDShrapt.TypesMap
{
    internal class UnresolvedBundle
    {
        public readonly List<(string godotTypeName, Type type)> ConstantsTypeIgnores = new List<(string godotTypeName, Type type)>();
        public readonly List<(string godotTypeName, string godotEnum)> EnumIgnore = new List<(string godotTypeName, string godotEnum)>();
        public readonly List<(string godotTypeName, Type type)> GlobalEnumIgnores = new List<(string godotTypeName, Type type)>();

        internal void AddConstantsTypeIgnore(string godotTypeName, Type type)
        {
            ConstantsTypeIgnores.Add((godotTypeName, type));
        }

        internal void AddEnumIgnore(string godotTypeName, string godotEnum)
        {
            EnumIgnore.Add((godotTypeName, godotEnum));
        }

        internal void AddGlobalEnumIgnore(string godotTypeName, Type type)
        {
            GlobalEnumIgnores.Add((godotTypeName, type));
        }

        public void Print()
        {

        }
    }
}