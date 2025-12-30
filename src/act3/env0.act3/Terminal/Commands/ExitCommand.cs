using Env0.Act3.Terminal;
using Env0.Act3.Filesystem;
using Env0.Act3.Network;

namespace Env0.Act3.Terminal.Commands
{
    public class ExitCommand : ICommand
    {
        public CommandResult Execute(SessionState session, string[] args)
        {
            var result = new CommandResult();

            if (session.SshStack != null && session.SshStack.Count > 0)
            {
                // Pop previous SSH context
                var prev = session.SshStack.Pop();

                session.Username = prev.Username;
                session.Hostname = prev.Hostname;
                session.CurrentWorkingDirectory = prev.CurrentWorkingDirectory;
                session.FilesystemManager = prev.FilesystemManager;
                session.NetworkManager = prev.NetworkManager;

                result.AddLine($"Connection to {session.Hostname} closed.\n", OutputType.Standard);
                result.StateChanged = true;
                result.UpdatedSession = session;
            }
            else
            {
                result.AddLine("logout: Not implemented.\n", OutputType.Error);
            }

            return result;
        }
    }
}
