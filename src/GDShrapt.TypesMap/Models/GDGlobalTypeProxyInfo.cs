namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents type proxy information for built-in GDScript types (e.g., int, float, Vector2).
    /// Maps GDScript type names to their C# equivalents.
    /// </summary>
    public class GDGlobalTypeProxyInfo
    {
        // ========================================
        // C# Names
        // ========================================

        /// <summary>
        /// Gets or sets the C# type name (e.g., "Int64", "Double", "Vector2").
        /// </summary>
        public string? CSharpTypeName { get; set; }

        /// <summary>
        /// Gets or sets the full C# type name including namespace (e.g., "System.Int64", "Godot.Vector2").
        /// </summary>
        public string? CSharpTypeFullName { get; set; }

        // ========================================
        // Value Information
        // ========================================

        /// <summary>
        /// Gets or sets the value equivalent (e.g., "null" for nil type).
        /// </summary>
        public string? ValueEquivalent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDGlobalTypeProxyInfo"/> class.
        /// </summary>
        public GDGlobalTypeProxyInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDGlobalTypeProxyInfo"/> class from a Type.
        /// </summary>
        /// <param name="type">The C# type.</param>
        /// <param name="valueEquivalent">Optional value equivalent string.</param>
        internal GDGlobalTypeProxyInfo(Type type, string? valueEquivalent = null)
        {
            CSharpTypeName = type.Name;
            CSharpTypeFullName = type.FullName;
            ValueEquivalent = valueEquivalent;
        }
    }
}
