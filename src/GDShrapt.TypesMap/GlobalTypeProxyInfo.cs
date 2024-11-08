
namespace GDShrapt.TypesMap
{
    public class GlobalTypeProxyInfo
    {
        public string? TypeName { get; set; }
        public string? ValueEqvivalent { get; set; }

        public GlobalTypeProxyInfo()
        {

        }

        internal GlobalTypeProxyInfo(Type type, string? valueEqvivalent = null)
        {
            TypeName = type.Name;
            ValueEqvivalent = valueEqvivalent;
        }
    }
}