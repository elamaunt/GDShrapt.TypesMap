# GDShrapt.TypesMap

[![NuGet](https://img.shields.io/nuget/v/GDShrapt.TypesMap.svg)](https://www.nuget.org/packages/GDShrapt.TypesMap)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Godot](https://img.shields.io/badge/Godot-4.5.1-478CBF?logo=godot-engine&logoColor=white)](https://godotengine.org/)
[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%208.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support%20GDShrapt-FF5E5B?logo=ko-fi&logoColor=white)](https://ko-fi.com/elamaunt)

A GDScript to C# type mapping library for Godot Engine. Part of the [GDShrapt](https://github.com/elamaunt/GDShrapt) project family for GDScript static analysis.

## Overview

GDShrapt.TypesMap provides comprehensive type metadata that bridges GDScript type names to their C# equivalents in Godot's GodotSharp bindings. It extracts and exposes information about:

- **Types** - Class names, inheritance, namespaces
- **Methods** - GDScript to C# name mapping, parameters, return types, overloads
- **Properties** - Property names, types, read/write capabilities
- **Signals** - Signal names and delegate information
- **Enums** - Enum values and constant mappings
- **Constants** - Global constants like PI, TAU, INF
- **Metadata** - Godot version, data format version, extraction timestamp

### Statistics (Godot 4.5.1)

| Category | Count |
|----------|-------|
| Types | 1,708 |
| Global Methods | 120 |
| Global Enums | 21 |
| Global Constants | 4 |

## Installation

```bash
dotnet add package GDShrapt.TypesMap
```

Or add to your `.csproj`:

```xml
<PackageReference Include="GDShrapt.TypesMap" Version="4.5.1.0" />
```

## Usage

### Three Data Sources

```csharp
using GDShrapt.TypesMap;

// 1. Load from embedded manifest (recommended for standalone/CLI)
// Works without Godot runtime - ideal for CLI tools and LSP servers
var data = GDTypeHelper.ExtractTypeDatasFromManifest();

// 2. Load from external JSON file
var data = GDTypeHelper.ExtractTypeDatasFromFile("path/to/AssemblyData.json");

// 3. Extract from GodotSharp assembly at runtime (requires Godot)
var data = GDTypeHelper.ExtractTypeDatasFromAssembly();
```

### Save Extracted Data

```csharp
// Save to specific path
GDTypeHelper.SaveAssemblyDataToFile(data, "path/to/save.json");

// Save to default location (next to executing assembly)
GDTypeHelper.SaveAssemblyDataToFile(data);
```

### In-Editor Extraction with Godot Node

```csharp
using Godot;
using GDShrapt.TypesMap;

[Tool]
public partial class MyTypeExtractor : GDTypeExtractorNode
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

### Working with Data

```csharp
var data = GDTypeHelper.ExtractTypeDatasFromManifest();

// Check metadata
Console.WriteLine($"Godot version: {data?.Metadata?.GodotVersion}");
Console.WriteLine($"Source: {data?.Metadata?.Source}");

// Access global methods
if (data?.GlobalData?.MethodDatas.TryGetValue("print", out var printMethods))
{
    foreach (var method in printMethods)
    {
        Console.WriteLine($"print -> {method.CSharpName}");
    }
}

// Access global constants
var pi = data?.GlobalData?.Constants["PI"];
Console.WriteLine($"PI constant: {pi?.Value}");

// Access type data
if (data?.TypeDatas.TryGetValue("Node2D", out var node2dVersions))
{
    var node2d = node2dVersions.Values.First();

    // Get properties
    var position = node2d.PropertyDatas?["position"];
    Console.WriteLine($"position -> {position?.CSharpName} ({position?.CSharpTypeName})");
}
```

## Data Model

```
GDAssemblyData
├── Metadata                     # Version and source info
│   ├── GodotVersion             # e.g., "4.5.1"
│   ├── DataFormatVersion        # For compatibility (currently 1)
│   ├── ExtractedAt              # UTC timestamp
│   ├── Source                   # "Assembly", "File", or "Manifest"
│   └── SourcePath               # File path (when Source is "File")
├── GlobalData                   # Global scope data
│   ├── MethodDatas              # Global methods (print, lerp, abs, etc.)
│   ├── PropertyDatas            # Global properties
│   ├── Constants                # Global constants (PI, TAU, INF, NAN)
│   ├── Enums                    # Global enums (Error, Key, etc.)
│   └── GlobalTypes              # Built-in types (int, float, Vector2, etc.)
└── TypeDatas                    # Per-type data indexed by GDScript name
    └── GDTypeData
        ├── MethodDatas          # Methods with GDScript→C# name mapping
        ├── PropertyDatas        # Properties with type info
        ├── SignalDatas          # Signals with delegate info
        ├── Enums                # Nested enums
        └── Constants            # Type constants
```

## Key Types

| Type | Description |
|------|-------------|
| `GDTypeHelper` | Main entry point - extraction and save methods |
| `GDTypeExtractorNode` | Base Node class for Godot editor extraction |
| `GDAssemblyData` | Root container with metadata |
| `GDAssemblyMetadata` | Version and source information |
| `GDGlobalData` | Global scope metadata |
| `GDTypeData` | Per-class type metadata |
| `GDMethodData` | Method signature with parameters |
| `GDPropertyData` | Property information with types |
| `GDSignalData` | Signal/event information |
| `GDParameterInfo` | Detailed parameter metadata |
| `GDEnumTypeInfo` | Enum value mappings |
| `GDConstantInfo` | Constant metadata |
| `GDGlobalTypeProxyInfo` | Built-in type proxies |

## Naming Convention

Properties use clear prefixes to distinguish GDScript vs C# names:
- `GDScriptName`, `GDScriptTypeName` - GDScript identifiers (snake_case)
- `CSharpName`, `CSharpTypeName`, `CSharpTypeFullName` - C# identifiers (PascalCase)

## Compatibility

- **Godot**: 4.5.1
- **.NET**: 6.0, 8.0
- **Dependencies**: GodotSharp 4.5.1, Mono.Cecil 0.11.6

## Integration with GDShrapt

This library is designed to work with:

- **GDShrapt.Reader** - GDScript parser and AST
- **GDShrapt.Validator** - Type inference and validation (uses `IGDRuntimeProvider`)
- **GDShrapt.Formatter** - Auto type hints generation

Example integration with validator:

```csharp
// Create runtime provider backed by TypesMap data
var assemblyData = GDTypeHelper.ExtractTypeDatasFromManifest();
var runtimeProvider = new CustomRuntimeProvider(assemblyData);

// Use with GDShrapt.Validator
var options = new GDValidationOptions
{
    RuntimeProvider = runtimeProvider
};
```

## Related Projects

This library is part of the **GDShrapt** ecosystem:

| Package | Description | NuGet |
|---------|-------------|-------|
| [GDShrapt.Reader](https://github.com/elamaunt/GDShrapt/tree/main/src/GDShrapt.Reader) | GDScript parser and AST | [![NuGet](https://img.shields.io/nuget/v/GDShrapt.Reader.svg)](https://www.nuget.org/packages/GDShrapt.Reader) |
| **GDShrapt.TypesMap** | Type mapping library | [![NuGet](https://img.shields.io/nuget/v/GDShrapt.TypesMap.svg)](https://www.nuget.org/packages/GDShrapt.TypesMap) |

## License

MIT License - see [LICENSE](LICENSE) for details.

## Author

[elamaunt](https://github.com/elamaunt)

## Links

- [GDShrapt](https://github.com/elamaunt/GDShrapt) - Main repository with parser and converter
- [NuGet Package](https://www.nuget.org/packages/GDShrapt.TypesMap)
- [Issues](https://github.com/elamaunt/GDShrapt.TypesMap/issues) - Report bugs or request features
