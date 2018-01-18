using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
 

namespace P7.Core.Reflection
{
    public class TypeHelper<T> 
    {

        public static bool IsPublicClassType(Type type)
        {
            return IsType(type) && type.IsPublicClass();
        }

        public static bool IsType(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public static bool IsSubclassOf(Type type)
        {
            return (type != null && type.GetTypeInfo().IsClass && type.GetTypeInfo().IsSubclassOf(typeof(T)));
        }

        public static IEnumerable<Type> FindDerivedTypes(Assembly assembly)
        {
            var baseType = typeof(T);
            return assembly.GetTypes().Where(baseType.IsAssignableFrom);
        }
 
        public static IEnumerable<Type> FindTypesInAssembly(Assembly assembly, bool includeSubClass = false)
        {
            // Go through all assemblies referenced by the application and search for types matching a predicate
            IEnumerable<Type> typesSoFar = Type.EmptyTypes;
            Predicate<Type> predicate = TypeHelper<T>.IsType;
            Type[] typesInAsm;
            try
            {
                typesInAsm = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                typesInAsm = ex.Types;
            }
            typesSoFar = typesSoFar.Concat(typesInAsm);

            return
                typesSoFar.Where(
                    type =>
                        type.IsPublicClass() &&
                        (predicate(type) || (!includeSubClass || IsSubclassOf(type))));
        }
        public static IEnumerable<Assembly> GetReferencingAssemblies(Assembly entryAssembly)
        {

            var assemblies = entryAssembly.GetReferencedAssemblies();

            foreach (var assemblyName in assemblies)
            {
                yield return Assembly.Load(assemblyName);
            }

        }
        public static IEnumerable<Type> FindTypesByAttributeByInteface<TCustomAttribute, TInterface>(Assembly entryAssembly)
            where TCustomAttribute : Attribute

        { 
            var refAss = GetReferencingAssemblies(entryAssembly);
            var calcs = from a in refAss
                from t in a.GetTypes()
                where t.GetTypeInfo().GetCustomAttribute<TCustomAttribute>() != null
                      && t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(TInterface))
                select t;
            return calcs;
        }
        public static IEnumerable<Type> FindTypesByInteface<TInterface>(Assembly entryAssembly)
        {
            var refAss = GetReferencingAssemblies(entryAssembly);
            var calcs = from a in refAss
                from t in a.GetTypes()
                where t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(TInterface))
                select t;
            return calcs;
        }

        public static IEnumerable<Type> FindTypesInAssembly(Assembly assembly, Predicate<Type> predicate)
        {
            // Go through all assemblies referenced by the application and search for types matching a predicate
            IEnumerable<Type> typesSoFar = Type.EmptyTypes;

            Type[] typesInAsm;
            try
            {
                typesInAsm = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                typesInAsm = ex.Types;
            }
            typesSoFar = typesSoFar.Concat(typesInAsm);

            return typesSoFar.Where(type => predicate(type));
        }


        public static IEnumerable<Type> FindTypesInAssembly2(Assembly assembly)
        {
            Predicate<Type> predicate = TypeHelper<T>.IsType;
            return FindTypesInAssembly2(assembly, predicate);
        }

        public static IEnumerable<Type> FindTypesInAssembly2(Assembly assembly,
            Predicate<Type> predicate)
        {
            // Go through all assemblies referenced by the application and search for types matching a predicate
            IEnumerable<Type> typesSoFar = Type.EmptyTypes;


            Type[] typesInAsm;
            try
            {
                typesInAsm = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                typesInAsm = ex.Types;
            }
            typesSoFar = typesSoFar.Concat(typesInAsm);
            return typesSoFar.Where(type => type.IsPublicClass() && predicate(type));
        }

        public static IEnumerable<Type> FindTypesInAssemblies(IEnumerable<Assembly> assemblies,
            Predicate<Type> predicate)
        {
            // Go through all assemblies referenced by the application and search for types matching a predicate
            IEnumerable<Type> typesSoFar = Type.EmptyTypes;

            foreach (Assembly assembly in assemblies)
            {
                typesSoFar = typesSoFar.Concat(FindTypesInAssembly2(assembly, predicate));
            }
            return typesSoFar;
        }
 

       
        public static IEnumerable<Type> FindTypesWithCustomAttribute<TAttributeType>(IEnumerable<Type> toBeEvaluatedTypes)
        {
            var types = new List<Type>();
            foreach (Type type in toBeEvaluatedTypes)
            {
                if (type.GetTypeInfo().GetCustomAttributes(typeof(TAttributeType), true).Any())
                {
                    types.Add(type);
                }
            }
            return types;
        }
    }
}