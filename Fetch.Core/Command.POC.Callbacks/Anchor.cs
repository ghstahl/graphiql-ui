using System.Reflection;

namespace CommandPOCCallbacks
{
    public static class Anchor
    {
        public static string FullName => Assembly.GetAssembly(typeof(Anchor)).FullName;
    }
}