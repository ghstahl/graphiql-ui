using System;

namespace Synoptic
{
    public class Command
    {
        public Command(string name, string description, string routeBase,Type linkedToType)
        {
            Name = name.ToHyphened();
            Description = description ?? String.Empty;
            RouteBase = routeBase ?? String.Empty;
            LinkedToType = linkedToType;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string RouteBase { get; private set; }
        public Type LinkedToType { get; private set; }
    }
}