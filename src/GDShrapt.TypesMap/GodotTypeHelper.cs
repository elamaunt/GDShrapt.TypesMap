using Godot;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using Error = Godot.Error;

namespace GDShrapt.TypesMap
{
    public static partial class GodotTypeHelper
    {
       /* private class ConstantsTypeInfo
        {
            public string? DotNetTypeFullName { get; set; }
            public Dictionary<string, string>? Values { get; set; }
        }*/

       /* private static bool TryParseJsonFromManifest<T>(string fileName, out T? values) where T : class
        {
            var data = ReadManifestFile(fileName);

            if (data == null)
            {
                values = null;
                return false;
            }

            values = JsonSerializer.Deserialize<T?>(data);
            return values != null;
        }*/

       /* private static string? ReadManifestFile(string name)
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
       */
        public static GodotAssemblyData ExtractTypeDatas()
        {
            return ExtractDataFromAssembly();
        }

        private static GodotAssemblyData ExtractDataFromAssembly()
        {
            var assembly = Assembly.Load("GodotSharp");
            var definition = AssemblyDefinition.ReadAssembly(assembly.Location);
            var types = assembly.ExportedTypes.ToArray();

            var typeDatas = new Dictionary<string, Dictionary<string, TypeData>>();
            var globalData = new GlobalTypeData();

            for (int i = 0; i < types.Length; i++)
            {              
                var t = types[i];
                string name = GetGodotTypeName(t);

                if (!typeDatas.TryGetValue(name, out var dict))
                    typeDatas[name] = dict = new Dictionary<string, TypeData>();

                dict.Add(t.FullName!, ExtractTypeData(globalData, name, definition, t));
            }

            AddEmbeddedGlobalEnums(globalData.Enums);
            globalData.BuildEnumsConstants();
          
            /*constantsDictionary[""] = new List<ConstantsTypeInfo>()
            {
                new ConstantsTypeInfo(typeof(Godot.Mathf).FullName, new string[]
                {
                    "Tau",
                    "Pi",
                    "Inf",
                    "NaN",
                    "E",
                    "Sqrt2",
                    "Epsilon"
                },
                new string[]
                {
                    "Tau",
                    "Pi",
                    "Inf",
                    "NaN",
                    "E",
                    "Sqrt2",
                    "Epsilon",
                })
            };*/


            // InflateConstants(typeDatas);
            // InflateEnums(typeDatas, globalData);

            return new GodotAssemblyData(globalData, typeDatas);
        }

        /*private static void InflateConstants(Dictionary<string, Dictionary<string, TypeData>> typeDatas)
        {
            var constantsDictionary = GetConstantsDictionary();

            if (constantsDictionary != null)
            {
                foreach (var pair in constantsDictionary)
                {
                    if (typeDatas.TryGetValue(pair.Key, out var dict))
                    {
                        for (int i = 0; i < pair.Value.Count; i++)
                        {
                            var item = pair.Value[i];
                            var t = item.DotNetTypeFullName;

                            if (dict.TryGetValue(t, out var data))
                            {
                                foreach (var x in item.Values)
                                    data.Constants[x.Key] = x.Value;
                            }
                            else
                            {
                                throw new Exception("Unknown dotnet type " + t);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown godot type " + pair.Key);
                    }
                }
            }
        }*/

        /*private static void InflateEnums(Dictionary<string, Dictionary<string, TypeData>> typeDatas, TypeData globalData)
        {
            var enumsDictionary = GetEnumsDictionary(typeDatas, globalData);

            if (enumsDictionary != null)
            {
                foreach (var pair in enumsDictionary)
                {
                    if (!typeDatas.TryGetValue(pair.Key, out var dict))
                    {
                        foreach (var item in pair.Value)
                        {
                            if (typeDatas.TryGetValue(item.DotnetEnumName, out dict))
                            {
                                typeDatas.Add(pair.Key, dict);
                                break;
                            }
                        }

                        if (dict == null)
                            throw new Exception("Unknown godot type " + pair.Key);
                    }

                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        var item = pair.Value[i];

                        if (dict.TryGetValue(item.DotnetEnumFullName, out var data))
                        {
                            if (!data.IsEnum)
                                throw new Exception("This is not an enum type " + item.DotnetEnumFullName);

                            foreach (var x in item.Values)
                                data.Constants[x.Key] = x.Value;

                            if (item.ContaingTypeName == "")
                            {
                                AppendEnumToParentTypeData(pair.Key, globalData, item);
                            }
                            else
                            {
                                if (typeDatas.TryGetValue(item.ContaingTypeName, out var parentTypes))
                                {
                                    var realParentTypeData = parentTypes.FirstOrDefault(x => x.Value.Type.GetNestedTypes().Any(y => y.FullName == item.DotnetEnumFullName)).Value;

                                    if (realParentTypeData == null)
                                        throw new Exception("There is no a real parent between types " + item.ContaingTypeName);

                                    AppendEnumToParentTypeData(pair.Key, realParentTypeData, item);
                                }
                                else
                                {
                                    throw new Exception("This is not a parent type " + item.ContaingTypeName);
                                }
                            }
                        }
                        else
                        {
                            if (typeDatas.TryGetValue(item.DotnetEnumName, out var otherDict))
                            {
                                if (otherDict.TryGetValue(item.DotnetEnumFullName, out data))
                                {
                                    if (!data.IsEnum)
                                        throw new Exception("This is not an enum type " + item.DotnetEnumFullName);

                                    foreach (var x in item.Values)
                                        data.Constants[x.Key] = x.Value;

                                    continue;
                                }
                            }

                            if (item.ContaingTypeName == "")
                            {
                                AppendEnumToParentTypeData(pair.Key, globalData, item);
                            }
                            else
                            {
                                if (typeDatas.TryGetValue(item.ContaingTypeName, out var parentTypes))
                                {
                                    var realParentTypeData = parentTypes.FirstOrDefault(x => x.Value.Type.GetNestedTypes().Any(y => y.FullName == item.DotnetEnumFullName)).Value;

                                    if (realParentTypeData == null)
                                        throw new Exception("There is no a real parent between types " + item.ContaingTypeName);

                                    AppendEnumToParentTypeData(pair.Key, realParentTypeData, item);
                                }
                                else
                                {
                                    throw new Exception("This is not a parent type " + item.ContaingTypeName);
                                }
                            }

                            throw new Exception("Unknown dotnet type " + item.DotnetEnumFullName);
                        }
                    }
                }
            }
        }*/

