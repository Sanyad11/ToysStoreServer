using System.Web;
using System.Web.SessionState;

namespace ToysStore.Managers
{
    public class SessionStateManager : ISessionStateManager
    {
        private HttpSessionState session = HttpContext.Current.Session;
        public void AddKey(string key, object value)
        {
            session[key] = value;
        }
        public object GetValue(string key)
        {
            return session[key];
        }

        public void AbandonSession()
        {
            session.Clear();
            session.Abandon();
        }
    }
}