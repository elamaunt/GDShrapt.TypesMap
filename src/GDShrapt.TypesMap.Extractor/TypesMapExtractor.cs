using Godot;
using GDShrapt.TypesMap;

/// <summary>
/// Extracts type data from GodotSharp assembly and saves it to AssemblyData.json.
/// Also extracts documentation from extension_api.json if available.
/// Run this project in Godot Editor to regenerate the embedded manifest.
/// </summary>
[Tool]
public partial class TypesMapExtractor : GDTypeExtractorNode
{
    // Path to AssemblyData.json in the main library
    private const string LibraryJsonPath = "../GDShrapt.TypesMap/Files/AssemblyData.json";
    private const string DocJsonPath = "../GDShrapt.TypesMap/Files/DocumentationData.json";

    public override void _Ready()
    {
        GD.Print("=== GDShrapt.TypesMap Extractor ===");
        GD.Print($"Godot version: {Engine.GetVersionInfo()["string"]}");

        // Extract data from GodotSharp assembly
        var data = Extract();

        // Save to the library folder
        var projectPath = ProjectSettings.GlobalizePath("res://");
        var targetPath = System.IO.Path.GetFullPath(
            System.IO.Path.Combine(projectPath, LibraryJsonPath));

        GDTypeHelper.SaveAssemblyDataToFile(data, targetPath);

        GD.Print($"Data saved to: {targetPath}");
        GD.Print($"Types extracted: {data.TypeDatas?.Count ?? 0}");
        GD.Print($"Global methods: {data.GlobalData?.MethodDatas?.Count ?? 0}");
        GD.Print($"Global enums: {data.GlobalData?.Enums?.Count ?? 0}");
        GD.Print($"Global constants: {data.GlobalData?.Constants?.Count ?? 0}");

        // Extract documentation from extension_api.json if available
        ExtractDocumentation(projectPath);

        GD.Print("=== Extraction complete! ===");
        GD.PrintRich("[color=green][b]SUCCESS:[/b] AssemblyData.json has been regenerated![/color]");
    }

    private void ExtractDocumentation(string projectPath)
    {
        // Look for extension_api.json generated with --dump-extension-api-with-docs
        var extensionApiPath = System.IO.Path.GetFullPath(
            System.IO.Path.Combine(projectPath, "extension_api.json"));

        if (!System.IO.File.Exists(extensionApiPath))
        {
            GD.Print("extension_api.json not found — skipping documentation extraction.");
            GD.Print("To generate it, run: godot --dump-extension-api-with-docs");
            return;
        }

        try
        {
            var docData = GDDocumentationMerger.ExtractDocumentation(extensionApiPath);
            var docTargetPath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(projectPath, DocJsonPath));

            GDDocumentationMerger.SaveDocumentationData(docData, docTargetPath);

            GD.Print($"Documentation saved to: {docTargetPath}");
            GD.Print($"Documented types: {docData.Types?.Count ?? 0}");
            GD.Print($"Documented global functions: {docData.GlobalFunctions?.Count ?? 0}");
            GD.PrintRich("[color=green][b]SUCCESS:[/b] DocumentationData.json has been generated![/color]");
        }
        catch (System.Exception ex)
        {
            GD.PrintErr($"Failed to extract documentation: {ex.Message}");
        }
    }
}