        /*private static Dictionary<string, List<EnumTypeInfo>>? GetEnumsDictionary()
        {
            if (TryParseJsonFromManifest<Dictionary<string, List<EnumTypeInfo>>>("enums.json", out var values))
                return values;

            var enumsDictionary = new Dictionary<string, List<EnumTypeInfo>>();

            AddEmbeddedGlobalEnums(enumsDictionary);

            for (int i = 0; i < types.Length; i++)
            {
                var v = types[i].Value;

                //if (listTypeChecked.Contains(v.Type))
                //    continue;

                var declaringTypes = CollectDeclaringTypes(v.Type);

                var last = declaringTypes.LastOrDefault();

                if (last == null)
                {
                    //if (listNoType.Add(v.Type))
                    //{
                        // Append($"DotNetTypeNotContainingClass   {k}  {v.Type.Name}");
                    //}
                }
                else
                {
                    var lastName = last.Name;
                    if (last.IsDefined(typeof(GodotClassNameAttribute), false))
                    {
                        var attr = last.GetCustomAttribute<GodotClassNameAttribute>(false);
                        lastName = attr.Name;
                    }

                    var enums = ClassDB.ClassGetEnumList(lastName, true);

                    for (int n = 0; n < enums.Length; n++)
                    {
                        var enumName = enums[n];
                        var dotnetEnum = GetNestedType(last, enumName) ?? GetNestedType(last, enumName + "Enum");

                        if (dotnetEnum == null)
                        {
                            //Append($"DotNetTypeNotFound   {last.Name} : {enumName}");
                            continue;
                        }

                        var dotnetValues = Enum.GetValues(dotnetEnum);

                        var constants = ClassDB.ClassGetEnumConstants(lastName, enumName, true);
                        var typeInfo = new EnumTypeInfo(lastName, dotnetEnum, constants, dotnetValues);

                        if (enumsDictionary.TryGetValue(enumName, out var infosList))
                            infosList.Add(typeInfo);
                        else
                            enumsDictionary.Add(enumName, new List<EnumTypeInfo>() { typeInfo });

                       // listTypeChecked.Add(dotnetEnum);
                    }
                }
            }
        }*/

        private static TypeData ExtractTypeData(GlobalTypeData globalData, string godotTypeName, AssemblyDefinition definition, Type type)
        {
            var methodDatas = ExtractMethods(definition, type);
            var propertyDatas = ExtractProperties(definition, type);
            var signalDatas = ExtractSignals(definition, type);
            var enums = ExtractEnums(godotTypeName, type);
            var constants = ExtractConstants(godotTypeName, type);

            if (type.DeclaringType == null && type.IsStatic())
            {
                foreach (var item in constants)
                    globalData.Constants.Add(item.Key, (type.Name, item.Value));

                foreach (var item in methodDatas)
                    globalData.MethodDatas.Add(item.Key, item.Value);

                foreach (var item in propertyDatas)
                    globalData.PropertyDatas.Add(item.Key, item.Value);

                foreach (var item in enums)
                    globalData.Enums.Add(item.Key, item.Value);
            }

            return new TypeData(type, methodDatas, propertyDatas, signalDatas, enums, constants);
        }

        private static Dictionary<string, List<MethodData>> ExtractMethods(AssemblyDefinition definition, Type type)
        {
            var methodNamesType = type.GetNestedType("MethodName");

            if (methodNamesType == null)
                return new Dictionary<string, List<MethodData>>();

            var methodNameTypeDefinition = definition.MainModule.GetType(methodNamesType.DeclaringType.FullName + "/MethodName");
            var constructorBody = methodNameTypeDefinition.Methods.FirstOrDefault(m => m.Name == ".cctor")?.Body;

            if (constructorBody == null)
                return new Dictionary<string, List<MethodData>>();

            var methods = type.GetMethods();
            var methodDatas = new Dictionary<string, List<MethodData>>();

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

                    methodDatas.Add(loadedString, methods.Where(x => x.Name == operand.Name).Select(x => new MethodData(operand.Name, x)).ToList());
                    loadedString = null;
                }
            }

