using Env0.Terminal;
using Env0.Terminal.Filesystem;

namespace Env0.Terminal.Terminal.Commands
{
    public class RunCommand : ICommand
    {
        private const string ScriptName = "enable_automation.sh";

        public CommandResult Execute(SessionState session, string[] args)
        {
            var result = new CommandResult();

            if (session?.FilesystemManager == null)
            {
                result.AddLine("bash: run: Filesystem not initialized.\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                result.AddLine("bash: run: No script specified.\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            var target = args[0].Trim();
            if (!session.FilesystemManager.TryGetEntry(target, out var entry, out var error))
            {
                result.AddLine($"bash: run: {error}\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            if (entry == null || entry.IsDirectory)
            {
                result.AddLine("bash: run: Not a file.\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            if (!string.Equals(entry.Name, ScriptName, System.StringComparison.OrdinalIgnoreCase))
            {
                result.AddLine("bash: run: Permission denied.\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            var coreSession = TerminalAutomationBridge.CoreSession;
            if (coreSession == null)
            {
                result.AddLine("automation: core session unavailable.\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            if (coreSession.AutomationEnabled)
            {
                result.AddLine("automation: already enabled.\n", OutputType.Standard);
                result.AddLine(string.Empty, OutputType.Standard);
                return result;
            }

            coreSession.AutomationEnabled = true;
            coreSession.AutomationStartTick = coreSession.InputTicks;
            coreSession.AutomationCompleted = 0;

            result.AddLine("automation: enabled.\n", OutputType.Standard);
            result.AddLine("automation: processing will continue during manual work.\n", OutputType.Standard);
            result.AddLine(string.Empty, OutputType.Standard);
            return result;
        }
    }
}
