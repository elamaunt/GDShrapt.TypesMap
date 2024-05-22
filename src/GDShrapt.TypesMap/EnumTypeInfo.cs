namespace GDShrapt.TypesMap
{
    public class EnumTypeInfo
    {
        public string? ContaingTypeName { get; set; }
        public string? DotnetEnumFullName { get; set; }
        public string? DotnetEnumName { get; set; }

        public Dictionary<string, string>? Values { get; set; }

        public EnumTypeInfo()
        {
        }

        public EnumTypeInfo(string containgTypeName, Type dotnetEnum, string[] constants, Array dotnetValues)
        {
            ContaingTypeName = containgTypeName;
            DotnetEnumFullName = dotnetEnum.FullName;
            DotnetEnumName = dotnetEnum.Name;

            Values = new Dictionary<string, string>();

            for (int i = 0; i < constants.Length; i++)
            {
                Values[constants[i]] = dotnetValues.GetValue(i)!.ToString()!;
            }
        }
    }
}