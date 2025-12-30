using System;
using Env0.Act3.Terminal;

namespace Env0.Act3.Terminal.Commands
{
    public class SudoCommand : ICommand
    {
        public CommandResult Execute(SessionState session, string[] args)
        {
            // Deterministic response keeps tests stable and matches contract expectation.
            return new CommandResult("Nice try.\n", OutputType.Error);
        }
    }
}
