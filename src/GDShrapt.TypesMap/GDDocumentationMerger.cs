using System.Text.Json;
using System.Text.Json.Serialization;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Extracts documentation from Godot's extension_api.json (with docs)
    /// and produces a DocumentationData.json for embedding.
    /// </summary>
    public static class GDDocumentationMerger
    {
        /// <summary>
        /// Extracts documentation from extension_api.json.
        /// The file must be generated with --dump-extension-api-with-docs flag.
        /// </summary>
        /// <param name="extensionApiJsonPath">Path to extension_api.json.</param>
        /// <returns>Extracted documentation data.</returns>
        public static GDDocumentationData ExtractDocumentation(string extensionApiJsonPath)
        {
            if (string.IsNullOrEmpty(extensionApiJsonPath))
                throw new ArgumentNullException(nameof(extensionApiJsonPath));

            if (!File.Exists(extensionApiJsonPath))
                throw new FileNotFoundException("extension_api.json not found", extensionApiJsonPath);

            var json = File.ReadAllText(extensionApiJsonPath);
            var api = JsonSerializer.Deserialize<GDExtensionApi>(json, _jsonOptions);

            if (api == null)
                throw new InvalidOperationException("Failed to deserialize extension_api.json");

            var result = new GDDocumentationData
            {
                Types = new Dictionary<string, GDTypeDocumentation>(),
                GlobalFunctions = new Dictionary<string, string>(),
                GlobalConstants = new Dictionary<string, string>(),
                GlobalEnums = new Dictionary<string, string>()
            };

            // Process classes
            ProcessClasses(api.Classes, result);

            // Process builtin classes (Vector2, Color, etc.)
            ProcessClasses(api.BuiltinClasses, result);

            // Process utility functions (print, lerp, etc.)
            if (api.UtilityFunctions != null)
            {
                foreach (var func in api.UtilityFunctions)
                {
                    if (!string.IsNullOrEmpty(func.Name) && !string.IsNullOrEmpty(func.Description))
                        result.GlobalFunctions[func.Name] = func.Description;
                }
            }

            // Process global constants
            if (api.GlobalConstants != null)
            {
                foreach (var constant in api.GlobalConstants)
                {
                    if (!string.IsNullOrEmpty(constant.Name) && !string.IsNullOrEmpty(constant.Description))
                        result.GlobalConstants[constant.Name] = constant.Description;
                }
            }

            // Process global enums
            if (api.GlobalEnums != null)
            {
                foreach (var enumDef in api.GlobalEnums)
                {
                    if (!string.IsNullOrEmpty(enumDef.Name) && !string.IsNullOrEmpty(enumDef.Description))
                        result.GlobalEnums[enumDef.Name] = enumDef.Description;
                }
            }

            return result;
        }

        /// <summary>
        /// Saves documentation data to a JSON file.
        /// </summary>
        public static void SaveDocumentationData(GDDocumentationData data, string outputPath)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(outputPath))
                throw new ArgumentNullException(nameof(outputPath));

            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(data, _writeJsonOptions);
            File.WriteAllText(outputPath, json);
        }

        private static void ProcessClasses(List<GDExtApiClass>? classes, GDDocumentationData result)
        {
            if (classes == null)
                return;

            foreach (var cls in classes)
            {
                if (string.IsNullOrEmpty(cls.Name))
                    continue;

                var typeDocs = new GDTypeDocumentation();
                var hasContent = false;

                if (!string.IsNullOrEmpty(cls.BriefDescription))
                {
                    typeDocs.Brief = cls.BriefDescription;
                    hasContent = true;
                }

                if (!string.IsNullOrEmpty(cls.Description))
                {
                    typeDocs.Description = cls.Description;
                    hasContent = true;
                }

                // Methods
                if (cls.Methods != null)
                {
                    foreach (var method in cls.Methods)
                    {
                        if (!string.IsNullOrEmpty(method.Name) && !string.IsNullOrEmpty(method.Description))
                        {
                            typeDocs.Methods ??= new Dictionary<string, string>();
                            typeDocs.Methods[method.Name] = method.Description;
                            hasContent = true;
                        }
                    }
                }

                // Properties
                if (cls.Properties != null)
                {
                    foreach (var prop in cls.Properties)
                    {
                        if (!string.IsNullOrEmpty(prop.Name) && !string.IsNullOrEmpty(prop.Description))
                        {
                            typeDocs.Properties ??= new Dictionary<string, string>();
                            typeDocs.Properties[prop.Name] = prop.Description;
                            hasContent = true;
                        }
                    }
                }

                // Members (builtin value types use "members" instead of "properties")
                if (cls.Members != null)
                {
                    foreach (var member in cls.Members)
                    {
                        if (!string.IsNullOrEmpty(member.Name) && !string.IsNullOrEmpty(member.Description))
                        {
                            typeDocs.Properties ??= new Dictionary<string, string>();
                            typeDocs.Properties[member.Name] = member.Description;
                            hasContent = true;
                        }
                    }
                }

                // Signals
                if (cls.Signals != null)
                {
                    foreach (var signal in cls.Signals)
                    {
                        if (!string.IsNullOrEmpty(signal.Name) && !string.IsNullOrEmpty(signal.Description))
                        {
                            typeDocs.Signals ??= new Dictionary<string, string>();
                            typeDocs.Signals[signal.Name] = signal.Description;
                            hasContent = true;
                        }
                    }
                }

                // Constants
                if (cls.Constants != null)
                {
                    foreach (var constant in cls.Constants)
                    {
                        if (!string.IsNullOrEmpty(constant.Name) && !string.IsNullOrEmpty(constant.Description))
                        {
                            typeDocs.Constants ??= new Dictionary<string, string>();
                            typeDocs.Constants[constant.Name] = constant.Description;
                            hasContent = true;
                        }
                    }
                }

                // Enums
                if (cls.Enums != null)
                {
                    foreach (var enumDef in cls.Enums)
                    {
                        if (!string.IsNullOrEmpty(enumDef.Name) && !string.IsNullOrEmpty(enumDef.Description))
                        {
                            typeDocs.Enums ??= new Dictionary<string, string>();
                            typeDocs.Enums[enumDef.Name] = enumDef.Description;
                            hasContent = true;
                        }
                    }
                }

                if (hasContent)
                    result.Types![cls.Name] = typeDocs;
            }
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions _writeJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };

        // ========================================
        // Internal models for extension_api.json
        // ========================================

        internal class GDExtensionApi
        {
            [JsonPropertyName("classes")]
            public List<GDExtApiClass>? Classes { get; set; }

            [JsonPropertyName("builtin_classes")]
            public List<GDExtApiClass>? BuiltinClasses { get; set; }

            [JsonPropertyName("utility_functions")]
            public List<GDExtApiMethod>? UtilityFunctions { get; set; }

            [JsonPropertyName("global_enums")]
            public List<GDExtApiEnum>? GlobalEnums { get; set; }

            [JsonPropertyName("global_constants")]
            public List<GDExtApiConstant>? GlobalConstants { get; set; }
        }

        internal class GDExtApiClass
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("brief_description")]
            public string? BriefDescription { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("methods")]
            public List<GDExtApiMethod>? Methods { get; set; }

            [JsonPropertyName("properties")]
            public List<GDExtApiProperty>? Properties { get; set; }

            [JsonPropertyName("signals")]
            public List<GDExtApiSignal>? Signals { get; set; }

            [JsonPropertyName("constants")]
            public List<GDExtApiConstant>? Constants { get; set; }

            [JsonPropertyName("enums")]
            public List<GDExtApiEnum>? Enums { get; set; }

            [JsonPropertyName("members")]
            public List<GDExtApiProperty>? Members { get; set; }
        }

        internal class GDExtApiMethod
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }
        }

        internal class GDExtApiProperty
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }
        }

        internal class GDExtApiSignal
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }
        }

        internal class GDExtApiConstant
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }
        }

        internal class GDExtApiEnum
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }
        }
    }
}
