using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Command.Contracts;

namespace Command.Common
{
    public class CommandCompositionHelper
    {
        public CommandCompositionHelper(string componentFolder)
        {
            ComponentFolder = componentFolder;
        }

        public string ComponentFolder { get; set; }

        public List<ICommandAssembly> CommandAssemblyPlugins { get; set; }

        public IEnumerable<ICommandAssembly> FetchCommandComponents()
        {
            string directoryPath = ComponentFolder;

            var assemblies = Directory
                .GetFiles(directoryPath, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(AssemblyLoadContext.GetAssemblyName)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyName)
                .ToList();

            foreach (var assembly in assemblies)
            {
                foreach (var ti in assembly.DefinedTypes)
                {
                    if (ti.ImplementedInterfaces.Contains(typeof(ICommandAssembly)))
                    {
                        yield return (ICommandAssembly) assembly.CreateInstance(ti.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Assembles the Command components
        /// </summary>
        public void AssembleCommandComponents()
        {

            try
            {
                var ca = new CommandAssemblyProvider().GetCommandAssemblies();
                var query = from i in ca
                    select i.Value;
                CommandAssemblyPlugins = query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Initialize()
        {
            foreach (var plugin in CommandAssemblyPlugins)
            {
                plugin.Initialize();
            }
        }

    }
}
