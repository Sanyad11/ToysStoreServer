namespace ToysStore.Managers
{
    public interface ISessionStateManager
    {
        void AddKey(string key, object value);
        object GetValue(string key);

        void AbandonSession();
    }
}