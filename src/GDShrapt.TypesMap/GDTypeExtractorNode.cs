using Godot;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Base Node class for extracting Godot type data from GodotSharp assembly.
    /// Inherit from this class with [Tool] attribute in your Godot project
    /// to enable in-editor data extraction.
    /// </summary>
    /// <example>
    /// <code>
    /// using Godot;
    /// using GDShrapt.TypesMap;
    ///
    /// [Tool]
    /// public partial class MyTypeExtractor : GDTypeExtractorNode
    /// {
    ///     public override void _Ready()
    ///     {
    ///         if (Engine.IsEditorHint())
    ///         {
    ///             OutputPath = "res://data/AssemblyData.json";
    ///             ExtractAndSave();
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public partial class GDTypeExtractorNode : Node
    {
        /// <summary>
        /// Path where the JSON file will be saved.
        /// If empty, saves to the default location next to the executing assembly.
        /// Supports Godot resource paths (res://) which will be converted to absolute paths.
        /// </summary>
        [Export]
        public string OutputPath { get; set; } = "";

        /// <summary>
        /// If true, prints progress information to the Godot console.
        /// </summary>
        [Export]
        public bool VerboseOutput { get; set; } = true;

        /// <summary>
        /// Extracts type data from the GodotSharp assembly and saves it to a file.
        /// </summary>
        /// <returns>The extracted assembly data.</returns>
        public virtual GDAssemblyData ExtractAndSave()
        {
            if (VerboseOutput)
                GD.Print("[GDTypeExtractor] Starting extraction from GodotSharp assembly...");

            var data = GDTypeHelper.ExtractTypeDatasFromAssembly();

            if (VerboseOutput)
            {
                var typeCount = data.TypeDatas?.Count ?? 0;
                var methodCount = data.GlobalData?.MethodDatas?.Count ?? 0;
                GD.Print($"[GDTypeExtractor] Extracted {typeCount} types and {methodCount} global methods");
            }

            string? path = string.IsNullOrEmpty(OutputPath) ? null : ResolvePath(OutputPath);
            GDTypeHelper.SaveAssemblyDataToFile(data, path);

            if (VerboseOutput)
                GD.Print($"[GDTypeExtractor] Data saved to: {path ?? GDTypeHelper.GetDefaultSavePath()}");

            return data;
        }

        /// <summary>
        /// Extracts type data from the GodotSharp assembly without saving.
        /// </summary>
        /// <returns>The extracted assembly data.</returns>
        public virtual GDAssemblyData Extract()
        {
            if (VerboseOutput)
                GD.Print("[GDTypeExtractor] Starting extraction from GodotSharp assembly...");

            var data = GDTypeHelper.ExtractTypeDatasFromAssembly();

            if (VerboseOutput)
            {
                var typeCount = data.TypeDatas?.Count ?? 0;
                GD.Print($"[GDTypeExtractor] Extracted {typeCount} types");
            }

            return data;
        }

        /// <summary>
        /// Loads type data from the embedded manifest.
        /// </summary>
        /// <returns>The loaded assembly data, or null if not available.</returns>
        public virtual GDAssemblyData? LoadFromManifest()
        {
            if (VerboseOutput)
                GD.Print("[GDTypeExtractor] Loading from embedded manifest...");

            return GDTypeHelper.ExtractTypeDatasFromManifest();
        }

        /// <summary>
        /// Loads type data from a JSON file.
        /// </summary>
        /// <param name="filePath">Path to the JSON file. Supports Godot resource paths (res://).</param>
        /// <returns>The loaded assembly data, or null if the file cannot be loaded.</returns>
        public virtual GDAssemblyData? LoadFromFile(string filePath)
        {
            if (VerboseOutput)
                GD.Print($"[GDTypeExtractor] Loading from file: {filePath}");

            var resolvedPath = ResolvePath(filePath);
            return GDTypeHelper.ExtractTypeDatasFromFile(resolvedPath);
        }

        /// <summary>
        /// Resolves a path, converting Godot resource paths (res://) to absolute paths.
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        /// <returns>The resolved absolute path.</returns>
        protected virtual string ResolvePath(string path)
        {
            if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase))
            {
                return ProjectSettings.GlobalizePath(path);
            }

            return path;
        }
    }
}
