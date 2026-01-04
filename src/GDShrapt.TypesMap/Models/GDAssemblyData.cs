namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Root container for all extracted Godot type metadata.
    /// Contains both global scope data and per-type data for all Godot classes.
    /// </summary>
    public class GDAssemblyData
    {
        /// <summary>
        /// Gets or sets the metadata about this assembly data (version, source, timestamp).
        /// </summary>
        public GDAssemblyMetadata? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the global scope data (global methods, constants, enums).
        /// </summary>
        public GDGlobalData? GlobalData { get; set; }

        /// <summary>
        /// Gets or sets the type data dictionary.
        /// Outer key is GDScript type name, inner key is C# full type name.
        /// This structure supports multiple C# type versions for the same GDScript type.
        /// </summary>
        public Dictionary<string, Dictionary<string, GDTypeData>>? TypeDatas { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDAssemblyData"/> class.
        /// </summary>
        public GDAssemblyData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDAssemblyData"/> class with data.
        /// </summary>
        /// <param name="globalData">The global scope data.</param>
        /// <param name="typeDatas">The per-type data dictionary.</param>
        internal GDAssemblyData(GDGlobalData globalData, Dictionary<string, Dictionary<string, GDTypeData>> typeDatas)
        {
            GlobalData = globalData;
            TypeDatas = typeDatas;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDAssemblyData"/> class with metadata.
        /// </summary>
        /// <param name="metadata">The metadata about this data.</param>
        /// <param name="globalData">The global scope data.</param>
        /// <param name="typeDatas">The per-type data dictionary.</param>
        internal GDAssemblyData(GDAssemblyMetadata metadata, GDGlobalData globalData, Dictionary<string, Dictionary<string, GDTypeData>> typeDatas)
        {
            Metadata = metadata;
            GlobalData = globalData;
            TypeDatas = typeDatas;
        }
    }
}
