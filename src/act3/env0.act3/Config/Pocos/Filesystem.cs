using System.Collections.Generic;

namespace Env0.Act3.Config.Pocos
{
    public class Filesystem
    {
        public Dictionary<string, FileEntry> Root { get; set; } = new Dictionary<string, FileEntry>();
    }
}
