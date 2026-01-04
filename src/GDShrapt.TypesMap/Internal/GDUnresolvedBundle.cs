namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Tracks unresolved type mappings during extraction.
    /// Used internally for debugging and identifying missing type mappings.
    /// </summary>
    internal class GDUnresolvedBundle
    {
        /// <summary>
        /// Types where constant extraction was skipped due to count mismatch.
        /// </summary>
        public readonly List<(string godotTypeName, Type type)> ConstantsTypeIgnores = new List<(string godotTypeName, Type type)>();

        /// <summary>
        /// Enums that could not be resolved to a .NET type.
        /// </summary>
        public readonly List<(string godotTypeName, string godotEnum)> EnumIgnore = new List<(string godotTypeName, string godotEnum)>();

        /// <summary>
        /// Global enums that require manual mapping.
        /// </summary>
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
    }
}