            return methodDatas;
        }

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

        private static Dictionary<string, EnumTypeInfo> ExtractEnums(string godotTypeName, Type type)
        {
            var enums = new Dictionary<string, EnumTypeInfo>();

            var godotEnums = ClassDB.ClassGetEnumList(godotTypeName, true);

            for (int i = 0; i < godotEnums.Length; i++)
            {
                var godotEnum = godotEnums[i];

                var dotnetEnum = GetNestedType(type, godotEnum) ?? GetNestedType(type, godotEnum + "Enum")!;
                var godotConstants = ClassDB.ClassGetEnumConstants(godotTypeName, godotEnum, true);
                var dotnetValues = Enum.GetValues(dotnetEnum!);

                enums.Add(godotEnum, new EnumTypeInfo(godotTypeName, dotnetEnum, godotConstants, dotnetValues));
            }

            return enums;
        }
        private static Dictionary<string, string> ExtractConstants(string godotTypeName, Type type)
        {
            var constants = new Dictionary<string, string>();
            
            var dotNetConstants = type.GetAllPublicConstants().Select(x => x.Name).ToArray();
            var godotConstants = GetGodotConstants(godotTypeName);

            for (int i = 0; i < dotNetConstants.Length; i++)
                constants.Add(godotConstants[i], dotNetConstants[i]);

            return constants;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static string[] GetGodotConstants(string godotsTypeName)
        {
            var constants = ClassDB.ClassGetIntegerConstantList(godotsTypeName, true);
            var enums = ClassDB.ClassGetEnumList(godotsTypeName, true);
            return constants.Except(enums.SelectMany(x => ClassDB.ClassGetEnumConstants(godotsTypeName, x, true)).ToArray()).ToArray();
        }

        private static void Add(Dictionary<string, EnumTypeInfo> dictionary, string enumName, EnumTypeInfo typeInfo)
        {
            dictionary.Add(enumName, typeInfo);
        }

        private static void AddEmbeddedGlobalEnums(Dictionary<string, EnumTypeInfo> dictionary)
        {
            Add(dictionary, "Side", new EnumTypeInfo("", typeof(Side),
                 new[]
                 {
                 "SIDE_LEFT",
                 "SIDE_TOP",
                 "SIDE_RIGHT",
                 "SIDE_BOTTOM",
                 },
            new[]
            {
            Side.Left,
                 Side.Top,
                 Side.Right,
                 Side.Bottom
                 }));

            Add(dictionary, "Corner", new EnumTypeInfo("", typeof(Corner),
                 new[]
                 {
                 "CORNER_TOP_LEFT",
                 "CORNER_TOP_RIGHT",
                 "CORNER_BOTTOM_RIGHT",
                 "CORNER_BOTTOM_LEFT",
                 },
            new[]
                 {
                 Corner.TopLeft,
                 Corner.TopRight,
                 Corner.BottomRight,
                 Corner.BottomLeft
                 }));

            Add(dictionary, "Orientation", new EnumTypeInfo("", typeof(Orientation),
              new[]
              {
               "HORIZONTAL",
               "VERTICAL",
              },
              new[]
              {
               Orientation.Horizontal,
               Orientation.Vertical,
              }));

            Add(dictionary, "ClockDirection", new EnumTypeInfo("", typeof(ClockDirection),
              new[]
              {
               "CLOCKWISE",
               "COUNTERCLOCKWISE",
              },
              new[]
              {
               ClockDirection.Clockwise,
               ClockDirection.Counterclockwise,
              }));

            Add(dictionary, "HorizontalAlignment", new EnumTypeInfo("", typeof(HorizontalAlignment),
              new[]
              {
               "HORIZONTAL_ALIGNMENT_LEFT",
               "HORIZONTAL_ALIGNMENT_CENTER",
               "HORIZONTAL_ALIGNMENT_RIGHT",
               "HORIZONTAL_ALIGNMENT_FILL",
              },
              new[]
              {
               HorizontalAlignment.Left,
               HorizontalAlignment.Center,
               HorizontalAlignment.Right,
               HorizontalAlignment.Fill,
              }));

            Add(dictionary, "VerticalAlignment", new EnumTypeInfo("", typeof(VerticalAlignment),
             new[]
             {
               "VERTICAL_ALIGNMENT_TOP",
               "VERTICAL_ALIGNMENT_CENTER",
               "VERTICAL_ALIGNMENT_BOTTOM",
               "VERTICAL_ALIGNMENT_FILL",
             },
             new[]
             {
               VerticalAlignment.Top,
               VerticalAlignment.Center,
               VerticalAlignment.Bottom,
               VerticalAlignment.Fill,
             }));

            Add(dictionary, "InlineAlignment", new EnumTypeInfo("", typeof(InlineAlignment),
             new[]
             {
               "INLINE_ALIGNMENT_TOP_TO",
               "INLINE_ALIGNMENT_CENTER_TO",
               "INLINE_ALIGNMENT_BASELINE_TO",
               "INLINE_ALIGNMENT_BOTTOM_TO",
               "INLINE_ALIGNMENT_TO_TOP",
               "INLINE_ALIGNMENT_TO_CENTER",
               "INLINE_ALIGNMENT_TO_BASELINE",
               "INLINE_ALIGNMENT_TO_BOTTOM",
               "INLINE_ALIGNMENT_TOP",
               "INLINE_ALIGNMENT_CENTER",
               "INLINE_ALIGNMENT_BOTTOM",
               "INLINE_ALIGNMENT_IMAGE_MASK",
               "INLINE_ALIGNMENT_TEXT_MASK",
             },
             new[]
             {
               InlineAlignment.TopTo,
               InlineAlignment.CenterTo,
               InlineAlignment.BaselineTo,
               InlineAlignment.BottomTo,
               InlineAlignment.ToTop,
               InlineAlignment.ToCenter,
               InlineAlignment.ToBaseline,
               InlineAlignment.ToBottom,
               InlineAlignment.Top,
               InlineAlignment.Center,
               InlineAlignment.Bottom,
               InlineAlignment.ImageMask,
               InlineAlignment.TextMask,
             }));

            Add(dictionary, "EulerOrder", new EnumTypeInfo("", typeof(EulerOrder),
            new[]
            {
               "EULER_ORDER_XYZ",
               "EULER_ORDER_XZY",
               "EULER_ORDER_YXZ",
               "EULER_ORDER_YZX",
               "EULER_ORDER_ZXY",
               "EULER_ORDER_ZYX",
            },
            new[]
            {
               EulerOrder.Xyz,
               EulerOrder.Xzy,
               EulerOrder.Yxz,
               EulerOrder.Yzx,
               EulerOrder.Zxy,
               EulerOrder.Zyx,
            }));

            Add(dictionary, "Key", new EnumTypeInfo("", typeof(Key),
            new[]
            {
               "KEY_NONE",
               "KEY_SPECIAL",
               "KEY_ESCAPE",
               "KEY_TAB",
               "KEY_BACKTAB",
               "KEY_BACKSPACE",
               "KEY_ENTER",
               "KEY_KP_ENTER",
               "KEY_INSERT",
               "KEY_DELETE",
               "KEY_PAUSE",
               "KEY_PRINT",
               "KEY_SYSREQ",
               "KEY_CLEAR",
               "KEY_HOME",
               "KEY_END",
               "KEY_LEFT",
               "KEY_UP",
               "KEY_RIGHT",
               "KEY_DOWN",
               "KEY_PAGEUP",
               "KEY_PAGEDOWN",
               "KEY_SHIFT",
               "KEY_CTRL",
               "KEY_META",
               "KEY_ALT",
               "KEY_CAPSLOCK",
               "KEY_NUMLOCK",
               "KEY_SCROLLLOCK",
               "KEY_F1",
               "KEY_F2",
               "KEY_F3",
               "KEY_F4",
               "KEY_F5",
               "KEY_F6",
               "KEY_F7",
               "KEY_F8",
               "KEY_F9",
               "KEY_F10",
               "KEY_F11",
               "KEY_F12",
               "KEY_F13",
               "KEY_F14",
               "KEY_F15",
               "KEY_F16",
               "KEY_F17",
               "KEY_F18",
               "KEY_F19",
               "KEY_F20",
               "KEY_F21",
               "KEY_F22",
               "KEY_F23",
               "KEY_F24",
               "KEY_F25",
               "KEY_F26",
               "KEY_F27",
               "KEY_F28",
               "KEY_F29",
               "KEY_F30",
               "KEY_F31",
               "KEY_F32",
               "KEY_F33",
               "KEY_F34",
               "KEY_F35",
               "KEY_KP_MULTIPLY",
               "KEY_KP_DIVIDE",
               "KEY_KP_SUBTRACT",
               "KEY_KP_PERIOD",
               "KEY_KP_ADD",
               "KEY_KP_0",
               "KEY_KP_1",
               "KEY_KP_2",
               "KEY_KP_3",
               "KEY_KP_4",
               "KEY_KP_5",
               "KEY_KP_6",
               "KEY_KP_7",
               "KEY_KP_8",
               "KEY_KP_9",
               "KEY_MENU",
               "KEY_HYPER",
               "KEY_HELP",
               "KEY_BACK",
               "KEY_FORWARD",
               "KEY_STOP",
               "KEY_REFRESH",
               "KEY_VOLUMEDOWN",
               "KEY_VOLUMEMUTE",
               "KEY_VOLUMEUP",
               "KEY_MEDIAPLAY",
               "KEY_MEDIASTOP",
               "KEY_MEDIAPREVIOUS",
               "KEY_MEDIANEXT",
               "KEY_MEDIARECORD",
               "KEY_HOMEPAGE",
               "KEY_FAVORITES",
               "KEY_SEARCH",
               "KEY_STANDBY",
               "KEY_OPENURL",
               "KEY_LAUNCHMAIL",
               "KEY_LAUNCHMEDIA",
               "KEY_LAUNCH0",
               "KEY_LAUNCH1",
               "KEY_LAUNCH2",
               "KEY_LAUNCH3",
               "KEY_LAUNCH4",
               "KEY_LAUNCH5",
               "KEY_LAUNCH6",
               "KEY_LAUNCH7",
               "KEY_LAUNCH8",
               "KEY_LAUNCH9",
               "KEY_LAUNCHA",
               "KEY_LAUNCHB",
               "KEY_LAUNCHC",
               "KEY_LAUNCHD",
               "KEY_LAUNCHE",
               "KEY_LAUNCHF",
               "KEY_GLOBE",
               "KEY_KEYBOARD",
               "KEY_JIS_EISU",
               "KEY_JIS_KANA",
               "KEY_UNKNOWN",
               "KEY_SPACE",
               "KEY_EXCLAM",
               "KEY_QUOTEDBL",
               "KEY_NUMBERSIGN",
               "KEY_DOLLAR",
               "KEY_PERCENT",
               "KEY_AMPERSAND",
               "KEY_APOSTROPHE",
               "KEY_PARENLEFT",
               "KEY_PARENRIGHT",
               "KEY_ASTERISK",
               "KEY_PLUS",
               "KEY_COMMA",
               "KEY_MINUS",
               "KEY_PERIOD",
               "KEY_SLASH",
               "KEY_0",
               "KEY_1",
               "KEY_2",
               "KEY_3",
               "KEY_4",
               "KEY_5",
               "KEY_6",
               "KEY_7",
               "KEY_8",
               "KEY_9",
               "KEY_COLON",
               "KEY_SEMICOLON",
               "KEY_LESS",
               "KEY_EQUAL",
               "KEY_GREATER",
               "KEY_QUESTION",
               "KEY_AT",
               "KEY_A",
               "KEY_B",
               "KEY_C",
               "KEY_D",
               "KEY_E",
               "KEY_F",
               "KEY_G",
               "KEY_H",
               "KEY_I",
               "KEY_J",
               "KEY_K",
               "KEY_L",
               "KEY_M",
               "KEY_N",
               "KEY_O",
               "KEY_P",
               "KEY_Q",
               "KEY_R",
               "KEY_S",
               "KEY_T",
               "KEY_U",
               "KEY_V",
               "KEY_W",
               "KEY_X",
               "KEY_Y",
               "KEY_Z",
               "KEY_BRACKETLEFT",
               "KEY_BACKSLASH",
               "KEY_BRACKETRIGHT",
               "KEY_ASCIICIRCUM",
               "KEY_UNDERSCORE",
               "KEY_QUOTELEFT",
               "KEY_BRACELEFT",
               "KEY_BAR",
               "KEY_BRACERIGHT",
               "KEY_ASCIITILDE",
               "KEY_YEN",
               "KEY_SECTION",
            },
            new[]
            {
               Key.None,
               Key.Special,
               Key.Escape,
               Key.Tab,
               Key.Backtab,
               Key.Backspace,
               Key.Enter,
               Key.KpEnter,
               Key.Insert,
               Key.Delete,
               Key.Pause ,
               Key.Print ,
               Key.Sysreq ,
               Key.Clear ,
               Key.Home ,
               Key.End ,
               Key.Left ,
               Key.Up ,
               Key.Right ,
               Key.Down ,
               Key.Pageup ,
               Key.Pagedown ,
               Key.Shift ,
               Key.Ctrl ,
               Key.Meta ,
               Key.Alt ,
               Key.Capslock ,
               Key.Numlock ,
               Key.Scrolllock ,
               Key.F1 ,
               Key.F2 ,
               Key.F3 ,
               Key.F4 ,
               Key.F5 ,
               Key.F6 ,
               Key.F7 ,
               Key.F8 ,
               Key.F9 ,
               Key.F10 ,
               Key.F11 ,
               Key.F12 ,
               Key.F13 ,
               Key.F14 ,
               Key.F15 ,
               Key.F16 ,
               Key.F17 ,
               Key.F18 ,
               Key.F19,
               Key.F20 ,
               Key.F21 ,
               Key.F22 ,
               Key.F23 ,
               Key.F24 ,
               Key.F25 ,
               Key.F26 ,
               Key.F27 ,
               Key.F28 ,
               Key.F29 ,
               Key.F30 ,
               Key.F31 ,
               Key.F32 ,
               Key.F33 ,
               Key.F34 ,
               Key.F35 ,
               Key.KpMultiply ,
               Key.KpDivide ,
               Key.KpSubtract ,
               Key.KpPeriod ,
               Key.KpAdd ,
               Key.Kp0 ,
               Key.Kp1 ,
               Key.Kp2 ,
               Key.Kp3 ,
               Key.Kp4 ,
               Key.Kp5 ,
               Key.Kp6 ,
               Key.Kp7 ,
               Key.Kp8 ,
               Key.Kp9 ,
               Key.Menu ,
               Key.Hyper ,
               Key.Help ,
               Key.Back ,
               Key.Forward ,
               Key.Stop ,
               Key.Refresh,
               Key.Volumedown ,
               Key.Volumemute ,
               Key.Volumeup ,
               Key.Mediaplay ,
               Key.Mediastop ,
               Key.Mediaprevious ,
               Key.Medianext ,
               Key.Mediarecord ,
               Key.Homepage ,
               Key.Favorites ,
               Key.Search ,
               Key.Standby ,
               Key.Openurl ,
               Key.Launchmail ,
               Key.Launchmedia ,
               Key.Launch0 ,
               Key.Launch1 ,
               Key.Launch2 ,
               Key.Launch3 ,
               Key.Launch4 ,
               Key.Launch5 ,
               Key.Launch6 ,
               Key.Launch7 ,
               Key.Launch8 ,
               Key.Launch9 ,
               Key.Launcha ,
               Key.Launchb ,
               Key.Launchc ,
               Key.Launchd ,
               Key.Launche ,
               Key.Launchf ,
               Key.Globe ,
               Key.Keyboard ,
               Key.JisEisu ,
               Key.JisKana ,
               Key.Unknown ,
               Key.Space ,
               Key.Exclam ,
               Key.Quotedbl ,
               Key.Numbersign ,
               Key.Dollar ,
               Key.Percent ,
               Key.Ampersand ,
               Key.Apostrophe ,
               Key.Parenleft ,
               Key.Parenright ,
               Key.Asterisk ,
               Key.Plus ,
               Key.Comma ,
               Key.Minus ,
               Key.Period ,
               Key.Slash ,
               Key.Key0 ,
               Key.Key1 ,
               Key.Key2 ,
               Key.Key3 ,
               Key.Key4 ,
               Key.Key5 ,
               Key.Key6 ,
               Key.Key7 ,
               Key.Key8 ,
               Key.Key9 ,
               Key.Colon ,
               Key.Semicolon ,
               Key.Less ,
               Key.Equal ,
               Key.Greater ,
               Key.Question ,
               Key.At ,
               Key.A ,
               Key.B ,
               Key.C ,
               Key.D ,
               Key.E ,
               Key.F ,
               Key.G ,
               Key.H ,
               Key.I ,
               Key.J ,
               Key.K ,
               Key.L ,
               Key.M,
               Key.N ,
               Key.O ,
               Key.P ,
               Key.Q ,
               Key.R ,
               Key.S ,
               Key.T ,
               Key.U ,
               Key.V ,
               Key.W ,
               Key.X ,
               Key.Y ,
               Key.Z ,
               Key.Bracketleft ,
               Key.Backslash ,
               Key.Bracketright ,
               Key.Asciicircum ,
               Key.Underscore ,
               Key.Quoteleft ,
               Key.Braceleft ,
               Key.Bar ,
               Key.Braceright ,
               Key.Asciitilde ,
               Key.Yen ,
               Key.Section ,
            }));

            Add(dictionary, "KeyModifierMask", new EnumTypeInfo("", typeof(KeyModifierMask),
           new[]
           {
               "KEY_CODE_MASK",
               "KEY_MODIFIER_MAS",
               "KEY_MASK_CMD_OR_CTRL",
               "KEY_MASK_SHIFT",
               "KEY_MASK_ALT",
               "KEY_MASK_META",
               "KEY_MASK_CTRL",
               "KEY_MASK_KPAD",
               "KEY_MASK_GROUP_SWITCH",
           },
           new[]
           {
               KeyModifierMask.CodeMask,
               KeyModifierMask.ModifierMask,
               KeyModifierMask.MaskCmdOrCtrl,
               KeyModifierMask.MaskShift,
               KeyModifierMask.MaskAlt,
               KeyModifierMask.MaskMeta,
               KeyModifierMask.MaskCtrl,
               KeyModifierMask.MaskKpad,
               KeyModifierMask.MaskGroupSwitch,
           }));

            Add(dictionary, "MouseButton", new EnumTypeInfo("", typeof(MouseButton),
            new[]
            {
                   "MOUSE_BUTTON_NONE",
                   "MOUSE_BUTTON_LEFT",
                   "MOUSE_BUTTON_RIGHT",
                   "MOUSE_BUTTON_MIDDLE",
                   "MOUSE_BUTTON_WHEEL_UP",
                   "MOUSE_BUTTON_WHEEL_DOWN",
                   "MOUSE_BUTTON_WHEEL_LEFT",
                   "MOUSE_BUTTON_WHEEL_RIGHT",
                   "MOUSE_BUTTON_XBUTTON1",
                   "MOUSE_BUTTON_XBUTTON2",
            },
            new[]
            {
                   MouseButton.None,
                   MouseButton.Left,
                   MouseButton.Right,
                   MouseButton.Middle,
                   MouseButton.WheelUp,
                   MouseButton.WheelDown,
                   MouseButton.WheelLeft,
                   MouseButton.WheelRight,
                   MouseButton.Xbutton1,
                   MouseButton.Xbutton2,
            }));

            Add(dictionary, "MouseButtonMask", new EnumTypeInfo("", typeof(MouseButtonMask),
            new[]
            {
                   "MOUSE_BUTTON_MASK_LEFT",
                   "MOUSE_BUTTON_MASK_RIGHT",
                   "MOUSE_BUTTON_MASK_MIDDLE",
                   "MOUSE_BUTTON_MASK_MB_XBUTTON1",
                   "MOUSE_BUTTON_MASK_MB_XBUTTON2",
            },
            new[]
            {
                   MouseButtonMask.Left,
                   MouseButtonMask.Right,
                   MouseButtonMask.Middle,
                   MouseButtonMask.MbXbutton1,
                   MouseButtonMask.MbXbutton2,
            }));

            Add(dictionary, "JoyButton", new EnumTypeInfo("", typeof(JoyButton),
            new[]
            {
                   "JOY_BUTTON_INVALID",
                   "JOY_BUTTON_A",
                   "JOY_BUTTON_B",
                   "JOY_BUTTON_X",
                   "JOY_BUTTON_Y",
                   "JOY_BUTTON_BACK",
                   "JOY_BUTTON_GUIDE",
                   "JOY_BUTTON_START",
                   "JOY_BUTTON_LEFT_STICK",
                   "JOY_BUTTON_RIGHT_STICK",
                   "JOY_BUTTON_LEFT_SHOULDER",
                   "JOY_BUTTON_RIGHT_SHOULDER",
                   "JOY_BUTTON_DPAD_UP",
                   "JOY_BUTTON_DPAD_DOWN",
                   "JOY_BUTTON_DPAD_LEFT",
                   "JOY_BUTTON_DPAD_RIGHT",
                   "JOY_BUTTON_MISC1",
                   "JOY_BUTTON_PADDLE1",
                   "JOY_BUTTON_PADDLE2",
                   "JOY_BUTTON_PADDLE3",
                   "JOY_BUTTON_PADDLE4",
                   "JOY_BUTTON_TOUCHPAD",
                   "JOY_BUTTON_SDL_MAX",
                   "JOY_BUTTON_MAX",
            },
            new[]
            {
                   JoyButton.Invalid,
                   JoyButton.A,
                   JoyButton.B,
                   JoyButton.X,
                   JoyButton.Y,
                   JoyButton.Back,
                   JoyButton.Guide,
                   JoyButton.Start,
                   JoyButton.LeftStick,
                   JoyButton.RightStick,
                   JoyButton.LeftShoulder,
                   JoyButton.RightShoulder,
                   JoyButton.DpadUp,
                   JoyButton.DpadDown,
                   JoyButton.DpadLeft,
                   JoyButton.DpadRight,
                   JoyButton.Misc1,
                   JoyButton.Paddle1,
                   JoyButton.Paddle2,
                   JoyButton.Paddle3,
                   JoyButton.Paddle4,
                   JoyButton.Touchpad,
                   JoyButton.SdlMax,
                   JoyButton.Max,
            }));

            Add(dictionary, "JoyAxis", new EnumTypeInfo("", typeof(JoyAxis),
            new[]
            {
                   "JOY_AXIS_INVALID",
                   "JOY_AXIS_LEFT_X",
                   "JOY_AXIS_LEFT_Y",
                   "JOY_AXIS_RIGHT_X",
                   "JOY_AXIS_RIGHT_Y",
                   "JOY_AXIS_TRIGGER_LEFT",
                   "JOY_AXIS_TRIGGER_RIGHT",
                   "JOY_AXIS_SDL_MAX",
                   "JOY_AXIS_MAX",
              },
              new[]
              {
                   JoyAxis.Invalid,
                   JoyAxis.LeftX,
                   JoyAxis.LeftY,
                   JoyAxis.RightX,
                   JoyAxis.RightY,
                   JoyAxis.TriggerLeft,
                   JoyAxis.TriggerRight,
                   JoyAxis.SdlMax,
                   JoyAxis.Max,
            }));

            Add(dictionary, "MidiMessage", new EnumTypeInfo("", typeof(MidiMessage),
            new[]
            {
                   "MIDI_MESSAGE_NONE",
                   "MIDI_MESSAGE_NOTE_OFF",
                   "MIDI_MESSAGE_NOTE_ON",
                   "MIDI_MESSAGE_AFTERTOUCH",
                   "MIDI_MESSAGE_CONTROL_CHANGE",
                   "MIDI_MESSAGE_PROGRAM_CHANGE",
                   "MIDI_MESSAGE_CHANNEL_PRESSURE",
                   "MIDI_MESSAGE_PITCH_BEND",
                   "MIDI_MESSAGE_SYSTEM_EXCLUSIVE",
                   "MIDI_MESSAGE_QUARTER_FRAME",
                   "MIDI_MESSAGE_SONG_POSITION_POINTER",
                   "MIDI_MESSAGE_SONG_SELECT",
                   "MIDI_MESSAGE_TUNE_REQUEST",
                   "MIDI_MESSAGE_TIMING_CLOCK",
                   "MIDI_MESSAGE_START",
                   "MIDI_MESSAGE_CONTINUE",
                   "MIDI_MESSAGE_STOP",
                   "MIDI_MESSAGE_ACTIVE_SENSING",
                   "MIDI_MESSAGE_SYSTEM_RESET",
              },
              new[]
              {
                   MidiMessage.None,
                   MidiMessage.NoteOff,
                   MidiMessage.NoteOn,
                   MidiMessage.Aftertouch,
                   MidiMessage.ControlChange,
                   MidiMessage.ProgramChange,
                   MidiMessage.ChannelPressure,
                   MidiMessage.PitchBend,
                   MidiMessage.SystemExclusive,
                   MidiMessage.QuarterFrame,
                   MidiMessage.SongPositionPointer,
                   MidiMessage.SongSelect,
                   MidiMessage.TuneRequest,
                   MidiMessage.TimingClock,
                   MidiMessage.Start,
                   MidiMessage.Continue,
                   MidiMessage.Stop,
                   MidiMessage.ActiveSensing,
                   MidiMessage.SystemReset,
            }));

            Add(dictionary, "Error", new EnumTypeInfo("", typeof(Error),
           new[]
           {
                   "OK",
                   "FAILED",
                   "ERR_UNAVAILABLE",
                   "ERR_UNCONFIGURED",
                   "ERR_UNAUTHORIZED",
                   "ERR_PARAMETER_RANGE_ERROR",
                   "ERR_OUT_OF_MEMORY",
                   "ERR_FILE_NOT_FOUND",
                   "ERR_FILE_BAD_DRIVE",
                   "ERR_FILE_BAD_PATH",
                   "ERR_FILE_NO_PERMISSION",
                   "ERR_FILE_ALREADY_IN_USE",
                   "ERR_FILE_CANT_OPEN",
                   "ERR_FILE_CANT_WRITE",
                   "ERR_FILE_CANT_READ",
                   "ERR_FILE_UNRECOGNIZED",
                   "ERR_FILE_CORRUPT",
                   "ERR_FILE_MISSING_DEPENDENCIES",
                   "ERR_FILE_EOF",
                   "ERR_CANT_OPEN",
                   "ERR_CANT_CREATE",
                   "ERR_QUERY_FAILED",
                   "ERR_ALREADY_IN_USE",
                   "ERR_LOCKED",
                   "ERR_TIMEOUT",
                   "ERR_CANT_CONNECT",
                   "ERR_CANT_RESOLVE",
                   "ERR_CONNECTION_ERROR",
                   "ERR_CANT_ACQUIRE_RESOURCE",
                   "ERR_CANT_FORK",
                   "ERR_INVALID_DATA",
                   "ERR_INVALID_PARAMETER",
                   "ERR_ALREADY_EXISTS",
                   "ERR_DOES_NOT_EXIST",
                   "ERR_DATABASE_CANT_READ",
                   "ERR_DATABASE_CANT_WRITE",
                   "ERR_COMPILATION_FAILED",
                   "ERR_METHOD_NOT_FOUND",
                   "ERR_LINK_FAILED",
                   "ERR_SCRIPT_FAILED",
                   "ERR_CYCLIC_LINK",
                   "ERR_INVALID_DECLARATION",
                   "ERR_DUPLICATE_SYMBOL",
                   "ERR_PARSE_ERROR",
                   "ERR_BUSY",
                   "ERR_SKIP",
                   "ERR_HELP",
                   "ERR_BUG",
                   "ERR_PRINTER_ON_FIRE",
             },
             new[]
             {
                   Error.Ok,
                   Error.Failed,
                   Error.Unavailable,
                   Error.Unconfigured,
                   Error.Unauthorized,
                   Error.ParameterRangeError,
                   Error.OutOfMemory,
                   Error.FileNotFound,
                   Error.FileBadDrive,
                   Error.FileBadPath,
                   Error.FileNoPermission,
                   Error.FileAlreadyInUse,
                   Error.FileCantOpen,
                   Error.FileCantWrite,
                   Error.FileCantRead,
                   Error.FileUnrecognized,
                   Error.FileCorrupt,
                   Error.FileMissingDependencies,
                   Error.FileEof,
                   Error.CantOpen,
                   Error.CantCreate,
                   Error.QueryFailed,
                   Error.AlreadyInUse,
                   Error.Locked,
                   Error.Timeout,
                   Error.CantConnect,
                   Error.CantResolve,
                   Error.ConnectionError,
                   Error.CantAcquireResource,
                   Error.CantFork,
                   Error.InvalidData,
                   Error.InvalidParameter,
                   Error.AlreadyExists,
                   Error.DoesNotExist,
                   Error.DatabaseCantRead,
                   Error.DatabaseCantWrite,
                   Error.CompilationFailed,
                   Error.MethodNotFound,
                   Error.LinkFailed,
                   Error.ScriptFailed,
                   Error.CyclicLink,
                   Error.InvalidDeclaration,
                   Error.DuplicateSymbol,
                   Error.ParseError,
                   Error.Busy,
                   Error.Skip,
                   Error.Help,
                   Error.Bug,
                   Error.PrinterOnFire,
           }));

            Add(dictionary, "PropertyHint", new EnumTypeInfo("", typeof(PropertyHint),
    new[]
    {
               "PROPERTY_HINT_NONE",
               "PROPERTY_HINT_RANGE",
               "PROPERTY_HINT_ENUM",
               "PROPERTY_HINT_ENUM_SUGGESTION",
               "PROPERTY_HINT_EXP_EASING",
               "PROPERTY_HINT_LINK",
               "PROPERTY_HINT_FLAGS",
               "PROPERTY_HINT_LAYERS_2D_RENDER",
               "PROPERTY_HINT_LAYERS_2D_PHYSICS",
               "PROPERTY_HINT_LAYERS_2D_NAVIGATION",
               "PROPERTY_HINT_LAYERS_3D_RENDER",
               "PROPERTY_HINT_LAYERS_3D_PHYSICS",
               "PROPERTY_HINT_LAYERS_3D_NAVIGATION",
               "PROPERTY_HINT_LAYERS_AVOIDANCE",
               "PROPERTY_HINT_FILE",
               "PROPERTY_HINT_DIR",
               "PROPERTY_HINT_GLOBAL_FILE",
               "PROPERTY_HINT_GLOBAL_DIR",
               "PROPERTY_HINT_RESOURCE_TYPE",
               "PROPERTY_HINT_MULTILINE_TEXT",
               "PROPERTY_HINT_EXPRESSION",
               "PROPERTY_HINT_PLACEHOLDER_TEXT",
               "PROPERTY_HINT_COLOR_NO_ALPHA",
               "PROPERTY_HINT_OBJECT_ID",
               "PROPERTY_HINT_TYPE_STRING",
               "PROPERTY_HINT_NODE_PATH_TO_EDITED_NODE",
               "PROPERTY_HINT_OBJECT_TOO_BIG",
               "PROPERTY_HINT_NODE_PATH_VALID_TYPES",
               "PROPERTY_HINT_SAVE_FILE",
               "PROPERTY_HINT_GLOBAL_SAVE_FILE",
               "PROPERTY_HINT_INT_IS_OBJECTID",
               "PROPERTY_HINT_INT_IS_POINTER",
               "PROPERTY_HINT_ARRAY_TYPE",
               "PROPERTY_HINT_LOCALE_ID",
               "PROPERTY_HINT_LOCALIZABLE_STRING",
               "PROPERTY_HINT_NODE_TYPE",
               "PROPERTY_HINT_HIDE_QUATERNION_EDIT",
               "PROPERTY_HINT_PASSWORD",
               "PROPERTY_HINT_MAX",
    },
    new[]
    {
               PropertyHint.None,
               PropertyHint.Range,
               PropertyHint.Enum,
               PropertyHint.EnumSuggestion,
               PropertyHint.ExpEasing,
               PropertyHint.Link,
               PropertyHint.Flags,
               PropertyHint.Layers2DRender,
               PropertyHint.Layers2DPhysics,
               PropertyHint.Layers2DNavigation,
               PropertyHint.Layers3DRender,
               PropertyHint.Layers3DPhysics,
               PropertyHint.Layers3DNavigation,
               PropertyHint.LayersAvoidance,
               PropertyHint.File,
               PropertyHint.Dir,
               PropertyHint.GlobalFile,
               PropertyHint.GlobalDir,
               PropertyHint.ResourceType,
               PropertyHint.MultilineText,
               PropertyHint.Expression,
               PropertyHint.PlaceholderText,
               PropertyHint.ColorNoAlpha,
               PropertyHint.ObjectId,
               PropertyHint.TypeString,
               PropertyHint.NodePathToEditedNode,
               PropertyHint.ObjectTooBig,
               PropertyHint.NodePathValidTypes,
               PropertyHint.SaveFile,
               PropertyHint.GlobalSaveFile,
               PropertyHint.IntIsObjectid,
               PropertyHint.IntIsPointer,
               PropertyHint.ArrayType,
               PropertyHint.LocaleId,
               PropertyHint.LocalizableString,
               PropertyHint.NodeType,
               PropertyHint.HideQuaternionEdit,
               PropertyHint.Password,
               PropertyHint.Max,
    }));


            Add(dictionary, "PropertyUsageFlags", new EnumTypeInfo("", typeof(PropertyUsageFlags),
            new[]
            {
                   "PROPERTY_USAGE_NONE",
                   "PROPERTY_USAGE_STORAGE",
                   "PROPERTY_USAGE_EDITOR",
                   "PROPERTY_USAGE_INTERNAL",
                   "PROPERTY_USAGE_CHECKABLE",
                   "PROPERTY_USAGE_CHECKED",
                   "PROPERTY_USAGE_GROUP",
                   "PROPERTY_USAGE_CATEGORY",
                   "PROPERTY_USAGE_SUBGROUP",
                   "PROPERTY_USAGE_CLASS_IS_BITFIELD",
                   "PROPERTY_USAGE_NO_INSTANCE_STATE",
                   "PROPERTY_USAGE_RESTART_IF_CHANGED",
                   "PROPERTY_USAGE_SCRIPT_VARIABLE",
                   "PROPERTY_USAGE_STORE_IF_NULL",
                   "PROPERTY_USAGE_UPDATE_ALL_IF_MODIFIED",
                   "PROPERTY_USAGE_SCRIPT_DEFAULT_VALUE",
                   "PROPERTY_USAGE_CLASS_IS_ENUM",
                   "PROPERTY_USAGE_NIL_IS_VARIANT",
                   "PROPERTY_USAGE_ARRAY",
                   "PROPERTY_USAGE_ALWAYS_DUPLICATE",
                   "PROPERTY_USAGE_NEVER_DUPLICATE",
                   "PROPERTY_USAGE_HIGH_END_GFX",
                   "PROPERTY_USAGE_NODE_PATH_FROM_SCENE_ROOT",
                   "PROPERTY_USAGE_RESOURCE_NOT_PERSISTENT",
                   "PROPERTY_USAGE_KEYING_INCREMENTS",
                   "PROPERTY_USAGE_DEFERRED_SET_RESOURCE",
                   "PROPERTY_USAGE_EDITOR_INSTANTIATE_OBJECT",
                   "PROPERTY_USAGE_EDITOR_BASIC_SETTING",
                   "PROPERTY_USAGE_READ_ONLY",
                   "PROPERTY_USAGE_SECRET",
                   "PROPERTY_USAGE_DEFAULT",
                   "PROPERTY_USAGE_NO_EDITOR",
              },
              new[]
              {
                   PropertyUsageFlags.None,
                   PropertyUsageFlags.Storage,
                   PropertyUsageFlags.Editor,
                   PropertyUsageFlags.Internal,
                   PropertyUsageFlags.Checkable,
                   PropertyUsageFlags.Checked,
                   PropertyUsageFlags.Group,
                   PropertyUsageFlags.Category,
                   PropertyUsageFlags.Subgroup,
                   PropertyUsageFlags.ClassIsBitfield,
                   PropertyUsageFlags.NoInstanceState,
                   PropertyUsageFlags.RestartIfChanged,
                   PropertyUsageFlags.ScriptVariable,
                   PropertyUsageFlags.StoreIfNull,
                   PropertyUsageFlags.UpdateAllIfModified,
                   PropertyUsageFlags.ScriptDefaultValue,
                   PropertyUsageFlags.ClassIsEnum,
                   PropertyUsageFlags.NilIsVariant,
                   PropertyUsageFlags.Array,
                   PropertyUsageFlags.AlwaysDuplicate,
                   PropertyUsageFlags.NeverDuplicate,
                   PropertyUsageFlags.HighEndGfx,
                   PropertyUsageFlags.NodePathFromSceneRoot,
                   PropertyUsageFlags.ResourceNotPersistent,
                   PropertyUsageFlags.KeyingIncrements,
                   PropertyUsageFlags.DeferredSetResource,
                   PropertyUsageFlags.EditorInstantiateObject,
                   PropertyUsageFlags.EditorBasicSetting,
                   PropertyUsageFlags.ReadOnly,
                   PropertyUsageFlags.Secret,
                   PropertyUsageFlags.Default,
                   PropertyUsageFlags.NoEditor,
            }));

            Add(dictionary, "MethodFlags", new EnumTypeInfo("", typeof(MethodFlags),
            new[]
            {
                   "METHOD_FLAG_NORMAL",
                   "METHOD_FLAG_EDITOR",
                   "METHOD_FLAG_CONST",
                   "METHOD_FLAG_VIRTUAL",
                   "METHOD_FLAG_VARARG",
                   "METHOD_FLAG_STATIC",
                   "METHOD_FLAG_OBJECT_CORE",
                   "METHOD_FLAGS_DEFAULT",
              },
              new[]
              {
                   MethodFlags.Normal,
                   MethodFlags.Editor,
                   MethodFlags.Const,
                   MethodFlags.Virtual,
                   MethodFlags.Vararg,
                   MethodFlags.Static,
                   MethodFlags.ObjectCore,
                   MethodFlags.Default,
            }));

            Add(dictionary, "Type", new EnumTypeInfo("", typeof(Variant.Type),
            new[]
            {
                   "TYPE_NIL",
                   "TYPE_BOOL",
                   "TYPE_INT",
                   "TYPE_FLOAT",
                   "TYPE_STRING",
                   "TYPE_VECTOR2",
                   "TYPE_VECTOR2I",
                   "TYPE_RECT2",
                   "TYPE_RECT2I",
                   "TYPE_VECTOR3",
                   "TYPE_VECTOR3I",
                   "TYPE_TRANSFORM2D",
                   "TYPE_VECTOR4",
                   "TYPE_VECTOR4I",
                   "TYPE_PLANE",
                   "TYPE_QUATERNION",
                   "TYPE_AABB",
                   "TYPE_BASIS",
                   "TYPE_TRANSFORM3D",
                   "TYPE_PROJECTION",
                   "TYPE_COLOR",
                   "TYPE_STRING_NAME",
                   "TYPE_NODE_PATH",
                   "TYPE_RID",
                   "TYPE_OBJECT",
                   "TYPE_CALLABLE",
                   "TYPE_SIGNAL",
                   "TYPE_DICTIONARY",
                   "TYPE_ARRAY",
                   "TYPE_PACKED_BYTE_ARRAY",
                   "TYPE_PACKED_INT32_ARRAY",
                   "TYPE_PACKED_INT64_ARRAY",
                   "TYPE_PACKED_FLOAT32_ARRAY",
                   "TYPE_PACKED_FLOAT64_ARRAY",
                   "TYPE_PACKED_STRING_ARRAY",
                   "TYPE_PACKED_VECTOR2_ARRAY",
                   "TYPE_PACKED_VECTOR3_ARRAY",
                   "TYPE_PACKED_COLOR_ARRAY",
                   "TYPE_MAX",
              },
              new[]
              {
                   Variant.Type.Nil,
                   Variant.Type.Bool,
                   Variant.Type.Int,
                   Variant.Type.Float,
                   Variant.Type.String,
                   Variant.Type.Vector2,
                   Variant.Type.Vector2I,
                   Variant.Type.Rect2,
                   Variant.Type.Rect2I,
                   Variant.Type.Vector3,
                   Variant.Type.Vector3I,
                   Variant.Type.Transform2D,
                   Variant.Type.Vector4,
                   Variant.Type.Vector4I,
                   Variant.Type.Plane,
                   Variant.Type.Quaternion,
                   Variant.Type.Aabb,
                   Variant.Type.Basis,
                   Variant.Type.Transform3D,
                   Variant.Type.Projection,
                   Variant.Type.Color,
                   Variant.Type.StringName,
                   Variant.Type.NodePath,
                   Variant.Type.Rid,
                   Variant.Type.Object,
                   Variant.Type.Callable,
                   Variant.Type.Signal,
                   Variant.Type.Dictionary,
                   Variant.Type.Array,
                   Variant.Type.PackedByteArray,
                   Variant.Type.PackedInt32Array,
                   Variant.Type.PackedInt64Array,
                   Variant.Type.PackedFloat32Array,
                   Variant.Type.PackedFloat64Array,
                   Variant.Type.PackedStringArray,
                   Variant.Type.PackedVector2Array,
                   Variant.Type.PackedVector3Array,
                   Variant.Type.PackedColorArray,
                   Variant.Type.Max,
            }));

            Add(dictionary, "Operator", new EnumTypeInfo("", typeof(Variant.Operator),
           new[]
           {
                   "OP_EQUAL",
                   "OP_NOT_EQUAL",
                   "OP_LESS",
                   "OP_LESS_EQUAL",
                   "OP_GREATER",
                   "OP_GREATER_EQUAL",
                   "OP_ADD",
                   "OP_SUBTRACT",
                   "OP_MULTIPLY",
                   "OP_DIVIDE",
                   "OP_NEGATE",
                   "OP_POSITIVE",
                   "OP_MODULE",
                   "OP_POWER",
                   "OP_SHIFT_LEFT",
                   "OP_SHIFT_RIGHT",
                   "OP_BIT_AND",
                   "OP_BIT_OR",
                   "OP_BIT_XOR",
                   "OP_BIT_NEGATE",
                   "OP_AND",
                   "OP_OR",
                   "OP_XOR",
                   "OP_NOT",
                   "OP_IN",
                   "OP_MAX",
             },
             new[]
             {
                   Variant.Operator.Equal,
                   Variant.Operator.NotEqual,
                   Variant.Operator.Less,
                   Variant.Operator.LessEqual,
                   Variant.Operator.Greater,
                   Variant.Operator.GreaterEqual,
                   Variant.Operator.Add,
                   Variant.Operator.Subtract,
                   Variant.Operator.Multiply,
                   Variant.Operator.Divide,
                   Variant.Operator.Negate,
                   Variant.Operator.Positive,
                   Variant.Operator.Module,
                   Variant.Operator.Power,
                   Variant.Operator.ShiftLeft,
                   Variant.Operator.ShiftRight,
                   Variant.Operator.BitAnd,
                   Variant.Operator.BitOr,
                   Variant.Operator.BitXor,
                   Variant.Operator.BitNegate,
                   Variant.Operator.And,
                   Variant.Operator.Or,
                   Variant.Operator.Xor,
                   Variant.Operator.Not,
                   Variant.Operator.In,
                   Variant.Operator.Max,
           }));

        }

        private static Type? GetNestedType(Type type, string name)
        {
            return type.GetNestedTypes().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /* private static Dictionary<string, List<ConstantsTypeInfo>>? GetConstantsDictionary()
         {
             if (TryParseJsonFromManifest<Dictionary<string, List<ConstantsTypeInfo>>>("constants.json", out var values))
                 return values;


         }*/


        /* private static void AppendEnumToParentTypeData(string enumName, TypeData parentData, EnumTypeInfo enumData)
         {
             parentData.Enums.Add(enumName, enumData);

             foreach (var item in enumData.Values)
                 parentData.EnumsConstants.Add(item.Key, enumData);
         }*/

        public static string GetGodotTypeName(this Type type)
        {
            var name = type.Name;

            var attrType = type.Assembly.GetType("GodotClassNameAttribute")!;

            if (type.IsDefined(attrType, false))
            {
                var attr = type.GetCustomAttribute(attrType, false);
                name = (string)attrType.GetProperty("Name")!.GetValue(attr)!;
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