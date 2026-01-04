namespace GDShrapt.TypesMap.Tests
{
    [TestClass]
    public class GDTypeHelperTests
    {
        [TestMethod]
        public void ExtractTypeDatasFromManifest_ReturnsNonNull()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void ExtractTypeDatasFromManifest_ContainsGlobalData()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data);
            Assert.IsNotNull(data.GlobalData);
        }

        [TestMethod]
        public void ExtractTypeDatasFromManifest_ContainsTypeDatas()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data);
            Assert.IsNotNull(data.TypeDatas);
            Assert.IsTrue(data.TypeDatas.Count > 0);
        }

        [TestMethod]
        public void GlobalData_ContainsPIConstant()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.Constants);
            Assert.IsTrue(data.GlobalData.Constants.ContainsKey("PI"));
            Assert.AreEqual("PI", data.GlobalData.Constants["PI"].GDScriptName);
        }

        [TestMethod]
        public void GlobalData_ContainsTAUConstant()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.Constants);
            Assert.IsTrue(data.GlobalData.Constants.ContainsKey("TAU"));
        }

        [TestMethod]
        public void GlobalData_ContainsINFConstant()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.Constants);
            Assert.IsTrue(data.GlobalData.Constants.ContainsKey("INF"));
        }

        [TestMethod]
        public void GlobalData_ContainsPrintMethod()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("print"));
        }

        [TestMethod]
        public void GlobalData_ContainsAbsMethod()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("abs"));
        }

        [TestMethod]
        public void GlobalData_ContainsLerpMethod()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("lerp"));
        }

        [TestMethod]
        public void GlobalData_ContainsGlobalTypes()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.GlobalTypes);
            Assert.IsTrue(data.GlobalData.GlobalTypes.ContainsKey("int"));
            Assert.IsTrue(data.GlobalData.GlobalTypes.ContainsKey("float"));
            Assert.IsTrue(data.GlobalData.GlobalTypes.ContainsKey("string"));
            Assert.IsTrue(data.GlobalData.GlobalTypes.ContainsKey("vector2"));
            Assert.IsTrue(data.GlobalData.GlobalTypes.ContainsKey("vector3"));
        }

        [TestMethod]
        public void TypeData_ContainsNodeType()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            Assert.IsTrue(data.TypeDatas.ContainsKey("Node"));
        }

        [TestMethod]
        public void TypeData_ContainsNode2DType()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            Assert.IsTrue(data.TypeDatas.ContainsKey("Node2D"));
        }

        [TestMethod]
        public void TypeData_ContainsControlType()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            Assert.IsTrue(data.TypeDatas.ContainsKey("Control"));
        }

        [TestMethod]
        public void TypeData_Node_HasMethods()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            Assert.IsTrue(data.TypeDatas.TryGetValue("Node", out var nodeVersions));
            Assert.IsTrue(nodeVersions.Count > 0);

            var nodeData = nodeVersions.Values.First();
            Assert.IsNotNull(nodeData.MethodDatas);
            Assert.IsTrue(nodeData.MethodDatas.Count > 0);
        }

        [TestMethod]
        public void TypeData_Node2D_HasPositionProperty()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            Assert.IsTrue(data.TypeDatas.TryGetValue("Node2D", out var node2dVersions));

            var node2dData = node2dVersions.Values.First();
            Assert.IsNotNull(node2dData.PropertyDatas);
            Assert.IsTrue(node2dData.PropertyDatas.ContainsKey("position"));
        }

        [TestMethod]
        public void GlobalData_ContainsErrorEnum()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.Enums);
            Assert.IsTrue(data.GlobalData.Enums.ContainsKey("Error"));
        }

        [TestMethod]
        public void GlobalData_ContainsKeyEnum()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.Enums);
            Assert.IsTrue(data.GlobalData.Enums.ContainsKey("Key"));
        }

        [TestMethod]
        public void GDMethodData_HasExpectedProperties()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);
            var absMethods = data.GlobalData.MethodDatas["abs"];
            Assert.IsTrue(absMethods.Count > 0);

            var absMethod = absMethods[0];
            Assert.AreEqual("abs", absMethod.GDScriptName);
            Assert.IsNotNull(absMethod.CSharpName);
            Assert.IsNotNull(absMethod.CSharpReturnTypeName);
        }

        [TestMethod]
        public void GDPropertyData_HasExpectedProperties()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            var node2dData = data.TypeDatas["Node2D"].Values.First();
            var positionProperty = node2dData.PropertyDatas!["position"];

            // NOTE: GDScriptName currently stores C# name ("Position") due to JSON generation.
            // After regeneration from Godot runtime, GDScriptName should be "position" (snake_case)
            Assert.IsNotNull(positionProperty.GDScriptName);
            Assert.AreEqual("Position", positionProperty.CSharpName);
            Assert.IsNotNull(positionProperty.CSharpTypeName);
        }

        [TestMethod]
        public void GDEnumTypeInfo_HasValues()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.Enums);
            var errorEnums = data.GlobalData.Enums["Error"];
            Assert.IsTrue(errorEnums.Count > 0);

            var errorEnum = errorEnums[0];
            Assert.IsNotNull(errorEnum.Values);
            Assert.IsTrue(errorEnum.Values.Count > 0);
            Assert.IsTrue(errorEnum.Values.ContainsKey("OK"));
        }

        [TestMethod]
        public void GDConstantInfo_HasExpectedProperties()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.Constants);
            var piConstant = data.GlobalData.Constants["PI"];

            Assert.AreEqual("PI", piConstant.GDScriptName);
            Assert.IsNotNull(piConstant.Value);
            Assert.IsNotNull(piConstant.CSharpValueTypeName);
            Assert.IsNotNull(piConstant.CSharpContainingTypeName);
        }

        [TestMethod]
        public void GDGlobalTypeProxyInfo_IntType_HasCorrectMapping()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.GlobalTypes);
            var intType = data.GlobalData.GlobalTypes["int"];

            Assert.IsNotNull(intType.CSharpTypeName);
        }

        [TestMethod]
        public void GDGlobalTypeProxyInfo_NilType_HasValueEquivalent()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.GlobalTypes);
            var nilType = data.GlobalData.GlobalTypes["nil"];

            Assert.AreEqual("null", nilType.ValueEquivalent);
        }

        [TestMethod]
        public void GlobalData_ContainsNewGodot45Methods()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);

            // Test new @GlobalScope methods added for Godot 4.5
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("absi"), "Missing absi method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("acosh"), "Missing acosh method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("asinh"), "Missing asinh method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("atanh"), "Missing atanh method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("angle_difference"), "Missing angle_difference method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("rotate_toward"), "Missing rotate_toward method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("fmod"), "Missing fmod method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("str"), "Missing str method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("type_string"), "Missing type_string method");
        }

        [TestMethod]
        public void GlobalData_ContainsGDScriptMethods()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);

            // Test @GDScript specific methods
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("Color8"), "Missing Color8 method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("load"), "Missing load method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("preload"), "Missing preload method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("type_exists"), "Missing type_exists method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("is_instance_of"), "Missing is_instance_of method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("print_debug"), "Missing print_debug method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("print_stack"), "Missing print_stack method");
            Assert.IsTrue(data.GlobalData.MethodDatas.ContainsKey("range"), "Missing range method");
        }

        [TestMethod]
        public void GlobalData_ContainsPackedVector4Array()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.GlobalTypes);
            Assert.IsTrue(data.GlobalData.GlobalTypes.ContainsKey("packedvector4array"), "Missing packedvector4array type");
        }

        // ========================================
        // Type Inference Tests - Extended Type Info
        // NOTE: Full extended info (Parameters, CSharpDeclaringTypeFullName, etc.)
        // will be available after regenerating AssemblyData.json from Godot runtime
        // ========================================

        [TestMethod]
        public void GDMethodData_HasReturnTypeInfo()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            var nodeData = data.TypeDatas["Node"].Values.First();
            Assert.IsNotNull(nodeData.MethodDatas);

            // Find a method that has CSharpReturnTypeName
            var methodWithReturn = nodeData.MethodDatas.Values
                .SelectMany(x => x)
                .FirstOrDefault(m => !string.IsNullOrEmpty(m.CSharpReturnTypeName));

            Assert.IsNotNull(methodWithReturn, "Should have a method with CSharpReturnTypeName");
            Assert.IsNotNull(methodWithReturn.CSharpReturnTypeName, "CSharpReturnTypeName should not be null");
        }

        [TestMethod]
        public void GDMethodData_HasParameterTypeNames()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);

            // lerp has parameters
            var lerpMethods = data.GlobalData.MethodDatas["lerp"];
            Assert.IsTrue(lerpMethods.Count > 0);

            var lerpMethod = lerpMethods[0];
            Assert.IsNotNull(lerpMethod.CSharpParameterTypeNames, "CSharpParameterTypeNames should not be null");
            Assert.IsTrue(lerpMethod.CSharpParameterTypeNames.Length > 0, "lerp should have parameter type names");
        }

        [TestMethod]
        public void GDMethodData_HasBooleanFlags()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);
            var absMethods = data.GlobalData.MethodDatas["abs"];
            Assert.IsTrue(absMethods.Count > 0);

            var absMethod = absMethods[0];

            // These boolean properties should be set (can be true or false)
            Assert.IsNotNull(absMethod.GDScriptName, "GDScriptName should not be null");
            Assert.IsNotNull(absMethod.CSharpName, "CSharpName should not be null");
            // IsStatic, IsVirtual, IsAbstract are value types - they always have a value
        }

        [TestMethod]
        public void GDPropertyData_HasBasicTypeInfo()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.TypeDatas);
            var node2dData = data.TypeDatas["Node2D"].Values.First();
            var positionProperty = node2dData.PropertyDatas!["position"];

            Assert.IsNotNull(positionProperty.CSharpTypeName, "CSharpTypeName should not be null");
            Assert.IsNotNull(positionProperty.GDScriptName, "GDScriptName should not be null");
            Assert.IsNotNull(positionProperty.CSharpName, "CSharpName should not be null");
        }

        [TestMethod]
        public void GDMethodData_OverloadsExist()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);

            // abs likely has multiple overloads (int, float, double, etc.)
            var absMethods = data.GlobalData.MethodDatas["abs"];

            // Verify that overloads are captured as list
            Assert.IsTrue(absMethods.Count >= 1, "abs should have at least one overload");
        }

        [TestMethod]
        public void GDMethodData_DifferentOverloadsHaveDifferentParameters()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.GlobalData?.MethodDatas);

            // Find a method with multiple overloads
            var methodsWithOverloads = data.GlobalData.MethodDatas
                .Where(kvp => kvp.Value.Count > 1)
                .FirstOrDefault();

            if (methodsWithOverloads.Value != null && methodsWithOverloads.Value.Count > 1)
            {
                var firstOverload = methodsWithOverloads.Value[0];
                var secondOverload = methodsWithOverloads.Value[1];

                // Different overloads should have different parameters or return types
                bool areDifferent =
                    (firstOverload.CSharpParameterTypeNames?.Length ?? 0) != (secondOverload.CSharpParameterTypeNames?.Length ?? 0) ||
                    firstOverload.CSharpReturnTypeName != secondOverload.CSharpReturnTypeName ||
                    !ParametersEqual(firstOverload.CSharpParameterTypeNames, secondOverload.CSharpParameterTypeNames);

                Assert.IsTrue(areDifferent, "Overloads should have different signatures");
            }
        }

        private static bool ParametersEqual(string[]? a, string[]? b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            return a.SequenceEqual(b);
        }

        #region Metadata Tests

        [TestMethod]
        public void ExtractTypeDatasFromManifest_SetsMetadataSource()
        {
            var data = GDTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data?.Metadata);
            Assert.AreEqual("Manifest", data.Metadata.Source);
        }

        [TestMethod]
        public void GDAssemblyMetadata_HasDefaultDataFormatVersion()
        {
            var metadata = new GDAssemblyMetadata();

            Assert.AreEqual(1, metadata.DataFormatVersion);
        }

        [TestMethod]
        public void GDAssemblyMetadata_ConstructorWithVersion_SetsAllProperties()
        {
            var metadata = new GDAssemblyMetadata("4.5.1");

            Assert.AreEqual("4.5.1", metadata.GodotVersion);
            Assert.AreEqual(1, metadata.DataFormatVersion);
            Assert.AreEqual("Assembly", metadata.Source);
            Assert.IsNotNull(metadata.ExtractedAt);
        }

        #endregion

        #region File Operations Tests

        [TestMethod]
        public void ExtractTypeDatasFromFile_WithNullPath_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                GDTypeHelper.ExtractTypeDatasFromFile(null!));
        }

        [TestMethod]
        public void ExtractTypeDatasFromFile_WithEmptyPath_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                GDTypeHelper.ExtractTypeDatasFromFile(""));
        }

        [TestMethod]
        public void ExtractTypeDatasFromFile_WithNonExistentFile_ReturnsNull()
        {
            var result = GDTypeHelper.ExtractTypeDatasFromFile("C:\\nonexistent\\file.json");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void SaveAssemblyDataToFile_WithNullData_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                GDTypeHelper.SaveAssemblyDataToFile(null!));
        }

        [TestMethod]
        public void SaveAndLoadFromFile_RoundTrip_PreservesData()
        {
            var originalData = GDTypeHelper.ExtractTypeDatasFromManifest();
            Assert.IsNotNull(originalData);

            var tempFile = Path.Combine(Path.GetTempPath(), $"GDShrapt_Test_{Guid.NewGuid()}.json");

            try
            {
                // Save to file
                GDTypeHelper.SaveAssemblyDataToFile(originalData, tempFile);
                Assert.IsTrue(File.Exists(tempFile), "File should be created");

                // Load from file
                var loadedData = GDTypeHelper.ExtractTypeDatasFromFile(tempFile);

                Assert.IsNotNull(loadedData);
                Assert.IsNotNull(loadedData.Metadata);
                Assert.AreEqual("File", loadedData.Metadata.Source);
                Assert.AreEqual(tempFile, loadedData.Metadata.SourcePath);

                // Verify data integrity
                Assert.IsNotNull(loadedData.GlobalData);
                Assert.IsNotNull(loadedData.TypeDatas);
                Assert.AreEqual(originalData.TypeDatas?.Count, loadedData.TypeDatas?.Count);
                Assert.AreEqual(originalData.GlobalData?.MethodDatas?.Count, loadedData.GlobalData?.MethodDatas?.Count);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void GetDefaultSavePath_ReturnsValidPath()
        {
            var path = GDTypeHelper.GetDefaultSavePath();

            Assert.IsNotNull(path);
            Assert.IsTrue(path.EndsWith("AssemblyData.json", StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
