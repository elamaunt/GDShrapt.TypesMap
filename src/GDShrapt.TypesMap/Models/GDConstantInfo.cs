namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents metadata about a constant value in GDScript and its C# equivalent.
    /// </summary>
    public class GDConstantInfo
    {
        // ========================================
        // GDScript Names
        // ========================================

        /// <summary>
        /// Gets or sets the GDScript constant name (e.g., "PI", "TAU", "INF").
        /// </summary>
        public string? GDScriptName { get; set; }

        // ========================================
        // C# Names
        // ========================================

        /// <summary>
        /// Gets or sets the C# constant/field name (e.g., "Pi", "Tau", "Inf").
        /// </summary>
        public string? CSharpName { get; set; }

        /// <summary>
        /// Gets or sets the C# type name of the constant value (e.g., "Single", "Double").
        /// </summary>
        public string? CSharpValueTypeName { get; set; }

        /// <summary>
        /// Gets or sets the C# containing type name (the class that declares this constant).
        /// </summary>
        public string? CSharpContainingTypeName { get; set; }

        // ========================================
        // Value Information
        // ========================================

        /// <summary>
        /// Gets or sets the constant value as a string representation.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDConstantInfo"/> class.
        /// </summary>
        public GDConstantInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDConstantInfo"/> class with specified values.
        /// </summary>
        /// <param name="gdScriptName">The GDScript constant name.</param>
        /// <param name="csharpName">The C# constant/field name.</param>
        /// <param name="valueType">The type of the constant value.</param>
        /// <param name="containingType">The type that contains this constant.</param>
        public GDConstantInfo(string gdScriptName, string csharpName, Type valueType, Type containingType)
        {
            GDScriptName = gdScriptName;
            CSharpName = csharpName;
            Value = csharpName;
            CSharpValueTypeName = valueType.Name;
            CSharpContainingTypeName = containingType.Name;
        }
    }
}
