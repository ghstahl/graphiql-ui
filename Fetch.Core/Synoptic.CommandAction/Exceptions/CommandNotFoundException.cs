﻿using System;
using System.Collections.Generic;
using System.Linq;
using Synoptic.ConsoleFormat;

namespace Synoptic.Exceptions
{
    internal class CommandNotFoundException : CommandParseExceptionBase
    {
        private readonly string _commandName;
        private readonly List<CommandRecord> _possibleCommands = new List<CommandRecord>();
        private readonly List<CommandRecord> _availableCommands = new List<CommandRecord>();

        public CommandNotFoundException(string commandName, IEnumerable<CommandRecord> possibleCommands)
            : this(commandName)
        {
            _possibleCommands.AddRange(possibleCommands);
            _commandName = commandName;
        }

        public CommandNotFoundException(string commandName)
        {
            _commandName = commandName;
        }

        public string CommandName
        {
            get { return _commandName; }
        }

        public List<CommandRecord> PossibleCommands
        {
            get { return _possibleCommands; }
        }

        public List<CommandRecord> AvailableCommands
        {
            get { return _availableCommands; }
        }

        public override void Render()
        {
            ConsoleFormatter.Write(new ConsoleTable(
                                           new ConsoleCell("'{0}' is not a valid command.",
                                                           CommandName).WithPadding(0)));

            var formattedCommandList = string.Join(" or ",
                                                   (PossibleCommands.Count > 0
                                                        ? PossibleCommands
                                                        : AvailableCommands).Select(
                                                            c => String.Format("'{0}'", c.Command.Name)).ToArray());

            ConsoleFormatter.Write(new ConsoleTable()
                                       .AddEmptyRow()
                                       .AddRow(
                                           new ConsoleCell("Did you mean:").WithPadding(0))
                                       .AddRow(
                                           new ConsoleCell(formattedCommandList)));
        }
    }
}