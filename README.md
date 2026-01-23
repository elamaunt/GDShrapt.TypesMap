# GDShrapt.TypesMap

<!-- Badges -->
<p align="center">
  <a href="https://www.nuget.org/packages/GDShrapt.TypesMap"><img src="https://img.shields.io/nuget/v/GDShrapt.TypesMap.svg" alt="NuGet" /></a>
  <img src="https://img.shields.io/badge/Godot-4.5.1-478CBF?logo=godot-engine&logoColor=white" alt="Godot" />
  <img src="https://img.shields.io/badge/.NET-6.0%20%7C%208.0-512BD4?logo=dotnet&logoColor=white" alt=".NET" />
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License" /></a>
</p>

Godot built-in type metadata library for GDScript static analysis.

GDShrapt.TypesMap provides comprehensive mapping between GDScript type names and their C# equivalents in Godot's GodotSharp bindings. It extracts and exposes metadata about classes, methods, properties, signals, enums, and constants — enabling type resolution, code completion, and semantic analysis without requiring the Godot runtime.

This library is a **submodule** of the [GDShrapt](https://github.com/elamaunt/GDShrapt) language intelligence platform.

---

## What It Provides

- **Type mappings** — GDScript to C# class name resolution
- **Method signatures** — Parameters, return types, overloads
- **Properties** — Types, read/write capabilities
- **Signals** — Event names and delegate information
- **Enums and constants** — Global and per-type values
- **Metadata** — Godot version, extraction timestamp

### Statistics (Godot 4.5.1)

| Category | Count |
|----------|-------|
| Types | 1,708 |
| Global Methods | 120 |
| Global Enums | 21 |
| Global Constants | 4 |

---

## Installation

```bash
dotnet add package GDShrapt.TypesMap
```

---

## Versioning

Package version follows GodotSharp versioning: `{GodotSharp version}.{patch}`

| Version | Meaning |
|---------|---------|
| `4.5.1.0` | GodotSharp 4.5.1, initial release |
| `4.5.1.1` | GodotSharp 4.5.1, patch 1 |
| `4.6.0.0` | GodotSharp 4.6.0, initial release |

The first three numbers match the target GodotSharp version. The fourth number is the library patch version.

---

## Usage

### Data Sources

```csharp
using GDShrapt.TypesMap;

// Load from embedded manifest (recommended — works without Godot runtime)
var data = GDTypeHelper.ExtractTypeDatasFromManifest();

// Load from external JSON file
var data = GDTypeHelper.ExtractTypeDatasFromFile("path/to/AssemblyData.json");

// Extract from GodotSharp assembly at runtime (requires Godot)
var data = GDTypeHelper.ExtractTypeDatasFromAssembly();
```

### Working with Data

```csharp
var data = GDTypeHelper.ExtractTypeDatasFromManifest();

// Metadata
Console.WriteLine($"Godot: {data?.Metadata?.GodotVersion}");

// Global methods
if (data?.GlobalData?.MethodDatas.TryGetValue("print", out var methods))
    Console.WriteLine($"print -> {methods.First().CSharpName}");

// Type properties
if (data?.TypeDatas.TryGetValue("Node2D", out var versions))
{
    var node2d = versions.Values.First();
    var pos = node2d.PropertyDatas?["position"];
    Console.WriteLine($"position -> {pos?.CSharpName}");
}
```

---

## Data Model

```
GDAssemblyData
├── Metadata          Version info, source, timestamp
├── GlobalData        Global scope (methods, constants, enums)
└── TypeDatas         Per-type metadata indexed by GDScript name
    └── GDTypeData
        ├── MethodDatas
        ├── PropertyDatas
        ├── SignalDatas
        ├── Enums
        └── Constants
```

---

## Integration with GDShrapt

TypesMap provides the runtime type information layer for the GDShrapt semantic engine:

```
┌─────────────────────────────────────────────────────┐
│                 GDShrapt.Semantics                  │
│     Project Model · Type Inference · Refactoring    │
├─────────────────────────────────────┬───────────────┤
│         GDShrapt.Abstractions       │  TypesMap     │
│    Validator · Linter · Formatter   │  (this lib)   │
├─────────────────────────────────────┴───────────────┤
│                  GDShrapt.Reader                    │
│               Parser · AST · Tokens                 │
└─────────────────────────────────────────────────────┘
```

---

## Regenerating Type Data

The embedded `AssemblyData.json` can be regenerated for different Godot versions:

1. Open `src/GDShrapt.TypesMap.Extractor/` in Godot Editor
2. Build C# solution (Alt+B)
3. Run the project (F5)
4. JSON is saved to `src/GDShrapt.TypesMap/Files/AssemblyData.json`

---

## Dependencies

- **GodotSharp** 4.5.1
- **Mono.Cecil** 0.11.6

---

## Related

| Package | Description |
|---------|-------------|
| [GDShrapt](https://github.com/elamaunt/GDShrapt) | Language intelligence platform |
| [GDShrapt.Reader](https://www.nuget.org/packages/GDShrapt.Reader) | GDScript parser and AST |

---

## License

MIT License — see [LICENSE](LICENSE) for details.

## Author

[elamaunt](https://github.com/elamaunt)
