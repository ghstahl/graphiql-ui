using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Synoptic
{
    internal class CommandFinder
    {
        public IEnumerable<Command> FindInAssembly(Assembly[] assemblies)
        {
            List<Type> commands = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var commandTypes = ReflectionUtilities.RetrieveTypesWithAttribute<CommandAttribute>(assembly);
                commands.AddRange(commandTypes);
            }
          
            return commands.Select(FindInType);
        }

        public Command FindInType(Type type)
        {
            var typeInfo = new CommandTypeWrapper(type);
            return new Command(typeInfo.Name, typeInfo.Description,typeInfo.RouteBase, typeInfo.LinkedToType);
        }
    }
}