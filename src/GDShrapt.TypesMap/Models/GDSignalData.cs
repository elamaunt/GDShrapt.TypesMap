using System.Reflection;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents metadata about a GDScript signal and its C# equivalent.
    /// </summary>
    public class GDSignalData
    {
        // ========================================
        // GDScript Names (snake_case convention)
        // ========================================

        /// <summary>
        /// Gets or sets the GDScript signal name (e.g., "tree_entered", "ready").
        /// </summary>
        public string? GDScriptName { get; set; }

        // ========================================
        // C# Names (PascalCase convention)
        // ========================================

        /// <summary>
        /// Gets or sets the C# event name (e.g., "TreeEntered", "Ready").
        /// </summary>
        public string? CSharpName { get; set; }

        /// <summary>
        /// Gets or sets the C# delegate return type name (simple name).
        /// </summary>
        public string? CSharpDelegateReturnTypeName { get; set; }

        /// <summary>
        /// Gets or sets the full C# delegate return type name including namespace.
        /// </summary>
        public string? CSharpDelegateReturnTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the C# delegate parameter type names (simple names).
        /// </summary>
        public string[]? CSharpDelegateParameterTypeNames { get; set; }

        /// <summary>
        /// Gets or sets the C# delegate type name (e.g., "Action", "EventHandler").
        /// </summary>
        public string? CSharpDelegateTypeName { get; set; }

        /// <summary>
        /// Gets or sets the full C# delegate type name including namespace.
        /// </summary>
        public string? CSharpDelegateTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the containing C# type full name (the class that declares this signal).
        /// </summary>
        public string? CSharpDeclaringTypeFullName { get; set; }

        // ========================================
        // Detailed Parameter Information
        // ========================================

        /// <summary>
        /// Gets or sets detailed parameter information for the signal handler.
        /// </summary>
        public GDParameterInfo[]? Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDSignalData"/> class.
        /// </summary>
        public GDSignalData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GDSignalData"/> class from an EventInfo.
        /// </summary>
        /// <param name="gdScriptName">The GDScript signal name.</param>
        /// <param name="info">The .NET EventInfo.</param>
        internal GDSignalData(string gdScriptName, EventInfo info)
        {
            GDScriptName = gdScriptName;
            CSharpName = info.Name;

            var handlerType = info.EventHandlerType;
            CSharpDelegateTypeName = handlerType?.Name;
            CSharpDelegateTypeFullName = handlerType?.FullName;

            var invokeMethod = handlerType?.GetMethod("Invoke");
            if (invokeMethod != null)
            {
                CSharpDelegateReturnTypeName = invokeMethod.ReturnType.Name;
                CSharpDelegateReturnTypeFullName = invokeMethod.ReturnType.FullName;

                var parameters = invokeMethod.GetParameters();
                CSharpDelegateParameterTypeNames = parameters.Select(x => x.ParameterType.Name).ToArray();
                Parameters = parameters.Select(p => new GDParameterInfo(p)).ToArray();
            }

            CSharpDeclaringTypeFullName = info.DeclaringType?.FullName;
        }
    }
}
