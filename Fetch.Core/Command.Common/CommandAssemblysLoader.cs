using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Command.Contracts;

namespace Command.Common
{
    public static class CommandAssemblysLoader
    {
        public static Assembly EntryAssembly { get; set; }
        private static List<Type> _commandAssemblyTypes;

        public static List<Type> CommandAssemblyTypes
        {
            get
            {
                if (_commandAssemblyTypes == null)
                {
                    LoadCommandAssemblyTypesByAttributeByInteface<CommandAssemblyAttribute, ICommandAssembly>(EntryAssembly);
                }

                return _commandAssemblyTypes.ToList();
            }
        }

        private static void LoadCommandAssemblyTypesByAttributeByInteface<TCustomAttribute, TInterface>(Assembly entryAssembly) 
            where TCustomAttribute: Attribute
            
        {
            if (_commandAssemblyTypes != null)
            {
                return;
            }
            var refAss = GetReferencingAssemblies(entryAssembly);
            var calcs = from a in refAss
                        from t in a.GetTypes()
                where t.GetTypeInfo().GetCustomAttribute<TCustomAttribute>() != null
                      && t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(TInterface))
                select t;
            _commandAssemblyTypes = calcs.ToList();
//            _commandAssemblyTypes = calcs
//                .OrderBy(t => t.GetTypeInfo().GetCustomAttribute<TCustomAttribute>().Order).ToList();
        }

        private static IEnumerable<Assembly> GetReferencingAssemblies(Assembly entryAssembly)
        {

            var assemblies = entryAssembly.GetReferencedAssemblies();

            foreach (var assemblyName in assemblies)
            {
                yield return Assembly.Load(assemblyName);
            }

        }
    }
}