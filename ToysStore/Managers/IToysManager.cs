using System.Collections.Generic;
using ToysStore.DataContracts;
using ToysStore.Entities;

namespace ToysStore.Managers
{
    public interface IToysManager
    {
        ToysData GetToyById(int toyId, out Errors errorCode);
        List<AllToysRequest> GetAllToys();

    }
}
