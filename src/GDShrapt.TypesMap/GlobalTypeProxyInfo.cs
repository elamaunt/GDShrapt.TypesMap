
namespace GDShrapt.TypesMap
{
    public class GlobalTypeProxyInfo
    {
        public Type Type { get; }
        public string? ValueEqvivalent { get; }
        public GlobalTypeProxyInfo(Type type, string? valueEqvivalent = null)
        {
            Type = type;
            ValueEqvivalent = valueEqvivalent;
        }
    }
}