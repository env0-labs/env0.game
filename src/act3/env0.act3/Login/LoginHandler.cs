using Env0.Act3.Terminal;

namespace Env0.Act3.Login
{
    /// <summary>
    /// Handles player login flow (for now: just stores username and password, no validation).
    /// </summary>
    public class LoginHandler
    {
        public void SetUsername(SessionState session, string username)
        {
            session.Username = username;
        }

        public void SetPassword(SessionState session, string password)
        {
            session.Password = password;
        }
    }
}
