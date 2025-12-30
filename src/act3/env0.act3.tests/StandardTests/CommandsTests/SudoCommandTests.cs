using Xunit;
using Env0.Act3.Terminal;
using Env0.Act3.Terminal.Commands;

namespace Env0.Act3.Tests.Commands
{
    public class SudoCommandTests
    {
        /// <summary>
        /// SudoCommand always returns Easter egg "Nice try."
        /// </summary>
        [Fact]
        public void SudoCommand_AlwaysReturnsEasterEgg()
        {
            var cmd = new SudoCommand();
            var session = new SessionState();
            var result = cmd.Execute(session, new string[0]);

            Assert.Contains("Nice try", result.Output);
        }
    }
}
