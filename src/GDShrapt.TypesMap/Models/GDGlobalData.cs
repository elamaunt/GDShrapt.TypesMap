namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents global scope data including methods, properties, constants, and enums
    /// available at the global level in GDScript (e.g., print, lerp, PI).
    /// </summary>
    public class GDGlobalData
    {
        /// <summary>
        /// Gets or sets the global methods, keyed by GDScript function name.
        /// </summary>
        public Dictionary<string, List<GDMethodData>> MethodDatas { get; set; } = new Dictionary<string, List<GDMethodData>>();

        /// <summary>
        /// Gets or sets the global properties, keyed by GDScript property name.
        /// </summary>
        public Dictionary<string, GDPropertyData> PropertyDatas { get; set; } = new Dictionary<string, GDPropertyData>();

        /// <summary>
        /// Gets or sets the global constants (e.g., PI, TAU, INF), keyed by constant name.
        /// </summary>
        public Dictionary<string, GDConstantInfo> Constants { get; set; } = new Dictionary<string, GDConstantInfo>();

        /// <summary>
        /// Gets or sets the global enums, keyed by enum name.
        /// </summary>
        public Dictionary<string, List<GDEnumTypeInfo>> Enums { get; set; } = new Dictionary<string, List<GDEnumTypeInfo>>();

        /// <summary>
        /// Gets or sets a flattened lookup of enum constants to their enum info.
        /// Built by calling <see cref="BuildEnumsConstants"/>.
        /// </summary>
        public Dictionary<string, List<GDEnumTypeInfo>> EnumsConstants { get; set; } = new Dictionary<string, List<GDEnumTypeInfo>>();

        /// <summary>
        /// Gets or sets the global type proxies for built-in types (e.g., int, float, Vector2).
        /// </summary>
        public Dictionary<string, GDGlobalTypeProxyInfo> GlobalTypes { get; set; } = new Dictionary<string, GDGlobalTypeProxyInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GDGlobalData"/> class.
        /// </summary>
        public GDGlobalData()
        {
        }

        /// <summary>
        /// Builds the <see cref="EnumsConstants"/> lookup from the <see cref="Enums"/> dictionary.
        /// </summary>
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
