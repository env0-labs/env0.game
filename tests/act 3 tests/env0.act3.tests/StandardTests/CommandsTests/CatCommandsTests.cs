using System.Collections.Generic;
using Env0.Act3.Config.Pocos;
using Env0.Act3.Filesystem;
using Env0.Act3.Terminal;
using Env0.Act3.Terminal.Commands;

namespace Env0.Act3.Tests.StandardTests.CommandsTests
{
    public class CatCommandTests
    {
        /// <summary>
        /// CatCommand should return an error for missing or directory target.
        /// </summary>
        [Fact]
        public void CatCommand_MissingOrDir_ReturnsError()
        {
            var root = new FileEntry() { Name = "/", Type = "dir", Children = new Dictionary<string, FileEntry>() };
            var session = new SessionState { FilesystemManager = new FilesystemManager(root) };
            var cmd = new CatCommand();

            var result = cmd.Execute(session, new string[0]);
            Assert.True(result.IsError);

            result = cmd.Execute(session, new[] { "/" });
            Assert.True(result.IsError);
        }

        /// <summary>
        /// CatCommand returns file contents when present.
        /// </summary>
        [Fact]
        public void CatCommand_File_ReturnsContents()
        {
            var file = new FileEntry
            {
                Name = "foo.txt",
                Type = "file",
                Content = "hello"
            };
            var root = new FileEntry
            {
                Name = "/",
                Type = "dir", // or just "" if you prefer
                Children = new Dictionary<string, FileEntry>()
            };
            root.Children.Add("foo.txt", file);
            file.Parent = root;

            var session = new SessionState { FilesystemManager = new FilesystemManager(root) };
            var cmd = new CatCommand();

            var result = cmd.Execute(session, new[] { "foo.txt" });
            Assert.False(result.IsError);
            Assert.Equal("hello", result.Output);
        }
    }
}
