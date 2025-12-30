using Xunit;
using Env0.Act3.Terminal;
using Env0.Act3.Terminal.Commands;

namespace Env0.Act3.Tests.Commands
{
    public class ExitCommandTests
    {
        [Fact]
        public void ExitCommand_AtRoot_ReturnsLogoutNotImplemented()
        {
            var command = new ExitCommand();
            var session = new SessionState(); // At root by default

            var result = command.Execute(session, new string[0]);

            Assert.NotNull(result);
            Assert.Contains("logout: not implemented", result.Output.ToLower());
        }
    }
}
