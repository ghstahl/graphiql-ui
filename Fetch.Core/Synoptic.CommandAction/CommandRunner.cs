using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mono.Options;
using Synoptic.Exceptions;

namespace Synoptic
{
    public class RouteQuery
    {
        public string Route { get; set; }
        public string Method { get; set; }
        public dynamic Body { get; set; }
    }
    public class CommandActionRecord
    {
        public CommandAction CommandAction { get; set; }
        public string Route { get; set; }
    }
    public class CommandRecord
    {
        public Command Command { get; set; }
        public List<CommandAction> CommandActions { get; set; }
    }
    public class CommandRunner
    {
        private ICommandDependencyResolver _resolver = new ActivatorCommandDependencyResolver();
        private static List<CommandRecord> _commandRecords;
        private static List<CommandActionRecord> _commandActionRecords;

        public CommandRunner()
        {
            
        }
        private List<CommandRecord> CommandRecords
        {
            get
            {
                if (_commandRecords == null)
                {
                    if (_availableCommands.Count == 0)
                        WithCommandsFromAssembly(AppDomain.CurrentDomain.GetAssemblies());
                    _commandRecords = new List<CommandRecord>();
                    foreach (var item in _availableCommands)
                    {
                        _commandRecords.Add(new CommandRecord(){Command = item});
                    }
                }
                return _commandRecords;
            }
        }

        private List<CommandActionRecord> CommandActionRecords
        {
            get
            {
                if (_commandActionRecords == null)
                {
                    EnsureCommandActions();
                    _commandActionRecords = new List<CommandActionRecord>();
                    foreach (var item in CommandRecords)
                    {
                        var routeBase = item.Command.RouteBase;
                        routeBase.Trim(" /".ToCharArray());
                        if (!string.IsNullOrEmpty(routeBase))
                        {
                            foreach (var action in item.CommandActions)
                            {
                                var route = action.Route;
                                route.Trim(" /".ToCharArray());
                                if (!string.IsNullOrEmpty(route))
                                {
                                    var commandActionRecord = new CommandActionRecord()
                                    {
                                        CommandAction = action,
                                        Route = $"{routeBase}/{route}".ToLower()
                                    };
                                    _commandActionRecords.Add(commandActionRecord);
                                }
                            }
                        }
                    }
                }
                return _commandActionRecords;
            }
        }

        private static readonly List<Command> _availableCommands = new List<Command>();
        private readonly CommandFinder _commandFinder = new CommandFinder();
        private readonly ICommandActionFinder _commandActionFinder = new CommandActionFinder();
        private readonly HelpGenerator _helpGenerator = new HelpGenerator();
        private OptionSet _optionSet;

        public async Task<RunResult> RunViaRouteAsync(RouteQuery routeQuery)
        {

            try
            {
                if (CommandRecords.Count == 0)
                    throw new NoCommandsDefinedException();

                string route = routeQuery.Route;
                string method = routeQuery.Method;
                method = method.ToUpper();
                route = route.ToLower();

                var query = from item in CommandActionRecords
                    where item.Route == route && item.CommandAction.Method == method
                    select item;
                var actionRecord = query.FirstOrDefault();

                if (actionRecord == null)
                {
                    return new RunResult() {ErrorCode = 404};
                }

                var action = actionRecord.CommandAction;

                var parser = new CommandLineParser();
                var clp = new List<CommandLineParameter> {new CommandLineParameter("body", routeQuery.Body) };

                CommandLineParseResult parseResult = new CommandLineParseResult(action, clp, new List<string>().ToArray());
              
                return await parseResult.CommandAction.RunAsync(_resolver, parseResult);
            }
            catch (CommandParseExceptionBase exception)
            {
                exception.Render();
            }
            catch (TargetInvocationException exception)
            {
                Exception innerException = exception.InnerException;
                if (innerException == null) throw;

                if (innerException is CommandParseExceptionBase)
                {
                    ((CommandParseExceptionBase)exception.InnerException).Render();
                    return new RunResult();
                }

                throw new CommandInvocationException("Error executing command", innerException);
            }
            return new RunResult() { ErrorCode = 404 };
        }

        public RunResult Run(string[] args)
        {
            if (args == null)
                args = new string[0];

            Queue<string> arguments = new Queue<string>(args);

            if (_optionSet != null)
                arguments = new Queue<string>(_optionSet.Parse(args));

            try
            {
                if (CommandRecords.Count == 0)
                    throw new NoCommandsDefinedException();

                if (arguments.Count == 0)
                {
                    _helpGenerator.ShowCommandUsage(_availableCommands, _optionSet);
                    return new RunResult();
                }

                var commandName = arguments.Dequeue();

                var commandSelector = new CommandSelector();
                var command = commandSelector.Select(commandName, CommandRecords);
                EnsureCommandActions(command);
                var actionName = arguments.Count > 0 ? arguments.Dequeue() : null;

                var availableActions = command.CommandActions;

                if (actionName == null)
                {
                    _helpGenerator.ShowCommandHelp(command.Command, availableActions.ToList());
                    return new RunResult();
                }

                var actionSelector = new ActionSelector();
                var action = actionSelector.Select(actionName, command.Command, availableActions);

                var parser = new CommandLineParser();

                CommandLineParseResult parseResult = parser.Parse(action, arguments.ToArray());
                return parseResult.CommandAction.Run(_resolver, parseResult);
            }
            catch (CommandParseExceptionBase exception)
            {
                exception.Render();
            }
            catch (TargetInvocationException exception)
            {
                Exception innerException = exception.InnerException;
                if (innerException == null) throw;

                if (innerException is CommandParseExceptionBase)
                {
                    ((CommandParseExceptionBase)exception.InnerException).Render();
                    return new RunResult();
                }

                throw new CommandInvocationException("Error executing command", innerException);
            }
            return new RunResult();
        }

        private void EnsureCommandActions(CommandRecord command)
        {
            if (command.CommandActions == null)
            {
                command.CommandActions = _commandActionFinder.FindInCommand(command.Command).ToList();
            }
        }
        private void EnsureCommandActions()
        {
            foreach (var item in CommandRecords)
            {
                EnsureCommandActions(item);
            }
        }

        public CommandRunner WithDependencyResolver(ICommandDependencyResolver resolver)
        {
            _resolver = resolver;
            return this;
        }

        public CommandRunner WithCommandFromType<T>()
        {
            _availableCommands.Add(_commandFinder.FindInType(typeof(T)));
            return this;
        }

        public CommandRunner WithCommandsFromAssembly(Assembly[] assemblies)
        {
            _availableCommands.AddRange(_commandFinder.FindInAssembly(assemblies));
            return this;
        }

        public CommandRunner WithGlobalOptions(OptionSet optionSet)
        {
            _optionSet = optionSet;
            return this;
        }
    }
}