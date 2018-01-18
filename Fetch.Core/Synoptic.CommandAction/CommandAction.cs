using System;
using System.Collections.Generic;
using System.Reflection;

namespace Synoptic
{
    public class CommandAction
    {
        private readonly List<ParameterInfoWrapper> _parameters = new List<ParameterInfoWrapper>();

        public CommandAction(string name, string description,string route,string method, MethodInfo linkedToMethod)
        {
            Name = name.ToHyphened();
            Description = description ?? String.Empty;
            Route = route ?? String.Empty;
            Route = Route.ToLower();
            Method = method ?? "GET";
            Method = Method.ToUpper();
            LinkedToMethod = linkedToMethod;

            foreach (var parameter in linkedToMethod.GetParameters())
            {
                _parameters.Add(new ParameterInfoWrapper(parameter));
            }
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Route { get; private set; }
        public string Method { get; private set; } // GET,PUT,POST,DELETE,HEAD
        public MethodInfo LinkedToMethod { get; private set; }
        public List<ParameterInfoWrapper> Parameters { get { return _parameters; } }
    }
}