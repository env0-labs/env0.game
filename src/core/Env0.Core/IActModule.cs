using System.Collections.Generic;

namespace Env0.Core;

public interface IActModule
{
    IEnumerable<OutputLine> Handle(string input, SessionState state);
}
