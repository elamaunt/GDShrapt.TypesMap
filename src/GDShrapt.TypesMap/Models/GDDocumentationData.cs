namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Root model for DocumentationData.json — contains all Godot type documentation.
    /// Loaded as a separate embedded resource and merged into GDAssemblyData at load time.
    /// </summary>
    public class GDDocumentationData
    {
        /// <summary>
        /// Documentation for types, keyed by GDScript type name.
        /// </summary>
        public Dictionary<string, GDTypeDocumentation>? Types { get; set; }

        /// <summary>
        /// Documentation for global utility functions, keyed by GDScript function name.
        /// </summary>
        public Dictionary<string, string>? GlobalFunctions { get; set; }

        /// <summary>
        /// Documentation for global constants, keyed by GDScript constant name.
        /// </summary>
        public Dictionary<string, string>? GlobalConstants { get; set; }

        /// <summary>
        /// Documentation for global enums, keyed by GDScript enum name.
        /// </summary>
        public Dictionary<string, string>? GlobalEnums { get; set; }
    }

    /// <summary>
    /// Documentation for a single type and all its members.
    /// </summary>
    public class GDTypeDocumentation
    {
        /// <summary>
        /// Brief one-line description of this type.
        /// </summary>
        public string? Brief { get; set; }

        /// <summary>
        /// Full description of this type. May contain BBCode formatting.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Documentation for methods, keyed by GDScript method name.
        /// </summary>
        public Dictionary<string, string>? Methods { get; set; }

        /// <summary>
        /// Documentation for properties, keyed by GDScript property name.
        /// </summary>
        public Dictionary<string, string>? Properties { get; set; }

        /// <summary>
        /// Documentation for signals, keyed by GDScript signal name.
        /// </summary>
        public Dictionary<string, string>? Signals { get; set; }

        /// <summary>
        /// Documentation for constants, keyed by GDScript constant name.
        /// </summary>
        public Dictionary<string, string>? Constants { get; set; }

        /// <summary>
        /// Documentation for enums, keyed by GDScript enum name.
        /// </summary>
        public Dictionary<string, string>? Enums { get; set; }
    }
}
