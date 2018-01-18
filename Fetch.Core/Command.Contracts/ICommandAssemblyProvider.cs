using System;
using System.Collections.Generic;

namespace Command.Contracts
{
    public interface ICommandAssemblyProvider
    {
        IDictionary<string, ICommandAssembly> GetCommandAssemblies();
        IEnumerable<string> GetNames();
        IEnumerable<Type> GetTypes();
    }
}