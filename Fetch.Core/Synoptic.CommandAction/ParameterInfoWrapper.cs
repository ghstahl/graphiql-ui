﻿using System;
using System.Linq;
using System.Reflection;

namespace Synoptic
{
    public class ParameterInfoWrapper
    {
        public ParameterInfoWrapper(ParameterInfo parameter)
        {
            Name = parameter.Name;
            Type = parameter.ParameterType;

            var attributes = parameter.GetCustomAttributes(typeof(CommandParameterAttribute), true);
            if (attributes.Length > 0)
            {
                var commandParameter = (CommandParameterAttribute)attributes.First();
                Prototype = commandParameter.Prototype;

                Description = Description.GetNewIfValid(commandParameter.Description);
                DefaultValue = commandParameter.DefaultValue;
                IsRequired = commandParameter.IsRequired;
                FromBody = commandParameter.FromBody;
            }

            IsValueRequiredWhenOptionIsPresent = parameter.ParameterType != typeof(bool);
        }

        public bool IsRequired { get; set; }
        public bool FromBody { get; set; }
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public string Description { get; private set; }
        public string Prototype { get; private set; }
        public bool IsValueRequiredWhenOptionIsPresent { get; private set; }
        public object DefaultValue { get; set; }

        public string GetOptionPrototype()
        {
            return this.ToOptionPrototype();
        }

        public string GetOptionPrototypeHelp()
        {
            var valueHelp = (IsValueRequiredWhenOptionIsPresent ? "<VALUE" + (DefaultValue != null ? "|" + DefaultValue : String.Empty) + ">" : String.Empty);
            return GetOptionPrototype() + valueHelp;
        }
    }
}