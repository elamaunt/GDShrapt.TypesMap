using System.Reflection;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents detailed metadata about a method parameter for Type Inference.
    /// </summary>
    public class GDParameterInfo
    {
        // ========================================
        // GDScript Names
        // ========================================

        /// <summary>
        /// Gets or sets the GDScript type name (e.g., "int", "float", "Vector2", "String").
        /// </summary>
        public string? GDScriptTypeName { get; set; }

        // ========================================
        // C# Names
        // ========================================

        /// <summary>
        /// Gets or sets the C# parameter name.
        /// </summary>
        public string? CSharpName { get; set; }

        /// <summary>
        /// Gets or sets the C# type name (simple name, e.g., "Vector2", "Int32").
        /// </summary>
        public string? CSharpTypeName { get; set; }

        /// <summary>
        /// Gets or sets the full C# type name including namespace (e.g., "Godot.Vector2", "System.Int32").
        /// </summary>
        public string? CSharpTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the C# generic type arguments if IsGenericType is true (e.g., for List&lt;Node&gt; this would be ["Node"]).
        /// </summary>
        public string[]? CSharpGenericTypeArguments { get; set; }

        // ========================================
        // Parameter Characteristics
        // ========================================

        /// <summary>
        /// Gets or sets the position of the parameter in the method signature (0-based).
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter has a default value.
        /// </summary>
        public bool HasDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the default value as a string representation (if HasDefaultValue is true).
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a 'ref' parameter.
        /// </summary>
        public bool IsRef { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an 'out' parameter.
        /// </summary>
        public bool IsOut { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an 'in' parameter.
        /// </summary>
        public bool IsIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a 'params' array parameter.
        /// </summary>
        public bool IsParams { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter type is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a generic type parameter.
        /// </summary>
        public bool IsGenericType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDParameterInfo"/> class.
        /// </summary>
        public GDParameterInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDParameterInfo"/> class from a ParameterInfo.
        /// </summary>
        /// <param name="info">The .NET ParameterInfo.</param>
        internal GDParameterInfo(ParameterInfo info)
        {
            CSharpName = info.Name;
            CSharpTypeName = info.ParameterType.Name;
            CSharpTypeFullName = info.ParameterType.FullName;
            GDScriptTypeName = MapCSharpTypeToGDScript(info.ParameterType);

            Position = info.Position;
            HasDefaultValue = info.HasDefaultValue;

            if (info.HasDefaultValue && info.DefaultValue != null)
            {
                DefaultValue = info.DefaultValue.ToString();
            }
            else if (info.HasDefaultValue)
            {
                DefaultValue = "null";
            }

            IsRef = info.ParameterType.IsByRef && !info.IsOut && !info.IsIn;
            IsOut = info.IsOut;
            IsIn = info.IsIn;
            IsParams = info.IsDefined(typeof(ParamArrayAttribute), false);

            var paramType = info.ParameterType;
            if (paramType.IsByRef)
            {
                paramType = paramType.GetElementType()!;
            }

            IsNullable = Nullable.GetUnderlyingType(paramType) != null;
            IsGenericType = paramType.IsGenericType;

            if (paramType.IsGenericType)
            {
                CSharpGenericTypeArguments = paramType.GetGenericArguments().Select(t => t.Name).ToArray();
            }
        }

        /// <summary>
        /// Maps a C# type to its GDScript equivalent name.
        /// </summary>
        private static string MapCSharpTypeToGDScript(Type type)
        {
            // Handle by-ref types
            if (type.IsByRef)
            {
                type = type.GetElementType()!;
            }

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
