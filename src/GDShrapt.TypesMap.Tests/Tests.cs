namespace GDShrapt.TypesMap.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ManifestLoadTest()
        {
            var data = GodotTypeHelper.ExtractTypeDatasFromManifest();

            Assert.IsNotNull(data);
        }

    }
}