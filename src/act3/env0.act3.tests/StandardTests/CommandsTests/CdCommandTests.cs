using System.Collections.Generic;
using Env0.Act3.Config.Pocos;
using Env0.Act3.Filesystem;
using Env0.Act3.Terminal;
using Env0.Act3.Terminal.Commands;

namespace Env0.Act3.Tests.StandardTests.CommandsTests
{
    public class CdCommandTests
    {
        /// <summary>
        /// CdCommand should succeed when changing to root.
        /// </summary>
        [Fact]
        public void CdCommand_ChangeToRoot_Succeeds()
        {
            
            var root = new FileEntry
            {
                Name = "/",
                Type = "dir", // or just "" if you prefer
                Children = new Dictionary<string, FileEntry>()
            };
  
            var session = new SessionState { FilesystemManager = new FilesystemManager(root) };
            var cmd = new CdCommand();

            var result = cmd.Execute(session, new string[0]);
            Assert.NotNull(result);
            Assert.False(result.IsError);
        }

        /// <summary>
        /// CdCommand should return error on invalid path.
        /// </summary>
        [Fact]
        public void CdCommand_InvalidPath_ReturnsError()
        {
            
            var root = new FileEntry
            {
                Name = "/",
                Type = "dir", // or just "" if you prefer
                Children = new Dictionary<string, FileEntry>()
            };

            var session = new SessionState { FilesystemManager = new FilesystemManager(root) };
            var cmd = new CdCommand();

            var result = cmd.Execute(session, new[] { "/nope" });
            Assert.True(result.IsError);
            Assert.Contains("No such directory", result.Output);
        }
    }
}
