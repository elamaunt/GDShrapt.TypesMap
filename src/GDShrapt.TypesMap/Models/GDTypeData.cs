namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents comprehensive metadata about a GDScript type and its C# equivalent.
    /// </summary>
    public class GDTypeData
    {
        // ========================================
        // GDScript Names
        // ========================================

        /// <summary>
        /// Gets or sets the GDScript type name (e.g., "Node2D", "Control").
        /// </summary>
        public string? GDScriptName { get; set; }

        /// <summary>
        /// Gets or sets the GDScript base type name (e.g., "Node" for Node2D).
        /// </summary>
        public string? GDScriptBaseTypeName { get; set; }

        // ========================================
        // C# Names
        // ========================================

        /// <summary>
        /// Gets or sets the C# class name (e.g., "Node2D", "Control").
        /// </summary>
        public string? CSharpName { get; set; }

        /// <summary>
        /// Gets or sets the C# namespace (e.g., "Godot").
        /// </summary>
        public string? CSharpNamespace { get; set; }

        /// <summary>
        /// Gets or sets the C# base type name (e.g., "Node").
        /// </summary>
        public string? CSharpBaseTypeName { get; set; }

        /// <summary>
        /// Gets or sets the C# base type namespace (e.g., "Godot").
        /// </summary>
        public string? CSharpBaseTypeNamespace { get; set; }

        // ========================================
        // Type Characteristics
        // ========================================

        /// <summary>
        /// Gets or sets a value indicating whether the type is an enum.
        /// </summary>
        public bool IsEnum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is static.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is a builtin value type
        /// (e.g., Vector2, Color, Array, Dictionary, Signal) rather than a class.
        /// </summary>
        public bool IsBuiltin { get; set; }

        // ========================================
        // Members (keyed by GDScript names)
        // ========================================

        /// <summary>
        /// Gets or sets the methods defined on this type, keyed by GDScript method name.
        /// </summary>
        public Dictionary<string, List<GDMethodData>>? MethodDatas { get; set; }

        /// <summary>
        /// Gets or sets the properties defined on this type, keyed by GDScript property name.
        /// </summary>
        public Dictionary<string, GDPropertyData>? PropertyDatas { get; set; }

        /// <summary>
        /// Gets or sets the signals defined on this type, keyed by GDScript signal name.
        /// </summary>
        public Dictionary<string, GDSignalData>? SignalDatas { get; set; }

        /// <summary>
        /// Gets or sets the constants defined on this type, keyed by GDScript constant name.
        /// </summary>
        public Dictionary<string, GDConstantInfo>? Constants { get; set; }

        /// <summary>
        /// Gets or sets the nested enums defined on this type, keyed by GDScript enum name.
        /// </summary>
        public Dictionary<string, GDEnumTypeInfo>? Enums { get; set; }

        /// <summary>
        /// Gets or sets a flattened lookup of GDScript enum constants to their enum info.
        /// </summary>
        public Dictionary<string, GDEnumTypeInfo>? EnumsConstants { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDTypeData"/> class.
        /// </summary>
        public GDTypeData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDTypeData"/> class with full metadata.
        /// </summary>
        internal GDTypeData(string gdScriptName, Type type,
            Dictionary<string, List<GDMethodData>> methodDatas,
            Dictionary<string, GDPropertyData> propertyDatas,
            Dictionary<string, GDSignalData> signalDatas,
            Dictionary<string, GDEnumTypeInfo> enumDatas,
            Dictionary<string, GDConstantInfo> constants)
        {
            GDScriptName = gdScriptName;

            MethodDatas = methodDatas;
            PropertyDatas = propertyDatas;
            SignalDatas = signalDatas;
            Enums = enumDatas;
            Constants = constants;

            CSharpName = type.Name;
            CSharpNamespace = type.Namespace;

            CSharpBaseTypeName = type.BaseType?.Name;
            CSharpBaseTypeNamespace = type.BaseType?.Namespace;

            if (type.BaseType != null)
                GDScriptBaseTypeName = GDTypeHelper.GetGodotTypeName(type.BaseType);

            IsEnum = type.IsEnum;
            IsStatic = type.IsStatic();

            EnumsConstants = enumDatas.SelectMany(x => x.Value.Values!.Keys.Select(y => (y, x.Value))).ToDictionary(x => x.y, x => x.Value);
        }
    }
}
