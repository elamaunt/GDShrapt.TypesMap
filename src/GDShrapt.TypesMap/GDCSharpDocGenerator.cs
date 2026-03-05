using System.Text;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Generates C# stub files with XML doc comments from TypesMap metadata.
    /// </summary>
    public static class GDCSharpDocGenerator
    {
        /// <summary>
        /// Generates a C# stub file for a single type with XML doc comments.
        /// </summary>
        public static string GenerateTypeFile(GDTypeData typeData)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Godot;");
            sb.AppendLine();
            sb.AppendLine("namespace Godot;");
            sb.AppendLine();

            // Type-level doc comment
            AppendXmlDoc(sb, typeData.BriefDescription, "");
            var baseType = typeData.GDScriptBaseTypeName;
            var extends = !string.IsNullOrEmpty(baseType) ? $" : {baseType}" : "";
            sb.AppendLine($"public partial class {typeData.GDScriptName}{extends}");
            sb.AppendLine("{");

            // Constants
            if (typeData.Constants != null)
            {
                foreach (var (name, constant) in typeData.Constants)
                {
                    AppendXmlDoc(sb, constant.Description, "    ");
                    var typeName = MapToGDScriptType(constant.CSharpValueTypeName);
                    sb.AppendLine($"    public const {typeName} {constant.GDScriptName ?? name} = default;");
                    sb.AppendLine();
                }
            }

            // Enums
            if (typeData.Enums != null)
            {
                foreach (var (name, enumInfo) in typeData.Enums)
                {
                    AppendXmlDoc(sb, enumInfo.Description, "    ");
                    sb.AppendLine($"    public enum {name}");
                    sb.AppendLine("    {");
                    if (enumInfo.IntValues != null)
                    {
                        foreach (var (valueName, intVal) in enumInfo.IntValues)
                        {
                            sb.AppendLine($"        {valueName} = {intVal},");
                        }
                    }
                    sb.AppendLine("    }");
                    sb.AppendLine();
                }
            }

            // Properties
            if (typeData.PropertyDatas != null)
            {
                foreach (var (name, prop) in typeData.PropertyDatas)
                {
                    AppendXmlDoc(sb, prop.Description, "    ");
                    var typeName = prop.GDScriptTypeName ?? "Variant";
                    var staticMod = prop.IsStatic ? "static " : "";
                    sb.AppendLine($"    public {staticMod}{typeName} {prop.GDScriptName ?? name} {{ get; set; }}");
                    sb.AppendLine();
                }
            }

            // Signals
            if (typeData.SignalDatas != null)
            {
                foreach (var (name, signal) in typeData.SignalDatas)
                {
                    AppendXmlDoc(sb, signal.Description, "    ");
                    sb.AppendLine($"    public signal {signal.GDScriptName ?? name}();");
                    sb.AppendLine();
                }
            }

            // Methods
            if (typeData.MethodDatas != null)
            {
                foreach (var (name, methodList) in typeData.MethodDatas)
                {
                    if (methodList == null || methodList.Count == 0)
                        continue;

                    var method = methodList[0];
                    AppendXmlDoc(sb, method.Description, "    ");

                    var returnType = method.GDScriptReturnTypeName ?? "void";
                    var staticMod = method.IsStatic ? "static " : "";
                    var paramStr = BuildParameterString(method);

                    sb.AppendLine($"    public {staticMod}{returnType} {method.GDScriptName ?? name}({paramStr}) {{ }}");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// Generates C# stubs for all types in the assembly data.
        /// </summary>
        public static Dictionary<string, string> GenerateAllTypeFiles(GDAssemblyData assemblyData)
        {
            var files = new Dictionary<string, string>();

            if (assemblyData.TypeDatas == null)
                return files;

            foreach (var (typeName, typeVariants) in assemblyData.TypeDatas)
            {
                var typeData = typeVariants.Values.FirstOrDefault();
                if (typeData == null)
                    continue;

                files[typeName] = GenerateTypeFile(typeData);
            }

            return files;
        }

        private static void AppendXmlDoc(StringBuilder sb, string? description, string indent)
        {
            if (string.IsNullOrEmpty(description))
                return;

            var plainText = StripBBCode(description);
            var lines = plainText.Split('\n');

            sb.AppendLine($"{indent}/// <summary>");
            foreach (var line in lines)
            {
                var trimmed = line.TrimEnd();
                if (string.IsNullOrEmpty(trimmed))
                    sb.AppendLine($"{indent}///");
                else
                    sb.AppendLine($"{indent}/// {EscapeXml(trimmed)}");
            }
            sb.AppendLine($"{indent}/// </summary>");
        }

        private static string StripBBCode(string text)
        {
            var sb = new StringBuilder(text.Length);
            int i = 0;
            while (i < text.Length)
            {
                if (text[i] == '[')
                {
                    int closeIndex = text.IndexOf(']', i + 1);
                    if (closeIndex > i)
                    {
                        var tag = text.Substring(i + 1, closeIndex - i - 1);

                        // Handle [br] as newline
                        if (tag == "br")
                            sb.Append('\n');

                        // Skip tag content
                        i = closeIndex + 1;
                        continue;
                    }
                }

                sb.Append(text[i]);
                i++;
            }
            return sb.ToString().Trim();
        }

        private static string EscapeXml(string text)
        {
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }

        private static string BuildParameterString(GDMethodData method)
        {
            if (method.Parameters == null || method.Parameters.Length == 0)
                return "";

            var parts = new List<string>();
            foreach (var param in method.Parameters)
            {
                var typeName = param.GDScriptTypeName ?? "Variant";
                var paramName = param.CSharpName ?? $"param{param.Position}";
                var defaultVal = param.HasDefaultValue ? $" = default" : "";
                parts.Add($"{typeName} {paramName}{defaultVal}");
            }
            return string.Join(", ", parts);
        }

        private static string MapToGDScriptType(string? csharpType)
        {
            if (string.IsNullOrEmpty(csharpType))
                return "Variant";

            return csharpType switch
            {
                "Int32" or "Int64" => "int",
                "Single" or "Double" => "float",
                "Boolean" => "bool",
                "String" => "String",
                _ => csharpType
            };
        }
    }
}
