using System.Collections.Generic;

namespace ToysStore.DataContracts
{
    public class BacketResponce
    {
        public List<AllToysRequest> ToysData { get; set; }

        public int Price { get; set; }
    }
}