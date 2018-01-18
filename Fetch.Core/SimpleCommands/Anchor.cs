using System.Reflection;

namespace SimpleCommands
{
    public static class Anchor
    {
        public static string FullName => Assembly.GetAssembly(typeof(Anchor)).FullName;
    }
}