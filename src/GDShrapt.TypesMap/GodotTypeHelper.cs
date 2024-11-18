using Godot;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GDShrapt.TypesMap
{
    public static partial class GodotTypeHelper
    {
        internal static UnresolvedBundle? LastUnresolvedBundle { get; private set; }

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
            var assembly = typeof(GodotTypeHelper).Assembly;

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
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static GodotAssemblyData? ExtractTypeDatasFromManifest()
        {
            TryParseJsonFromManifest<GodotAssemblyData>("AssemblyData.json", out var data);
            return data;
        }

        public static GodotAssemblyData ExtractTypeDatasFromAssembly()
        {
            return ExtractDataFromAssembly();
        }

        private static GodotAssemblyData ExtractDataFromAssembly()
        {
            var assembly = Assembly.Load("GodotSharp");
            var definition = AssemblyDefinition.ReadAssembly(assembly.Location);
            var types = assembly.ExportedTypes.ToArray();

            var typeDatas = new Dictionary<string, Dictionary<string, TypeData>>();
            var globalData = new GlobalData();

            AddEmbeddedGlobalEnums(globalData.Enums);
            AddEmbeddedGlobalMethods(globalData.MethodDatas);
            AddEmbeddedGlobalConstants(globalData.Constants);
            AddEmbeddedGlobalTypes(globalData.GlobalTypes);

            var unresolvedBundle = new UnresolvedBundle();

            var manualMappedGlobalTypes = new HashSet<Type>
            {
                typeof(Mathf),
                typeof(GD)
            };

            for (int i = 0; i < types.Length; i++)
            {              
                var t = types[i];

                if (t.Name.EndsWith("Instance", StringComparison.Ordinal))
                {
                    //GD.Print("Skip type " + t.Name);
                    continue;
                }

                string name = GetGodotTypeName(t);

                if (!typeDatas.TryGetValue(name, out var dict))
                    typeDatas[name] = dict = new Dictionary<string, TypeData>();

                dict.Add(t.FullName!, ExtractTypeData(globalData, name, definition, t, unresolvedBundle, manualMappedGlobalTypes));
            }

            globalData.BuildEnumsConstants();

            //unresolvedBundle.Print();
            //LastUnresolvedBundle = unresolvedBundle;

            return new GodotAssemblyData(globalData, typeDatas);
        }

        private static TypeData ExtractTypeData(GlobalData globalData, string godotTypeName, AssemblyDefinition definition, Type type, UnresolvedBundle bundle, HashSet<Type> manualMappedGlobalTypes)
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
                        globalData.Enums.Add(item.Key, new List<EnumTypeInfo>() { item.Value });
                    }
                }
            }

            return new TypeData(godotTypeName, type, methodDatas, propertyDatas, signalDatas, enums, constants);
        }

        private static Dictionary<string, List<MethodData>> ExtractMethods(string godotTypeName, AssemblyDefinition definition, Type type)
        {
           // GD.Print($"TYPE: {godotTypeName}");
            var methodNamesType = type.GetNestedType("MethodName");

            if (methodNamesType == null)
                return new Dictionary<string, List<MethodData>>();

            bool printed = false;

            var methodNameTypeDefinition = definition.MainModule.GetType(methodNamesType.DeclaringType.FullName + "/MethodName");
            var constructorBody = methodNameTypeDefinition.Methods.FirstOrDefault(m => m.Name == ".cctor")?.Body;

            if (constructorBody == null)
                return new Dictionary<string, List<MethodData>>();

            var methods = type.GetMethods();
            var methodDatas = new Dictionary<string, List<MethodData>>();

            string? loadedString = null;
            string? replacedString = null;

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

                    //if (loadedString.Length > 1 && !ClassDB.ClassHasMethod(godotTypeName, loadedString) && loadedString.StartsWith('_'))
                    //{

                    //    replacedString = ToGodotStyleWithEarth(loadedString);
                    //}

                    if (methodDatas.ContainsKey(loadedString))
                    {
                        //GD.Print($"Key already handled for C# godot's method name: '{loadedString}', Type name: '{godotTypeName}'");
                        // if (!ClassDB.ClassHasMethod(godotTypeName, loadedString))
                        // {

                        //}

                        /*if (!printed)
                        {
                            printed = true;

                            var classMethods = ClassDB.ClassGetMethodList(godotTypeName);
                            var builder = new StringBuilder();
                            for (int i = 0; i < classMethods.Count; i++)
                            {
                                builder.Append($"|{classMethods[i]["name"]}");
                            }

                            GD.Print(builder.ToString());
                        }*/
                    }
                    else
                    {
                        var list = methods.Where(x => x.Name == operand.Name).Select(x => new MethodData(operand.Name, x)).ToList();
                        methodDatas.Add(loadedString, list);

                       /* if (replacedString != null)
                        {
                            var withStart = "_" + loadedString;

                            if (!methodDatas.ContainsKey(withStart))
                                methodDatas.Add(withStart, list);
                        }*/
                    }

                    loadedString = null;
                }
            }

            return methodDatas;
        }

       /* private static string ToGodotStyleWithEarth(string? loadedString)
        {
            string? replacedString;
            var b = new StringBuilder(loadedString.Length + 4);

            b.Append('_');
            b.Append(char.ToLowerInvariant(loadedString[1]));

            for (int i = 2; i < loadedString.Length; i++)
            {
                if (char.IsUpper(loadedString[i]))
                {
                    b.Append('_');
                    b.Append(char.ToLowerInvariant(loadedString[i]));
                }
                else
                {
                    b.Append(char.ToLowerInvariant(loadedString[i]));
                }
            }

            replacedString = loadedString;
            return replacedString;
        }*/

        private static Dictionary<string, PropertyData> ExtractProperties(AssemblyDefinition definition, Type type)
        {
            var propertyNamesType = type.GetNestedType("PropertyName");

            if (propertyNamesType == null)
                return new Dictionary<string, PropertyData>();

            var methodNameTypeDefinition = definition.MainModule.GetType(propertyNamesType.DeclaringType.FullName + "/PropertyName");
            var constructorBody = methodNameTypeDefinition.Methods.FirstOrDefault(m => m.Name == ".cctor")?.Body;

            if (constructorBody == null)
                return new Dictionary<string, PropertyData>();

            var properties = type.GetProperties();
            var propertyDatas = new Dictionary<string, PropertyData>();

            string loadedString = null;

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

                    propertyDatas.Add(loadedString, properties.Where(x => x.Name == operand.Name).Select(x => new PropertyData(operand.Name, x)).First());
                    loadedString = null;
                }
            }

            return propertyDatas;
        }

        private static Dictionary<string, SignalData> ExtractSignals(AssemblyDefinition definition, Type type)
        {
            var signalsNamesType = type.GetNestedType("SignalName");

            if (signalsNamesType == null)
                return new Dictionary<string, SignalData>();

            var methodNameTypeDefinition = definition.MainModule.GetType(signalsNamesType.DeclaringType.FullName + "/SignalName");
            var constructorBody = methodNameTypeDefinition.Methods.FirstOrDefault(m => m.Name == ".cctor")?.Body;

            if (constructorBody == null)
                return new Dictionary<string, SignalData>();

            var events = type.GetEvents();
            var propertyDatas = new Dictionary<string, SignalData>();

            string loadedString = null;

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

                    propertyDatas.Add(loadedString, events.Where(x => x.Name == operand.Name).Select(x => new SignalData(operand.Name, x)).First());
                    loadedString = null;
                }
            }

            return propertyDatas;
        }

        private static Dictionary<string, EnumTypeInfo> ExtractEnums(string godotTypeName, Type type, UnresolvedBundle bundle)
        {
            var enums = new Dictionary<string, EnumTypeInfo>();

            var godotEnums = ClassDB.ClassGetEnumList(godotTypeName, true);

            for (int i = 0; i < godotEnums.Length; i++)
            {
                var godotEnum = godotEnums[i];

                var dotnetEnum = GetNestedType(type, godotEnum) ?? GetNestedType(type, godotEnum + "Enum")!;

                if (dotnetEnum == null)
                {
                    if (!type.Name.EndsWith("Instance", StringComparison.Ordinal))
                    {
                       // GD.Print($"DotNet type for godots enum type not found: {godotTypeName}.{godotEnum}");
                        // GD.Print($"DotNet parent type: {type}");
                        // GD.Print($"DotNet nested types: {string.Join(", ", type.GetNestedTypes().Select(x => x.Name))}");

                       // bundle.AddEnumIgnore(godotTypeName, godotEnum);
                    }
                    continue;
                }

                var godotConstants = ClassDB.ClassGetEnumConstants(godotTypeName, godotEnum, true);
                var dotnetValues = Enum.GetValues(dotnetEnum);

                enums.Add(godotEnum, new EnumTypeInfo(godotTypeName, dotnetEnum, godotConstants, dotnetValues));
            }

            return enums;
        }
        private static Dictionary<string, ConstantInfo> ExtractConstants(AssemblyDefinition definition, string godotTypeName, Type type, UnresolvedBundle bundle)
        {
            var constants = new Dictionary<string, ConstantInfo>();
            string[] godotConstants;

            if (type.IsEnum)
            {
                var dotNetConstants = Enum.GetValues(type).Cast<object>().Select(x => x.ToString()!).ToArray();

                var declaring = type.DeclaringType;

                if (declaring == null)
                {
                    //GD.Print("Found global enum type. Should be handled manually: " + godotTypeName);
                    bundle.AddGlobalEnumIgnore(godotTypeName, type);
                    return constants;
                }

                var declaringTypeName = declaring.GetGodotTypeName();


                godotConstants = ClassDB.ClassGetEnumConstants(declaringTypeName, godotTypeName, true);

                if (dotNetConstants.Length != godotConstants.Length)
                {
                    //GD.Print("Ignoring type constants fot godots enum type: " + godotTypeName);
                    //var enums = ClassDB.ClassGetEnumList(godotTypeName, true);
                    // GD.Print($"Enums inside declaring class {declaringTypeName}: " + string.Join(", ",enums));

                    //for (int i = 0; i < godotConstants.Length; i++)
                    //    GD.Print(godotConstants[i]);
                    //for (int i = 0; i < dotNetConstants.Length; i++)
                    //    GD.Print(dotNetConstants[i]);
                    //bundle.AddConstantsTypeIgnore(godotTypeName, type);
                    return constants;
                }

                for (int i = 0; i < dotNetConstants.Length; i++)
                    constants.Add(godotConstants[i], new ConstantInfo(godotConstants[i], dotNetConstants[i], type.BaseType ?? typeof(long), type));
            }
            else
            {
                godotConstants = GetGodotConstants(godotTypeName);
                var dotNetConstants = type.GetAllPublicConstants().ToArray();

                if (dotNetConstants.Length != godotConstants.Length)
                {
                    //GD.Print("Ignoring type constants fot godots type: " + godotTypeName);
                    //GD.Print("Dotnet type: " + type.FullName);

                    bundle.AddConstantsTypeIgnore(godotTypeName, type);
                    return constants;
                }

                for (int i = 0; i < dotNetConstants.Length; i++)
                    constants.Add(godotConstants[i], new ConstantInfo(godotConstants[i], dotNetConstants[i].Name, dotNetConstants[i].FieldType, type));
            }

            return constants;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static string[] GetGodotConstants(string godotsTypeName)
        {
            var constants = ClassDB.ClassGetIntegerConstantList(godotsTypeName, true);
            var enums = ClassDB.ClassGetEnumList(godotsTypeName, true);
            return constants.Except(enums.SelectMany(x => ClassDB.ClassGetEnumConstants(godotsTypeName, x, true)).ToArray()).ToArray();
        }

        private static void Add(Dictionary<string, List<EnumTypeInfo>> dictionary, string enumName, EnumTypeInfo typeInfo)
        {
            if (dictionary.TryGetValue(enumName, out var list))
            {
                list.Add(typeInfo);
            }
            else
            {
                dictionary.Add(enumName, new List<EnumTypeInfo>() { typeInfo });
            }
        }

        private static Type? GetNestedType(Type type, string name)
        {
            return type.GetNestedTypes().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

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
                    else
                    {
                        //GD.Print($"Declaring type does not contain the enum. DeclaringType: {declaringTypeName}, Enum: {name}. ");
                    }
                }
            }

            if (type.IsDefined(typeof(GodotClassNameAttribute), false))
            {
                var attr = type.GetCustomAttribute<GodotClassNameAttribute>(false);
                name = attr!.Name;
            }

            return name;
        }

        public static bool IsStatic(this Type type)
        {
           return type.IsAbstract && type.IsSealed;
        }

        public static List<FieldInfo> GetAllPublicConstants(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.DeclaringType == type)
                .ToList();
        }
    }
}