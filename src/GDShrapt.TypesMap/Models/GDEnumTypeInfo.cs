namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents metadata about a GDScript enum and its C# equivalent.
    /// </summary>
    public class GDEnumTypeInfo
    {
        // ========================================
        // GDScript Names
        // ========================================

        /// <summary>
        /// Gets or sets the GDScript containing type name (if enum is nested).
        /// </summary>
        public string? GDScriptContainingTypeName { get; set; }

        /// <summary>
        /// Gets or sets the mapping of GDScript enum constant names to their C# values.
        /// Key: GDScript constant name (e.g., "SIDE_LEFT"), Value: C# enum value name (e.g., "Left").
        /// </summary>
        public Dictionary<string, string>? Values { get; set; }

        // ========================================
        // C# Names
        // ========================================

        /// <summary>
        /// Gets or sets the simple C# enum type name (e.g., "Side").
        /// </summary>
        public string? CSharpEnumName { get; set; }

        /// <summary>
        /// Gets or sets the full C# enum type name including namespace (e.g., "Godot.Side").
        /// </summary>
        public string? CSharpEnumFullName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDEnumTypeInfo"/> class.
        /// </summary>
        public GDEnumTypeInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDEnumTypeInfo"/> class with specified values.
        /// </summary>
        /// <param name="gdScriptContainingTypeName">The GDScript containing type name.</param>
        /// <param name="csharpEnumType">The C# enum type.</param>
        /// <param name="gdScriptConstants">The GDScript constant names.</param>
        /// <param name="csharpValues">The corresponding C# enum values.</param>
        public GDEnumTypeInfo(string gdScriptContainingTypeName, Type csharpEnumType, string[] gdScriptConstants, Array csharpValues)
        {
            GDScriptContainingTypeName = gdScriptContainingTypeName;
            CSharpEnumFullName = csharpEnumType.FullName;
            CSharpEnumName = csharpEnumType.Name;

            Values = new Dictionary<string, string>();

            for (int i = 0; i < gdScriptConstants.Length; i++)
            {
                Values[gdScriptConstants[i]] = csharpValues.GetValue(i)!.ToString()!;
            }
        }
    }
}
