using Godot;
using GDShrapt.TypesMap;

/// <summary>
/// Extracts type data from GodotSharp assembly and saves it to AssemblyData.json.
/// Run this project in Godot Editor to regenerate the embedded manifest.
/// </summary>
[Tool]
public partial class TypesMapExtractor : GDTypeExtractorNode
{
    // Path to AssemblyData.json in the main library
    private const string LibraryJsonPath = "../GDShrapt.TypesMap/Files/AssemblyData.json";

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
        GD.Print("=== Extraction complete! ===");
        GD.PrintRich("[color=green][b]SUCCESS:[/b] AssemblyData.json has been regenerated![/color]");
    }
}
