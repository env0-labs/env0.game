using Env0.Core;

namespace Env0.Terminal
{
    internal static class TerminalAutomationBridge
    {
        public static SessionState CoreSession { get; private set; }

        public static void Bind(SessionState session)
        {
            CoreSession = session;
        }
    }
}
