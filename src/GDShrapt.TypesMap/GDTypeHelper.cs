using Godot;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Text.Json;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Provides methods for extracting and mapping GDScript type information to C# types.
    /// This is the main entry point for the GDShrapt.TypesMap library.
    /// </summary>
    public static partial class GDTypeHelper
    {
        internal static GDUnresolvedBundle? LastUnresolvedBundle { get; private set; }

        private static bool TryParseJsonFromManifest<T>(string fileName, out T? values) where T : class
        {
            var data = ReadManifestFile(fileName);

            if (data == null)
            {
                values = null;
                return false;
            }

            values = JsonSerializer.Deserialize<T?>(data);
            return values != null;
        }

        private static string? ReadManifestFile(string name)
        {
            var assembly = typeof(GDTypeHelper).Assembly;

            try
            {
                var stream = assembly.GetManifestResourceStream($"GDShrapt.TypesMap.Files.{name}");

                if (stream == null)
                    return null;

                using (stream)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception) when (IsIOException())
            {
                return null;
            }

            static bool IsIOException() => true;
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = false
        };

        /// <summary>
        /// Extracts type data from the embedded manifest (AssemblyData.json).
        /// This method works without requiring Godot runtime and is the preferred
        /// method for standalone/CLI usage.
        /// </summary>
        /// <returns>The extracted assembly data, or null if the manifest cannot be loaded.</returns>
        public static GDAssemblyData? ExtractTypeDatasFromManifest()
        {
            TryParseJsonFromManifest<GDAssemblyData>("AssemblyData.json", out var data);

            if (data != null)
            {
                if (data.Metadata == null)
                {
                    data.Metadata = new GDAssemblyMetadata { Source = "Manifest" };
                }
                else
                {
                    data.Metadata.Source = "Manifest";
                }

                // Apply embedded globals to override/extend the loaded data
                // This ensures runtime code changes take effect without regenerating JSON
                if (data.GlobalData != null)
                {
                    AddEmbeddedGlobalEnums(data.GlobalData.Enums);
                    AddEmbeddedGlobalMethods(data.GlobalData.MethodDatas);
                    AddEmbeddedGlobalConstants(data.GlobalData.Constants);
                    AddEmbeddedGlobalTypes(data.GlobalData.GlobalTypes);
                    AddEmbeddedGDScriptMethods(data.GlobalData.MethodDatas);
                    data.GlobalData.BuildEnumsConstants();
                }
            }

            return data;
        }

        /// <summary>
        /// Extracts type data from a JSON file at the specified path.
        /// </summary>
        /// <param name="filePath">Path to the JSON file containing assembly data.</param>
        /// <returns>The extracted assembly data, or null if the file cannot be loaded.</returns>
        public static GDAssemblyData? ExtractTypeDatasFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                return null;

            try
            {
                var json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<GDAssemblyData>(json);

                if (data != null)
                {
                    if (data.Metadata == null)
                    {
                        data.Metadata = new GDAssemblyMetadata();
                    }

                    data.Metadata.Source = "File";
                    data.Metadata.SourcePath = filePath;
                }

                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Extracts type data directly from the GodotSharp assembly at runtime.
        /// This method requires Godot runtime and uses reflection and IL inspection.
        /// </summary>
        /// <returns>The extracted assembly data.</returns>
        public static GDAssemblyData ExtractTypeDatasFromAssembly()
        {
            return ExtractDataFromAssembly();
        }

        /// <summary>
        /// Saves assembly data to a JSON file.
        /// </summary>
        /// <param name="data">The assembly data to save.</param>
        /// <param name="filePath">
        /// Path to save the file. If null or empty, saves to "AssemblyData.json"
        /// in the same directory as the executing assembly.
        /// </param>
        public static void SaveAssemblyDataToFile(GDAssemblyData data, string? filePath = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetDefaultSavePath();
            }

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(data, JsonOptions);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Gets the default path for saving assembly data.
        /// </summary>
        /// <returns>Path to AssemblyData.json next to the executing assembly.</returns>
        public static string GetDefaultSavePath()
        {
            var assembly = typeof(GDTypeHelper).Assembly;
            var dir = Path.GetDirectoryName(assembly.Location) ?? ".";
            return Path.Combine(dir, "AssemblyData.json");
        }

        private static GDAssemblyData ExtractDataFromAssembly()
        {
            var assembly = Assembly.Load("GodotSharp");
            var definition = AssemblyDefinition.ReadAssembly(assembly.Location);
            var types = assembly.ExportedTypes.ToArray();

            var typeDatas = new Dictionary<string, Dictionary<string, GDTypeData>>();
            var globalData = new GDGlobalData();

            AddEmbeddedGlobalEnums(globalData.Enums);
            AddEmbeddedGlobalMethods(globalData.MethodDatas);
            AddEmbeddedGlobalConstants(globalData.Constants);
            AddEmbeddedGlobalTypes(globalData.GlobalTypes);
            AddEmbeddedGDScriptMethods(globalData.MethodDatas);

            var unresolvedBundle = new GDUnresolvedBundle();

            var manualMappedGlobalTypes = new HashSet<Type>
            {
                typeof(Mathf),
                typeof(GD)
            };

            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];

                if (t.Name.EndsWith("Instance", StringComparison.Ordinal))
                    continue;

                string name = GetGodotTypeName(t);

                if (!typeDatas.TryGetValue(name, out var dict))
                    typeDatas[name] = dict = new Dictionary<string, GDTypeData>();

                dict.Add(t.FullName!, ExtractTypeData(globalData, name, definition, t, unresolvedBundle, manualMappedGlobalTypes));
            }

            globalData.BuildEnumsConstants();

            LastUnresolvedBundle = unresolvedBundle;

            var metadata = new GDAssemblyMetadata
            {
                GodotVersion = Engine.GetVersionInfo()["string"].AsString(),
                DataFormatVersion = 1,
                ExtractedAt = DateTime.UtcNow,
                Source = "Assembly"
            };

            return new GDAssemblyData(metadata, globalData, typeDatas);
        }

        private static GDTypeData ExtractTypeData(GDGlobalData globalData, string godotTypeName, AssemblyDefinition definition, Type type, GDUnresolvedBundle bundle, HashSet<Type> manualMappedGlobalTypes)
        {
            var methodDatas = ExtractMethods(godotTypeName, definition, type);
            var propertyDatas = ExtractProperties(definition, type);
            var signalDatas = ExtractSignals(definition, type);
            var enums = ExtractEnums(godotTypeName, type, bundle);
            var constants = ExtractConstants(definition, godotTypeName, type, bundle);

            if (manualMappedGlobalTypes.Contains(type))
            {
                foreach (var item in constants)
                    globalData.Constants.Add(item.Key, item.Value);

                foreach (var item in methodDatas)
                {
                    if (!globalData.MethodDatas.TryAdd(item.Key, item.Value))
                    {
                        var list = globalData.MethodDatas[item.Key];
                        list.AddRange(item.Value);
                    }
                }

                foreach (var item in propertyDatas)
                    globalData.PropertyDatas.Add(item.Key, item.Value);

                foreach (var item in enums)
                {
                    if (globalData.Enums.TryGetValue(item.Key, out var list))
                    {
                        list.Add(item.Value);
                    }
                    else
                    {
                        globalData.Enums.Add(item.Key, new List<GDEnumTypeInfo>() { item.Value });
                    }
                }
            }

            return new GDTypeData(godotTypeName, type, methodDatas, propertyDatas, signalDatas, enums, constants);
        }

        private static Dictionary<string, List<GDMethodData>> ExtractMethods(string godotTypeName, AssemblyDefinition definition, Type type)
        {
            var methodNamesType = type.GetNestedType("MethodName");

            if (methodNamesType == null)
                return new Dictionary<string, List<GDMethodData>>();

            var methodNameTypeDefinition = definition.MainModule.GetType(methodNamesType.DeclaringType!.FullName + "/MethodName");
            var constructorBody = methodNameTypeDefinition.Methods.FirstOrDefault(m => m.Name == ".cctor")?.Body;

            if (constructorBody == null)
                return new Dictionary<string, List<GDMethodData>>();

            var methods = type.GetMethods();
            var methodDatas = new Dictionary<string, List<GDMethodData>>();

            string? loadedString = null;

            foreach (Instruction instruction in constructorBody.Instructions)
            {
                if (instruction.OpCode.Name == "ldstr" && instruction.Operand is string value)
                {
                    loadedString = value;
                }
                else if (instruction.OpCode.Name == "stsfld" && instruction.Operand is FieldReference operand)
                {
                    if (loadedString == null)
                        continue;

                    if (methodDatas.ContainsKey(loadedString))
                    {
                        GD.Print($"Key already handled for C# godot's method name: '{loadedString}', Type name: '{godotTypeName}'");
                    }
                    else
                    {
                        var list = methods.Where(x => x.Name == operand.Name).Select(x => new GDMethodData(loadedString, x)).ToList();
                        methodDatas.Add(loadedString, list);
                    }

                    loadedString = null;
                }
            }

            return methodDatas;
        }

        private static Dictionary<string, GDPropertyData> ExtractProperties(AssemblyDefinition definition, Type type)
        {
            var propertyNamesType = type.GetNestedType("PropertyName");

            if (propertyNamesType == null)
                return new Dictionary<string, GDPropertyData>();

            var methodNameTypeDefinition = definition.MainModule.GetType(propertyNamesType.DeclaringType!.FullName + "/PropertyName");
            var constructorBody = methodNameTypeDefinition.Methods.FirstOrDefault(m => m.Name == ".cctor")?.Body;

            if (constructorBody == null)
                return new Dictionary<string, GDPropertyData>();

            var properties = type.GetProperties();
            var propertyDatas = new Dictionary<string, GDPropertyData>();

            string? loadedString = null;

            foreach (Instruction instruction in constructorBody.Instructions)
            {
                if (instruction.OpCode.Name == "ldstr" && instruction.Operand is string value)
                {
                    loadedString = value;
                }
                else if (instruction.OpCode.Name == "stsfld" && instruction.Operand is FieldReference operand)
                {
                    if (loadedString == null)
                        continue;

                    var property = properties.FirstOrDefault(x => x.Name == operand.Name);
                    if (property != null)
                    {
                        propertyDatas.Add(loadedString, new GDPropertyData(operand.Name, property));
                    }

                    loadedString = null;
                }
            }

            return propertyDatas;
        }

        private static Dictionary<string, GDSignalData> ExtractSignals(AssemblyDefinition definition, Type type)
        {
            var signalsNamesType = type.GetNestedType("SignalName");

            if (signalsNamesType == null)
                return new Dictionary<string, GDSignalData>();

            var methodNameTypeDefinition = definition.MainModule.GetType(signalsNamesType.DeclaringType!.FullName + "/SignalName");
            var constructorBody = methodNameTypeDefinition.Methods.FirstOrDefault(m => m.Name == ".cctor")?.Body;

            if (constructorBody == null)
                return new Dictionary<string, GDSignalData>();

            var events = type.GetEvents();
            var signalDatas = new Dictionary<string, GDSignalData>();

            string? loadedString = null;

            foreach (Instruction instruction in constructorBody.Instructions)
            {
                if (instruction.OpCode.Name == "ldstr" && instruction.Operand is string value)
                {
                    loadedString = value;
                }
                else if (instruction.OpCode.Name == "stsfld" && instruction.Operand is FieldReference operand)
                {
                    if (loadedString == null)
                        continue;

                    var eventInfo = events.FirstOrDefault(x => x.Name == operand.Name);
                    if (eventInfo != null)
                    {
                        signalDatas.Add(loadedString, new GDSignalData(operand.Name, eventInfo));
                    }

                    loadedString = null;
                }
            }

            return signalDatas;
        }

        private static Dictionary<string, GDEnumTypeInfo> ExtractEnums(string godotTypeName, Type type, GDUnresolvedBundle bundle)
        {
            var enums = new Dictionary<string, GDEnumTypeInfo>();

            var godotEnums = ClassDB.ClassGetEnumList(godotTypeName, true);

            for (int i = 0; i < godotEnums.Length; i++)
            {
                var godotEnum = godotEnums[i];

                var dotnetEnum = GetNestedType(type, godotEnum) ?? GetNestedType(type, godotEnum + "Enum")!;

                if (dotnetEnum == null)
                {
                    if (!type.Name.EndsWith("Instance", StringComparison.Ordinal))
                    {
                        bundle.AddEnumIgnore(godotTypeName, godotEnum);
                    }
                    continue;
                }

                var godotConstants = ClassDB.ClassGetEnumConstants(godotTypeName, godotEnum, true);
                var dotnetValues = Enum.GetValues(dotnetEnum);

                enums.Add(godotEnum, new GDEnumTypeInfo(godotTypeName, dotnetEnum, godotConstants, dotnetValues));
            }

            return enums;
        }

        private static Dictionary<string, GDConstantInfo> ExtractConstants(AssemblyDefinition definition, string godotTypeName, Type type, GDUnresolvedBundle bundle)
        {
            var constants = new Dictionary<string, GDConstantInfo>();
            string[] godotConstants;

            if (type.IsEnum)
            {
                var dotNetConstants = Enum.GetValues(type).Cast<object>().Select(x => x.ToString()!).ToArray();

                var declaring = type.DeclaringType;

                if (declaring == null)
                {
                    bundle.AddGlobalEnumIgnore(godotTypeName, type);
                    return constants;
                }

                var declaringTypeName = declaring.GetGodotTypeName();

                godotConstants = ClassDB.ClassGetEnumConstants(declaringTypeName, godotTypeName, true);

                if (dotNetConstants.Length != godotConstants.Length)
                {
                    return constants;
                }

                for (int i = 0; i < dotNetConstants.Length; i++)
                    constants.Add(godotConstants[i], new GDConstantInfo(godotConstants[i], dotNetConstants[i], type.BaseType ?? typeof(long), type));
            }
            else
            {
                godotConstants = GetGodotConstants(godotTypeName);
                var dotNetConstants = type.GetAllPublicConstants().ToArray();

                if (dotNetConstants.Length != godotConstants.Length)
                {
                    bundle.AddConstantsTypeIgnore(godotTypeName, type);
                    return constants;
                }

                for (int i = 0; i < dotNetConstants.Length; i++)
                    constants.Add(godotConstants[i], new GDConstantInfo(godotConstants[i], dotNetConstants[i].Name, dotNetConstants[i].FieldType, type));
            }

            return constants;
        }

        private static string[] GetGodotConstants(string godotTypeName)
        {
            var constants = ClassDB.ClassGetIntegerConstantList(godotTypeName, true);
            var enums = ClassDB.ClassGetEnumList(godotTypeName, true);
            return constants.Except(enums.SelectMany(x => ClassDB.ClassGetEnumConstants(godotTypeName, x, true)).ToArray()).ToArray();
        }

        private static void Add(Dictionary<string, List<GDEnumTypeInfo>> dictionary, string enumName, GDEnumTypeInfo typeInfo)
        {
            if (dictionary.TryGetValue(enumName, out var list))
            {
                list.Add(typeInfo);
            }
            else
            {
                dictionary.Add(enumName, new List<GDEnumTypeInfo>() { typeInfo });
            }
        }

        private static Type? GetNestedType(Type type, string name)
        {
            return type.GetNestedTypes().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the GDScript type name for a .NET type.
        /// </summary>
        /// <param name="type">The .NET type.</param>
        /// <returns>The corresponding GDScript type name.</returns>
        public static string GetGodotTypeName(this Type type)
        {
            var name = type.Name;

            if (type.IsEnum)
            {
                if (name.EndsWith("enum", StringComparison.OrdinalIgnoreCase))
                    name = name.Substring(0, name.Length - "enum".Length);

                var declaringType = type.DeclaringType;

                if (declaringType != null)
                {
                    var declaringTypeName = declaringType.GetGodotTypeName();

                    var enums = ClassDB.ClassGetEnumList(declaringTypeName, true);
                    var first = enums?.FirstOrDefault(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));

                    if (first != null)
                        name = first;
                }
            }

            if (type.IsDefined(typeof(GodotClassNameAttribute), false))
            {
                var attr = type.GetCustomAttribute<GodotClassNameAttribute>(false);
                name = attr!.Name;
            }

            return name;
        }

        /// <summary>
        /// Determines whether the specified type is static.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is static; otherwise, false.</returns>
        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// Gets all public constant fields declared on the specified type.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <returns>A list of constant field infos.</returns>
        public static List<FieldInfo> GetAllPublicConstants(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.DeclaringType == type)
                .ToList();
        }
    }
}
