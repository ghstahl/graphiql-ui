using System;
using Command.Contracts;
using Programs.Repository;

namespace ProgramsCommand
{
    [CommandAssembly(AssemblyType=typeof(CommandAssembly))]
    public class CommandAssembly : ICommandAssembly
    {
        public void Initialize()
        {
            var programsRepository = new ProgramsRepository();
            ProgramsCommand.Programs.ProgramsRepository = programsRepository;
            ProgramsCommand.Processes.ProgramsRepository = programsRepository;
        }
    }
}
