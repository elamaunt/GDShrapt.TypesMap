# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

GDShrapt.TypesMap is a C# library that maps GDScript type names to their C# equivalents in Godot Engine's GodotSharp bindings. It extracts and provides type metadata (methods, properties, signals, enums, constants) for bridging GDScript and C#. Part of the GDShrapt ecosystem for GDScript static analysis.

## Build and Test Commands

```bash
# Build the solution
dotnet build src/GDShrapt.TypesMap/GDShrapt.TypesMap.sln

# Build Release
dotnet build src/GDShrapt.TypesMap/GDShrapt.TypesMap.sln -c Release

# Run tests
dotnet test src/GDShrapt.TypesMap/GDShrapt.TypesMap.sln

# Create NuGet package
dotnet pack src/GDShrapt.TypesMap/GDShrapt.TypesMap.csproj -c Release
```

## Architecture

### Core Entry Point

`GDTypeHelper` (static class) provides three data source methods:
- `ExtractTypeDatasFromManifest()` - Loads pre-extracted data from embedded `AssemblyData.json` (fast, no runtime reflection, works standalone)
- `ExtractTypeDatasFromFile(string filePath)` - Loads data from external JSON file
- `ExtractTypeDatasFromAssembly()` - Runtime extraction from GodotSharp assembly using Reflection and Mono.Cecil (requires Godot runtime)

Additional methods:
- `SaveAssemblyDataToFile(data, filePath?)` - Saves extracted data to JSON file
- `GetDefaultSavePath()` - Returns default path next to executing assembly

### Godot Editor Integration

`GDTypeExtractorNode` - Base Node class for in-editor data extraction. Inherit with `[Tool]` attribute:

```csharp
[Tool]
public partial class MyExtractor : GDTypeExtractorNode
{
    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            OutputPath = "res://data/AssemblyData.json";
            ExtractAndSave();
        }
    }
}
```

### Data Model

```
GDAssemblyData
├── GDAssemblyMetadata           # Version info, source, timestamp
│   ├── GodotVersion             # e.g., "4.5.1"
│   ├── DataFormatVersion        # For compatibility (currently 1)
│   ├── ExtractedAt              # UTC timestamp
│   ├── Source                   # "Assembly", "File", or "Manifest"
│   └── SourcePath               # File path (when Source is "File")
├── GDGlobalData                 # Global scope (Mathf, GD functions)
│   ├── MethodDatas              # Global methods like print(), lerp()
│   ├── PropertyDatas            # Global properties
│   ├── Constants                # Global constants like PI, TAU
│   ├── Enums                    # Global enums
│   └── GlobalTypes              # Built-in types (int, float, Vector2)
└── TypeDatas                    # Per-type data indexed by GDScript name
    └── GDTypeData
        ├── MethodDatas          # Methods with GDScript→C# name mapping
        ├── PropertyDatas        # Properties with type info
        ├── SignalDatas          # Signals with delegate info
        ├── Enums                # Nested enums
        └── Constants            # Type constants
```

### Key Types (all prefixed with GD)

- `GDTypeHelper` - Entry point, extraction/save methods
- `GDTypeExtractorNode` - Base Node for Godot editor extraction
- `GDAssemblyData` - Root container with metadata
- `GDAssemblyMetadata` - Version and source info
- `GDGlobalData` - Global scope metadata
- `GDTypeData` - Per-type metadata
- `GDMethodData` - Method signature info with parameters
- `GDPropertyData` - Property info with type details
- `GDSignalData` - Signal/event info
- `GDParameterInfo` - Detailed parameter metadata
- `GDEnumTypeInfo` - Enum mappings
- `GDConstantInfo` - Constant metadata
- `GDGlobalTypeProxyInfo` - Built-in type proxies

### Naming Convention

Properties use clear prefixes to distinguish GDScript vs C# names:
- `GDScriptName`, `GDScriptTypeName` - GDScript identifiers (snake_case)
- `CSharpName`, `CSharpTypeName`, `CSharpTypeFullName` - C# identifiers (PascalCase)

### Key Implementation Details

- **Partial Class Split**: `GDTypeHelper.cs` contains core extraction logic; `GDTypeHelper_EmbeddedGlobals.cs` contains manually-mapped global functions from Mathf/GD classes
- **IL Inspection**: Uses Mono.Cecil to parse static constructors of nested `MethodName`/`PropertyName`/`SignalName` types to extract GDScript→C# name mappings
- **Dictionary Structure**: `TypeDatas` is `Dictionary<GodotTypeName, Dictionary<CSharpFullName, GDTypeData>>` to handle type versions
- **Metadata Tracking**: All data sources populate `GDAssemblyMetadata` for version tracking

### Embedded Resource

`Files/AssemblyData.json` (~25MB) contains pre-extracted type data for Godot 4.5.1 (1708 types, 120 global methods, 21 enums, 4 constants). This is embedded as a resource and loaded by `ExtractTypeDatasFromManifest()`.

### Extractor Project

`src/GDShrapt.TypesMap.Extractor/` is a Godot project that regenerates `AssemblyData.json`:
1. Open in Godot Editor 4.5.1
2. Build C# solution (Alt+B)
3. Run the project (F5)
4. JSON is saved to `src/GDShrapt.TypesMap/Files/AssemblyData.json`

## Dependencies

- **GodotSharp** (v4.5.1) - Godot engine C# bindings
- **Mono.Cecil** (v0.11.6) - IL inspection for extracting name mappings

## Target Frameworks

net6.0, net8.0
