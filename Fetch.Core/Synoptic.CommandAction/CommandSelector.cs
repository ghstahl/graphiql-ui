using System.Collections.Generic;
using Synoptic.Exceptions;

namespace Synoptic
{
    internal class CommandSelector
    {
        public CommandRecord Select(string commandName, List<CommandRecord> commandActionRecords)
        {
            var command = new MatchSelector<CommandRecord>().Match(commandName, commandActionRecords, c => c.Command.Name);
            if (command != null)
                return command;

            var exception = new CommandNotFoundException(commandName);
            exception.AvailableCommands.AddRange(commandActionRecords);

            var possibleCommands = new MatchSelector<CommandRecord>().PartialMatch(commandName, commandActionRecords, c => c.Command.Name);
            exception.PossibleCommands.AddRange(possibleCommands);

            throw exception;
        }
    }
}