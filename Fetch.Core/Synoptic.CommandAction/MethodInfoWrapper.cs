﻿using System.Linq;
using System.Reflection;

namespace Synoptic
{
    internal class MethodInfoWrapper
    {
        public MethodInfoWrapper(MethodInfo method)
        {
            LinkedToMethod = method;

            Name = method.Name;

            var attributes = method.GetCustomAttributes(typeof(CommandActionAttribute), true);
            if (attributes.Length <= 0) return;
            
            var commandParameter = (CommandActionAttribute)attributes.First();

            Description = Description.GetNewIfValid(commandParameter.Description);
            Name = Name.GetNewIfValid(commandParameter.Name);
            Route = Route.GetNewIfValid(commandParameter.Route);
            Method = Method.GetNewIfValid(commandParameter.Method);
        }

        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Route { get; set; }
        public string Method { get; set; }
        public MethodInfo LinkedToMethod { get; set; }
    }
}