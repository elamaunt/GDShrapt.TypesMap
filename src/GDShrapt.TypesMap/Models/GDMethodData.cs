using System.Reflection;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents metadata about a GDScript method and its C# equivalent.
    /// </summary>
    public class GDMethodData
    {
        // ========================================
        // GDScript Names (snake_case convention)
        // ========================================

        /// <summary>
        /// Gets or sets the GDScript method name (e.g., "get_node", "add_child").
        /// </summary>
        public string? GDScriptName { get; set; }

        /// <summary>
        /// Gets or sets the GDScript return type name (e.g., "Node", "Vector2", "int", "float").
        /// </summary>
        public string? GDScriptReturnTypeName { get; set; }

        /// <summary>
        /// Gets or sets the GDScript parameter type names (e.g., ["int", "float", "String"]).
        /// </summary>
        public string[]? GDScriptParameterTypeNames { get; set; }

        // ========================================
        // C# Names (PascalCase convention)
        // ========================================

        /// <summary>
        /// Gets or sets the C# method name (e.g., "GetNode", "AddChild").
        /// </summary>
        public string? CSharpName { get; set; }

        /// <summary>
        /// Gets or sets the C# return type name (simple name, e.g., "Node", "Int32").
        /// </summary>
        public string? CSharpReturnTypeName { get; set; }

        /// <summary>
        /// Gets or sets the full C# return type name including namespace (e.g., "Godot.Node", "System.Int32").
        /// </summary>
        public string? CSharpReturnTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the C# parameter type names (simple names, e.g., ["Int32", "Single"]).
        /// </summary>
        public string[]? CSharpParameterTypeNames { get; set; }

        /// <summary>
        /// Gets or sets the containing C# type full name (the class that declares this method).
        /// </summary>
        public string? CSharpDeclaringTypeFullName { get; set; }

        // ========================================
        // Detailed Parameter Information
        // ========================================

        /// <summary>
        /// Gets or sets detailed parameter information for Type Inference.
        /// </summary>
        public GDParameterInfo[]? Parameters { get; set; }

        // ========================================
        // Method Characteristics
        // ========================================

        /// <summary>
        /// Gets or sets a value indicating whether the method can be overridden.
        /// </summary>
        public bool IsOverridable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is static.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is generic.
        /// </summary>
        public bool IsGeneric { get; set; }

        /// <summary>
        /// Gets or sets the generic type parameter names (e.g., ["T", "TResult"]).
        /// </summary>
        public string[]? GenericTypeParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is virtual.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is abstract.
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the return type is nullable.
        /// </summary>
        public bool ReturnsNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this method returns void.
        /// </summary>
        public bool ReturnsVoid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDMethodData"/> class.
        /// </summary>
        public GDMethodData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDMethodData"/> class from a MethodInfo.
        /// </summary>
        /// <param name="gdScriptName">The GDScript method name.</param>
        /// <param name="method">The .NET MethodInfo.</param>
        internal GDMethodData(string gdScriptName, MethodInfo method)
        {
            // GDScript names
            GDScriptName = gdScriptName;

            // C# names
            CSharpName = method.Name;

            var returnType = method.ReturnType;
            CSharpReturnTypeName = returnType.Name;
            CSharpReturnTypeFullName = returnType.FullName;
            GDScriptReturnTypeName = MapCSharpTypeToGDScript(returnType);

            ReturnsVoid = returnType == typeof(void);
            ReturnsNullable = Nullable.GetUnderlyingType(returnType) != null;

            var parameters = method.GetParameters();
            CSharpParameterTypeNames = parameters.Select(x => x.ParameterType.Name).ToArray();
            GDScriptParameterTypeNames = parameters.Select(x => MapCSharpTypeToGDScript(x.ParameterType)).ToArray();
            Parameters = parameters.Select(p => new GDParameterInfo(p)).ToArray();

            IsOverridable = (method.IsVirtual || method.IsAbstract) && !method.IsFinal;
            IsStatic = method.IsStatic;
            IsGeneric = method.IsGenericMethod;
            IsVirtual = method.IsVirtual;
            IsAbstract = method.IsAbstract;

            if (method.IsGenericMethod)
            {
                GenericTypeParameters = method.GetGenericArguments().Select(t => t.Name).ToArray();
            }

            CSharpDeclaringTypeFullName = method.DeclaringType?.FullName;
        }

        /// <summary>
        /// Maps a C# type to its GDScript equivalent name.
        /// </summary>
        private static string MapCSharpTypeToGDScript(Type type)
        {
            if (type == typeof(void)) return "void";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(int) || type == typeof(long) || type == typeof(Int32) || type == typeof(Int64)) return "int";
            if (type == typeof(float) || type == typeof(double) || type == typeof(Single) || type == typeof(Double)) return "float";
            if (type == typeof(string) || type == typeof(String)) return "String";

            // For Godot types, use the type name directly (usually matches)
            return type.Name;
        }
    }
}
