using ToysStore.DataContracts;
using ToysStore.Entities;

namespace ToysStore.Managers
{
    public interface IBacketManager
    {
        Errors UpdateBacket(BacketData backetData);
        Errors RemoveFromBacket(int userId, int toyId);
        BacketResponce GetBucketList(int userId);
        Errors BuyAllToysFromBacket(int userId);
    }
}
