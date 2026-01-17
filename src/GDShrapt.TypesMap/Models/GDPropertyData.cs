using System.Reflection;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents metadata about a GDScript property and its C# equivalent.
    /// </summary>
    public class GDPropertyData
    {
        // ========================================
        // GDScript Names (snake_case convention)
        // ========================================

        /// <summary>
        /// Gets or sets the GDScript property name (e.g., "position", "global_position").
        /// </summary>
        public string? GDScriptName { get; set; }

        /// <summary>
        /// Gets or sets the GDScript property type name (e.g., "Vector2", "int", "float").
        /// </summary>
        public string? GDScriptTypeName { get; set; }

        // ========================================
        // C# Names (PascalCase convention)
        // ========================================

        /// <summary>
        /// Gets or sets the C# property name (e.g., "Position", "GlobalPosition").
        /// </summary>
        public string? CSharpName { get; set; }

        /// <summary>
        /// Gets or sets the C# property type name (simple name, e.g., "Vector2", "Int32").
        /// </summary>
        public string? CSharpTypeName { get; set; }

        /// <summary>
        /// Gets or sets the full C# property type name including namespace (e.g., "Godot.Vector2", "System.Int32").
        /// </summary>
        public string? CSharpTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the containing C# type full name (the class that declares this property).
        /// </summary>
        public string? CSharpDeclaringTypeFullName { get; set; }

        // ========================================
        // Property Characteristics
        // ========================================

        /// <summary>
        /// Gets or sets a value indicating whether the property can be written to.
        /// </summary>
        public bool CanWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property can be read.
        /// </summary>
        public bool CanRead { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property type is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is static.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property type is a generic type.
        /// </summary>
        public bool IsGenericType { get; set; }

        /// <summary>
        /// Gets or sets the generic type arguments if IsGenericType is true (C# type names).
        /// </summary>
        public string[]? CSharpGenericTypeArguments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDPropertyData"/> class.
        /// </summary>
        public GDPropertyData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDPropertyData"/> class from a PropertyInfo.
        /// </summary>
        /// <param name="gdScriptName">The GDScript property name.</param>
        /// <param name="info">The .NET PropertyInfo.</param>
        internal GDPropertyData(string gdScriptName, PropertyInfo info)
        {
            // GDScript names
            GDScriptName = gdScriptName;

            // C# names
            CSharpName = info.Name;

            var propType = info.PropertyType;
            CSharpTypeName = propType.Name;
            CSharpTypeFullName = propType.FullName;
            GDScriptTypeName = MapCSharpTypeToGDScript(propType);

            CanWrite = info.CanWrite;
            CanRead = info.CanRead;

            IsNullable = Nullable.GetUnderlyingType(propType) != null;
            IsGenericType = propType.IsGenericType;

            if (propType.IsGenericType)
            {
                CSharpGenericTypeArguments = propType.GetGenericArguments().Select(t => t.Name).ToArray();
            }

            var getMethod = info.GetGetMethod();
            var setMethod = info.GetSetMethod();
            IsStatic = (getMethod?.IsStatic ?? false) || (setMethod?.IsStatic ?? false);

            CSharpDeclaringTypeFullName = info.DeclaringType?.FullName;
        }

        /// <summary>
        /// Maps a C# type to its GDScript equivalent name.
        /// </summary>
        private static string MapCSharpTypeToGDScript(Type type)
        {
            if (type == typeof(bool)) return "bool";

            // Unsigned integers → int in GDScript
            if (type == typeof(uint) || type == typeof(UInt32) ||
                type == typeof(ulong) || type == typeof(UInt64) ||
                type == typeof(ushort) || type == typeof(UInt16) ||
                type == typeof(byte) || type == typeof(sbyte))
                return "int";

            if (type == typeof(int) || type == typeof(long) || type == typeof(Int32) || type == typeof(Int64) ||
                type == typeof(short) || type == typeof(Int16))
                return "int";

            if (type == typeof(float) || type == typeof(double) || type == typeof(Single) || type == typeof(Double)) return "float";
            if (type == typeof(string) || type == typeof(String)) return "String";

            // For Godot types, use the type name directly
            return type.Name;
        }
    }
}
