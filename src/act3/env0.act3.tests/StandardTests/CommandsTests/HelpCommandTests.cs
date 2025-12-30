using Xunit;
using Env0.Act3.Terminal;
using Env0.Act3.Terminal.Commands;

namespace Env0.Act3.Tests.Commands
{
    public class HelpCommandTests
    {
        [Fact]
        public void HelpCommand_ReturnsComprehensiveHelp()
        {
            var command = new HelpCommand();
            var session = new SessionState();

            var result = command.Execute(session, new string[0]);

            Assert.NotNull(result);
            Assert.Contains("available commands", result.Output.ToLower());
        }
    }
}
