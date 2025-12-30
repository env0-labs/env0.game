namespace Env0.Act3.Terminal
{
    public interface ICommand
    {
        CommandResult Execute(SessionState session, string[] args);
    }
}
