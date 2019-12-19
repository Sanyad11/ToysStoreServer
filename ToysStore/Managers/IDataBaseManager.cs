using System.Collections.Generic;
using ToysStore.DataContracts;
using ToysStore.Entities;

namespace ToysStore.Managers
{
    public interface IDataBaseManager
    {
        bool isExistUser(UserData user);
        bool IsExistUser(string login, string password);
        void AddUser(UserData user);
        bool IsExistToy(int toyId);
        ToysData GetToyById(int toyId);
        List<AllToysRequest> GetAllToys();
        void UpdateBacket(BacketData backetData);
        List<AllToysRequest> GetBucketListBacketData(int userId);
        void BuyAllToysFromBacket(int userId);
        int GetBucketListPrice(int userId);
        void RemoveFromBacket(int userId, int toyId);
    }
}
