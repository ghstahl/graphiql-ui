using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Command.Contracts;


namespace Command.Common
{
    public class CommandAssemblyProvider : ICommandAssemblyProvider
    {
        public CommandAssemblyProvider()
        {
        }

        public IDictionary<string, ICommandAssembly> GetCommandAssemblies()
        {
            var result = new Dictionary<string, ICommandAssembly>();

            foreach (var type in CommandAssemblysLoader.CommandAssemblyTypes)
            {
                var instance = (ICommandAssembly) Activator.CreateInstance(type);
                var name = type.GetTypeInfo().GetCustomAttribute<CommandAssemblyAttribute>().Name;
                result.Add(name, instance);
            }
            return result;
        }

        public IEnumerable<string> GetNames()
        {
            return CommandAssemblysLoader.CommandAssemblyTypes
                .Select(t => IntrospectionExtensions.GetTypeInfo(t).GetCustomAttribute<CommandAssemblyAttribute>()?.Name)
                .Where(t => !string.IsNullOrWhiteSpace(t));
        }

        public IEnumerable<Type> GetTypes()
        {
            return CommandAssemblysLoader.CommandAssemblyTypes;
        }
    }
}